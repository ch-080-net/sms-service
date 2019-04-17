using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;
using Model.Interfaces;
using Model.ViewModels.OperatorViewModels;
using AutoMapper;
using System.Linq;
using System.IO;
using Model.DTOs;
using System.Drawing;
using System.Drawing.Imaging;
using BAL.Wrappers;

namespace BAL.Managers
{
    /// <summary>
    /// Manger for CRUD operations on Operators
    /// </summary>
    public class OperatorManager : BaseManager, IOperatorManager
    {
        IFileIoWrapper fileIo;
        public OperatorManager(IUnitOfWork unitOfWork, IMapper mapper, IFileIoWrapper fileIo) : base(unitOfWork, mapper)
        {
            this.fileIo = fileIo;
        }

        /// <summary>
        /// Get all operators from DB and transform them to OperatorViewModel
        /// </summary>
        public IEnumerable<OperatorViewModel> GetAll()
        {
            var operators = unitOfWork.Operators.GetAll();

            var result = mapper.Map<IEnumerable<Operator>, IEnumerable<OperatorViewModel>>(operators);
            return result;
        }

        /// <summary>
        /// Transform OperatorViewModel <paramref name="newOperator"/> to Code and insert it to Codes table of DB
        /// </summary>
        /// <param name="newOperator">
        /// Should contain not null or empty Name.
        /// Name must be unique
        /// </param>
        /// <returns>Success, if transaction succesfull; !Success if not, Details contains error message if any</returns>
        public TransactionResultDTO Add(OperatorViewModel newOperator)
        {
            if (string.IsNullOrEmpty(newOperator.Name))
				return new TransactionResultDTO() { Success = false, Details = "Operator name cannot be empty"};
            var check = unitOfWork.Operators.Get(o => o.Name == newOperator.Name).FirstOrDefault();
            if (check != null)
                return new TransactionResultDTO() { Success = false, Details = "Operator with this name already exist" };
            var result = mapper.Map<Operator>(newOperator);
            try
            {
                unitOfWork.Operators.Insert(result);
                unitOfWork.Save();
            }
            catch
            {
                return new TransactionResultDTO() { Success = false, Details = "Internal error" };
            }
            return new TransactionResultDTO() { Success = true };
        }

        /// <summary>
        /// Get OperatorViewModel by Id
        /// </summary>
        /// <param name="id">Id of Operator in Operators table</param>
        /// <returns>Provides empty OperatorViewModel if provided null</returns>
        public OperatorViewModel GetById(int id)
        {
            var oper = unitOfWork.Operators.GetById(id);
            return mapper.Map<OperatorViewModel>(oper);
        }

		public OperatorViewModel GetByName (string name)
		{
			var oper = unitOfWork.Operators.Get(o => o.Name == name);
			return mapper.Map<OperatorViewModel>(oper.FirstOrDefault());
		}

        /// <summary>
        /// Remove entry from Operators table with corresponding <paramref name="id"/>
        /// Also remove Logo from OperatorLogo folder
        /// </summary>
        /// <returns>Success, if transaction succesfull; !Success if not, Details contains error message if any</returns>
        public TransactionResultDTO Remove(int id)
        {
            var oper = unitOfWork.Operators.GetById(id);
            if (oper == null)
                return new TransactionResultDTO() { Success = false, Details = "Operator already removed" };
            else if (oper.Tariffs.Any())
                return new TransactionResultDTO() { Success = false, Details = "Operator cannot be removed when he have tariffs" };

            string logoPath = "wwwroot/images/OperatorLogo/Logo_Id=" + Convert.ToString(id) + ".png";
            if (fileIo.FileExists(logoPath))
            {
                try
                {
                    fileIo.FileDelete(logoPath);
                }
                catch
                {
                    return new TransactionResultDTO() { Success = false, Details = "Can`t find operator logo" };
                }
            }

            try
            {
                unitOfWork.Operators.Delete(oper);                
                unitOfWork.Save();
            }
            catch
            {
                return new TransactionResultDTO() { Success = false, Details = "Operator delete error" };
            }
            return new TransactionResultDTO() { Success = true };
        }

        /// <summary>
        /// Transform OperatorViewModel <paramref name="updatedOperator"/> to Operator and update corresponding row in Operators table of DB
        /// </summary>
        /// <param name="updatedOperator">
        /// Should contain not null or empty Name and Id of existing Operators table entry.
        /// Name must be unique
        /// </param>
        /// <returns>Success, if transaction succesfull; !Success if not, Details contains error message if any</returns>
        public TransactionResultDTO Update(OperatorViewModel updatedOperator)
        {
            if (string.IsNullOrEmpty(updatedOperator.Name))
                return new TransactionResultDTO() { Success = false, Details = "Operator name cannot be empty" };
            var check = unitOfWork.Operators.Get(o => o.Name == updatedOperator.Name).FirstOrDefault();
            if (check != null)
                return new TransactionResultDTO() { Success = false, Details = "Operator name already exist" };
            var result = mapper.Map<Operator>(updatedOperator);
            try
            {
                unitOfWork.Operators.Update(result);
                unitOfWork.Save();
            }
            catch
            {
                return new TransactionResultDTO() { Success = false, Details = "Internal error" };
            }
            return new TransactionResultDTO() { Success = true };
        }

        /// <summary>
        /// Get Page, which corresponds to <paramref name="pageState"/> 
        /// </summary>
        /// <param name="pageState">Represents page state, including search querry, page, number of pages, entries on one page</param>
        /// <returns>Object, which contains valid page state and corresponding enumeration of OperatorViewModel</returns>
        public Page GetPage(PageState pageState)
        {
            if (pageState == null)
                return null;

            if (pageState.SearchQuerry == null)
                pageState.SearchQuerry = "";

            var operators = unitOfWork.Operators.Get(o => o.Name.Contains(pageState.SearchQuerry));

            if (pageState.OperatorsOnPage < 1)
                pageState.OperatorsOnPage = 10;

            pageState.LastPage = operators.Count() / pageState.OperatorsOnPage
                + (((operators.Count() % pageState.OperatorsOnPage) == 0) ? 0 : 1);

            if (pageState.Page > pageState.LastPage)
                pageState.Page = pageState.LastPage;

            if (pageState.Page < 1)
                pageState.Page = 1;

            var result = new Page();

            result.OperatorList = mapper.Map<IEnumerable<Operator>, IEnumerable<OperatorViewModel>>
                (operators.Skip((pageState.Page - 1) * pageState.OperatorsOnPage).Take(pageState.OperatorsOnPage));

            result.PageState = pageState;

            return result;
        }


        /// <summary>
        /// Resize logo and write to .png file
        /// </summary>
        /// <param name="logo"> Should contain not 0 OperatorId and not null Logo</param>
        /// <returns>Success, if transaction succesfull; !Success if not, Details contains error message if any</returns>
        public TransactionResultDTO AddLogo(LogoViewModel logo)
        {
            if (logo.Logo == null)
                return new TransactionResultDTO() { Success = false, Details = "No logo sent" };

            if (logo.OperatorId == 0)
                return new TransactionResultDTO() { Success = false, Details = "Empty operator id" };

            // Create bitmap
            var stream = logo.Logo.OpenReadStream();
            Bitmap image;
            try
            {
                image = new Bitmap(stream);
                image = new Bitmap(image, 43, 43);
                // .setResolution() doesnt work. Bug, possibly
            }
            catch(ArgumentException)
            {
                return new TransactionResultDTO() { Success = false, Details = "Image data corrupted" };
            }
            catch(Exception)
            {
                return new TransactionResultDTO() { Success = false, Details = "Image can't be resized" };
            }

            if(!fileIo.Exists("wwwroot/images/OperatorLogo/"))
            {
                try
                {
                    fileIo.CreateDirectory("wwwroot/images/OperatorLogo/");
                }
                catch (Exception)
                {
                    return new TransactionResultDTO() { Success = false, Details = "Can't create directory for logos" };
                }
            }

            try
            {
                fileIo.SaveBitmap(image, "wwwroot/images/OperatorLogo/Logo_Id=" + Convert.ToString(logo.OperatorId) + ".png"
                    , ImageFormat.Png);
            }
            catch(ArgumentNullException)
            {
                return new TransactionResultDTO() { Success = false, Details = "Internal error" };
            }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                return new TransactionResultDTO() { Success = false, Details = "Saving destination cannot be reached" };
            }
            catch (Exception)
            {
                return new TransactionResultDTO() { Success = false, Details = "Internal error" };
            }

            return new TransactionResultDTO() { Success = true };
        }
    }
}