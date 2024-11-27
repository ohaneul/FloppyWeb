using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace FloppyWeb.Network
{
    public class RequestHandler
    {
        private readonly HttpClient httpClient;
        private readonly RequestCache requestCache;

        public RequestHandler()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = ValidateServerCertificate
            };

            httpClient = new HttpClient(handler);
            requestCache = new RequestCache();
        }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
        {
            try
            {
                // Check cache first
                if (request.Method == HttpMethod.Get)
                {
                    var cachedResponse = requestCache.GetCachedResponse(request.RequestUri.ToString());
                    if (cachedResponse != null)
                    {
                        return cachedResponse;
                    }
                }

                // Send request
                var response = await httpClient.SendAsync(request);

                // Cache the response if it's a GET request
                if (request.Method == HttpMethod.Get && response.IsSuccessStatusCode)
                {
                    requestCache.CacheResponse(request.RequestUri.ToString(), response);
                }

                return response;
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Request error: {ex.Message}");
                throw;
            }
        }

        private bool ValidateServerCertificate(
            HttpRequestMessage requestMessage,
            X509Certificate2 certificate,
            X509Chain chain,
            SslPolicyErrors sslErrors)
        {
            // Add custom certificate validation logic here
            return sslErrors == SslPolicyErrors.None;
        }
    }
} 