using Bridge;
using System;
using static Retyped.webix.webix2;
using webix = Retyped.webix.webix2;

namespace Retyped
{
    public static class _webix
    {
        [ObjectLiteral]
        public class formEventHash : EventHash
        {
            [Name(nameof(ui.formEventName.onSubmit))]
            public Action<ui.form, Bridge.Html5.KeyboardEvent> onSubmit { get; set; }
        }

        [ObjectLiteral]
        public class contextmenuEventHash : EventHash
        {
            [Name(nameof(ui.contextmenuEventName.onItemClick))]
            public Action<string, MouseEvents, dom.HTMLElement> onItemClick { get; set; }
        }
    }
}


//using Bridge;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using number = System.Int32;
//using function = System.Object;

//[External]
//public class webix
//{
//    public class ui
//    {
//        [Template("new webix.ui({xxx})")]
//        public static T create<T>(T obj) => obj;

//        public class baseview
//        {
//        }

//        [ObjectLiteral]
//        public class form : baseview
//        {
//            public string view { get; set; }
//            public string container { get; set; }
//            public Union<bool, object> animate { get; set; }

//            /// <summary>
//            /// adds a new view to a layout-like widget
//            /// </summary>
//            /// <param name="view">the configuration of the new view</param>
//            /// <param name="index">the index a new view will be added at</param>
//            /// <returns>the id of the new view</returns>
//            public Union<string, number> addView(object view, number index) => null;

//            /// <summary>
//            /// adjusts the component to the size of the parent HTML container
//            /// </summary>
//            public void adjust() { }

//            /// <summary>
//            /// attaches the handler to an inner event of the component (allows behavior customizations)
//            /// </summary>
//            /// <param name="type">the event name, case-insensitive</param>
//            /// <param name="functor">the function object or name</param>
//            /// <param name="id">optional, the event id</param>
//            /// <returns>the id of the attached event handler</returns>
//            public string attachEvent(string type, function functor, string id = null) => null;

//            /// <summary>
//            /// binds components
//            /// </summary>
//            /// <param name="target">an object that binds to the calling component</param>
//            /// <param name="rule">optional, sets the rule according which components will be bound</param>
//            /// <param name="format">optional, the format of bound data</param>
//            public void bind(object target, function rule = null, string format = null) { }
//        }
//    }
//}