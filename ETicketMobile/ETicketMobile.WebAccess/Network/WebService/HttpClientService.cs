using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Android.Net;

namespace ETicketMobile.WebAccess.Network.WebService
{
    public class HttpClientService
    {
        private readonly HttpClient httpClient;

        public HttpClientService()
        {
            var androidClientHandler = new AndroidClientHandler();
            httpClient = new HttpClient(androidClientHandler);
        }

        public async Task<T> GetAsync<T>(Uri endPoint, string token)
        {
            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpResponse = await httpClient.GetAsync(endPoint).ConfigureAwait(false);
            var responseData = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<T>(responseData);

            return response;
        }

        public async Task<TDestination> PostAsync<TSource, TDestination>(Uri endPoint, TSource item, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = JsonConvert.SerializeObject(item);
            var httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpResponse = await httpClient.PostAsync(endPoint, httpContent).ConfigureAwait(false);
            var response = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TDestination>(response);
        }
    }
}