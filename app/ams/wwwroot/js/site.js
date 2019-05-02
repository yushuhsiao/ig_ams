(function () {

    function ApiResult(src, xhr) {
        try {
            if (src != null) {
                for (var n in src) {
                    this[n] = src[n];
                }
            }
        }
        catch ($e) {
        }
        try {
            if (xhr != null) {
                this.httpStatus = xhr.status;
                this.httpStatusText = xhr.statusText;
            }
        } catch ($e1) {
        }

        if (this.Errors == null)
            this.Errors = {};
        if (this.StatusCode == null)
            this.StatusCode = 0;
        if (this.StatusText == null)
            this.StatusText = "Unknown";
        if (this.Message == null) {
            if (this.httpStatus != null) {
                this.Message = "HTTP " + this.httpStatus + this.httpStatusText;
            }
            else
                this.Message = "Unknown";
        }
        if (this.Data == null)
            this.Data = {};

        if (this.httpStatus == null)
            this.httpStatus = 200;
        if (this.httpStatusText == null)
            this.httpStatusText = "OK";
    }

    Object.defineProperty(ApiResult.prototype, "IsSuccess", {
        get: function () {
            return this.StatusCode === 1;
        }
    });


    if (!window.util)
        window.util = {};

    util.sizes = {
        xs: 0,
        sm: 576,
        md: 768,
        lg: 992,
        xl: 1200
    };

    window.util.api = function (url, data, callback, opts) {
        if (window.jQuery) {
            api_jquery(url, data, callback, opts);
        } else {
            api_webix(url, data, callback, opts);
        }
    }

    function api_jquery(url, data, callback, opts) {
        if (opts == null)
            opts = {};

        var json = null;
        var result = null;

        $.ajax($.extend({
            success: function (response_data, textStatus, jqXHR) {
                try {
                    json = JSON.parse(response_data);
                    result = new ApiResult(json, jqXHR);
                } catch ($e1) {
                    console.log($e1);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                try {
                    json = JSON.parse(jqXHR.response);
                    result = new ApiResult(json, jqXHR);
                } catch ($e1) {
                    console.log($e1);
                }
                
            },
            complete: function (jqXHR, textStatus) {
                if (result == null)
                    result = new ApiResult(json, jqXHR);
                try {
                    callback(result);
                } catch ($e1) {
                    console.log($e1);
                }
            }
        }, opts, {
                url: url,
                data: JSON.stringify(data),
                contentType: "application/json",
                type: "post",
                dataType: "text",
                cache: false,
                async: true
            })
        );
    }
    
    function api_webix(url, data, callback, opts) {
        try {
            var data_json = JSON.stringify(data);
            var p = webix.ajax().headers({ "Content-Type": "application/json" }).post(url, data);

            p.then(function (n) {

                var result;
                try {
                    result = new ApiResult(n.json(), null);
                } catch ($e2) {
                    result = new ApiResult(null, null);
                }

                try {
                    callback(result);
                } catch ($e2) {
                    console.log($e2);
                }

            }, function (xhr) {

                var result;
                try {
                    var json = xhr.responseText;
                    result = new ApiResult(JSON.parse(json), xhr);
                } catch ($e2) {
                    result = new ApiResult(null, xhr);
                }

                try {
                    callback(result);
                } catch ($e2) {
                    console.log($e2);
                }

            });
        }
        catch ($e1) {
        }
    }
})();
webix.events = {
    datatable: ['onAfterAdd', 'onAfterAreaAdd', 'onAfterAreaRemove', 'onAfterBlockSelect', 'onAfterColumnDrop', 'onAfterColumnDropOrder', 'onAfterColumnHide', 'onAfterColumnShow', 'onAfterContextMenu', 'onAfterDelete', 'onAfterDrop', 'onAfterDropOrder', 'onAfterEditStart', 'onAfterEditStop', 'onAfterFilter', 'onAfterLoad', 'onAfterRender', 'onAfterScroll', 'onAfterSelect', 'onAfterSort', 'onAfterUnSelect', 'onAreaDrag', 'onBeforeAdd', 'onBeforeAreaAdd', 'onBeforeAreaRemove', 'onBeforeBlockSelect', 'onBeforeColumnDrag', 'onBeforeColumnDrop', 'onBeforeColumnDropOrder', 'onBeforeColumnHide', 'onBeforeColumnShow', 'onBeforeContextMenu', 'onBeforeDelete', 'onBeforeDrag', 'onBeforeDragIn', 'onBeforeDrop', 'onBeforeDropOrder', 'onBeforeDropOut', 'onBeforeEditStart', 'onBeforeEditStop', 'onBeforeFilter', 'onBeforeLoad', 'onBeforeRender', 'onBeforeSelect', 'onBeforeSort', 'onBeforeUnSelect', 'onBindRequest', 'onBlur', 'onCheck', 'onCollectValues', 'onColumnGroupCollapse', 'onColumnResize', 'onDataRequest', 'onDataUpdate', 'onDestruct', 'onDragOut', 'onEditorChange', 'onEnter', 'onFocus', 'onHeaderClick', 'onItemClick', 'onItemDblClick', 'onKeyPress', 'onLiveEdit', 'onLoadError', 'onLongTouch', 'onMouseMove', 'onMouseMoving', 'onMouseOut', 'onPaste', 'onResize', 'onRowResize', 'onScrollX', 'onScrollY', 'onSelectChange', 'onStructureLoad', 'onStructureUpdate', 'onSubViewClose', 'onSubViewCreate', 'onSubViewOpen', 'onSubViewRender', 'onSwipeX', 'onSwipeY', 'onTabFocus', 'onTimedKeyPress', 'onTouchEnd', 'onTouchMove', 'onTouchStart', 'onValidationError', 'onValidationSuccess', 'onViewResize'],
    tabbar: ['onAfterRender', 'onAfterScroll', 'onAfterTabClick', 'onBeforeRender', 'onBeforeTabClick', 'onBeforeTabClose', 'onBindRequest', 'onBlur', 'onChange', 'onDestruct', 'onEnter', 'onFocus', 'onItemClick', 'onKeyPress', 'onLongTouch', 'onOptionRemove', 'onSwipeX', 'onSwipeY', 'onTimedKeyPress', 'onTouchEnd', 'onTouchMove', 'onTouchStart', 'onViewResize'],
    iframe: ['onAfterLoad', 'onAfterScroll', 'onBeforeLoad', 'onBindRequest', 'onBlur', 'onDestruct', 'onEnter', 'onFocus', 'onKeyPress', 'onLongTouch', 'onSwipeX', 'onSwipeY', 'onTimedKeyPress', 'onTouchEnd', 'onTouchMove', 'onTouchStart', 'onViewResize'],
    tree: ['onAfterAdd', 'onAfterClose', 'onAfterContextMenu', 'onAfterDelete', 'onAfterDrop', 'onAfterLoad', 'onAfterOpen', 'onAfterRender', 'onAfterScroll', 'onAfterSelect', 'onAfterSort', 'onBeforeAdd', 'onBeforeClose', 'onBeforeContextMenu', 'onBeforeDelete', 'onBeforeDrag', 'onBeforeDragIn', 'onBeforeDrop', 'onBeforeDropOut', 'onBeforeLoad', 'onBeforeOpen', 'onBeforeRender', 'onBeforeSelect', 'onBeforeSort', 'onBindRequest', 'onBlur', 'onDataRequest', 'onDataUpdate', 'onDestruct', 'onDragOut', 'onEnter', 'onFocus', 'onItemCheck', 'onItemClick', 'onItemDblClick', 'onItemRender', 'onKeyPress', 'onLoadError', 'onLongTouch', 'onMouseMove', 'onMouseMoving', 'onMouseOut', 'onPartialRender', 'onPaste', 'onSelectChange', 'onSwipeX', 'onSwipeY', 'onTabFocus', 'onTimedKeyPress', 'onTouchEnd', 'onTouchMove', 'onTouchStart', 'onValidationError', 'onValidationSuccess', 'onViewResize', 'onViewShow']
};

function attachEvents(src, list, excludes) {

    function on_event(_src, n) {
        _src.attachEvent(n, function () {
            console.log(n, arguments);
        });
    }

    if (!Array.isArray(excludes))
        excludes = [];

    for (var i = 0; i < list.length; i++) {
        if (excludes.indexOf(list[i]) === -1)
            new on_event(src, list[i]);
    }
}
