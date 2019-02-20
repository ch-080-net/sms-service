using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using WebCustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class RecipientController : Controller
    {
        private IRecipientManager recipientManager;

        public RecipientController (IRecipientManager recipient)
        {
            this.recipientManager = recipient;
        }

        public IEnumerable<Recipient> GetAll()
        {
            return recipientManager.GetRecipients();
        }
    }
}
