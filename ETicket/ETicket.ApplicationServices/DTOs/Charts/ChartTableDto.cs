using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.DTOs.Charts
{
    public class ChartTableDto : BaseChartDto
    {
        public string[,] Data { get; set; }
        public int MaxPassengersByRoute { get; set; }
    }
}
