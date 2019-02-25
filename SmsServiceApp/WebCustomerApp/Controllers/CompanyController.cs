using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCustomerApp.Models;
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
        private readonly IRecipientManager recipientManager;
        private readonly IOperatorManager operatorManager;
        private readonly ITariffManager tariffManager;
        private static string userId;

        public CompanyController(ICompanyManager company, IRecipientManager recipient, IOperatorManager _operator, ITariffManager tariff)
        {
            this.recipientManager = recipient;
            this.companyManager = company;
            this.operatorManager = _operator;
            this.tariffManager = tariff;
        }

        [HttpGet]
        public IActionResult Index()
        {
            userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(companyManager.GetCompanies(userId));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] CompanyViewModel item)
        {
            if (ModelState.IsValid)
            {
                companyManager.Insert(item, userId);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CompanyViewModel company = companyManager.GetCompanies(userId).FirstOrDefault(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind]CompanyViewModel company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                companyManager.Update(company, userId);
                return RedirectToAction("Index");
            }
            return View(company);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CompanyViewModel company = companyManager.GetCompanies(userId).FirstOrDefault(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            companyManager.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CompanyViewModel company = companyManager.GetCompanies(userId).FirstOrDefault(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpGet]
        public IActionResult Operators(int companyId)
        {
            IEnumerable<OperatorViewModel> operators = operatorManager.GetAll();
            ViewBag.operators = operators;
            ViewData["companyId"] = companyId;
            return View();
        }

        [HttpGet]
        public IActionResult Tariffs(int id, int companyId)
        {
            IEnumerable<TariffViewModel> tariffs = tariffManager.GetTariffs(id);
            ViewBag.tariffs = tariffs;
            ViewData["companyId"] = companyId;
            return View();
        }

        [HttpGet]
        public IActionResult ChangeTariff(int companyId, int tariffId)
        {
            CompanyViewModel currentCompany = companyManager.Get(companyId);
            currentCompany.TariffId = tariffId;
            companyManager.Update(currentCompany, userId);
            return RedirectToAction("Index","Company");
        }
    }
}
