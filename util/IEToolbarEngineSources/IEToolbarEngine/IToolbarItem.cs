using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace IEToolbarEngine
{
    /// <summary>
    /// Element type
    /// </summary>
    public enum ToolbarItemType
    {
        Unknown = 0,
        Link,
        LinkList,
        RssTicker,
        SearchBox,
        Widget,
        MainMenu,
    };

    /// <summary>
    /// Element interface.
    /// </summary>
    public interface IToolbarItem : IDisposable
    {
        /// <summary>
        /// Visibylity.
        /// </summary>
        bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Type identifier
        /// </summary>
        ToolbarItemType TypeID
        {
            get;            
        }

        /// <summary>
        /// State clearing.
        /// </summary>
        void Reset ();
    }    
}
