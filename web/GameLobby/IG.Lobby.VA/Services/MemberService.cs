using IG.Dal;
using IG.Lobby.VA.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IG.Lobby.VA.Services
{
    public class MemberService
    {
        private IGEntities dbContext;

        public MemberService(IGEntities context)
        {
            dbContext = context;
        }

        public Member CreateDemoPlayer()
        {
            var accessToken = Guid.NewGuid().ToString("N");
            var account = accessToken.Substring(0, 8);

            var player = new Member
            {
                ParentId = 2,
                Account = account,
                Password = "123456",
                Nickname = account.ToUpper(),
                Balance = 10000000,
                Role = MemberRole.Player,
                Status = MemberStatus.Active,
                RegisterTime = DateTime.Now,
                AccessToken = accessToken,
                MemberBaccaratConfig = new MemberBaccaratConfig
                {
                    BetLimitId = 1,
                    UpperBetLimitId = 1,
                    RakeType = RakeType.AllBet,
                    RakeRatio = 0,
                    FundRatio = 0,
                    DesktopBetChip = "1,5,10,20,50,100,200",
                    MobileBetChip = "1,5,10,20,50"
                },
                MemberRouletteConfig = new MemberRouletteConfig
                {
                    BetLimitId = 1,
                    UpperBetLimitId = 1,
                    FundRatio = 0,
                    DesktopBetChip = "1,5,10,20,50,100,200",
                    MobileBetChip = "1,5,10,20,50"
                },
                MemberSicboConfig = new MemberSicboConfig
                {
                    BetLimitId = 1,
                    UpperBetLimitId = 1,
                    FundRatio = 0,
                    DesktopBetChip = "1,5,10,20,50,100,200",
                    MobileBetChip = "1,5,10,20,50"
                }
            };

            dbContext.Member.Add(player);
            dbContext.SaveChanges();

            return player;
        }

        public Member GetPlayerByAccessToken(string accessToken)
        {
            return dbContext.Member.FirstOrDefault(x => x.AccessToken == accessToken && x.Role == MemberRole.Player && x.Status != MemberStatus.Delete);
        }

        public Member VerifyAccount(string account, string password)
        {
            var member = dbContext.Member.FirstOrDefault(x => x.Account == account && x.Role == MemberRole.Player && x.Status != MemberStatus.Delete);

            if (member == null)
            {
                return null;
            }

            if (!VerifyPassword(password, member.Password))
            {
                return null;
            }

            return member;
        }

        public Member Login(Member member, string ipAddress)
        {
            member.LastLoginIp = ipAddress;
            member.LastLoginTime = DateTime.Now;
            member.AccessToken = Guid.NewGuid().ToString("N");

            dbContext.Entry(member).State = EntityState.Modified;
            dbContext.SaveChanges();

            return member;
        }

        public Member LoginByAccessToken(Member member, string ipAddress)
        {
            member.LastLoginIp = ipAddress;
            member.LastLoginTime = DateTime.Now;

            dbContext.Entry(member).State = EntityState.Modified;
            dbContext.SaveChanges();

            return member;
        }

        public bool CheckAccessToken(string accessToken)
        {
            return dbContext.Member.Any(x => x.AccessToken == accessToken);
        }

        public bool IsParentDisable(int id)
        {
            return GetAllParents(id).Any(x => x.Status == MemberStatus.Disable);
        }

        private bool VerifyPassword(string password, string encryptPassword)
        {
            return NcryptHelper.Verify(password, encryptPassword);
        }

        private IEnumerable<Member> GetAllParents(int id)
        {
            return dbContext.Database.SqlQuery<Member>("EXEC dbo.usp_GetAllParents @Id = {0}", id);
        }
    }
}
