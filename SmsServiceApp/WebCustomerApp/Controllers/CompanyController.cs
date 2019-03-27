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
using Microsoft.AspNetCore.Mvc.Rendering;
using BAL.Interfaces;
using Model.ViewModels.RecievedMessageViewModel;
using Model.ViewModels.AnswersCodeViewModels;
using System.Text.RegularExpressions;

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

        public CompanyController(ICompanyManager company, IOperatorManager _operator, ITariffManager tariff, 
                                 UserManager<ApplicationUser> userManager, IGroupManager groupManager,
                                 IRecipientManager recipientManager, IPhoneManager phoneManager,
                                 IRecievedMessageManager recievedMessageManager, IAnswersCodeManager answersCodeManager)
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
            return View(companyManager.GetCompanies(GetGroupId()));
        }

        /// <summary>
        /// View for creation new Company
        /// </summary>
        /// <returns>Create Company View</returns>
        [HttpGet]
        public IActionResult Create()
        {
            CompanyViewModel company = new CompanyViewModel();
            var phoneId = groupManager.Get(GetGroupId()).PhoneId;
            company.PhoneNumber = phoneManager.GetPhoneNumber(phoneId);
            return View(company);
        }

        /// <summary>
        /// Send new Company fron view to db
        /// </summary>
        /// <param name="item">ViewModel of Company from View</param>
        /// <returns>Company index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] CompanyViewModel item)
        {
            if (ModelState.IsValid)
            {
                if (phoneManager.IsPhoneNumberExist(item.PhoneNumber))
                {
                    item.PhoneId = phoneManager.GetPhoneId(item.PhoneNumber);
                }
                else
                {
                    Phone newPhone = new Phone();
                    newPhone.PhoneNumber = item.PhoneNumber;
                    phoneManager.Insert(newPhone);
                    item.PhoneId = phoneManager.GetPhones().FirstOrDefault(p => p.PhoneNumber == item.PhoneNumber).Id;
                }
                item.ApplicationGroupId = GetGroupId();
                int companyId = companyManager.InsertWithId(item);
                if (item.Type == CompanyType.Send)
                {
                    return RedirectToAction("Send", new { companyId });
                }
                if (item.Type == CompanyType.Recieve)
                {
                    return RedirectToAction("Recieve", new { companyId });
                }
                if (item.Type == CompanyType.SendAndRecieve)
                {
                    return RedirectToAction("SendRecieve", new { companyId });
                }
            }
            return View(item);
        }

        /// <summary>
        /// Return View of Send details
        /// </summary>
        /// <param name="companyId">companyId</param>
        /// <returns>View of send details</returns>
        [HttpGet]
        public IActionResult Send(int companyId)
        {
            ViewData["companyId"] = companyId;
            CompanyViewModel company = companyManager.Get(companyId);
            SendViewModel item = new SendViewModel();
            item.Id = companyId;
            item.TariffId = company.TariffId;
            item.RecipientsCount = recipientManager.GetRecipients(companyId).Count();
            if (item.TariffId != 0)
            {
                var tariff = tariffManager.GetById(item.TariffId).Name;
                item.Tariff = tariff;
            }
            return View(item);
        }

        /// <summary>
        /// Update company in db with send info
        /// </summary>
        /// <param name="item">Model from View</param>
        /// <returns>Index if all valid</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Send(SendViewModel item)
        {
            
            if (ModelState.IsValid)
            {
                if (item.SendingTime < DateTime.Now)
                {
                    item.SendingTime = DateTime.Now.AddMinutes(1);
                }
                item.TariffId = tariffManager.GetAll().FirstOrDefault(t => t.Name == item.Tariff).Id;
                companyManager.AddSend(item);
                return RedirectToAction("Index","Company");
            }
            item.RecipientsCount = recipientManager.GetRecipients(item.Id).Count();
            return View(item);
        }

        /// <summary>
        /// Return View of Recieve details
        /// </summary>
        /// <param name="companyId">companyId</param>
        /// <returns>View of recieve details</returns>
        [HttpGet]
        public IActionResult Recieve(int companyId)
        {
            ViewData["companyId"] = companyId;
            RecieveViewModel item = new RecieveViewModel();
            item.Id = companyId;
            return View(item);
        }

        /// <summary>
        /// Update company in db with recieve info
        /// </summary>
        /// <param name="item">Model from View</param>
        /// <returns>Index if all valid</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Recieve(RecieveViewModel item)
        {
            if(item.StartTime <= DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "The date can not be less than the current");
            }
            if (item.EndTime <= item.StartTime)
            {
                ModelState.AddModelError(string.Empty, "The date can not be less than the start date");
            }
            if (ModelState.IsValid)
            {
                companyManager.AddRecieve(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        /// <summary>
        /// Return View of SendAndRecieve details
        /// </summary>
        /// <param name="companyId">companyId</param>
        /// <returns>View of sendRecieve details</returns>
        [HttpGet]
        public IActionResult SendRecieve(int companyId)
        {
            ViewData["companyId"] = companyId;
            CompanyViewModel company = companyManager.Get(companyId);
            SendRecieveViewModel item = new SendRecieveViewModel();
            item.Id = companyId;
            item.TariffId = company.TariffId;
            item.RecipientsCount = recipientManager.GetRecipients(companyId).Count();
            if (item.TariffId != 0)
            {
                var tariff = tariffManager.GetById(item.TariffId).Name;
                item.Tariff = tariff;
            }
            return View(item);
        }

        /// <summary>
        /// Update company in db with sendRecieve info
        /// </summary>
        /// <param name="item">Model from View</param>
        /// <returns>Index if all valid</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendRecieve(SendRecieveViewModel item)
        {
            if (item.StartTime <= DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "The date can not be less than the current");
            }
            if (item.EndTime <= item.StartTime)
            {
                ModelState.AddModelError(string.Empty, "The date can not be less than the start date");
            }
     
            if (ModelState.IsValid)
            {
                if (item.SendingTime < DateTime.Now)
                {
                    item.SendingTime = DateTime.Now.AddMinutes(1);
                }
                item.TariffId = tariffManager.GetAll().FirstOrDefault(t => t.Name == item.Tariff).Id;
                companyManager.AddSendRecieve(item);
                return RedirectToAction("Index");
            }
            item.RecipientsCount = recipientManager.GetRecipients(item.Id).Count();
            return View(item);
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
        public IActionResult Tariffs(int id, int companyId)
        {
			TariffsViewModel model = new TariffsViewModel();
            model.TariffsList = tariffManager.GetTariffs(id);
            ViewData["companyId"] = companyId;
            return View(model);
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
                    AnswersCodeViewModel answersCode = answersCodes.FirstOrDefault(item => item.Code == int.Parse(rm.MessageText));
                    if (Regex.IsMatch(rm.MessageText, @"^\d+$") && answersCode != null)
                        rm.MessageText = answersCodes.First(ac => ac.Code == int.Parse(rm.MessageText)).Answer;
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
