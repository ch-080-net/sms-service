using AutoMapper;
using Model.Interfaces;
using Model.ViewModels.CompanyViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    /// <summary>
    /// Manager for Companies, include all methods needed to work with Company storage.
    /// Inherited from BaseManager and have additional methods.
    /// </summary>
    public class CompanyManager : BaseManager, ICompanyManager
    {
        public CompanyManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <summary>
        /// Method for getting all companies which belong to specified group
        /// </summary>
        /// <param name="groupId">Takes Id of group wich belongs need companies</param>
        /// <returns>IEnumerable of mapped to ViewModel objects</returns>
        public IEnumerable<CompanyViewModel> GetCompanies(int groupId)
        {
            IEnumerable<Company> companies = unitOfWork.Companies.GetAll().Where(c => c.ApplicationGroupId == groupId);
            return mapper.Map<IEnumerable<Company>, IEnumerable<CompanyViewModel>>(companies);
        }

        /// <summary>
        /// Method for inserting new company to db
        /// </summary>
        /// <param name="item">ViewModel of Company</param>
        /// <param name="groupId">Id of Group wich create this company</param>
        public void Insert(CompanyViewModel item)
        {
            Company company = mapper.Map<CompanyViewModel, Company>(item);
            unitOfWork.Companies.Insert(company);
            unitOfWork.Save();
        }

        /// <summary>
        /// Gets limit of recipients according to the chosen tariff
        /// </summary>
        /// <param name="companyId">Id of company</param>
        /// <returns>limit of recipients amount in this company</returns>
		public int GetTariffLimit(int companyId)
		{
			Company comp = unitOfWork.Companies.Get(filter: c => c.Id == companyId).FirstOrDefault();
			Tariff tariff = unitOfWork.Tariffs.Get(c => c.Id == comp.TariffId).FirstOrDefault();
			return tariff.Limit;
		}

        /// <summary>
        /// Update Company in db
        /// </summary>
        /// <param name="item">ViewModel wich need to update in db</param>
        /// <param name="groupId">Id of group which belongs this company</param>
        /// <param name="tariffId">Id of chosen tariff to this company</param>
        public void Update(CompanyViewModel item, int groupId, int tariffId)
        {
            Company company = mapper.Map<CompanyViewModel, Company>(item);
            company.ApplicationGroupId = groupId;
            if (tariffId != 0)
            {
                company.TariffId = tariffId;
            }
            else
            {
                company.TariffId = null;
            }
            unitOfWork.Companies.Update(company);
            unitOfWork.Save();
        }

        public void AddSend(SendViewModel item)
        {
            Company company = unitOfWork.Companies.GetById(item.Id);
            company.TariffId = item.TariffId;
            company.Message = item.Message;
            company.SendingTime = item.SendingTime;
            unitOfWork.Companies.Update(company);
            unitOfWork.Save();
        }

        public void AddRecieve(RecieveViewModel item)
        {
            Company company = unitOfWork.Companies.GetById(item.Id);
            company.StartTime = item.StartTime;
            company.EndTime = item.EndTime;
            unitOfWork.Companies.Update(company);
            unitOfWork.Save();
        }

        public void AddSendRecieve(SendRecieveViewModel item)
        {
            Company company = unitOfWork.Companies.GetById(item.Id);
            company.TariffId = item.TariffId;
            company.Message = item.Message;
            company.SendingTime = item.SendingTime;
            company.StartTime = item.StartTime;
            company.EndTime = item.EndTime;
            unitOfWork.Companies.Update(company);
            unitOfWork.Save();
        }

        /// <summary>
        /// Get one company from db by Id
        /// </summary>
        /// <param name="id">Id of company wich you need</param>
        /// <returns>ViewModel of company from db</returns>
        public CompanyViewModel Get(int id)
        {
            Company company = unitOfWork.Companies.GetById(id);
            return mapper.Map<Company, CompanyViewModel>(company);
        }

        /// <summary>
        /// Delete company by Id
        /// </summary>
        /// <param name="id">Id of company wich need to delete</param>
        public void Delete(int id)
        {
            Company company = unitOfWork.Companies.GetById(id);
            unitOfWork.Companies.Delete(company);
            unitOfWork.Save();
        }

        public int InsertWithId(CompanyViewModel item)
        {
            Company company = mapper.Map<CompanyViewModel, Company>(item);
            company.TariffId = null;
            int id = unitOfWork.Companies.InsertWithId(company);
            return id;
        }
    }
}
