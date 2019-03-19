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

        /// <summary>
        /// Get view with AnswersCodes which belongs to this Company
        /// </summary>
        /// <returns>View with AnswersCodes</returns>
        [HttpGet]
        public IActionResult Index(int companyId)
        {
            ViewData["CompanyId"] = companyId;
            return View(answersCodeManager.GetAnswersCodes(companyId).ToList());
        }

        /// <summary>
        /// View for creation of new AnswersCode
        /// </summary>
        /// <returns>Create AncwersCode View</returns>
        [HttpGet]
        public IActionResult Create(int companyId)
        {
            ViewData["CompanyId"] = companyId;
            return View();
        }

        /// <summary>
        /// Send new AnswersCode from view to db
        /// </summary>
        /// <param name="item">ViewModel of AnswersCode from View</param>
        /// <returns>AnswersCode index View</returns>
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

        /// <summary>
        /// View for editing of AnswersCode
        /// </summary>
        /// <param name="id">id of AnswersCode for editing</param>
        /// <returns>Edit View with AnswersCode for editing</returns>
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

        /// <summary>
        /// Send edited AnswersCode from view to db
        /// </summary>
        /// <param name="id">id of AnswersCode for editing</param>
        /// <param name="answersCode">edited AnswersCode</param>
        /// <returns>View with AnswersCodes</returns>
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

        /// <summary>
        /// Delete AnswersCode
        /// </summary>
        /// <param name="id">id of AnswersCode for deleting</param>
        /// <param name="companyId">id of company</param>
        /// <returns>View with AnswersCode</returns>
        public IActionResult Delete(int id, int companyId)
        {
            AnswersCodeViewModel answersCode = answersCodeManager.GetAnswersCodeById(id);
            answersCodeManager.Delete(id);
            return RedirectToAction("Index", "AnswersCode", new { companyId });
        }
    }
}
