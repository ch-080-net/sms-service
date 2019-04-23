using AutoMapper;
using Model.Interfaces;
using Model.ViewModels.CompanyViewModels;
using Model.ViewModels.RecipientViewModels;
using Model.ViewModels.StepViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Models;
using BAL.Notifications;
using BAL.Notifications.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BAL.Managers
{
    /// <summary>
    /// Manager for Companies, include all methods needed to work with Company storage.
    /// Inherited from BaseManager and have additional methods.
    /// </summary>
    public class CompanyManager : BaseManager, ICompanyManager
    {
        private readonly INotificationsGenerator<Company> notificationsGenerator;
        public CompanyManager(IUnitOfWork unitOfWork, IMapper mapper
            , INotificationsGenerator<Company> notificationsGenerator) : base(unitOfWork, mapper)
        {
            this.notificationsGenerator = notificationsGenerator;
        }

        /// <summary>
        /// Method for getting all companies which belong to specified group
        /// </summary>
        /// <param name="groupId">Takes Id of group wich belongs need companies</param>
        /// <returns>IEnumerable of mapped to ViewModel objects</returns>
        public IEnumerable<CompanyViewModel> GetCompanies(int groupId)
        {
            IEnumerable<Company> companies = unitOfWork.Companies.Get(c => c.ApplicationGroupId == groupId);
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
            IEnumerable<Company> Campaigns = unitOfWork.Companies.Get(c => (c.ApplicationGroupId == groupId)
                &&( c.Name.Contains(searchValue)||c.Description.Contains(searchValue)))
                .Skip((page - 1) * countOnPage).Take(countOnPage);
            
            return mapper.Map<IEnumerable<Company>, List<CompanyViewModel>>(Campaigns);
        }

        public int GetCampaignsCount(int groupId, string searchValue)
        {
            return unitOfWork.Companies.Get(ec =>( ec.ApplicationGroupId == groupId) && ec.Name.Contains(searchValue)).Count();
        }

        /// <summary>
        /// Method for inserting new company to db
        /// </summary>
        /// <param name="item">ViewModel of Company</param>
        /// <param name="groupId">Id of Group wich create this company</param>
        public bool Insert(CompanyViewModel item)
        {
                Company company = mapper.Map<CompanyViewModel, Company>(item);
                try
                {
                    unitOfWork.Companies.Insert(company);
                    notificationsGenerator.SupplyWithCampaignNotifications(company);
                    unitOfWork.Save();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;


        }

        /// <summary>
        /// Gets limit of recipients according to the chosen tariff
        /// </summary>
        /// <param name="companyId">Id of company</param>
        /// <returns>limit of recipients amount in this company</returns>
		public int GetTariffLimit(int companyId)
		{
			Company comp = unitOfWork.Companies.Get(filter: c => c.Id == companyId).FirstOrDefault();
            Tariff tariff = unitOfWork.Tariffs.GetById((int)comp.TariffId);
			return tariff.Limit;
		}

        /// <summary>
        /// Update base entity of Company
        /// </summary>
        /// <param name="item">CompanyViewModel item from view</param>
        public bool Update(CompanyViewModel item)
        {
                Company company = unitOfWork.Companies.GetById(item.Id);
               
                if (item.TariffId > 0)
                {
                    company.TariffId = item.TariffId;
                }
                else
                {
                    company.TariffId = null;
                }

                try
                {
                    unitOfWork.Companies.Update(company);
                    unitOfWork.Save();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;

        }

        public bool CreateWithRecipient(ManageViewModel item, List<RecipientViewModel> recipientList)
        {
            try { 
            Company company = mapper.Map<ManageViewModel, Company>(item);
           
            Phone phone = unitOfWork.Phones.Get(filter: e => e.PhoneNumber == item.PhoneNumber).FirstOrDefault();
            if (phone == null)
            {
                phone = new Phone();
                phone.PhoneNumber = item.PhoneNumber;
                unitOfWork.Phones.Insert(phone);
                company.Phone = phone;
            }
            else
            {
                company.PhoneId = phone.Id;
            }
            if (company.TariffId == 0)
                company.TariffId = null;

            unitOfWork.Companies.Insert(company);
            notificationsGenerator.SupplyWithCampaignNotifications(company);
            unitOfWork.Save();

            SubscribeWord subWord = unitOfWork.SubscribeWords.Get(sw => sw.Word == "start").FirstOrDefault();

            if (subWord != null)
                unitOfWork.CompanySubscribeWords.Insert(new CompanySubscribeWord()
                {
                    Company = company,
                    SubscribeWord = subWord
                });

            foreach (var recipient in recipientList)
            {
                Recipient newRecepient = mapper.Map<RecipientViewModel, Recipient>(recipient);
                newRecepient.CompanyId = company.Id;
                phone = unitOfWork.Phones.Get(filter: e => e.PhoneNumber == recipient.Phonenumber).FirstOrDefault();
                if (phone == null)
                {
                    phone = new Phone();
                    phone.PhoneNumber = recipient.Phonenumber;
                    unitOfWork.Phones.Insert(phone);
                    newRecepient.Phone = phone;
                }
                else
                {
                    newRecepient.PhoneId = phone.Id;
                }
                unitOfWork.Recipients.Insert(newRecepient);
                unitOfWork.Save();
            }

            return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        public bool CreateCampaignCopy(ManageViewModel item)
        {
            try { 
            Company company = mapper.Map<ManageViewModel, Company>(item);
            company.Id = 0;
            company.ApplicationGroupId = item.ApplicationGroupId;
            Phone phone = unitOfWork.Phones.Get(filter: e => e.PhoneNumber == item.PhoneNumber).FirstOrDefault();
            if (phone == null)
            {
                phone = new Phone();
                phone.PhoneNumber = item.PhoneNumber;
                unitOfWork.Phones.Insert(phone);
                company.Phone = phone;
            }
            else
            {
                company.PhoneId = phone.Id;
            }

            if (company.TariffId == 0)
                company.TariffId = null;

            unitOfWork.Companies.Insert(company);
            notificationsGenerator.SupplyWithCampaignNotifications(company);
            unitOfWork.Save();
            
            return true;
            }

            catch (Exception)
            {
            return false;
            }

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
        public bool Delete(int id)
        {
            Company company = unitOfWork.Companies.GetById(id);
            if (company == null)
            {
                return false;
            }

           unitOfWork.Companies.Delete(company);
           unitOfWork.Save();
            return true;

        }

            
    }
}
