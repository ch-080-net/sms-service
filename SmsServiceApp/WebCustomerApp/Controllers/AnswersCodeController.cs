using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL.Interfaces;
using BAL.Managers;
using BAL.Services;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.AnswersCodeViewModels;

namespace WebApp.Controllers
{
    public class AnswersCodeController : Controller
    {
        private readonly IAnswersCodeManager answersCodeManager;
        private readonly ILoggerManager logger;

        public AnswersCodeController(IAnswersCodeManager answersCodeManager, ILoggerManager logger)
        {
            this.answersCodeManager = answersCodeManager;
            this.logger = logger;
        }

        /// <summary>
        /// Get view with AnswersCodes which belongs to this Company
        /// </summary>
        /// <returns>View with AnswersCodes</returns>
        [HttpGet]
        public IActionResult Index(int companyId)
        {
            ViewData["CompanyId"] = companyId;
            logger.LogInfo("Fetching all the AnswerCodes from the storage");
            var answerCodes = answersCodeManager.GetAnswersCodes(companyId).ToList();
            logger.LogInfo($"Returning {answerCodes.Count()} answerCodes.");
            return View(answerCodes);
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
                logger.LogInfo($"AnswerCode {item.Code} was inserted to db");
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
                logger.LogInfo($"AnswerCode {id} was updated");
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
            answersCodeManager.Delete(id);
            logger.LogInfo($"AnswerCode {id} was deleted");
            return RedirectToAction("Index", "AnswersCode", new { companyId });
        }
    }
}
