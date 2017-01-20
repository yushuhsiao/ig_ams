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
    
    public partial class Table
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Table()
        {
            this.BaccaratGame = new HashSet<BaccaratGame>();
            this.RouletteGame = new HashSet<RouletteGame>();
            this.SicboGame = new HashSet<SicboGame>();
            this.ValidDeputy = new HashSet<ValidDeputy>();
        }
    
        public int Id { get; set; }
        public int GameId { get; set; }
        public string TableName_EN { get; set; }
        public string TableName_CHS { get; set; }
        public string TableName_CHT { get; set; }
        public string RoomName_EN { get; set; }
        public string RoomName_CHS { get; set; }
        public string RoomName_CHT { get; set; }
        public string Announcement_EN { get; set; }
        public string Announcement_CHS { get; set; }
        public string Announcement_CHT { get; set; }
        public string StreamUrl { get; set; }
        public string StreamName { get; set; }
        public string Dealer { get; set; }
        public string Password { get; set; }
        public bool IsVipTable { get; set; }
        public bool IsPeekTable { get; set; }
        public bool IsCountdownTable { get; set; }
        public int BetTimeLimit { get; set; }
        public int Sort { get; set; }
        public TableType Type { get; set; }
        public TableStatus Status { get; set; }
        public System.DateTime CreateTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BaccaratGame> BaccaratGame { get; set; }
        public virtual Game Game { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RouletteGame> RouletteGame { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SicboGame> SicboGame { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ValidDeputy> ValidDeputy { get; set; }
    }
}