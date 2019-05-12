// $Id: Action.cs 71137f497bf2 2012/04/16 20:01:27 azizatif $

namespace System
{
    delegate void Action();
    delegate void Action< T1,  T2>(T1 arg1, T2 arg2);
    delegate void Action< T1,  T2,  T3>(T1 arg1, T2 arg2, T3 arg3);
    delegate void Action< T1,  T2,  T3,  T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
}

