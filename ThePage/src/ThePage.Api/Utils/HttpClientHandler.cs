using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ThePage.Api
{
    public class HttpClientHandler : DelegatingHandler
    {
        readonly string _token;

        public HttpClientHandler(HttpMessageHandler innerHandler = null)
            : base(innerHandler ?? new System.Net.Http.HttpClientHandler())
        {
        }

        public HttpClientHandler(string token, HttpMessageHandler innerHandler = null)
            : base(innerHandler ?? new System.Net.Http.HttpClientHandler())
        {
            _token = token;
        }

        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var req = request;
            var id = Guid.NewGuid().ToString();
#if DEBUG
            var msg = $"[{id} -   Request]";

            Debug.WriteLine($"{msg}========Start Request==========");
            Debug.WriteLine($"{msg} {req.Method} {req.RequestUri.PathAndQuery} {req.RequestUri.Scheme}/{req.Version}");
            Debug.WriteLine($"{msg} Host: {req.RequestUri.Scheme}://{req.RequestUri.Host}");
#endif
            if (req.Content != null)
            {
                if (req.Content is StringContent || IsTextBasedContentType(req.Headers) ||
                    IsTextBasedContentType(req.Content.Headers))
                {
                    var result = await req.Content.ReadAsStringAsync();
#if DEBUG
                    var parsedJson = JToken.Parse(result);
                    var beautified = parsedJson.ToString(Formatting.Indented);

                    Debug.WriteLine($"{msg} Content:");
                    Debug.WriteLine($"{beautified}");
#endif
                }
            }

            var start = DateTime.Now;


            if (_token != null)
            {
                var auth = request.Headers.Authorization;
                if (auth != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, _token);
                }
            }
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var end = DateTime.Now;
#if DEBUG

            Debug.WriteLine($"{msg} Duration: {end - start}");
            Debug.WriteLine($"{msg}==========End Request==========");

            msg = $"[{id} - Response]";
            Debug.WriteLine($"{msg}=========Start Response=========");
#endif
            var resp = response;

#if DEBUG
            Debug.WriteLine(
                $"{msg} {req.RequestUri.Scheme.ToUpper()}/{resp.Version} {(int)resp.StatusCode} {resp.ReasonPhrase}");
#endif
            if (resp.Content != null)
            {
                if (resp.Content is StringContent || IsTextBasedContentType(resp.Headers) ||
                    IsTextBasedContentType(resp.Content.Headers))
                {
                    start = DateTime.Now;
                    var result = await resp.Content.ReadAsStringAsync();
                    end = DateTime.Now;
#if DEBUG
                    var parsedJson = JToken.Parse(result);
                    var beautified = parsedJson.ToString(Formatting.Indented);

                    Debug.WriteLine($"{msg} Content:");
                    Debug.WriteLine($"{beautified}");
                    Debug.WriteLine($"{msg} Duration: {end - start}");
#endif
                }
            }
#if DEBUG
            Debug.WriteLine($"{msg}==========End Response==========");
#endif
            return response;
        }

        readonly string[] types = new[] { "html", "text", "xml", "json", "txt", "x-www-form-urlencoded" };

        bool IsTextBasedContentType(HttpHeaders headers)
        {
            if (!headers.TryGetValues("Content-Type", out IEnumerable<string> values))
                return false;
            var header = string.Join(" ", values).ToLowerInvariant();

            return types.Any(t => header.Contains(t));
        }
    }
}