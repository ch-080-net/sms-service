using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BAL.Interfaces;
using BAL.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Interfaces;
using Model.ViewModels.CompanyViewModels;
using Model.ViewModels.OperatorViewModels;
using Model.ViewModels.RecievedMessageViewModel;
using Model.ViewModels.TariffViewModels;
using WebApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    public class SMSEmulatorController : Controller
    {
        private readonly ICompanyManager companyManager;
        private readonly IOperatorManager operatorManager;
        private readonly ITariffManager tariffManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGroupManager groupManager;
        private readonly IPhoneManager phoneManager;
        private readonly IRecipientManager recipientManager;
        private readonly IRecievedMessageManager recievedMessageManager;

        public SMSEmulatorController(ICompanyManager company, IOperatorManager _operator, ITariffManager tariff,
                                UserManager<ApplicationUser> userManager, IGroupManager groupManager,
                                IRecipientManager recipientManager, IPhoneManager phoneManager,
                                IRecievedMessageManager recievedMessageManager)
        {
            this.companyManager = company;
            this.operatorManager = _operator;
            this.tariffManager = tariff;
            this.userManager = userManager;
            this.groupManager = groupManager;
            this.phoneManager = phoneManager;
            this.recipientManager = recipientManager;
            this.recievedMessageManager = recievedMessageManager;
         
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
            }
            return View("SendRecieve");
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
            item.RecipientViewModels = recipientManager.GetRecipients(companyId);
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
        public IActionResult SendRecieve(SendRecieveViewModel item, int companyId)
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
            ViewData["companyId"] = companyId;
            CompanyViewModel company = companyManager.Get(companyId);
            item.RecipientViewModels = recipientManager.GetRecipients(companyId);
            item.TariffId = company.TariffId;
            if (item.TariffId != 0)
            {
                var tariff = tariffManager.GetById(item.TariffId).Name;
                item.Tariff = tariff;
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
            return RedirectToAction("SendRecieve", "Company", new { companyId });
          
        }

        /// <summary>
        /// Redirect to action by company type
        /// </summary>
        /// <param name="companyId">company id</param>
        /// <returns>view depended of type</returns>
        public IActionResult RedirectByType(int companyId)
        {
            CompanyViewModel company = companyManager.Get(companyId);
            return RedirectToAction("SendRecieve", "Company", new { companyId });
      
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
            ViewBag.RecievedMessages = recievedMessages;
            ViewData["companyId"] = companyId;
            return View();
        }



    }
}
