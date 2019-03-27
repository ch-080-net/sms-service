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
        ///  Method for getting all the companies that have this phone
        /// </summary>
        /// <param phone="Destination">getting all the companies that have this phone</param>
        /// <returns>IEnumerable of mapped to ViewModel objects</returns>
        public IEnumerable<CompanyViewModel> GetCompaniesByPhoneId(int phoneId)
        {
            IEnumerable<Company> companies = unitOfWork.Companies.Get(p => p.PhoneId == phoneId);
            return mapper.Map<IEnumerable<Company>, IEnumerable<CompanyViewModel>>(companies);
        }

        public List<CompanyViewModel> GetCampaigns(int groupId, int page, int countOnPage, string searchValue)
        {
            IEnumerable<Company> Campaigns = unitOfWork.Companies.Get(ec => ec.ApplicationGroupId == groupId
                &&( ec.Name.Contains(searchValue))||(ec.Description.Contains(searchValue)))
                .Skip((page - 1) * countOnPage).Take(countOnPage);
            
            return mapper.Map<IEnumerable<Company>, List<CompanyViewModel>>(Campaigns);
        }

        public int GetCampaignsCount(int groupId, string searchValue)
        {
            return unitOfWork.Companies.Get(ec => ec.ApplicationGroupId == groupId && ec.Name.Contains(searchValue)).Count();
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
        /// Update base entity of Company
        /// </summary>
        /// <param name="item">CompanyViewModel item from view</param>
        public void Update(CompanyViewModel item)
        {
            Company company = unitOfWork.Companies.GetById(item.Id);
            company.Name = item.Name;
            company.Description = item.Description;
            if (item.TariffId > 0)
            {
                company.TariffId = item.TariffId;
            }
            else
            {
                company.TariffId = null;
            }
            unitOfWork.Companies.Update(company);
            unitOfWork.Save();
        }

        /// <summary>
        /// Find base entity of company from db
        /// and add Send info from view
        /// </summary>
        /// <param name="item">SendViewModel from send view</param>
        public void AddSend(SendViewModel item)
        {
            Company company = unitOfWork.Companies.GetById(item.Id);
            company.TariffId = item.TariffId;
            company.Message = item.Message;
            company.SendingTime = item.SendingTime;
            unitOfWork.Companies.Update(company);
            unitOfWork.Save();
        }

        /// <summary>
        /// Find base entity of company from db
        /// and add Recieve info from view
        /// </summary>
        /// <param name="item">RecieveViewModel from view</param>
        public void AddRecieve(RecieveViewModel item)
        {
            Company company = unitOfWork.Companies.GetById(item.Id);
            company.StartTime = item.StartTime;
            company.EndTime = item.EndTime;
            unitOfWork.Companies.Update(company);
            unitOfWork.Save();
        }

        /// <summary>
        /// Find base entity of company from db
        /// and add SendAndRecieve info from view
        /// </summary>
        /// <param name="item">SendRecieveViewModel from view</param>
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
        /// Get full info about company from db
        /// </summary>
        /// <param name="id">id of company</param>
        /// <returns>ManageViewModel with full info about compaign</returns>
        public ManageViewModel GetDetails(int id)
        {
            Company company = unitOfWork.Companies.GetById(id);
            return mapper.Map<Company, ManageViewModel>(company);
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

        /// <summary>
        /// Insert company entity to db and return Id
        /// </summary>
        /// <param name="item">CompanyViewModel that we isert to db</param>
        /// <returns>Id of inserted company</returns>
        public int InsertWithId(CompanyViewModel item)
        {
            Company company = mapper.Map<CompanyViewModel, Company>(item);
            company.TariffId = null;
            int id = unitOfWork.Companies.InsertWithId(company);
            return id;
        }
    }
}
