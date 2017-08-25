//------------------------------------------------------------------------------
// <copyright file="SystemInfo.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------

namespace System.Web.Util {

    internal static class SystemInfo {
        static int  _trueNumberOfProcessors;

		static internal int GetNumProcessCPUs()
		{
			return 0;
        }
    }
}
