/// <reference path="~/jqwidgets/jqx-all.js" />
;
(function ($) {
    if (!('startsWith' in String.prototype)) {
        String.prototype.startsWith = function (prefix) {
            return (this.substr(0, prefix.length) === prefix);
        }
    }

    $.isNullOrEmpty = function (str) {
        if (str == null) return true;
        if (str == '') return true;
        return false;
    }

    if (!('find' in Array.prototype)) {
        Array.prototype.find = function (callback, thisArg) {
            if (thisArg == null)
                thisArg = this;
            for (var i = 0, n = this.length; i < n; i++)
                if (callback.call(thisArg, this[i]) === true)
                    return this[i];
        }
    }

    $.init_tick = function (handler, timeout) {
        var busy = false;
        if (timeout == undefined)
            timeout = 100;
        var handle = window.setInterval(function () {
            if (busy === true) return;
            try {
                busy = true;
                if (handler() !== true) return;
                window.clearInterval(handle);
            }
            finally {
                busy = false;
            }
        }, timeout);
    };

    $.n = function (n) { return n; }

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
                }
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
                }
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
                            list_args.Filters = {
                            };
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
                //loadServerData: function (serverdata, source, callback) {
                //    console.log('loadServerData', arguments);
                //},
                //beforesend: function (xhr, settings) {
                //    console.log('beforesend', arguments);
                //},
                //beforeprocessing: function (data, state, xhr) {
                //    //console.log('beforeprocessing', arguments);
                //    return data;
                //},
                //downloadComplete: function (records) {
                //    console.log('downloadComplete', arguments);
                //    return records;
                //},
                //loadComplete: function (data) {
                //    //console.log('loadComplete', arguments);
                //},
                //loadError: function (xhr, status, error) {
                //    console.log('loadError', arguments);
                //},
                //addrow: function (rowid, rowdata, position, commit) {
                //    console.log('addrow', arguments);
                //    commit(true);
                //},
                //deleterow: function (rowid, commit) {
                //    console.log('deleterow', arguments);
                //    commit(true);
                //},
                //updaterow: function (rowid, newdata, commit) {
                //    console.log('updaterow', arguments);
                //    commit(true);
                //},
                //loadServerData: function (serverdata, source, callback) {
                //    console.log('loadServerData', arguments);
                //},
            },
            ext: {}
        }, settings);

        function translate(column) {
            if ($.isArray(column.translate.source) && (column.cellsrenderer == null)) {
                var $this = this;
                $.extend(true, $this, { id: '_value', text: '_text' }, column.translate);
                column.cellsrenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
                    var n = $this.source.find(function (n) { return n[$this.id] == value; })
                    if (n == null) return defaulthtml;
                    return this.owner._defaultcellsrenderer(n[$this.text], this);
                }
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
            }
            $this.set_item = function (datafield) {
                var n1 = $this.get_item(datafield.name)
                if (n1 === -1)
                    $this.values.push(datafield);
                else
                    $this.values[n1] = $.extend(true, $this.values[n1], datafield);
            }
        }
        var columns = new function () {
            var $this = this;
            $this.values = new Array();
            $this.get_item = function (name) {
                for (var i = 0; i < $this.values.length; i++)
                    if ($this.values[i].name === name)
                        return i;
                return -1;
            }
            $this.set_item = function (column, op) {
                var n = -1;
                if (column.name !== undefined) {
                    column.name = $.trim(column.name);
                    n = $this.get_item(column.name)
                    if (n !== -1)
                        column = $.extend(true, $this.values[n], column)
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
                else $this.values[n] = column;
            }
            $this.get_item2 = function (datafield) {
                for (var i = 0; i < $this.values.length; i++)
                    if ($this.values[i].datafield === datafield)
                        return $this.values[i];
            }
        }

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
            })
        });

        this.jqxGrid(settings);
        return $grid;
    }

    var _defineInstance = $.jqx._jqxGrid.prototype.defineInstance;
    var _createInstance = $.jqx._jqxGrid.prototype.createInstance;
    $.extend($.jqx._jqxGrid.prototype, {
        defineInstance: function () {
            var opt1 = _defineInstance.apply(this, arguments);
            var opt2 = {
                $scope: null,
                $scope_prop: '',
                ext: {
                    grps: [],
                    add_url: '',
                    update_url: '',
                    OnAddingRow: function (addRow) { },
                    OnAddRow: function (data, jqXHR, settings) { },
                    OnUpdateRow: function (rowid, grp, grpdata, jqXHR, settings) { },
                },
            };
            $.extend(true, this, opt2);
            return $.extend(true, opt1, opt2);
        },
        createInstance: function (opts) {
            var $grid = this;
            try { $grid.element.$grid = $grid; } catch (ex) { }
            var settings = opts[0];
            $grid.ext = new grid_extension();
            if (!$.isArray($grid.ext.grps))
                $grid.ext.grps = new Array();
            var ext_grps = new Array();
            $grid.ext.grps.forEach(function (n) { ext_grps.push(n); });
            var _rows = {};
            var _active_row = null

            function row_ext_new(uid) {
                var $row_ext = this;
                Object.defineProperty(this, 'isNew', {
                    get: function () { return true; }
                });

                function _get_row() { return $grid.getrowdatabyuid(uid); }
                function _set_row(value) { $grid.SetRowData(_get_rowid(), value); }
                function _get_rowid(row) {
                    if (row == null)
                        row = _get_row();
                    return $grid.getrowid(row.boundindex);
                }
                function init_grps(grp_name) {
                    if ($grps.hasOwnProperty(grp_name)) return;
                    return $grps[grp_name] = new function () {
                        var $this = this;
                        var _data = null;
                        $this.edit = null;
                        $this.error = null;
                        var _updating = false;
                        $this.loading = false;

                        $this.begin_update = function () {
                            if (_updating === true) return true;
                            $this.error = null;
                            _updating = true;
                            return false;
                        }
                        $this.end_update = function () {
                            if (_updating === true) {
                                _updating = false;
                                if ($grid.$scope != null)
                                    $grid.$scope.$applyAsync();
                            }
                        }

                        function loadData() {
                            if ($this.loading) return;
                            $this.loading = true;
                            if (isNew) return;
                            function finish() {
                                $this.loading = false;
                            }
                            try {
                                _invoke('OnLoadRow', [_get_rowid(), grp_name, function (data) {
                                    $this.data = data;
                                    finish();
                                }, finish]);
                            }
                            catch (e) {
                                finish();
                            }
                        }

                        var p_data = grp_name === '' ? { get: _get_row, set: _set_row } : {
                            get: function () {
                                if (_data == null)
                                    loadData();
                                return _data;
                            },
                            set: function (value) {
                                _data = value;
                            }
                        }
                        Object.defineProperty(this, 'data', p_data);
                        Object.defineProperty($row_ext.data, grp_name, { get: p_data.get });
                    };
                }
                function get_grp(grp_name, cb) {
                    if (grp_name == null) grp_name = '';
                    var $row_grp = $grps[grp_name];
                    if ($row_grp != null) return cb($row_grp);
                }

                var $grp_default = init_grps('');
                ext_grps.forEach(init_grps);

                $row_ext.beginEdit = function (grp_name) { };
                $row_ext.endEdit = function (grp_name, success) {
                    function commit(data) {
                        var row = _get_row();
                        var rowid = _get_rowid(row);
                        if (data === null)
                            $grid.deleterow(rowid);
                        else
                            $grid.SetRowData(rowid, data);
                        delete _rows[row.uid];
                        $grid.selectrow(row.boundindex);
                        $grp_default.end_update();
                    }

                    if ($grp_default.begin_update()) return;
                    if (success !== true) {
                        commit(null);
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
                                    $grp_default.end_update();
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
                                    $grp_default.error = {
                                        Message: 'No result'
                                    };
                                }
                                else
                                    commit(data);
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                var data = jqXHR.responseJSON;
                                $grp_default.error = data;
                                if (data != null) {
                                    if (data.hasOwnProperty('Data') && data.hasOwnProperty('Status')) {
                                    }
                                }
                            },
                            complete: function () {
                                $grp_default.end_update();
                            }
                        });
                    }
                    catch (ex) {
                        $grp_default.end_update();
                        throw ex;
                    }
                }
                $row_ext.isEditing = function (grp_name) {
                    return true;
                };
                $row_ext.isLoading = function (grp_name) {
                    return false;
                };
                $row_ext.commitEdit = function (grp_name) { return $row_ext.endEdit(grp_name, true); };
                $row_ext.cancelEdit = function (grp_name) { return $row_ext.endEdit(grp_name, false); };
                $row_ext.errorMsg = function (grp_name, field_name) {
                    return get_grp(grp_name, function ($row_grp) {
                        if ($row_grp.error == null) return null;
                        if (field_name == null)
                            return $row_grp.error
                        else
                            return $row_grp.error[field_name];
                    });
                }
                $row_ext.hasError = function (grp_name, field_name) {
                    return get_grp(grp_name, function ($row_grp) {
                        if ($row_grp.error == null) return false;
                        if (field_name == null)
                            return true;
                        else
                            return $row_grp.error[field_name] != null;
                    });
                }
            }

            function row_ext(uid) {
                var $row_ext = this;
                Object.defineProperty(this, 'isNew', {
                    get: function () { return false; }
                });
                var $grps = {};
                $row_ext.data = {};

                function _get_row() { return $grid.getrowdatabyuid(uid); }
                function _set_row(value) { $grid.SetRowData(_get_rowid(), value); }
                function _get_rowid(row) {
                    if (row == null)
                        row = _get_row();
                    return $grid.getrowid(row.boundindex);
                }
                function init_grps(grp_name) {
                    if ($grps.hasOwnProperty(grp_name)) return;
                    return $grps[grp_name] = new function () {
                        var $this = this;
                        var _data = null;
                        $this.edit = null;
                        $this.error = null;
                        var _updating = false;
                        $this.loading = false;

                        $this.begin_update = function () {
                            if (_updating === true) return true;
                            $this.error = null;
                            _updating = true;
                            return false;
                        }
                        $this.end_update = function () {
                            if (_updating === true) {
                                _updating = false;
                                if ($grid.$scope != null)
                                    $grid.$scope.$applyAsync();
                            }
                        }

                        function loadData() {
                            if ($this.loading) return;
                            $this.loading = true;
                            if (isNew) return;
                            function finish() {
                                $this.loading = false;
                            }
                            try {
                                _invoke('OnLoadRow', [_get_rowid(), grp_name, function (data) {
                                    $this.data = data;
                                    finish();
                                }, finish]);
                            }
                            catch (e) {
                                finish();
                            }
                        }

                        var p_data = grp_name === '' ? { get: _get_row, set: _set_row } : {
                            get: function () {
                                if (_data == null)
                                    loadData();
                                return _data;
                            },
                            set: function (value) {
                                _data = value;
                            }
                        }
                        Object.defineProperty(this, 'data', p_data);
                        Object.defineProperty($row_ext.data, grp_name, { get: p_data.get });
                    };
                }
                function get_grp(grp_name, cb) {
                    if (grp_name == null) grp_name = '';
                    var $row_grp = $grps[grp_name];
                    if ($row_grp != null) return cb($row_grp);
                }

                var $grp_default = init_grps('');
                ext_grps.forEach(init_grps);

                $row_ext.beginEdit = function (grp_name) {
                    return get_grp(grp_name, function ($row_grp) {
                        if ($row_grp.edit == null)
                            $row_grp.edit = $.extend(true, {}, $row_grp.data);
                    });
                };
                $row_ext.endEdit = function (grp_name, success) {
                    return get_grp(grp_name, function ($row_grp) {
                        function commit(data) {
                            if (data == null)
                                $row_grp.data = $row_grp.edit;
                            else
                                $row_grp.data = data;
                            $row_grp.edit = null;
                            $row_grp.end_update();
                        }
                        if ($row_grp.edit == null) return;
                        if ($grp_default.begin_update()) return;
                        if (success !== true) {
                            commit(null);
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
                                    if ($grid.ext.OnUpdateRow.call($grid, rowid, grp_name, $row_grp.data, jqXHR, settings) === false) {
                                        $row_grp.end_update();
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
                                        $grp_default.error = {
                                            Message: 'No result'
                                        };
                                    }
                                    else
                                        commit(data);
                                },
                                error: function (jqXHR, textStatus, errorThrown) {
                                    var data = jqXHR.responseJSON;
                                    $row_grp.error = data;
                                    if (data != null) {
                                        if (data.hasOwnProperty('Data') && data.hasOwnProperty('Status')) {
                                        }
                                    }
                                },
                                complete: function () {
                                    $row_grp.end_update();
                                }
                            });
                        }
                        catch (ex) {
                            $row_grp.end_update();
                            throw ex;
                        }
                    });
                };
                $row_ext.isEditing = function (grp_name) {
                    return get_grp(grp_name, function ($row_grp) {
                        return $row_grp.edit != null;
                    });
                };
                $row_ext.isLoading = function (grp_name) {
                    return get_grp(grp_name, function ($row_grp) {
                        return $row_grp.loading;
                    });
                };
                $row_ext.commitEdit = function (grp_name) { return $row_ext.endEdit(grp_name, true); };
                $row_ext.cancelEdit = function (grp_name) { return $row_ext.endEdit(grp_name, false); };

                $row_ext.errorMsg = function (grp_name, field_name) {
                    return get_grp(grp_name, function ($row_grp) {
                        if ($row_grp.error == null) return null;
                        if (field_name == null)
                            return $row_grp.error
                        else
                            return $row_grp.error[field_name];
                    });
                }

                $row_ext.hasError = function (grp_name, field_name) {
                    return get_grp(grp_name, function ($row_grp) {
                        if ($row_grp.error == null) return false;
                        if (field_name == null)
                            return true;
                        else
                            return $row_grp.error[field_name] != null;
                    });
                }
            }

            var row_ext_null = { isNull: true, '': {} };

            function grid_extension() {
                var $grid_ext = this;
                $.extend(true, $grid_ext, $grid.ext);

                function reset() {
                    _rows = {};
                    _active_row = null;
                };

                function getRow(row) {
                    if (row == null) return null;
                    if (_rows.hasOwnProperty(row.uid))
                        return _rows[row.uid];
                    else
                        return _rows[row.uid] = new row_ext(row.uid);
                };

                function getSelRow() {
                    var sel_index = $grid.getselectedrowindex();
                    var sel_row = $grid.getrowdata(sel_index);
                    return getRow(sel_row);
                };

                function addRow(id, _newdata) {
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
                    };
                    if ((id == null) && (_newdata == null) && $.isFunction($grid.ext.OnAddingRow)) {
                        $grid.ext.OnAddingRow.call($grid, _addRow);
                    }
                    else _addRow(id, _newdata);
                };

                //Object.defineProperty($grid_ext, 'sel', { get: getSelRow, });
                $grid_ext.reset = reset;
                $grid_ext.getRow = getRow;
                $grid_ext.getSelRow = getSelRow;
                $grid_ext.addRow = addRow;
            }

            if ($grid.$scope != null) {
                if (($grid.$scope != null) && ($grid.$scope_prop != null)) {
                    $grid.host.on('rowselect', function () {
                        _active_row = null;
                        $grid.$scope.$applyAsync();
                    })
                    Object.defineProperty($scope, $grid.$scope_prop, {
                        get: function () {
                            if (_active_row == null)
                                _active_row = $grid.ext.getSelRow();
                            if (_active_row == null) return row_ext_null;
                            return _active_row;
                        },
                    });
                }
            }

            var loadComplete = $grid.source.loadComplete;
            if (!$.isFunction(loadComplete)) loadComplete = $.noop;
            $grid.source.loadComplete = function () {
                $grid.ext.reset();
                return loadComplete.apply(this, arguments);
            }
            return _createInstance.apply(this, arguments);
        },

        getrowdatabyuid: function (uid) {
            return this.getboundrows().find(function(row) { return row.uid == uid });
        },
        getSelectedRowData: function () {
            return this.getrowdata(this.getselectedrowindex());
        },
        deleteSelectedRow: function () {
            var rowindex = this.getselectedrowindex();
            if (rowindex < 0) return;
            var rowid = this.getrowid(rowindex);
            if (rowid == null) return;
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
            })
            return $grid.updaterow(rowIds, data);
        },

        _refreshView: null,
        refreshView: function (ms) {
            var $grid = this;
            var s = parseInt(ms);
            if (isNaN(s)) s = 100;

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
            if (s == 0) cb();
            else $grid._refreshView = setTimeout(cb, s);
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

    var _columns_datafield

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



                //createEverPresentRowWidget: function (datafield, htmlElement, popup, addCallback) {
                //    console.log('createEverPresentRowWidget', arguments);
                //    var inputTag = $("<input style='border: none;'/>").appendTo(htmlElement);
                //    inputTag.jqxInput({ popupZIndex: 99999999, placeHolder: "Enter Name: ", source: getSourceAdapter("name"), displayMember: 'name', width: '100%', height: 30 });
                //    $(document).on('keydown.name', function (event) {
                //        if (event.keyCode == 13) {
                //            if (event.target === inputTag[0]) {
                //                addCallback();
                //            }
                //        }
                //    });
                //    return inputTag;
                //},
                //initEverPresentRowWidget: function (datafield, htmlElement) {
                //    console.log('initEverPresentRowWidget', arguments);
                //},
                //validateEverPresentRowWidgetValue: function (datafield, value, rowValues) {
                //    console.log('validateEverPresentRowWidgetValue', arguments);
                //    if (!value || (value && value.length < 5)) {
                //        return { message: "Entered value should be more than 5 characters", result: false };
                //    }
                //    return true;
                //},
                //getEverPresentRowWidgetValue: function (datafield, htmlElement, validate) {
                //    console.log('getEverPresentRowWidgetValue', arguments);
                //    var value = htmlElement.val();
                //    return value;
                //},
                //resetEverPresentRowWidgetValue: function (datafield, htmlElement) {
                //    console.log('resetEverPresentRowWidgetValue', arguments);
                //    htmlElement.val("");
                //}



            }, opts);
        },
        //datetime: function (opts) { return $.extend(true, _colutil.date1, _colutil.datafield(opts), _colutil.date2); },
        //CreateTime: function (opts) { return $.extend(true, _colutil.date1(), { datafield: { name: 'CreateTime' } }, _colutil.datafield(opts), _colutil.date2()); },
        //ModifyTime: function (opts) { return $.extend(true, _colutil.date1(), { datafield: { name: 'ModifyTime' } }, _colutil.datafield(opts), _colutil.date2()); },
        //CreateUser: function (opts) { return $.extend(true, _colutil.user1(), { datafield: { name: 'CreateUser' } }, _colutil.datafield(opts), _colutil.user2()); },
        //ModifyUser: function (opts) { return $.extend(true, _colutil.user1(), { datafield: { name: 'ModifyUser' } }, _colutil.datafield(opts), _colutil.user2()); },
    };
})($);

// Array.push - add to last
// Array.pop  - remove from last
// Array.unshift - add to first
// Array.shift   - remove from first


