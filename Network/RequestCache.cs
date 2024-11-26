using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace FloppyWeb.Network
{
    public class RequestCache
    {
        private readonly ConcurrentDictionary<string, CachedResponse> cache;
        private readonly TimeSpan defaultCacheTime = TimeSpan.FromMinutes(5);

        public RequestCache()
        {
            cache = new ConcurrentDictionary<string, CachedResponse>();
        }

        public HttpResponseMessage GetCachedResponse(string url)
        {
            if (cache.TryGetValue(url, out var cachedResponse))
            {
                if (!cachedResponse.IsExpired)
                {
                    return cachedResponse.Response;
                }
                else
                {
                    cache.TryRemove(url, out _);
                }
            }
            return null;
        }

        public void CacheResponse(string url, HttpResponseMessage response)
        {
            var cachedResponse = new CachedResponse(response, DateTime.Now.Add(defaultCacheTime));
            cache.AddOrUpdate(url, cachedResponse, (key, oldValue) => cachedResponse);
        }

        private class CachedResponse
        {
            public HttpResponseMessage Response { get; }
            public DateTime ExpirationTime { get; }
            public bool IsExpired => DateTime.Now > ExpirationTime;

            public CachedResponse(HttpResponseMessage response, DateTime expirationTime)
            {
                Response = response;
                ExpirationTime = expirationTime;
            }
        }
    }
} 