using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public class jqxNumberInput : jqxBase
    {
        public string value
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the input's number. 
        /// </summary>
        public int @decimal
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the input's minimum value. 
        /// </summary>
        public int? min
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the input's maximum value. 
        /// </summary>
        public int? max
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public string validationMessage
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the alignment. 
        /// </summary>
        public horizontalAlignment textAlign
        {
            get { return _get<horizontalAlignment>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the readOnly state of the input. 
        /// </summary>
        public bool readOnly
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the prompt char displayed when an editable char is empty. 
        /// Possible Values:  '_', '?', ';', '#'
        /// </summary>
        public string promptChar
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Indicates the number of decimal places to use in numeric values. 
        /// </summary>
        public int decimalDigits
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the char to use as the decimal separator in numeric values. 
        /// </summary>
        public char decimalSeparator
        {
            get { return _get<char>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the string that separates groups of digits to the left of the decimal in numeric values. 
        /// </summary>
        public char groupSeparator
        {
            get { return _get<char>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the number of digits in each group to the left of the decimal in numeric values. 
        /// </summary>
        public int groupSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the string to use as currency or percentage symbol. 
        /// </summary>
        public string symbol
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the position of the symbol in the input. 
        /// </summary>
        public horizontalAlignment symbolPosition
        {
            get { return _get<horizontalAlignment>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the digits in the input 
        /// </summary>
        public int digits
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public bool negative
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the string to use as negative symbol. 
        /// </summary>
        public string negativeSymbol
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the input's mode. 
        /// </summary>
        public inputMode inputMode
        {
            get { return _get<inputMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Shows or hides the spin buttons. 
        /// </summary>
        public bool spinButtons
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the width of the spin buttons. 
        /// </summary>
        public int spinButtonsWidth
        {
            get { return _get<int>(); }
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

        public bool autoValidate
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the spin mode. 
        /// </summary>
        public inputMode spinMode
        {
            get { return _get<inputMode>(); }
            set { _set(value); }
        }

        public bool enableMouseWheel
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the widget's value could be null. 
        /// </summary>
        public bool allowNull
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the widget's place holder displayed when the widget's value is null. 
        /// </summary>
        public string placeHolder
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public template template
        {
            get { return _get<template>(); }
            set { _set(value); }
        }
    }
}