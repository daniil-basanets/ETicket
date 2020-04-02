using System.Xml.Serialization;
using ETicket.PrivatBankApi.Interfaces;

namespace ETicket.PrivatBankApi.AnyBank
{
    [XmlRoot(ElementName = "payment")]
    public class SendToAnyBankCardResponse : IResponseData
    {
        [XmlAttribute(AttributeName = "id")]
        public string RequestId { get; set; }

        [XmlAttribute(AttributeName = "state")]
        public int State { get; set; }

        [XmlAttribute(AttributeName = "message")]
        public string Message { get; set; }

        [XmlAttribute(AttributeName = "ref")]
        public string Reference { get; set; }

        [XmlAttribute(AttributeName = "amt")]
        public decimal Amount { get; set; }

        [XmlAttribute(AttributeName = "ccy")]
        public string Currency { get; set; }

        [XmlAttribute(AttributeName = "comis")]
        public decimal Comissions { get; set; }

        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "cardinfo")]
        public string CardInfo { get; set; }
    }
}