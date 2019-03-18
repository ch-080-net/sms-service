﻿using Model.ViewModels.CompanyViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace BAL.Managers
{
    /// <summary>
    /// Interface with CRUD operation for Companies
    /// </summary>
    public interface ICompanyManager
    {
        CompanyViewModel Get(int id);
        IEnumerable<CompanyViewModel> GetCompanies(int groupId);
		int GetTariffLimit(int companyId);
        int InsertWithId(CompanyViewModel item);
		void Insert(CompanyViewModel item);
		void Update(CompanyViewModel item, int groupId, int tariffId);
		void Delete(int id);
        void AddSend(SendViewModel item);
        void AddRecieve(RecieveViewModel item);
        void AddSendRecieve(SendRecieveViewModel item);
    }
}
