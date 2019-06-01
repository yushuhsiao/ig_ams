using System.Data;

namespace InnateGlory.Entity
{
    [TableName("AclDefine", Database = _Consts.db.CoreDB)]
    public class AclDefine
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int Flag { get; set; }
        public AclFlags DefaultFlags { get; set; }

        public bool CheckParentLevel { get; set; }
        public bool CheckParentUser { get; set; }
    }

    [TableName("UserAcl", Database = _Consts.db.UserDB)]
    public class UserAcl
    {
        public int AclId { get; set; }
        public UserId UserId { get; set; }
        public AclFlags Flags { get; set; }

        //public int GetState() => this.State ?? this.GetDefine().DefaultState;
    }

    // 跨站台存取權限控制
    [TableName("UserAclDelegate", Database = _Consts.db.UserDB)]
    public class UserAclDelegate
    {
        public CorpId CorpId;
        public int AclId;
        public UserId UserId;
    }
}
