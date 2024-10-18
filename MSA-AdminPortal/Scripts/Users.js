$(function () {




    // Document.ready -> link up remove event handler
    $(".ActionLink").click(function () {
        // Get the id from the link
       
    });

});

$('#basic_modalPopup').on('shown.bs.modal', function (e) {
    $('#btnSave').addClass('defaultBtnClass');
})

$('#basic_modalPopup').on('hidden.bs.modal', function () {
    $('#btnSave').removeClass('defaultBtnClass');
});