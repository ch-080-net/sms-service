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

        public IEnumerable<CompanyViewModel> GetCompanies()
        {
            IEnumerable<Company> companies = unitOfWork.Companies.GetAll();
            return mapper.Map<IEnumerable<Company>, IEnumerable<CompanyViewModel>>(companies);
        }

        public void Insert(CompanyViewModel item)
        {
            Company company = mapper.Map<CompanyViewModel, Company>(item);
            unitOfWork.Companies.Insert(company);
            unitOfWork.Save();
        }

        public void Update(CompanyViewModel item)
        {
            Company company = mapper.Map<CompanyViewModel, Company>(item);
            unitOfWork.Companies.Update(company);
            unitOfWork.Save();
        }

        public void Delete(CompanyViewModel item)
        {
            Company company = mapper.Map<CompanyViewModel, Company>(item);
            unitOfWork.Companies.Delete(company);
            unitOfWork.Save();
        }


        public void SetStateModified(CompanyViewModel item)
        {
            Company company = mapper.Map<CompanyViewModel, Company>(item);
            unitOfWork.Companies.SetStateModified(company);
            unitOfWork.Save();
        }
    }
}
