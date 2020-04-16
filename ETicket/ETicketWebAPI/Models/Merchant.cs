using ETicketWebAPI.Models.Interfaces;

namespace ETicketWebAPI.Models
{
    public class Merchant : IMerchant
    {
        public int MerchantId { get; set; }
        public string Password { get; set; }
    }
}