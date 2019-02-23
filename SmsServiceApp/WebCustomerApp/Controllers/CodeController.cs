using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.CodeViewModels;
using Model.ViewModels.OperatorViewModels;
using BAL.Managers;

namespace WebApp.Controllers
{
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
        public IActionResult Codes(int OperatorId, int Page = 1)
        {
            ViewBag.NumOfPages = codeManager.GetNumberOfPages(OperatorId, 20);
            ViewBag.CurrentPage = (Page <= ViewBag.NumOfPages) ? Page : ViewBag.NumOfPages;
            ViewBag.OperatorName = operatorManager.GetById(OperatorId).Name;
            ViewBag.OperatorId = OperatorId;
            var codes = codeManager.GetPage(OperatorId, Page, 20);
            var navcodes = new List<CodeWithNavigationViewModel>();
            foreach (var iter in codes)
            {
                navcodes.Add(new CodeWithNavigationViewModel()
                {
                    Code = iter,
                    Page = Page,
                });
            }
            ViewBag.Codes = navcodes;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Codes(CodeWithNavigationViewModel newCode)
        {
            if (ModelState.IsValid)
            {
                bool result = codeManager.Add(newCode.Code);
                if (!result)
                {
                    return RedirectToAction("Codes", "Code", new { newCode.Code.OperatorId });
                }
                else
                {
                    return RedirectToAction("Codes", "Code"
                        , new {newCode.Code.OperatorId, newCode.Page});
                }
            }
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult Remove(int CodeId, int OperatorId, int Page = 1)
        {
            bool result = codeManager.Remove(CodeId);
            if (!result)
            {
                return RedirectToAction("Codes", "Code", new { OperatorId });
            }
            else
            {
                return RedirectToAction("Codes", "Code", new {OperatorId, Page});
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Code(CodeWithNavigationViewModel editedCode)
        {
            if (ModelState.IsValid)
            {
                var result = codeManager.Update(editedCode.Code);
                if (!result)
                {
                    return RedirectToAction("Codes", "Code", new { editedCode.Code.OperatorId });
                }
                else
                {
                    return RedirectToAction("Codes", "Code"
                        , new { editedCode.Code.OperatorId, editedCode.Page});
                }
            }
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult NextPage(int OperatorId, int CurrentPage)
        {
            if (CurrentPage < codeManager.GetNumberOfPages(OperatorId))
                return RedirectToAction("Codes", "Code", new { Page = ++CurrentPage, OperatorId });
            else
                return RedirectToAction("Codes", "Code", new { Page = CurrentPage, OperatorId });
        }

        public IActionResult PreviousPage(int OperatorId, int CurrentPage)
        {
            if (CurrentPage > 1)
                return RedirectToAction("Codes", "Code", new { Page = --CurrentPage, OperatorId});
            else
                return RedirectToAction("Codes", "Code", new { Page = CurrentPage, OperatorId});
        }

    }
}
