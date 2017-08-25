/// <reference path="~/jqwidgets/jqx-all.js" />
;
//$.jqx.theme = 'bootstrap';
//var xx = function (widget) {
//}

//function GridData($grid, $scope, newid_min, newid_max, _newdata, _newdata_grp) {
//    var $this = this;

//    function _datastore(init_row) {
//        var _this = this;
//        this._edit = {};
//        this._data = {'':init_row};
//        this.isnew = false;

//        this.data = function (grp) {
//            if (grp == null) grp = '';
//            return _this._data[grp];
//        }

//        this.editing = function (grp) {
//            if (grp == null) grp = '';
//            return _this._edit.hasOwnProperty(grp);
//        }

//        this.editrow = function (grps) {
//            if (grps == null) grps = '';
//            if (!$.isArray(grps))
//                grps = [grps];
//            grps.forEach(function (grp) {
//                if (_this._edit.hasOwnProperty(grp)) return; // is editing

//                if (_this._data.hasOwnProperty(grp)) {
//                    _this._edit[grp] = $.extend(true, {}, _this._data[grp]);
//                }
//                else {
//                    _this._data[grp] = {};
//                    _this._edit[grp] = null;
//                }
//            });
//        }

//        this.cancelEdit = function (grps) {
//            if (grps == null) grps = '';
//            if (!$.isArray(grps))
//                grps = [grps];

//            grps.forEach(function (grp) {
//                if (_this._edit.hasOwnProperty(grp)) {
//                    if (grp == '') {
//                        var row = _this._data[''];
//                        var rowid = $grid.getrowid(row.boundindex);
//                        $grid.updaterow(rowid, _this._edit['']);
//                        _this._data[''] = $grid.getrowdatabyid(rowid);
//                    }
//                    else {
//                        if (_this._edit[grp] == null)
//                            delete _this._data[grp];
//                        else
//                            _this._data[grp] = _this._edit[grp];
//                    }
//                    delete _this._edit[grp];
//                }
//            });
//        }
//    }

//    $this._store = {};
//    $this._active = null;

//    this.set_active = function (row) {
//        //var data = this.getdata(row);
//        //if (!data.hasOwnProperty(''))
//        //    data[''] = row;
//        $this._active = $this.getstore(row);
//        console.log($this._active);
//    }

//    this.getstore = function (row) {
//        if ($this._store.hasOwnProperty(row.uid))
//            return $this._store[row.uid];
//        else
//            return $this._store[row.uid] = new _datastore(row);
//    }

//    this.active = function (grp) {
//        if ($this._active == null) return false;
//        return $this._active.data(grp);
//    }

//    this.editing = function (grp) {
//        if ($this._active == null) return false;
//        return $this._active.editing(grp);
//    }

//    this.isnew = function () {
//        if ($this._active == null) return false;
//        return $this._active.isnew;
//    }

//    this.editrow = function (grps) {
//        if ($this._active == null) return false;
//        return $this._active.editrow(grps);
//    }

//    this.cancelEdit = function (grps) {
//        if ($this._active == null) return;
//        if ($this._active.isnew === true) {
//            var boundindex = $this._active._data[''].boundindex;
//            var rowid = $grid.getrowid(boundindex)
//            $grid.deleterow(rowid);
//            delete $this._store[$this._active._data[''].uid];
//            $this._active = null;
//            $grid.selectrow(boundindex);;
//        }
//        else {
//            $this._active.cancelEdit(grps);
//        }
//    }

//    this.addrow = function (grps) {
//        for (var id = newid_min; id < newid_max; id++) {
//            var row = $grid.getrowdatabyid(id);
//            if (row == null) {
//                $grid.addrow(id, $.extend(true, {}, _newdata), 'first');
//                row = $grid.getrowdata(0);
//                if (row != null) {
//                    var store = $this.getstore(row);
//                    store.isnew = true;
//                    $grid.selectrow(row.boundindex);;
//                    return;
//                }
//            }
//        }
//    };
//}

//var _jqxGrid = {
//    _defaultcellsrenderer: jqx._jqxGrid.prototype._defaultcellsrenderer,
//};
//jqx._jqxGrid.prototype._defaultcellsrenderer = function (value, column) {
//    if (column.renderValue)
//        value = column.renderValue(value, column);
//    return _jqxGrid._defaultcellsrenderer.call(this, value, column);
//}

//$.fn.jqxInit = function (widget_name, options) {
//    this[widget_name](options);
//    var instance = this[widget_name]('getInstance');
//    instance[widget_name] = this[widget_name];
//    return instance;
//}

//$.jqx.get_tmp = function (selector, clone) {
//    var n = $('.jqx-templates ' + selector);
//    if (clone === true)
//        n = n.clone();
//    return n.removeClass(selector);
//}

//$.fn.jqxGrid2 = function (opts) {
//    var _this = this;
//    opts = $.extend(true, {
//        width: '100%', height: '100%',
//        altrows: true,
//        //filterrowheight: 24,
//        columnsheight: 24,
//        columnsresize: true,
//        rowsheight: 24,
//        server_sorting: true,
//        server_filtering: true,
//        server_paging: true,
//        showfiltermenuitems: false,
//        pagerbuttonscount: 5,
//        pagesize: 50,
//        pagesizeoptions: [10, 20, 50, 100],
//        enabletooltips: true,
//        source: {
//            contenttype: 'application/json',
//            dataType: 'json',
//            type: 'post',
//            //processData: function (data) {
//            //    console.log('processData', arguments);
//            //},
//            //formatData: function (data) {
//            //    console.log('formatData', arguments);
//            //},
//            //beforesend: function (xhr) {
//            //    console.log('beforesend', arguments);
//            //},
//            //beforeprocessing: function (data, state, xhr) {
//            //    console.log('beforeprocessing', arguments);
//            //    return data;
//            //},
//            //downloadComplete: function (records) {
//            //    console.log('downloadComplete', arguments);
//            //    return records;
//            //},
//            //loadComplete: function (data) {
//            //    console.log('loadComplete', arguments);
//            //},
//            //loadError: function (xhr, status, error) {
//            //    console.log('loadError', arguments);
//            //},
//            //loadServerData: function (serverdata, source, callback) {
//            //    console.log('loadServerData', arguments);
//            //},
//            //addrow: function (rowid, rowdata, position, commit) {
//            //    console.log('addrow', arguments);
//            //    commit(true);
//            //},
//            //deleterow: function (rowid, commit) {
//            //    console.log('deleterow', arguments);
//            //    commit(true);
//            //},
//            //updaterow: function (rowid, newdata, commit) {
//            //    console.log('updaterow', arguments);
//            //    commit(true);
//            //},
//            //loadServerData: function (serverdata, source, callback) {
//            //    console.log('loadServerData', arguments);
//            //},
//        }

//    }, opts);
//    var grid;

//    if (opts.server_sorting == true) {
//        opts.source.sort = function (field, is_asc) {
//            //console.log('sort', arguments);
//            grid.updatebounddata('sort');
//        };
//    }

//    if (opts.server_filtering == true) {
//        opts.source.filter = function (filters, data, count) {
//            //console.log('filter', arguments);
//            grid.updatebounddata('sort');
//        }
//    }

//    if ((opts.pageable == true) && (opts.server_paging == true)) {
//        opts = $.extend(true, opts, {
//            //source: {
//            //    beforeprocessing: function (data) {
//            //        return data;
//            //        // source.totalrecords = data[0].TotalRows;
//            //        //console.log('beforeprocessing', arguments);
//            //        //return {
//            //        //    records: data,
//            //        //    totalrecords: 10000,
//            //        //};
//            //    },
//            //},
//            virtualmode: true,
//            rendergridrows: function (params) {
//                return params.data;
//            },
//        });
//    }

//    if (opts.toolbar) {
//        var _toolbar = $(template_class + opts.toolbar);
//        _toolbar.removeClass(opts.toolbar.substring(1));
//        //console.log(_toolbar.outerHeight());
//        opts = $.extend(true, {
//            showtoolbar: true,
//            toolbarheight: 30,
//            rendertoolbar: function (toolbar) {
//                toolbar.addClass('btn-toolbar');
//                _toolbar.css('margin-top', '-1px');
//                _toolbar.appendTo(toolbar);
//            }
//        }, opts);
//    }

//    var n1 = $.jqx._jqxGrid.prototype.defineInstance();
//    for (var n2 in opts) if (!n1.hasOwnProperty(n2)) delete opts[n2];
//    this.jqxGrid(opts);
//    grid = this.jqxGrid('getInstance');
//    grid.$scope = $scope;
//    grid.filter_toggle = function () {
//        _this.jqxGrid('showfilterrow', !grid.showfilterrow);
//    }
//    grid.filter_class = function () {
//        if (grid.showfilterrow)
//            return 'active';
//    }
//    return grid;
//}

//function _parse_value(value) {
//    if (value == '')
//        return value;
//    if (!isNaN(value))
//        return parseInt(value);
//    if (value == 'true')
//        return true;
//    if (value == 'false')
//        return false;
//    return value;
//}

//function _import(settings, attrs, template) {
//    for (var n1 in template) {
//        if (attrs.hasOwnProperty(n1)) {
//            var v = attrs[n1];
//            if ($.isArray(template[n1]) && (typeof v == 'string'))
//                settings[n1] = JSON.parse(v);
//            else
//                settings[n1] = _parse_value(v);
//        }
//    }
//    return settings;
//}

//function _map_value(src, items) {
//    var ret = {};
//    for (var n1 in items) {
//        var n2 = items[n1];
//        if (src.hasOwnProperty(n2))
//            ret[n1] = _parse_value(src[n2]);
//    }
//}

//function _jqx_init2(scope, elem, attrs, name, cb) {
//    var template = $.jqx['_' + name].prototype.defineInstance();
//    var settings = {};
//    if (attrs.hasOwnProperty('settings')) {
//        var _settings = eval('scope.' + attrs.settings);
//        if (_settings.hasOwnProperty('events')) {
//            if (_settings.events.hasOwnProperty('*')) {
//                for (var _n2 = 0; _n2 < template.events.length; _n2++) {
//                    var n2_1 = template.events[_n2];
//                    var n2_2 = n2_1.toLowerCase();
//                    elem.on(n2_1, _settings.events['*']);
//                    if (n2_1 === n2_2) continue;
//                    elem.on(n2_2, _settings.events['*']);
//                }
//            }
//            for (var n1 in _settings.events) {
//                if (n1 === "*") { }
//                else { elem.on(n1, _settings.events[n1]); }
//            }
//        }
//        for (var n1 in _settings) {
//            if (n1 === 'events') { }
//            else if (template.hasOwnProperty(n1)) { settings[n1] = _settings[n1]; }
//        }
//        //for (var n1 in template)
//        //    if (_settings.hasOwnProperty(n1))
//        //        settings[n1] = _settings[n1];
//        //var _on_all = null;
//        //if (_settings.hasOwnProperty('on-*'))
//        //    _on_all = _settings['on-*'];
//        //for (var _n2 = 0; _n2 < template.events.length; _n2++) {
//        //    var n2 = template.events[_n2];
//        //    for (; ;) {
//        //        var n3 = 'on-' + n2;
//        //        if (_settings.hasOwnProperty(n3))
//        //            elem.on(n2, _settings[n3]);
//        //        if (_on_all != null)
//        //            elem.on(n2, _on_all);
//        //        var n2_tmp = n2.toLowerCase();
//        //        if (n2 === n2_tmp) break;
//        //        n2 = n2_tmp;
//        //    }
//        //}
//    }
//    settings = _import(settings, attrs, template)
//    if ($.isFunction(cb)) {
//        var tmp = cb(settings, template);
//        if (tmp != null)
//            settings = tmp;
//    }
//    elem[name](settings);
//    var instance = elem[name]('getInstance');
//    if (attrs.hasOwnProperty('instance'))
//        eval('scope.' + attrs.instance + ' = instance');
//    if ($.isFunction(instance.refresh))
//        $(document).ready(function () {
//            instance.refresh();
//        });
//}

(function ($) {

    var template_class = '.jqx-templates ';

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

    $.jqx.columns = {
        row_number: function (opt) {
            return $.extend(true, {
                dataField: '__row_number', menu: false, pinned: true, exportable: false, editable: false, sortable: false, filterable: false, text: '', cellsalign: 'center', columntype: 'number',
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



            }, opt);
        },
        row_command: function (opts) {
            return $.extend(true, {
                dataField: '__row_command', width: 120, menu: false, pinned: false, exportable: false, editable: false, sortable: false, filterable: false, text: '', cellsalign: 'left', columntype: 'custom',
                //renderer: function () {
                //    console.log('renderer', arguments);
                //},
                //rendered: function (columnHeaderElement) {
                //    console.log('rendered', arguments);
                //},
                //cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                //    console.log('cellsrenderer', arguments);
                //    return '<div></div>'
                //    return this.owner._defaultcellsrenderer(value + 1, this);
                //},
                createwidget: function (row, column, value, cellElement) {
                    var n = $(template_class + '.row-command').clone().removeClass('row-command').appendTo(cellElement);
                    var n1 = $('[data-op]', n);
                    n1.addClass('hidden');
                    //.click(function () {
                    //    column.owner.selectrow(row.boundindex);
                    //    column.owner.beginrowedit(row.boundindex);
                    //})
                    //.each(function () {
                    //    console.log(this);
                    //    });
                    $('[data-op=edit]', n).removeClass('hidden');
                    $('[data-op=del]', n).removeClass('hidden');
                    console.log('createwidget', arguments);
                },
                initwidget: function (row, column, value, cellElement) {
                    //console.log('initwidget', arguments);
                },
                //createeditor: function (row, cellvalue, editor, celltext, cellwidth, cellheight) {
                //    console.log('createeditor', arguments);
                //},
                //geteditorvalue: function (row, cellvalue, editor) {
                //    console.log('geteditorvalue', arguments);
                //}
            }, opts);
        },
    };

    $.fn.import_data = function (template) {
        var _datas = this.data();
        var result = {};
        if (_datas == null) return result;
        for (var n1 in template) {
            if (n1 == 'source') {
                if (typeof _datas.source == 'string') {
                    result.source = JSON.parse(_datas.source.replaceAll('\'', '"'));
                }
                continue;
            }
            if (_datas.hasOwnProperty(n1)) {
                result[n1] = _datas[n1];
            }
            else {
                var n2 = n1.toLowerCase();
                if (_datas.hasOwnProperty(n2)) {
                    result[n1] = _datas[n2];
                }
            }
        }
        return result;
    }

    $.fn.jqxWidget = function (widget_name, cb, cb_instance) {
        var elem = this;
        var template = $.jqx['_' + widget_name].prototype.defineInstance();
        var settings = elem.import_data(template);
        var settings2a = elem.data('settings');
        if (settings2a != null) {
            var _settings = eval(settings2a);
            if (_settings.hasOwnProperty('events')) {
                if (_settings.events.hasOwnProperty('*')) {
                    var _events = template.events;
                    if (_events == null)
                        _events = template._events;
                    if ((_events != null) && $.isArray(_events)) {
                        for (var _n2 = 0; _n2 < _events.length; _n2++) {
                            var n2_1 = _events[_n2];
                            var n2_2 = n2_1.toLowerCase();
                            elem.on(n2_1, _settings.events['*']);
                            if (n2_1 === n2_2) continue;
                            elem.on(n2_2, _settings.events['*']);
                        }
                    }
                }
                for (var n1 in _settings.events) {
                    if (n1 === "*") { }
                    else { elem.on(n1, _settings.events[n1]); }
                }
            }
            for (var n1 in _settings) {
                if (n1 === 'events') { }
                else if (template.hasOwnProperty(n1)) { settings[n1] = _settings[n1]; }
            }
        }
        if ($.isFunction(cb)) {
            var tmp = cb(settings, template);
            if (tmp != null)
                settings = tmp;
        }
        elem[widget_name](settings);
        var instance = elem[widget_name]('getInstance');
        var _instance = elem.data('instance');
        if (_instance != null)
            eval(_instance + ' = instance');
        if ($.isFunction(cb_instance))
            cb_instance(instance);
        if ($.isFunction(instance.refresh)) {
            $(document).ready(function () {
                instance.refresh();
            });
        }
    };

    [
        'jqxValidator      ', 'jqxButton         ', 'jqxLinkButton     ', 'jqxRepeatButton   ', 'jqxToggleButton   ',
        'jqxDropDownButton ', 'jqxColorPicker    ', 'jqxScrollBar      ', 'jqxPanel          ',
        'jqxTooltip        ', 'jqxCalendar       ', 'jqxDateTimeInput  ', 'jqxDraw           ', 'jqxChart          ',
        'jqxLinearGauge    ', 'jqxGauge          ', 'jqxCheckBox       ', 'jqxButtonGroup    ', 'jqxListBox        ',
        'jqxTree           ', 'jqxDragDrop       ', 'jqxComboBox       ', 'jqxWindow         ',
        'jqxDocking        ', 'jqxDockPanel      ', 'jqxMaskedInput    ', 'jqxMenu           ', 'jqxExpander       ',
        'jqxNavigationBar  ', 'jqxNumberInput    ', 'jqxProgressBar    ', 'jqxRadioButton    ', 'jqxRating         ',
        'jqxSlider         ', 'jqxTabs           ', 'jqxListMenu       ', 'jqxScrollView     ', 'jqxTouch          ',
        'jqxInput          ', 'jqxTreeMap        ', 'jqxPasswordInput  ', 'jqxRangeSelector  ', 'jqxDataTable      ',
        'jqxTreeGrid       ', 'jqxBulletChart    ', 'jqxEditor         ', 'jqxNotification   ', 'jqxToolBar        ',
        'jqxComplexInput   ', 'jqxFormattedInput ', 'jqxRibbon         ', 'jqxNavBar         ', 'jqxFileUpload     ',
        'jqxLoader         ', 'jqxTextArea       ', 'jqxPopover        ', 'jqxLayout         ', 'jqxDockingLayout  ',
        'jqxResponsivePanel', 'jqxTagCloud       ', 'jqxScheduler      ', 'jqxKnob           ', 'jqxSortable       ',
        'jqxKanban         ', 'jqxBarGauge       '].forEach(function (n) {
            var n = n.trim();
            angular.module('ui.directives').directive('uiJ' + n.substring(1), [function () {
                return {
                    restrict: 'A', link: function (scope, elem, attrs) {
                        elem.jqxWidget(n);
                    }
                }
            }]);
        });

    angular.module('ui.directives').directive('uiJqxSplitter', [function () {
        var _panel = {
            size: 1,
            min: 1,
            collapsible: true,
            collapsed: false
        }

        return {
            restrict: 'A',
            link: function (scope, elem, attrs) {
                elem.jqxWidget('jqxSplitter', function (settings) {
                    settings.panels = new Array();
                    $('>[ui-jqx-splitter-panel]', elem).each(function () {
                        settings.panels.push($(this).import_data(_panel));
                    });
                });
            }
        }
    }]);

    angular.module('ui.directives').directive('uiJqxGrid', [function () {
        var tmp_source = {
            beforeprocessing: null,
            beforesend: null,
            loaderror: null,
            localdata: null,
            data: null,
            datatype: "array",
            datafields: [],
            url: "",
            root: "",
            record: "",
            id: "",
            totalrecords: 0,
            recordstartindex: 0,
            recordendindex: 0,
            loadallrecords: true,
            sortcolumn: null,
            sortdirection: null,
            sort: null,
            filter: null,
            sortcomparer: null
        };

        //var tmp_column = new function () {
        //    this.datafield = null;
        //    this.displayfield = null;
        //    this.text = "";
        //    this.createfilterpanel = null;
        //    this.sortable = true;
        //    this.hideable = true;
        //    this.editable = true;
        //    this.hidden = false;
        //    this.groupable = true;
        //    this.renderer = null;
        //    this.cellsrenderer = null;
        //    this.checkchange = null;
        //    this.threestatecheckbox = false;
        //    this.buttonclick = null;
        //    this.columntype = null;
        //    this.cellsformat = "";
        //    this.align = "left";
        //    this.cellsalign = "left";
        //    this.width = "auto";
        //    this.minwidth = 25;
        //    this.maxwidth = "auto";
        //    this.pinned = false;
        //    this.visibleindex = -1;
        //    this.filterable = true;
        //    this.filter = null;
        //    this.filteritems = [];
        //    this.resizable = true;
        //    this.initeditor = null;
        //    this.createeditor = null;
        //    this.createwidget = null;
        //    this.initwidget = null;
        //    this.destroywidget = null;
        //    this.destroyeditor = null;
        //    this.geteditorvalue = null;
        //    this.validation = null;
        //    this.classname = "";
        //    this.cellclassname = "";
        //    this.cellendedit = null;
        //    this.cellbeginedit = null;
        //    this.cellvaluechanging = null;
        //    this.aggregates = null;
        //    this.aggregatesrenderer = null;
        //    this.menu = true;
        //    this.createfilterwidget = null;
        //    this.filtertype = "default";
        //    this.filtercondition = null;
        //    this.rendered = null;
        //    this.exportable = true;
        //    this.exporting = false;
        //    this.draggable = true;
        //    this.nullable = true;
        //    this.clipboard = true;
        //    this.enabletooltips = true;
        //    this.columngroup = null;
        //    this.filterdelay = 800;
        //    this.reseteverpresentrowwidgetvalue = null;
        //    this.geteverpresentrowwidgetvalue = null;
        //    this.createeverpresentrowwidget = null;
        //    this.initeverpresentrowwidget = null;
        //    this.validateeverpresentrowwidgetvalue = null;
        //    this.destroyeverpresentrowwidget = null;
        //    return this
        //}

        //function set_datafield(source, datafield) {
        //    for (var i = 0; i < source.datafields.length; i++) {
        //        if (source.datafields[i].name == datafield.name) {
        //            source.datafields[i] = $.extend(true, source.datafields[i], datafield);
        //            return datafield.name;
        //        }
        //    }
        //    source.datafields.push(datafield);
        //    return datafield.name;
        //}

        return {
            restrict: 'A',
            //controller: ["$scope", "$attrs", "$element", "$transclude", function ($scope, $attrs, $element, $transclude) { }],
            link: function (scope, elem, attrs) {
                elem.jqxWidget('jqxGrid', function (settings, template) {
                    var opts_toolbar = {};
                    $('>[ui-jqx-grid-toolbar]:first', elem).each(function () {
                        var _toolbar = $(this);
                        opts_toolbar = $.extend(true, {
                            showtoolbar: true,
                            toolbarheight: 30,
                            rendertoolbar: function (toolbar) {
                                toolbar.addClass('btn-toolbar');
                                _toolbar.css('margin-top', '-1px');
                                _toolbar.appendTo(toolbar);
                            }
                        }, _toolbar.import_data(template));
                    });
                    if (settings.source == null) settings.source = {};
                    settings.source = $.extend(true, {
                        contenttype: 'application/json',
                        dataType: 'json',
                        type: 'post',
                        datafields: [],
                        //processData: function (data) {
                        //    console.log('processData', arguments);
                        //},
                        //formatData: function (data) {
                        //    console.log('formatData', arguments);
                        //},
                        //beforesend: function (xhr) {
                        //    console.log('beforesend', arguments);
                        //},
                        //beforeprocessing: function (data, state, xhr) {
                        //    console.log('beforeprocessing', arguments);
                        //    return data;
                        //},
                        //downloadComplete: function (records) {
                        //    console.log('downloadComplete', arguments);
                        //    return records;
                        //},
                        //loadComplete: function (data) {
                        //    console.log('loadComplete', arguments);
                        //},
                        //loadError: function (xhr, status, error) {
                        //    console.log('loadError', arguments);
                        //},
                        //loadServerData: function (serverdata, source, callback) {
                        //    console.log('loadServerData', arguments);
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
                    }, settings.source, $('>[ui-jqx-grid-source]', elem).import_data(tmp_source));
                    //for (var i = 0; i < settings.columns.length; i++) {
                    //    if (settings.columns[i].hasOwnProperty('name'))
                    //        settings.columns[i].name = settings.columns[i].name.trim();
                    //}
                    //$('>[ui-jqx-grid-column]', elem).each(function () {
                    //    var $this = $(this);
                    //    var name = $this.data('name');
                    //    if (name == null) return;
                    //    name = name.trim();
                    //    for (var i = 0; i < settings.columns.length; i++) {
                    //        var column1 = settings.columns[i];
                    //        if (column1.name == name) {
                    //            var column2 = $this.import_data(tmp_column);
                    //            var _datafield = $this.data('_datafield');
                    //            if (_datafield != null) {
                    //                try { column2.dataField = JSON.parse(_datafield.replaceAll('\'', '"')); }
                    //                catch (err) { }
                    //            }
                    //            settings.columns[i] = $.extend(true, column2, column1);
                    //            break;
                    //        }
                    //    }
                    //});
                    var data = elem.data();
                    var opts_sort = {};
                    if (data.server_sorting === true) {
                        opts_sort = {
                            source: {
                                sort: function (field, is_asc) {
                                    //console.log('sort', arguments);
                                    elem.jqxGrid('updatebounddata', 'sort');
                                }
                            }
                        };
                    }
                    var opts_filter = {};
                    if (data.server_filtering === true) {
                        opts_filter = {
                            source: {
                                filter: function (filters, data, count) {
                                    //console.log('filter', arguments);
                                    elem.jqxGrid('updatebounddata', 'sort');
                                }
                            }
                        };
                    }
                    var opts_paging = {};
                    if ((settings.pageable === true) && (data.server_paging === true)) {
                        opts_paging = {
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
                            virtualmode: true,
                            rendergridrows: function (params) {
                                return params.data;
                            }
                        }
                    }

                    settings = $.extend(true, {
                        width: '100%', height: "100%",
                        altrows: true,
                        //filterrowheight: 24,
                        columnsheight: 24,
                        columnsresize: true,
                        rowsheight: 24,
                        everpresentrowheight: 24,
                        //server_sorting: true,
                        //server_filtering: true,
                        //server_paging = true;
                        showfiltermenuitems: false,
                        pagerbuttonscount: 5,
                        pagesize: 50,
                        pagesizeoptions: [10, 20, 50, 100],
                        enabletooltips: true,
                    }, opts_toolbar, settings, opts_sort, opts_filter, opts_paging);
                    return settings;
                });
            }
        }
    }]);

    ['jqxDropDownList'].forEach(function (n) {
        var n1 = n.trim();
        var n2 = 'uiJ' + n1.substring(1);
        angular.module('ui.directives').directive(n2, [function () {
            return {
                restrict: 'A',
                require: '?ngModel', // get a hold of NgModelController
                link: function (scope, elem, attrs, ngModel) {
                    elem.jqxWidget(n1, function (settings, template) {
                    }, function (instance) {
                        if (ngModel != null) {
                            elem.on('change', function (event) {
                                ngModel.$setViewValue(instance.val());
                            });
                            scope.$watch(function () {
                                return ngModel.$modelValue;
                            }, function (newValue, oldValue) {
                                instance.selectItem(instance.getItemByValue({ value: newValue }));
                            });
                        }
                    });
                }
            }
        }]);
    });

    ['jqxSwitchButton'].forEach(function (n) {
        var n1 = n.trim();
        var n2 = 'uiJ' + n1.substring(1);
        angular.module('ui.directives').directive(n2, [function () {
            return {
                restrict: 'A',
                require: '?ngModel', // get a hold of NgModelController
                link: function (scope, elem, attrs, ngModel) {
                    elem.jqxWidget(n1, function (settings, template) {
                    }, function (instance) {
                        if (ngModel != null) {
                            elem.on('change', function (event) {
                                ngModel.$setViewValue(instance.val());
                            });
                            scope.$watch(function () {
                                return ngModel.$modelValue;
                            }, function (newValue, oldValue) {
                                elem[n1]({ checked: newValue });
                            });
                        }
                    });
                }
            }
        }]);
    });

    function _row_extra_data(uid) {
        this._uid = uid;
        this._edit = {};
        this._data = {};
        this.isnew = false;
    }

    function _ext_data($grid) {
        var $ext = this;
        this._selected_row = null;

        this.reset = function () {
            $ext._rows = {};
        }
        this.reset();

        $grid.host.on('rowselect', function (event) {
            $ext._selected_row = getrow(event.args.row);
        });
        $grid.host.on('bindingcomplete', function (event) {
            $ext.reset();
        });

        function getrow(row) {
            if (row == null) return null;
            if ($ext._rows.hasOwnProperty(row.uid))
                return $ext._rows[row.uid];
            else
                return $ext._rows[row.uid] = new _row_extra_data(row.uid);
        }

        this.getselrow = function (grp) {
            if ($ext._selected_row == null) return null;
            if ((grp == null) || (grp == '')) return $grid.getrowdatabyuid($ext._selected_row._uid);
            else return $ext._selected_row._data[grp];
        }

        this.isNew = function () {
            if ($ext._selected_row == null) return false;
            return $ext._selected_row.isnew;
        }

        this.addrow = function (id_min, id_max, _newdata, _newdata_grp) {
            for (var id = id_min; id < id_max; id++) {
                var row = $grid.getrowdatabyid(id);
                if (row == null) {
                    $grid.addrow(id, $.extend(true, {}, _newdata), 'first');
                    row = $grid.getrowdatabyid(id);
                    if (row != null) {
                        var rowx = getrow(row);
                        rowx.isnew = true;
                        $grid.selectrow($grid.getrowboundindexbyid(id));
                        return;
                    }
                }
            }
        }

        this.isEditing = function (grp) {
            if ($ext._selected_row == null) return false;
            if (grp == null) grp = '';
            return $ext._selected_row._edit.hasOwnProperty(grp);
        }

        this.beginEdit = function (grps) {
            if ($ext._selected_row == null) return;
            var row = $grid.getrowdatabyuid($ext._selected_row._uid);
            var rowid = $grid.getrowid(row.boundindex);

            _grps(grps, function (grp) {
                if ($ext._selected_row._edit.hasOwnProperty(grp)) return; // is editing
                if (grp == '') {
                    $ext._selected_row._edit[''] = $.extend(true, {}, row);
                }
                else {
                    if ($ext._selected_row._data.hasOwnProperty(grp)) {
                        $ext._selected_row._edit[grp] = $.extend(true, {}, $ext._selected_row._data[grp]);
                    }
                    else {
                        $ext._selected_row._data[grp] = {};
                        $ext._selected_row._edit[grp] = null;
                    }
                }
            });
        }

        this.endEdit = function (grps, success) {
            if ($ext._selected_row == null) return;
            var row = $grid.getrowdatabyuid($ext._selected_row._uid);
            var rowid = $grid.getrowid(row.boundindex);

            if ($ext._selected_row.isnew === true) {
                $grid.deleterow(rowid);
                delete $ext._rows[row.uid];
                $ext._selected_row = null;
                $grid.selectrow(row.boundindex);
            }
            else {
                var updates = new Array();
                _grps(grps, function (grp) {
                    var data = null, edit = null;
                    if ($ext._selected_row._data.hasOwnProperty(grp))
                    { data = $ext._selected_row._data[grp]; delete $ext._selected_row._data[grp] }
                    if ($ext._selected_row._edit.hasOwnProperty(grp))
                    { edit = $ext._selected_row._edit[grp]; delete $ext._selected_row._edit[grp] }
                    if (success === true) {
                        if (grp == '') {
                            updates.push({ rowid: rowid, grp: grp, data: row });
                        } //$grid.updaterow(rowid, data);
                        else {
                            $ext._selected_row._data[grp] = data;
                            updates.push({ rowid: rowid, grp: grp, data: data });
                        }
                    }
                    else if (edit != null) {
                        if (grp == '') $grid.updaterow(rowid, edit);
                        else $ext._selected_row._data[grp] = edit;
                    }
                });
                updates.forEach(function (data) {
                    console.log('update', data);
                });
            }
        }
    }

    function _grps(grps, cb) {
        if (grps == null)
            grps = '';
        if (!$.isArray(grps))
            grps = [grps];
        grps.forEach(cb);
    }

    var tmp_column = new function () {
        this.datafield = null;
        this.displayfield = null;
        this.text = "";
        this.createfilterpanel = null;
        this.sortable = true;
        this.hideable = true;
        this.editable = true;
        this.hidden = false;
        this.groupable = true;
        this.renderer = null;
        this.cellsrenderer = null;
        this.checkchange = null;
        this.threestatecheckbox = false;
        this.buttonclick = null;
        this.columntype = null;
        this.cellsformat = "";
        this.align = "left";
        this.cellsalign = "left";
        this.width = "auto";
        this.minwidth = 25;
        this.maxwidth = "auto";
        this.pinned = false;
        this.visibleindex = -1;
        this.filterable = true;
        this.filter = null;
        this.filteritems = [];
        this.resizable = true;
        this.initeditor = null;
        this.createeditor = null;
        this.createwidget = null;
        this.initwidget = null;
        this.destroywidget = null;
        this.destroyeditor = null;
        this.geteditorvalue = null;
        this.validation = null;
        this.classname = "";
        this.cellclassname = "";
        this.cellendedit = null;
        this.cellbeginedit = null;
        this.cellvaluechanging = null;
        this.aggregates = null;
        this.aggregatesrenderer = null;
        this.menu = true;
        this.createfilterwidget = null;
        this.filtertype = "default";
        this.filtercondition = null;
        this.rendered = null;
        this.exportable = true;
        this.exporting = false;
        this.draggable = true;
        this.nullable = true;
        this.clipboard = true;
        this.enabletooltips = true;
        this.columngroup = null;
        this.filterdelay = 800;
        this.reseteverpresentrowwidgetvalue = null;
        this.geteverpresentrowwidgetvalue = null;
        this.createeverpresentrowwidget = null;
        this.initeverpresentrowwidget = null;
        this.validateeverpresentrowwidgetvalue = null;
        this.destroyeverpresentrowwidget = null;
        return this
    }

    var _defineInstance = $.jqx._jqxGrid.prototype.defineInstance;
    var _createInstance = $.jqx._jqxGrid.prototype.createInstance;
    $.extend($.jqx._jqxGrid.prototype, {
        //defineInstance: function () {
        //    var opts = _defineInstance.apply(this, arguments);
        //    opts = $.extend(true, {
        //    }, opts);
        //    return opts;
        //},
        createInstance: function (opts) {
            var $grid = this;
            this.ext = new _ext_data(this);
            var settings = opts[0];

            function set_datafield(source, datafield) {
                for (var i = 0; i < source.datafields.length; i++) {
                    if (source.datafields[i].name == datafield.name) {
                        source.datafields[i] = $.extend(true, source.datafields[i], datafield);
                        return datafield.name;
                    }
                }
                source.datafields.push(datafield);
                return datafield.name;
            }

            settings.columns.forEach(function (column) {
                if (column.hasOwnProperty('name'))
                    column.name = column.name.trim();
            });

            $('>[ui-jqx-grid-column]', $grid.host).each(function () {
                var $this = $(this);
                var name = $this.data('name');
                if (name == null) return;
                name = name.trim();
                for (var i = 0; i < settings.columns.length; i++) {
                    var column1 = settings.columns[i];
                    if (column1.name == name) {
                        var column2 = $this.import_data(tmp_column);
                        var _datafield = $this.data('_datafield');
                        if (_datafield != null) {
                            try { column2.dataField = JSON.parse(_datafield.replaceAll('\'', '"')); }
                            catch (err) { }
                        }
                        settings.columns[i] = $.extend(true, column2, column1);
                        break;
                    }
                }
            });

            for (var i = 0; i < this.columns.length; i++) {
                var column = this.columns[i];
                delete column.name;
                if (typeof column.dataField == 'object') {
                    column.dataField = set_datafield(this.source, column.dataField);
                }
            }

            return _createInstance.apply(this, arguments);
        },
        //extAddRow: function (id_min, id_max, _newdata, _newdata_grp) {
        //},
        //extBeginEdit: function (grps) {
        //},
        //extEndEdit: function (grps, success) {
        //},
        getrowdatabyuid: function (uid) {
            var rows = this.getboundrows();
            for (var i = 0; i < rows.length; i++)
                if (rows[i].uid == uid)
                    return rows[i];
            return null;
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

        _refreshGrid: null,
        refreshGrid: function (ms) {
            var $grid = this;
            var s = parseInt(ms);
            if (isNaN(s)) s = 100;

            function _clearTimeout() {
                if ($grid.hasOwnProperty('_refreshGrid')) {
                    clearTimeout($grid._refreshGrid);
                    delete $grid._refreshGrid;
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
            else $grid._refreshGrid = setTimeout(cb, s);
        },
    });
})($);

