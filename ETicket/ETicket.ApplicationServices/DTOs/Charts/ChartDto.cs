using System.Collections.Generic;

namespace ETicket.ApplicationServices.Charts.DTOs
{
    public class ChartDto
    {
        public IList<string> Data { get; set; }
        public IList<string> Labels { get; set; }
        public string ErrorMessage { get; set; }
    }

    
    public class ChartTableDto
    {
        public string[,] Data { get; set; }
        public int MaxPassengersByRoute { get; set; }
        public IList<string> Labels { get; set; }
        public string ErrorMessage { get; set; }
    }
}
