using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.DTOs.Charts
{
    public class MultiLineChartDto : BaseChartDto
    {
        public IList<string> LineLable { get; set; }
        public string[,] Data { get; set; }
    }
}
