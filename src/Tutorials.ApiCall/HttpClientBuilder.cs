using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Tutorials.ApiCall
{
    public class HttpClientBuilder
    {
        private readonly HttpClientService clientService;
        private readonly HttpClientHandler handler;

        public HttpClientBuilder()
        {
            handler = new HttpClientHandler();
            clientService = new HttpClientService(handler)
            {
                ContentType = "application/json",
                Encoding = Encoding.UTF8
            };
        }

        public HttpClientBuilder ContentTypeJson()
        {
            return ContentType("application/json");
        }

        public HttpClientBuilder ContentTypeXml()
        {
            return ContentType("application/xml");
        }

        public HttpClientBuilder ContentTypeFormUrlencoded()
        {
            return ContentType("application/x-www-form-urlencoded");
        }

        public HttpClientBuilder ContentType(string value)
        {
            clientService.ContentType = value;
            Headers("ContentType", value);
            return this;
        }

        public HttpClientBuilder Headers(string name, string value)
        {
            clientService.DefaultRequestHeaders.Add(name, value);
            return this;
        }

        public HttpClientBuilder AcceptJson()
        {
            return Accept("application/json");
        }
        public HttpClientBuilder AcceptXml()
        {
            return Accept("application/xml");
        }
        public HttpClientBuilder Accept(string value)
        {
            clientService.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(value));
            return this;
        }

        public HttpClientBuilder Authorization(string token)
        {
            return Headers("Authorization", token);
        }

        public HttpClientBuilder TimeOut(TimeSpan value)
        {
            clientService.Timeout = value;
            return this;
        }

        public HttpClientBuilder AutomaticDecompression(bool value)
        {
            var decompression = value
                ? DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli |
                  DecompressionMethods.All
                : DecompressionMethods.None;
            handler.AutomaticDecompression = decompression;
            return this;
        }

        public IHttpClient Build()
        {
            return clientService;
        }
    }
}