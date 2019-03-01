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
        private readonly IRecipientManager recipientManager;
        private readonly ICompanyManager companyManager;

        public RecipientController (IRecipientManager recipient, ICompanyManager companyManager)
        {
            this.recipientManager = recipient;
            this.companyManager = companyManager;
        }

        [HttpGet]
        public IActionResult Index(int companyId)
        {
			int limit = companyManager.GetTariffLimit(companyId);
			int count = recipientManager.GetRecipients(companyId).Count();

			ViewData["CompanyId"] = companyId;

			if (limit == count)
				ViewData["warningMessage"] = "Recipients limit is full";
			else if (limit < count)
				ViewData["warningMessage"] = "Recipients limit is overflowing";

			return View(recipientManager.GetRecipients(companyId).ToList());
        }

        [HttpGet]
        public IActionResult Create(int companyId)
        {
			ViewData["CompanyId"] = companyId;
			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] RecipientViewModel item, int companyId)
        {
            if(companyId != 0)
            {
                TempData["companyId"] = companyId;
            }
            TempData.Keep("companyId");
            bool IsRecipientPhoneExist = recipientManager.GetRecipients(companyId).Any(r => r.PhoneNumber == item.PhoneNumber);
            if (IsRecipientPhoneExist)
            {
                ModelState.AddModelError("PhoneNumber", "Recipient with this number already exists");
            }
            if (ModelState.IsValid)
            {
                recipientManager.Insert(item, (int)TempData.Peek("companyId"));
                return RedirectToAction("Index", "Recipient", new { companyId = (int)TempData.Peek("companyId") });
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
            recipient.CompanyId = companyId;
            if (ModelState.IsValid)
            {
                recipientManager.Update(recipient);
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
