namespace InnateGlory
{
    public enum PlatformActiveState : sbyte
    {
        /// <summary>
        /// 永久停用
        /// </summary>
        Disabled = ActiveState.Disabled,
        /// <summary>
        /// 啟用
        /// </summary>
        Active = ActiveState.Active,
        /// <summary>
        /// 維護中
        /// </summary>
        Maintenance = 2,
    }
}
