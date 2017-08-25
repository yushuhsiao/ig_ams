using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public class jqxPasswordInput : jqxBase
    {
        /// <summary>
        /// Gets or sets the password input's placeholder text. 
        /// </summary>
        public string placeHolder
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Gets or sets whether a tooltip which shows the password's strength will be shown. 
        /// </summary>
        public bool showStrength
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public enum position { top, bottom, left, right }
        /// <summary>
        /// Gets or sets the position of the tooltip which shows the password strength.
        /// </summary>
        public position showStrengthPosition
        {
            get { return _get<position>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Gets or sets the maximal number of characters in the password. 
        /// </summary>
        public int maxLength
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public int minLength
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Gets or sets whether the Show/Hide password icon will appear. 
        /// </summary>
        public bool showPasswordIcon
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// A callback function for custom rendering of the tooltip which shows the password strength. The function has three parameters: 
        /// strengthTypeRenderer: function (password, characters, defaultStrength)
        /// </summary>
        public _function strengthTypeRenderer
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// A callback function for defining a custom strength rule. The function has three parameters: 
        /// passwordStrength: function (password, characters, defaultStrength)
        /// </summary>
        public _function passwordStrength
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public string changeType
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets the various text values used in the widget. Useful for localization. 
        /// </summary>
        public object localization
        {
            get { return _get<object>(); }
            set { _set(value); }
        }
        /* localization: {
            passwordStrengthString: "Password strength",
            tooShort: "Too short",
            weak: "Weak",
            fair: "Fair",
            good: "Good",
            strong: "Strong",
            showPasswordString: "Show Password"
        } */

        /// <summary>
        /// Sets the the colors used in the tooltip which shows the strength. 
        /// </summary>
        public object strengthColors
        {
            get { return _get<object>(); }
            set { _set(value); }
        }
        /* strengthColors: {
            tooShort: "rgb(170, 0, 51)",
            weak: "rgb(170, 0, 51)",
            fair: "rgb(255, 204, 51)",
            good: "rgb(45, 152, 243)",
            strong: "rgb(118, 194, 97)"
        } */
    }
}