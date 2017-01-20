//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            this.BaccaratBetLog = new HashSet<BaccaratBetLog>();
            this.JackpotLog = new HashSet<JackpotLog>();
            this.RouletteBetLog = new HashSet<RouletteBetLog>();
            this.SicboBetLog = new HashSet<SicboBetLog>();
            this.ValidDeputy = new HashSet<ValidDeputy>();
            this.Wallet = new HashSet<Wallet>();
            this.MemberPicture = new HashSet<MemberPicture>();
            this.DouDizhuBet = new HashSet<DouDizhuBet>();
            this.DouDizhuGame = new HashSet<DouDizhuGame>();
            this.TexasBet = new HashSet<TexasBet>();
        }
    
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public decimal Balance { get; set; }
        public int Stock { get; set; }
        public MemberRole Role { get; set; }
        public byte Type { get; set; }
        public MemberStatus Status { get; set; }
        public string Email { get; set; }
        public System.DateTime RegisterTime { get; set; }
        public string LastLoginIp { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public string AccessToken { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BaccaratBetLog> BaccaratBetLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JackpotLog> JackpotLog { get; set; }
        public virtual MemberBaccaratConfig MemberBaccaratConfig { get; set; }
        public virtual MemberRouletteConfig MemberRouletteConfig { get; set; }
        public virtual MemberSicboConfig MemberSicboConfig { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RouletteBetLog> RouletteBetLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SicboBetLog> SicboBetLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ValidDeputy> ValidDeputy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wallet> Wallet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberPicture> MemberPicture { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DouDizhuBet> DouDizhuBet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DouDizhuGame> DouDizhuGame { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TexasBet> TexasBet { get; set; }
    }
}