; (function ($) {
    $.app_init = function () {
        window.app_init = new Array();
        window.app = angular
            .module('app', ['ui.bootstrap', 'ui.directives', 'jqwidgets'])
            .controller('main', function ($scope, $compile, $http) {
                window.$scope = $scope;
                window.$compile = $compile;
                window.$http = $http;
                app_init.forEach(function (cb) { cb.call(app, $scope, $compile, $http); });
                $(document).trigger('ng-app-init', [$scope, $compile, $http]);
            });
        delete $.app_init;
    }

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
    $.isBoolean = function (obj) {
        if (obj == true) return true;
        if (obj == false) return true;
        return false;
    }
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

    $.blockUI.defaults = {
        // message displayed when blocking (use null for no message)
        message: null,

        title: null,		// title string; only used when theme == true
        draggable: true,	// only used when theme == true (requires jquery-ui.js to be loaded)

        theme: false, // set to true to use with jQuery UI themes

        // styles for the message when blocking; if you wish to disable
        // these and use an external stylesheet then do this in your code:
        // $.blockUI.defaults.css = {};
        css: {
            padding: 0,
            margin: 0,
            width: '30%',
            top: '40%',
            left: '35%',
            textAlign: 'center',
            color: '#000',
            border: '3px solid #aaa',
            backgroundColor: '#fff',
            cursor: null
        },

        // minimal style set used when themes are used
        themedCSS: {
            width: '30%',
            top: '40%',
            left: '35%'
        },

        // styles for the overlay
        overlayCSS: {
            backgroundColor: '#000',
            opacity: 0.6,
            cursor: null
        },

        // style to replace wait cursor before unblocking to correct issue
        // of lingering wait cursor
        cursorReset: 'default',

        // styles applied when using $.growlUI
        growlCSS: {
            width: '350px',
            top: '10px',
            left: '',
            right: '10px',
            border: 'none',
            padding: '5px',
            opacity: 0.6,
            cursor: 'default',
            color: '#fff',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            'border-radius': '10px'
        },

        // IE issues: 'about:blank' fails on HTTPS and javascript:false is s-l-o-w
        // (hat tip to Jorge H. N. de Vasconcelos)
        /*jshint scripturl:true */
        iframeSrc: /^https/i.test(window.location.href || '') ? 'javascript:false' : 'about:blank',

        // force usage of iframe in non-IE browsers (handy for blocking applets)
        forceIframe: false,

        // z-index for the blocking overlay
        baseZ: 1000,

        // set these to true to have the message automatically centered
        centerX: true, // <-- only effects element blocking (page block controlled via css above)
        centerY: true,

        // allow body element to be stetched in ie6; this makes blocking look better
        // on "short" pages.  disable if you wish to prevent changes to the body height
        allowBodyStretch: true,

        // enable if you want key and mouse events to be disabled for content that is blocked
        bindEvents: true,

        // be default blockUI will supress tab navigation from leaving blocking content
        // (if bindEvents is true)
        constrainTabKey: true,

        // fadeIn time in millis; set to 0 to disable fadeIn on block
        fadeIn: 200,

        // fadeOut time in millis; set to 0 to disable fadeOut on unblock
        fadeOut: 400,

        // time in millis to wait before auto-unblocking; set to 0 to disable auto-unblock
        timeout: 0,

        // disable if you don't want to show the overlay
        showOverlay: true,

        // if true, focus will be placed in the first available input field when
        // page blocking
        focusInput: true,

        // elements that can receive focus
        focusableElements: ':input:enabled:visible',

        // suppresses the use of overlay styles on FF/Linux (due to performance issues with opacity)
        // no longer needed in 2012
        // applyPlatformOpacityRules: true,

        // callback method invoked when fadeIn has completed and blocking message is visible
        onBlock: null,

        // callback method invoked when unblocking has completed; the callback is
        // passed the element that has been unblocked (which is the window object for page
        // blocks) and the options that were passed to the unblock call:
        //	onUnblock(element, options)
        onUnblock: null,

        // callback method invoked when the overlay area is clicked.
        // setting this will turn the cursor to a pointer, otherwise cursor defined in overlayCss will be used.
        onOverlayClick: null,

        // don't ask; if you really must know: http://groups.google.com/group/jquery-en/browse_thread/thread/36640a8730503595/2f6a79a77a78e493#2f6a79a77a78e493
        quirksmodeOffsetHack: 4,

        // class name of the message block
        blockMsgClass: 'blockMsg',

        // if it is already blocked, then ignore it (don't unblock and reblock)
        ignoreIfBlocked: true
    };

    $.iframe_auto_height = function (o) {
        var o = $.extend({
            init: 0,
            interval: 10,
            duration: 200,
            animate: false
        }, o);
        var lastHeight = 0;// document.body.clientHeight;
        function run() {
            var height = document.body.scrollHeight; //scrollHeight
            if (lastHeight == height) {
                timer = setTimeout(run, o.interval);
            }
            else {
                lastHeight = height
                if (o.animate) {
                    $(window.frameElement).animate(
                        { height: height },
                        { queue: false, duration: o.duration, complete: run });
                }
                else {
                    var iframe = $(window.frameElement);
                    if (iframe.length == 0) return;
                    iframe.height(height);
                    timer = setTimeout(run, o.interval);
                }
            }
        };
        setTimeout(run, o.init);
    }
})($);