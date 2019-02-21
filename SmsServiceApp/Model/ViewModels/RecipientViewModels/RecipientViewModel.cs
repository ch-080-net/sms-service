using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.RecipientViewModels
{
    class RecipientViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int PhoneId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte Gender { get; set; }
        public string Priority { get; set; }
        public string KeyWords { get; set; }
    }
}
