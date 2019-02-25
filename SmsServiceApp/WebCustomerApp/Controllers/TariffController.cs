using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using Model.Interfaces;
using Model.ViewModels.TariffViewModels;
using WebCustomerApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    public class TariffController : Controller
    {
        private ITariffManager tariffManager;

        public TariffController(ITariffManager tariff)
        {
            this.tariffManager = tariff;
        }
        //// get: /<controller>/
        public IActionResult index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult GetTariffList()
        {
            ViewBag.Tariff = tariffManager.GetAll();
            return View();
        }
       
        
        [HttpGet]
        public IActionResult Create(int OperatorId)
        {
            ViewData["OperatorId"] = OperatorId;
            return View();
        }
        [Route("~/Tariff/Create")]
        [HttpPost]
     
        public IActionResult Create(TariffViewModel item)
        {
            tariffManager.Insert(item);
            return new ObjectResult("Tariff added successfully!");
        }
        [HttpPost]
        [Route("~/Tariff/Update")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(TariffViewModel editedtariff)
        {
            var result = tariffManager.Update(editedtariff);
            if (ModelState.IsValid)
            {
                if (result == false)
                {
                    ModelState.AddModelError(string.Empty, "Modify failed");
                    return RedirectToAction("Index", "Tariff");
                }
                else
                {
                    return RedirectToAction("Index", "Tariff");
                }
            }
            return RedirectToAction("Index", "Tariff");
        }
        public IActionResult Remove(int tariffId)
        {
            bool result = tariffManager.Delete(tariffId);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Delete failed");
                return RedirectToAction("Operators", "Operator");
            }
            else
            {
                return RedirectToAction("Operators", "Operator");
            }
        }
        public IActionResult NextPage(int CurrentPage)
        {
            if (CurrentPage < tariffManager.GetNumberOfPages())
                return RedirectToAction("Index", "Tariff", new { Page = ++CurrentPage });
            else
                return RedirectToAction("Index", "Tariff", new { Page = CurrentPage });
        }

        public IActionResult PreviousPage(int CurrentPage)
        {
            if (CurrentPage > 1)
                return RedirectToAction("Index", "Tariff", new { Page = --CurrentPage });
            else
                return RedirectToAction("Index", "Tariff", new { Page = CurrentPage });
        }

    }
}