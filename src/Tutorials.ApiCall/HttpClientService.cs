using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tutorials.ApiCall
{
    public class HttpClientService : HttpClient, IHttpClient
    {
        public Encoding Encoding { get; set; }
        public string ContentType { get; set; }
        public HttpClientService(HttpClientHandler handler): base(handler)
        {
            
        }
        public async Task<string> Get(Uri url, CancellationToken cancellationToken = default)
        {
            try
            {
                BaseAddress = url;
                var result = await GetAsync(url, cancellationToken);
                return await HttpResponseMessageHandler(result);
            }
            finally
            {
                Dispose();
            }
        }

        

        public async Task<string> Post(Uri url, string data, CancellationToken cancellationToken = default)
        {

            try
            {
                BaseAddress = url;
                var content = new StringContent(data, Encoding, ContentType);
                var result = await PostAsync(url, content, cancellationToken);
                return await HttpResponseMessageHandler(result);
            }
            finally
            {
                Dispose();
            }
        }

        public async Task<string> Put(Uri url, string data, CancellationToken cancellationToken = default)
        {

            try
            {
                BaseAddress = url;
                var content = new StringContent(data, Encoding, ContentType);
                var result = await PutAsync(url, content, cancellationToken);
                return await HttpResponseMessageHandler(result);
            }
            finally
            {
                Dispose();
            }
        }

        public async Task<string> Put(Uri url, CancellationToken cancellationToken = default)
        {

            try
            {
                BaseAddress = url;
                var result = await DeleteAsync(url, cancellationToken);
                return await HttpResponseMessageHandler(result);
            }
            finally
            {
                Dispose();
            }
        }

        private async Task<string> HttpResponseMessageHandler(HttpResponseMessage message)
        {
            var byteArray = await message.Content.ReadAsByteArrayAsync();
            var result = Encoding.GetString(byteArray, 0, byteArray.Length);
            if (!message.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }

            return result;
        }
    }
}