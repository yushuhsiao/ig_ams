using ams;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace jqx
{
    public enum datatype { xml, json, jsonp, tsv, csv, local, array, observablearray }
    //public enum sortOrder { asc, desc }
    public enum everpresentrowposition { top, bottom, topAboveFilterRow }
    public enum rowactions { add, reset, update, delete }
    public enum rowactionsmode { popup, buttons, columns }
    public enum filtermode { @default, excel }
    public enum pagermode { simple, @default }
    public enum columnPropertyName { text, hidden, hideable, renderer, cellsrenderer, align, cellsalign, cellsformat, pinned, contenttype, resizable, filterable, editable, cellclassname, classname, width, minwidth, maxwidth }
    #region public enum selectionmode
    public enum selectionmode
    {
        /// <summary>
        /// disables the selection
        /// </summary>
        none,
        /// <summary>
        /// full row selection
        /// </summary>
        singlerow,
        /// <summary>
        /// each click selects a new row. Click on a selected row unselects it
        /// </summary>
        multiplerows,
        /// <summary>
        /// multiple rows selection with drag and drop. The selection behavior resembles the selection of icons on your desktop
        /// </summary>
        multiplerowsextended,
        /// <summary>
        /// single cell selection
        /// </summary>
        singlecell,
        /// <summary>
        /// each click selects a new cell. Click on a selected cell unselects it
        /// </summary>
        multiplecells,
        /// <summary>
        /// in this mode, users can select multiple cells with a drag and drop. The selection behavior resembles the selection of icons on your desktop
        /// </summary>
        multiplecellsextended,
        /// <summary>
        /// this mode is the most advanced cells selection mode. In this mode, users can select multiple cells with a drag and drop. The selection behavior resembles the selection of cells in a spreadsheet
        /// </summary>
        multiplecellsadvanced,
        /// <summary>
        /// multiple rows selection through a checkbox.
        /// </summary>
        checkbox,
    }
    #endregion
    #region public enum editmode
    public enum editmode
    {
        /// <summary>
        /// Marks the clicked cell as selected and shows the editor. The editor’s value is equal to the cell’s value
        /// </summary>
        click,
        /// <summary>
        /// Marks the cell as selected. A second click on the selected cell shows the editor. The editor’s value is equal to the cell’s value
        /// </summary>
        selectedcell,
        /// <summary>
        /// A second click on a selected row shows the row editors.
        /// </summary>
        selectedrow,
        /// <summary>
        /// Marks the clicked cell as selected and shows the editor. The editor’s value is equal to the cell’s value
        /// </summary>
        dblclick,
        /// <summary>
        /// Cell editors are activated and deactivated only through the API(see begincelledit and endcelledit methods)
        /// </summary>
        programmatic
    }
    #endregion
    #region public enum scrollmode
    public enum scrollmode
    {
        @default,
        /// <summary>
        /// the movement of the scrollbar thumb is by row, not by pixel
        /// </summary>
        logical,
        /// <summary>
        /// content is stationary when the user drags the Thumb of a ScrollBar
        /// </summary>
        deferred
    }
    #endregion
    #region public enum columntype
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum columntype
    {
        /// <summary>
        /// readonly column with numbers.
        /// </summary>
        number,
        /// <summary>
        /// readonly checkbox when the editing is disabled. Checkbox input when editing is enabled.
        /// threestatecheckbox - determines whether the checkbox has an indeterminate state when the value is null. The default value is false.
        /// </summary>
        checkbox,
        /// <summary>
        /// sets a number input editor as a default editor for the column. Requires: jqxnumberinput.js
        /// </summary>
        numberinput,
        /// <summary>
        /// sets a dropdownlist editor as a default editor for the column. Requires: jqxlistbox.js and jqxdropdownlist.js
        /// </summary>
        dropdownlist,
        /// <summary>
        /// sets a combobox editor as a default editor for the column. Requires: jqxlistbox.js and jqxcombobox.js
        /// </summary>
        combobox,
        /// <summary>
        /// sets a datetimeinput editor as a default editor for the column. Requires: jquery.global.js, jqxcalendar.js and jqxdatetimeinput.js
        /// </summary>
        datetimeinput,
        /// <summary>
        /// sets a textbox editor as a default editor for the column.
        /// </summary>
        textbox,
        /// <summary>
        /// sets a custom editor as a default editor for the column. The editor should be created in the "createeditor" callback. The editor should be synchronized with the cell's value in the "initeditor" callback. The editor's value should be retrieved in the "geteditorvalue" callback.
        /// </summary>
        template,
        /// <summary>
        /// sets a custom editor as a default editor for a cell. That setting enables you to have multiple editors in a Grid column. The editors should be created in the "createeditor" callback - it is called for each row when the "columntype=custom". The editors should be synchronized with the cell's value in the "initeditor" callback. The editor's value should be retrieved in the "geteditorvalue" callback.
        /// </summary>
        custom,
    }
    #endregion
    #region public enum filtertype
    public enum filtertype
    {
        /// <summary>
        /// basic text field.
        /// </summary>
        textbox,
        /// <summary>
        /// input field with dropdownlist for choosing the filter condition. *Only when "showfilterrow" is true.
        /// </summary>
        input,
        /// <summary>
        /// dropdownlist with checkboxes that specify which records should be visible and hidden.
        /// </summary>
        checkedlist,
        /// <summary>
        /// dropdownlist which specifies the visible records depending on the selection.
        /// </summary>
        list,
        /// <summary>
        /// numeric input field. * Only when "showfilterrow" is true.
        /// </summary>
        number,
        /// <summary>
        /// filter for boolean data. *Only when "showfilterrow" is true.
        /// </summary>
        checkbox,
        /// <summary>
        /// filter for dates.
        /// </summary>
        date,
        /// <summary>
        /// filter for date ranges. *Only when "showfilterrow" is true.
        /// </summary>
        range,
        /// <summary>
        /// allows you to create custom filter menu widgets. *Only when "showfilterrow" is false.
        /// </summary>
        custom
    }
    #endregion
    public enum dataFieldType { _string, _date__, _number, _bool__, _object, }

    public partial class jqxGrid : _jqxBase, IHtmlString, jqxGrid.IColumns
    {
        public jqxGrid()
        {
            this.scope_id = "grid1";
            //this.scope_prop = "sel_row";
        }

        public Formatting JsonFormatting = Formatting.None;

        string IHtmlString.ToHtmlString() => json.SerializeObject(_values, quoteName: true, quoteChar: '\'', formatting: JsonFormatting);

        #region pager
        __pager _pager;
        public __pager pager
        {
            get { return _pager = __pager._inst(_pager, this); }
            set { _pager = value; }
        }
        public class __pager : ___groups<__pager>
        {
            protected override void init()
            {
                this.pageable = true;
                this.server_paging = true;
            }

            /// <summary>
            /// Enables or disables the Grid Paging feature.When the value of this property is true, the Grid displays a pager below the rows.
            /// </summary>
            public bool pageable
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Set the pagesizeoptions property.
            /// </summary>
            public int[] pagesizeoptions
            {
                get { return _get<int[]>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Sets or gets the rendering mode of the pager.Available values - "simple" and "default".
            /// </summary>
            public pagermode pagermode
            {
                get { return _get<pagermode>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Sets or gets the buttons displayed in the pager when the "pagermode" is set to "simple".
            /// </summary>
            public int pagerbuttonscount
            {
                get { return _get<int>(); }
                set { _set(value); }
            }

            /// <summary>
            /// The function is called when the Grid Pager is rendered.This allows you to customize the default rendering of the pager.
            /// function ()
            /// </summary>
            public _function pagerrenderer
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Sets or gets the height of the Grid Pager.
            /// </summary>
            public int pagerheight
            {
                get { return _get<int>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Sets or gets the number of visible rows per page when the Grid paging is enabled.
            /// </summary>
            public int pagesize
            {
                get { return _get<int>(); }
                set { _set(value); }
            }

            public bool server_paging
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
        }
        #endregion

        #region filter
        __filter _filter;
        public __filter filter
        {
            get { return _filter = __filter._inst(_filter, this); }
            set { _filter = value; }
        }
        public class __filter : ___groups<__filter>
        {
            /// <summary>
            /// Enables or disables the Grid Filtering feature.When the value of this property is true, the Grid displays a filtering panel in the columns popup menus.
            /// </summary>
            public bool filterable
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            public bool server_filtering
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Displays the filter icon only when the column is filtered.When the value of this property is set to false, all grid columns will display a filter icon when the filtering is enabled.
            /// </summary>
            public bool autoshowfiltericon
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Sets or gets the filter row's height.
            /// </summary>
            public int filterrowheight
            {
                get { return _get<int>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Set the filtermode property.
            /// </summary>
            public filtermode filtermode
            {
                get { return _get<filtermode>(); }
                set { _set(value); }
            }

            /// <summary>
            /// When this property is true, the Grid adds an additional visual style to the grid cells in the filter column(s).
            /// </summary>
            public bool showfiltercolumnbackground
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Determines whether to display the filtering items in the column's menu.
            /// </summary>
            public bool showfiltermenuitems
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Shows or hides the filter row.
            /// </summary>
            public bool showfilterrow
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
        }
        #endregion

        #region sorting
        __sorting _sorting;
        public __sorting sorting
        {
            get { return _sorting = __sorting._inst(_sorting, this); }
            set { _sorting = value; }
        }
        public class __sorting : ___groups<__sorting>
        {
            /// <summary>
            /// The sortable property enables or disables the sorting feature.
            /// </summary>
            public bool sortable
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }

            /// <summary>
            /// When this property is true, the Grid adds an additional visual style to the grid cells in the sort column.
            /// </summary>
            public bool showsortcolumnbackground
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Determines whether to display the sort menu items.
            /// </summary>
            public bool showsortmenuitems
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Sets the sort toggle states.
            /// 
            /// '0'-disables toggling
            /// '1'-enables togging.Click on a column toggles the sort direction
            /// '2'-enables remove sorting option
            /// 
            /// </summary>
            public string sorttogglestates
            {
                get { return _get<string>(); }
                set { _set(value); }
            }

            public bool server_sorting
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
        }
        #endregion

        #region everpresentrow
        __everpresentrow _everpresentrow;
        public __everpresentrow everpresentrow
        {
            get { return _everpresentrow = __everpresentrow._inst(_everpresentrow, this); }
            set { _everpresentrow = value; }
        }
        public class __everpresentrow : ___groups<__everpresentrow>
        {
            /// <summary>
            /// Set the everpresentrowposition property.
            /// </summary>
            public everpresentrowposition everpresentrowposition
            {
                get { return _get<everpresentrowposition>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Set the everpresentrowheight property.
            /// </summary>
            public int everpresentrowheight
            {
                get { return _get<int>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Set the everpresentrowactions property.
            /// </summary>
            public rowactions everpresentrowactions
            {
                get { return _get<rowactions>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Sets the actions display mode. By default they are displayed in a popup. You can set the property to "columns" and define columns with datafields - addButtonColumn, resetButtonColumn, updateButtonColumn and deleteButtonColumn to display the actions in columns.
            /// </summary>
            public rowactionsmode everpresentrowactionsmode
            {
                get { return _get<rowactionsmode>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Shows or hides an additional row in jqxGrid which allows you to easily add new rows.
            /// </summary>
            public bool showeverpresentrow
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
        }
        #endregion

        #region rowdetail
        __rowdetail _rowdetail;
        public __rowdetail rowdetail
        {
            get { return _rowdetail = __rowdetail._inst(_rowdetail, this); }
            set { _rowdetail = value; }
        }
        public class __rowdetail : ___groups<__rowdetail>
        {
            /// <summary>
            /// Enables or disables the row details. When this option is enabled, the Grid can show additional information below each grid row.
            /// </summary>
            public bool rowdetails
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }

            /// <summary>
            /// This function is called when the user expands the row details and the details are going to be rendered.
            /// function (index, parentElement, gridElement, datarecord)
            /// </summary>
            public _function initrowdetails
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            public bool multi_rowdetails
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Shows an additional column with expand/collapse toggle buttons when the Row details feature is enabled.
            /// </summary>
            public bool showrowdetailscolumn
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Determines the template of the row details. The rowdetails field specifies the HTML used for details. The rowdetailsheight specifies the height of the details.
            /// </summary>
            public rowdetailstemplate rowdetailstemplate
            {
                get { return _get(create: () => new rowdetailstemplate()); }
                set { _set(value); }
            }

            public bool rowdetailshidden
            {
                get { return rowdetailstemplate.rowdetailshidden; }
                set { rowdetailstemplate.rowdetailshidden = value; }
            }
            public int rowdetailsheight
            {
                get { return rowdetailstemplate.rowdetailsheight; }
                set { rowdetailstemplate.rowdetailsheight = value; }
            }
            public string rowdetails_html
            {
                get { return rowdetailstemplate.rowdetails; }
                set { rowdetailstemplate.rowdetails = value; }
            }
        }
        #endregion

        public class rowdetailstemplate : _jqxBase
        {
            public bool rowdetailshidden
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            public int rowdetailsheight
            {
                get { return _get<int>(); }
                set { _set(value); }
            }
            public string rowdetails
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
        }

        public class ___groups<T> where T : ___groups<T>, new()
        {
            protected jqxGrid grid;
            internal static T _inst(T src, jqxGrid grid)
            {
                if (src == null)
                {
                    src = new T() { grid = grid };
                }
                return src;
            }

            protected virtual void init() { }

            protected T2 _get<T2>([CallerMemberName] string name = null, Func<T2> create = null)
            {
                return grid._get<T2>(name, create);
            }
            protected void _set<T2>(T2 value, [CallerMemberName] string name = null, bool quote = true)
            {
                grid._set<T2>(value, name, quote);
            }
        }

        #region WebViewPage

        public interface IColumns { jqxGrid._column this[string name] { get; } }
        public IColumns Columns { get { return this; } }
        jqxGrid._column IColumns.this[string name]
        {
            get { return this.GetColumn(name); }
        }

        //public string scope_id = "grid1";
        public string scope_id
        {
            get { return _get<string>(); }
            set { _set(value); }
        }
        public _WebViewPage page;
        public jqxGrid._column GetColumn(string name)
        {
            name = name.Trim();
            foreach (jqxGrid._column c in this.columns)
                if (c.name == name)
                    return c;
            return null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Enables or disables the alternating rows.
        /// </summary>
        public bool altrows
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This property specifies the first alternating row.
        /// </summary>
        public int altstart
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the alternating step
        /// </summary>
        public int altstep
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the loading image should be displayed until the Grid's data is loaded.
        /// </summary>
        public bool autoshowloadelement
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the columns menu button will be displayed only when the mouse cursor is over a columns header or will be always displayed.
        /// </summary>
        public bool autoshowcolumnsmenubutton
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the clipboard operations
        /// </summary>
        public bool clipboard
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// When the value of this property is true, a close button is displayed in each grouping column.
        /// </summary>
        public bool closeablegroups
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the columns menu width.
        /// </summary>
        public int columnsmenuwidth
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback called when a column menu is opening. You can use it for changing the size of the menu or cancelling the opening. Three params are passed - menu, datafield and menu's height. If you return false, the opening will be cancelled.
        /// function (menu, datafield, height)
        /// </summary>
        public _function columnmenuopening
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback called when a column menu is opening. You can use it for changing the size of the menu or cancelling the opening. Three params are passed - menu, datafield and menu's height. If you return false, the opening will be cancelled.
        /// function (menu, datafield, height)
        /// </summary>
        public _function columnmenuclosing
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// function (cellhtmlElement, x, y)
        /// </summary>
        public _function cellhover
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the delete of a cell/row values by using the "delete" key.
        /// </summary>
        public bool enablekeyboarddelete
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether ellipsis will be displayed, if the cells or columns content overflows.
        /// </summary>
        public bool enableellipsis
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether mousewheel scrolling is enabled.
        /// </summary>
        public bool enablemousewheel
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the grid animations.
        /// </summary>
        public bool enableanimations
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the grid tooltips.
        /// </summary>
        public bool enabletooltips
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the grid rows hover state.
        /// </summary>
        public bool enablehover
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables the text selection of the browser.
        /// </summary>
        public bool enablebrowserselection
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public everpresentrowposition everpresentrowposition
        {
            get { return _get<everpresentrowposition>(); }
            set { _set(value); }
        }

        public int everpresentrowheight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public rowactions everpresentrowactions
        {
            get { return _get<rowactions>(); }
            set { _set(value); }
        }

        public rowactionsmode everpresentrowactionsmode
        {
            get { return _get<rowactionsmode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This function is called when a group is rendered.You can use it to customize the default group rendering.
        /// function (text, group, expanded)
        /// </summary>
        public _function groupsrenderer
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets a custom renderer for the grouping columns displayed in the grouping header when the grouping feature is enabled.
        /// function (text)
        /// </summary>
        public _function groupcolumnrenderer
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the default state of the grouped rows.
        /// </summary>
        public bool groupsexpandedbydefault
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// The function is called when a key is pressed. If the result of the function is true, the default keyboard navigation will be overriden for the pressed key.
        /// function(event)
        /// </summary>
        public _function handlekeyboardnavigation
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the Grid should display the built-in loading element or should use a DIV tag with class 'jqx-grid-load'
        /// </summary>
        public bool showdefaultloadelement
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// When this property is true, the Grid adds an additional visual style to the grid cells in the pinned column(s).
        /// </summary>
        public bool showpinnedcolumnbackground
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether to display the group menu items.
        /// </summary>
        public bool showgroupmenuitems
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the showheader property.
        /// </summary>
        public bool showheader
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Shows or hides the groups header area.
        /// </summary>
        public bool showgroupsheader
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Shows or hides the aggregates in the grid's statusbar.
        /// </summary>
        public bool showaggregates
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool showeverpresentrow
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Shows or hides the empty row label when the Grid has no records to display.
        /// </summary>
        public bool showemptyrow
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Shows or hides the grid's statusbar.
        /// </summary>
        public bool showstatusbar
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets the statusbar's height.
        /// </summary>
        public int statusbarheight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the selection mode.
        /// </summary>
        public selectionmode selectionmode
        {
            get { return _get<selectionmode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Shows or hides the grid's toolbar.
        /// </summary>
        public object showtoolbar
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the toolbarheight property.
        /// </summary>
        public int toolbarheight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        #endregion


        public bool autobind
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public object width
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        public object height
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        public bool row_numbers
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public jqxGrid._source source
        {
            get { return _get(create: () => new jqxGrid._source()); }
            set { _set(value); }
        }

        #region Layout

        /// <summary>
        /// Sets or gets the height of the grid to be equal to the summary height of the grid rows. This option should be set when the Grid is in paging mode.
        /// </summary>
        public bool autoheight
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This property works along with the "autoheight" property. When it is set to true, the height of the Grid rows is dynamically changed depending on the cell values.
        /// </summary>
        public bool autorowheight
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the columns height.
        /// </summary>
        public int columnsheight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the cell values displayed in a tooltip next to the scrollbar when the "scrollmode" is set to "deferred".
        /// </summary>
        public string[] deferreddatafields
        {
            get { return _get<string[]>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the Grid Groups Header.
        /// </summary>
        public int groupsheaderheight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the group indent size.This size is used when the grid is grouped.This is the size of the columns with expand/collapse toggle buttons.
        /// </summary>
        public int groupindentwidth
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the grid rows.
        /// </summary>
        public int rowsheight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the scrollbars size.
        /// </summary>
        public int scrollbarsize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the scrolling mode.
        /// </summary>
        public scrollmode scrollmode
        {
            get { return _get<scrollmode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// When the "scrollmode" is set to "deferred", the "scrollfeedback" function may be used to display custom UI Tooltip next to the scrollbar.
        /// function(row)
        /// </summary>
        public _function scrollfeedback
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        #endregion

        #region Behavior

        /// <summary>
        /// Determines whether the Grid automatically saves its current state.
        /// </summary>
        public bool autosavestate
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the Grid automatically loads its current state(if there's already saved one). The Grid's state is loaded when the page is refreshed.
        /// </summary>
        public bool autoloadstate
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public Type RowType { get; set; }

        /// <summary>
        /// Sets the Grid columns.
        /// </summary>
        public jqxGrid._column[] columns
        {
            get { return _get<jqxGrid._column[]>(); }
            set
            {
                if (this.RowType != null)
                {
                    TableNameAttribute tableName = TableNameAttribute.GetInstance(this.RowType);
                    foreach (var c in value)
                    {
                        string name = c.name;
                        c.sortable = tableName.IsSortable(name);
                        c.filterable = tableName.IsFilterable(name);
                    }
                }
                _set(value);
            }
        }

        /// <summary>
        /// The columngroups property enables you to create a Grid with multi column headers. Possible values for each array entry:
        /// </summary>
        public jqxGrid.columngroup[] columngroups
        {
            get { return _get<jqxGrid.columngroup[]>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the columns dropdown menu.
        /// </summary>
        public bool columnsmenu
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the columns resizing.
        /// </summary>
        public bool columnsresize
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the columns resizing when the column's border is double-clicked and columnsresize is set to true.
        /// </summary>
        public bool columnsautoresize
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the columns reordering.
        /// </summary>
        public bool columnsreorder
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// The editable property enables or disables the Grid editing feature.
        /// </summary>
        public bool editable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// The editmode property specifies the action that the end-user should make to open an editor.
        /// </summary>
        public editmode editmode
        {
            get { return _get<editmode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This property enables or disables the grouping feature.
        /// </summary>
        public bool groupable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the Grid groups when the Grouping feature is enabled.
        /// </summary>
        public string[] groups
        {
            get { return _get<string[]>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the scrollbar's step when the user clicks the scroll arrows.
        /// </summary>
        public int horizontalscrollbarstep
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the scrollbar's large step. This property specifies the step with which the horizontal scrollbar's value is changed when the user clicks the area above or below the thumb.
        /// </summary>
        public int horizontalscrollbarlargestep
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the keyboard navigation.
        /// </summary>
        public bool keyboardnavigation
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This function is called when the grid is initialized and the binding is complete.
        /// function()
        /// </summary>
        public _function ready
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called when the jqxGrid's render function is called either internally or not.
        /// function()
        /// </summary>
        public _function rendered
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the renderstatusbar property.
        /// function(statusbar)
        /// </summary>
        public _function renderstatusbar
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which allows you to customize the rendering of the Grid's toolbar.
        /// function(toolbar)
        /// </summary>
        public _function rendertoolbar
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is a function called when the grid is used in virtual mode. The function should return an array of rows which will be rendered by the Grid.
        /// function (params)
        /// </summary>
        public _function rendergridrows
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Selects a row at a specified index.
        /// </summary>
        public int selectedrowindex
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Selects single or multiple rows.
        /// </summary>
        public int[] selectedrowindexes
        {
            get { return _get<int[]>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the rendering update delay. This could be used for deferred scrolling scenarios.
        /// </summary>
        public int updatedelay
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the virtual data mode.
        /// </summary>
        public bool virtualmode
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the scrollbar's step when the user clicks the scroll arrows.
        /// </summary>
        public int verticalscrollbarstep
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the scrollbar's large step. This property specifies the step with which the vertical scrollbar's value is changed when the user clicks the area above or below the thumb.
        /// </summary>
        public int verticalscrollbarlargestep
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        #endregion

        #region Events

        public _event OnInitialized
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a row is clicked.
        /// </summary>
        public _event OnRowClick
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a row is selected.
        /// </summary>
        public _event OnRowSelect
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a row is unselected.
        /// </summary>
        public _event OnRowUnselect
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a group is expanded.
        /// </summary>
        public _event OnGroupExpand
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a group is collapsed.
        /// </summary>
        public _event OnGroupCollapse
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the Grid is sorted.
        /// </summary>
        public _event OnSort
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a column is clicked.
        /// </summary>
        public _event OnColumnClick
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a cell is clicked.
        /// </summary>
        public _event OnCellClick
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the current page is changed.
        /// </summary>
        public _event OnPageChanged
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the page size is changed.
        /// </summary>
        public _event OnPageSizeChanged
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the binding is completed. Note: Bind to that event before the Grid's initialization, because if you data bind the Grid to a local data source and bind to the "bindingcomplete" event after the initializaation, the data binding will be already completed.
        /// </summary>
        public _event OnBindingComplete
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a group is added, inserted or removed.
        /// </summary>
        public _event OnGroupsChanged
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the Grid is filtered.
        /// </summary>
        public _event OnFilter
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a Grid Column is resized.
        /// </summary>
        public _event OnColumnResized
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a cell is selected.
        /// </summary>
        public _event OnCellSelect
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a cell is unselected.
        /// </summary>
        public _event OnCellUnselect
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a cell's editor is displayed.
        /// </summary>
        public _event OnCellBeginEdit
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a cell's edit operation has ended.
        /// </summary>
        public _event OnCellEndEdit
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a cell's value is changed.
        /// </summary>
        public _event OnCellValueChanged
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a row with details is expanded.
        /// </summary>
        public _event OnRowExpand
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a row with details is collapsed.
        /// </summary>
        public _event OnRowCollapse
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a row is double clicked.
        /// </summary>
        public _event OnRowDoubleClick
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a cell is double-clicked.
        /// </summary>
        public _event OnCellDoubleClick
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a Grid Column is moved to a new position.
        /// </summary>
        public _event OnColumnReordered
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        public _event OnPageChanging
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        #endregion

        #region Methods

        //#region jqxgrid.js

        ///// <summary>
        ///// Sets the keyboard Focus to the jqxGrid widget.
        ///// </summary>
        //public void focus() { }

        ///// <summary>
        ///// Localizes the grid strings. This method allows you to change the valus of all Grid strings and also to change the cells formatting settings.
        ///// </summary>
        ///// <param name="localizationObject"></param>
        //public void localizestrings(object localizationObject) { }

        ///// <summary>
        ///// Gets bound data information.
        ///// </summary>
        ///// <returns></returns>
        //public object getdatainformation() { return null; }

        ///// <summary>
        ///// Gets the sort information.
        ///// </summary>
        ///// <returns>object.sortcolumn - sort column's datafield or null. object.sortdirection - Object with two fields: { 'ascending': true, 'descending': false }</returns>
        //public object getsortinformation() { return null; }

        ///// <summary>
        ///// Gets the paging information.
        ///// </summary>
        ///// <returns>object.pagenum - page number, object.pagesize - page size, object.pagescount - total pages count.</returns>
        //public object getpaginginformation() { return null; }

        ///// <summary>
        ///// Displays a column's menu.
        ///// </summary>
        ///// <param name="dataField"></param>
        //public void openmenu(string dataField) { }

        ///// <summary>
        ///// Closes a column's menu.
        ///// </summary>
        //public void closemenu() { }

        ///// <summary>
        ///// Scrolls the grid contents.
        ///// </summary>
        ///// <param name="top"></param>
        ///// <param name="left"></param>
        //public void scrolloffset(int top, int left) { }

        //public void scrollleft(int left) { }

        //public void scrolltop(int left) { }

        ///// <summary>
        ///// Starts an update operation. This is appropriate when calling multiple methods or set multiple properties at once. Optional boolean parameter: suspendAllActions. When you call beginupdate with parameter equal to true, the jqxGrid stops all rendering processes and when you call "endupdate", it will call the "render" method. Otherwise, it will try to resume its state with minimial performance impact. Use the suspendAllActions when you make multiple changes which require full-rerender such as changing the Grid's source, columns, groups.
        ///// </summary>
        //public void beginupdate() { }

        ///// <summary>
        ///// Ends the update operation.
        ///// </summary>
        //public void endupdate() { }

        //public void resumeupdate() { }

        ///// <summary>
        ///// Gets the updating operation state. Returns a boolean value.
        ///// </summary>
        ///// <returns></returns>
        //public bool updating() { return default(bool); }

        ///// <summary>
        ///// Shows the data loading image.
        ///// </summary>
        //public void showloadelement() { }

        ///// <summary>
        ///// Hides the data loading image.
        ///// </summary>
        //public void hideloadelement() { }

        ///// <summary>
        ///// Scrolls to a row. The parameter is a bound index.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        //public void ensurerowvisible(int rowBoundIndex) { }

        //public void ensurecellvisible(int rowBoundIndex, int columnBoundIndex) { }

        //public void setrowheight(int rowBoundIndex, int height) { }

        //public int getrowheight(int rowBoundIndex) { return 0; }

        ///// <summary>
        ///// Gets a column by datafield value.Column's fields:
        ///// </summary>
        ///// <param name="dataField"></param>
        ///// <returns></returns>
        //public jqxGrid.column getcolumn(string dataField) { return null; }

        ///// <summary>
        ///// Sets a property of a column.Possible property names: 'text', 'hidden', 'hideable', 'renderer', 'cellsrenderer', 'align', 'cellsalign', 'cellsformat', 'pinned', 'contenttype', 'resizable', 'filterable', 'editable', 'cellclassname', 'classname', 'width', 'minwidth', 'maxwidth'
        ///// </summary>
        ///// <param name="dataField"></param>
        ///// <param name="propertyName"></param>
        ///// <param name="propertyValue"></param>
        //public void setcolumnproperty(string dataField, columnPropertyName propertyName, object propertyValue) { }

        ///// <summary>
        ///// Gets a property of a column. Possible property names: 'text', 'hidden', 'hideable', 'renderer', 'cellsrenderer', 'align', 'cellsalign', 'cellsformat', 'pinned', 'contenttype', 'resizable', 'filterable', 'editable', 'cellclassname', 'classname', 'width', 'minwidth', 'maxwidth'
        ///// </summary>
        ///// <param name="dataField"></param>
        ///// <param name="propertyName"></param>
        ///// <returns></returns>
        //public object getcolumnproperty(string dataField, columnPropertyName propertyName) { return null; }

        ///// <summary>
        ///// Hides a column.
        ///// </summary>
        ///// <param name="dataField"></param>
        //public void hidecolumn(string dataField) { }

        ///// <summary>
        ///// Shows a column.
        ///// </summary>
        ///// <param name="dataField"></param>
        //public void showcolumn(string dataField) { }

        ///// <summary>
        ///// Gets whether a column is visible. Returns a boolean value.
        ///// </summary>
        ///// <param name="dataField"></param>
        ///// <returns></returns>
        //public bool iscolumnvisible(string dataField) { return false; }

        ///// <summary>
        ///// Pins the column.
        ///// </summary>
        ///// <param name="dataField"></param>
        //public void pincolumn(string dataField) { }

        ///// <summary>
        ///// Unpins the column.
        ///// </summary>
        ///// <param name="dataField"></param>
        //public void unpincolumn(string dataField) { }

        ///// <summary>
        ///// Gets whether a column is pinned. Returns a boolean value.
        ///// </summary>
        ///// <param name="dataField"></param>
        ///// <returns></returns>
        //public bool iscolumnpinned(string dataField) { return false; }

        ///// <summary>
        ///// Shows the details of a row.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        //public void showrowdetails(int rowBoundIndex) { }

        ///// <summary>
        ///// Hides the details of a row.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        //public void hiderowdetails(int rowBoundIndex) { }

        //public void hiderow(int rowBoundIndex) { }

        //public void showrow(int rowBoundIndex) { }

        ///// <summary>
        ///// Updates the bound data and refreshes the grid. You can pass 'filter' or 'sort' as parameter, if the update reason is change in 'filtering' or 'sorting'. To update only the data without the columns, use the 'data' parameter. To make a quick update of the cells, pass "cells" as parameter. Passing "cells" will refresh only the cells values when the new rows count is equal to the previous rows count. To make a full update, do not pass any parameter.
        ///// </summary>
        ///// <param name="type"></param>
        //public void updatebounddata(string type) { }

        ///// <summary>
        ///// Refreshes the data.
        ///// </summary>
        //public void refreshdata() { }

        ///// <summary>
        ///// Repaints the Grid View.
        ///// </summary>
        //public void refresh() { }

        ///// <summary>
        ///// Renders the Grid contents. This method completely refreshes the Grid cells, columns, layout and repaints the view.
        ///// </summary>
        //public void render() { }

        ///// <summary>
        ///// Clears the Grid contents.
        ///// </summary>
        //public void clear() { }

        ///// <summary>
        ///// Gets the text of a cell.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        ///// <param name="dataField"></param>
        ///// <returns></returns>
        //public string getcelltext(int rowBoundIndex, string dataField) { return null; }

        ///// <summary>
        ///// Gets the text of a cell.
        ///// </summary>
        ///// <param name="rowID"></param>
        ///// <param name="dataField"></param>
        ///// <returns></returns>
        //public string getcelltextbyid(string rowID, string dataField) { return null; }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        ///// <param name="datafield"></param>
        ///// <returns></returns>
        //public jqxGrid.cell getcell(int rowBoundIndex, string datafield) { return null; }

        ///// <summary>
        ///// Sets a new value to a cell.
        ///// </summary>
        ///// <param name="rowID"></param>
        ///// <param name="dataField"></param>
        ///// <param name="value"></param>
        //public void setcellvaluebyid(string rowID, string dataField, object value) { }

        ///// <summary>
        ///// Gets the value of a cell.
        ///// </summary>
        ///// <param name="rowID"></param>
        ///// <param name="dataField"></param>
        ///// <returns></returns>
        //public object getcellvaluebyid(string rowID, string dataField) { return null; }

        ///// <summary>
        ///// Sets a new value to a cell.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        ///// <param name="dataField"></param>
        ///// <param name="value"></param>
        //public void setcellvalue(int rowBoundIndex, string dataField, object value) { }

        ///// <summary>
        ///// Gets the value of a cell.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        ///// <param name="dataField"></param>
        ///// <returns></returns>
        //public object getcellvalue(int rowBoundIndex, string dataField) { return null; }

        ///// <summary>
        ///// Gets all rows. Returns an array of all rows loaded in the Grid. If the Grid is filtered, the returned value is an array of the filtered records.
        ///// </summary>
        ///// <returns></returns>
        //public object[] getrows() { return null; }

        ///// <summary>
        ///// Gets the index of a row in the array returned by the getboundrows method.
        ///// </summary>
        ///// <param name="rowID"></param>
        ///// <returns></returns>
        //public int getrowboundindexbyid(string rowID) { return 0; }

        ///// <summary>
        ///// Gets the data of a row. The returned value is a JSON Object. The parameter is the row's id.
        ///// </summary>
        ///// <param name="rowID"></param>
        ///// <returns></returns>
        //public int getrowdatabyid(string rowID) { return 0; }

        ///// <summary>
        ///// Gets the data of a row. The returned value is a JSON Object. The parameter is the row's bound index. Note: If you pass a bound index of a row which is not visible in the Grid, the method returns null.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        ///// <returns></returns>
        //public object[] getrowdata(int rowBoundIndex) { return null; }

        ///// <summary>
        ///// Gets all rows loaded from the data source. The method returns an Array of all rows. The Grid's sorting, filtering, grouping and paging will not affect the result of this method. It will always return the rows collection loaded from the data source.
        ///// </summary>
        ///// <returns></returns>
        //public object[] getboundrows() { return null; }

        ///// <summary>
        ///// Gets the index of a row in the array returned by the getboundrows method.
        ///// </summary>
        ///// <param name="rowDisplayIndex"></param>
        ///// <returns></returns>
        //public int getrowboundindex(string rowDisplayIndex) { return 0; }

        ///// <summary>
        ///// Gets all rows that are currently displayed in the Grid. The method returns an Array of the displayed rows. The Grid's sorting, filtering, grouping and paging will affect the result of this method.
        ///// </summary>
        ///// <returns></returns>
        //public object[] getdisplayrows() { return null; }

        ///// <summary>
        ///// Gets the id of a row. The returned value is a 'String' or 'Number' depending on the id's type. The parameter is the row's bound index.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        ///// <returns></returns>
        //public string getrowid(int rowBoundIndex) { return null; }

        ///// <summary>
        ///// Updates a row or multiple rows.
        ///// </summary>
        ///// <param name="rowIds">rowID or rowIds. You can use getrowid method for getting the ID of a row.</param>
        ///// <param name="data"></param>
        //public void updaterow(object rowIds, object data) { }

        ///// <summary>
        ///// Deletes a row or multiple rows. Returns a boolean value.
        ///// </summary>
        ///// <param name="rowIds">rowID or rowIds. You can use getrowid method for getting the ID of a row.</param>
        //public void deleterow(object rowIds) { }

        ///// <summary>
        ///// Adds a new row or multiple rows.
        ///// </summary>
        ///// <param name="rowIds">rowID or rowIds. You can use getrowid method for getting the ID of a row.</param>
        ///// <param name="data"></param>
        ///// <param name="rowPosition">"first" or "last"</param>
        //public void addrow(object rowIds, object data, string rowPosition) { }

        ///// <summary>
        ///// Removes the Grid from the document and releases its resources.
        ///// </summary>
        //public void destroy() { }

        ///// <summary>
        ///// Gets a cell at specific position. Returns an object with the following fields:
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="top"></param>
        ///// <returns></returns>
        //public jqxGrid.cell getcellatposition(int left, int top) { return null; }

        ///// <summary>
        ///// Returns whether the binding is completed and if the result is true, this means that you can invoke methods and set properties. Otherwise, if the binding is not completed and you try to set a property or invoke a method, the widget will throw an exception.
        ///// </summary>
        ///// <returns></returns>
        //public bool isbindingcompleted() { return false; }

        //#endregion

        //#region jqxgrid.selection.js

        ///// <summary>
        ///// The selection mode should be set to: 'multiplerows' or 'multiplerowsextended'
        ///// </summary>
        //public void selectallrows() { }

        //public void unselectallrows() { }

        ///// <summary>
        ///// Selects a row.        
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        ///// <remarks>The expected selection mode is 'singlerow', 'multiplerows' or 'multiplerowsextended'</remarks>
        //public void selectrow(int rowBoundIndex) { }

        ///// <summary>
        ///// Unselects a row.
        ///// </summary>
        ///// <remarks>The expected selection mode is 'singlerow', 'multiplerows' or 'multiplerowsextended'</remarks>
        ///// <param name="rowBoundIndex"></param>
        //public void unselectrow(int rowBoundIndex) { }

        ///// <summary>
        ///// Selects a cell.
        ///// </summary>
        ///// <remarks>The expected selection mode is 'singlecell', 'multiplecells' or 'multiplecellsextended'</remarks>
        ///// <param name="rowBoundIndex"></param>
        ///// <param name="dataField"></param>
        //public void selectcell(int rowBoundIndex, string dataField) { }

        ///// <summary>
        ///// Unselects a cell.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        ///// <param name="dataField"></param>
        ///// <remarks>The expected selection mode is 'singlecell', 'multiplecells' or 'multiplecellsextended'</remarks>
        //public void unselectcell(int rowBoundIndex, string dataField) { }

        ///// <summary>
        ///// Clears the selection.
        ///// </summary>
        //public void clearselection() { }

        ///// <summary>
        ///// Gets the bound index of the selected row. Returns -1, if there's no selection.
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks>The expected selection mode is 'singlerow', 'multiplerows' or 'multiplerowsextended'</remarks>
        //public int getselectedrowindex() { return 0; }

        ///// <summary>
        ///// Gets the indexes of the selected rows. Returns an array of the selected rows.
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks>The expected selection mode is 'singlerow', 'multiplerows' or 'multiplerowsextended'</remarks>
        //public int[] getselectedrowindexes() { return null; }

        ///// <summary>
        ///// Gets the selected cell. The returned value is an Object with two fields: 'rowindex' - the row's bound index and 'datafield' - the column's datafield.
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks>The expected selection mode is 'singlecell', 'multiplecells' or 'multiplecellsextended'</remarks>
        //public jqxGrid.cell getselectedcell() { return null; }

        ///// <summary>
        ///// Gets all selected cells. Returns an array of all selected cells. Each cell in the array is an Object with two fields: 'rowindex' - the row's bound index and 'datafield' - the column's datafield.
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks>The expected selection mode is 'singlecell', 'multiplecells' or 'multiplecellsextended'</remarks>
        //public jqxGrid.cell[] getselectedcells() { return null; }

        //public void deleteselection() { }

        //public void copyselection() { }

        //public void pasteselection() { }

        //public void selectprevcell(int rowBoundIndex, string dataField) { }

        //public void selectnextcell(int rowBoundIndex, string dataField) { }

        //#endregion

        //#region jqxgrid.pager.js

        ///// <summary>
        ///// Navigates to a page when the Grid paging is enabled i.e when the pageable property value is true.
        ///// </summary>
        ///// <param name="pageNumber"></param>
        //public void gotopage(int pageNumber) { }

        ///// <summary>
        ///// Navigates to a previous page when the Grid paging is enabled i.e when the pageable property value is true.
        ///// </summary>
        //public void gotoprevpage() { }

        ///// <summary>
        ///// Navigates to a next page when the Grid paging is enabled i.e when the pageable property value is true.
        ///// </summary>
        //public void gotonextpage() { }

        //public void updatepagerdetails() { }

        //#endregion

        //#region jqxgrid.edit.js

        ///// <summary>
        ///// Shows the cell's editor.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        ///// <param name="dataField"></param>
        //public void begincelledit(int rowBoundIndex, string dataField) { }

        ///// <summary>
        ///// Hides the edit cell's editor and saves or cancels the changes.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        ///// <param name="dataField"></param>
        ///// <param name="confirmChanges"></param>
        //public void endcelledit(int rowBoundIndex, string dataField, bool confirmChanges) { }

        ///// <summary>
        ///// Shows the cell editors for an entire row.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        //public void beginrowedit(int rowBoundIndex) { }

        ///// <summary>
        ///// Hides the edited row's editors and saves or cancels the changes.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        ///// <param name="confirmChanges"></param>
        //public void endrowedit(int rowBoundIndex, bool confirmChanges) { }

        ///// <summary>
        ///// Displays a validation popup below a Grid cell.
        ///// </summary>
        ///// <param name="rowBoundIndex"></param>
        ///// <param name="dataField"></param>
        ///// <param name="validationMessage"></param>
        //public void showvalidationpopup(int rowBoundIndex, string dataField, string validationMessage) { }

        //public void hidevalidationpopups() { }

        //#endregion

        //#region jqxgrid.sort.js

        ///// <summary>
        ///// Gets the sort column.Returns the column's datafield or null, if sorting is not applied.
        ///// </summary>
        ///// <returns></returns>
        //public string getsortcolumn() { return null; }

        ///// <summary>
        ///// Removes the sorting.
        ///// </summary>
        //public void removesort() { }

        ///// <summary>
        ///// Sorts the Grid data.
        ///// </summary>
        ///// <param name="dataField"></param>
        ///// <param name="sortOrder"></param>
        //public void sortby(string dataField, sortOrder? sortOrder) { }

        //#endregion

        //#region jqxgrid.filter.js

        //public void clearfilterrow(string dataField) { }

        ///// <summary>
        ///// Refreshes the filter row and updates the filter widgets. The filter row's widgets are synchronized with the applied filters.
        ///// </summary>
        //public void refreshfilterrow() { }

        ///// <summary>
        ///// Adds a filter to the Grid.
        ///// </summary>
        ///// <param name="dataField"></param>
        ///// <param name="filterGroup"></param>
        ///// <param name="refreshGrid"></param>
        //public void addfilter(string dataField, object filterGroup, bool refreshGrid) { }

        ///// <summary>
        ///// Removes a filter from the Grid.
        ///// </summary>
        ///// <param name="dataField"></param>
        ///// <param name="refreshGrid"></param>
        //public void removefilter(string dataField, bool refreshGrid) { }

        ///// <summary>
        ///// Applies all filters to the Grid.
        ///// </summary>
        //public void applyfilters() { }

        ///// <summary>
        ///// Gets the information about the Grid filters. The method returns an array of the applied filters. The returned information includes the filter objects and filter columns.
        ///// </summary>
        ///// <returns></returns>
        //public object[] getfilterinformation() { return null; }

        ///// <summary>
        ///// Clears all filters from the Grid. You can call the method with optional boolean parameter. If the parameter is "true" or you call the method without parameter, the Grid will clear the filters and refresh the Grid(default behavior). If the parameter is "false", the method will clear the filters without refreshing the Grid.
        ///// </summary>
        //public void clearfilters() { }


        //#endregion

        //#region jqxgrid.columnsresize.js

        ///// <summary>
        ///// Auto-resizes all columns.
        ///// </summary>
        ///// <param name="type">"all", "cells" or "column"</param>
        //public void autoresizecolumns(string type) { }

        ///// <summary>
        ///// Auto-resizes a column.
        ///// </summary>
        ///// <param name="dataField"></param>
        ///// <param name="type">"all", "cells" or "column"</param>
        //public void autoresizecolumn(string dataField, string type) { }

        //#endregion

        //#region jqxgrid.columnsreorder.js

        ///// <summary>
        ///// Gets the index of a column in the columns collection.
        ///// </summary>
        ///// <param name="dataField"></param>
        ///// <returns></returns>
        //public int getcolumnindex(string dataField) { return 0; }

        ///// <summary>
        ///// Sets the index of a column in the columns collection.
        ///// </summary>
        ///// <param name="dataField"></param>
        ///// <param name="index"></param>
        //public void setcolumnindex(string dataField, int index) { }

        //public jqxGrid.column getcolumnbytext(string text) { return null; }

        //#endregion

        //#region jqxgrid.grouping.js

        ///// <summary>
        ///// Groups by a column.
        ///// </summary>
        ///// <param name="dataField"></param>
        //public void addgroup(string dataField) { }

        ///// <summary>
        ///// Groups by a column.
        ///// </summary>
        ///// <param name="groupIndex"></param>
        ///// <param name="dataField"></param>
        //public void insertgroup(int groupIndex, string dataField) { }

        //public void refreshgroups() { }

        ///// <summary>
        ///// Removes a group at specific index.
        ///// </summary>
        ///// <param name="groupIndex"></param>
        //public void removegroupat(int groupIndex) { }

        ///// <summary>
        ///// Removes a group.
        ///// </summary>
        ///// <param name="dataField"></param>
        //public void removegroup(string dataField) { }

        ///// <summary>
        ///// Clears all groups.
        ///// </summary>
        //public void cleargroups() { }

        ///// <summary>
        ///// Gets the number of root groups.
        ///// </summary>
        ///// <returns></returns>
        //public int getrootgroupscount() { return 0; }

        ///// <summary>
        ///// Collapses a group.
        ///// </summary>
        ///// <param name="group">Number for root groups or String like "1.1" for sub groups</param>
        //public void collapsegroup(string group) { }

        ///// <summary>
        ///// Expands a group.
        ///// </summary>
        ///// <param name="group">Number for root groups or String like "1.1" for sub groups</param>
        //public void expandgroup(string group) { }

        ///// <summary>
        ///// Collapses all groups.
        ///// </summary>
        //public void collapseallgroups() { }

        ///// <summary>
        ///// Expands all groups.
        ///// </summary>
        //public void expandallgroups() { }

        ///// <summary>
        ///// Gets a group. The method returns an Object with details about the Group. The object has the following fields:
        /////     group - group's name.
        /////     level - group's level in the group's hierarchy.
        /////     expanded - group's expand state.
        /////     subgroups - an array of sub groups or null.
        /////     subrows - an array of rows or null.
        ///// </summary>
        ///// <param name="groupIndex"></param>
        ///// <returns></returns>
        //public object getgroup(int groupIndex) { return null; }

        //public object getrootgroups() { return null; }

        ///// <summary>
        ///// Gets whether the user can group by a column. Returns a boolean value.
        ///// </summary>
        ///// <returns></returns>
        //public bool iscolumngroupable() { return true; }

        //#endregion

        //#region jqxgrid.aggregates.js

        ///// <summary>
        ///// Gets the aggregated data of a Grid column. Returns a JSON object. Each field name is the aggregate's type('min', 'max', 'sum', etc.).
        ///// </summary>
        ///// <param name="dataField">column's data field</param>
        ///// <param name="aggregates">Array of aggregates 'min', 'max', 'sum', etc.</param>
        //public void getcolumnaggregateddata(string dataField, object[] aggregates) { }

        ///// <summary>
        ///// Refreshes the Aggregates in the Grid's status bar.
        ///// </summary>
        //public void refreshaggregates() { }

        ///// <summary>
        ///// Renders the aggregates in the Grid's status bar.
        ///// </summary>
        //public void renderaggregates() { }

        //#endregion

        //#region jqxgrid.export.js

        ///// <summary>
        ///// Exports all rows loaded within the Grid to Excel, XML, CSV, TSV, HTML or JSON.
        ///// </summary>
        ///// <param name="dataType"></param>
        ///// <param name="fileName"></param>
        ///// <param name="exportHeader"></param>
        ///// <param name="rows"></param>
        ///// <param name="exportHiddenColumns"></param>
        ///// <param name="serverURL"></param>
        ///// <param name="charSet"></param>
        //public void exportdata(string dataType, string fileName, bool exportHeader, object[] rows, bool exportHiddenColumns, string serverURL, string charSet) { }

        //#endregion

        //#region jqxgrid.storage.js

        ///// <summary>
        ///// Saves the Grid's current state. the savestate method saves the following information: 'sort column, sort order, page number, page size, applied filters and filter row values, column widths and visibility, cells and rows selection and groups.
        ///// </summary>
        //public void savestate() { }

        ///// <summary>
        ///// Loads the Grid's state. the loadstate method loads the following information: 'sort column, sort order, page number, page size, applied filters and filter row values, column widths and visibility, cells and rows selection and groups.
        ///// </summary>
        ///// <param name="stateObject">The state object returned by saveState method call.</param>
        //public void loadstate(object stateObject) { }

        ///// <summary>
        ///// Gets the Grid's state. the getstate method gets the following information: 'sort column, sort order, page number, page size, applied filters and filter row values, column widths and visibility, cells and rows selection and groups.
        ///// </summary>
        ///// <returns></returns>
        //public object getstate() { return null; }

        //#endregion

        #endregion
    }

    public class jqxGrid<T> : jqxGrid
    {
        public jqxGrid()
        {
            base.RowType = typeof(T);
            base.enablebrowserselection = true;
            base.row_numbers = true;
        }
    }

    partial class jqxGrid
    {
        public class datafield : _jqxBase
        {
            /// <summary>
            /// A string containing the data field's name. 
            /// </summary>
            public string name
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// A string containing the data field's type. Possible values: 'string', 'date', 'number', 'bool'. 
            /// </summary>
            public dataFieldType type
            {
                get
                {
                    switch (_get<string>())
                    {
                        case "string": return dataFieldType._string;
                        case "date": return dataFieldType._date__;
                        case "number": return dataFieldType._number;
                        case "bool": return dataFieldType._bool__;
                        default: return dataFieldType._object;
                    }
                }
                set
                {
                    switch (value)
                    {
                        case dataFieldType._string: _set("string"); break;
                        case dataFieldType._date__: _set("date"); break;
                        case dataFieldType._number: _set("number"); break;
                        case dataFieldType._bool__: _set("bool"); break;
                        case dataFieldType._object: _set("object"); break;
                    }
                }
            }
            /// <summary>
            /// (optional) Sets the data formatting. By setting the format, the jqxDataAdapter plug-in will try to format the data before loading
            /// </summary>
            public string format
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// (optional) A mapping to the data field. 
            /// </summary>
            public string map
            {
                get { return _get<string>(); }
                set { _set(value); }
            }

            /// <summary>
            /// determines the id of a record in a foreign collection which should match to the record's name in the source collection.
            /// </summary>
            public string id
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// determines the display field from the foreign collection.
            /// </summary>
            public string text
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// determines the foreign collection associated to the data field. The expected value is an array.
            /// </summary>
            public object[] source
            {
                get { return _get<object[]>(); }
                set { _set(value); }
            }
        }
        public class _source : _jqxBase
        {
            /// <summary>
            /// Before the data is sent to the server, you can fully override it by using the 'formatdata' function of the source object. The result that the 'formatdata' function returns is actually what will be sent to the server.
            /// function (data)
            /// </summary>
            public _function formatdata
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            /// <summary>
            /// function (data, state, xhr)
            /// </summary>
            public _function beforeprocessing
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            /// <summary>
            /// function (data)
            /// </summary>
            public _function loadComplete
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            /// <summary>
            /// 
            /// </summary>
            public _function beforesend
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            /// <summary>
            /// function (xhr, status, error)
            /// </summary>
            public _function loaderror
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            /// <summary>
            /// data array or data string pointing to a local data source.
            /// </summary>
            public object[] localdata
            {
                get { return _get<object[]>(); }
                set { _set(value); }
            }

            /// <summary>
            /// Data to be sent to the server.
            /// </summary>
            public object data
            {
                get { return _get<object>(); }
                set { _set(value); }
            }

            /// <summary>
            /// the data's type. Possible values: 'xml', 'json', 'jsonp', 'tsv', 'csv', 'local', 'array', 'observablearray'. 
            /// </summary>
            public datatype datatype
            {
                get { return _get<datatype>(); }
                set { _set(value); }
            }

            /// <summary>
            /// An array describing the fields in a particular record. Each datafield must define the following members
            /// </summary>
            public datafield[] datafields
            {
                get { return _get<datafield[]>(); }
                set { _set(value); }
            }

            /// <summary>
            /// A string containing the URL to which the request is sent. 
            /// </summary>
            public string url
            {
                get { return _get<string>(); }
                set { _set(value); }
            }

            /// <summary>
            /// The type of request to make ("POST" or "GET"), default is "GET". 
            /// </summary>
            public string type
            {
                get { return _get<string>(); }
                set { _set(value); }
            }

            /// <summary>
            /// A string describing where the data begins and all other loops begin from this element. 
            /// </summary>
            public string root
            {
                get { return _get<string>(); }
                set { _set(value); }
            }

            /// <summary>
            /// A string describing the information for a particular record. 
            /// </summary>
            public string record
            {
                get { return _get<string>(); }
                set { _set(value); }
            }

            /// <summary>
            /// A string containing the Id data field. 
            /// </summary>
            public string id
            {
                get { return _get<string>(); }
                set { _set(value); }
            }

            public int totalrecords
            {
                get { return _get<int>(); }
                set { _set(value); }
            }

            public int recordstartindex
            {
                get { return _get<int>(); }
                set { _set(value); }
            }

            public int recordendindex
            {
                get { return _get<int>(); }
                set { _set(value); }
            }

            public bool loadallrecords
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }

            /// <summary>
            /// determines the initial sort column. The expected value is a data field name.
            /// </summary>
            public string sortcolumn
            {
                get { return _get<string>(); }
                set { _set(value); }
            }

            /// <summary>
            /// determines the sort order. The expected value is 'asc' for (A to Z) sorting or 'desc' for (Z to A) sorting.
            /// </summary>
            public SortOrder sortdirection
            {
                get { return _get<SortOrder>(); }
                set { _set(value); }
            }

            /// <summary>
            /// callback function called when the sort column or sort order is changed. 
            /// function (column, direction)
            /// </summary>
            public _function sort
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            /// <summary>
            /// callback function called when a filter is applied or removed.
            /// function(filters, recordsArray)
            /// </summary>
            public _function filter
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            public _function sortcomparer
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }



            /// <summary>
            /// determines the initial page number when paging is enabled.
            /// </summary>
            public int pagenum
            {
                get { return _get<int>(); }
                set { _set(value); }
            }

            /// <summary>
            /// determines the page size when paging is enabled.
            /// </summary>
            public int pagesize
            {
                get { return _get<int>(); }
                set { _set(value); }
            }

            /// <summary>
            /// callback function called when the current page or page size is changed.
            /// function (pagenum, pagesize, oldpagenum)
            /// </summary>
            public _function pager
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            /// <summary>
            /// callback function, called when a new row is/are added. If multiple rows are added, the rowid and rowdata parameters are arrays of row ids and rows. 
            /// function (rowid, rowdata, position, commit)
            /// </summary>
            public _function addrow
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            /// <summary>
            /// callback function, called when a row is deleted. If multiple rows are deleted, the rowid parameter is an array of row ids. 
            /// function (rowid, commit)
            /// </summary>
            public _function deleterow
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            /// <summary>
            /// callback function, called when a row is updated. If multiple rows are added, the rowid and rowdata parameters are arrays of row ids and rows. 
            /// function (rowid, newdata, commit)
            /// </summary>
            public _function updaterow
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            /// <summary>
            /// extend the default data object sent to the server. 
            /// function (data)
            /// </summary>
            public _function processdata
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }

            public string contenttype
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
        }
        public static class _columns
        {
            public class DateTime : jqxGrid._column
            {
                public DateTime()
                {
                    this._datafield = new jqxGrid.datafield() { type = dataFieldType._date__ };
                    this.width = 130;
                    this.columntype = columntype.datetimeinput;
                    this.cellsformat = "yyyy/MM/dd HH:mm:ss";
                    this.filtertype = filtertype.range;
                }
                public override datafield _datafield
                {
                    get { return base._datafield; }
                    set
                    {
                        base._datafield = value;
                        if (value != null)
                            value.type = dataFieldType._date__;
                    }
                }
            }
            public class User____ : jqxGrid._column
            {
                public User____()
                {
                    this.width = 100;
                }
                public override datafield _datafield
                {
                    get { return base._datafield; }
                    set
                    {
                        base._datafield = value;
                        if (value != null)
                            value.type = dataFieldType._number;
                    }
                }
            }
            public class GameClass : jqxGrid._column
            {
                public GameClass()
                {
                    this.filtertype = filtertype.checkedlist;
                    //this.datafield = langHelper.value_name;
                    this.displayfield = langHelper.label_name;
                    //this.filteritems = langHelper.GetEnums(ams.GameClass.Others);
                }
                public override datafield _datafield
                {
                    get { return base._datafield; }
                    set
                    {
                        base._datafield = value;
                        if (value != null)
                            value.type = dataFieldType._string;
                    }
                }
            }
            public class Text : jqxGrid._column
            {
                public Text()
                {
                    this.width = 300;
                }
                public override datafield _datafield
                {
                    get { return base._datafield; }
                    set
                    {
                        base._datafield = value;
                        if (value != null)
                            value.type = dataFieldType._string;
                    }
                }
            }
            //public static jqxGrid._column CreateTime(_WebViewPage page) { return new jqxGrid._column { text = page.lang["CreateTime"], _datafield = { name = "CreateTime", type = "date" }, width = 130, columntype = columntype.datetimeinput, cellsformat = "yyyy/MM/dd HH:mm:ss", filtertype = filtertype.range, }; }
            //public static jqxGrid._column ModifyTime(_WebViewPage page) { return new jqxGrid._column { text = page.lang["ModifyTime"], _datafield = { name = "ModifyTime", type = "date" }, width = 130, columntype = columntype.datetimeinput, cellsformat = "yyyy/MM/dd HH:mm:ss", filtertype = filtertype.range, }; }
            //public static jqxGrid._column CreateUser(_WebViewPage page) { return new jqxGrid._column { text = page.lang["CreateUser"], _datafield = { name = "CreateUser", type = "number" }, width = 100, }; }
            //public static jqxGrid._column ModifyUser(_WebViewPage page) { return new jqxGrid._column { text = page.lang["ModifyUser"], _datafield = { name = "ModifyUser", type = "number" }, width = 100, }; }
        }
        public class _column : _jqxBase
        {
            public string name
            {
                get
                {
                    string name;
                    if (!this._get(out name))
                        _set(name = this.datafield ?? this._get<datafield>("datafield")?.name);
                    return name;
                }
                set { _set(value); }
            }

            public _column() { this.align = this.cellsalign = alignment.center; }
            public _column(string name) : this() { this.name = name; }

            public virtual string ColumnDefine
            {
                get { return _get<string>(); }
                set { _set(value); }
            }


            //internal datafield GetDatafield() => _get<datafield>("datafield");

            public virtual datafield _datafield
            {
                get { return _get("datafield", () => new datafield()); }
                set { _set(value, "datafield"); }
            }

            /// <summary>
            /// sets the column datafield.
            /// </summary>
            public virtual string datafield
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the column's displayfield. The displayfield specifies the field in the data source from which the column to retrieve strings for display.
            /// </summary>
            public string displayfield
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the column text.
            /// </summary>
            public string text
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function for overriding the Grid's Filter Menu. The column's filtertype should be set to "custom" in order to use that option.The grid passes the column's field and filter panel as parameters.
            /// function (datafield, filterPanel) 
            /// </summary>
            public _function createfilterpanel
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// enables or disables the sorting.
            /// </summary>
            public bool sortable
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            /// <summary>
            /// enables or disables whether the column can be hidden.
            /// </summary>
            public bool hideable
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            /// <summary>
            /// enables or disables the cells editing
            /// </summary>
            public bool editable
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            /// <summary>
            ///  hides or shows the column.
            /// </summary>
            public bool hidden
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets whether the user can group by this column.
            /// </summary>
            public bool groupable
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom column renderer.This can be used for changing the built-in rendering of the column's header.
            /// </summary>
            public _function renderer
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom rendering function.The cellsrenderer function is called when a cell in the column is rendered.You can use it to override the built-in cells rendering.The cellsRenderer function has 6 parameters passed by jqxGrid - row index, data field, cell value, defaultHtml string that is rendered by the grid, column's settings and the entire row's data as JSON object.
            /// cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties)
            /// </summary>
            public _function cellsrenderer
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            public _function checkchange
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            public bool threestatecheckbox
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            public _function buttonclick
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the column's type.
            /// </summary>
            public columntype columntype
            {
                get { return _get<columntype>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the formatting of the grid cells.
            /// 
            /// Possible Number strings: 
            /// "d" - decimal numbers. 
            /// "f" - floating-point numbers. 
            /// "n" - integer numbers. 
            /// "c" - currency numbers. 
            /// "p" - percentage numbers. 
            /// 
            /// For adding decimal places to the numbers, add a number after the formatting string. 
            /// For example: "c3" displays a number in this format $25.256 
            /// Possible built-in Date formats: 
            /// 
            /// // short date pattern d: "M/d/yyyy", 
            /// // long date pattern D: "dddd, MMMM dd, yyyy", 
            /// // short time pattern t: "h:mm tt", 
            /// // long time pattern T: "h:mm:ss tt", 
            /// // long date, short time pattern f: "dddd, MMMM dd, yyyy h:mm tt", 
            /// // long date, long time pattern F: "dddd, MMMM dd, yyyy h:mm:ss tt", 
            /// // month/day pattern M: "MMMM dd", 
            /// // month/year pattern Y: "yyyy MMMM", 
            /// // S is a sortable format that does not vary by culture S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss" 
            /// 
            /// Possible Date format strings: 
            /// 
            /// "d"-the day of the month;
            /// "dd"-the day of the month; 
            /// "ddd"-the abbreviated name of the day of the week;
            /// "dddd"- the full name of the day of the week;
            /// "h"-the hour, using a 12-hour clock from 1 to 12; 
            /// "hh"-the hour, using a 12-hour clock from 01 to 12; 
            /// "H"-the hour, using a 24-hour clock from 0 to 23;
            /// "HH"- the hour, using a 24-hour clock from 00 to 23; 
            /// "m"-the minute, from 0 through 59;
            /// "mm"-the minutes,from 00 though59;
            /// "M"- the month, from 1 through 12;
            /// "MM"- the month, from 01 through 12;
            /// "MMM"-the abbreviated name of the month;
            /// "MMMM"-the full name of the month;
            /// "s"-the second, from 0 through 59; 
            /// "ss"-the second, from 00 through 59; 
            /// "t"- the first character of the AM/PM designator;
            /// "tt"-the AM/PM designator; 
            /// "y"- the year, from 0 to 99; 
            /// "yy"- the year, from 00 to 99; 
            /// "yyy"-the year, with a minimum of three digits; 
            /// "yyyy"-the year as a four-digit number; 
            /// "yyyyy"-the year as a four-digit number. 
            /// </summary>
            public string cellsformat
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the column header's alignment to 'left', 'center' or 'right'
            /// </summary>
            public alignment align
            {
                get { return _get<alignment>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the cells alignment to 'left', 'center' or 'right'.
            /// </summary>
            public alignment cellsalign
            {
                get { return _get<alignment>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the column width.
            /// </summary>
            public int width
            {
                get { return _get<int>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the column's min width.
            /// </summary>
            public int minwidth
            {
                get { return _get<int>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the column's max width.
            /// </summary>
            public int maxwidth
            {
                get { return _get<int>(); }
                set { _set(value); }
            }
            /// <summary>
            /// pins or unpins the column.If the column is pinned, it will be displayed as frozen and will be visible when the user horizontally scrolls the grid contents.
            /// </summary>
            public bool pinned
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            public int visibleindex
            {
                get { return _get<int>(); }
                set { _set(value); }
            }
            /// <summary>
            /// enables or disables the filtering.
            /// </summary>
            public bool filterable
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the column's initialization filter. A $.jqx.filter object is expected.
            /// </summary>
            public jqx_filter filter
            {
                get { return _get<jqx_filter>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the items displayed in the list filter - when the "showfilterrow" property value is true and the filter's type is "list" or "checkedlist". The expected value is Array or jqxDataAdapter instance.
            /// </summary>
            public object filteritems
            {
                get { return _get<object>(); }
                set { _set(value); }
            }
            public object filterdefault
            {
                get { return _get<object>(); }
                set { _set(value); }
            }

            /// <summary>
            /// enables or disables the column resizing.
            /// </summary>
            public bool resizable
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when the cells editor is opened.The Grid passes 6 parameters to it - row index, cell value, the editor element, cell's text, the pressed char. The function can be used for adding some custom parameters to the editor. This function is called each time an editor is opened.
            /// function (row, cellvalue, editor, celltext, pressedChar)
            /// </summary>
            public _function initeditor
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when the cells editor is created.The Grid passes 6 parameters to it - row index, cell value, the editor element, cell's text, cell's width and cell's height. The function can be used for adding some custom parameters to the editor. This function is called only once - when the editor is created.
            /// function (row, cellvalue, editor, celltext, cellwidth, cellheight)
            /// </summary>
            public _function createeditor
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a widget in a cell is created.You can use this callback function to create a custom read-only column which displays widgets in the cells. The Grid passes 4 parameters to it - row, column, cell value and the cell's element.
            /// function (row, column, value, cellElement)
            /// </summary>
            public _function createwidget
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a widget in a cell needs to be updated.The function is called only if "createwidget" is defined.You can use this callback function to update a widget inside a custom read-only column. The Grid passes 4 parameters to it - row, column, cell value and the cell's element.
            /// function (row, column, value, cellElement)
            /// </summary>
            public _function initwidget
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            public _function destroywidget
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a custom cell editor is destroyed.The function is called only when the "columntype" property is set to "custom "and "template". In all other cases, jqxGrid automatically destroys the editors.The Grid passes 1 parameter to it - the editor element.If the "columntype" is "custom", the Grid passes the row's bound index as a second parameter.
            /// 
            /// </summary>
            public _function destroyeditor
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// gets the editor's value to be displayed in the cell. The function can be used for overriding the value returned by the editor. It is useful for advanced scenarios with custom editors and edit templates. The Grid passes 3 parameters to it - row's bound index index, cell value and the editor element.
            /// function (row, cellvalue, editor)
            /// </summary>
            public _function geteditorvalue
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom validation function.The Grid passes 2 parameters to it - edit cell and the cell's value. The function should return true or false, depending on the user's validation logic.It can also return a validation object with 2 fields - "result" - true or false, and "message" - validation string displayed to the users.
            /// function (cell, value)
            /// </summary>
            public _function validation
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom CSS class for the column's header
            /// </summary>
            public string classname
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom CSS class for the column's cells. The value could be a "String" or "Function". 
            /// function (row, column, value, data)
            /// </summary>
            public string cellclassname
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a cell leaves the edit mode.The Grid passes 5 parameters to it - row index, column data field, column type, old cell value, new cell value.The function can be used for canceling the changes of a specific Grid cell. To cancel the changes, the function should return false.
            /// function (row, datafield, columntype, oldvalue, newvalue)
            /// </summary>
            public _function cellendedit
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a cell enters into edit mode.The Grid passes 3 parameters to it - row index, column data field and column type. The function can be used for canceling the editing of a specific Grid cell. To cancel the editing, the function should return false.
            /// function (row, datafield, columntype)
            /// </summary>
            public _function cellbeginedit
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a cell's value is going to be changed The Grid passes 5 parameters to it - row index, column data field, column type, old cell value, new cell value. The function can be used for modifying the edited value.
            /// function (row, datafield, columntype, oldvalue, newvalue)
            /// </summary>
            public _function cellvaluechanging
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// Aggregate functions:
            /// 'avg' - Average aggregate
            /// 'count' - Count aggregate
            /// 'min' - Min aggregate
            /// 'max' - Max aggregate
            /// 'sum' - Sum aggregate
            /// 'product' - Product aggregate
            /// 'stdev' - Standard deviation on a sample.
            /// 'stdevp' - Standard deviation on an entire population.
            /// 'varp' - Variance on an entire population.
            /// 'var' - Variance on a sample.
            /// 
            /// Custom Aggregate
            /// 
            /// aggregates: [{ 'In Stock':
            ///     function (aggregatedValue, currentValue) {
            ///         if (currentValue) {
            ///             return aggregatedValue + 1;
            ///         }
            ///         return aggregatedValue;
            ///     }
            /// }
            /// 
            /// Custom Aggregate which aggregates values from two columns 
            /// 
            /// { text: 'Price', datafield: 'price', cellsalign: 'right', cellsformat: 'c2', aggregates: [{ 'Total':
            ///             function (aggregatedValue, currentValue, column, record) {
            ///                 var total = currentValue * parseInt(record['quantity']);
            ///                 return aggregatedValue + total;
            ///             }
            ///         }]                  
            ///  }
            /// 
            /// 'In Stock' - the aggregate's display name. The function has 2 params - the aggregated value and the current value. It should return an aggregated value.
            /// </summary>
            public string aggregates
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom rendering function of the column's aggregates. The function gets passed one parameter - the column's aggregates.
            /// function (aggregates)
            /// </summary>
            public _function aggregatesrenderer
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets whether the menu button is displayed when the user moves the mouse cursor over the column's header.
            /// </summary>
            public bool menu
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a widget from the filter row is created.You can use this callback function to set up additional settings of the filter widget.The Grid passes 3 parameters to it - column, the column's HTML element and the filter widget.
            /// function (column, columnElement, widget)
            /// </summary>
            public _function createfilterwidget
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the filter's type. 
            /// </summary>
            public filtertype filtertype
            {
                get { return _get<filtertype>(); }
                set { _set(value); }
            }
            /// <summary>
            /// determines the filter condition of columns with filtertype equal to 'textbox' or 'number'. 
            /// </summary>
            public filter_conditions filtercondition
            {
                get { return _get<filter_conditions>(); }
                set { _set(value); }
            }
            /// <summary>
            /// callback function that is called when the column is rendered.You can use it to set additional settings to the column's header element.
            /// function (columnHeaderElement)
            /// </summary>
            public _function rendered
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// determines whether the column will be exported when the Grid's export method is called.
            /// </summary>
            public bool exportable
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            public bool exporting
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            /// <summary>
            /// enables or disables the column dragging
            /// </summary>
            public bool draggable
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            /// <summary>
            /// enables or disables whether null values are allowed.
            /// </summary>
            public bool nullable
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            public bool clipboard
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            /// <summary>
            /// determines whether tooltips are enabled.
            /// </summary>
            public bool enabletooltips
            {
                get { return _get<bool>(); }
                set { _set(value); }
            }
            /// <summary>
            /// determines the name of the column's parent group.
            /// </summary>
            public string columngroup
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets the auto-filter timeout delay for 'text' and 'number' filters in the filter row.Default value: 800
            /// </summary>
            public string filterdelay
            {
                get { return _get<string>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a widget in the ever present row(showeverpresentrow should be true) should reset its value.
            /// function (htmlElement)
            /// </summary>
            public _function reseteverpresentrowwidgetvalue
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a widget in the ever present row(showeverpresentrow should be true) should return its value.
            /// function (datafield, htmlElement)
            /// </summary>
            public _function geteverpresentrowwidgetvalue
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a widget in the ever present row(showeverpresentrow should be true) is created.datafield is the column's datafield. htmlElement is the Cell's DIV tag.popup is the popup displayed below the cell.addRowCallback is a function which you can call to trigger the "Add" action.
            /// function (datafield, htmlElement, popup, addRowCallback)
            /// </summary>
            public _function createeverpresentrowwidget
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a widget in the ever present row(showeverpresentrow should be true) is being initialized.
            /// function (datafield, htmlElement, popup) 
            /// </summary>
            public _function initeverpresentrowwidget
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a widget in the ever present row(showeverpresentrow should be true) should validate its value.
            /// function (datafield, value. rowValues)
            /// </summary>
            public _function validateeverpresentrowwidgetvalue
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
            /// <summary>
            /// sets a custom function which is called when a widget in the ever present row(showeverpresentrow should be true) is destroyed.The function is useful for destroying custom widgets.
            /// function (htmlElement)
            /// </summary>
            public _function destroyeverpresentrowwidget
            {
                get { return _get<_function>(); }
                set { _set(value); }
            }
        }
        //public class ext : _jqxBase { }
        #region public class columngroup
        public class columngroup
        {
            /// <summary>
            /// sets the column header's name.
            /// </summary>
            public string name;
            public string text;
            /// <summary>
            /// sets the column header's parent group name.
            /// </summary>
            public string parentgroup;
            /// <summary>
            /// sets the column header's alignment to 'left', 'center' or 'right'.
            /// </summary>
            public alignment align;

        }
        #endregion
        #region public class cell
        public class cell
        {
            /// <summary>
            /// cell's value.
            /// </summary>
            public object value;
            /// <summary>
            /// cell's row number.
            /// </summary>
            public int row;
            /// <summary>
            /// column's datafield.
            /// </summary>
            public string column;
        }
        #endregion
        #region public class filter
        #endregion
    }
}