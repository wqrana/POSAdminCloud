jQuery(document).ready(function () {
    postUserTimeZone();
});
function postUserTimeZone() {
    var timezone = moment().format('Z');
    $.ajax({
        type: 'POST',
        url: '/Account/PostTimeZone/?timeZone=' + encodeURIComponent(timezone.replace('+', '')),
        success: function (data) {
            console.log('time zone saved.');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log('error on saving timezone.');
        }
    });
}