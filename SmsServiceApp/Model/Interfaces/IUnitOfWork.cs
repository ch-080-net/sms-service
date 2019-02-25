using WebCustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Model.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Recipient> Recipients { get; }
        IContactRepository Contacts { get; }
        IBaseRepository<Phone> Phones { get; }
        IBaseRepository<Company> Companies { get; }
        UserManager<ApplicationUser> Users { get; }
        IBaseRepository<Operator> Operators { get; }
        IBaseRepository<Code> Codes { get; }
        IBaseRepository<Tariff> Tariffs { get; }
        int Save();
    }

}
