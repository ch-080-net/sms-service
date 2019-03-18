using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.TariffViewModels
{
	public class TariffsViewModel
	{
		[Display(Name = "Tariff")]
		public IEnumerable<TariffViewModel> TariffsList { get; set; }
	}
}
