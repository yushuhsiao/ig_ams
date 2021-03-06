using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;

namespace InnateGlory.Entity
{
    [TableName("Corps", Database = _Consts.db.CoreDB, SortKey = nameof(Id))]
    [DebuggerDisplay("Id : {Id}, Name : {Name}")]
    public class CorpInfo : Abstractions.BaseData
    {
        public CorpId Id { get; set; }
        public UserName Name { get; set; }

        public ActiveState Active
        {
            get => Id.IsRoot ? ActiveState.Active : _active;
            set => _active = value;
        }
        //[DbImport(nameof(Active))]
        private ActiveState _active = ActiveState.Disabled;
        public string DisplayName { get; set; }
        public CurrencyCode Currency { get; set; }
    }
}
namespace InnateGlory.Models
{
    [TableName(typeof(Entity.CorpInfo))]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CorpModel
    {
        [JsonProperty]
        public CorpId? Id { get; set; }
        [JsonProperty]
        public UserName Name { get; set; }
        [JsonProperty]
        public ActiveState? Active { get; set; }
        [JsonProperty]
        public string DisplayName { get; set; }
        [JsonProperty]
        public CurrencyCode? Currency { get; set; }

        [JsonProperty]
        public DbConnectionString UserDB_R { get; set; }
        [JsonProperty]
        public DbConnectionString UserDB_W { get; set; }
        [JsonProperty]
        public DbConnectionString LogDB_R { get; set; }
        [JsonProperty]
        public DbConnectionString LogDB_W { get; set; }
    }
}