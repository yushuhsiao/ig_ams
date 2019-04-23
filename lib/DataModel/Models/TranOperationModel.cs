using System;
using System.ComponentModel.DataAnnotations;

namespace InnateGlory.Models
{
    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TranOperationModel
    {
        [Required]
        public Guid? TranId { get; set; }

        public bool? Accept { get; set; }

        public bool? Finish { get; set; }

        public bool? Delete { get; set; }
    }
}