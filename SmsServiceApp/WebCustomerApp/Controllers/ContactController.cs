using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BAL.Managers;
using LINQtoCSV;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.ContactViewModels;
using WebApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IContactManager contactManager;
        private readonly UserManager<ApplicationUser> userManager;

        public ContactController(IContactManager contactManager, UserManager<ApplicationUser> userManager)
        {
            this.contactManager = contactManager;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get Contact View 
        /// </summary>
        /// <returns>Contact View</returns>
        public IActionResult Contacts()
        {
            return View();
        }

        /// <summary>
        /// Get List with contacts on current page with search value
        /// </summary>
        /// <returns>List with contacts</returns>
        [Route("~/Contact/GetContactList")]
        [HttpGet]
        public List<ContactViewModel> GetContactList(int pageNumber, int pageSize, string searchValue)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return new List<ContactViewModel>();
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int groupId = userManager.Users.FirstOrDefault(u => u.Id == userId).ApplicationGroupId;
                if (searchValue == null)
                    return contactManager.GetContact(groupId, pageNumber, pageSize);
                else
                    return contactManager.GetContactBySearchValue(groupId, pageNumber, pageSize, searchValue);
            }
            catch
            {
                return new List<ContactViewModel>();
            }
        }

        /// <summary>
        /// Get count of contacts with searchValue
        /// </summary>
        /// <returns>Count of contacts</returns>
        [Route("~/Contact/GetContactCount")]
        [HttpGet]
        public int GetContactCount(string searchValue)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return 0;
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int groupId = userManager.Users.FirstOrDefault(u => u.Id == userId).ApplicationGroupId;
                if (searchValue == null)
                    return contactManager.GetContactCount(groupId);
                else
                    return contactManager.GetContactCountBySearchValue(groupId, searchValue);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Send new Contact from view to db
        /// </summary>
        /// <param name="obj">ViewModel of Contact from View</param>
        /// <returns>String with information of action success</returns>
        [Route("~/Contact/AddContact")]
        [HttpPost]
        public IActionResult AddContact(ContactViewModel obj)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int groupId = userManager.Users.FirstOrDefault(u => u.Id == userId).ApplicationGroupId;
                if (obj.Name == null)
                    obj.Name = "";
                if (obj.Surname == null)
                    obj.Surname = "";
                if (obj.Notes == null)
                    obj.Notes = "";
                if (obj.KeyWords == null)
                    obj.KeyWords = "";
                if (contactManager.CreateContact(obj, groupId))
                    return new ObjectResult("Phone added successfully!");
                else
                    return new ObjectResult("Contact with this phone number already exist!");
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Delete selected item from db
        /// </summary>
        /// <param name="id">Id of Contact which select to delete</param>
        /// <returns>String with information of action success</returns>
        [Route("~/Contact/DeleteContact/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                contactManager.DeleteContact(id);
                return new ObjectResult("Phone deleted successfully!");
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Send updated Contact to db
        /// </summary>
        /// <param name="obj">ViewModel of Contact from View</param>
        /// <returns>String with information of action success</returns>
        [Route("~/Contact/UpdateContact")]
        [HttpPut]
        public IActionResult UpdateContact(ContactViewModel obj)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int groupId = userManager.Users.FirstOrDefault(u => u.Id == userId).ApplicationGroupId;
                if (obj.Name == null)
                    obj.Name = "";
                if (obj.Surname == null)
                    obj.Surname = "";
                if (obj.Notes == null)
                    obj.Notes = "";
                if (obj.KeyWords == null)
                    obj.KeyWords = "";
                contactManager.UpdateContact(obj, groupId);
                return new ObjectResult("Phone modified successfully!");
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Get Contact by id
        /// </summary>
        /// <param name="id">Id of contact</param>
        /// <returns>Contact by id</returns>
        [Route("~/Contact/GetContact/{id}")]
        [HttpGet]
        public ContactViewModel GetContact(int id)
        {
            try
            {
                ContactViewModel contact = contactManager.GetContact(id);
                return contact;
            }
            catch
            {
                return null;
            }
        }

        public void UploadFile(IFormFile ContactFile)
        {
            contactManager.AddContactFromFile(ContactFile);
        }

    }
}
