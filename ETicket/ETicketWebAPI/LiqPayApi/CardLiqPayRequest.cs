using LiqPay.SDK.Dto;
using Newtonsoft.Json;

namespace ETicket.WebAPI.LiqPayApi
{
    public class CardLiqPayRequest : LiqPayRequest
    {
        [JsonProperty("card")]
        public string Card { get; set; }

        [JsonProperty("card_exp_month")]
        public string CardExpirationMonth { get; set; }

        [JsonProperty("card_exp_year")]
        public string CardExpirationYear { get; set; }

        [JsonProperty("card_cvv")]
        public string CardCvv2 { get; set; }
    }
}