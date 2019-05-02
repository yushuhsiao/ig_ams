using System;

namespace InnateGlory.Messages
{
    [Flags]
    public enum InstanceFlags
    {
        /// <summary>
        /// Create new instance
        /// </summary>
        None,
        /// <summary>
        /// Use same instance
        /// </summary>
        CreateOnce,
        /// <summary>
        /// Get instance from service
        /// </summary>
        FromService,
        /// <summary>
        /// Get instance from service, if service not exist, action will not execute
        /// </summary>
        FromRequiredService
    }
}
