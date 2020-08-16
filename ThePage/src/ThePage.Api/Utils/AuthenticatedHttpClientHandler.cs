using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ThePage.Api
{
    public class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        readonly string _token;

        public AuthenticatedHttpClientHandler(string token)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // See if the request has an authorize header
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, _token);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}