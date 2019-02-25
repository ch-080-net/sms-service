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

        public RecipientController (IRecipientManager recipient)
        {
            this.recipientManager = recipient;
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
            if (ModelState.IsValid)
            {
                recipientManager.Insert(item, companyId);
                return RedirectToAction("Index", "Recipient", new { companyId });
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Edit(int? id, int companyId)
        {
            if (id == null)
            {
                return NotFound();
            }
            RecipientViewModel recipient = recipientManager.GetRecipients(companyId).FirstOrDefault(r => r.Id == id);

            if (recipient == null)
            {
                return NotFound();
            }
            return View(recipient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind]RecipientViewModel recipient, int companyId)
        {
            if (id != recipient.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                recipientManager.Update(recipient, companyId);
                return RedirectToAction("Index", "Recipient");
            }
            return View(recipient);
        }

        [HttpGet]
        public IActionResult Delete(int? id, int companyId)
        {
            if (id == null)
            {
                return NotFound();
            }

            RecipientViewModel recipient = recipientManager.GetRecipients(companyId).FirstOrDefault(r => r.Id == id);

            if (recipient == null)
            {
                return NotFound();
            }
            return View(recipient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id, int companyId)
        {
            RecipientViewModel recipient = recipientManager.GetRecipients(companyId).FirstOrDefault(r => r.Id == id);
            recipientManager.Delete(recipient);
            return RedirectToAction("Index", "Recipient");
        }

    }
}
