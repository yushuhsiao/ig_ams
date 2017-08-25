using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public enum inputMode { simple, advanced }
    public class jqxInput : jqxBase
    {
        /// <summary>
        /// Sets or gets the jqxInput's dropDown width. 
        /// </summary>
        public int dropDownWidth
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables you to update the input's value, after a selection from the auto-complete popup. 
        /// renderer: function (itemValue, inputValue)
        /// </summary>
        public _function renderer
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value indicating whether the auto-suggest popup is opened.
        /// </summary>
        public bool opened
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets the widget's data source. The 'source' function is passed two arguments, the input field's value and a callback function. The 'source' function may be used synchronously by returning an array of items or asynchronously via the callback. 
        /// source: function (query, response)
        /// </summary>
        public object source
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        public bool roundedCorners
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the search mode. When the user types into the edit field, the jqxInput widget tries to find the searched item using the entered text and the selected search mode. 
        /// </summary>
        public searchMode searchMode
        {
            get { return _get<searchMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the input's place holder. 
        /// </summary>
        public string placeHolder
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public string value
        {
            get { return _get<string>(); }
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

        /// <summary>
        /// Sets or gets the auto-suggest popup's z-index. 
        /// </summary>
        public int popupZIndex
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the maximum number of items to display in the popup menu. 
        /// </summary>
        public int items
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the minimum character length needed before triggering auto-complete suggestions. 
        /// </summary>
        public int minLength
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the maximum character length of the input. 
        /// </summary>
        public int maxLength
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the input's query. 
        /// </summary>
        public string query
        {
            get { return _get<string>(); }
            set { _set(value); }
        }
    }
}