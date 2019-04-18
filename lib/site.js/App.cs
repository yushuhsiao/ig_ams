using AngularJS;
using Bridge;
using System;
[assembly: OutputBy(OutputBy.Module)]
//[assembly: Module("ams")]

namespace InnateGlory
{
    public class xxx
    {
        public static void ng_init(string module_name, string controller_name, string function)
        {
            Angular.Module(module_name).Controller(controller_name, function);
            //return new xxx();
        }
        //
        //public static void Start([Name("$scope")] AppScope scope, [Name("$http")] Http http)
        //{
        //}
    }

    //[Module("site")]
    //public class xx
    //{
    //}
    //public class bb
    //{
    //}
    public class TestPage
    {
        //[Init(InitPosition.After)]
        //public static void Init()
        //{
        //    Angular.Module("app").Controller<AppScope, Http>("main", Start);
        //}




        [Name("$scope")]
        AppScope scope;

        [Name("$http")]
        Http http;

        private void Test1()
        {
            Console.WriteLine("123");
        }
    }

    public class AppScope
    {
        public Action test1;
    }

}