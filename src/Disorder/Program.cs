using System.Threading.Tasks;
using Disorder.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Disorder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostingContext, services) =>
           {
               services.AddHostedService<Startup>();
               services.Configure<Appsettings>(hostingContext.Configuration);
           });

    }
}
