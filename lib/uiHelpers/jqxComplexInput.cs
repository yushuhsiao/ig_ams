using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public class jqxComplexInput : jqxBase
    {
        public enum _decimalNotation
        {
            /// <summary>
            /// decimal notation, e.g. '330000 - 200i'
            /// </summary>
            @default,
            /// <summary>
            /// e.g. '3.3e+5 - 2e+2i'
            /// </summary>
            exponential,
            /// <summary>
            /// e.g. '3.3×10⁵ - 2×10²i'
            /// </summary>
            scientific,
            /// <summary>
            /// e.g. '330×10³ - 200×10⁰i'
            /// </summary>
            engineering,
        }
        /// <summary>
        /// Sets or gets the notation in which to display the real and imaginary parts of the complex number. 
        /// </summary>
        public _decimalNotation decimalNotation
        {
            get { return _get<_decimalNotation>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the value of the jqxComplexInput widget. 
        /// </summary>
        public string value
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Shows or hides the spin buttons. 
        /// the spin buttons require an additional empty div element in the initialization div of jqxComplexInput.
        /// </summary>
        public bool spinButtons
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the increase/decrease step. 
        /// </summary>
        public int spinButtonsStep
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxComplexInput's placeholder. 
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
    }
}