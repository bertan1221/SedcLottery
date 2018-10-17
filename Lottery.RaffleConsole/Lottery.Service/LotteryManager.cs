using Lottery.Data;
using Lottery.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Lottery.Service
{
    public class LotteryManager : ILotteryManager
    {
        private readonly IRepository<Award> _awardRepository;
        private readonly IRepository<UserCodeAward> _userCodeAwardRepository;
        private readonly IRepository<UserCode> _userCodeRepository;
        private readonly IConfigurationRoot _configurationRoot;

        public LotteryManager(IRepository<Award> awardRepository,
            IRepository<UserCodeAward> userCodeAwardRepository,
            IRepository<UserCode> userCodeRepository,
            IConfigurationRoot configurationRoot)
        {
            _awardRepository = awardRepository;
            _userCodeAwardRepository = userCodeAwardRepository;
            _userCodeRepository = userCodeRepository;
            _configurationRoot = configurationRoot;
        }

        public void Raffle()
        {
            var finalRaffle = DateTime.Parse(_configurationRoot.GetSection("FinalRaffle").Value);

            if (DateTime.Now.Date <= finalRaffle)
            {
                GiveAwards(RaffledType.PerDay);
            }

            if (DateTime.Now.Date == finalRaffle)
            {
                GiveAwards(RaffledType.Final);
            }
        }

        private void GiveAwards(RaffledType type)
        {
            var awardsQuantity = GetAwardQuantityPerType(type);

            for (int i = 0; i < awardsQuantity; i++)
            {
                GiveAward(type);
            }
        }

        private int GetAwardQuantityPerType(RaffledType type)
        {
            var awardsQuantity = _awardRepository.GetAll().Where(x => x.RuffledType == (byte)type)
                .Select(x => x.Quantity).Sum();

            return awardsQuantity;
        }

        private void GiveAward(RaffledType type)
        {
            var losers = _userCodeRepository.GetAll().Include(x => x.Code).Where(x => !x.Code.IsWinning);

            if (type == RaffledType.PerDay)
            {
                losers = losers.Where(x => x.SentAt.Date == DateTime.Now.Date);
            }

            var losersList = losers.ToList();

            var userCodeAward = _userCodeAwardRepository.GetAll().ToList();

            losersList = losersList.Where(x => userCodeAward.All(y => y.UserCodeId != x.Id)).ToList();

            if (!losersList.Any()) return;

            var rnd = new Random();
            var randomLoserIndex = rnd.Next(0, losersList.Count - 1);
            var winningUser = losersList[randomLoserIndex];

            var randomAward = GetRandomAward(type);

            _userCodeAwardRepository.Insert(new UserCodeAward
            {
                Award = randomAward,
                UserCode = winningUser,
                WonAt = DateTime.Now
            });
        }

        private Award GetRandomAward(RaffledType type)
        {
            var awards = _awardRepository.GetAll().Where(x => x.RuffledType == (byte)type).ToList();
            var awardedAwards = _userCodeAwardRepository
                .GetAll()
                .Where(x => x.Award.RuffledType == (byte)type);

            if (type == RaffledType.PerDay)
            {
                awardedAwards = awardedAwards.Where(x => x.WonAt.Date == DateTime.Now.Date);
            }

            var awardedAwardsGroup = awardedAwards
                .Select(x => x.Award)
                .GroupBy(x => x.Id)
                .ToList();

            var availableAwards = new List<Award>();

            foreach (var award in awards)
            {
                var numberOfAwardedAwards = awardedAwardsGroup
                    .FirstOrDefault(x => x.Key == award.Id)?.Count() ?? 0;
                var awardsLeft = award.Quantity - numberOfAwardedAwards;
                availableAwards.AddRange(Enumerable.Repeat(award, awardsLeft));
            }

            if (availableAwards.Count == 0)
                throw new ApplicationException("We are out of awards. Sorry!");

            var rnd = new Random();
            var randomAwardIndex = rnd.Next(0, availableAwards.Count - 1);
            return availableAwards[randomAwardIndex];
        }
    }
}
