using System;
using System.Reflection;

namespace Tools
{
    static class util
    {
#if NETSTANDARD1_0
#elif NETSTANDARD1_1
#elif PORTABLE40
#elif PORTABLE
#elif DOTNET
#elif NET20
#elif NET35
#elif NET40
#else
#endif

#if NET20 // .NET Framework 2.0
#elif NET35 // .NET Framework 3.5
#elif NET40 // .NET Framework 4.0
#elif NET45 // .NET Framework 4.5
#elif NET451 // .NET Framework 4.5.1
#elif NET452 // .NET Framework 4.5.2
#elif NET46 // .NET Framework 4.6
#elif NET461 // .NET Framework 4.6.1
#elif NET462 // .NET Framework 4.6.2
#elif NETSTANDARD1_0 // .NET Standard 1.0 
#elif NETSTANDARD1_1 // .NET Standard 1.1
#elif NETSTANDARD1_2 // .NET Standard 1.2    
#elif NETSTANDARD1_3 // .NET Standard 1.3    
#elif NETSTANDARD1_4 // .NET Standard 1.4    
#elif NETSTANDARD1_5 // .NET Standard 1.5    
#elif NETSTANDARD1_6 // .NET Standard 1.6    
#elif NETSTANDARD2_0
#elif NETCOREAPP1_0
#elif NETCOREAPP2_0
#endif
    }
}