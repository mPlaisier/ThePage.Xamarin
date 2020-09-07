using System;
using System.Net.Http;

namespace ThePage.Api
{
    public static class HttpUtils
    {
        public static HttpClient GetHttpClient(string uri, string token = null)
        {
            var client = new HttpClient(new HttpClientHandler(token))
            {
                BaseAddress = new Uri(uri)
            };
            return client;
        }
    }
}