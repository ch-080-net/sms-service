﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Model.ViewModels.ManageViewModels
{
    public class NotificationsViewModel
    {
        public bool EmailNotEnabled { get; set; }
        public bool SmsNotEnabled { get; set; }
    }
}
