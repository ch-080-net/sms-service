using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.ContactViewModels
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Gender { get; set; }
        public string Priority { get; set; }
        public string KeyWords { get; set; }

    }
}
