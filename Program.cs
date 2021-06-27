using EfCoreBugDemo.Models;
using EfCoreBugDemo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EfCoreBugDemo
{
    class Program : BackgroundService
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, builder) => builder.AddConsole())
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<ConsoleLifetimeOptions>(options =>
                        options.SuppressStatusMessages = true);

                    services.AddDbContextFactory<ShopContext>(options =>
                    {
                        options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ShopDb;Trusted_Connection=True;");
                    });

                    services.AddHostedService<Program>();
                });


        private readonly IServiceProvider provider;


        public Program(IServiceProvider provider)
        {
            this.provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine();
            Console.WriteLine("Please wait 30s for timeout exception.");
            Console.WriteLine();

            var hostLifetime = provider.GetRequiredService<IHostApplicationLifetime>();

            var dbContextFactory = provider
                .GetRequiredService<IDbContextFactory<ShopContext>>();

            var service = new UpdateCategoryService(dbContextFactory);
            await service.ExecuteAsync();

            hostLifetime.StopApplication();
        }
    }
}
