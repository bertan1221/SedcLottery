using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Lottery.Data;
using Lottery.Data.Model;
using Lottery.Data.Model.DataModels;
using Lottery.Mapper;
using Lottery.Service.UoW;
using Lottery.View.Model;

namespace Lottery.Service
{
    public class LotteryManager : ILotteryManager
    {
        private readonly DbContext dbContext;
        private readonly IRepository<Code> codeRepository;
        private readonly IRepository<Award> awardRepository;
        private readonly IRepository<UserCode> userCodeRepository;
        private readonly IRepository<UserCodeAward> userCodeAwardRepository;

        public LotteryManager(DbContext dbContext, IRepository<Code> codeRepository, IRepository<Award> awardRepository,
            IRepository<UserCode> userCodeRepository, IRepository<UserCodeAward> userCodeAwardRepository)
        {
            this.dbContext = dbContext;
            this.codeRepository = codeRepository;
            this.awardRepository = awardRepository;
            this.userCodeRepository = userCodeRepository;
            this.userCodeAwardRepository = userCodeAwardRepository;
        }

        public AwardModel CheckCode(UserCodeModel userCodeModel)
        {
            using(var uow = new UnitOfWork(dbContext))
            {
                var code = codeRepository.GetAll().FirstOrDefault(x => x.CodeValue == userCodeModel.Code.CodeValue);

                if(code == null)
                {
                    throw new ApplicationException("Invalid Code");
                }
                if (code.IsUsed)
                {
                    throw new ApplicationException("Code is used");
                }

                var userCode = new UserCode(userCodeModel.FirstName, userCodeModel.LastName, userCodeModel.Email, DateTime.Now, code);

                userCodeRepository.Insert(userCode);

                Award award = null;
                if (code.IsWinning)
                {
                    award = GetRandomAward(RuffledType.Immediate);

                    var userCodeAward = new UserCodeAward(award, userCode, DateTime.Now);

                    userCodeAwardRepository.Insert(userCodeAward);
                }

                code.IsUsed = true;
                uow.Commit();

                return award?.Map<Award, AwardModel>();
                
            }
        }

        public List<UserCodeAwardModel> GetWinnersList()
        {
            using(new UnitOfWork(dbContext))
            {
                var winners = userCodeAwardRepository.GetAll().Include(x => x.UserCode.Code).Include(x => x.Award).ToList();

                return winners.Select(x => x.Map<UserCodeAward, UserCodeAwardModel>()).ToList();
            }
        }

        private Award GetRandomAward(RuffledType type)
        {
            var awards = awardRepository.GetAll().Where(x => x.RuffledType == (byte)type).ToList();
            var awardedAwards = userCodeAwardRepository.GetAll().Where(x => x.Award.RuffledType == (byte)type)
                .Select(x => x.Award)
                .GroupBy(x => x.Id)
                .ToList();

            var availableAwards = new List<Award>();

            foreach (var award in awards)
            {
                var numberOfAwardedAwards = awardedAwards.FirstOrDefault(x => x.Key == award.Id)?.Count() ?? 0;
                var awardsLeft = award.Quantity - numberOfAwardedAwards;
                availableAwards.AddRange(Enumerable.Repeat(award, awardsLeft));
            }

            if (availableAwards.Count == 0)
            {
                throw new ApplicationException("We are out of awards. Sorry!");
            }

            var rnd = new Random();
            var randomAwardIndex = rnd.Next(0, availableAwards.Count);
            return availableAwards[randomAwardIndex];
        }
    }
}
