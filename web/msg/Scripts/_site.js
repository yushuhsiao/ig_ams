if (typeof jQuery === 'undefined') { }
else {
    (function ($) {
        jQuery.extend({
            invoke_api: function (options, data) {
                var opt = {
                    contentType: 'application/json',
                    type: 'post',
                    dataType: 'json',
                    cache: false,
                    async: true,
                    data: JSON.stringify(data)
                };
                if (typeof options == 'string') {
                    opt['url'] = options;
                }
                else {
                    if (options.data)
                        delete options.data;
                    opt = $.extend(true, opt, options);
                }
                return $.ajax(opt);
            }
        });
    })(jQuery);

    if ($.jqx) $.jqx.theme = 'bootstrap';
}

$.fn.getFormData = function (reset) {
    var formData = {};
    $('input[type="text"], input[type="password"], input[type="radio"]:checked, select, textarea', this).each(function () {
        var name = $(this).prop('name') || '';
        if (name.length > 0) {
            formData[name] = $.trim($(this).val()) || '';
            if (reset == true)
                $(this).val('');
        }
    });
    return formData
}
