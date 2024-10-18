
var options = {
    "closeButton": true,
    "debug": false,
    "positionClass": "toast-top-center",
    "onclick": null,
    "showDuration": "1000",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut",
    "preventDuplicates" : true
}

function displayErrorMessage(message) {

    toastr.options = options;

    if (!hasSameErrorToastr(message)) {
        toastr.error(message, "Error!");
    }
}

function displayWarningMessage(message) {

    toastr.options = options;
    
    if (!hasSameWarningToastr(message)) {
        toastr.warning(message, "Warning!");
    }
}

function displaySuccessMessage(message) {

    toastr.options = options;
    toastr.success(message, "Success!");
}


function displaySuccessMessageOnce(message) {

    if (!hasSameSuccessToastr(message)) {
        toastr.options = options;
        toastr.success(message, "Success!");
    }
}

function displayInfoMessage(message) {

    toastr.options = options;
    toastr.info(message, "Info!");
}

function RedirectToIndex(path)
{
    window.location.href = path;
}

function hasSameErrorToastr(message) {
    //debugger;
    var hasSameErrorToastr = false;

    var $toastContainer = $('#toast-container');
    if ($toastContainer.length > 0) {
        var $errorToastr = $toastContainer.find('.toast-error');
        if ($errorToastr.length > 1) return true;
        if ($errorToastr.length > 0) {
            var currentText = $errorToastr.find('.toast-message').text();
            var areEqual = message.toUpperCase() === currentText.toUpperCase();
            if (areEqual) {
                hasSameErrorToastr = true;
            }
        }
    }

    return hasSameErrorToastr;
}


function hasSameWarningToastr(message) {
    //debugger;
    var hasSameWarningToastr = false;

    var $toastContainer = $('#toast-container');
    if ($toastContainer.length > 0) {
        var $errorToastr = $toastContainer.find('.toast-warning');
        if ($errorToastr.length > 0) return true;
        if ($errorToastr.length > 1) {
            var currentText = $errorToastr.find('.toast-message').text();
            var areEqual = message.toUpperCase() === currentText.toUpperCase();
            if (areEqual) {
                hasSameWarningToastr = true;
            }
        }
    }

    return hasSameWarningToastr;
}

function hasSameSuccessToastr(message) {
    //debugger;
    var hasSameWarningToastr = false;

    var $toastContainer = $('#toast-container');
    if ($toastContainer.length > 0) {
        var $errorToastr = $toastContainer.find('.toast-success');
        if ($errorToastr.length > 0) return true;
        if ($errorToastr.length > 1) {
            var currentText = $errorToastr.find('.toast-message').text();
            var areEqual = message.toUpperCase() === currentText.toUpperCase();
            if (areEqual) {
                hasSameWarningToastr = true;
            }
        }
    }

    return hasSameWarningToastr;
}

function escapeHtml(text) {
    var map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        //'"': '&quot;',
        '"': '\\"',
        "'": '&#039;'
    };

    return text.replace(/[&<>'"]/g, function (m) { return map[m]; }).replace(/ /g, "&nbsp;");
}


function formatDollar(num) {
    //debugger;
    var twoPlacedFloat = parseFloat(num).toFixed(2)
    var p = twoPlacedFloat.split(".");
    return "$" + p[0].split("").reverse().reduce(function (acc, twoPlacedFloat, i, orig) {
        return twoPlacedFloat == "-" ? acc : twoPlacedFloat + (i && !(i % 3) ? "," : "") + acc;
    }, "") + "." + p[1];
}

//attach keypress to input
$('.numericOnly').keydown(function (event) {
    // Allow special chars + arrows 
    if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9
        || event.keyCode == 27 || event.keyCode == 13
        || (event.keyCode == 65 && event.ctrlKey === true)
        || (event.keyCode >= 35 && event.keyCode <= 39)) {
        return;
    } else {
        // If it's not a number stop the keypress
        if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
            event.preventDefault();
        }
    }
});

$.fn.enterKey = function (fnc) {
    return this.each(function () {
        $(this).keypress(function (ev) {
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if (keycode == '13') {
                fnc.call(this, ev);
            }
        })
    })
}

$.fn.escKey = function (fnc) {
    return this.each(function () {
        $(this).keydown(function (ev) {
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if (keycode == '27') {
                fnc.call(this, ev);
            }
        })
    })
}

$.fn.numericOnly = function (fnc) {
    return $(this).keydown(function (event) {
        // Allow special chars + arrows 
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9
            || event.keyCode == 27 || event.keyCode == 13
            || (event.keyCode == 65 && event.ctrlKey === true)
            || (event.keyCode >= 35 && event.keyCode <= 39)) {
            return;
        } else {
            // If it's not a number stop the keypress
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });
}

