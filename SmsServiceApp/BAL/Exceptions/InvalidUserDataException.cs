using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Exceptions
{
	public class InvalidSMPPUserDataException : Exception
	{
		public InvalidSMPPUserDataException(string errorMessage) : base (errorMessage)
		{

		}
	}
}
