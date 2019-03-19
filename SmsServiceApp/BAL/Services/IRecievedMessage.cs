using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Services
{
   public interface IRecievedMessage
    {
        void SearchStopWordInMeaasge(string Originator, string Destination, string Content);
    }
}
