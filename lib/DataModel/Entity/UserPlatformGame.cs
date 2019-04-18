using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory.Entity
{
    // GameId == 0, UserId == 0     Corp-Platform
    // GameId != 0, UserId == 0     Corp-Platform-Game
    // GameId == 0, UserId != 0     Agent-Platform / Member-Platform
    // GameId != 0, UserId != 0     Agent-Platform-Game / Member-Platform-Game
    //[TableName("UserPlatformGame", Database = _Consts.UserDB)]
    //public class UserPlatformGame
    //{
    //    [DbImport]
    //    public CorpId CorpId;
    //    [DbImport]
    //    public PlatformId PlatformId;
    //    [DbImport]
    //    public GameId GameId;
    //    [DbImport]
    //    public UserId UserId;
    //    [DbImport]
    //    public short Flags;
    //}

    [TableName("CorpGame", Database = _Consts.db.UserDB)]
    public class CorpGame
    {
        public CorpId CorpId;
        public GameId GameId;
    }

    [TableName("CorpPlatform", Database = _Consts.db.UserDB)]
    public class CorpPlatform
    {
        public CorpId CorpId;
        public PlatformId PlatformId;
    }

    [TableName("AgentGame", Database = _Consts.db.UserDB)]
    public class AgentGame
    {
        public UserId AgentId;
        public GameId GameId;
    }

    [TableName("AgentPlatform", Database = _Consts.db.UserDB)]
    public class AgentPlatform
    {
        public UserId AgentId;
        public PlatformId PlatformId;
    }

    [TableName("MemberGame", Database = _Consts.db.UserDB)]
    public class MemberGame
    {
        public UserId MemberId;
        public GameId GameId;
    }

    [TableName("MemberPlatform", Database = _Consts.db.UserDB)]
    public class MemberPlatform
    {
        public UserId MemberId;
        public PlatformId PlatformId;
    }
}
