using System;
using System.IO;
using System.Threading;
using Lottery.Data;
using Lottery.Data.Model;
using Lottery.Scheduler;
using Lottery.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lottery.RaffleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Configure();

            var cronScheduler = serviceProvider.GetService<IHostedService>();
            cronScheduler.StartAsync(new CancellationToken());

            while (true)
            {
            }
    }

        static IServiceProvider Configure()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            IConfigurationRoot configuration = builder.Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(provider => configuration)
                .AddSingleton<DbContext, LotteryContext>()
                .AddSingleton<ILotteryManager, LotteryManager>()
                .AddSingleton(typeof(IRepository<>), typeof(Repository<>))
                .AddSingleton<IHostedService, ScheduleTask>()
                .AddSingleton<ICodesManager, LocalCodesManager>()
                .AddSingleton<IExcelManager, ExcelManager>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
