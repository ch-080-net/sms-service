using BAL.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Interfaces;
using Model.ViewModels.CompanyViewModels;
using Model.ViewModels.OperatorViewModels;
using Model.ViewModels.TariffViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApp.Models;
using BAL.Interfaces;
using Model.ViewModels.RecievedMessageViewModel;
using Model.ViewModels.AnswersCodeViewModels;
using System.Text.RegularExpressions;
using BAL.Services;
using Model.ViewModels.SubscribeWordViewModels;
using Model.ViewModels.StepViewModels;
using Model.ViewModels.RecipientViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ICompanyManager companyManager;
        private readonly IOperatorManager operatorManager;
        private readonly ITariffManager tariffManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGroupManager groupManager;
        private readonly IPhoneManager phoneManager;
        private readonly IRecipientManager recipientManager;
        private readonly IRecievedMessageManager recievedMessageManager;
        private readonly IAnswersCodeManager answersCodeManager;
        private readonly ISubscribeWordManager subscribeWordManager;
        private readonly IContactManager contactManager;


        public CompanyController(ICompanyManager company, IOperatorManager _operator, ITariffManager tariff,
                                 UserManager<ApplicationUser> userManager, IGroupManager groupManager,
                                 IRecipientManager recipientManager, IPhoneManager phoneManager,
                                 IRecievedMessageManager recievedMessageManager, IAnswersCodeManager answersCodeManager,
                                 ISubscribeWordManager subscribeWordManager, IContactManager contactManager)
        {
            this.companyManager = company;
            this.operatorManager = _operator;
            this.tariffManager = tariff;
            this.userManager = userManager;
            this.groupManager = groupManager;
            this.phoneManager = phoneManager;
            this.recipientManager = recipientManager;
            this.recievedMessageManager = recievedMessageManager;
            this.answersCodeManager = answersCodeManager;
            this.subscribeWordManager = subscribeWordManager;
            this.contactManager = contactManager;
        }

        /// <summary>
        /// Method for getting current UserGroupId
        /// </summary>
        /// <returns>ApplicationGroupId</returns>
        public int GetGroupId()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = userManager.Users.FirstOrDefault(u => u.Id == userId);
            var groupId = user.ApplicationGroupId;
            return groupId;
        }

       

        /// <summary>
        /// Get view with Companies which belongs to this user ApplicationGroup
        /// </summary>
        /// <returns>View with companies</returns>
        [HttpGet]
        public IActionResult Index()
        {
            var companies = companyManager.GetCompanies(GetGroupId());
            return View(companies);
        }

        [HttpGet]
        public IActionResult GetCampaignCopy(int companyId)
        {
            var item = companyManager.GetDetails(companyId);
            item.PhoneNumber = phoneManager.GetPhoneById(item.PhoneId).PhoneNumber;
            if (item.Type == CompanyType.Send || item.Type == CompanyType.SendAndRecieve)
            {
                if (item.TariffId <= 0)
                {
                    item.Tariff = "Tariff not choosen";
                }
                else
                {
                    item.Tariff = tariffManager.GetTariffById(item.TariffId).Name;
                }
            }
            return View(item);
        }
        [HttpPost]
        public IActionResult GetCampaignCopy(ManageViewModel item, int companyId)
        {
            item.ApplicationGroupId = GetGroupId();
            item.RecipientViewModels = recipientManager.GetRecipients(companyId);
            if (item.SendingTime < DateTime.Now)
                item.SendingTime = DateTime.Now;
            companyManager.CreateCampaignCopy(item);
            return RedirectToAction("Index","Company");
        }

            public IEnumerable<CompanyViewModel> Get(int page, int countOnPage, string searchValue)
        {
            if (searchValue == null)
            {
                searchValue = "";
            }
            if (!User.Identity.IsAuthenticated)
            {
                return new List<CompanyViewModel>();
            }
            return companyManager.GetCampaigns(GetGroupId(), page, countOnPage, searchValue);
        }

        public int GetCampaignsCount(string searchValue)
        {
            if (searchValue == null)
            {
                searchValue = "";
            }
            if (!User.Identity.IsAuthenticated)
            {
                return 0;
            }
            return companyManager.GetCampaignsCount(GetGroupId(), searchValue);
        }

      
        /// <summary>
        /// View for creation new Company
        /// </summary>
        /// <returns>Create Company View</returns>
        [HttpGet]
        public IActionResult Create(int id)
        {
            StepViewModel company = new StepViewModel();
            var phoneId = groupManager.Get(GetGroupId()).PhoneId;
            company.CompanyModel = new CompanyViewModel();
            company.CompanyModel.PhoneNumber = phoneManager.GetPhoneNumber(phoneId);
            company.OperatorModel = new OperatorsViewModel();
            company.OperatorModel.OperatorsList = operatorManager.GetAll();
            company.TariffModel = new TariffsViewModel();
            company.TariffModel.TariffsList = tariffManager.GetTariffs(id).ToList();
            company.RecieveModel = new RecieveViewModel();
            ViewBag.Contacts = contactManager.GetContact(GetGroupId(), 1, contactManager.GetContactCount(GetGroupId()));
            return View(company);
        }

        [HttpPost]
        public IActionResult CreateCampaign(ManageViewModel item, List<RecipientViewModel> recipient)
        {
            item.ApplicationGroupId = GetGroupId();
            if (item.SendingTime < DateTime.Now)
                item.SendingTime = DateTime.Now;
            companyManager.CreateWithRecipient(item,recipient);
            
            return Json(new {newUrl = Url.Action("Index", "Company") });
        }



        /// <summary>
        /// Return View with all company info
        /// </summary>
        /// <param name="companyId">companyId</param>
        /// <returns>View Manage company</returns>
        [HttpGet]
        public IActionResult Details(int companyId)
        {
            var item = companyManager.GetDetails(companyId);
            item.PhoneNumber = phoneManager.GetPhoneById(item.PhoneId).PhoneNumber;
            if (item.Type == CompanyType.Send || item.Type == CompanyType.SendAndRecieve)
            {
                if (item.TariffId <= 0)
                {
                    item.Tariff = "Tariff not choosen";
                }
                else
                {
                    item.Tariff = tariffManager.GetTariffById(item.TariffId).Name;
                }
            }
            return View(item);
        }

        public int GetTariffById(int id)
        {
            var limit = tariffManager.GetTariffById(id).Limit;
            return limit;
        }

        public void ChangeCampaignState(int companyId, bool newState)
        {
	        var item = companyManager.Get(companyId);
	        item.IsPaused = newState;
			companyManager.Update(item);
        }

		/// <summary>
		/// Return view with operators list
		/// </summary>
		/// <param name="companyId"></param>
		/// <returns></returns>
		[HttpGet]
        public IActionResult Operators(int companyId)
        {
            OperatorsViewModel model = new OperatorsViewModel();
            model.OperatorsList = operatorManager.GetAll();
            ViewData["companyId"] = companyId;
            return View(model);
        }

        /// <summary>
        /// Return tariffs list, related to choosing operator,
        /// for choosing tariff for current company
        /// </summary>
        /// <param name="id">Operator id</param>
        /// <param name="companyId">Current company id</param>
        /// <returns>Tariffs list, related to choosing operator</returns>
        [HttpGet]
        public List<TariffViewModel> Tariffs(int id)
        {
            List<TariffViewModel> model = tariffManager.GetTariffs(id).ToList();
            return model;
        }

        /// <summary>
        /// Change tariff for current company
        /// </summary>
        /// <param name="companyId">Current company id</param>
        /// <param name="tariffId">Selected tariff</param>
        /// <returns>Companies list</returns>
        [HttpGet]
        public IActionResult ChangeTariff(int companyId, int tariffId)
        {
            CompanyViewModel currentCompany = companyManager.Get(companyId);
            currentCompany.TariffId = tariffId;
            companyManager.Update(currentCompany);
            if (currentCompany.Type == CompanyType.Send)
            {
                return RedirectToAction("Send", "Company", new { companyId });
            }
            else
            {
                return RedirectToAction("SendRecieve", "Company", new { companyId });
            }
        }

        /// <summary>
        /// Redirect to action by company type
        /// </summary>
        /// <param name="companyId">company id</param>
        /// <returns>view depended of type</returns>
        public IActionResult RedirectByType(int companyId)
        {
            CompanyViewModel company = companyManager.Get(companyId);
            if (company.Type == CompanyType.Send)
            {
                return RedirectToAction("Send", "Company", new { companyId });
            }
            else
            {
                return RedirectToAction("SendRecieve", "Company", new { companyId });
            }
        }

        /// <summary>
        /// Delete selected item from db
        /// </summary>
        /// <param name="id">Id of Company which select to delete</param>
        /// <returns>Company Index View</returns>
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int companyId)
        {
            companyManager.Delete(companyId);
            return RedirectToAction("Index");
        }

  
        [HttpGet, ActionName("SubscribeWord")]
        public IActionResult SubscribeWord(int companyId)
        {
            var sword =subscribeWordManager.GetWordsByCompanyId(companyId);
            ViewData["CompanyId"] = companyId;
            if (!sword.Any())
            {
                return RedirectToAction("Create", "SubscribeWord",new{CompanyId=companyId});
            }
          
            return View(sword);
        }

        /// <summary>
        /// Gets EditView with Company info from db
        /// </summary>
        /// <param name="id">Id of company which need to edit</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int companyId)
        {
            CompanyViewModel company = companyManager.Get(companyId);
            company.PhoneNumber = phoneManager.GetPhoneNumber(company.PhoneId);
            return View(company);
        }

        /// <summary>
        /// Send updated Company to db
        /// </summary>
        /// <param name="id">Id of company which need to edit</param>
        /// <param name="company">ViewModel of Company from View</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CompanyViewModel company)
        {
            if (ModelState.IsValid)
            {
                companyManager.Update(company);
                return RedirectToAction("Index");
            }
            return View(company);
        }

        /// <summary>
        /// Gets IncomingView with Recieved messages from db
        /// </summary>
        /// <param name="companyId">Id of company</param>
        /// <returns>View with incoming messages</returns>
        [HttpGet]
        public IActionResult Incoming(int companyId)
        {
            CompanyViewModel company = companyManager.Get(companyId);
            IEnumerable<RecievedMessageViewModel> recievedMessages =
                recievedMessageManager.GetRecievedMessages(companyId);
            if (company.Type == CompanyType.Recieve || company.Type == CompanyType.SendAndRecieve)
            {
                IEnumerable<AnswersCodeViewModel> answersCodes = answersCodeManager.GetAnswersCodes(companyId);
                foreach (var rm in recievedMessages)
                {
                    if (Regex.IsMatch(rm.MessageText, @"^\d+$"))
                    {
                        AnswersCodeViewModel answersCode = answersCodes.FirstOrDefault(item => item.Code == int.Parse(rm.MessageText));
                        if (answersCode != null)
                            rm.MessageText = answersCode.Answer;
                    }
                }
            }
            ViewBag.RecievedMessages = recievedMessages;
            ViewData["companyId"] = companyId;
            return View();
        }

        /// <summary>
        /// Delete selected message from db
        /// </summary>
        /// <param name="companyId">id of company</param>
        /// <param name="recievedMessageId">id of message to delete</param>
        /// <returns>Redirect to View with incoming messages</returns>
        public IActionResult DeleteIncomingMessage(int companyId, int recievedMessageId)
        {
            recievedMessageManager.Delete(recievedMessageId);
            return RedirectToAction("Incoming", "Company", new { companyId });
        }
    }
}