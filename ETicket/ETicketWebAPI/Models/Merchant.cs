using ETicket.WebAPI.Models.Interfaces;

namespace ETicket.WebAPI.Models
{
    public class Merchant : IMerchant
    {
        public string PublicKey { get; set; }

        public string PrivateKey { get; set; }
    }
}