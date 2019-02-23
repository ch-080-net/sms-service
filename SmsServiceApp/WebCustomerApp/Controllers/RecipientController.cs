using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using WebCustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.ViewModels.RecipientViewModels;

namespace WebApp.Controllers
{
    public class RecipientController : Controller
    {
        private IRecipientManager recipientManager;
        private static int companyId;

        public RecipientController (IRecipientManager recipient)
        {
            this.recipientManager = recipient;
        }

       [Route("~/Recipient/Index/{id}")]
        public IActionResult Index(int id)
        {
            companyId = id;
            return View(recipientManager.GetRecipients(id).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] RecipientViewModel item)
        {
            if (ModelState.IsValid)
            {
                recipientManager.Insert(item, companyId);
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
            RecipientViewModel recipient = recipientManager.GetRecipients(companyId).FirstOrDefault(r => r.Id == id);

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
            if (id != recipient.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                recipientManager.Update(recipient, companyId);
                return RedirectToAction("Index");
            }
            return View(recipient);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
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
        public IActionResult DeleteConfirmed(int? id)
        {
            RecipientViewModel recipient = recipientManager.GetRecipients(companyId).FirstOrDefault(r => r.Id == id);
            recipientManager.Delete(recipient);
            return RedirectToAction("Index");
        }

    }
}
