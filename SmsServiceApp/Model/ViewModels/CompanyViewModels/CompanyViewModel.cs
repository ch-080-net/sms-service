using Model.ViewModels.RecipientViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.CompanyViewModels
{
    class CompanyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ApplicationUserId { get; set; }
        public int TariffId { get; set; }
        public string Message { get; set; }
        public List<RecipientViewModel> RecipientViewModels { get; set; }
    }
}
