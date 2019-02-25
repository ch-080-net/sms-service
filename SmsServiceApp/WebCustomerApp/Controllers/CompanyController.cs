using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCustomerApp.Models;
using Model.ViewModels.CompanyViewModels;
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
        private static string userId;

        public CompanyController(ICompanyManager company, IRecipientManager recipient)
        {
            this.recipientManager = recipient;
            this.companyManager = company;
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
                string name = "{0}";

                //change according to further requirement
                item.Message = item.Message.Replace("#name", name).Replace("#company", item.Name);

                //then move to the send function to the SMPP 
                //foreach (var res in item.RecipientViewModels)
                //{
                //    string outServisMessage = String.Format(item.Message, RecipientViewModel.name)
                //}

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
        //public IActionResult Create(CompanyViewModel item)
        //{
        //    string name = "{0}";

        //    //change according to further requirement
        //    item.Message = item.Message.Replace("#name", name).Replace("#company", item.Name);
            
        //    //then move to the send function to the SMPP 
        //    //foreach (var res in item.RecipientViewModels)
        //    //{
        //    //    string outServisMessage = String.Format(item.Message, RecipientViewModel.name)
        //    //}

        //    string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    item.ApplicationUserId = userId;
          
        //    companyManager.Insert(item);
        //    return new ObjectResult("Recipient added successfully!");
        //}
    }
}
