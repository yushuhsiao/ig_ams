// $Id: Func.cs 71137f497bf2 2012/04/16 20:01:27 azizatif $

namespace System
{
    delegate TResult Func< TResult>();
    delegate TResult Func< T,  TResult>(T a);
    delegate TResult Func< T1,  T2,  TResult>(T1 arg1, T2 arg2);
    delegate TResult Func< T1,  T2,  T3,  TResult>(T1 arg1, T2 arg2, T3 arg3);
    delegate TResult Func< T1,  T2,  T3,  T4,  TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
}

