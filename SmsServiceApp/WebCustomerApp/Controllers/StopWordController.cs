using BAL.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Interfaces;
using Model.ViewModels.StopWordViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Controllers;
using WebApp.Models;

namespace WebApp.Controllers
{
   
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class StopWordController : Controller
    {
        private readonly IStopWordManager stopWordManager;

        public StopWordController(IStopWordManager stopWord)
        {
            this.stopWordManager = stopWord;
        }
        /// <summary>
        /// Get view with StopWord 
        /// </summary>
        /// <returns>View with stopword</returns>
        public IActionResult Index()
        {
            return View(stopWordManager.GetStopWords());
        }
        /// <summary>
        /// View for creation new StopWord
        /// </summary>
        /// <returns>Create StopWord View</returns>
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Send new StopWord fron view to db
        /// </summary>
        /// <param name="item">ViewModel of StopWord from View</param>
        /// <returns>StopWord index View</returns>
        [Route("~/StopWord/Create")]
        [HttpPost]
        public IActionResult Create(StopWordViewModel item)
        {
            if (ModelState.IsValid)
            {
                stopWordManager.Insert(item);
            }


            return RedirectToAction("Index", "StopWord");
        }

        /// <summary>
        /// Gets EditView with StopWord info from db
        /// </summary>
        /// <param name="id">Id of stopword which need to edit</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            StopWordViewModel word = stopWordManager.GetStopWords().FirstOrDefault(c => c.Id == id);

            if (word == null)
            {
                return NotFound();
            }
            return View(word);
        }
        /// <summary>
        ///  Send updated StopWord to db
        /// </summary>
        /// <param name="wordEdit">Edited fo stopword</param>
        /// <returns>StopWord index View</returns>
        [HttpPost]
        [Route("~/StopWord/Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit( StopWordViewModel wordEdit)
        {
            
            if (ModelState.IsValid)
            {
                stopWordManager.Update(wordEdit);
                return RedirectToAction("Index");
            }
            return View(wordEdit);
        }

        /// <summary>
        /// Get Delete Confirmation View with StopWord information
        /// </summary>
        /// <param name="id">Id of selected item</param>
        /// <returns>View with selected StopWord info</returns>
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StopWordViewModel word = stopWordManager.GetStopWords().FirstOrDefault(c => c.Id == id);

            if (word == null)
            {
                return NotFound();
            }
            return View(word);
        }

        /// <summary>
        /// Delete selected item from db
        /// </summary>
        /// <param name="id">Id of StopWord which select to delete</param>
        /// <returns>StopWord Index View</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            stopWordManager.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
