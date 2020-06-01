using System.Collections.Generic;

namespace ETicket.ApplicationServices.Charts.DTOs
{
    public class ChartDto : BaseChartDto
    {
        public IList<string> Data { get; set; }
    }
}