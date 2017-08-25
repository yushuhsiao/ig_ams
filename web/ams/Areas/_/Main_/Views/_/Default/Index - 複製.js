
var _sidebar = {
    toggle_panel: function () { $(document.body).toggleClass('sidebar-toggle');},
    toggle: function () { $(this).siblings('.list-group').collapse('toggle'); },
    showAll: function () { $('#sidebar .list-group .list-group').collapse('show'); },
    hideAll: function () { $('#sidebar .list-group .list-group').collapse('hide'); }
};

$(document).ready(function () {
    //$('#sidebar .list-group > li').addClass('list-group-item');
    //$('#sidebar .list-group .list-group').addClass('collapse').parent().find('div,a').click(_sidebar.toggle);
    //$('#sidebar .list-group-item').each(function () {
    //    var group = $('.list-group', this);
    //    var div = $('<div></div>').appendTo(this);
    //    $(this).children().appendTo(div);
    //    group.appendTo(this);
    //});

    $('.sidebar-toggle-button').click(_sidebar.toggle_panel);

    $('#navbar-wrapper .dropdown')
        .on('show.bs.dropdown', function () { $('#content-mask').removeClass('hidden'); })
        .on('hidden.bs.dropdown', function () { $('#content-mask').addClass('hidden'); });
});
