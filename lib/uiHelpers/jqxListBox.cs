using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jqx
{
    public enum dropAction { @default, copy, none }
    public enum searchMode { none, contains, containsignorecase, equals, equalsignorecase, startswithignorecase, startswith, endswithignorecase, endswith, }


    public class jqxListBox : jqxBase
    {
        /// <summary>
        /// Enables/disables the multiple selection. When this property is set to true, the user can select multiple items.
        /// </summary>
        public bool multiple
        {
            get { return _get<bool>(); }
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
        /// Sets or gets the item's source.
        /// </summary>
        public object source
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the scrollBarSize property.
        /// </summary>
        public int scrollBarSize
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
        /// Sets or gets whether the items width should be equal to the listbox's width.
        /// </summary>
        public bool equalItemsWidth
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the jqxListBox Items. When the itemHeight == - 1, each item's height is equal to its desired height.
        /// </summary>
        public int itemHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the listbox should display a checkbox next to each item.
        /// </summary>
        public bool checkboxes
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the checkboxes have three states - checked, unchecked and indeterminate.
        /// </summary>
        public bool hasThreeStates
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the listbox's height is equal to the sum of each item's height
        /// </summary>
        public bool autoHeight
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool autoItemsHeight
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool roundedcorners
        {
            get { return _get<bool>(); }
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

        public string groupMember
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

        public string searchMember
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the item incremental search mode. When the user types some text in a focused ListBox, the jqxListBox widget tries to find the searched item using the entered text and the selected search mode.
        /// </summary>
        public searchMode searchMode
        {
            get { return _get<searchMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the incrementalSearch property. An incremental search begins searching as soon as you type the first character of the search string. As you type in the search string, jqxListBox automatically selects the found item.
        /// </summary>
        public bool incrementalSearch
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets ot gets the incrementalSearchDelay property. The incrementalSearchDelay specifies the time-interval in milliseconds after which the previous search string is deleted. The timer starts when you stop typing.
        /// </summary>
        public int incrementalSearchDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public int incrementalSearchKeyDownDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables/disables the dragging of ListBox Items.
        /// </summary>
        public bool allowDrag
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables/disables the dropping of ListBox Items.
        /// </summary>
        public bool allowDrop
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the drop action when an item is dropped.
        /// </summary>
        public dropAction dropAction
        {
            get { return _get<dropAction>(); }
            set { _set(value); }
        }

        public bool keyboardNavigation
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool enableMouseWheel
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables/disables the multiple selection using Shift and Ctrl keys. When this property is set to true, the user can select multiple items by clicking on item and holding Shift or Ctrl.
        /// </summary>
        public bool multipleextended
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public _function rendered
        {
            get { return _get<_function>(); }
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
        /// Callback function which is called when a drag operation starts.
        /// function (item)
        /// </summary>
        public _function dragStart
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called when a drag operation ends.
        /// function (dragItem, dropItem) 
        /// </summary>
        public _function dragEnd
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public bool focusable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public _function ready
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public bool autoBind
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

        public _function filterChange
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }
    }
}