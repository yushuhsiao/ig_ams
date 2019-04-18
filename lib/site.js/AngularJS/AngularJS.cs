using Bridge;
using System;

namespace AngularJS
{
    [External]
    [Name("angular")]
    public class Angular
    {
        [Template("{this}.module({name}, [])")]
        public static Module Module(string name)
        {
            return default(Module);
        }
    }

    [External]
    public class Module
    {
        [Template("{this}.controller({name}, {function})")]
        public void Controller(string name, string function) { }

        [Template("{this}.controller({name}, {function})")]
        public void Controller<T>(string name, Action<T> function) { }

        [Template("{this}.controller({name}, {function})")]
        public void Controller<T1, T2>(string name, Action<T1, T2> function) { }

        [Template("{this}.controller({name}, {function})")]
        public void Controller<T1, T2, T3>(string name, Action<T1, T2, T3> function) { }

        [Template("{this}.controller({name}, {function})")]
        public void Controller<T1, T2, T3, T4>(string name, Action<T1, T2, T3, T4> function) { }

        [Template("{this}.controller({name}, {function})")]
        public void Controller<T1, T2, T3, T4, T5>(string name, Action<T1, T2, T3, T4, T5> function) { }
    }

    public class _Scope
    {
    }

    public class Http
    {
    }
}