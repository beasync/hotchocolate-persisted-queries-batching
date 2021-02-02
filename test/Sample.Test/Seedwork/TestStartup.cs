using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Test
{
    internal class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();

            Configuration.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            Configuration.Configure(app);
        }
    }
}
