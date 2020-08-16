using System;
using System.Net.Http;

namespace ThePage.Api
{
    public static class HttpUtils
    {
        public static HttpClient GetHttpClient(string uri, string token = null)
        {
            HttpClient client = null;

#if DEBUG
            client = new HttpClient(new HttpLoggingHandler(token))
            {
                BaseAddress = new Uri(uri)
            };
#else
            if (string.IsNullOrEmpty(token))
            {
                client = new HttpClient()
                {
                    BaseAddress = new Uri(uri)
                };
            }
            else
            {
                client = new HttpClient(new AuthenticatedHttpClientHandler(token))
                {
                    BaseAddress = new Uri(uri)
                };
            }
#endif

            return client;
        }
    }
}
