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

$(document).ready(function () {

    $(window).resize(function () {
        if (document.documentElement.clientWidth < util.sizes.md) {
            _page.dock_l = _page.show_l = _page.dock_r = _page.show_r = false;
        }
        else {
            _page.dock_l = true;
            _page.show_l = _page.dock_r = _page.show_r = false;
        }
        _page.update();

        //$$('frame-content').adjust();
    });


    $('.p22-loading').hide();
    $('.p22-iframe').load(function () {
        $('.p22-loading').hide();
    });
    $('a[target="content"]').click(function () {
        event.preventDefault();
        var url = $(this).attr('href');
        if (url != null) {
            $('.p22-loading').show();
            $('.p22-iframe').attr('src', url);
            $('.p22-loading').show();
        }
    });
    $('#loading').remove();
});