$(document).ready(function () {
    $("#Name").focus();
    $('.submitClass').click(function () {

        var element = $("#Name");


        if (element.val().trim().length == 0) {
            displayWarningMessage("Please enter value for Grade.");
            return false;
        }

        
        updateGrade();

    })
});

function updateGrade() {
    $('#btnGradeEdit').attr('disabled', 'disabled');
    $.ajax({
        type: "post",
        url: '/Grade/Edit',
        data: {
            "gradeId": $("#Id").val(),
            "gradeName": $("#Name").val(),
        },
        dataType: "json",
        success: function (data) {
            $('#btnGradeEdit').removeAttr('disabled');
            if (data == "error") {
                displayErrorMessage("Error in updating Grade. Please try again!");
            }
            if (data == "duplicate") {
                displayWarningMessage("This grade already exists in the system. Please use the name of a different grade.");
                $('#btnGradeEdit').removeAttr('disabled');
            }
            else {
                displaySuccessMessage('The Grade record has been updated successfully.');

                window.location.replace("/Grade");
            }
        },
        error: function (data) {
            $('#btnGradeEdit').removeAttr('disabled');
            displayErrorMessage("Error in updating Grade. Please try again!");
        }
    });
}