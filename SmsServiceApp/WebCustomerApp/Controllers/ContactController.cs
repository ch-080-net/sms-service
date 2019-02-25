using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL.Managers;
using Microsoft.AspNetCore.Mvc;

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
        public void GetContactList(int pageNumber, int pageSize, string searchValue)
        {
            if (searchValue == "")
                _contactManager.GetContact(pageNumber, pageSize);
            else
                _contactManager.GetContactByName(pageNumber, pageSize, searchValue);
        }

        public void GetPhoneCount(string searchValue)
        {
            if (searchValue == "")
                _contactManager.GetContactCount();
            else
                _contactManager.GetContactByNameCount(searchValue);
        }
        //// GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
