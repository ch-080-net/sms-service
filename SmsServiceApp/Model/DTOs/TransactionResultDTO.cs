using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public class TransactionResultDTO
    {
        public bool Success { get; set; }

        public string Details { get; set; }

        public object AdditionalInfo { get; set; }
    }
}
