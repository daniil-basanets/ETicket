using System.Xml.Serialization;
using ETicket.WebAPI.PrivatBankApi.Interfaces;

namespace ETicket.WebAPI.PrivatBankApi.PrivatBank
{
    public class SendToPrivatBankCardResponse : IResponseData
    {
        [XmlElement(ElementName = "payment")]
        public Payment Payment { get; set; }
    }
}