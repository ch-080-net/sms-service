using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.TariffViewModels;
using WebCustomerApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    public class TariffController : Controller
    {
        private readonly ITariffManager tariffManager;
        public TariffController(ITariffManager tariff)
        {
            this.tariffManager = tariff;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        [Route("~/Tariff/GetAll")]
        [HttpGet]
        public IEnumerable<TariffViewModel> GetAll()
        {
            string operatorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //change tommorow
            IEnumerable<TariffViewModel> tariffs = tariffManager.GetTariffs().Where(com =>com.Name  == operatorId);
            return tariffs;
        }
        [Route("~/Tariff/Create")]
        [HttpPost]
        public IActionResult Create(TariffViewModel item)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Tariff tariff = new Tariff();
            tariffManager.Insert(item);
            return new ObjectResult("Recipient added successfully!");
        }

    }
}
