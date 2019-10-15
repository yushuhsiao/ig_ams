using System;
using System.Data;
using System.Diagnostics;

namespace InnateGlory.Entity
{
    [DebuggerDisplay("Id : {Id}, Name : {Name}")]
    [TableName("Users")]
    public abstract class UserData : Abstractions.BaseData
    {
        public abstract UserType UserType { get; }

        #region Properties

        public UserId Id { get; set; }

        public CorpId CorpId { get; set; }

        public UserName Name { get; set; }

        public ActiveState Active
        {
            get => Id.IsRoot ? ActiveState.Active : _active;
            set => _active = value;
        }
        private ActiveState _active;

        public UserId ParentId { get; set; }

        public string DisplayName { get; set; }

        public int Depth { get; set; }

        #endregion
    }

    public class Users
    {
        public UserId Id;
        public UserType UserType;
        public CorpId CorpId;
        public string Name;
        public ActiveState Active;
        public UserId ParentId;
        public string DisplayName;
        public int Depth;
        public DateTime CreateTime;
        public UserId CreateUser;
        public DateTime ModifyTime;
        public UserId ModifyUser;
    }

    public class User_Agent
    {
        public UserId Id;
        public int MaxDepth;
        public int MaxAgents;
        public int MaxAdmins;
        public int MaxMembers;
    }

    public class User_Admin
    {
        public UserId Id;
    }

    public class Users_Member
    {
    }
}
