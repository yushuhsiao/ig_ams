using Newtonsoft.Json;
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
        [DbImport]
        public string Type;
        [DbImport]
        public string Key;
        [DbImport]
        public int LCID;
        [DbImport]
        public string Text;

        //public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
namespace InnateGlory.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public struct LangModel
    {
        [JsonProperty]
        public PlatformId? PlatformId { get; set; }
        [JsonProperty]
        public string Path { get; set; }
        [JsonProperty]
        public string Type { get; set; }
        [JsonProperty]
        public string Key { get; set; }
        [JsonProperty]
        public int? LCID { get; set; }
        [JsonProperty]
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
