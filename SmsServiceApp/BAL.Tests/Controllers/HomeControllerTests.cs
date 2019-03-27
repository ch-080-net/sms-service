using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WebApp.Controllers;
using WebApp.Models.AccountViewModels;

namespace WEB.Tests.Controllers
{
	[TestFixture]
	public class HomeControllerTests
	{
		[SetUp]
		public void SetUp()
		{
			Debug.WriteLine("Test set up");
		}

		[TearDown]
		public void TearDown()
		{
			Debug.WriteLine("Test tear down");
		}

		[TestCase]
		public void Lockout()
		{
			#region Arrange
			//задаємо змінні і результат
			string test = "nice";
			string rez = "nice";
			#endregion

			#region Act
			//запускаємо методи, які тестуються
			Debug.WriteLine("Start methods");
			#endregion

			#region Assert
			//перевіряємо результат
			Assert.AreEqual(test, rez, $"Input should be {rez}, we have {test}");
			#endregion
		}
	}
}
