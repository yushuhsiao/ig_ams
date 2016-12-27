/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
;
(function ($) {
    for (var _name in $.jqx) {
        if (_name.startsWith('_jqx')) {
            new function (widget) {
                var $src = this;
                $src.defineInstance = widget.prototype.defineInstance;
                $src.createInstance = widget.prototype.createInstance;
                widget.prototype.defineInstance = function () {
                    var opt1 = $src.defineInstance.apply(this, arguments);
                    this.event = {};
                    if (opt1 != null)
                        opt1.event = this.event;
                    return opt1;
                };
                widget.prototype.createInstance = function () {
                    var $this = this;
                    for (var e1 in $this.event) {
                        var e2 = $this.event[e1];
                        if ($.isFunction(e2)) {
                            if ($.isArray($this['events'])) {
                                $this['events'].forEach(function (e3) {
                                    var e4 = e3.toLowerCase();
                                    if ((e1 === '*') || (e1 === e3) || (e1 == e4)) {
                                        $this.host.on(e4, e2);
                                    }
                                });
                            }
                            else if (e1 !== '*') {
                                $this.host.on(e1, e2);
                            }
                        }
                    }
                    return $src.createInstance.apply(this, arguments);
                };
            }($.jqx[_name]);
        }
    }
    $.extend($.jqx._jqxTree.prototype, {
        getSelectedValue: function () {
            var n = this.getSelectedItem();
            if (n == null)
                return null;
            else
                return n.value;
        },
        getItemByID: function (id) {
            var items = this.getItems();
            for (var i = 0; i < items.length; i++) {
                if (items[i].id == id)
                    return items[i];
            }
        }
    });

    $.EditControl = function (opt1, opt2) {
        var that = this;
        var opt = $.extend(true, {
            modal: null,
            OnCreate: function () { },

            updaterow: false,
            update_url: null,
            OnUpdate: function (data, jqXHR, settings) { },
            OnUpdateComplete: function (data, textStatus, jqXHR) { },

            load_url: null,
            OnLoading: function (data, jqXHR, settings) { },
            OnLoadComplete: function (data, textStatus, jqXHR) { },
        }, opt1, opt2);
        if (opt.$scope == null) { opt.$scope = { $applyAsync: $.noop } };
        if (opt.name == null) opt.name = '';
        opt.$scope[opt.name] = that;
        that.Row = opt.row;
        var _edit = null;
        var _ajax_busy = false;
        var _error = null;

        that.Create = function () {
            _edit = that.Row = opt.OnCreate();
            _error = null;
            $(opt.modal).modal();
        }
        that.EndCreate = function (success, url) {
            if (success === true)
                that.UpdateData(url);
            else
                $(opt.modal).modal('hide');
        }

        function _ajax(settings) {
            if (_ajax_busy === true) return;
            _ajax_busy = true;
            _error = null;
            var beforeSend = settings.beforeSend;
            var success = settings.success;
            try {
                $.ajax($.extend(true, settings, {
                    type: 'post',
                    contentType: 'application/json',
                    dataType: 'json',
                    cache: false,
                    async: true,
                    beforeSend: function (jqXHR, settings) {
                        settings.url = null;
                        if (beforeSend.call(this, jqXHR, settings) === false) {
                            _ajax_busy = false;
                            return false;
                        }
                        settings.data = JSON.stringify(settings.data);
                    },
                    success: function (data, textStatus, jqXHR) {
                        if (data == null) {
                            _error = { Message: 'No result' };
                        }
                        else {
                            success.call(this, data, textStatus, jqXHR);
                        }
                        try {
                            if (window.frameElement != null) {
                                if ($.isFunction(window.frameElement.OnRowUpdate)) {
                                    if (opt.updaterow) window.frameElement.OnRowUpdate.call(that, opt.name, data);
                                }
                            }
                        }
                        catch (ex) { }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        var data = jqXHR.responseJSON;
                        _error = data;
                        if (data != null) {
                            if (data.hasOwnProperty('Data') && data.hasOwnProperty('Status')) {
                            }
                        }
                    },
                    complete: function () {
                        _ajax_busy = false;
                        opt.$scope.$applyAsync();
                    }
                }));
            }
            catch (ex) {
                _ajax_busy = false;
                throw ex;
            }
        }

        Object.defineProperty(that, 'IsEditing', {
            get: function () { return _edit != null; }
        });
        Object.defineProperty(that, 'IsLoading', {
            get: function () { return _loading; }
        });
        that.BeginEdit = function () {
            if (_edit == null) {
                _edit = that.Row;
                that.Row = $.extend(true, {}, _edit);
            }
        }
        that.EndEdit = function (success) {
            if (_edit == null)
                return;
            if (success !== true) {
                that.Row = _edit;
                _edit = null;
                return;
            }
            that.UpdateData();
        }
        that.UpdateData = function (url) {
            _ajax({
                beforeSend: function (jqXHR, settings) {
                    settings.url = url;
                    settings.data = that.Row;
                    var tmp = opt.OnUpdate.call(that, that.Row, jqXHR, settings);
                    if (tmp === false)
                        return false;
                    if (tmp != null)
                        settings.data = tmp;
                    if ((settings.url == null) || (settings.url == '')) {
                        settings.url = opt.update_url;
                        if ((settings.url == null) || (settings.url == ''))
                            throw "update_url is null";
                    }
                },
                success: function (data, textStatus, jqXHR) {
                    var tmp = opt.OnUpdateComplete.call(that, data, textStatus, jqXHR);
                    if (tmp != null)
                        data = tmp;
                    that.Row = data;
                    _edit = null;
                    $(opt.modal).modal('hide');
                },
            });
        }
        that.LoadData = function (force) {
            if (_edit != null)
                return;
            if ((that.Row != null) && (force !== true))
                return;
            _ajax({
                beforeSend: function (jqXHR, settings) {
                    settings.data = {};
                    if (opt.OnLoading.call(that, settings.data, jqXHR, settings) === false)
                        return false;
                    if ((settings.url == null) || (settings.url == '')) {
                        settings.url = opt.load_url;
                        if ((settings.url == null) || (settings.url == ''))
                            throw "load_url is null";
                    }
                },
                success: function (data, textStatus, jqXHR) {
                    var tmp = opt.OnLoadComplete.call(that, data, textStatus, jqXHR);
                    if (tmp != null)
                        data = tmp;
                    that.Row = data;
                },
            });
        }

        that.err_class = function (field_name) {
            if (_error == null)
                return '';
            if (field_name == null)
                return 'has-error';
            else if (_error[field_name] != null)
                return 'has-error';
        }
        that.err_msg = function (field_name) {
            if (_error == null)
                return null;
            if (field_name == null)
                return _error;
            else
                return _error[field_name];
        }
    }

    function _replace(obj, name, func) {
        var old = obj[name];
        if (!$.isFunction(old))
            old = $.noop;
        obj[name] = function () {
            var ret = old.apply(this, arguments);
            func.apply(this, arguments);
            return ret;
        }
    }

    function _true() { return true; }
    function _false() { return false; }

    $.jqx.row_collection = function (src) {
        Object.defineProperty(this, 'records', {
            get: function () { return src.records; },
            set: function (value) { src.records = value; }
        });
        Object.defineProperty(this, 'owner', {
            get: function () { return src.owner; },
            set: function (value) { src.owner = value; }
        });
        Object.defineProperty(this, 'updating', {
            get: function () { return src.updating; },
            set: function (value) { src.updating = value; }
        });
        this.beginupdate = src.beginupdate;
        this.resumeupdate = src.resumeupdate;
        this._raiseEvent = src._raiseEvent;
        this.clear = src.clear;
        this.replace = src.replace;
        this.isempty = src.isempty;
        this.initialize = src.initialize;
        this.length = src.length;
        this.indexOf = src.indexOf;
        this.add = src.add;
        this.insertAt = src.insertAt;
        this.remove = src.remove;
        this.removeAt = src.removeAt;
        this.getrow = function (rowid) {
            for (var i in src.records)
                if (src.records[i].boundindex == rowid)
                    return src.records[i];
        }
        this.getrowbyid = function (uid) {
            for (var i in src.records)
                if (src.records[i].bounddata.uid == uid)
                    return src.records[i];
        }
        return this
    };

    $.fn.jqxGridEx = function () {
        var $this = this;
        var $grid;
        var settings = {
            width: '100%', height: "100%",
            altrows: true,
            //filterrowheight: 24,
            columnsheight: 24,
            columnsresize: true,
            rowsheight: 24,
            everpresentrowheight: 24,
            showfiltermenuitems: false,
            pagerbuttonscount: 5,
            pagesize: 50,
            pagesizeoptions: [10, 20, 50, 100],
            enabletooltips: true,
            //processdata: function (data) {
            //    console.log('processData', arguments);
            //},
            source: {
                contenttype: 'application/json',
                dataType: 'json',
                type: 'post',
                datafields: [],
                //processData: function (data) {
                //    console.log('processData', arguments);
                //},
                formatData: function (data) {
                    //console.log('formatData', arguments);
                    var data2 = { jqx: data };
                    $grid.getfilterinformation().forEach(function (f1) {
                        data2[f1.datafield] = f1.filter.getfilters();
                        //if (data2.Filters == null)
                        //    data2.Filters = {};
                        //data2[f1.datafield] = new Array();
                        //f1.filter.getfilters().forEach(function (f2) {
                        //    data2[f1.datafield].push(f2.value);
                        //});
                        //data2[f1.datafield] = f1.filter.getfilters()[0];
                        //f1.filter.getfilters().forEach(function (f2) {
                        //    if (data2.Filters == null)
                        //        data2.Filters = new Array();
                        //    data2.Filters.push({
                        //        DataField: f1.datafield,
                        //        Condition: f2.condition,
                        //        Operator: f2.operator,
                        //        Id: f2.id,
                        //        Value: f2.value
                        //    });
                        //});
                    });
                    //$grid.getfilterinformation().forEach(function (f1) {
                    //    if (data2.Filters == null)
                    //        data2.Filters = new Array();
                    //    f1.filter.getfilters().forEach(function (f2) {
                    //        for (var i = 0; i < data.filterscount; i++) {
                    //            if ((data['filterdatafield' + i] === f1.datafield) &&
                    //                (data['filtercondition' + i] === f2.condition) &&
                    //                (data['filteroperator' + i] === f2.operator)) {
                    //                data2.Filters.push({
                    //                    DataField: f1.datafield,
                    //                    Condition: f2.condition,
                    //                    Operator: f2.operator,
                    //                    Value: data['filtervalue' + i]
                    //                });
                    //            }
                    //        }
                    //    });
                    //    //list_args.Filters[f1.datafield] = values;
                    //});

                    data2.PageSize = data.pagesize;
                    data2.PageNumber = data.pagenum;
                    data2.SortField = data.sortdatafield;
                    data2.SortOrder = data.sortorder;
                    if ($.isFunction($grid.ext.formatData)) {
                        var n = $grid.ext.formatData(data2);
                        if (n != undefined)
                            data2 = n;
                    }
                    return JSON.stringify(data2);
                },
            },
            ext: {},
            initrowdetails: function (index, parentElement, gridElement, datarecord) {
                if ($.isFunction($grid.ext.rowdetails_url)) {
                    var url = $grid.ext.rowdetails_url.apply(this, arguments)
                    if (url != null)
                        $('<iframe style="width: 100%; height: 100%; /*border: 0px solid;*/" src="' + url + '"></iframe>').appendTo(parentElement);
                }
            },
        };
        function translate(column) {
            if ($.isArray(column.translate.source) && (column.cellsrenderer == null)) {
                var $this = this;
                $.extend(true, $this, { id: '_value', text: '_text' }, column.translate);
                column.cellsrenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
                    var n = $this.source.find(function (n) { return n[$this.id] == value; });
                    if (n == null)
                        return defaulthtml;
                    return this.owner._defaultcellsrenderer(n[$this.text], this);
                };
            }
        }
        var datafields = new function () {
            var $this = this;
            $this.values = new Array();
            $this.get_item = function (name) {
                for (var i = 0; i < $this.values.length; i++)
                    if ($this.values[i].name === name)
                        return i;
                return -1;
            };
            $this.set_item = function (datafield) {
                var n1 = $this.get_item(datafield.name);
                if (n1 === -1)
                    $this.values.push(datafield);
                else
                    $this.values[n1] = $.extend(true, $this.values[n1], datafield);
            };
        };
        var columns = new Array();
        for (var i = 0; i < arguments.length; i++) {
            var settings_n = arguments[i];
            settings = $.extend(true, settings, settings_n);
            if (settings_n.hasOwnProperty('source')) {
                if (settings_n.source.hasOwnProperty('datafields') && $.isArray(settings_n.source.datafields)) {
                    settings_n.source.datafields.forEach(datafields.set_item);
                }
            }
            if (settings_n.hasOwnProperty('columns') && $.isArray(settings_n.columns)) {
                settings_n.columns.forEach(function (column) {
                    column.name = $.trim(column.name);
                    if (column.name == undefined) return;
                    for (var n = 0; n < columns.length; n++) {
                        if (columns[n].name === column.name) {
                            columns[n] = $.extend(true, columns[n], column);
                            return;
                        }
                    }
                    columns.push(column);
                });
            }
        }
        if (!$.isBoolean(settings.showtoolbar)) {
            try {
                var selector = settings.showtoolbar;
                $.extend(true, settings, {
                    showtoolbar: true,
                    rendertoolbar: function (toolbar) {
                        $(selector).children().appendTo(toolbar);
                        $(selector).remove();
                    },
                });
            }
            catch (e) { }
        }

        if (settings.row_numbers === true) {
            columns.unshift($.jqx.columns.row_number());
            //for (var n = 0; n < columns.length; n++) {
            //    if (columns[n].name === column.name) {
            //        columns[n] = $.extend(true, columns[n], column);
            //        return;
            //    }
            //}
            //columns.set_item($.jqx.columns.row_number(), 'unshift');
        }
        delete settings.row_numbers;
        if (settings.server_sorting === true) {
            $.extend(true, settings, {
                source: {
                    sort: function (field, is_asc) {
                        $grid.updatebounddata('sort');
                    }
                }
            });
        }
        delete settings.server_sorting;
        if (settings.server_filtering === true) {
            $.extend(true, settings, {
                source: {
                    filter: function (filters, data, count) {
                        $grid.updatebounddata('filter');
                    }
                }
            });
        }
        delete settings.server_filtering;
        if (settings.server_paging === true) {
            $.extend(true, settings, {
                //source: {
                //    beforeprocessing: function (data) {
                //        return data;
                //        // source.totalrecords = data[0].TotalRows;
                //        //console.log('beforeprocessing', arguments);
                //        //return {
                //        //    records: data,
                //        //    totalrecords: 10000,
                //        //};
                //    },
                //},
                pageable: true,
                virtualmode: true,
                rendergridrows: function (params) {
                    return params.data;
                }
            });
        }
        delete settings.server_paging;
        for (var i = 0; i < columns.length; i++) {
            if (typeof columns[i].datafield == 'string')
                columns[i].datafield = {
                    name: columns[i].datafield
                };

            if (columns[i].hasOwnProperty('ColumnDefine')) {
                var ColumnDefine = columns[i].ColumnDefine;
                delete columns[i].ColumnDefine;
                if ($.jqx.columns.hasOwnProperty(ColumnDefine))
                    columns[i] = $.jqx.columns[ColumnDefine](columns[i]);
            }

            if (typeof columns[i].datafield === 'object') {
                datafields.set_item(columns[i].datafield);
                columns[i].datafield = columns[i].datafield.name;
            }
            if (typeof columns[i].translate === 'object') {
                columns[i].translate = new translate(columns[i]);
            }

            if (columns[i].filterdefault != null) {
                var filtergroup = new $.jqx.filter();
                var f1 = filtergroup.createfilter('numericfilter', columns[i].filterdefault, 'EQUAL');
                filtergroup.addfilter('and', f1);
                columns[i].filter = filtergroup;
            }
        }
        settings.ext.columns = settings.columns = columns;//.values;
        settings.ext.datafields = datafields;
        settings.source.datafields = datafields.values;
        this.each(function () {
            Object.defineProperty(this, '$grid', {
                get: function () {
                    return $grid;
                },
                set: function (value) {
                    $grid = value;
                }
            });
        });
        //console.log('jqxGrid', settings);
        //var scope_id = settings.scope_id;
        //delete settings.scope_id;
        this.jqxGrid(settings);
        return $grid;
    };

    var p_jqxGrid = $.extend(true, $.jqx._jqxGrid.prototype);
    $.extend($.jqx._jqxGrid.prototype, {
        _rendercell: function ($grid, column, row, cellvalue, element) {
            var $scope = $grid.$scope;
            if ($grid.editcell && $grid.editrow == undefined) {
                if ($grid.editmode == "selectedrow" && column.editable && $grid.editable) {
                    if ($grid.editcell.row == $grid.getboundindex(row)) {
                        if ($grid._showcelleditor) {
                            if (!$grid.hScrollInstance.isScrolling() && !$grid.vScrollInstance.isScrolling()) {
                                $grid._showcelleditor($grid.editcell.row, column, element, $grid.editcell.init)
                            } else {
                                $grid._showcelleditor($grid.editcell.row, column, element, false, false)
                            }
                            return
                        }
                    }
                } else {
                    if ($grid.editcell.row == $grid.getboundindex(row) && $grid.editcell.column == column.datafield) {
                        $grid.editcell.element = element;
                        if ($grid.editcell.editing) {
                            if ($grid._showcelleditor) {
                                if (!$grid.hScrollInstance.isScrolling() && !$grid.vScrollInstance.isScrolling()) {
                                    $grid._showcelleditor($grid.editcell.row, column, $grid.editcell.element, $grid.editcell.init)
                                } else {
                                    $grid._showcelleditor($grid.editcell.row, column, $grid.editcell.element, $grid.editcell.init, false)
                                }
                                return
                            }
                        }
                    }
                }
            }
            $grid._rendercell_column = column;
            $grid._rendercell_row = row;
            $grid._rendercell_element = element;
            var n = p_jqxGrid._rendercell.apply(this, arguments);
            if (column._opts) {
                if (column._opts.cellsrendered) {
                    //column.cellsrenderer($grid.getboundindex(row), column.datafield, cellvalue, null, column.getcolumnproperties(), row.bounddata, column, row, element)
                    column._opts.cellsrendered($grid.getboundindex(row), column, row, element)
                }
            }
            $grid._rendercell_column = $grid._rendercell_row = $grid._rendercell_element = null;
            return n;
        },
        defineInstance: function () {
            var opt1 = p_jqxGrid.defineInstance.apply(this, arguments);
            var opt2 = {
                $scope: null,
                scope_id: null,
                //row_detail: null,
                multi_rowdetails: false,
                ext: {
                    rowdetails_url: function (index, parentElement, gridElement, datarecord) { },
                    grps: [],
                    add_url: '',
                    update_url: '',
                    OnAddingRow: function (addRow) { },
                    OnAddRow: function (data, jqXHR, settings) { },
                },
            };
            $.extend(true, this, opt2);
            return $.extend(true, opt1, opt2);
        },
        createInstance: function (opts) {
            var $grid = this;
            if ($grid.$scope == null) { $grid.$scope = { $applyAsync: function () { } }; };
            var $scope = $grid.$scope;
            if ($grid.scope_id) { $scope[$grid.scope_id] = $grid; }

            try { $grid.element.$grid = $grid; }
            catch (ex) { }
            var settings = opts[0];
            $grid.ext = new grid_extension();
            var ext_grps = {};
            if ($grid.ext.grps == null)
                $grid.ext.grps = {};
            if (!$grid.ext.grps.hasOwnProperty(''))
                $grid.ext.grps[''] = {};
            for (var n in $grid.ext.grps) {
                ext_grps[n] = $.extend(true, {
                    OnUpdate: function (rowid, rowdata, data, jqXHR, settings) { },
                    OnUpdateComplete: function (rowid, row, data) { },
                    OnLoading: function (rowid, row, jqXHR, settings) { },
                    OnLoadComplete: function (rowid, row, data) { },
                }, $grid.ext.grps[n]);
            }
            var _rows = {};
            var _ng_row = null;
            var _tab_index = 0;
            function _set_ng_row(ng_row, tab_index) {
                if (ng_row == null)
                    _ng_row = row_ext_null;
                else
                    _ng_row = ng_row;
                _tab_index = tab_index;
                for (var n in ext_grps) {
                    try {
                        var $row_grp = _ng_row[n];
                        if ($row_grp == null)
                            continue;
                        if ($.isFunction($row_grp.loadData))
                            $row_grp.loadData();
                    }
                    catch (e) { }
                }
                $scope.$applyAsync();
            }
            Object.defineProperty($grid, 'ng_row', {
                get: function () { return _ng_row; },
                set: function (value) { _set_ng_row(value, _tab_index); }
            });
            Object.defineProperty($grid, 'tab_index', {
                get: function () { return _tab_index; },
                set: function (value) { _set_ng_row(_ng_row, value); }
            });
            $grid.host.on('rowselect', function (event) {
                _set_ng_row($grid.ext.getRow(event.args.row), _tab_index);
            });
            function row_ext_new(uid) {
                var $row_ext = this;
                Object.defineProperty(this, 'isNew', { get: _true });
                function _get_row() {
                    return $grid.getrowdatabyuid(uid);
                }
                function _set_row(value) {
                    $grid.updaterow(_get_rowid(), value);
                }
                function _get_rowid(row) {
                    if (row == null)
                        row = _get_row();
                    return $grid.getrowid(row.boundindex);
                }
                function row_ext_grp(grp_name) {
                    var $row_ext = this;
                    var _error = null;
                    var _updating = false;
                    function _begin_update() {
                        if (_updating === true)
                            return true;
                        _error = null;
                        _updating = true;
                        return false;
                    }
                    function _end_update() {
                        if (_updating === true) {
                            _updating = false;
                            $scope.$applyAsync();
                        }
                    }
                    function _commit_update(data) {
                        var row = _get_row();
                        var rowid = _get_rowid(row);
                        if (data === null)
                            $grid.deleterow(rowid);
                        else
                            $grid.updaterow(rowid, data);
                        delete _rows[row.uid];
                        $grid.selectrow(row.boundindex);
                        //$row_ext.$row_detail.delete();
                        _end_update();
                    }
                    $row_ext.beginEdit = function () { };
                    $row_ext.endEdit = function (success, url) {
                        if (_begin_update())
                            return;
                        if (success !== true) {
                            _commit_update(null);
                            return;
                        }
                        try {
                            $.ajax({
                                type: 'post',
                                contentType: 'application/json',
                                dataType: 'json',
                                cache: false,
                                async: true,
                                beforeSend: function (jqXHR, settings) {
                                    settings.url = url;
                                    var row_data = _get_row();
                                    if ($grid.ext.OnAddRow.call($grid, row_data, jqXHR, settings) === false) {
                                        _end_update();
                                        return false;
                                    }
                                    if ((settings.url == null) || (settings.url == '')) {
                                        settings.url = $grid.ext.update_url;
                                        if ((settings.url == null) || (settings.url == ''))
                                            throw "add_url is null";
                                    }
                                    settings.data = JSON.stringify(row_data);
                                },
                                success: function (data, textStatus, jqXHR) {
                                    if (data == null) {
                                        _error = {
                                            Message: 'No result'
                                        };
                                    }
                                    else
                                        _commit_update(data);
                                },
                                error: function (jqXHR, textStatus, errorThrown) {
                                    var data = jqXHR.responseJSON;
                                    _error = data;
                                    if (data != null) {
                                        if (data.hasOwnProperty('Data') && data.hasOwnProperty('Status')) {
                                        }
                                    }
                                },
                                complete: function () {
                                    _end_update();
                                }
                            });
                        }
                        catch (ex) {
                            _end_update();
                            throw ex;
                        }
                    };
                    Object.defineProperty($row_ext, 'isEditing', { get: _true });
                    Object.defineProperty($row_ext, 'isLoading', { get: _false });
                    $row_ext.errorMsg = function (field_name) {
                        if (_error == null)
                            return null;
                        if (field_name == null)
                            return _error;
                        else
                            return _error[field_name];
                    };
                    $row_ext.hasError = function (field_name) {
                        if (_error == null)
                            return false;
                        if (field_name == null)
                            return true;
                        else
                            return _error[field_name] != null;
                    };
                    Object.defineProperty(this, 'data', { get: _get_row, set: _set_row });
                    //Object.defineProperty($row_ext.data, '', { get: _get_row });
                }
                $row_ext[''] = new row_ext_grp(n);
                for (var n in ext_grps) {
                    if (n !== '')
                        $row_ext[n] = $row_ext[''];
                }
            }
            function row_ext(uid) {
                var $row_ext = this;
                $row_ext.$grid = $grid;
                var _edit = null;

                function _get_row() {
                    return $grid.getrowdatabyuid(uid);
                }
                function _set_row(value) {
                    $grid.updaterow(_get_rowid(), value);
                }
                function _get_rowid(row) {
                    if (row == null)
                        row = _get_row();
                    if (row == null)
                        return null;
                    return $grid.getrowid(row.boundindex);
                }

                function row_ext_grp(grp_name, grp_opts) {
                    var $row_grp = this;
                    //var _data = null;
                    var _edit = null;
                    var _error = null;
                    var _updating = false;
                    var _loading = false;
                    function _begin_update() {
                        if (_updating === true)
                            return true;
                        _error = null;
                        _updating = true;
                        return false;
                    }
                    function _end_update() {
                        if (_updating === true) {
                            _updating = false;
                            $scope.$applyAsync();
                        }
                    }
                    function _commit_update(data) {
                        if (data == null)
                            $row_grp.data = _edit;
                        else
                            $row_grp.data = data;
                        _edit = null;
                        _end_update();
                    }
                    function invoke_complete(m, data, textStatus, jqXHR) {
                        if ($.isFunction(grp_opts[m])) {
                            var tmp = grp_opts[m].call($row_grp, _get_rowid(), $row_ext[''].data, data);
                            if (tmp != null)
                                return tmp;
                        }
                        return data;
                    }
                    $row_grp.beginEdit = function () {
                        if (_edit == null)
                            _edit = $.extend(true, {}, $row_grp.data);
                    };
                    $row_grp.endEdit = function (success) {
                        if (_edit == null)
                            return;
                        if (_begin_update())
                            return;
                        if (success !== true) {
                            _commit_update(null);
                            return;
                        }
                        try {
                            $.ajax({
                                type: 'post',
                                contentType: 'application/json',
                                dataType: 'json',
                                cache: false,
                                async: true,
                                beforeSend: function (jqXHR, settings) {
                                    settings.url = null;
                                    var rowid = _get_rowid();
                                    if (grp_opts.OnUpdate.call($row_grp, rowid, $row_ext[''].data, $row_grp.data, jqXHR, settings) === false) {
                                        _end_update();
                                        return false;
                                    }
                                    if ((settings.url == null) || (settings.url == '')) {
                                        settings.url = $grid.ext.update_url;
                                        if ((settings.url == null) || (settings.url == ''))
                                            throw "update_url is null";
                                    }
                                    settings.data = JSON.stringify($row_grp.data);
                                },
                                success: function (data, textStatus, jqXHR) {
                                    if (data == null) {
                                        _error = { Message: 'No result' };
                                    }
                                    else {
                                        _commit_update(invoke_complete('OnUpdateComplete', data, textStatus, jqXHR));
                                    }
                                },
                                error: function (jqXHR, textStatus, errorThrown) {
                                    var data = jqXHR.responseJSON;
                                    _error = data;
                                    if (data != null) {
                                        if (data.hasOwnProperty('Data') && data.hasOwnProperty('Status')) {
                                        }
                                    }
                                },
                                complete: function () {
                                    _end_update();
                                }
                            });
                        }
                        catch (ex) {
                            _end_update();
                            throw ex;
                        }
                    };
                    Object.defineProperty($row_grp, 'isEditing', {
                        get: function () { return _edit != null; }
                    });
                    Object.defineProperty($row_grp, 'isLoading', {
                        get: function () { return _loading; }
                    });
                    $row_grp.errorMsg = function (field_name) {
                        if (_error == null)
                            return null;
                        if (field_name == null)
                            return _error;
                        else
                            return _error[field_name];
                    };
                    $row_grp.hasError = function (field_name) {
                        if (_error == null)
                            return false;
                        if (field_name == null)
                            return true;
                        else
                            return _error[field_name] != null;
                    };
                    if (grp_name == '') {
                        Object.defineProperty($row_grp, 'data', { get: _get_row, set: _set_row });
                    }
                    else
                        $row_grp.data = null;
                    //var p_data = grp_name === '' ? { get: _get_row, set: _set_row } : {
                    //    get: function () { return _data; },
                    //    set: function (value) { _data = value; }
                    //}
                    function _begin_load() {
                        if (_loading)
                            return true;
                        _loading = true;
                        return false;
                    }
                    function _end_load() {
                        _loading = false;
                        $scope.$applyAsync();
                    }
                    $row_grp.loadData = function () {
                        if ((grp_opts.TabIndex !== null) && (grp_opts.TabIndex !== _tab_index))
                            return;
                        console.log('loaddata', { uid: uid, grp: grp_name });
                        if (_begin_load())
                            return;
                        try {
                            $.ajax({
                                type: 'post',
                                contentType: 'application/json',
                                dataType: 'json',
                                cache: false,
                                async: true,
                                beforeSend: function (jqXHR, settings) {
                                    settings.url = null;
                                    var tmp = grp_opts.OnLoading.call($row_grp, _get_rowid(), $row_ext[''].data, jqXHR, settings);
                                    if ((settings.url != null) && (settings.url != '')) {
                                        settings.data = JSON.stringify(settings.data);
                                        return true;
                                    }
                                    _end_load();
                                    return false;
                                    //if ((tmp === false) || (settings.url == null) || (settings.url == '')) {
                                    //}
                                },
                                success: function (data, textStatus, jqXHR) {
                                    if (data == null) {
                                        _error = { Message: 'No result' };
                                    }
                                    else {
                                        $row_grp.data = invoke_complete('OnLoadComplete', data, textStatus, jqXHR);
                                    }
                                },
                                error: function (jqXHR, textStatus, errorThrown) {
                                    var data = jqXHR.responseJSON;
                                    _error = data;
                                    if (data != null) {
                                        if (data.hasOwnProperty('Data') && data.hasOwnProperty('Status')) {
                                        }
                                    }
                                },
                                complete: function () {
                                    _end_load();
                                }
                            });
                        }
                        catch (ex) {
                            _end_load();
                            throw ex;
                        }
                    };
                }
                for (var n in ext_grps) {
                    $row_ext[n] = new row_ext_grp(n, ext_grps[n]);
                }
            }
            var row_ext_null = new function () {
                var $row_ext = this;
                Object.defineProperty(this, 'isNull', { get: _true });
                function row_ext_grp(grp_name) { }
                ;
                $row_ext[''] = new row_ext_grp(n);
                for (var n in ext_grps) {
                    if (n !== '')
                        $row_ext[n] = $row_ext[''];
                }
                //$row_ext[''] = new row_ext_grp('');
                //ext_grps.forEach(function (grp_name) {
                //    if (!$row_ext.hasOwnProperty(grp_name))
                //        $row_ext[grp_name] = new row_ext_grp(grp_name);
                //});
            };
            function grid_extension() {
                var $grid_ext = this;
                $.extend(true, $grid_ext, $grid.ext);
                $grid_ext.reset = function () {
                    _rows = {};
                    _set_ng_row($grid_ext.getSelRow(), _tab_index);
                };
                $grid_ext.getRow = function (row) {
                    if (row == null)
                        return null;
                    if (_rows.hasOwnProperty(row.uid))
                        return _rows[row.uid];
                    else
                        return _rows[row.uid] = new row_ext(row.uid);
                };
                $grid_ext.getSelRow = function () {
                    var sel_index = $grid.getselectedrowindex();
                    var sel_row = $grid.getrowdata(sel_index);
                    return $grid_ext.getRow(sel_row);
                };
                $grid_ext.addRow = function (id, _newdata) {
                    function _addRow(id, _newdata) {
                        if (id == null) {
                            $grid.addrow(null, _newdata, 'first');
                            var row = $grid.getrowdata(0);
                            if (row != null) {
                                _rows[row.uid] = new row_ext_new(row.uid);
                                $grid.selectrow(0);
                            }
                        }
                        else {
                            $grid.addrow(id, _newdata, 'first');
                            var row = $grid.getrowdatabyid(id);
                            if (row != null) {
                                _rows[row.uid] = new row_ext_new(row.uid);
                                $grid.selectrow($grid.getrowboundindexbyid(id));
                            }
                        }
                    }
                    if ((id == null) && (_newdata == null) && $.isFunction($grid_ext.OnAddingRow)) {
                        $grid_ext.OnAddingRow.call($grid, _addRow);
                    }
                    else
                        _addRow(id, _newdata);
                };

                _replace($grid.source, 'loadComplete', function () {
                    $grid_ext.reset();
                });
                //_replace($grid.source, 'ready', function () {
                //    $grid_ext.reset();
                //});
            }

            var rows = null;
            Object.defineProperty($grid, 'rows', {
                get: function () { return rows; },
                set: function (value) {
                    if (value.getrow == null)
                        rows = new $.jqx.row_collection(value);
                    else
                        rows = value;
                }
            });
            //var _columns = $grid.columns;
            //Object.defineProperty($grid, 'columns', {
            //    get: function () {
            //        return _columns;
            //    },
            //    set: function (value) {
            //        _columns = value;
            //    },
            //});
            var _details = $grid.details;
            Object.defineProperty($grid, 'details', {
                get: function () {
                    return _details;
                },
                set: function (value) {
                    if (_details != value) {
                        _details = value;
                        _popups = {};
                    }
                },
            });
            var _popups = {};
            Object.defineProperty($grid, 'popups', {
                get: function () { return _popups; },
            });

            return p_jqxGrid.createInstance.apply(this, arguments);
        },
        //_defaultcellsrenderer: function (cellvalue, column) { return p_jqxGrid._defaultcellsrenderer.apply(this, arguments); },
        _setrowdetailsvisibility: function (rowBoundIndex, rowdetailstemplate, visible) {
            if (this.multi_rowdetails !== true) {
                if (this.details) {
                    for (var i = 0; i < this.details.length; i++) {
                        if (i == rowBoundIndex) continue;
                        if (this.details[i] == null) continue;
                        p_jqxGrid._setrowdetailsvisibility.call(this, i, this.details[i], true);
                    }
                }
            }
            return p_jqxGrid._setrowdetailsvisibility.apply(this, arguments);
        },
        getrowdatabyuid: function (uid) {
            var rows = this.getboundrows();
            if (rows == null) return null;
            for (var i in rows) {
                if (rows[i].uid == uid)
                    return rows[i];
            }
        },
        getSelectedRowData: function () {
            return this.getrowdata(this.getselectedrowindex());
        },
        deleteSelectedRow: function () {
            var rowindex = this.getselectedrowindex();
            if (rowindex < 0)
                return;
            var rowid = this.getrowid(rowindex);
            if (rowid == null)
                return;
            this.deleterow(rowid);
        },
        set_url: function (url) {
            var source = this.host.jqxGrid('source');
            source._source.url = url;
            this.host.jqxGrid('source', source);
        },
        updaterow: function (rowIds, data) {
            var $grid = this;
            var datafields = $grid.source._source.datafields;
            datafields.forEach(function (datafield) {
                if (data.hasOwnProperty(datafield.name)) {
                    data[datafield.name] = $grid.source.getvaluebytype(data[datafield.name], datafield);
                }
            });
            arguments[1] = data;
            return p_jqxGrid.updaterow.apply(this, arguments);
            //return p_jqxGrid.updaterow(rowIds, data);
        },
        _refreshView: null,
        refreshView: function (ms) {
            var $grid = this;
            var s = parseInt(ms);
            if (isNaN(s))
                s = 100;
            function _clearTimeout() {
                if ($grid.hasOwnProperty('_refreshView')) {
                    clearTimeout($grid._refreshView);
                    delete $grid._refreshView;
                }
            }
            function cb() {
                _clearTimeout();
                var p = $grid.scrollposition();
                $grid.refreshdata();
                $grid.scrolloffset(p.top, p.left);
            }
            _clearTimeout();
            if (s == 0)
                cb();
            else
                $grid._refreshView = setTimeout(cb, s);
        },

        getFilterValue: function (datafield) {
            var ret;
            this.getfilterinformation().forEach(function (f1) {
                if (f1.datafield == datafield) {
                    try { ret = f1.filter.getfilters()[0].value; }
                    catch (err) { }
                }
            });
            return ret;
        },

        getPopupGroup: function (group) {
            if (group == null) group = '';
            if (this.popups.hasOwnProperty(group))
                return this.popups[group];
            else
                return this.popups[group] = {};
        },
        _renderbuttoncell: function ($grid, element, column, row, text) {
            if (column._opts) {
                if (column._opts.renderbuttoncell) {
                    return column._opts.renderbuttoncell.call($grid, $grid, element, column, row, text)
                }
            }
            return p_jqxGrid._renderbuttoncell.apply(this, arguments);
        },

        BeginRowEdit: function (boundrow) {
            if (boundrow._edit) return;
            boundrow._edit = {
                data: $.extend(true, {}, boundrow.bounddata)
            };
            this._renderrows(this.virtualsizeinfo)
        },
        EndRowEdit: function (boundrow) {
        },
    });



    $.jqx.columns = {
        row_number: function (opts) {
            return $.extend(true, {
                menu: false, pinned: true, exportable: false, editable: false, sortable: false, filterable: false, text: '', cellsalign: 'center', columntype: 'number',
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    return this.owner._defaultcellsrenderer(value + 1, this);
                },
                //rendered: function (columnHeaderElement) {
                //    console.log(this, arguments);
                //},
                filtertype: 'custom',
                //createfilterpanel: function (datafield, filterPanel) {
                //    console.log('createfilterpanel', arguments);
                //},
                createfilterwidget: function (column, columnElement, widget) {
                    var $grid = column.owner;
                    //var $e = $('<i class="fa fa-fw fa-remove"></i>');
                    var $e = $('<a class="btn btn-block"><i class="fa fa-fw fa-remove"></i></a>');
                    $e.appendTo(columnElement);
                    $e.css({
                        'margin-top': "4px",
                        'margin-bottom': "4px",
                        padding: 0,
                    });
                    $e.on('click', function () {
                        $grid.clearfilters();
                    });
                    //console.log('createfilterwidget', arguments);
                },
            }, opts);
        },
        Amount: function (opts) {
            return $.extend(true, {
                datafield: { type: 'number' },
                cellsformat: 'c',
                width: 100,
                //columntype: 'number',
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    var tmp = this.owner.gridlocalization.currencysymbol;
                    this.owner.gridlocalization.currencysymbol = '';
                    var ret = this.owner._defaultcellsrenderer(value, this);
                    this.owner.gridlocalization.currencysymbol = tmp;
                    return ret;
                },
            }, opts);
        },
        DateTime: function (opts) {
            return $.extend(true, {
                width: 130,
                columntype: 'datetimeinput',
                cellsformat: 'yyyy/MM/dd HH:mm:ss',
                filtertype: 'range',
            }, opts, {
                datafield: { type: 'date' },
            });
        },
        ActionEdit: function (opts) {
            return $.extend(true, {
                width: 80,
                //buttonclick: function (row) {
                //    var grid = this.owner;
                //    //this.owner.beginupdate();
                //    if (grid.editcell) {
                //        grid.endrowedit(this.owner.editcell.row);
                //        //grid.refreshView();
                //        setTimeout(function(){
                //            grid.beginrowedit(row);
                //        }, 10);
                //    }
                //    else                    
                //        grid.beginrowedit(row);
                //    //this.owner.endupdate();
                //    console.log(arguments);
                //}
            }, opts, {
                //columntype: 'button',
                //editable: false,
                filterable: false,
            });
        },
        ActionPopup: function (opts) {
            return opts = $.extend(true, {
                OnRowUpdate: function (rowId, rowdata) { },
                renderbuttoncell: function ($grid, element, column, row, text) {
                    var $e = $(element);
                    $e.empty();
                    var text = opts.ButtonText;
                    if ($.isFunction(text))
                        text = text.call($grid, element, column, row);
                    var $btn = $('<div class="btn btn-sm btn-block" style="height: 100%;"><i class="fa fa-info-circle"> ' + text + '</button>');
                    $btn.appendTo(element);
                    var rowIndex = $grid.getboundindex(row);
                    $btn.click(function () {
                        //console.log(column, row);
                        $(opts.ModalSelector + ' .modal-body > iframe').hide();
                        var $grid = column.owner;
                        var rowId = $grid.getrowid(rowIndex);
                        var _grp = $grid.popups[opts.name];
                        if (_grp == null)
                            _grp = $grid.popups[opts.name] = {};
                        var $iframe = _grp[rowId];
                        if ($iframe == null) {
                            if ($.isFunction(opts.get_url)) {
                                var rowdata = $grid.getrowdatabyid(rowId);
                                if (rowdata == null) return;
                                var url = opts.get_url(rowdata);
                                if (url == null) return;
                                $iframe = _grp[rowId] = $('<iframe style="width: 100%; height: 100%; border: none;"></iframe>');
                                $(opts.ModalSelector + ' .modal-body .loading').removeClass('hidden');
                                $iframe.appendTo($(opts.ModalSelector + ' .modal-body')).hide();
                                var iframe = $iframe[0];
                                iframe.onload = function () {
                                    $iframe.show();
                                    $(opts.ModalSelector + ' .modal-body .loading').addClass('hidden');
                                    //this.contentWindow.$.iframe_auto_height();
                                };
                                iframe.OnRowUpdate = function (grp_name, rowdata) {
                                    var tmp = opts.OnRowUpdate(rowId, rowdata);
                                    if (tmp === false) return;
                                    $grid.updaterow(rowId, rowdata);
                                };
                                iframe.get_modal = function () {
                                    return $(opts.ModalSelector);
                                }
                                setTimeout(function () {
                                    $iframe.prop('src', url);
                                }, 500);
                            }
                        }
                        else $iframe.show();
                        $(opts.ModalSelector).modal();
                    });
                },
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                    this._opts = opts;
                    return defaulthtml;
                    if (opts.ButtonText) value = opts.ButtonText;
                    return '<div class="btn btn-sm btn-block" style="height: 100%;"><i class="fa fa-info-circle"> ' + value + '</div>';
                    return this.owner._defaultcellsrenderer(value, this);
                    //return defaulthtml;
                },
            }, opts, {
                columntype: 'button',
                filterable: false,
            });
        },
        ActionButton: function (opts) {
            return opts = $.extend(true, {
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                    this._opts = opts;
                    return defaulthtml;
                },
            }, opts, {
                columntype: 'button',
                filterable: false,
            })
        },
        ActionDropdown: function (opts) {
            return opts = $.extend(true, {
                //renderbuttoncell: function ($grid, element, column, row, text) {
                //    console.log(arguments);
                //},
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                    this._opts = opts;
                    return defaulthtml;
                },
            }, opts, {
                columntype: 'button',
                filterable: false,
            })
        },
    };
})($);
// Array.push - add to last
// Array.pop  - remove from last
// Array.unshift - add to first
// Array.shift   - remove from first
