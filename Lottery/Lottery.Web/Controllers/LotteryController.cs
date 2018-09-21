using Lottery.Service;
using Lottery.View.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lottery.Web.Controllers
{
    public class LotteryController : ApiController
    {
        private readonly ILotteryManager lotteryManager;

        public LotteryController(ILotteryManager lotteryManager)
        {
            this.lotteryManager = lotteryManager;
        }

        [HttpPost]
        public AwardModel SubmitCode([FromBody] UserCodeModel userCodeModel)
        {
            return lotteryManager.CheckCode(userCodeModel);
        }

        [HttpGet]
        public List<UserCodeAwardModel> GetAllWinners()
        {
            return lotteryManager.GetWinnersList();
        }
    }
}
