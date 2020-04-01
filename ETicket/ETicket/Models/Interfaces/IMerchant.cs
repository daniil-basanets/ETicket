namespace ETicket.Models.Interfaces
{
    public interface IMerchant
    {
        int MerchantId { get; set; }
        string Password { get; set; }
    }
}