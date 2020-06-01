using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.DTOs.Charts
{
    public abstract class BaseChartDto
    {
        public IList<string> Labels { get; set; }
        public string ErrorMessage { get; set; }
    }
}
