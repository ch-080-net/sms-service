using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using WebCustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.ViewModels.RecipientViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class RecipientController : Controller
    {
        private IRecipientManager recipientManager;
		private ICompanyManager companyManager;

        public RecipientController (IRecipientManager recipient, ICompanyManager company)
        {
            this.recipientManager = recipient;
			this.companyManager = company;
        }

        [HttpGet]
        public IActionResult Index(int companyId)
        {
            ViewData["CompanyId"] = companyId;
            return View(recipientManager.GetRecipients(companyId).ToList());
        }

        [HttpGet]
        public IActionResult Create(int companyId)
        {
			int limit = companyManager.GetTariffLimit(companyId);
			int count = recipientManager.GetRecipients(companyId).Count();

			if (limit > count)
			{
				ViewData["CompanyId"] = companyId;
				return View();
			}
			else
			{
				ViewData["errorMessage"] = "Recipients limit is over";
				return RedirectToAction("Index", "Recipient", new { companyId });
			}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] RecipientViewModel item, int companyId)
        {
            if (ModelState.IsValid)
            {
                recipientManager.Insert(item, companyId);
                return RedirectToAction("Index", "Recipient", new { companyId });
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            RecipientViewModel recipient = recipientManager.GetRecipientById(id);

            if (recipient == null)
            {
                return NotFound();
            }
            return View(recipient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind]RecipientViewModel recipient)
        {
            RecipientViewModel recipientToEdit = recipientManager.GetRecipientById(id);
            int companyId = recipientToEdit.CompanyId;
            if (ModelState.IsValid)
            {
                recipientManager.Update(recipientToEdit);
                return RedirectToAction("Index", "Recipient", new { companyId });
            }
            return View(recipient);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            RecipientViewModel recipient = recipientManager.GetRecipientById(id);
            if (recipient == null)
            {
                return NotFound();
            }
            return View(recipient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            RecipientViewModel recipient = recipientManager.GetRecipientById(id);
            int companyId = recipient.CompanyId;
            recipientManager.Delete(id);
            return RedirectToAction("Index", "Recipient", new { companyId });
        }

    }
}
