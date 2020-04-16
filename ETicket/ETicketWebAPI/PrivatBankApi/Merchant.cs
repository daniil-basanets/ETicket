using System.Xml.Serialization;

namespace ETicketWebAPI.PrivatBankApi
{
    public class Merchant
    {
        [XmlElement(ElementName = "id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "signature")]
        public string Signature { get; set; }
    }
}