using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jqx
{
    public class jqxComboBox : jqxBase
    {
        public List<object> source
        {
            get { return _get<List<object>>(); }
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
        /// Sets or gets whether items will wrap when they reach the width of the dropDown.
        /// </summary>
        public bool autoItemsHeight
        {
            get { return _get<bool>(); }
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
        /// Sets or gets the height of the jqxComboBox Items. When the itemHeight == - 1, each item's height is equal to its desired height.
        /// </summary>
        public int itemHeight
        {
            get { return _get<int>(); }
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
        /// Sets the delay of the 'close' animation.
        /// </summary>
        public int closeDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets the type of the animation.
        /// </summary>
        public animationType animationType
        {
            get { return _get<animationType>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the width of the jqxComboBox's ListBox displayed in the widget's DropDown.
        /// </summary>
        public int dropDownWidth
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the jqxComboBox's ListBox displayed in the widget's DropDown.
        /// </summary>
        public int dropDownHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the height of the jqxComboBox's ListBox displayed in the widget's DropDown is calculated as a sum of the items heights.
        /// </summary>
        public bool autoDropDownHeight
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
        /// Sets or gets the item search mode. When the user types into the edit field, the jqxComboBox widget tries to find the searched item using the entered text and the selected search mode. 
        /// </summary>
        public searchMode searchMode
        {
            get { return _get<searchMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the whether the 'autoComplete' feature is enabled or disabled. When this feature is enabled, the jqxComboBox displays in the popup listbox, only the items that match the searched text.
        /// </summary>
        public bool autoComplete
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the items displayed in the popup come from a remote data source. When this property is set to true, the jqxComboBox calls the 'search' callback function when the user types into the input field. 
        /// </summary>
        public bool remoteAutoComplete
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the delay between two keystrokes. The search callback function is called on timeout. The value is specified in milliseconds. 
        /// </summary>
        public int remoteAutoCompleteDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the minimum number of characters that need to be entered by the user for search in remote data source when remoteAutoComplete property is set to true. 
        /// </summary>
        public int minLength
        {
            get { return _get<int>(); }
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

        public bool keyboardSelection
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called when an item is rendered. By using the renderer function, you can customize the look of the list items. 
        /// function (index, label, value)
        /// </summary>
        public _function renderer
        {
            get { return _get<_function>(); }
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
        /// Determines the button's template as an alternative of the default styles. 
        /// </summary>
        public template template
        {
            get { return _get<template>(); }
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
        /// Sets or gets the input field's place holder. 
        /// </summary>
        public string placeHolder
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether an item can be selected in multi-select mode. 
        /// function(itemValue)
        /// </summary>
        public _function validateSelection
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the jqxComboBox shows the items close buttons in multi-select mode. 
        /// </summary>
        public bool showCloseButtons
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called when the selected item is rendered. By using the renderSelectedItem function, you can customize the displayed text in the ComboBox's input field. 
        /// function(index, item)
        /// </summary>
        public _function renderSelectedItem
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called when the 'remoteAutoComplate' property is set to true and the user types into the ComboBox's input field. 
        /// function (searchString)
        /// </summary>
        public _function search
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

        /// <summary>
        /// Determines whether the jqxComboBox is in multi-select mode. 
        /// </summary>
        public bool multiSelect
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the jqxComboBox shows its dropdown button. 
        /// </summary>
        public bool showArrow
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool autoBind
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }
    }
}