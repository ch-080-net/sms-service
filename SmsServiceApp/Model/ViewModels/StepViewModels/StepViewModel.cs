using Model.ViewModels.CompanyViewModels;
using Model.ViewModels.OperatorViewModels;
using Model.ViewModels.TariffViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.StepViewModels
{
    public class StepViewModel
    {
        public CompanyViewModel CompanyModel { get; set; }
        public OperatorsViewModel OperatorModel { get; set; }
        public TariffsViewModel TariffModel { get; set; }
        public TariffViewModel TariffModels { get; set; }
    }
}
