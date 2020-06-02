using System.Collections.Generic;

namespace ETicket.ApplicationServices.DTOs.Charts
{
    public class ChartDto : BaseChartDto
    {
        public IList<string> Data { get; set; }
    }
}