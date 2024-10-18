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
        updateHomeroom();

    })
});

function updateHomeroom() {
    $('#btnHomeRoomEdit').attr('disabled', 'disabled');
    $.ajax({
        type: "post",
        url: '/Homeroom/EditHomeroom',
        data: {
            "homeroomId": $("#Id").val(),
            "homeroomName": $("#Name").val(),
            "schoolId": $('#SchoolList').val(),
        },
        dataType: "json",
        success: function (data) {
            $('#btnHomeRoomEdit').removeAttr('disabled');
            if (data == "error") {
                displayErrorMessage("Error in updating Homeroom. Please try again!");
                $('#btnHomeRoomEdit').removeAttr('disabled');
            }
            else if (data == "duplicate") {
                displayWarningMessage("This homeroom already exists in the system. Please use the name of a different homeroom.");
                $('#btnHomeRoomEdit').removeAttr('disabled');
            } else {
                displaySuccessMessage('The Homeroom record has been updated successfully.');
                window.location.replace("/Homeroom");
            }
        },
        error: function (data) {
            $('#btnHomeRoomEdit').removeAttr('disabled');
            displayErrorMessage("Error in updating Homeroom. Please try again!");
        }
    });
}