using System;
using System.Collections.Generic;
using System.Text;
using BAL.Managers;
using Model.ViewModels.StopWordViewModels;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WebApp.Models;

namespace BAL.Tests.ManagersTests
{
	[TestFixture]
	public class StopWordManagerTests : TestInitializer
	{
		private StopWordManager manager;

		[SetUp]
		protected override void Initialize()
		{
			manager = new StopWordManager(mockUnitOfWork.Object, mockMapper.Object);
		}

		[Test]
		public void GetStopWords_GettingCollection_ReturnCollection()
		{
			List<StopWord> stopWordsList = new List<StopWord>(){new StopWord()};
			List<StopWordViewModel> stopWordsViewModelList = new List<StopWordViewModel>() { new StopWordViewModel() };

			mockUnitOfWork.Setup(m => m.StopWords.GetAll()).Returns(stopWordsList);
			mockMapper
				.Setup(m => m.Map<IEnumerable<StopWord>, IEnumerable<StopWordViewModel>>(stopWordsList))
				.Returns(stopWordsViewModelList);

			var result = manager.GetStopWords();

			Assert.That(result, Is.TypeOf<List<StopWordViewModel>>());
		}

		[Test]
		public void Insert_ItemForInsert_ThrowsNothing()
		{
			StopWordViewModel stopWordViewModel = new StopWordViewModel();
			StopWord stopWord = new StopWord();

			mockMapper.Setup(m => m.Map<StopWordViewModel, StopWord>(stopWordViewModel)).Returns(stopWord);
			mockUnitOfWork.Setup(m => m.StopWords.Insert(stopWord));

			Assert.That(() => manager.Insert(stopWordViewModel), Throws.Nothing);
		}

		[Test]
		public void Update_ItemForUpdate_ThrowsNothing()
		{
			StopWordViewModel stopWordViewModel = new StopWordViewModel();
			StopWord stopWord = new StopWord();

			mockMapper.Setup(m => m.Map<StopWordViewModel, StopWord>(stopWordViewModel)).Returns(stopWord);
			mockUnitOfWork.Setup(m => m.StopWords.Update(stopWord));

			Assert.That(() => manager.Update(stopWordViewModel), Throws.Nothing);
		}

		[Test]
		public void Delete_ValidItem_ThrowsNothing()
		{
			StopWord testStopWord = new StopWord(){Id = 1};	
		
			mockUnitOfWork.Setup(m => m.StopWords.GetById(1)).Returns(testStopWord);
			mockUnitOfWork.Setup(m => m.StopWords.Delete(testStopWord));

			Assert.That(() => manager.Delete(1), Throws.Nothing);
		}
	}
}
