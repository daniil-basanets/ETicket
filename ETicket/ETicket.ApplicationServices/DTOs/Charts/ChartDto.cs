using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.Charts.DTOs
{
    public class ChartDto
    {
        IList<string> Data { get; set; }
        IList<string> Labels { get; set; }
    }
}
