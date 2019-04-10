using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace Model.Interfaces
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        int InsertWithId(Company item);
        int InsertRecieveCampaign(Company item);
    }
}
