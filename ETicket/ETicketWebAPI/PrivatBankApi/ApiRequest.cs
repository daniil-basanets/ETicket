using ETicketWebAPI.PrivatBankApi.Interfaces;
using ETicketWebAPI.Utils;

namespace ETicketWebAPI.PrivatBankApi
{
    public class ApiRequest<TRequest>
        where TRequest : IRequestData
    {
        public int MerchantId { get; }

        public string Signature { get; private set; }

        public TRequest RequestData { get; }

        public ApiRequest(
            int merchantId,
            string password,
            TRequest requestData
        )
        {
            MerchantId = merchantId;
            RequestData = requestData;
            Signature = SignatureHelper.GetSignature(requestData.GetXML(), password);
        }

        public string GetXML()
        {
            return @"<?xml version=""1.0"" encoding=""UTF-8""?>"
                 + @"<request version=""1.0"">"
                 + @"<merchant>"
                 + $@"<id>{MerchantId}</id>"
                 + $@"<signature>{Signature}</signature>"
                 + @"</merchant>"
                 + $@"<data>{RequestData.GetXML()}</data>"
                 + @"</request>";
        }
    }
}