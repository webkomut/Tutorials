using System;
using Xunit;

namespace Tutorials.ApiCall.Test
{
    public class HttpClientBuilderTest
    {
        private IHttpClient builder;
        [Fact]
        public void Bulder_Structure()
        {
            builder = new HttpClientBuilder()
                .ContentTypeJson()
                .ContentType("application/json")
                .Accept("text/xml")
                .AcceptJson()
                .AcceptXml()
                .Headers("name", "value")
                .Headers("api-key", "key")
                .ContentTypeXml()
                .Authorization("toke")
                .Build();
            Assert.Equal("Tutorials.ApiCall.HttpClientService", builder.GetType().FullName);
        }
    }
}
