using Microsoft.AspNetCore.Mvc;
using System;

namespace InnateGlory.WebSockets
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class WebSocketActionAttribute : Attribute, INonAction
    {
        //private NonActionAttribute nonAction;
    }
}