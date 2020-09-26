using Tutorials.ApiCall.Test.LocalApiService;
using Xunit;

namespace Tutorials.ApiCall.Test
{
    public class LocalApiClientTest
    {
        private readonly LocalApiClient client;
        public LocalApiClientTest()
        {
            var config = new MyApiConfiguration();
            var auth = new LocalApiAuth(config);
            var token = auth.Login(new LoginRequest
            {
                Username = "testuser",
                Password = "tst@45f98"
            }).Result;
            config.Token = token.Token;

            client = new LocalApiClient(config);
        }

        [Fact]
        public async void Get_Profile()
        {
            var resul = await client.GetProfile();
            Assert.Equal("5cf774ca-e954-452b-b661-798ba8863cb1", resul.Id.ToString());
            Assert.Equal("testuser", resul.Username);
        }

        [Fact]
        public async void Get_Emails()
        {
            var result = await client.GetEmailsByUserId("5cf774ca-e954-452b-b661-798ba8863cb1");
            Assert.True(result.Length > 0);
        }
    }
}