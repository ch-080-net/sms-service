using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL.Interfaces;
using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.AnswersCodeViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    public class AnswersCodeController : Controller
    {
        private readonly IAnswersCodeManager answersCodeManager;
        private readonly ICompanyManager companyManager;

        public AnswersCodeController(IAnswersCodeManager answersCodeManager, ICompanyManager companyManager)
        {
            this.answersCodeManager = answersCodeManager;
            this.companyManager = companyManager;
        }

        [HttpGet]
        public IActionResult Index(int companyId)
        {
            ViewData["CompanyId"] = companyId;
            return View(answersCodeManager.GetAnswersCodes(companyId).ToList());
        }

        [HttpGet]
        public IActionResult Create(int companyId)
        {
            ViewData["CompanyId"] = companyId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] AnswersCodeViewModel item, int companyId)
        {
            if (companyId != 0)
            {
                TempData["companyId"] = companyId;
            }
            TempData.Keep("companyId");
            if (ModelState.IsValid)
            {
                answersCodeManager.Insert(item, (int)TempData.Peek("companyId"));
                return RedirectToAction("Index", "AnswersCode", new { companyId = (int)TempData.Peek("companyId") });
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            AnswersCodeViewModel answersCode = answersCodeManager.GetAnswersCodeById(id);

            if (answersCode == null)
            {
                return NotFound();
            }
            return View(answersCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind]AnswersCodeViewModel answersCode)
        {
            AnswersCodeViewModel answersCodeToEdit = answersCodeManager.GetAnswersCodeById(id);
            answersCode.CompanyId = answersCodeToEdit.CompanyId;
            if (ModelState.IsValid)
            {
                answersCodeManager.Update(answersCode);
                return RedirectToAction("Index", "AnswersCode", new { answersCodeToEdit.CompanyId });
            }
            return View(answersCode);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            AnswersCodeViewModel answersCode = answersCodeManager.GetAnswersCodeById(id);
            if (answersCode == null)
            {
                return NotFound();
            }
            return View(answersCode);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            AnswersCodeViewModel answersCode = answersCodeManager.GetAnswersCodeById(id);
            int companyId = answersCode.CompanyId;
            answersCodeManager.Delete(id);
            return RedirectToAction("Index", "AnswersCode", new { companyId });
        }
    }
}
