namespace Sanji
{
    using System;
    using System.Net.Http;

    public static class ServiceHttpExtension
    {
        public static HttpClient CreateHttpClient(this Service self)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:{self.Settings.Port}"),
            };
            return httpClient;
        }
    }
}
