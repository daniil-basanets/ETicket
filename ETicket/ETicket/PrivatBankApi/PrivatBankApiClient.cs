using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ETicket.PrivatBankApi.Interfaces;
using ETicket.Utils;

namespace ETicket.PrivatBankApi
{
    public class PrivatBankApiClient
    {
        private readonly int merchantId;
        private readonly string password;

        private readonly HttpClient httpClient;

        public PrivatBankApiClient(int merchantId, string password)
        {
            this.merchantId = merchantId;
            this.password = password;

            httpClient = new HttpClient();
        }

        public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request)
            where TRequest : IRequestData
            where TResponse : IResponseData
        {
            var responseText = await PostAsync(request);
            var response = XmlHelper.Deserialize<ApiResponse<TResponse>>(responseText);

            return response.Data;
        }

        private async Task<string> PostAsync<TRequest>(TRequest request)
            where TRequest : IRequestData
        {
            var apiRequest = new ApiRequest<TRequest>(merchantId, password, request);
            var requestXml = apiRequest.GetXML();

            var httpContent = new StringContent(requestXml, Encoding.UTF8, "application/xml");
            var httpResponse = await httpClient.PostAsync(request.Endpoint, httpContent);
            var response = await httpResponse.Content.ReadAsStringAsync();

            return response;
        }
    }
}