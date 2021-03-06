﻿using Model.ViewModels.RecipientViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebApp.Models;

namespace Model.ViewModels.CompanyViewModels
{
    /// <summary>
    /// View model with full info about company 
    /// </summary>
    public class ManageViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Tariff")]
        public string Tariff { get; set; }
        public int ApplicationGroupId { get; set; }
        public int TariffId { get; set; }
        [StringLength(500)]
        [Display(Name = "Message")]
        public string Message { get; set; }
        [Display(Name = "Time for send")]
        [DataType(DataType.DateTime)]
        public DateTime SendingTime { get; set; }
        [Display(Name = "Recipients")]
        public IEnumerable<RecipientViewModel> RecipientViewModels { get; set; }
        [Display(Name = "Start time")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }
        [Display(Name = "End time")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }
        [StringLength(100)]
		[Display(Name = "Name")]
        public string Name { get; set; }
        [StringLength(500)]
        [Display(Name="Description")]
        public string Description { get; set; }
        [Display(Name = "Type of compaign")]
        public CompanyType Type { get; set; }
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        public int PhoneId { get; set; }
       
    }
}
