using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace InnateGlory.Entity
{
    [TableName("Lang", Database = _Consts.db.CoreDB)]
    public class Lang
    {
        [DbImport]
        public PlatformId PlatformId;

        [DbImport]
        public string Path;

        /// <summary>
        /// enum type name
        /// </summary>
        [DbImport]
        public string Type;

        [DbImport]
        public string Key;

        [DbImport]
        public int LCID;

        [DbImport]
        public string Text;
    }
}
namespace InnateGlory.Models
{
    public class LangInitModel
    {
        [Required]
        public PlatformId? PlatformId { get; set; }

        [Required]
        public string ResPath { get; set; }
    }

    public class LangModel
    {
        [Required]
        public PlatformId? PlatformId { get; set; }

        public string Path { get; set; }

        public string Type { get; set; }

        [Required]
        public string Key { get; set; }

        public int? LCID { get; set; }

        public string Text { get; set; }
    }
}
//namespace InnateGlory.Data.Abstractions
//{
//    public class Lang
//    {
//        [DbImport]
//        public int LCID;
//        [DbImport]
//        public PlatformId PlatfrmId;
//        [DbImport]
//        public string Key1;
//        [DbImport]
//        public string Key2;
//        [DbImport]
//        public string Text;
//    }
//}
//namespace InnateGlory.Data
//{

//    [TableName("Lang1", Database = _Consts.CoreDB)]
//    public class _Lang : Abstractions.Lang { }
//    [TableName("Lang2", Database = _Consts.CoreDB)]
//    public class _Enum : Abstractions.Lang { }
//}
