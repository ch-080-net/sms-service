using AutoMapper;
using Model.Interfaces;
using Model.ViewModels.TariffViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    /// <summary>
    /// Manager for Tariffs, include all methods needed to work with Tariff storage.
    /// Inherited from BaseManager and have additional methods.
    /// </summary>
    public class TariffManager : BaseManager, ITariffManager
    {
        public TariffManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <summary>
        /// Get Tariff
        /// </summary>
        /// <param name="operatorId">Get operator id</param>
        /// <returns>Get Tariff from Operators</returns>
        public IEnumerable<TariffViewModel> GetTariffs(int operatorId)
        {
            IEnumerable<Tariff> tariffs = unitOfWork.Tariffs.Get(op => op.OperatorId == operatorId);

            return mapper.Map<IEnumerable<Tariff>, IEnumerable<TariffViewModel>>(tariffs);
        }

        /// <summary>
        /// Get Tariff by Tariff Id
        /// </summary>
        /// <param name="id">Tariff Id</param>
        /// <returns>Get Tariff by Id</returns>
        public TariffViewModel GetTariffById(int id)
        {
            Tariff tariff = unitOfWork.Tariffs.GetById(id);
            return mapper.Map<Tariff, TariffViewModel>(tariff);
        }
        /// <summary>
        /// Get All Tariffs
        /// </summary>
        /// <returns>All Tariffs</returns>
        IEnumerable<TariffViewModel> ITariffManager.GetAll()
        {
            IEnumerable<Tariff> tariffs = unitOfWork.Tariffs.GetAll();
            return mapper.Map<IEnumerable<Tariff>, IEnumerable<TariffViewModel>>(tariffs);
        }


        /// <summary>
        /// Insert New Tariff
        /// </summary>
        /// <param name="item">ViewModel wich need to update in db</param>
        /// <returns>Success, if transaction succesfull; !Success if not, Details contains error message if any</returns>
        public bool Insert(TariffViewModel item)
        {
            var check = unitOfWork.Tariffs.Get(o => o.Id == item.Id).FirstOrDefault();
            if (check != null)
                return false;
            var result = mapper.Map<Tariff>(item);
            try
            {
                unitOfWork.Tariffs.Insert(result);
                unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Update exist Tariff
        /// </summary>
        /// <param name="item">ViewModel wich need to update in db</param>
        /// <returns>Success, if transaction succesfull; !Success if not, Details contains error message if any</returns>
        public bool Update(TariffViewModel item)
        {
            var result = mapper.Map<Tariff>(item);
            try
            {
                unitOfWork.Tariffs.Update(result);
                unitOfWork.Save();
            }
            catch
            {
                return false;
            }
         
            return true;
        }

        /// <summary>
        /// Delete exist Tariff
        /// </summary>
        /// <param name="item">ViewModel wich need to update in db</param>
        /// <param name="id">Tariff Id</param>
        /// <returns></returns>
        public bool Delete(TariffViewModel item, int id)
        {
            var tar = unitOfWork.Tariffs.GetById(id);
            if (tar == null)
                return false;
            try
            {
                unitOfWork.Tariffs.Delete(tar);
                unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Get Tariff By Id
        /// </summary>
        /// <param name="Id">Tariff Id</param>
        /// <returns>Selected Tariff</returns>        
        public TariffViewModel GetById(int Id)
        {
            var tar = unitOfWork.Tariffs.GetById(Id);
            return mapper.Map<TariffViewModel>(tar);
        }

       
    }
}
