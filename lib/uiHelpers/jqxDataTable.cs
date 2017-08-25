using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public enum filterMode { @default, simple, advanced }
    public enum pagerMode { @default, advanced }
    public enum selectionMode { multipleRows, singleRow, custom }
    public enum pageSizeMode { @default, root }
    public class jqxDataTable : jqxBase
    {
        public class column
        {
        }
        #region public class columnGroup
        public class columnGroup
        {
            /// <summary>
            /// string property which determines the parent group's name.
            /// </summary>
            public string parentGroup;
            /// <summary>
            /// string property which determines the group's name.
            /// </summary>
            public string name;
            /// <summary>
            /// string property which determines the column header's alignment. Possible values: 'left', 'center' or 'right'.
            /// </summary>
            public alignment align;
        }
        #endregion
        #region public class editSetting
        [JsonObject]
        public class editSetting
        {
            [JsonProperty]
            public bool? saveOnPageChange;
            [JsonProperty]
            public bool? saveOnBlur;
            [JsonProperty]
            public bool? saveOnSelectionChange;
            [JsonProperty]
            public bool? cancelOnEsc;
            [JsonProperty]
            public bool? saveOnEnter;
            [JsonProperty]
            public bool? editSingleCell;
            [JsonProperty]
            public bool? editOnDoubleClick;
            [JsonProperty]
            public bool? editOnF2;
        }
        #endregion
        #region public class exportSetting
        public class exportSetting
        {
            /// <summary>
            /// determines whether to export the column's header.
            /// </summary>
            public bool columnsHeader;
            /// <summary>
            /// determines whether to export the hidden columns.
            /// </summary>
            public bool hiddenColumns;
            /// <summary>
            /// determines the URL of the save-file.php.
            /// </summary>
            public string serverURL;
            /// <summary>
            /// determines the char set.
            /// </summary>
            public string characterSet;
            public bool collapsedRecords;
            /// <summary>
            /// determines whether to export all records or to take also the filtering and sorting into account.
            /// </summary>
            public bool recordsInView;
            /// <summary>
            /// determines the file's name. Set this to null if you want to export the data to a local variable.
            /// </summary>
            public string fileName;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Sets or gets whether the jqxDataTable automatically alternates row colors.
        /// </summary>
        public bool altRows
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the aggregates bar. Aggregates bar is displayed after setting showAggregates to true.
        /// </summary>
        public int aggregatesHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the loading html element with animated gif is automatically displayed by the widget during the data binding process.
        /// </summary>
        public bool autoShowLoadElement
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the jqxDataTable automatically calculates the rows height and wraps the cell text.
        /// </summary>
        public bool autoRowHeight
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the columns header.
        /// </summary>
        public int columnsHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets the jqxDataTable's columns.
        /// </summary>
        public column[] columns
        {
            get { return _get<column[]>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets the jqxDataTable's column groups.
        /// </summary>
        public columnGroup[] columnGroups
        {
            get { return _get<columnGroup[]>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDataTable's columnsResize.
        /// </summary>
        public bool columnsResize
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDataTable's columnsReorder.
        /// </summary>
        public bool columnsReorder
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the jqxDataTable editing is enabled.
        /// </summary>
        public bool editable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDataTable's edit settings.
        /// </summary>
        public editSetting editSettings
        {
            get { return _get(create: () => new editSetting()); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether row highlighting is enabled.
        /// </summary>
        public bool enableHover
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the default text selection of the web browser.
        /// </summary>
        public bool enableBrowserSelection
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the filterHeight property.
        /// </summary>
        public int filterHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables/Disables the filtering feature.
        /// </summary>
        public bool filterable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the Filter's mode. Possible values: "default", "simple" and "advanced"
        /// </summary>
        public filterMode filterMode
        {
            get { return _get<filterMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which allows you to customize the rendering of the group headers.
        /// function(value, rowData, level)
        /// </summary>
        public _function groupsRenderer
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDataTable's data groups. Set this property if you want to display the data grouped by a set of column(s).
        /// </summary>
        public string[] groups
        {
            get { return _get<string[]>(); }
            set { _set(value); }
        }

        public int headerZIndex
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public _function handleKeyboardNavigation
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public int indentWidth
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is used for initialization of the expanded row's details. The function is called just once when the row is expanded for first time.
        /// function (id, row, element, rowinfo) 
        ///     id/key - expanded row's id/key.
        ///     dataRow - the expanded row as a set of Key/Value pairs.
        ///     element - the row's details HTML element as a jQuery selector.
        ///     rowInfo - object which enables you to modify the height of the row details by setting the rowInfo's detailsHeight
        /// </summary>
        public _function initRowDetails
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public string loadingErrorMessage
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Applies a localization to the jqxDataTable's Strings.
        /// </summary>
        public object localization
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the jqxDataTable's Pager(s). Pager(s) is(are) displayed after setting pageable to true.
        /// </summary>
        public int pagerHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the rows count per page when paging is enabled.
        /// </summary>
        public int pageSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDataTable's page size options when paging is enabled and the pagerMode property is set to "advanced".
        /// </summary>
        public int[] pageSizeOptions
        {
            get { return _get<int[]>(); }
            set { _set(value); }
        }

        public bool pageable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the jqxDataTable is in paging mode.
        /// </summary>
        public verticalPosition pagerPosition
        {
            get { return _get<verticalPosition>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the Pager's mode. Possible values: "default" and "advanced"
        /// </summary>
        public pagerMode pagerMode
        {
            get { return _get<pagerMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the count of the buttons displayed on the Pager when pagerMode is set to "default".
        /// </summary>
        public int pagerButtonsCount
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables custom rendering of the Pager.
        /// function()
        /// </summary>
        public _function pagerRenderer
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called when the jqxDataTable is rendered and data binding is completed..
        /// function()
        /// </summary>
        public _function ready
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables custom rendering of the Toolbar.
        /// function(toolBar)
        ///     toolBar - The jQuery selection of the Toolbar HTML Element
        /// </summary>
        public _function rendertoolbar
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the jqxDataTable rows have details and can be expanded/collapsed. See the initRowDetails for initialization of the row details.
        /// </summary>
        public bool rowDetails
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables custom rendering of the Statusbar.
        /// function(statusBar)
        ///     statusBar - The jQuery selection of the Statusbar HTML Element
        /// </summary>
        public _function renderStatusbar
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called after the rendering of the jqxDataTable's row.
        ///     function ()
        /// </summary>
        public _function rendered
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called before the rendering of the jqxDataTable's rows.
        ///     function()
        /// </summary>
        public _function rendering
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables/Disables the sorting feature.
        /// </summary>
        public bool sortable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the jqxDataTable's Toolbar is visible.
        /// </summary>
        public bool showtoolbar
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the jqxDataTable's Statusbar is visible.
        /// </summary>
        public bool showstatusbar
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the Statusbar. Statusbar is displayed after setting showStatusbar to true.
        /// </summary>
        public int statusBarHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the Paging, Sorting and Filtering are handled by a Server and jqxDataTable sends Ajax requests to a Server and displays the returned data.When the current page, page size, sort order or sort column is changed, jqxDataTable will automatically perform a new data binding with the updated parameters.For server synchronization after adding, removing, updating rows, see the source property documentation.
        /// 
        /// By default, the jqxDataTable sends the following data to the server:
        ///     sortdatafield - the sort column's datafield.
        ///     sortorder - the sort order - 'asc', 'desc' or ''.
        ///     pagenum - the current page's number when the paging feature is enabled.
        ///     pagesize - the page's size which represents the number of rows displayed in the view.
        ///     filterscount - the number of filters applied to the jqxDataTable.
        ///     filtervalue - the filter's value. The filtervalue name for the first filter is "filtervalue0", for the second filter is "filtervalue1" and so on.
        ///     filtercondition - the filter's condition. The condition can be any of these: "CONTAINS", "DOES_NOT_CONTAIN", "EQUAL", "EQUAL_CASE_SENSITIVE", NOT_EQUAL","GREATER_THAN", "GREATER_THAN_OR_EQUAL", "LESS_THAN", "LESS_THAN_OR_EQUAL", "STARTS_WITH", "STARTS_WITH_CASE_SENSITIVE", "ENDS_WITH", "ENDS_WITH_CASE_SENSITIVE", "NULL", "NOT_NULL", "EMPTY", "NOT_EMPTY".
        ///     filterdatafield - the filter column's datafield.
        ///     filteroperator - the filter's operator - 0 for "AND" and 1 for "OR".
        /// </summary>
        public bool serverProcessing
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the selection mode. Possible values: "multipleRows", "singleRow" and "custom". In the "custom" mode, rows could be selected/unselected only through the API.
        /// </summary>
        public selectionMode selectionMode
        {
            get { return _get<selectionMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the size of the scrollbars.
        /// </summary>
        public int scrollBarSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public int touchScrollBarSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the jqxDataTable's Aggregates bar is visible.
        /// </summary>
        public bool showAggregates
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDataTable's columns visibility.
        /// </summary>
        public bool showHeader
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool autoBind
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public _function beginEdit
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public _function endEdit
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public bool autokoupdates
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool columnsVirtualization
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the Data Export settings used by jqxDataTable when exportData is called. See also the exportData method.
        /// </summary>
        public exportSetting exportSettings
        {
            get { return _get<exportSetting>(); }
            set { _set(value); }
        }

        public object source
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the Toolbar. Toolbar is displayed after setting showToolbar to true.
        /// </summary>
        public bool toolbarHeight
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the incremental search is enabled. The feature allows you to quickly find and select data records by typing when the widget is on focus.
        /// </summary>
        public bool incrementalSearch
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        #endregion

        #region Events

        /// <summary>
        /// This event is triggered when the jqxDataTable binding is completed. *Bind to that event before the jqxDataTable's initialization. Otherwise, if you are populating the widget from a local data source and bind to bindingComplete after the initialization, the event could be already raised when you attach an event handler to it.
        /// </summary>
        public _event OnBindingComplete
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the jqxDataTable sort order or sort column is changed.
        /// </summary>
        public _event OnSort
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the jqxDataTable's rows filter is changed.
        /// </summary>
        public _event OnFilter
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when the jqxDataTable's current page is changed.
        /// </summary>
        public _event OnPageChanged
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        public _event OnPageSizeChanged
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row is clicked.
        /// </summary>
        public _event OnRowClick
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row is double-clicked.
        /// </summary>
        public _event OnRowDoubleClick
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a cell value is changed.
        /// </summary>
        public _event OnCellValueChanged
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row edit begins.
        /// </summary>
        public _event OnRowBeginEdit
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row edit ends.
        /// </summary>
        public _event OnRowEndEdit
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row is selected.
        /// </summary>
        public _event OnRowSelect
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row is unselected.
        /// </summary>
        public _event OnRowUnselect
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        public _event OnRowCheck
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        public _event OnRowUncheck
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a column is resized.
        /// </summary>
        public _event OnColumnResized
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the column's order is changed.
        /// </summary>
        public _event OnColumnReordered
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row is expanded.
        /// </summary>
        public _event OnRowExpand
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row is collapsed.
        /// </summary>
        public _event OnRowCollapse
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a cell edit begins. Note: To turn on cell editing, you should set the editSettings property and make sure that its editSingleCell sub property is set to true.
        /// </summary>
        public _event OnCellBeginEdit
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a cell edit ends. Note: To turn on cell editing, you should set the editSettings property and make sure that its editSingleCell sub property is set to true.
        /// </summary>
        public _event OnCellEndEdit
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }
        #endregion
    }
    public class jqxTreeGrid : jqxDataTable
    {
    }
    public class jqxDataTable2 : jqxBase
    {
        public class column
        {
        }
        #region public class columnGroup
        public class columnGroup
        {
            /// <summary>
            /// string property which determines the parent group's name.
            /// </summary>
            public string parentGroup;
            /// <summary>
            /// string property which determines the group's name.
            /// </summary>
            public string name;
            /// <summary>
            /// string property which determines the column header's alignment. Possible values: 'left', 'center' or 'right'.
            /// </summary>
            public alignment align;
        }
        #endregion
        #region public class editSetting
        public class editSetting
        {
            public bool saveOnPageChange;
            public bool saveOnBlur;
            public bool saveOnSelectionChange;
            public bool cancelOnEsc;
            public bool saveOnEnter;
            public bool editSingleCell;
            public bool editOnDoubleClick;
            public bool editOnF2;
        }
        #endregion
        #region public class exportSetting
        public class exportSetting
        {
            /// <summary>
            /// determines whether to export the column's header.
            /// </summary>
            public bool columnsHeader;
            /// <summary>
            /// determines whether to export the hidden columns.
            /// </summary>
            public bool hiddenColumns;
            /// <summary>
            /// determines the URL of the save-file.php.
            /// </summary>
            public string serverURL;
            /// <summary>
            /// determines the char set.
            /// </summary>
            public string characterSet;
            public bool collapsedRecords;
            /// <summary>
            /// determines whether to export all records or to take also the filtering and sorting into account.
            /// </summary>
            public bool recordsInView;
            /// <summary>
            /// determines the file's name. Set this to null if you want to export the data to a local variable.
            /// </summary>
            public string fileName;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Sets or gets whether the jqxDataTable automatically alternates row colors.
        /// </summary>
        public bool altRows
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the aggregates bar. Aggregates bar is displayed after setting showAggregates to true.
        /// </summary>
        public int aggregatesHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the loading html element with animated gif is automatically displayed by the widget during the data binding process.
        /// </summary>
        public bool autoShowLoadElement
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the jqxDataTable automatically calculates the rows height and wraps the cell text.
        /// </summary>
        public bool autoRowHeight
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the columns header.
        /// </summary>
        public int columnsHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets the jqxDataTable's columns.
        /// </summary>
        public column[] columns
        {
            get { return _get<column[]>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets the jqxDataTable's column groups.
        /// </summary>
        public columnGroup[] columnGroups
        {
            get { return _get<columnGroup[]>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDataTable's columnsResize.
        /// </summary>
        public bool columnsResize
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDataTable's columnsReorder.
        /// </summary>
        public bool columnsReorder
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the jqxDataTable editing is enabled.
        /// </summary>
        public bool editable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDataTable's edit settings.
        /// </summary>
        public editSetting editSettings
        {
            get { return _get<editSetting>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether row highlighting is enabled.
        /// </summary>
        public bool enableHover
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables or disables the default text selection of the web browser.
        /// </summary>
        public bool enableBrowserSelection
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the filterHeight property.
        /// </summary>
        public int filterHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables/Disables the filtering feature.
        /// </summary>
        public bool filterable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the Filter's mode. Possible values: "default", "simple" and "advanced"
        /// </summary>
        public filterMode filterMode
        {
            get { return _get<filterMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which allows you to customize the rendering of the group headers.
        /// function(value, rowData, level)
        /// </summary>
        public _function groupsRenderer
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDataTable's data groups. Set this property if you want to display the data grouped by a set of column(s).
        /// </summary>
        public string[] groups
        {
            get { return _get<string[]>(); }
            set { _set(value); }
        }

        public int headerZIndex
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public _function handleKeyboardNavigation
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public int indentWidth
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is used for initialization of the expanded row's details. The function is called just once when the row is expanded for first time.
        /// function (id, row, element, rowinfo) 
        ///     id/key - expanded row's id/key.
        ///     dataRow - the expanded row as a set of Key/Value pairs.
        ///     element - the row's details HTML element as a jQuery selector.
        ///     rowInfo - object which enables you to modify the height of the row details by setting the rowInfo's detailsHeight
        /// </summary>
        public _function initRowDetails
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public string loadingErrorMessage
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Applies a localization to the jqxDataTable's Strings.
        /// </summary>
        public object localization
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the jqxDataTable's Pager(s). Pager(s) is(are) displayed after setting pageable to true.
        /// </summary>
        public int pagerHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the rows count per page when paging is enabled.
        /// </summary>
        public int pageSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDataTable's page size options when paging is enabled and the pagerMode property is set to "advanced".
        /// </summary>
        public int[] pageSizeOptions
        {
            get { return _get<int[]>(); }
            set { _set(value); }
        }

        public bool pageable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the jqxDataTable is in paging mode.
        /// </summary>
        public verticalPosition pagerPosition
        {
            get { return _get<verticalPosition>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the Pager's mode. Possible values: "default" and "advanced"
        /// </summary>
        public pagerMode pagerMode
        {
            get { return _get<pagerMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the count of the buttons displayed on the Pager when pagerMode is set to "default".
        /// </summary>
        public int pagerButtonsCount
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables custom rendering of the Pager.
        /// function()
        /// </summary>
        public _function pagerRenderer
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called when the jqxDataTable is rendered and data binding is completed..
        /// function()
        /// </summary>
        public _function ready
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables custom rendering of the Toolbar.
        /// function(toolBar)
        ///     toolBar - The jQuery selection of the Toolbar HTML Element
        /// </summary>
        public _function rendertoolbar
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the jqxDataTable rows have details and can be expanded/collapsed. See the initRowDetails for initialization of the row details.
        /// </summary>
        public bool rowDetails
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables custom rendering of the Statusbar.
        /// function(statusBar)
        ///     statusBar - The jQuery selection of the Statusbar HTML Element
        /// </summary>
        public _function renderStatusbar
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called after the rendering of the jqxDataTable's row.
        ///     function ()
        /// </summary>
        public _function rendered
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is called before the rendering of the jqxDataTable's rows.
        ///     function()
        /// </summary>
        public _function rendering
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Enables/Disables the sorting feature.
        /// </summary>
        public bool sortable
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the jqxDataTable's Toolbar is visible.
        /// </summary>
        public bool showtoolbar
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the jqxDataTable's Statusbar is visible.
        /// </summary>
        public bool showstatusbar
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the Statusbar. Statusbar is displayed after setting showStatusbar to true.
        /// </summary>
        public int statusBarHeight
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets whether the Paging, Sorting and Filtering are handled by a Server and jqxDataTable sends Ajax requests to a Server and displays the returned data.When the current page, page size, sort order or sort column is changed, jqxDataTable will automatically perform a new data binding with the updated parameters.For server synchronization after adding, removing, updating rows, see the source property documentation.
        /// 
        /// By default, the jqxDataTable sends the following data to the server:
        ///     sortdatafield - the sort column's datafield.
        ///     sortorder - the sort order - 'asc', 'desc' or ''.
        ///     pagenum - the current page's number when the paging feature is enabled.
        ///     pagesize - the page's size which represents the number of rows displayed in the view.
        ///     filterscount - the number of filters applied to the jqxDataTable.
        ///     filtervalue - the filter's value. The filtervalue name for the first filter is "filtervalue0", for the second filter is "filtervalue1" and so on.
        ///     filtercondition - the filter's condition. The condition can be any of these: "CONTAINS", "DOES_NOT_CONTAIN", "EQUAL", "EQUAL_CASE_SENSITIVE", NOT_EQUAL","GREATER_THAN", "GREATER_THAN_OR_EQUAL", "LESS_THAN", "LESS_THAN_OR_EQUAL", "STARTS_WITH", "STARTS_WITH_CASE_SENSITIVE", "ENDS_WITH", "ENDS_WITH_CASE_SENSITIVE", "NULL", "NOT_NULL", "EMPTY", "NOT_EMPTY".
        ///     filterdatafield - the filter column's datafield.
        ///     filteroperator - the filter's operator - 0 for "AND" and 1 for "OR".
        /// </summary>
        public bool serverProcessing
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the selection mode. Possible values: "multipleRows", "singleRow" and "custom". In the "custom" mode, rows could be selected/unselected only through the API.
        /// </summary>
        public selectionMode selectionMode
        {
            get { return _get<selectionMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the size of the scrollbars.
        /// </summary>
        public int scrollBarSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        public int touchScrollBarSize
        {
            get { return _get<int>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the jqxDataTable's Aggregates bar is visible.
        /// </summary>
        public bool showAggregates
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the jqxDataTable's columns visibility.
        /// </summary>
        public bool showHeader
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool autoBind
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public _function beginEdit
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public _function endEdit
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public bool autokoupdates
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool columnsVirtualization
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines the Data Export settings used by jqxDataTable when exportData is called. See also the exportData method.
        /// </summary>
        public exportSetting exportSettings
        {
            get { return _get<exportSetting>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the height of the Toolbar. Toolbar is displayed after setting showToolbar to true.
        /// </summary>
        public bool toolbarHeight
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether the incremental search is enabled. The feature allows you to quickly find and select data records by typing when the widget is on focus.
        /// </summary>
        public bool incrementalSearch
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        #endregion

        #region Events

        /// <summary>
        /// This event is triggered when the jqxDataTable binding is completed. *Bind to that event before the jqxDataTable's initialization. Otherwise, if you are populating the widget from a local data source and bind to bindingComplete after the initialization, the event could be already raised when you attach an event handler to it.
        /// </summary>
        public _event OnBindingComplete
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the jqxDataTable sort order or sort column is changed.
        /// </summary>
        public _event OnSort
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the jqxDataTable's rows filter is changed.
        /// </summary>
        public _event OnFilter
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when the jqxDataTable's current page is changed.
        /// </summary>
        public _event OnPageChanged
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        public _event OnPageSizeChanged
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row is clicked.
        /// </summary>
        public _event OnRowClick
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row is double-clicked.
        /// </summary>
        public _event OnRowDoubleClick
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a cell value is changed.
        /// </summary>
        public _event OnCellValueChanged
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row edit begins.
        /// </summary>
        public _event OnRowBeginEdit
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row edit ends.
        /// </summary>
        public _event OnRowEndEdit
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row is selected.
        /// </summary>
        public _event OnRowSelect
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row is unselected.
        /// </summary>
        public _event OnRowUnselect
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        public _event OnRowCheck
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        public _event OnRowUncheck
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when a column is resized.
        /// </summary>
        public _event OnColumnResized
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This event is triggered when the column's order is changed.
        /// </summary>
        public _event OnColumnReordered
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row is expanded.
        /// </summary>
        public _event OnRowExpand
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a row is collapsed.
        /// </summary>
        public _event OnRowCollapse
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a cell edit begins. Note: To turn on cell editing, you should set the editSettings property and make sure that its editSingleCell sub property is set to true.
        /// </summary>
        public _event OnCellBeginEdit
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }

        /// <summary>
        /// This is triggered when a cell edit ends. Note: To turn on cell editing, you should set the editSettings property and make sure that its editSingleCell sub property is set to true.
        /// </summary>
        public _event OnCellEndEdit
        {
            get { return _get<_event>(); }
            set { _set(value); }
        }
        #endregion
    }
    public class jqxTreeGrid2 : jqxDataTable2
    {

        #region Properties

        /// <summary>
        /// Sets or gets the Pager Size's mode. Possible values: "default" and "root"
        /// </summary>
        public pageSizeMode pageSizeMode
        {
            get { return _get<pageSizeMode>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether checkboxes are displayed or not.
        /// </summary>
        public bool checkboxes
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether changing a checkbox state affects the parent/child records check state. Note: "checkboxes" property value is expected to be true.
        /// </summary>
        public bool hierarchicalCheckboxes
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether icons are displayed or not.
        /// </summary>
        public bool icons
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Determines whether jqxTreeGrid would calculate summary values for all values within a group of records and would display the result inside footer cell after each group.
        /// </summary>
        public bool showSubAggregates
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Callback function which is used for rendering of the row's details.
        /// function (rowKey, row)
        ///     id/key - row's id/key.
        ///     dataRow - the row as a set of Key/Value pairs.
        /// </summary>
        public _function rowDetailsRenderer
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// By defining that function you can load data into jqxTreeGrid dynamically. For each record only when required, jqxTreeGrid calls virtualModeCreateRecords function and this allows you to provide records on demand. In order to enable the load on demand feature, you should also define the virtualModeRecordCreating function.
        /// function(expandedRecord, done)
        ///     Row Key/ID - unique ID. If null is passed, that the function's result would be the records of the first hierarchy level.
        ///     Done - callback function. Call it when data is loaded and pass the loaded data as an Array.
        /// </summary>
        public _function virtualModeCreateRecords
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        /// <summary>
        /// By defining that function you can initialize the dynamic data that you load into jqxTreeGrid. In order to enable the load on demand feature, you should also define virtualModeCreateRecords function.
        ///     function(record)
        ///         record - Object with the following reserved members:
        ///             checked - Boolean value.Determines the row's checked state.
        ///             expanded - Boolean value. Determines the row's expanded state.
        ///             icon - String value. Determines the row's icon url.
        ///             leaf - Boolean value. Determines whether the row is a leaf in the hierarchy. When you set leaf to true, the expand/collapse button would not be displayed.
        ///             level - Integer value. Returns the row's hierarchy level.
        ///             selected - Boolean value. Determines the row's selected state.
        /// </summary>
        public _function virtualModeRecordCreating
        {
            get { return _get<_function>(); }
            set { _set(value); }
        }

        public bool loadingFailed
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        #endregion
    }
}
