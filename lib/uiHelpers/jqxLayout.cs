using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public class jqxLayout : jqxBase
    {
        public abstract class panel
        {
        }

        #region Properties

        /// <summary>
        /// Sets the default minimumn height for groups which are vertically aligned within their parent group.
        /// </summary>
        public int minGroupHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets the default minimumn width for groups which are horizontally aligned within their parent group.
        /// </summary>
        public int minGroupWidth
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the layout. This property determines the position of the layout elements and their characteristics. The layout array always contains one root item of type 'layoutGroup'.
        /// </summary>
        public object[] layout
        {
            get { return _get<object[]>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the resizable property.
        /// </summary>
        public bool resizable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets wheter a custom context menu will appear when certain elements of the widget are right-clicked.
        /// </summary>
        public bool contextMenu
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        #endregion

        #region Events

        public _event OnCreate
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        public _event OnResize
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        public _event OnPin
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        public _event OnUnpin
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        #endregion
    }

    public class jqxDockingLayout : jqxLayout
    {
        #region Events

        public _event OnFloat
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        public _event OnDock
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        #endregion
    }
}
