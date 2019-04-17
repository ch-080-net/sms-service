using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BAL.Exceptions
{
    public class SendingToHimselfExeption : ApplicationException
    {
        public SendingToHimselfExeption(string errorMessage) : base(errorMessage)
        {

        }

        protected SendingToHimselfExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
