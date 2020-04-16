using System.Xml.Serialization;
using ETicketWebAPI.PrivatBankApi.Interfaces;

namespace ETicketWebAPI.PrivatBankApi
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