using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.CodeViewModels;
using Model.ViewModels.OperatorViewModels;
using BAL.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Model.Interfaces;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CodeController : Controller
    {
        private ICodeManager codeManager;
        private IOperatorManager operatorManager;

        public CodeController(ICodeManager codeManager, IOperatorManager operatorManager)
        {
            this.codeManager = codeManager;
            this.operatorManager = operatorManager;
        }

        [HttpGet]
        public IActionResult Codes(PageState pageState)
        {
            if (TempData.ContainsKey("OperatorId"))
                pageState.OperatorId = Convert.ToInt32(TempData["OperatorId"]);


            ViewBag.Codes = codeManager.GetPage(OperatorId, this.CurrentPage, 20, this.SearchQuerry);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Codes(CodeViewModel newCode)
        {
            if (ModelState.IsValid)
            {
                newCode.OperatorId = this.OperatorId;
                bool result = codeManager.Add(newCode);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Error occurred while adding code";
                    return RedirectToAction("Codes", "Code");
                }
                else
                {
                    return RedirectToAction("Codes", "Code");
                }
            }
            TempData["ErrorMessage"] = "Internal error";
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult Remove(int CodeId)
        {
            bool result = codeManager.Remove(CodeId);
            if (!result)
            {
                TempData["ErrorMessage"] = "Error occurred while removing code";
                return RedirectToAction("Codes", "Code");
            }
            else
            {
                return RedirectToAction("Codes", "Code");
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Code(CodeViewModel editedCode)
        {
            if (ModelState.IsValid)
            {
                editedCode.OperatorId = this.OperatorId;
                var result = codeManager.Update(editedCode);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Error occurred while editing code";
                    return RedirectToAction("Codes", "Code");
                }
                else
                {
                    return RedirectToAction("Codes", "Code");
                }
            }
            TempData["ErrorMessage"] = "Internal error";
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult NextPage()
        {
            if (this.CurrentPage < codeManager.GetNumberOfPages(this.OperatorId, 20, this.SearchQuerry))
            {
                this.CurrentPage++;
                return RedirectToAction("Codes", "Code");
            }
            else
                return RedirectToAction("Codes", "Code");
        }

        public IActionResult PreviousPage()
        {
            if (this.CurrentPage > 1)
            {
                this.CurrentPage--;
                return RedirectToAction("Codes", "Code");
            }
            else
                return RedirectToAction("Codes", "Code");
        }

        public IActionResult SelectPage(int Page)
        {
            this.CurrentPage = Page;
            return RedirectToAction("Codes", "Code");
        }

        [HttpPost]
        public IActionResult SearchCodes(CodeSearchViewModel Search)
        {
            if (ModelState.IsValid)
            {
                this.SearchQuerry = Search.SearchQuerry ?? "";
                return RedirectToAction("Codes", "Code");
            }
            else
            {
                return RedirectToAction("Codes", "Code");
            }
        }

    }
}
