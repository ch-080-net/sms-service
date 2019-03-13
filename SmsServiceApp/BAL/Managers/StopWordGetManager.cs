using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WebCustomerApp.Models;

namespace BAL.Managers
{
   public class StopWordGetManager
    {
        public void Pars(string str)
        {
            Regex reg = new Regex(@"$Message From: '(<Originator>)', To: '(<Destination>)', Content:\w Text:'(<stopWord>)' \w$");

            foreach (Match m in reg.Matches(str))
            {
                string Originator=  m.Groups["Originator"].Value;
                string Destination = m.Groups["Destination"].Value;
                string stopWord = m.Groups["stopWord"].Value;
            }
        }
    }
}
