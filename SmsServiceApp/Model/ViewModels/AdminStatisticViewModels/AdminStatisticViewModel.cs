using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.AdminStatisticViewModel
{
    public class AdminStatisticViewModel
    {
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
        [Display(Name = "Count")]
        public int Count { get; set; }

    }
    
}
