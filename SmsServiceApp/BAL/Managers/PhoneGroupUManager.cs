using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Model.Interfaces;
using Model.ViewModels.CompanyViewModels;
using WebCustomerApp.Models;

namespace BAL.Managers
{
    public class PhoneGroupUManager : BaseManager 
    {
        private readonly ICompanyManager companyManager;
        private readonly IPhoneManager phoneManager;


        public PhoneGroupUManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            companyManager = new CompanyManager(unitOfWork, mapper);
            phoneManager = new PhoneManager(unitOfWork, mapper);
        
        }

        void AddGroupPhone(string OPhone, string DGroup)
        {
            var orignator = phoneManager.GetPhoneId(OPhone);
            var destination = phoneManager.GetPhoneId(DGroup);
            IEnumerable<CompanyViewModel> companies = companyManager.GetCompaniesByPhoneId(destination);
            if (companies != null)//add check for repetition
            {
                foreach (var company in companies)
                {
                    PhoneGroupUnsubscribe phoneGroup = new PhoneGroupUnsubscribe() { GroupId = company.ApplicationGroupId , PhoneId=orignator};
                    unitOfWork.PhoneGroupUnsubscribes.Insert(phoneGroup);
                    unitOfWork.Save();
                }
            }
        }
    }
}
