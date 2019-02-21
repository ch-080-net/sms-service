using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Managers
{
   public interface IStopWordManager
    {
        IEnumerable<StopWord> GetStopWords();
    }
}
