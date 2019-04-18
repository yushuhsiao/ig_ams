namespace InnateGlory.Messages
{
    public sealed class ServerCommands
    {
        /// <summary> samples </summary>
        /// 
        // <see cref="SqlConfig.PurgeCache"/>.
        /// 
        /// 
        /// 
        private readonly ServerInfo _this;

        internal ServerCommands(ServerInfo info)
        {
            this._this = info;
        }

        public void PurgeCache2(params string[] cacheTypes)
        {
            //_this.SendMessage(nameof(PurgeCache), cacheTypes);
        }
        public void PurgeCache(params string[] cacheTypes)
        {
            //_this.SendMessage(nameof(PurgeCache), new { cacheTypes = cacheTypes });
        }
    }
}
