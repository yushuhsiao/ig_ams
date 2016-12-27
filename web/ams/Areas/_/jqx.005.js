/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
;
(function ($) {
    if (!String.prototype.startsWith) {
        String.prototype.startsWith = function (str) {
            return this.indexOf(str) == 0;
        };
    }
    if (!('startsWith' in String.prototype)) {
        String.prototype.startsWith = function (prefix) {
            return (this.substr(0, prefix.length) === prefix);
        };
    }
    $.isNullOrEmpty = function (str) {
        if (str == null)
            return true;
        if (str == '')
            return true;
        return false;
    };
    if (!('find' in Array.prototype)) {
        Array.prototype.find = function (callback, thisArg) {
            if (thisArg == null)
                thisArg = this;
            for (var i = 0, n = this.length; i < n; i++)
                if (callback.call(thisArg, this[i]) === true)
                    return this[i];
        };
    }
    $.init_tick = function (handler, timeout) {
        var busy = false;
        if (timeout == undefined)
            timeout = 100;
        var handle = window.setInterval(function () {
            if (busy === true)
                return;
            try {
                busy = true;
                if (handler() !== true)
                    return;
                window.clearInterval(handle);
            }
            finally {
                busy = false;
            }
        }, timeout);
    };
    $.n = function (n) { return n; };
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
})($);
(function ($) {
    function _true() { return true; }
    function _false() { return false; }
    $.fn.jqxGridEx = function () {
        var $this = this;
        var $grid;
        var settings = $.extend(true, {
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
                        if (list_args.Filters == null)
                            list_args.Filters = {};
                        var values = new Array();
                        f1.filter.getfilters().forEach(function (f2) {
                            for (var i = 0; i < data.filterscount; i++) {
                                if ((data['filterdatafield' + i] === f1.datafield) &&
                                    (data['filtercondition' + i] === f2.condition) &&
                                    (data['filteroperator' + i] === f2.operator)) {
                                    values.push({
                                        Condition: f2.condition,
                                        Operator: f2.operator,
                                        Value: data['filtervalue' + i]
                                    });
                                    break;
                                }
                            }
                        });
                        list_args.Filters[f1.datafield] = values;
                    });
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
            ext: {}
        }, settings);
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
        var columns = new function () {
            var $this = this;
            $this.values = new Array();
            $this.get_item = function (name) {
                for (var i = 0; i < $this.values.length; i++)
                    if ($this.values[i].name === name)
                        return i;
                return -1;
            };
            $this.set_item = function (column, op) {
                var n = -1;
                if (column.name !== undefined) {
                    column.name = $.trim(column.name);
                    n = $this.get_item(column.name);
                    if (n !== -1)
                        column = $.extend(true, $this.values[n], column);
                }
                if (typeof column.datafield === 'object') {
                    datafields.set_item(column.datafield);
                    column.datafield = column.datafield.name;
                }
                if (typeof column.translate === 'object') {
                    column.translate = new translate(column);
                }
                if (n === -1) {
                    if (op == null)
                        op = 'push';
                    $this.values[op](column);
                }
                else
                    $this.values[n] = column;
            };
            $this.get_item2 = function (datafield) {
                for (var i = 0; i < $this.values.length; i++)
                    if ($this.values[i].datafield === datafield)
                        return $this.values[i];
            };
        };
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
                    if (column.hasOwnProperty('columntype')) {
                        if ($.jqx.columns.hasOwnProperty(column.columntype))
                            column = $.jqx.columns[column.columntype](column);
                    }
                    columns.set_item(column);
                });
            }
        }
        if (settings.row_numbers === true)
            columns.set_item($.jqx.columns.row_number(), 'unshift');
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
        settings.ext.columns = columns;
        settings.ext.datafields = datafields;
        settings.columns = columns.values;
        settings.source.datafields = datafields.values;
        this.each(function () {
            Object.defineProperty(this, '$grid', {
                get: function () { return $grid; },
                set: function (value) { $grid = value; }
            });
        });
        this.jqxGrid(settings);
        return $grid;
    };
    var _defineInstance = $.jqx._jqxGrid.prototype.defineInstance;
    var _createInstance = $.jqx._jqxGrid.prototype.createInstance;
    var __rendercell = $.jqx._jqxGrid.prototype._rendercell;
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
            return __rendercell.apply(this, arguments);
        },
        defineInstance: function () {
            var opt1 = _defineInstance.apply(this, arguments);
            var opt2 = {
                $scope: null,
                //row_detail: null,
                ext: {
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
            //var $row_detail_container = $('.ams-row-detail-container');
            //var $row_detail_mask = $('.loading_mask', $row_detail_container);
            //function row_detail($row_ext) {
            //    $row_ext.$row_detail = this;
            //    var $frame = $(null);
            //    var frame_ready = true;
            //    this.show = function (change) {
            //        $('.ams-row-detail').hide();
            //        if ($frame == null) return;
            //        if (frame_ready === true) {
            //            if (change)
            //                $frame.fadeIn(100);
            //            else
            //                $frame.show();
            //            $row_detail_mask.hide();
            //        }
            //        else {
            //            $row_detail_mask.show();
            //            $frame.hide();
            //        }
            //    }
            //    this.delete = function () {
            //        $row_ext.$row_detail = null;
            //    }
            //    if (($row_detail_container.length > 0) && !$row_ext.isNew) {
            //        frame_ready = false;
            //        $frame = $('<iframe class="ams-row-detail" src="' + window.location + '?' + ($row_ext.isNew === true ? 'addrow' : 'details') + '"></iframe');
            //        //$($frame[0].contentDocument).ready(function () {
            //        //    console.log('contentDocument ready');
            //        //});
            //        $frame.on('load', function () {
            //            $frame[0].contentWindow.ng_set($row_ext);
            //            frame_ready = true;
            //            $row_ext.$row_detail.show(true);
            //        });
            //        $frame.appendTo($row_detail_container).hide();
            //    }
            //}

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
                        //if (n == '') continue;
                        var $row_grp = _ng_row[n];
                        if ($row_grp == null)
                            continue;
                        if ($.isFunction($row_grp.loadData))
                            $row_grp.loadData();
                    }
                    catch (e) { }
                }
                //if (ng_row != null)
                //    ng_row.$row_detail.show();
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
                //new row_detail($row_ext, true);
                function _get_row() {
                    return $grid.getrowdatabyuid(uid);
                }
                function _set_row(value) {
                    $grid.SetRowData(_get_rowid(), value);
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
                            $grid.SetRowData(rowid, data);
                        delete _rows[row.uid];
                        $grid.selectrow(row.boundindex);
                        //$row_ext.$row_detail.delete();
                        _end_update();
                    }
                    $row_ext.beginEdit = function () { };
                    $row_ext.endEdit = function (success) {
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
                //$row_ext[''] = new row_ext_grp('');
                //ext_grps.forEach(function (grp_name) {
                //    if (!$row_ext.hasOwnProperty(grp_name))
                //        $row_ext[grp_name] = new row_ext_grp(grp_name);
                //});
            }
            function row_ext(uid) {
                var $row_ext = this;
                $row_ext.$grid = $grid;
                Object.defineProperty(this, 'isNew', { get: _false });
                //new row_detail(this);
                function _get_row() {
                    return $grid.getrowdatabyuid(uid);
                }
                function _set_row(value) {
                    $grid.SetRowData(_get_rowid(), value);
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
                //Object.defineProperty($grid_ext, 'sel', { get: getSelRow, });
                $grid_ext.reset = function () {
                    _rows = {};
                    $('iframe.ams-row-detail').remove();
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
                var loadComplete = $grid.source.loadComplete;
                if (!$.isFunction(loadComplete))
                    loadComplete = $.noop;
                $grid.source.loadComplete = function () {
                    var ret = loadComplete.apply(this, arguments);
                    $grid_ext.reset();
                    return ret;
                };
            }
            return _createInstance.apply(this, arguments);
        },
        getrowdatabyuid: function (uid) {
            return this.getboundrows().find(function (row) { return row.uid == uid; });
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
        SetRowData: function (rowIds, data) {
            var $grid = this;
            var datafields = $grid.source._source.datafields;
            datafields.forEach(function (datafield) {
                if (data.hasOwnProperty(datafield.name)) {
                    data[datafield.name] = $grid.source.getvaluebytype(data[datafield.name], datafield);
                }
            });
            return $grid.updaterow(rowIds, data);
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
    });
    //var _colutil = {
    //    date1: function () { return { datafield: { type: "date" }, cellsformat: "yyyy:MM:dd HH:mm:ss", filtertype: 'range', width: 130, } },
    //    date2: function () { return { columntype: 'datetimeinput', } },
    //    user1: function () { return { datafield: { type: "number" }, width: 100, } },
    //    user2: function () { return {} },
    //    datafield: function (opts) {
    //        if (typeof opts.datafield === 'string')
    //            opts.datafield = { name: opts.datafield };
    //        return opts;
    //    }
    //}
    //var _columns_datafield
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
                    //var $e = $('<i class="fa fa-fw fa-remove"></i>');
                    var $e = $('<a class="btn btn-block"><i class="fa fa-fw fa-remove"></i></a>');
                    $e.appendTo(columnElement);
                    $e.css({
                        'margin-top': "4px",
                        'margin-bottom': "4px",
                        padding: 0,
                    });
                    $e.on('click', function () {
                        column.owner.clearfilters();
                    });
                    //console.log('createfilterwidget', arguments);
                },
            }, opts);
        },
    };
})($);
// Array.push - add to last
// Array.pop  - remove from last
// Array.unshift - add to first
// Array.shift   - remove from first
