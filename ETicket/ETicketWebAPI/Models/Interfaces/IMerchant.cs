namespace ETicket.WebAPI.Models.Interfaces
{
    public interface IMerchant
    {
        string PublicKey { get; set; }

        string PrivateKey { get; set; }
    }
}