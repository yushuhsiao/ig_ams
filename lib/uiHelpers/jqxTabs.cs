using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jqx
{
    public enum scrollPosition { left, right, both }
    public enum tabsToggleMode { click, dblclick, mouseenter, none, }
    public class jqxTabs : jqxBase
    {
        /// <summary>
        /// Sets or gets the duration of the scroll animation. 
        /// </summary>
        public int scrollAnimationDuration
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the enabledHover property. 
        /// </summary>
        public bool enabledHover
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the collapsible feature. 
        /// </summary>
        public bool collapsible
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the animation type of switching tabs. 
        /// </summary>
        public animationType animationType
        {
            get { return _get<animationType>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the scroll animation is enabled. 
        /// </summary>
        public bool enableScrollAnimation
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the duration of the content's fade animation which occurs when the user selects a tab. This setting has effect when the 'animationType' is set to 'fade'. 
        /// </summary>
        public int contentTransitionDuration
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets user interaction used for switching the different tabs.
        /// </summary>
        public tabsToggleMode toggleMode
        {
            get { return _get<tabsToggleMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets selected tab. 
        /// </summary>
        public int selectedItem
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the tabs are positioned at 'top' or 'bottom. 
        /// </summary>
        public verticalAlignment position
        {
            get { return _get<verticalAlignment>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the selection tracker is enabled. 
        /// </summary>
        public bool selectionTracker
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the scrolling is enabled. 
        /// </summary>
        public bool scrollable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the position of the scroll arrows. 
        /// </summary>
        public scrollPosition scrollPosition
        {
            get { return _get<scrollPosition>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the scrolling step. 
        /// </summary>
        public int scrollStep
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the jqxTabs header's height will be equal to the item with max height. 
        /// </summary>
        public bool autoHeight
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public object headerHeight
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether a close button is displayed in each tab. 
        /// </summary>
        public bool showCloseButtons
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool canCloseAllTabs
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the close button size. 
        /// </summary>
        public int closeButtonSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public int arrowButtonSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the keyboard navigation. 
        /// </summary>
        public bool keyboardNavigation
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the reorder feature. When this feature is enabled, the end-user can drag a tab and drop it over another tab. As a result the tabs will be reordered. 
        /// </summary>
        public bool reorder
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public int selectionTrackerAnimationDuration
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public bool roundedCorners
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool isCollapsed
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function that the tab calls when a content panel needs to be initialized. 
        /// function (tab)
        /// </summary>
        public _function initTabContent
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }
    }
}