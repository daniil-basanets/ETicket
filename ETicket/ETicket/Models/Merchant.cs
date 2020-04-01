using ETicket.Models.Interfaces;

namespace ETicket.Models
{
    public class Merchant : IMerchant
    {
        public int MerchantId { get; set; }
        public string Password { get; set; }
    }
}