var _page = {
    ApiUrl: null,
    Logout_Url: null,
    relogin_token_name: null,

    dock_l: true,
    show_l: false,
    size_l: 300,
    left_1: function () {
        if (_page.dock_l)
            return _page.size_l;
        else
            return 0;
    },
    left_2: function () {
        if (_page.dock_l || _page.show_l)
            return 0;
        return -_page.size_l;
    },

    dock_r: false,
    show_r: false,
    size_r: 300,
    right_1: function () {
        if (_page.dock_r)
            return _page.size_r;
        else
            return 0;
    },
    right_2: function () {
        if (_page.dock_r || _page.show_r)
            return 0;
        return -_page.size_r;
    },

    tabIndex: 2,

    toggle_left: function (isSmall) {
        if (isSmall) {
            _page.show_l = !_page.show_l;
            _page.dock_l = false;
            _page.dock_r = _page.show_r = false;
        }
        else {
            _page.show_l = false;
            _page.dock_l = !_page.dock_l;
        }

        _page.update();
    },

    toggle_right: function (index, isSmall) {
        if (index == null)
            index = _page.tabIndex;

        if (isSmall) {
            _page.show_r = !_page.show_r;
            _page.dock_r = false;
            _page.dock_l = _page.show_l = false;
        }
        else {
            if (_page.tabIndex == index) {
                _page.show_r = !_page.show_r;
                _page.dock_r = false;
            }
            else {
                _page.show_r = true;
                _page.dock_r = false;
            }
            _page.tabIndex = index;
        }

        _page.update();
    },

    mask_click: function () {
        _page.show_r = false;
        _page.update();
    },

    update: function () {
        $('.p22').css({
            'padding-left': _page.left_1(),
            'padding-right': _page.right_1()
        });
        $('.p21').css({
            'left': _page.left_2(),
            'width': _page.size_l
        });
        $('.p23').css({
            'right': _page.right_2(),
            'width': _page.size_r
        });

        $('.p23-1').hide();
        $('.p23-2').hide();
        if (_page.tabIndex == 1) {
            $('.p23-1').show();
        }
        else if (_page.tabIndex == 2) {
            $('.p23-2').show();
        }

        if (_page.show_r) {
            $('.p22-mask').show();
        }
        else {
            $('.p22-mask').hide();
        }
    },

    logout: function () {
        var _n = this.relogin_token_name;
        util.api(this.ApiUrl, this.Logout_Url, {}, function (result) {
            if (result.IsSuccess) {
                setCookie(_n, '0')
                window.location.reload(true);
            }
        });
    }
};

_page.update();

//$('[data-url]').click(function () {
//    var url = $(this).data('url');
//    console.log(this, arguments);
//    console.log(url);
//    $('.p22-iframe').attr('src', url);
//});


//$('.p22-iframe').on('unload', function () {
//    console.log('iframe unload', arguments);
//});
//$('.p22-iframe').on('load', function () {
//    console.log('iframe load', arguments);
//});

//$(window).resize(function () {
//    if (document.documentElement.clientWidth < util.sizes.md) {
//        _page.dock_l = _page.show_l = _page.dock_r = _page.show_r = false;
//    }
//    else {
//        _page.dock_l = true;
//        _page.show_l = _page.dock_r = _page.show_r = false;
//    }
//    _page.update();
//    //console.log('resize');
//    //console.log(document.documentElement.clientWidth);
//});

//var menu_data = [
//    {
//        id: "dashboard", icon: "mdi mdi-view-dashboard", value: "Dashboards", data: [
//            { id: "dashboard1", value: "Dashboard 1" },
//            { id: "dashboard2", value: "Dashboard 2" }
//        ]
//    },
//    {
//        id: "layouts", icon: "mdi mdi-view-column", value: "Layouts", data: [
//            { id: "accordions", value: "Accordions" },
//            { id: "portlets", value: "Portlets" }
//        ]
//    },
//    {
//        id: "tables", icon: "mdi mdi-table", value: "Data Tables", data: [
//            { id: "tables1", value: "Datatable" },
//            { id: "tables2", value: "TreeTable" },
//            { id: "tables3", value: "Pivot" }
//        ]
//    },
//    {
//        id: "uis", icon: "mdi mdi-puzzle", value: "UI Components", data: [
//            { id: "dataview", value: "DataView" },
//            { id: "list", value: "List" },
//            { id: "menu", value: "Menu" },
//            { id: "tree", value: "Tree" }
//        ]
//    },
//    {
//        id: "tools", icon: "mdi mdi-calendar", value: "Tools", data: [
//            { id: "kanban", value: "Kanban Board" },
//            { id: "pivot", value: "Pivot Chart" },
//            { id: "scheduler", value: "Calendar" }
//        ]
//    },
//    {
//        id: "forms", icon: "mdi mdi-pencil", value: "Forms", data: [
//            { id: "buttons", value: "Buttons" },
//            { id: "selects", value: "Select boxes" },
//            { id: "inputs", value: "Inputs" }
//        ]
//    },
//    { id: "demo", icon: "mdi mdi-book", value: "Documentation" }
//];

webix.ready(function () {
    //$('.p22-loading').show();
    var iframe = webix.ui({
        view: 'iframe',
        container: $('.p22-iframe-container')[0],
        id: 'frame-content',
        borderless: true,
        css: 'p22-iframe',
        width: -1,
        height: -1,
        on: {
            onBeforeLoad: function () {
                $('.p22-loading').show();
            },
            onAfterLoad: function () {
                $('.p22-loading').hide();
            }
        }
    });
    //attachEvents($$('frame-content'), webix.events.iframe)

    webix.event(window, "resize", function () {
        if (document.documentElement.clientWidth < util.sizes.md) {
            _page.dock_l = _page.show_l = _page.dock_r = _page.show_r = false;
        }
        else {
            _page.dock_l = true;
            _page.show_l = _page.dock_r = _page.show_r = false;
        }
        _page.update();

        $$('frame-content').adjust();
    });
    
    $('a[target="content"]').click(function () {
        event.preventDefault();
        var url = $(this).attr('href');
        if (url != null)
            $$('frame-content').define("src", url);
    });

    //webix.ui({
    //    container: 'left_sidebar',
    //    view: "sidebar",
    //    id: 'sidebar1',
    //    //css: "webix_dark",
    //    data: menu_data,
    //    width: left_sidebar.clientWidth,
    //    height: -1,
    //    on: {
    //        onAfterSelect: function (id) {
    //            webix.message("Selected: " + this.getItem(id).value);
    //        }
    //    }
    //});

    //var ww = _page.size_l;
    //setInterval(function () {
    //    var _ww = left_sidebar.clientWidth;
    //    if (ww != _ww) {
    //        ww = _ww;
    //        var n = $$('sidebar1');
    //        n.define('width', ww); //_this.$el.clientWidth;
    //        n.resize();
    //    }
    //}, 10);


    $('#loading').remove();
});