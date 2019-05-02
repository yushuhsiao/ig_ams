using System.Data;

namespace InnateGlory.Entity
{
    [TableName("AclDefine", Database = _Consts.db.CoreDB)]
    public class AclDefine
    {
        [DbImport]
        public int Id { get; set; }
        [DbImport]
        public string Path { get; set; }
        [DbImport]
        public int Flag { get; set; }
        [DbImport]
        public AclFlags DefaultFlags { get; set; }

        public bool CheckParentLevel { get; set; }
        public bool CheckParentUser { get; set; }
    }

    [TableName("UserAcl", Database = _Consts.db.UserDB)]
    public class UserAcl
    {
        [DbImport]
        public int AclId { get; set; }

        [DbImport]
        public UserId UserId { get; set; }

        [DbImport]
        public AclFlags Flags { get; set; }

        //public int GetState() => this.State ?? this.GetDefine().DefaultState;
    }

    // 跨站台存取權限控制
    [TableName("UserAclDelegate", Database = _Consts.db.UserDB)]
    public class UserAclDelegate
    {
        [DbImport]
        public CorpId CorpId;

        [DbImport]
        public int AclId;

        [DbImport]
        public UserId UserId;
    }
}
