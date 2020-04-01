using System.Xml.Serialization;
using ETicket.PrivatBankApi.Interfaces;

namespace ETicket.PrivatBankApi
{
    [XmlRoot(ElementName = "response")]
    public class ApiResponse<T>
        where T : IResponseData
    {
        [XmlElement(ElementName = "merchant")]
        public Merchant Merchant { get; set; }

        [XmlElement(ElementName = "data")]
        public T Data { get; set; }
    }
}