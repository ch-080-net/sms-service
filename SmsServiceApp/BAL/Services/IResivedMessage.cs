using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Services
{
   public interface IResivedMessage
    {
        void SearchStopWordInMeaasge(string Originator, string Destination, string Content);
    }
}
