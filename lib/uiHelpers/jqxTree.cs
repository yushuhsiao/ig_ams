using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public enum toggleMode { click, dblclick }
    public class jqxTree : jqxBase
    {
        #region public struct source_item
        public struct source_item
        {
            /// <summary>
            /// sets the item's label.
            /// </summary>
            public string label;
            /// <summary>
            /// sets the item's value.
            /// </summary>
            public object value;
            /// <summary>
            /// item's html. The html to be displayed in the item.
            /// </summary>
            public string html;
            /// <summary>
            /// sets the item's id.
            /// </summary>
            public object id;
            /// <summary>
            /// sets whether the item is enabled/disabled.
            /// </summary>
            public string disabled;
            /// <summary>
            /// sets whether the item is checked/unchecked(when checkboxes are enabled).
            /// </summary>
            public string @checked;
            /// <summary>
            /// sets whether the item is expanded or collapsed.
            /// </summary>
            public string expanded;
            /// <summary>
            /// sets whether the item is selected.
            /// </summary>
            public bool selected;
            /// <summary>
            /// sets an array of sub items.
            /// </summary>
            public List<source_item> items;
            /// <summary>
            /// sets the item's icon(url is expected).
            /// </summary>
            public string icon;
            /// <summary>
            /// sets the size of the item's icon.
            /// </summary>
            public string iconsize;
        }
        #endregion
        #region public struct item
        public struct item
        {
            /// <summary>
            /// gets item's label.
            /// </summary>
            public string label;
            /// <summary>
            /// gets the item's value.
            /// </summary>
            public string value;
            /// <summary>
            /// gets whether the item is enabled/disabled.
            /// </summary>
            public string disabled;
            /// <summary>
            /// gets whether the item is checked/unchecked.
            /// </summary>
            public string @checked;
            /// <summary>
            /// gets the item's LI tag.
            /// </summary>
            public string element;
            /// <summary>
            /// gets the item's parent LI tag.
            /// </summary>
            public string parentElement;
            /// <summary>
            /// gets whether the item is expanded or collapsed.
            /// </summary>
            public string isExpanded;
            /// <summary>
            /// gets whether the item is selected or not.
            /// </summary>
            public string selected;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Sets or gets the animation's easing to one of the JQuery's supported easings.
        /// </summary>
        public string easing
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the duration of the show animation.
        /// (350)
        /// </summary>
        public object animationShowDuration
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the duration of the hide animation.
        /// (fast)
        /// </summary>
        public object animationHideDuration
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the hover state.
        /// </summary>
        public bool enableHover
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the key navigation.
        /// </summary>
        public bool keyboardNavigation
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool enableKeyboardNavigation
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets user interaction used for expanding or collapsing any item.
        /// </summary>
        public toggleMode toggleMode
        {
            get { return _get<toggleMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Specifies the jqxTree's data source. Use this property to populate the jqxTree.
        /// </summary>
        public List<source_item> source
        {
            get { return _get<List<source_item>>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the tree should display a checkbox next to each item. In order to use this feature, you need to include the jqxcheckbox.js.
        /// (false)
        /// </summary>
        public bool checkboxes
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public int checkSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the size of the expand/collapse arrows.
        /// </summary>
        public int toggleIndicatorSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the tree checkboxes have three states - checked, unchecked and indeterminate.
        /// </summary>
        public bool hasThreeStates
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public string touchMode
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables the dragging of Tree Items.(requires jqxdragdrop.js)
        /// (false)
        /// </summary>
        public bool allowDrag
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the allowDrop property.
        /// (false)
        /// </summary>
        public bool allowDrop
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public string searchMode
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the incremental search is enabled. The feature allows you to quickly find and select items by typing when the widget is on focus.
        /// </summary>
        public bool incrementalSearch
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public int incrementalSearchDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public int animationHideDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public bool submitCheckedItems
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called when a drag operation starts.
        /// function(item) { }
        /// </summary>
        public _function dragStart
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the dragEnd property.
        /// function (dragItem, dropItem, args, dropPosition, tree) { }
        /// </summary>
        public _function dragEnd
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public string dropAction
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        #endregion

        #region Events

        /// <summary>
        /// This event is triggered when the user expands a tree item.
        /// </summary>
        public _event OnExpand
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the user collapses a tree item.
        /// </summary>
        public _event OnCollapse
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the user selects a tree item.
        /// </summary>
        public _event OnSelect
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the jqxTree is created and initialized.
        /// </summary>
        public _event OnInitialized
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the user adds a new tree item.
        /// </summary>
        public _event OnAdded
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the user removes a tree item.
        /// </summary>
        public _event OnRemoved
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the user checks, unchecks or the checkbox is in indeterminate state.
        /// </summary>
        public _event OnCheckChange
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the user drops an item.
        /// </summary>
        public _event OnDragEnd
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the user starts a drag operation.
        /// </summary>
        public _event OnDragStart
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the user clicks a tree item.
        /// </summary>
        public _event OnItemClick
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the jqxTree widget. The refresh method will update the jqxTree's layout and size.
        /// </summary>
        public void refresh() { }

        /// <summary>
        /// Ensures the visibility of an element.
        /// </summary>
        /// <param name="item"></param>
        public void ensureVisible(object item) { }

        /// <summary>
        /// Gets an item at specific position. The method expects 2 parameters - left and top. The coordinates are relative to the document.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public object hitTest(int left, int top) { return null; }

        /// <summary>
        /// Adds an item as a sibling of another item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        public void addBefore(object item, string id) { }

        /// <summary>
        /// Adds an item as a sibling of another item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        public void addAfter(object item, string id) { }

        /// <summary>
        /// Adds an item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        public void addTo(object item, string id) { }

        /// <summary>
        /// Updates an item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newItem"></param>
        public void updateItem(object item, object newItem) { }

        /// <summary>
        /// Removes an item.
        /// </summary>
        /// <param name="item"></param>
        public void removeItem(object item) { }

        /// <summary>
        /// Removes all elements.
        /// </summary>
        public void clear() { }

        /// <summary>
        /// Disables a tree item.
        /// </summary>
        /// <param name="item"></param>
        public void disableItem(object item) { }

        /// <summary>
        /// Gets an array with all checked tree items.
        /// </summary>
        /// <returns></returns>
        public item[] getCheckedItems() { return null; }

        /// <summary>
        /// Gets an array with all unchecked tree items.
        /// </summary>
        /// <returns></returns>
        public item[] getUncheckedItems() { return null; }

        /// <summary>
        /// Checks all tree items.
        /// </summary>
        public void checkAll() { }

        /// <summary>
        /// Unchecks all tree items.
        /// </summary>
        public void uncheckAll() { }

        /// <summary>
        /// Checks a tree item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="checked"></param>
        public void checkItem(object item, bool? @checked) { }

        /// <summary>
        /// Unchecks a tree item.
        /// </summary>
        /// <param name="item"></param>
        public void uncheckItem(object item) { }

        /// <summary>
        /// Enables a tree item.
        /// </summary>
        /// <param name="item"></param>
        public void enableItem(object item) { }

        public void enableAll(object item) { }

        public void lockItem(object item) { }

        public void unlockItem(object item) { }

        /// <summary>
        /// Gets an array with all tree items.
        /// </summary>
        /// <returns></returns>
        public item[] getItems() { return null; }

        /// <summary>
        /// Gets the tree item associated to a LI tag passed as parameter. The returned value is an Object.
        /// </summary>
        /// <returns></returns>
        public item getItem() { return default(item); }

        /// <summary>
        /// Gets the item above another item. The returned value is an Object.
        /// </summary>
        /// <returns></returns>
        public item getPrevItem() { return default(item); }

        /// <summary>
        /// Gets the item below another item. The returned value is an Object.
        /// </summary>
        /// <returns></returns>
        public item getNextItem() { return default(item); }

        /// <summary>
        /// Gets the selected tree item.
        /// </summary>
        /// <returns></returns>
        public item getSelectedItem() { return default(item); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string val(string value) { return null; }

        public void clearSelection() { }

        /// <summary>
        /// Selects an item.
        /// </summary>
        /// <param name="item"></param>
        public void selectItem(object item) { }

        /// <summary>
        /// Collapses all items.
        /// </summary>
        public void collapseAll() { }

        /// <summary>
        /// Expandes all items.
        /// </summary>
        public void expandAll() { }

        /// <summary>
        /// Collapses a tree item by passing an element as parameter.
        /// </summary>
        /// <param name="item"></param>
        public void collapseItem(object item) { }

        /// <summary>
        /// Expands a tree item by passing an element as parameter.
        /// </summary>
        /// <param name="item"></param>
        public void expandItem(object item) { }

        /// <summary>
        /// Renders the jqxTree widget.
        /// </summary>
        public void render() { }

        /// <summary>
        /// Sets the focus to the widget.
        /// </summary>
        public void focus() { }

        /// <summary>
        /// Destroy the jqxTree widget. The destroy method removes the jqxTree widget from the web page.
        /// </summary>
        public void destroy() { }

        #endregion
    }
}
