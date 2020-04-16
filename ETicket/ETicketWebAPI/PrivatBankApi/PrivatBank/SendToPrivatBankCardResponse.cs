using System.Xml.Serialization;
using ETicketWebAPI.PrivatBankApi.Interfaces;

namespace ETicketWebAPI.PrivatBankApi.PrivatBank
{
    public class SendToPrivatBankCardResponse : IResponseData
    {
        [XmlElement(ElementName = "payment")]
        public Payment Payment { get; set; }
    }
}