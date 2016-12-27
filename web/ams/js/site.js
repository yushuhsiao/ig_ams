;
var bs = {
    screen: {
        xs: 480,    // @screen-xs
        sm: 768,    // @screen-sm
        md: 992,    // @screen-md
        lg: 1200,   // @screen-lg
        current: function () {
            var w = window.innerWidth;
            if (w >= bs.screen.lg) { return 'lg'; }
            if (w >= bs.screen.md) { return 'md'; }
            if (w >= bs.screen.sm) { return 'sm'; }
            return 'xs';
        }
    }
}
;
if (typeof jQuery === 'undefined') { }
else {
    (function ($) {
        jQuery.extend({
            invoke_api: function (options) {
                var opt = $.extend(true, {
                    contentType: 'application/json',
                    type: 'post',
                    dataType: 'json',
                    cache: false,
                    async: true,
                    //success: function (data, textStatus, jqXHR) {
                    //    if (data.Status == 1) {
                    //        if ($.isFunction(options.api_success)) {
                    //            options.api_success.call(this, data.Data, data.Message);
                    //        }
                    //    }
                    //    else {
                    //        if ($.isFunction(options.api_success)) {
                    //            options.api_success.call(this, data.Status, data.Data, data.Message);
                    //        }
                    //    }
                    //}
                }, options);
                if (opt.data) {
                    opt.data = JSON.stringify(opt.data)
                }
                return $.ajax(opt);
            }
        });
        return;
        $.invoke_api({
            beforeSend: function (jqXHR, settings) {
            },
            success: function (data, textStatus, jqXHR) {
            },
            api_success: function (data, message) {
            },
            api_error: function (errCode, data) {
            },
            error: function (jqXHR, textStatus, errorThrown) {
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    })(jQuery);

    //$.fn.getFormData = function (reset) {
    //    var formData = {};
    //    $('input[type="text"], input[type="password"], input[type="radio"]:checked, select, textarea', this).each(function () {
    //        var name = $(this).prop('name') || '';
    //        if (name.length > 0) {
    //            formData[name] = $.trim($(this).val()) || '';
    //            if (reset == true)
    //                $(this).val('');
    //        }
    //    });
    //    return formData
    //}
}
;
//function postMessage(event) {
//    var args = [];
//    for (var i = 1; i < arguments.length; i++)
//        args.push(arguments[i]);
//    (function _send() {
//        var iframes = this.document.getElementsByTagName('iframe');
//        for (var i = 0; i < iframes.length; i++) { _send.apply(iframes[i].contentWindow); }
//        $(this.document).trigger(event, args);
//    }).apply(window.top);
//};
function postMessage(target_method) {
    var args = [];
    for (var i = 1; i < arguments.length; i++)
        args.push(arguments[i]);
    (function _send() {
        var iframes = this.document.getElementsByTagName('iframe');
        for (var i = 0; i < iframes.length; i++) { _send.apply(iframes[i].contentWindow); }
        $(this.document).trigger('msg');
        if (typeof (this[target_method]) == 'function') {
            this[target_method].apply(this, args);
        }
        //console.log(typeof (this[target_method]), this[target_method]);
        //if (typeof this.recvMessage === 'function')
        //    this.recvMessage.apply(this, args);
    }).apply(window.top);
};

(function () {
    //判斷字串開頭是否為指定的字
    //回傳: bool
    if (!('startsWith' in String.prototype)) {
        String.prototype.startsWith = function (prefix) {
            return (this.substr(0, prefix.length) === prefix);
        }
    }

    //判斷字串結尾是否為指定的字
    //回傳: bool
    if (!('endsWith' in String.prototype)) {
        String.prototype.endsWith = function (suffix) {
            return (this.substr(this.length - suffix.length) === suffix);
        }
    }

    //判斷字串是否包含指定的字
    //回傳: bool
    if (!('contains' in String.prototype)) {
        String.prototype.contains = function (txt) {
            return (this.indexOf(txt) >= 0);
        }
    }

    if (!('replaceAll' in String.prototype)) {
        String.prototype.replaceAll = function (find, replace) {
            return this.replace(new RegExp(find, 'g'), replace);
        }
    }
})();
