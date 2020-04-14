using ETicket.Models.Interfaces;

namespace ETicket.Models
{
    public class MerchantSettings : IMerchantSettings
    {
        public string CardNumber { get ; set; }
    }
}