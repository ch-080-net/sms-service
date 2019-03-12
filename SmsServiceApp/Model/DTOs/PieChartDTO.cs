using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Model.DTOs
{
    public class PieChartDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Tuple<string, int>> Categories { get; set; }
    }
}
