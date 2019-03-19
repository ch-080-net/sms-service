using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BAL.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Model.Interfaces;
using Model.ViewModels.TariffViewModels;
using WebApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    [Authorize (Roles = "Admin")]
    public class TariffController : Controller
    {
        private readonly ITariffManager tariffManager;
        
        public TariffController(ITariffManager tariff)
        {
            this.tariffManager = tariff;
        }

        /// <summary>
        /// Get all Tariffs
        /// </summary>
        /// <param name="OperatorId">The operator for which we show the tariff</param>
        /// <returns>Index View</returns>
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

        /// <summary>
        /// Create tariff View
        /// </summary>
        /// <param name="OperatorId">The operator for which we add the tariff</param>
        /// <returns>Create View</returns>
        [HttpGet]
        public IActionResult Create(int OperatorId)
        {
            ViewData["OperatorId"] = OperatorId;
            return View(); 
        }

        /// <summary>
        /// Create tariff plan and send them to database
        /// </summary>
        /// <param name="item">Model from View</param>
        /// <returns>Operators List</returns>
        [Route("~/Tariff/Create")]
        [HttpPost]
        public IActionResult Create(TariffViewModel item)
        {
            if (ModelState.IsValid) { 
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Tariff tariff = new Tariff();
            tariffManager.Insert(item);
            return RedirectToAction("Operators", "Operator");
            }
            else
            {
                return new ObjectResult("Fill in all fields"); ;
            }
        }

        /// <summary>
        /// Gets EditView with Tariff info from db
        /// </summary>
        /// <param name="id">Id of tariff which need to edit</param>
        /// <returns>Edit View</returns>
        [HttpGet]
        public IActionResult Update(int id)
        {  
          ViewData["id"] = id;
          return View(tariffManager.GetTariffById(id));
        }

        /// <summary>
        /// Send updated Tariff to db
        /// </summary>
        /// <param name="item">Id of Tariff which need to edit</param>
        /// <returns>List of Tariff for selected Operator</returns>
        [HttpPost]
        [Route("~/Tariff/Update")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(TariffViewModel item)
        {
            if (ModelState.IsValid)
            {
                tariffManager.Update(item);
                return View();
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        ///  Get Delete Confirmation View with Tariff information
        /// </summary>
        /// <param name="id">Id of selected item</param>
        /// <returns>View with selected tariff info</returns>
        [HttpGet]
        [Route("~/Tariff/Delete/{id?}")]
        public IActionResult Delete( int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TariffViewModel tariff = tariffManager.GetTariffById(id.Value);

            if (tariff == null)
            {
                return NotFound();
            }
            return View(tariff);
        }


        /// <summary>
        ///  Delete selected item from db
        /// </summary>
        /// <param name="id">Id of Tariff which select to delete</param>
        /// <returns>Operators Index View</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            TariffViewModel tariff = tariffManager.GetTariffs(id.Value).FirstOrDefault(r => r.Id == id);
            tariffManager.Delete(tariff, id.Value);
            return RedirectToAction("Operators","Operator");
        }
          
    }
}