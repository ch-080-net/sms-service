using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using Model.ViewModels.CompanyViewModels;
using Model.ViewModels.OperatorViewModels;
using Model.ViewModels.TariffViewModels;
using Model.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public CompanyController(ICompanyManager company, IOperatorManager _operator, ITariffManager tariff, 
                                 UserManager<ApplicationUser> userManager, IGroupManager groupManager,
                                 IRecipientManager recipientManager, IPhoneManager phoneManager)
        {
            this.companyManager = company;
            this.operatorManager = _operator;
            this.tariffManager = tariff;
            this.userManager = userManager;
            this.groupManager = groupManager;
            this.phoneManager = phoneManager;
            this.recipientManager = recipientManager;
        }

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
                item.PhoneId = phoneManager.GetPhoneId(item.PhoneNumber);
                item.ApplicationGroupId = GetGroupId();
                int companyId = companyManager.InsertWithId(item);
                if (item.Type == 1)
                {
                    return RedirectToAction("Send", new { companyId });
                }
                if (item.Type == 2)
                {
                    return RedirectToAction("Recieve", new { companyId });
                }
                if (item.Type == 3)
                {
                    return RedirectToAction("SendRecieve", new { companyId });
                }
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Send(int companyId)
        {
            ViewData["companyId"] = companyId;
            CompanyViewModel company = companyManager.Get(companyId);
            SendViewModel item = new SendViewModel();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Send(SendViewModel item)
        {
            if (ModelState.IsValid)
            {
                item.TariffId = tariffManager.GetAll().FirstOrDefault(t => t.Name == item.Tariff).Id;
                companyManager.AddSend(item);
                return RedirectToAction("Index");
            }
            item.RecipientViewModels = recipientManager.GetRecipients(item.Id);
            return View(item);
        }

        [HttpGet]
        public IActionResult Recieve(int companyId)
        {
            ViewData["companyId"] = companyId;
            CompanyViewModel company = companyManager.Get(companyId);
            RecieveViewModel item = new RecieveViewModel();
            item.Id = companyId;
            return View(item);
        }

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
                item.TariffId = tariffManager.GetAll().FirstOrDefault(t => t.Name == item.Tariff).Id;
                companyManager.AddSendRecieve(item);
                return RedirectToAction("Index");
            }
            item.RecipientViewModels = recipientManager.GetRecipients(item.Id);
            return View(item);
        }

        /// <summary>
        /// Get Delete Confirmation View with company information
        /// </summary>
        /// <param name="id">Id of selected item</param>
        /// <returns>View with selected company info</returns>
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CompanyViewModel company = companyManager.GetCompanies(GetGroupId()).FirstOrDefault(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        /// <summary>
        /// Delete selected item from db
        /// </summary>
        /// <param name="id">Id of Company which select to delete</param>
        /// <returns>Company Index View</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            companyManager.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Operators(int companyId)
        {
            IEnumerable<OperatorViewModel> operators = operatorManager.GetAll();
            //ViewBag.operators = operators;
            ViewData["companyId"] = companyId;
            return View();
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
            IEnumerable<TariffViewModel> tariffs = tariffManager.GetTariffs(id);
            ViewBag.tariffs = tariffs;
            ViewData["companyId"] = companyId;
            return View();
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
            companyManager.Update(currentCompany, GetGroupId(), tariffId);
            if (currentCompany.Type == 1)
            {
                return RedirectToAction("Send", "Company", new { companyId });
            }
            else
            {
                return RedirectToAction("SendRecieve", "Company", new { companyId });
            }
        }

        public IActionResult RedirectByType(int companyId)
        {
            CompanyViewModel company = companyManager.Get(companyId);
            if (company.Type == 1)
            {
                return RedirectToAction("Send", "Company", new { companyId });
            }
            else
            {
                return RedirectToAction("SendRecieve", "Company", new { companyId });
            }
        }
    }
}
