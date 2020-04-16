using ETicketWebAPI.Models.Interfaces;

namespace ETicketWebAPI.Models
{
    public class MerchantSettings : IMerchantSettings
    {
        public string CardNumber { get ; set; }
    }
}