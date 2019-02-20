using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.DB
{
    public class Recipient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Comment { get; set; }
        //public string Comment2 { get; set; }
    }
}
