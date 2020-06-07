using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class TicketTypeDto
    {
        public int Id { get; set; }
        
        [DisplayName("Type name")]
        public string TypeName { get; set; }
        
        [DisplayName("Duration (in hours)")]
        public uint DurationHours { get; set; }
        
        [DisplayName("Personal")]
        public bool IsPersonal { get; set; }
        
        public decimal Coefficient { get; set; }
    }
}