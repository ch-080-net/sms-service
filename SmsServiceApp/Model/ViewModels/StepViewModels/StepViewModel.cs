using Model.ViewModels.CompanyViewModels;
using Model.ViewModels.OperatorViewModels;
using Model.ViewModels.RecipientViewModels;
using Model.ViewModels.TariffViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace Model.ViewModels.StepViewModels
{
    public class StepViewModel
    {
        public CompanyViewModel CompanyModel { get; set; }
        public OperatorsViewModel OperatorModel { get; set; }
        public TariffsViewModel TariffModel { get; set; }
        public TariffViewModel TariffModels { get; set; }
        public RecieveViewModel RecieveModel { get; set; }
        public RecipientViewModel RecipientModel { get; set; }
        
        public SendRecieveViewModel SendRecieveModel { get; set; }

    }
}
