namespace System
{
    using System.Runtime.CompilerServices;

    public delegate TResult Func<in T, out TResult>(T a);
}

