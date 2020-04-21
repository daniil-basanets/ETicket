using ETicket.WebAPI.Models.Interfaces;

namespace ETicket.WebAPI.Models
{
    public class MerchantSettings : IMerchantSettings
    {
        public string CardNumber { get ; set; }
    }
}