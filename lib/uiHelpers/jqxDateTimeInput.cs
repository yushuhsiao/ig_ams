using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public enum dayNameFormat { @default, shortest, firstTwoLetters, firstLetter, full, }
    public enum dateSelectionMode { none, @default, range }
    public class jqxCalendar : jqxBase
    {
        /// <summary>
        /// Sets or gets the calendar's restricted dates. These are dates which cannot be clicked. 
        /// </summary>
        public DateTime[] restrictedDates
        {
            get { return _get<DateTime[]>(); }
            set { _set(value); }
        }

        public int multipleMonthRows
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public int multipleMonthColumns
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public DateTime minDate
        {
            get { return _get<DateTime>(); }
            set { _set(value); }
        }

        public DateTime maxDate
        {
            get { return _get<DateTime>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Represents the minimum navigation date. 
        /// </summary>
        public DateTime min
        {
            get { return _get<DateTime>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Represents the maximum navigation date. 
        /// </summary>
        public DateTime max
        {
            get { return _get<DateTime>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the animation delay between switching views. 
        /// </summary>
        public int navigationDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Represents the calendar`s navigation step when the left or right navigation button is clicked. 
        /// </summary>
        public int stepMonths
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the Calendar's value. 
        /// </summary>
        public DateTime value
        {
            get { return _get<DateTime>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets which day to display in the first day column. By default the calendar displays 'Sunday' as first day. 
        /// </summary>
        public int firstDayOfWeek
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value whether the week`s numbers are displayed. 
        /// </summary>
        public bool showWeekNumbers
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value whether the day names are displayed. By default, the day names are displayed. 
        /// </summary>
        public bool showDayNames
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value indicating whether weekend persists its view state. 
        /// </summary>
        public bool enableWeekend
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value indicating whether the other month days are enabled. 
        /// </summary>
        public bool enableOtherMonthDays
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value whether the other month days are displayed. 
        /// </summary>
        public bool showOtherMonthDays
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the row header width. 
        /// </summary>
        public int rowHeaderWidth
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the Calendar colomn header's height. In the column header are displayed the calendar day names. 
        /// </summary>
        public int columnHeaderHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the title height where the navigation arrows are displayed. 
        /// </summary>
        public int titleHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the name format of days of the week. 
        /// </summary>
        public dayNameFormat dayNameFormat
        {
            get { return _get<dayNameFormat>(); }
            set { _set(value); }
        }

        public string monthNameFormat
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the title format for the title section. 
        /// Possible Values: 
        ///   '   d' - the day of the month
        ///   '  dd' - the day of the month
        ///   ' ddd' - the abbreviated name of the day of the week
        ///   'dddd' - the full name of the day of the week
        ///   '   h' - the hour, using a 12-hour clock from 1 to 12
        ///   '  hh' - the hour, using a 12-hour clock from 01 to 12
        ///   '   H' - the hour, using a 24-hour clock from 0 to 23
        ///   '  HH' - the hour, using a 24-hour clock from 00 to 23
        ///   '   m' - the minute, from 0 through 59
        ///   '  mm' - the minutes, from 00 though59
        ///   '   M' - the month, from 1 through 12;
        ///   '  MM' - the month, from 01 through 12
        ///   ' MMM' - the abbreviated name of the month
        ///   'MMMM' - the full name of the month
        ///   '   s' - the second, from 0 through 59
        ///   '  ss' - the second, from 00 through 59
        ///   '   t' - the first character of the AM/PM designator
        ///   '  tt' - the AM/PM designator
        ///   '   y' - the year, from 0 to 99
        ///   '  yy' - the year, from 00 to 99
        ///   ' yyy' - the year, with a minimum of three digits
        ///   'yyyy' - the year as a four-digit number
        /// </summary>
        public string titleFormat
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether switching between month, year and decade views is enabled. 
        /// </summary>
        public bool enableViews
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the calendar in read only state. 
        /// </summary>
        public bool readOnly
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxCalendar's culture. The culture settings are contained within a file with the language code appended to the name, e.g. jquery.glob.de-DE.js for German. To set the culture, you need to include the jquery.glob.de-DE.js and set the culture property to the culture's name, e.g. 'de-DE'. 
        /// </summary>
        public string culture
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value indicating whether the fast navigation is enabled. 
        /// </summary>
        public bool enableFastNavigation
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value indicating whether the hover state is enabled. The hover state is activated when the mouse cursor is over a calendar cell. The hover state is automatically disabled when the calendar is displayed in touch devices. 
        /// </summary>
        public bool enableHover
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value indicating whether the auto navigation is enabled. When this property is true, click on other month date will automatically navigate to the previous or next month. 
        /// </summary>
        public bool enableAutoNavigation
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value indicating whether the tool tips are enabled. 
        /// </summary>
        public bool enableTooltips
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the tooltip text displayed when the mouse cursor is over the back navigation button. 
        /// </summary>
        public string backText
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the tooltip text displayed when the mouse cursor is over the forward navigation button.EnableTooltips property should be set to true. 
        /// </summary>
        public string forwardText
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a special date to the Calendar. 
        /// </summary>
        public DateTime[] specialDates
        {
            get { return _get<DateTime[]>(); }
            set { _set(value); }
        }

        public bool keyboardNavigation
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public dateSelectionMode selectionMode
        {
            get { return _get<dateSelectionMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]
        /// </summary>
        public string[] selectableDays
        {
            get { return _get<string[]>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the 'Today' string displayed when the 'showFooter' property is true. 
        /// </summary>
        public string todayString
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the 'Clear' string displayed when the 'showFooter' property is true. 
        /// </summary>
        public string clearString
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value indicating whether the calendar's footer is displayed. 
        /// </summary>
        public bool showFooter
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public object selection
        {
            get { return _get<object>(); }
            set { _set(value); }
        }
        /* selection: {
            from: null,
            to: null
        }*/

        public bool canRender
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public string view
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// ["month", "year", "decade"]
        /// </summary>
        public string[] views
        {
            get { return _get<string[]>(); }
            set { _set(value); }
        }

        public object changing
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        public object change
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        public object localization
        {
            get { return _get<object>(); }
            set { _set(value); }
        }
        /* localization: {
            backString: "Back",
            forwardString: "Forward",
            todayString: "Today",
            clearString: "Clear",
            calendar: {
                name: "Gregorian_USEnglish",
                "/": "/",
                ":": ":",
                firstDay: 0,
                days: {
                    names: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
                    namesAbbr: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
                    namesShort: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"]
                },
                months: {
                    names: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", ""],
                    namesAbbr: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", ""]
                },
                AM: ["AM", "am", "AM"],
                PM: ["PM", "pm", "PM"],
                eras: [{
                    name: "A.D.",
                    start: null,
                    offset: 0
                }],
                twoDigitYearMax: 2029,
                patterns: {
                    d: "M/d/yyyy",
                    D: "dddd, MMMM dd, yyyy",
                    t: "h:mm tt",
                    T: "h:mm:ss tt",
                    f: "dddd, MMMM dd, yyyy h:mm tt",
                    F: "dddd, MMMM dd, yyyy h:mm:ss tt",
                    M: "MMMM dd",
                    Y: "yyyy MMMM",
                    S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss",
                    ISO: "yyyy-MM-dd hh:mm:ss"
                }
            }
        } */
    }
    public class jqxDateTimeInput : jqxBase
    {
        /// <summary>
        /// Sets or gets the jqxDateTimeInput value. 
        /// </summary>
        public DateTime value
        {
            get { return _get<DateTime>(); }
            set { _set(value); }
        }

        public DateTime minDate
        {
            get { return _get<DateTime>(); }
            set { _set(value); }
        }

        public DateTime maxDate
        {
            get { return _get<DateTime>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDateTimeInput's minumun date. 
        /// </summary>
        public DateTime min
        {
            get { return _get<DateTime>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDateTimeInput's maximum date. 
        /// </summary>
        public DateTime max
        {
            get { return _get<DateTime>(); }
            set { _set(value); }
        }

        public int rowHeaderWidth
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public bool enableViews
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// ["month", "year", "decade"]
        /// </summary>
        public string[] views
        {
            get { return _get<string[]>(); }
            set { _set(value); }
        }

        /// <summary>
        /// ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]
        /// </summary>
        public string[] selectableDays
        {
            get { return _get<string[]>(); }
            set { _set(value); }
        }

        public object change
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        public object changing
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        public template template
        {
            get { return _get<template>(); }
            set { _set(value); }
        }

        public int columnHeaderHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public int titleHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets which day to display in the first day column. By default the calendar displays 'Sunday' as first day. 
        /// </summary>
        public int firstDayOfWeek
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value whether the week`s numbers are displayed. 
        /// </summary>
        public bool showWeekNumbers
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the time button is visible. 
        /// </summary>
        public bool showTimeButton
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool cookies
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public object cookieoptions
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a value indicating whether the dropdown calendar's footer is displayed. 
        /// </summary>
        public bool showFooter
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the date time input format of the date. 
        /// 
        /// Possible Values: 
        ///   'd'    - the day of the month
        ///   'dd'   - the day of the month
        ///   'ddd'  - the abbreviated name of the day of the week
        ///   'dddd' - the full name of the day of the week
        ///   'h'    - the hour, using a 12-hour clock from 1 to 12
        ///   'hh'   - the hour, using a 12-hour clock from 01 to 12
        ///   'H'    - the hour, using a 24-hour clock from 0 to 23
        ///   'HH'   - the hour, using a 24-hour clock from 00 to 23
        ///   'm'    - the minute, from 0 through 59
        ///   'mm'   - the minutes, from 00 though59
        ///   'M'    - the month, from 1 through 12;
        ///   'MM'   - the month, from 01 through 12
        ///   'MMM'  - the abbreviated name of the month
        ///   'MMMM' - the full name of the month
        ///   's'    - the second, from 0 through 59
        ///   'ss'   - the second, from 00 through 59
        ///   't'    - the first character of the AM/PM designator
        ///   'tt'   - the AM/PM designator
        ///   'y'    - the year, from 0 to 99
        ///   'yy'   - the year, from 00 to 99
        ///   'yyy'  - the year, with a minimum of three digits
        ///   'yyyy' - the year as a four-digit number
        /// </summary>
        public string formatString
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public dayNameFormat dayNameFormat
        {
            get { return _get<dayNameFormat>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the position of the text. 
        /// </summary>
        public horizontalAlignment textAlign
        {
            get { return _get<horizontalAlignment>(); }
            set { _set(value); }
        }

        public bool @readonly
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDateTimeInput's culture. The culture settings are contained within a file with the language code appended to the name, e.g. jquery.glob.de-DE.js for German. To set the culture, you need to include the jquery.glob.de-DE.js and set the culture property to the culture's name, e.g. 'de-DE'. 
        /// </summary>
        public string culture
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public object activeEditor
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the calendar button is visible. 
        /// </summary>
        public bool showCalendarButton
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Specifies the animation duration of the popup calendar when it is going to be displayed. 
        /// </summary>
        public int openDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Specifies the animation duration of the popup calendar when it is going to be hidden. 
        /// </summary>
        public int closeDelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether or not the popup calendar must be closed after selection. 
        /// </summary>
        public bool closeCalendarAfterSelection
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool isEditing
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// When this property is set to true, the popup calendar may open above the input, if there's not enough space below the DateTimeInput. 
        /// </summary>
        public bool enableBrowserBoundsDetection
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets the DropDown's alignment. 
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
        /// This setting enables the user to select only one symbol at a time when typing into the text input field. 
        /// </summary>
        public bool enableAbsoluteSelection
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public int buttonSize
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

        public string dropDownWidth
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public string dropDownHeight
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public DateTime[] restrictedDates
        {
            get { return _get<DateTime[]>(); }
            set { _set(value); }
        }

        public dateSelectionMode selectionMode
        {
            get { return _get<dateSelectionMode>(); }
            set { _set(value); }
        }

        public string renderMode
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public object timeRange
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the 'Today' string displayed in the dropdown Calendar when the 'showFooter' property is true. 
        /// </summary>
        public string todayString
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the 'Clear' string displayed when the 'showFooter' property is true. 
        /// </summary>
        public string clearString
        {
            get { return _get<string>(); }
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
        /// Determines whether Null is allowed as a value. 
        /// </summary>
        public bool allowNullDate
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public object changeType
        {
            get { return _get<object>(); }
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

        public bool enableHover
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether Backspace and Delete keys are handled by the widget. 
        /// </summary>
        public bool allowKeyboardDelete
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public object localization
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }
        /* localization: {
    backString: "Back",
    forwardString: "Forward",
    todayString: "Today",
    clearString: "Clear",
    calendar: {
        name: "Gregorian_USEnglish",
        "/": "/",
        ":": ":",
        firstDay: 0,
        days: {
            names: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
            namesAbbr: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
            namesShort: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"]
        },
        months: {
            names: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", ""],
            namesAbbr: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", ""]
        },
        AM: ["AM", "am", "AM"],
        PM: ["PM", "pm", "PM"],
        eras: [{
            name: "A.D.",
            start: null,
            offset: 0
        }],
        twoDigitYearMax: 2029,
        patterns: {
            d: "M/d/yyyy",
            D: "dddd, MMMM dd, yyyy",
            t: "h:mm tt",
            T: "h:mm:ss tt",
            f: "dddd, MMMM dd, yyyy h:mm tt",
            F: "dddd, MMMM dd, yyyy h:mm:ss tt",
            M: "MMMM dd",
            Y: "yyyy MMMM",
            S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss",
            ISO: "yyyy-MM-dd hh:mm:ss"
        }
    }
} */
    }
}