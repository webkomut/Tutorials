using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tutorials.ApiCall.Test.LocalApiService
{
    public class LocalApiClient
    {
        private readonly MyApiConfiguration configuration;
        private readonly IHttpClient client;

        public LocalApiClient(MyApiConfiguration configuration)
        {
            this.configuration = configuration;
            client = new HttpClientBuilder()
                .AcceptJson()
                .ContentTypeJson()
                .TimeOut(new TimeSpan(0, 0, 0,50))
                .Authorization($"Bearer {configuration.Token}")
                .Build();
        }

        public async Task<ProfileResponse> GetProfile(CancellationToken cancellationToken = default)
        {
            var uri = new Uri($"{configuration.Host}/profile");
            var result = await client.Get(uri, cancellationToken);
            return JsonConvert.DeserializeObject<ProfileResponse>(result);
        }

        public async Task<EmailResponse[]> GetEmailsByUserId(string userId,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri($"{configuration.Host}/email/{userId}");
            var result = await client.Get(uri, cancellationToken);
            return JsonConvert.DeserializeObject<EmailResponse[]>(result);
        }
    }
}