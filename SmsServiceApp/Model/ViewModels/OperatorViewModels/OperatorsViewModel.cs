using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.OperatorViewModels
{
	public class OperatorsViewModel
	{
		[Display(Name = "Operator")]
		public IEnumerable<OperatorViewModel> OperatorsList { get; set; }
	}
}
