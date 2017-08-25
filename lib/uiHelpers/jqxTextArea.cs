using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public class jqxTextArea : jqxBase
    {
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
        /// Sets or gets the jqxTextArea's dropdown (pop-up) width.
        /// </summary>
        public int dropDownWidth
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
        /// Sets or gets the maximum character length of the textarea.
        /// </summary>
        public int maxLength
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
        /// Gets a value indicating whether the auto-suggest popup is opened.
        /// </summary>
        public bool opened
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the textarea's placeholder.
        /// </summary>
        public string placeHolder
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
        /// Determines the textarea's query.
        /// </summary>
        public string query
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables you to update the textarea's value, after a selection from the auto-complete popup.
        /// </summary>
        public _function renderer
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the rounded corners functionality. This property setting has effect in browsers which support CSS border-radius.
        /// </summary>
        public bool roundedCorners
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the size of the scrollbar.
        /// </summary>
        public int scrollBarSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the search mode. When the user types into the textarea, the widget tries to find the searched item using the entered text and the selected search mode.
        /// </summary>
        public searchMode searchMode
        {
            get { return _get<searchMode>(); }
            set { _set(value); }
        }
    }
}
