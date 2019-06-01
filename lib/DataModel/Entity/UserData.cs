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
}
