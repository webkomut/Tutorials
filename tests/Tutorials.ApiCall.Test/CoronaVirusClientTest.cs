using Tutorials.ApiCall.Test.CoronaService;
using Xunit;

namespace Tutorials.ApiCall.Test
{
    public class CoronaVirusClientTest
    {
        private readonly CoronaVirusClient client;

        public CoronaVirusClientTest()
        {
            client = new CoronaVirusClient(new CoronaConfiguration
            {
                Host = "https://api.quarantine.country/api/v1"
            });
        }

        [Fact]
        public async void Get_Region()
        {
            var result = await client.GetByRegion("turkey");
            Assert.Equal(200, result.status);
        }

        [Fact]
        public async void Get_Latest()
        {
            var result = await client.GetLatest();
            Assert.Equal(200, result.status);
        }
    }
}