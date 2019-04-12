using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BAL.Exceptions
{
	[Serializable]
	public class InvalidSmppUserDataException : ApplicationException
	{
		public InvalidSmppUserDataException(string errorMessage) : base(errorMessage)
		{

		}

		protected InvalidSmppUserDataException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}
}
