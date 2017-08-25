using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public enum radix
    {
        /// <summary>
        /// specifies the binary numeral system. Allowed characters for this numeral system are the digits and;
        /// </summary>
        binary = 2,
        /// <summary>
        /// specifies the octal numeral system. Allowed characters for this numeral system are the digits from to;
        /// </summary>
        octal = 8,
        /// <summary>
        /// specifies the decimal numeral system. Allowed characters for this numeral system are the digits from to;
        /// </summary>
        @decimal = 10,
        /// <summary>
        /// specifies the hexadecimal numeral system. Allowed characters for this numeral system are the digits from to and letters from to (case insenstive). 
        /// </summary>
        hexadecimal = 16,
    }
    public class jqxFormattedInput : jqxBase
    {
        /// <summary>
        /// Sets or gets the radix of the jqxFormattedInput. The radix specifies the numeral system in which to display the widget's value. 
        /// </summary>
        public radix radix
        {
            get { return _get<radix>(); }
            set { _set(value); }
        }

        public enum _decimalNotation
        {
            /// <summary>
            /// the default representation of decimal numbers, e.g. ; 
            /// </summary>
            @default,
            /// <summary>
            /// representation of decimal numbers in scientific exponential notation(E notation), e.g. .
            /// </summary>
            exponential,
        }
        /// <summary>
        /// Sets or gets the notation in which to display decimal numbers. 
        /// </summary>
        public _decimalNotation decimalNotation
        {
            get { return _get<_decimalNotation>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the value of the jqxFormattedInput widget. The value is in the numeral system specified by the radix property. 
        /// </summary>
        public string value
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the minimum value of the widget. The value of min should be in the same numeral system as value. The min property can be set to no less than '-9223372036854775808' (-2⁶³) in decimal. 
        /// </summary>
        public string min
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the maximum value of the widget. The value of max should be in the same numeral system as value. The max property can be set to no more than '9223372036854775807' (2⁶³ - 1) in decimal. 
        /// </summary>
        public string max
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether to use upper case when the radix property is set to 16 or 'hexadecimal'. 
        /// </summary>
        public bool upperCase
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Shows or hides the spin buttons. 
        /// the spin buttons require an additional empty element in the initialization div of jqxFormattedInput.
        /// </summary>
        public bool spinButtons
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the increase/decrease step. The value of spinButtonsStep is a decimal number. 
        /// </summary>
        public int spinButtonsStep
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the jqxFormattedInput's dropdown (pop-up) will be enabled. The dropdown allows the user to choose the radix (numeral system) of the displayed number. 
        /// the dropdown requires an additional empty element in the initialization div of jqxFormattedInput.
        /// </summary>
        public bool dropDown
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the width of the jqxFormattedInput's dropdown (pop-up). 
        /// </summary>
        public object dropDownWidth
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the pop-up's z-index. 
        /// </summary>
        public int popupZIndex
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxFormattedInput's placeholder. 
        /// </summary>
        public string placeHolder
        {
            get { return _get<string>(); }
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

        public object changeType
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        public template template
        {
            get { return _get<template>(); }
            set { _set(value); }
        }

        public object item
        {
            get { return _get<object>(); }
            set { _set(value); }
        }
    }
}