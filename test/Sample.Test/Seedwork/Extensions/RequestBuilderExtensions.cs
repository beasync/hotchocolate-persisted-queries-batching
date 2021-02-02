using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Microsoft.AspNetCore.TestHost
{
    public static class RequestBuilderExtensions
    {
        public static RequestBuilder WithJsonBody(
            this RequestBuilder builder,
            object body)
        {
            return builder.And(configure =>
            {
                configure.Content = new StringContent(
                    content: JsonConvert.SerializeObject(body),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json");
            });
        }
    }
}
