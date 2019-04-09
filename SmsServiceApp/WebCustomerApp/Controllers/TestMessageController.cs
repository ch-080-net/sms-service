using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.TestMessageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class TestMessageController : Controller
    {
        private ITestMessageManager testMessageManager;

        public TestMessageController(ITestMessageManager testMessageManager)
        {
            this.testMessageManager = testMessageManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(TestMessageViewModel testMessage)
        {
            testMessageManager.SendTestMessage(testMessage);
            return RedirectToAction("Index", "Home");
        }
    }
}
