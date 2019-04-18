; requirejs.config({
    baseUrl: "/",
    map: {
        "*": {
            "css": "js/requirejs/css.min.js"
        }
    },
    paths: {
        "jquery": "lib/jQuery/dist/jquery.min",
        "bootstrap": "lib/bootstrap/dist/js/bootstrap.bundle.min",
        "bootstrap-css": "lib/bootstrap/dist/css/bootstrap.min",
        "font-awesome": "lib/Font-Awesome/web-fonts-with-css/css/fontawesome-all.min",
        //"site-css": "css/site",
        "index-css": "css/Index",
        "jqx": "js/jqx",
        "jqx-css": "css/jqx",
        "webix": "lib/webix/codebase/webix_debug",
        "webix-css": "lib/webix/codebase/webix",
        "bridge": "js/bridge"
    },
    shim: {
        "bootstrap": {
            deps: ["css!bootstrap-css", "jquery"]
        },
        "jqx": {
            deps: ["jquery", "css!jqx-css"]
        },
        "webix": {
            deps: ["css!webix-css"]
        },
        "bridge": {
            deps: ["jquery"]
        },
    }
});

