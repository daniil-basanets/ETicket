namespace ETicket.ApplicationServices.DTOs
{
    public class TicketTypeDto
    {
        public int Id { get; set; }
        
        public string TypeName { get; set; }
        
        public uint DurationHours { get; set; }
        
        public bool IsPersonal { get; set; }
        
        public decimal Coefficient { get; set; }
    }
}