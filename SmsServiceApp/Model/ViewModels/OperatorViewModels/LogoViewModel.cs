using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.OperatorViewModels
{
    public class LogoViewModel
    {
        public int OperatorId { get; set; }

        public IFormFile Logo { get; set; }
    }
}
