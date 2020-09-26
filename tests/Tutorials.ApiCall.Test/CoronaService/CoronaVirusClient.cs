using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tutorials.ApiCall.Test.CoronaService
{
    public class CoronaVirusClient
    {
        private readonly IHttpClient client;
        private readonly CoronaConfiguration configuration;
        public CoronaVirusClient(CoronaConfiguration configuration)
        {
            this.configuration = configuration;
            client = new HttpClientBuilder()
                .AcceptJson()
                .Build();
        }

        public async Task<CoronaModel> GetByRegion(string region, CancellationToken cancellationToken = default)
        {
            var uri = new Uri($"{configuration.Host}/spots/region?region={region}");
            var result = await client.Get(uri, cancellationToken);
            return JsonConvert.DeserializeObject<CoronaModel>(result);
        }

        public async Task<CoronaModel> GetLatest(CancellationToken cancellationToken = default)
        {
            var uri = new Uri($"{configuration.Host}/summary/latest");
            var result = await client.Get(uri, cancellationToken);
            return JsonConvert.DeserializeObject<CoronaModel>(result);
        }
    }

    public class CoronaConfiguration
    {
        public string Host { get; set; }
    }
}