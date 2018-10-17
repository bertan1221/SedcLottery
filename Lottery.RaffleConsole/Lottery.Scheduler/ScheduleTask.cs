using System;
using System.Threading.Tasks;
using Lottery.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Scheduler
{
    public class ScheduleTask : ScheduledProcessor
    {
        private readonly ILotteryManager _lotteryManager;
        private readonly ICodesManager _codesManager;
        public ScheduleTask(IServiceScopeFactory serviceScopeFactory, ILotteryManager lotteryManager, ICodesManager codesManager) : base(serviceScopeFactory)
        {
            _lotteryManager = lotteryManager;
            _codesManager = codesManager;
        }

        public override Task ProcessInScope(IServiceProvider serviceProvider)
        {
            try
            {
                Console.WriteLine($"Raffle start at: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                _lotteryManager.Raffle();
                Console.WriteLine($"Raffle finished at: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");

               // Console.WriteLine($"Codes processing start at: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                //_codesManager.ProcessCodes();
                //Console.WriteLine($"Codes processing finished at: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }

        protected override string Schedule => "* * * * *";
    }
}
