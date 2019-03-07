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

namespace WebApp.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ICompanyManager companyManager;
        private readonly IOperatorManager operatorManager;
        private readonly ITariffManager tariffManager;
        private readonly UserManager<ApplicationUser> userManager;
        private static int groupId;

        public CompanyController(ICompanyManager company, IOperatorManager _operator, ITariffManager tariff, UserManager<ApplicationUser> userManager)
        {
            this.companyManager = company;
            this.operatorManager = _operator;
            this.tariffManager = tariff;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get view with Companies which belongs to this user ApplicationGroup
        /// </summary>
        /// <returns>View with companies</returns>
        [HttpGet]
        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = userManager.Users.FirstOrDefault(u => u.Id == userId);
            groupId = user.ApplicationGroupId;
            return View(companyManager.GetCompanies(groupId));
        }

        /// <summary>
        /// View for creation new Company
        /// </summary>
        /// <returns>Create Company View</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
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
                item.Message = item.Message.Replace("#company", item.Name);
                companyManager.Insert(item, groupId);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        /// <summary>
        /// Gets EditView with Company info from db
        /// </summary>
        /// <param name="id">Id of company which need to edit</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CompanyViewModel company = companyManager.GetCompanies(groupId).FirstOrDefault(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }
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
        public IActionResult Edit(int id, [Bind]CompanyViewModel company)
        {
            if (ModelState.IsValid)
            {
                var tariffId = companyManager.Get(id).TariffId;
                company.Message = company.Message.Replace("#company", company.Name);
                if (tariffId == 0)
                {
                    companyManager.Update(company, groupId, 0);
                }
                else
                {
                    companyManager.Update(company, groupId, tariffId);
                }
                return RedirectToAction("Index");
            }
            return View(company);
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

            CompanyViewModel company = companyManager.GetCompanies(groupId).FirstOrDefault(c => c.Id == id);

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
            ViewBag.operators = operators;
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
            companyManager.Update(currentCompany, groupId, tariffId);
            return RedirectToAction("Index","Company");
        }
    }
}
