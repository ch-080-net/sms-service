﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BAL.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Interfaces;
using Model.ViewModels.TariffViewModels;
using WebCustomerApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    [Authorize (Roles = "Admin")]
    public class TariffController : Controller
    {
        private ITariffManager tariffManager;
       

        public TariffController(ITariffManager tariff)
        {
            this.tariffManager = tariff;
        }
       
        [Route("~/Tariff/Index")]
        public IActionResult Index( int OperatorId)
        {
            ViewData["OperatorId"] = OperatorId;
           
            return View(tariffManager.GetTariffs(OperatorId).ToList());
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

        [HttpGet]
        public IActionResult Update(int id)
        {  
          ViewData["id"] = id;
          return View(tariffManager.GetTariffById(id));
        }

        [HttpPost]
        [Route("~/Tariff/Update")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(TariffViewModel item)
        {
            tariffManager.Update(item);
            return View();
        }
      
        [HttpGet]
        [Route("~/Tariff/Delete/{id?}")]
        public IActionResult Delete( int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TariffViewModel tariff = tariffManager.GetTariffById(id.Value);
            tariffManager.Delete(tariff);

            if (tariff == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            TariffViewModel tariff = tariffManager.GetTariffs(id.Value).FirstOrDefault(r => r.Id == id);
            tariffManager.Delete(tariff);
            return RedirectToAction("Index");
        }
          
    }
}