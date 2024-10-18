if (!idleAfter) idleAfter = 15;
if (!timeout) timeout = 30000;
if (!warningLength) warningLength = 120;


var UIIdleTimeout = function () {

    return {

        //main function to initiate the module
        init: function () {

            // cache a reference to the countdown element so we don't have to query the DOM for it on each ping.
            var $countdown;

            $('body').append('<div class="modal fade" id="idle-timeout-dialog" data-backdrop="static"><div class="modal-dialog modal-small"><div class="modal-content"><div class="modal-header"><h4 class="modal-title">Your session is about to expire.</h4></div><div class="modal-body"><p><i class="fa fa-warning"></i> Your session will be locked in <span id="idle-timeout-counter"></span> seconds.</p><p>Do you want to continue your session?</p></div><div class="modal-footer"><button id="idle-timeout-dialog-logout" type="button" class="btn btn-default">No, Logout</button><button id="idle-timeout-dialog-keepalive" type="button" class="btn btn-primary" data-dismiss="modal" onclick="keepSessionAlive()">Yes, Keep Working</button></div></div></div></div>');

            // start the idle timer plugin
            $.idleTimeout('#idle-timeout-dialog', '.modal-content button:last', {
                idleAfter: idleAfter, // 5 seconds
                timeout: timeout * 1000, //30 seconds to timeout
                pollingInterval: 5, // 5 seconds
                keepAliveURL: '/',
                serverResponseEquals: 'OK',
                onTimeout: function () {
                    window.location = "/Account/Login/";
                },
                onIdle: function () {
                    $('#idle-timeout-dialog').modal('show');
                    $countdown = $('#idle-timeout-counter');

                    $('#idle-timeout-dialog-keepalive').on('click', function () {
                        $('#idle-timeout-dialog').modal('hide');
                    });

                    $('#idle-timeout-dialog-logout').on('click', function () {
                        $('#idle-timeout-dialog').modal('hide');
                        $.idleTimeout.options.onTimeout.call(this);
                    });
                },
                onCountdown: function (counter) {
                    $countdown.html(counter); // update the counter
                }
            });

        }

    };

}();

function keepSessionAlive() {
    $.ajax({
        type: "POST",
        url: "/Home/KeepSessionAlive",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            UIIdleTimeout.init();
        },
        error: function (request, status, error) {
            window.location = "/Account/Login/";
        }
    });
}