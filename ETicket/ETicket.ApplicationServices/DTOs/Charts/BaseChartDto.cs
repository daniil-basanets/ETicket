using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.Charts.DTOs
{
    public abstract class BaseChartDto
    {
        public IList<string> Labels { get; set; }
        public string ErrorMessage { get; set; }
    }
}
