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
    
    public partial class SicboBetLimit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SicboBetLimit()
        {
            this.MemberSicboConfig = new HashSet<MemberSicboConfig>();
        }
    
        public int Id { get; set; }
        public decimal UpperLimit1x50_1x150 { get; set; }
        public decimal LowerLimit1x50_1x150 { get; set; }
        public decimal UpperLimit1x12_1x24 { get; set; }
        public decimal LowerLimit1x12_1x24 { get; set; }
        public decimal UpperLimit1x5_1x8 { get; set; }
        public decimal LowerLimit1x5_1x8 { get; set; }
        public decimal UpperLimit1x1_1x3 { get; set; }
        public decimal LowerLimit1x1_1x3 { get; set; }
        public System.DateTime CreateTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberSicboConfig> MemberSicboConfig { get; set; }
    }
}