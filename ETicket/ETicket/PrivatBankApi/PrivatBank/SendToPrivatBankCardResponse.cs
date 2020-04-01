using System.Xml.Serialization;
using ETicket.PrivatBankApi.Interfaces;

namespace ETicket.PrivatBankApi.PrivatBank
{
    public class SendToPrivatBankCardResponse : IResponseData
    {
        [XmlElement(ElementName = "payment")]
        public Payment Payment { get; set; }
    }
}