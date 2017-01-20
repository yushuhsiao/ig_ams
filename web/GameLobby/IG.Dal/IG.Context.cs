﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace IG.Dal
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class IGEntities : DbContext
    {
        public IGEntities()
            : base("name=IGEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<GameConfig> GameConfig { get; set; }
        public virtual DbSet<Jackpot> Jackpot { get; set; }
        public virtual DbSet<JackpotLog> JackpotLog { get; set; }
        public virtual DbSet<Wallet> Wallet { get; set; }
        public virtual DbSet<MemberBaccaratConfig> MemberBaccaratConfig { get; set; }
        public virtual DbSet<MemberRouletteConfig> MemberRouletteConfig { get; set; }
        public virtual DbSet<MemberSicboConfig> MemberSicboConfig { get; set; }
        public virtual DbSet<BaccaratGame> BaccaratGame { get; set; }
        public virtual DbSet<RouletteGame> RouletteGame { get; set; }
        public virtual DbSet<SicboGame> SicboGame { get; set; }
        public virtual DbSet<BaccaratBetLog> BaccaratBetLog { get; set; }
        public virtual DbSet<RouletteBetLog> RouletteBetLog { get; set; }
        public virtual DbSet<SicboBetLog> SicboBetLog { get; set; }
        public virtual DbSet<BaccaratBetLimit> BaccaratBetLimit { get; set; }
        public virtual DbSet<RouletteBetLimit> RouletteBetLimit { get; set; }
        public virtual DbSet<SicboBetLimit> SicboBetLimit { get; set; }
        public virtual DbSet<ValidDeputy> ValidDeputy { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<GdMahjongConfig> GdMahjongConfig { get; set; }
        public virtual DbSet<TwMahjongConfig> TwMahjongConfig { get; set; }
        public virtual DbSet<Table> Table { get; set; }
        public virtual DbSet<MemberPicture> MemberPicture { get; set; }
        public virtual DbSet<TwMahjongGame> TwMahjongGame { get; set; }
        public virtual DbSet<DouDizhuBet> DouDizhuBet { get; set; }
        public virtual DbSet<DouDizhuGame> DouDizhuGame { get; set; }
        public virtual DbSet<TexasBet> TexasBet { get; set; }
        public virtual DbSet<TexasGame> TexasGame { get; set; }
        public virtual DbSet<GdMahjongGame> GdMahjongGame { get; set; }
    }
}