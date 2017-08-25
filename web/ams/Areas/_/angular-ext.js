//angular.module('ams.ext').directive('uiEventx', ['$parse',
//  function ($parse) {
//      return function (scope, elm, attrs) {
//          //var events = scope.$eval(attrs.uiEvent);
//          //angular.forEach(events, function (uiEvent, eventName) {
//          //    var fn = $parse(uiEvent);
//          //    elm.bind(eventName, function (evt) {
//          //        var params = Array.prototype.slice.call(arguments);
//          //        //Take out first paramater (event object);
//          //        params = params.splice(1);
//          //        scope.$apply(function () {
//          //            fn(scope, { $event: evt, $params: params });
//          //        });
//          //    });
//          //});
//      };
//  }]);

//angular.module('ui.directives').directive('uiResize', ['$window', '$parse', function ($window, $parse) {
//    return {
//        link: function (scope, elem, attrs) {
//            var fn = $parse(attrs.uiResize);
//            var w = angular.element($window);
//            w.bind('resize', function () {
//                //console.log('resize', fn);
//                fn(scope, { $elem: [elem] });
//                //scope.$eval(attrs.uiResize);
//            });
//            //console.log('ui-keypress2', $window);
//            //console.log('ui-keypress2', arguments);
//            //scope.$eval(attrs.uiResize);
//        }
//    };
//}]);

//function ng_app(name) {
//    var modules = [/*'ngAnimate',*/ 'ui.bootstrap', 'ui.directives'];
//    if ($ != null) {
//        if ($.jqx) { modules.push('jqwidgets'); }
//        //if ($.jstree) { modules.push('ngJsTree'); }
//    }
//    var app = angular.module('app', modules);
//    //app.config([
//    //        '$animateProvider', function ($animateProvider) {
//    //            $animateProvider.classNameFilter(/^(?:(?!ng-animate-disabled).)*$/); /* disable animation */
//    //        }]);
//    return app;
//}

//angular.module('ui.directives').directive('ngInit2', ['$parse', function ($parse) {
//    // <div ui-dock="tree.docking($info)></div>
//    return {
//        link: function (scope, elem, attrs) {
//            var fn = $parse(attrs.ngInit2);
//            fn(scope, { $info: { element: elem } });
//        }
//    };
//}]);

//angular.module('ui.directives').directive('uiResize', ['$window', '$parse', function ($window, $parse) {
//    // <div ui-resize="resize($info)></div>
//    return {
//        link: function (scope, elem, attrs) {
//            var parent = elem.parent();
//            var fn = $parse(attrs.uiResize);

//            function _set_size() {
//                var size = { element: elem, parent: parent };
//                size.parentWidth = parent.innerWidth();
//                size.parentHeight = parent.innerHeight();
//                size.currentWidth = elem.outerWidth();
//                size.currentHeight = elem.outerHeight();
//                size.border_x = size.currentWidth - elem.innerWidth();
//                size.border_y = size.currentHeight - elem.innerHeight();
//                fn(scope, { $size: size });
//            }
//            _set_size();
//            angular.element($window).bind('resize', function () {
//                _set_size();
//                scope.$apply();
//            });
//        }
//    };
//}]);

//angular.module('ui.directives').directive('uiJqxDock2', ['$window', '$parse', function ($window, $parse) {
//    return {
//        link: function (scope, elem, attrs) {
//            var parent = elem.parent();

//            function _set_size(cb) {
//                if (!attrs.hasOwnProperty('jqxSettings')) return;
//                if (!scope.hasOwnProperty(attrs.jqxSettings)) return;

//                var jqxSettings = scope[attrs.jqxSettings];
//                var x1 = parent.innerWidth();
//                var y1 = parent.innerHeight();
//                var x2 = elem.outerWidth() - elem.innerWidth();
//                var y2 = elem.outerHeight() - elem.innerHeight();
//                jqxSettings.width = x1 - x2;
//                jqxSettings.height = y1 - y2;

//                for (var n in jqxSettings) {
//                    if (n.startsWith('jqx') && $.isFunction(jqxSettings[n])) {
//                        try { return cb(jqxSettings[n], jqxSettings); }
//                        catch (e) { }
//                    }
//                }
//            }

//            _set_size($.noop);

//            angular.element($window).bind('resize', function () {
//                _set_size(function (jqxFn, jqxSettings) {
//                    jqxFn({
//                        width: jqxSettings.width,
//                        height: jqxSettings.height
//                    });
//                    console.log('ui-jqx-dock', [jqxSettings, scope, elem, attrs]);
//                });
//            });
//        }
//    };
//}]);

//// auto dock with watching
//angular.module('ui.directives').directive('uiJqxDock', ['$window', '$parse', function ($window, $parse) {
//    return {
//        link: function (scope, elem, attrs) {
//            if (!attrs.hasOwnProperty('jqxSettings')) return;
//            if (!scope.hasOwnProperty(attrs.jqxSettings)) return;
//            var jqxSettings = scope[attrs.jqxSettings];
//            var parent = elem.parent();

//            scope.$watch(function () {
//                return parent.innerWidth();//$window.innerWidth;
//            }, function (oldValue, value) {
//                jqxSettings.width = parent.innerWidth() - (elem.outerWidth() - elem.innerWidth());
//                //console.log(attrs.jqxSettings + " x " + jqxSettings.width, arguments);
//            });
//            scope.$watch(function () {
//                return parent.innerHeight();//$window.innerHeight;
//            }, function (oldValue, value) {
//                jqxSettings.height = parent.innerHeight() - (elem.outerHeight() - elem.innerHeight());
//                //console.log(attrs.jqxSettings + " y " + jqxSettings.height, arguments);
//            });

//            //function _set_size() {
//            //    var x1 = parent.innerWidth();
//            //    var y1 = parent.innerHeight();
//            //    var x2 = elem.outerWidth() - elem.innerWidth();
//            //    var y2 = elem.outerHeight() - elem.innerHeight();
//            //    jqxSettings.width = x1 - x2;
//            //    jqxSettings.height = y1 - y2;
//            //    console.log(jqxSettings);
//            //}
//            //_set_size();
//            //angular.element($window).bind('resize', function () {
//            //    _set_size();
//            //    scope.$apply();
//            //});
//        }
//    };
//}]);
