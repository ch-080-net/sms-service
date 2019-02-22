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
        IBaseRepository<Operator> Operators { get; }
        IBaseRepository<Tariff> Tariffs { get; }
        UserManager<ApplicationUser> Users { get; }
        int Save();
    }

}
