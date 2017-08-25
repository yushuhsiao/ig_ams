using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jqx
{
    public enum animationType { fade, slide, none }
    public enum template
    {
        /// <summary>
        /// the default template. The style depends only on the "theme" property value.
        /// </summary>
        @default,
        /// <summary>
        /// dark blue style for extra visual weight.
        /// </summary>
        primary,
        /// <summary>
        /// green style for successful or positive action.
        /// </summary>
        success,
        /// <summary>
        /// orange style which indicates caution.
        /// </summary>
        warning,
        /// <summary>
        /// red style which indicates a dangerous or negative action.
        /// </summary>
        danger,
        /// <summary>
        /// blue button, not tied to a semantic action or use.
        /// </summary>
        info,
        /// <summary>
        ///  dark gray button, not tied to a semantic action or use.
        /// </summary>
        inverse,
        /// <summary>
        ///  making it look like a link .
        /// </summary>
        link,
    }

    public class jqxDropDownList : jqxBase
    {
        #region public struct source_item
        public struct source_item
        {
            /// <summary>
            /// determines the item's label.
            /// </summary>
            public string label;
            /// <summary>
            /// determines the item's value.
            /// </summary>
            public object value;
            /// <summary>
            /// sets whether the item is enabled/disabled.
            /// </summary>
            public bool? disabled;
            /// <summary>
            /// determines whether the item is enabled/disabled.
            /// </summary>
            public bool? @checked;
            /// <summary>
            /// determines whether the item's checkbox supports three states
            /// </summary>
            public bool? hasThreeStates;
            /// <summary>
            /// determines the item's display html. This can be used instead of label.
            /// </summary>
            public string html;
            /// <summary>
            /// determines the item's group.
            /// </summary>
            public string group;
        }
        #endregion

        public object source
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the selected index. 
        /// </summary>
        public int selectedIndex
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the scrollbars size. 
        /// </summary>
        public int scrollBarSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public int arrowSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables/disables the hover state. 
        /// </summary>
        public bool enableHover
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables/disables the selection. 
        /// </summary>
        public bool enableSelection
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether items will wrap when they reach the width of the dropDown.
        /// </summary>
        public bool autoItemsHeight
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool equalItemsWidth
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the jqxDropDownList Items. When the itemHeight == - 1, each item's height is equal to its desired height. 
        /// </summary>
        public int itemHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether checkboxes will be displayed next to the list items. (The feature requires jqxcheckbox.js) 
        /// </summary>
        public bool checkboxes
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the delay of the 'open' animation. 
        /// </summary>
        public int openDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the delay of the 'close' animation. 
        /// </summary>
        public int closeDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the type of the animation. 
        /// </summary>
        public animationType animationType
        {
            get { return _get<animationType>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the DropDown is automatically opened when the mouse cursor is moved over the widget.
        /// </summary>
        public bool autoOpen
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the width of the jqxDropDownList's ListBox displayed in the widget's DropDown. 
        /// </summary>
        public object dropDownWidth
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the jqxDropDownList's ListBox displayed in the widget's DropDown. 
        /// </summary>
        public object dropDownHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the height of the jqxDropDownList's ListBox displayed in the widget's DropDown is calculated as a sum of the items heights. 
        /// </summary>
        public bool autoDropDownHeight
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool keyboardSelection
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the dropdown detects the browser window's bounds and automatically adjusts the dropdown's position. 
        /// </summary>
        public bool enableBrowserBoundsDetection
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the DropDown's alignment. 
        /// </summary>
        public horizontalAlignment dropDownHorizontalAlignment
        {
            get { return _get<horizontalAlignment>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the DropDown's alignment. 
        /// </summary>
        public verticalAlignment dropDownVerticalAlignment
        {
            get { return _get<verticalAlignment>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the displayMember of the Items. The displayMember specifies the name of an object property to display. The name is contained in the collection specified by the 'source' property. 
        /// </summary>
        public string displayMember
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the valueMember of the Items. The valueMember specifies the name of an object property to set as a 'value' of the list items. The name is contained in the collection specified by the 'source' property. 
        /// </summary>
        public string valueMember
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public string groupMember
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public string searchMember
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the item incremental search mode. When the user types some text in a focused DropDownList, the jqxListBox widget tries to find the searched item using the entered text and the selected search mode. 
        /// </summary>
        public searchMode searchMode
        {
            get { return _get<searchMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the incrementalSearch property. An incremental search begins searching as soon as you type the first character of the search string. As you type in the search string, jqxDropDownList automatically selects the found item. 
        /// </summary>
        public bool incrementalSearch
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the incrementalSearchDelay property. The incrementalSearchDelay specifies the time-interval in milliseconds after which the previous search string is deleted. The timer starts when you stop typing. 
        /// </summary>
        public int incrementalSearchDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called when an item is rendered. By using the renderer function, you can customize the look of the list items. 
        /// </summary>
        public _function renderer
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Text displayed when the selection is empty. 
        /// </summary>
        public string placeHolder
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called when the selected item is rendered in the jqxDropDownList's content area. By using the selectionRenderer function, you can customize the look of the selected item. 
        /// </summary>
        public _function selectionRenderer
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the popup's z-index. 
        /// </summary>
        public int popupZIndex
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public bool autoBind
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool focusable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the Filtering is enabled. 
        /// </summary>
        public bool filterable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the Filter's height. 
        /// </summary>
        public int filterHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the Filter input's place holder. 
        /// </summary>
        public string filterPlaceHolder
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the Filter's delay. After 100 milliseconds, the widget automatically filters its data based on the filter input's value. To perform filter only on "Enter" key press, set this property to 0. 
        /// </summary>
        public int filterDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the template as an alternative of the default styles. 
        /// </summary>
        public template template
        {
            get { return _get<template>(); }
            set { _set(value); }
        }
    }
}