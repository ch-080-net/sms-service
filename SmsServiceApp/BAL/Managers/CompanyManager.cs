using AutoMapper;
using Model.Interfaces;
using Model.ViewModels.CompanyViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public class CompanyManager : BaseManager, ICompanyManager
    {
        public CompanyManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public IEnumerable<CompanyViewModel> GetCompanies(int groupId)
        {
            IEnumerable<Company> companies = unitOfWork.Companies.GetAll().Where(c => c.ApplicationGroupId == groupId);
            return mapper.Map<IEnumerable<Company>, IEnumerable<CompanyViewModel>>(companies);
        }

        public void Insert(CompanyViewModel item, int groupId)
        {
            Company company = mapper.Map<CompanyViewModel, Company>(item);
            company.ApplicationGroupId = groupId;
            company.TariffId = null;
            unitOfWork.Companies.Insert(company);
            unitOfWork.Save();
        }

		public int GetTariffLimit(int companyId)
		{
			Company comp = unitOfWork.Companies.Get(filter: c => c.Id == companyId).FirstOrDefault();
			Tariff tariff = unitOfWork.Tariffs.Get(c => c.Id == comp.TariffId).FirstOrDefault();
			return tariff.Limit;
		}

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

        public CompanyViewModel Get(int id)
        {
            Company company = unitOfWork.Companies.GetById(id);
            return mapper.Map<Company, CompanyViewModel>(company);
        }

        public void Delete(int id)
        {
            Company company = unitOfWork.Companies.GetById(id);
            unitOfWork.Companies.Delete(company);
            unitOfWork.Save();
        }
    }
}
