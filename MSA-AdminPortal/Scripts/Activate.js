
var activateUrl = $('#ActivateUrl').val();

function Activate(e) {

    var id = $(e).attr("data-id");
    //debugger;
    if (id != '') {
        $('#btnActivate').prop('disabled', true);
        $.ajax({
            type: "get",
            url: activateUrl,
            data: { "id": id},
            dataType: "json",
            success: function (data) {
                
                if (data.IsError) {
                    $("#Activate").modal('hide');
                    displayWarningMessage(data.Message);
                }
                else {
                    $('#Id').val(id);
                    $('#IsActive').val(data.IsActive);
                    $('#span-activate-title').text(data.Name);
                    $('#span-activate-name').text(data.Name);
                    $('#activeStringTitle').text(data.ActiveString);
                    $('#activeStringMsg').text(data.ActiveString.toLowerCase());
                    $('#btnActivate').text(data.ActiveString);
                    $('#btnActivate').prop('disabled', false);
                    $("#Activate").modal('show');
                }
            },
            error: function (data) {
                $("#Activate").modal('hide');
                displayErrorMessage(data.Message);
            }
        });
    }
}

$(function () {

    $(".ActivateLink").click(function () {
        
        Activate(this);
    });

    $("#btnActivate").click(function () {
        
        var id = $("#Id").val();
        var isActive = $("#IsActive").val();

        if (id != '') {

            $.ajax({
                type: "post",
                url: activateUrl,
                data: {
                    "Id": id,
                    "isActive": isActive
                },
                dataType: "json",
                success: function (data) {
                    if (data.IsError) {
                        $("#Activate").modal('hide');
                        displayWarningMessage(data.Message);
                    }
                    else {
                        displaySuccessMessage(data.Message);
                        window.location.reload(true);
                    }
                },
                error: function (data) {
                    $("#Activate").modal('hide');
                    displayWarningMessage(data.Message);
                }
            });
        }
    });
});