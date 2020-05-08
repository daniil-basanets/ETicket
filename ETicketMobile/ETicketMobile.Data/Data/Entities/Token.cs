namespace ETicketMobile.Data.Domain.Entities
{
    public class Token
    {
        public string AcessJwtToken { get; set; }

        public string RefreshJwtToken { get; set; }
    }
}