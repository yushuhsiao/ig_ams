using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jqx
{
    public enum position { left, top, center, bottom, right, topLeft, bottomLeft, topRight, bottomRight }
    public enum textImageRelation { imageBeforeText, imageAboveText, textAboveImage, textBeforeImage, overlay }
    public enum roundedCorners
    {
        /// <summary>
        /// for all corners
        /// </summary>
        all,
        /// <summary>
        /// for top corners
        /// </summary>
        top,
        /// <summary>
        /// for bottom corners
        /// </summary>
        bottom,
        /// <summary>
        /// for left corners
        /// </summary>
        left,
        /// <summary>
        /// for right corners
        /// </summary>
        right,
        /// <summary>
        /// for top right corners
        /// </summary>
        top_right,
        /// <summary>
        /// for top left corners
        /// </summary>
        top_left,
        /// <summary>
        /// for bottom right corners
        /// </summary>
        bottom_right,
        /// <summary>
        /// for bottom left corners
        /// </summary>
        bottom_left,
    }

    public class jqxButton : jqxBase
    {
        protected override string ToHtmlString(string name, object value)
        {
            if (value is roundedCorners)
            {
                string s = value.ToString().Replace("_", "-");
            }
            return base.ToHtmlString(name, value);
        }

        public string cursor
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the rounded corners functionality. This property setting has effect in browsers which support CSS border-radius. 
        /// </summary>
        public roundedCorners roundedCorners
        {
            get { return _get<roundedCorners>(); }
            set { _set(value); }
        }

        public bool enableHover
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool enableDefault
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool enablePressed
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the button's image position. Possible values: "left", "top", "center", "bottom", "right", "topLeft", "bottomLeft", "topRight", "bottomRight". 
        /// </summary>
        public position imgPosition
        {
            get { return _get<position>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the button's image source. 
        /// </summary>
        public string imgSrc
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the button's image width. 
        /// </summary>
        public int imgWidth
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the button's image height. 
        /// </summary>
        public int imgHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the button's value. 
        /// </summary>
        public string value
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the button's text position. Possible values: "left", "top", "center", "bottom", "right", "topLeft", "bottomLeft", "topRight", "bottomRight". 
        /// </summary>
        public position textPosition
        {
            get { return _get<position>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the button's text image relation. Possible values: "imageBeforeText", "imageAboveText", "textAboveImage", "textBeforeImage" and "overlay". 
        /// </summary>
        public textImageRelation textImageRelation
        {
            get { return _get<textImageRelation>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the button's template as an alternative of the default styles. 
        /// </summary>
        public template template
        {
            get { return _get<template>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the button's toggle state. ( available in jqxToggleButton ). 
        /// </summary>
        public bool toggled
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }
    }

    public class jqxLinkButton : jqxBase
    {
        public string href
        {
            get { return _get<string>(); }
            set { _set(value); }
        }
    }

    public class jqxRepeatButton : jqxButton
    {
        /// <summary>
        /// Specifies the interval between two Click events. This property is available only in the jqxRepeatButton. The jqxRepeatButton raises Click events repeatedly when the button is pressed. 
        /// </summary>
        public int delay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }
    }

    public class jqxToggleButton : jqxButton
    {
        public bool uiToggle
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }
    }

    public class jqxDropDownButton : jqxBase
    {
        public int arrowSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public bool enableHover
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the delay of the 'open' animation. 
        /// </summary>
        public int openDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the delay of the 'close' animation. 
        /// </summary>
        public int closeDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the type of the animation. 
        /// </summary>
        public animationType animationType
        {
            get { return _get<animationType>(); }
            set { _set(value); }
        }

        /// <summary>
        /// When this property is set to true, the popup may open above the button, if there's not enough available space below the button. 
        /// </summary>
        public bool enableBrowserBoundsDetection
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the DropDown's alignment. 
        /// </summary>
        public horizontalAlignment dropDownHorizontalAlignment
        {
            get { return _get<horizontalAlignment>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the DropDown's alignment. 
        /// </summary>
        public verticalAlignment dropDownVerticalAlignment
        {
            get { return _get<verticalAlignment>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the popup's z-index. 
        /// </summary>
        public int popupZIndex
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the DropDown is automatically opened when the mouse cursor is moved over the widget. 
        /// </summary>
        public bool autoOpen
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets a function which initializes the button's content. 
        /// initContent: function() 
        /// </summary>
        public _function initContent
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public object dropDownWidth
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        public object dropDownHeight
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        public bool focusable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public template template
        {
            get { return _get<template>(); }
            set { _set(value); }
        }
    }

    public class jqxCheckBox : jqxBase
    {
        /// <summary>
        /// Sets or gets the delay of the fade animation when the CheckBox is going to be checked. 
        /// </summary>
        public int animationShowDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the delay of the fade animation when the CheckBox is going to be unchecked. 
        /// </summary>
        public int animationHideDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the checkbox's size. 
        /// </summary>
        public string boxSize
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the check state. 
        /// Possible Values:(when the hasThreeStates property value is true)
        /// 'true', 'false', 'null'
        /// </summary>
        public bool? @checked
        {
            get { return _get<bool?>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the checkbox has 3 states - checked, unchecked and indeterminate. 
        /// </summary>
        public bool hasThreeStates
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the clicks on the container are handled as clicks on the check box. 
        /// </summary>
        public bool enableContainerClick
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the checkbox is locked. In this mode the user is not allowed to check/uncheck the checkbox. 
        /// </summary>
        public bool locked
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the group name. When this property is set, the checkboxes in the same group behave as radio buttons. 
        /// </summary>
        public string groupName
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public bool keyboardCheck
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool enableHover
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool hasInput
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public object updated
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        public bool disabledContainer
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public object changeType
        {
            get { return _get<object>(); }
            set { _set(value); }
        }
    }

    public class jqxRadioButton : jqxBase
    {
        public int animationShowDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public int animationHideDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public string boxSize
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public bool? @checked
        {
            get { return _get<bool?>(); }
            set { _set(value); }
        }

        public bool hasThreeStates
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool enableContainerClick
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool locked
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public string groupName
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public object changeType
        {
            get { return _get<object>(); }
            set { _set(value); }
        }
    }

    public class jqxSwitchButton : jqxBase
    {
        /// <summary>
        /// Sets or gets the check state. 
        /// </summary>
        public bool @checked
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the string displayed when the button is checked. 
        /// </summary>
        public string onLabel
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the string displayed when the button is unchecked. 
        /// </summary>
        public bool offLabel
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public enum _toggleMode { @default, click, slide }

        public _toggleMode toggleMode
        {
            get { return _get<_toggleMode>(); }
            set { _set(value); }
        }

        public int animationDuration
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public bool animationEnabled
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the size of the thumb in percentages. 
        /// </summary>
        public string thumbSize
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the jqxSwitchButton's orientation. 
        /// </summary>
        public orientation orientation
        {
            get { return _get<orientation>(); }
            set { _set(value); }
        }

        public string switchRatio
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public bool metroMode
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }
    }

    public class jqxButtonGroup : jqxBase
    {
        public enum _mode { checkbox, radio, @default }

        /// <summary>
        /// Sets or gets the jqxButtonGroup's mode.
        /// </summary>
        public string mode
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public bool roundedCorners
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool enableHover
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public orientation orientation
        {
            get { return _get<orientation>(); }
            set { _set(value); }
        }

        public template template
        {
            get { return _get<template>(); }
            set { _set(value); }
        }
    }
}