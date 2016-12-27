using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public enum orientation { vertical, horizontal }
    public class jqxSplitter : jqxBase
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class panel : _jqxBase
        {
            //public panel(int index)
            //{
            //    this.index = index;
            //}

            //protected override void TagName(StringBuilder s)
            //{
            //    base.TagName(s, typeof(jqxSplitter), this.GetType());
            //}

            [JsonProperty]
            public object size
            {
                get { return _get<object>(); }
                set { _set(value); }
            }
            [JsonProperty]
            public object min
            {
                get { return _get<object>(); }
                set { _set(value); }
            }
            [JsonProperty]
            public bool collapsible
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            [JsonProperty]
            public bool collapsed
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
        }

        //string name_panels;



        #region Properties

        /// <summary>
        /// Sets or gets the panels property.
        /// </summary>
        panel[] panels
        {
            get { return _get<panel[]>(create: () => new panel[] { new panel(), new panel() }); }
            set { _set(value); }
        }
        //panel[] _panels = new panel[] { new panel(), new panel() };
        public panel panel1
        {
            get { return this.panels[0]; }
            set { _set(value); }
        }
        public panel panel2
        {
            get { return this.panels[1]; }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the orientation property.
        /// </summary>
        public orientation orientation
        {
            get { return _get<orientation>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the size of the split bar.
        /// </summary>
        public int splitBarSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the resizable property. When this property is set to false, the user will not be able to move the split bar.
        /// </summary>
        public bool resizable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the split bar is displayed.
        /// </summary>
        public bool showSplitBar
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        #endregion

        #region Events

        /// <summary>
        /// This event is triggered when the 'resize' operation has ended.
        /// </summary>
        public _event OnResize
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a panel is expanded.
        /// </summary>
        public _event OnExpanded
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a panel is collapsed.
        /// </summary>
        public _event OnCollapsed
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the 'resizeStart' operation has started.
        /// </summary>
        public _event OnResizeStart
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        public _event OnLayout
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        #endregion
    }
}