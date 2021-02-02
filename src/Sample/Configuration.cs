using HotChocolate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Sample
{
    public class Configuration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddInMemoryQueryStorage();

            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddInMemoryQueryStorage()
                .UsePersistedQueryPipeline();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
