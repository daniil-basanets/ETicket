using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.Charts.DTOs
{
    public class ChartDto
    {
        public IList<string> Data { get; set; }
        public IList<string> Labels { get; set; }
    }
}
