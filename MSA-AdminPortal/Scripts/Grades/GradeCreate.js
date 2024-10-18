$(document).ready(function () {
    $("#Name").focus();

    $('.submitClass').click(function () {

        var element = $("#Name");


        if (element.val().trim().length == 0) {
            displayWarningMessage("Please enter value for Grade.");
            return false;
        }


        createGrade();


    })
});

function createGrade() {
    $('#btnGradeCreate').attr('disabled', 'disabled');
    $.ajax({
        type: "post",
        url: '/Grade/Create',
        data: {
            "gradeName": $("#Name").val(),
        },
        dataType: "json",
        success: function (data) {
            $('#btnGradeCreate').removeAttr('disabled');
            if (data=="error") {
                displayErrorMessage("Error in creating Grade. Please try again!");
            }
            if (data == "duplicate")
            {
                displayWarningMessage("This grade already exists in the system. Please use the name of a different grade.");
                $('#btnGradeCreate').removeAttr('disabled');
            }
            else {
                displaySuccessMessage('The Grade record has been created successfully.');

                window.location.replace("/Grade");
            }
        },
        error: function (data) {
            $('#btnGradeCreate').removeAttr('disabled');
            displayErrorMessage("Error in creating Grade. Please try again!");
        }
    });
}