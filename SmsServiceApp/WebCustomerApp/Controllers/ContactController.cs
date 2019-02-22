using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.ContactViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactManager _contactManager;

        public ContactController(IContactManager contactManager)
        {
            _contactManager = contactManager;
        }

        public IActionResult Contacts()
        {
            return View();
        }

        [Route("~/Contact/GetContactList")]
        [HttpGet]
        public List<ContactViewModel> GetContactList(int pageNumber, int pageSize, string searchValue)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (searchValue == null)
                return _contactManager.GetContact(userId, pageNumber, pageSize);
            else
                return _contactManager.GetContactBySearchValue(userId, pageNumber, pageSize, searchValue);
        }

        [Route("~/Contact/GetContactCount")]
        [HttpGet]
        public int GetContactCount(string searchValue)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (searchValue == null)
                return _contactManager.GetContactCount(userId);
            else
                return _contactManager.GetContactBySearchValueCount(userId, searchValue);
        }

        [Route("~/Contact/AddContact")]
        [HttpPost]
        public IActionResult AddContact(ContactViewModel obj)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _contactManager.CreateContact(obj, userId);
            return new ObjectResult("Phone added successfully!");
        }

        [Route("~/Contact/DeleteContact/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _contactManager.DeleteContact(id);
            return new ObjectResult("Phone deleted successfully!");
        }

        [Route("~/Contact/UpdateContact")]
        [HttpPut]
        public IActionResult UpdateContact(ContactViewModel obj)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _contactManager.UpdateContact(obj, userId);
            return new ObjectResult("Phone modified successfully!");
        }
    }
}
