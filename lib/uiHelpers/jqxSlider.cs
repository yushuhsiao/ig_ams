using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public enum jqxSlider_layout { normal, reverse }
    public enum jqxSlider_mode { @default, @fixed }
    public class jqxSlider : jqxBase
    {
        /// <summary>
        /// Sets or gets scroll buttons position.
        /// </summary>
        public scrollPosition buttonsPosition
        {
            get { return _get<scrollPosition>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the slider's layout.
        /// </summary>
        public jqxSlider_layout layout
        {
            get { return _get<jqxSlider_layout>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets slider's mode.
        /// </summary>
        public jqxSlider_mode mode
        {
            get { return _get<jqxSlider_mode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets slider's minor ticks frequency.
        /// </summary>
        public int minorTicksFrequency
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the minorTickSize property.
        /// </summary>
        public int minorTickSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the max property.
        /// </summary>
        public int max
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets slider's minimum value.
        /// </summary>
        public int min
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the slider is displayed as a range slider and has 2 thumbs. This allows the user to select a range of values. By default, end-users can select only a single value.
        /// </summary>
        public bool rangeSlider
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the slider's step when the user is using the keyboard arrows, slider increment and decrement buttons or the mouse wheel for changing the slider's value.
        /// </summary>
        public int step
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether ticks will be shown.
        /// </summary>
        public bool showTicks
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether minor ticks will be shown.
        /// </summary>
        public bool showMinorTicks
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether major tick labels will be shown.
        /// </summary>
        public bool showTickLabels
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the scroll buttons will be shown.
        /// </summary>
        public bool showButtons
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the slider range background is displayed. This is the fill between the slider's left button and the slider's thumb to indicate the selected value. In range slider mode, the space between the handles is filled to indicate the selected values.
        /// </summary>
        public bool showRange
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public template template
        {
            get { return _get<template>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets slider's ticks position.
        /// </summary>
        public verticalPosition ticksPosition
        {
            get { return _get<verticalPosition>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets slider's major ticks frequency.
        /// </summary>
        public int ticksFrequency
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets slider's major ticks size.
        /// </summary>
        public int tickSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the major ticks labels formatting function.
        /// tickLabelFormatFunction: function (value) { }
        /// </summary>
        public _function tickLabelFormatFunction
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets slider's value. This poperty will be an object with the following structure { rangeStart: range_start, rangeEnd: range_end } if the slider is range slider otherwise it's going to be a number.
        /// </summary>
        public int value
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets range slider's values.The 'rangeSlider' property should be set to true.
        /// </summary>
        public int[] values
        {
            get { return _get<int[]>(); }
            set { _set(value); }
        }
    }
}
