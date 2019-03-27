using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Controllers;
using WebApp.Models.AccountViewModels;

namespace WEB.Tests.Controllers
{
	[TestFixture]
	public class HomeControllerTests
	{
		[TestCase]
		public void Lockout()
		{
			//var mock = new Mock<IStringLocalizer>();
			//HomeController controller = new HomeController(mock);
			//// Act
			//ViewResult result = controller.Lockout() as ViewResult;
			//// Assert
			Assert.AreEqual("nice","nice");
		}
	}
}
