using System.Data;
using System.Diagnostics;

namespace InnateGlory.Entity
{
    [DebuggerDisplay("Id : {Id}, Name : {Name}")]
    public abstract class UserData : Abstractions.BaseData
    {
        public abstract UserType UserType { get; }

        #region Properties

        [DbImport]
        public virtual UserId Id { get; set; }

        [DbImport]
        public virtual CorpId CorpId { get; set; }

        [DbImport]
        public virtual UserName Name { get; set; }

        [DbImport]
        public ActiveState Active
        {
            get => Id.IsRoot ? ActiveState.Active : _Active;
            set => _Active = value;
        }
        private ActiveState _Active;

        [DbImport]
        public virtual UserId ParentId { get; set; }

        [DbImport]
        public virtual string DisplayName { get; set; }

        [DbImport]
        public virtual int Depth { get; set; }

        #endregion
    }
}
