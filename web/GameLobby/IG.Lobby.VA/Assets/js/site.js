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

function openNewWindow(url, width, height) {
    return window.open(url, '_blank', 'resizable=yes,location=no,menubar=no,scrollbars=no,status=no,titlebar=no,toolbar=no,width=' + width + ',height=' + height);
}

function formatMoney(money) {
    return (parseFloat(money, 10).toFixed(2) + '').replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}
