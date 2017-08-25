using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jqx
{
    public class jqxLoader : jqxBase
    {
        /// <summary>
        /// Sets or gets whether the loader will be shown after it's creation.
        /// </summary>
        public bool autoOpen
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets the loader's content.
        /// </summary>
        public string html
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the loader is displayed as a modal dialog. If the jqxLoader's mode is set to modal, the loader blocks user interaction with the underlying user interface.
        /// </summary>
        public bool isModal
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the image's position. Possible values: 'top', 'bottom' and 'center'
        /// </summary>
        public verticalPosition imagePosition
        {
            get { return _get<verticalPosition>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the loader's text.
        /// </summary>
        public string text
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the alignment.
        /// </summary>
        public alignment textPosition
        {
            get { return _get<alignment>(); }
            set { _set(value); }
        }
    }
}