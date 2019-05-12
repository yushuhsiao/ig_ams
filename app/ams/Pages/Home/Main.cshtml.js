var a1 = {
    Logout_Url: null,

    dock_l: true,
    show_l: false,
    size_l: 300,
    left_1: function () {
        if (a1.dock_l)
            return a1.size_l;
        else
            return 0;
    },
    left_2: function () {
        if (a1.dock_l || a1.show_l)
            return 0;
        return -a1.size_l;
    },

    dock_r: false,
    show_r: false,
    size_r: 300,
    right_1: function () {
        if (a1.dock_r)
            return a1.size_r;
        else
            return 0;
    },
    right_2: function () {
        if (a1.dock_r || a1.show_r)
            return 0;
        return -a1.size_r;
    },

    tabIndex: 2,

    toggle_left: function (isSmall) {
        if (isSmall) {
            a1.show_l = !a1.show_l;
            a1.dock_l = false;
            a1.dock_r = a1.show_r = false;
        }
        else {
            a1.show_l = false;
            a1.dock_l = !a1.dock_l;
        }

        a1.update();
    },

    toggle_right: function (index, isSmall) {
        if (index == null)
            index = a1.tabIndex;

        if (isSmall) {
            a1.show_r = !a1.show_r;
            a1.dock_r = false;
            a1.dock_l = a1.show_l = false;
        }
        else {
            if (a1.tabIndex == index) {
                a1.show_r = !a1.show_r;
                a1.dock_r = false;
            }
            else {
                a1.show_r = true;
                a1.dock_r = false;
            }
            a1.tabIndex = index;
        }

        a1.update();
    },

    mask_click: function () {
        a1.show_r = false;
        a1.update();
    },

    update: function () {
        $('.p22').css({
            'padding-left': a1.left_1(),
            'padding-right': a1.right_1()
        });
        $('.p21').css({
            'left': a1.left_2(),
            'width': a1.size_l
        });
        $('.p23').css({
            'right': a1.right_2(),
            'width': a1.size_r
        });

        $('.p23-1').hide();
        $('.p23-2').hide();
        if (a1.tabIndex == 1) {
            $('.p23-1').show();
        }
        else if (a1.tabIndex == 2) {
            $('.p23-2').show();
        }

        if (a1.show_r) {
            $('.p22-mask').show();
        }
        else {
            $('.p22-mask').hide();
        }
    },

    logout: function () {
        util.api(this.Logout_Url, {}, function (result) {
            if (result.IsSuccess) {
                window.location.reload(true);
            }
        });
    }
};

a1.update();

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
//        a1.dock_l = a1.show_l = a1.dock_r = a1.show_r = false;
//    }
//    else {
//        a1.dock_l = true;
//        a1.show_l = a1.dock_r = a1.show_r = false;
//    }
//    a1.update();
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
            a1.dock_l = a1.show_l = a1.dock_r = a1.show_r = false;
        }
        else {
            a1.dock_l = true;
            a1.show_l = a1.dock_r = a1.show_r = false;
        }
        a1.update();

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

    //var ww = a1.size_l;
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