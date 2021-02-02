using HotChocolate.Execution;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Sample.Test
{
    public class ServerFixture
    {
        private IHost host;

        public ServerFixture()
        {
            InitializeTestServerAsync()
                .GetAwaiter()
                .GetResult();
        }

        public TestServer Server { get; private set; }

        public async Task WriteQueryAsync(string queryId, IQuery query)
        {
            using var scope = host.Services.CreateScope();
            var storage = scope.ServiceProvider.GetService<IWriteStoredQueries>();

            await storage.WriteQueryAsync(queryId, query);
        }

        private async Task InitializeTestServerAsync()
        {
            host = new HostBuilder()
                .UseEnvironment("Development")
                .ConfigureWebHost(builder =>
                {
                    builder
                        .ConfigureServices(
                        services => services.AddSingleton<IServer>(
                            serviceProvider => new TestServer(serviceProvider)))
                        .UseStartup<TestStartup>();
                })
                .Build();

            await host.StartAsync();

            Server = host.GetTestServer();
        }
    }
}
