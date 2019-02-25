using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.TariffViewModels
{
   public class TariffViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int OperatorId { get; set; }
        public int Limit { get; set; }

    }
}
