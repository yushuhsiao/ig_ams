/*
*  webui popover angular.js plugin
*/
'use strict';

angular.module('webui.popover', []).directive('webuiPopover', function () {
    return {
        link: function link(scope, element, attrs) {
            attrs.$observe('show', function (value) {
                value = JSON.parse(value);
                if (value === true) {
                    element.webuiPopover('show');
                } else if (value === false) {
                    element.webuiPopover('hide');
                }
            });
            attrs.$observe('content', function (value) {
                element.webuiPopover('destroy');
                element.webuiPopover({
                    trigger: 'manual'
                });
            });
        }
    };
});

