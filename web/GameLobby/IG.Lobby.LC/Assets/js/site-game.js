var waitForFinalEvent = (function () {
    var timers = {};

    return function (callback, ms, uniqueId) {
        if (!uniqueId) {
            uniqueId = "Don't call this twice without a uniqueId";
        }
        if (timers[uniqueId]) {
            clearTimeout(timers[uniqueId]);
        }

        timers[uniqueId] = setTimeout(callback, ms);
    };
})();

function isFlashPlayerInstalled() {
    return swfobject.getFlashPlayerVersion().major !== 0;
}

function embedSwf(swfUrl, flashvars, id, width, height) {
    var params = {
        allowFullScreen: true,
        allowFullScreenInteractive: true,
        allowScriptAccess: 'always'
    };
    var attributes = {
        id: id
    };

    swfobject.embedSWF(swfUrl, attributes.id, width, height, '10.0.0', '/assets/swfobject/expressinstall.swf', flashvars, params, attributes);
    console.log(flashvars);
}

function removeSWF(id) {
    swfobject.removeSWF(id);
}
