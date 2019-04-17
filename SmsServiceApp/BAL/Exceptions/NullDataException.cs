using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BAL.Exceptions
{
    [Serializable]
    public class NullDataException : ApplicationException
    {
        public NullDataException(string errorMessage) : base(errorMessage)
        {

        }

        protected NullDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
