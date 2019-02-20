using Model.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Recipient> Recipients { get; }
        int Save();
    }
}
