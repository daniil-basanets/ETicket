using ETicket.WebAPI.Models.Interfaces;

namespace ETicket.WebAPI.Models
{
    public class Merchant : IMerchant
    {
        public int MerchantId { get; set; }
        public string Password { get; set; }
    }
}