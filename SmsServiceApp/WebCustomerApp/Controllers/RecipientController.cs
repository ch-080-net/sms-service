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
        private IPhoneManager phoneManager;

        public RecipientController (IRecipientManager recipient, IPhoneManager phoneManager)
        {
            this.recipientManager = recipient;
            this.phoneManager = phoneManager;
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
            ViewData["CompanyId"] = companyId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] RecipientViewModel item, int companyId)
        {
            string phoneNumber = item.PhoneNumber;
            bool IsPhoneNumberExist = phoneManager.GetPhones().Any(p => p.PhoneNumber == phoneNumber);
   
            if (IsPhoneNumberExist == true)
            {
                ModelState.AddModelError("PhoneNumber", "This phone number already exists");
            }
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
            recipient.CompanyId = companyId;

            string phoneNumber = recipient.PhoneNumber;
            bool IsPhoneNumberExist = phoneManager.GetPhones().Any(p => p.PhoneNumber == phoneNumber);

            if (IsPhoneNumberExist == true)
            {
                ModelState.AddModelError("PhoneNumber", "This phone number already exists");
            }
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
