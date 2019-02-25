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

        public IEnumerable<CompanyViewModel> GetCompanies(string userId)
        {
            IEnumerable<Company> companies = unitOfWork.Companies.GetAll().Where(c => c.ApplicationUserId == userId);
            return mapper.Map<IEnumerable<Company>, IEnumerable<CompanyViewModel>>(companies);
        }

        public void Insert(CompanyViewModel item, string userId)
        {
            Company company = mapper.Map<CompanyViewModel, Company>(item);
            company.ApplicationUserId = userId;
            company.TariffId = null;
            unitOfWork.Companies.Insert(company);
            unitOfWork.Save();
        }

        public void Update(CompanyViewModel item, string userId, int tariffId)
        {
            Company company = mapper.Map<CompanyViewModel, Company>(item);
            company.ApplicationUserId = userId;
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
