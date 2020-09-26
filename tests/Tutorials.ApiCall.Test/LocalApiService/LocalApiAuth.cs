using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tutorials.ApiCall.Test.LocalApiService
{
    public class LocalApiAuth
    {
        private readonly MyApiConfiguration configuration;
        private readonly IHttpClient client;

        public LocalApiAuth(MyApiConfiguration configuration)
        {
            this.configuration = configuration;
            client = new HttpClientBuilder()
                .AcceptJson()
                .ContentTypeJson()
                .Build();
        }

        public async Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri($"{configuration.Host}/auth/login");
            var data = JsonConvert.SerializeObject(request);
            var result = await client.Post(uri, data, cancellationToken);
            return JsonConvert.DeserializeObject<LoginResponse>(result);
        }
    }
}