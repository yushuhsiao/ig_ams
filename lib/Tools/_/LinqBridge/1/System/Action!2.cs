﻿namespace System
{
    using System.Runtime.CompilerServices;

    public delegate void Action<in T1, in T2>(T1 arg1, T2 arg2);
}

