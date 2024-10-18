$(document).ready(function () {
    // Dropdowns
    $('#SchoolList').select2();
    $("#Name").focus();
    $('.submitClass').click(function () {

        var element = $("#Name");


        if (element.val().trim().length == 0) {
            displayWarningMessage("Please enter value for homeroom.");
            return false;
        }

        var schoolVal = $('#SchoolList').val();
        if (schoolVal == "") {
            displayWarningMessage("Please select a school.");
            return false;
        }
        createHomeroom();


    })
});

function createHomeroom() {
    $('#btnHomeRoomSave').attr('disabled', 'disabled');
    $.ajax({
        type: "post",
        url: '/Homeroom/CreateHomeroom',
        data: {
            "homeRoomName": $("#Name").val(),
            "SchoolID": $('#SchoolList').val(),
        },
        dataType: "json",
        success: function (data) {
            $('#btnHomeRoomSave').removeAttr('disabled');
            if (data === "duplicate") {
                displayWarningMessage("This homeroom already exists in the system. Please use the name of a different homeroom.");
                $('#btnHomeRoomSave').removeAttr('disabled');
            } else if (data === "error") {
                displayErrorMessage("Error in creating Homeroom. Please try again!");
                $('#btnHomeRoomSave').removeAttr('disabled');
            }
            else {
                displaySuccessMessage('The Homeroom record has been created successfully.');

                window.location.replace("/Homeroom");
            }
        },
        error: function (data) {
            $('#btnHomeRoomSave').removeAttr('disabled');
            displayErrorMessage("Error in creating Homeroom. Please try again!");
        }
    });
}