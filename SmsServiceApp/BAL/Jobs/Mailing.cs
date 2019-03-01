using System;
using System.Threading.Tasks;
using Quartz;
using Model.Interfaces;
using AutoMapper;

namespace BAL.Jobs
{

    public class Mailing : IJob, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public Mailing(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    unitOfWork.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
