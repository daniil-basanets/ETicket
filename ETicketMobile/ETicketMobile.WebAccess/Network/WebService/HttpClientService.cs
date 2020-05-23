﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Android.Net;

namespace ETicketMobile.WebAccess.Network.WebService
{
    public class HttpClientService
    {
        #region Fields

        private readonly HttpClient httpClient;

        private readonly Uri serverAddress;

        #endregion

        public HttpClientService(Uri serverAddress)
        {
            var androidClientHandler = new AndroidClientHandler();
            httpClient = new HttpClient(androidClientHandler);

            this.serverAddress = serverAddress
                ?? throw new ArgumentNullException(nameof(serverAddress));
        }

        public async Task<T> GetAsync<T>(Uri endpoint, string token)
        {
            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var fullEndpoint = new Uri(serverAddress, endpoint);

            var httpResponse = await httpClient.GetAsync(fullEndpoint).ConfigureAwait(false);
            var responseData = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<T>(responseData);

            return response;
        }

        public async Task<TDestination> PostAsync<TSource, TDestination>(Uri endpoint, TSource item, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = JsonConvert.SerializeObject(item);
            var httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var fullEndpoint = new Uri(serverAddress, endpoint);
            var httpResponse = await httpClient.PostAsync(fullEndpoint, httpContent).ConfigureAwait(false);
            var response = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TDestination>(response);
        }
    }
}