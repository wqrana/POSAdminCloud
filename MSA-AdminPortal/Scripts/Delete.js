
var deleteUrl = $('#DeleteUrl').val();

function Delete(e) {
    debugger;
    var id = $(e).attr("data-id");

    if (id != '') {
        $.ajax({
            type: "get",
            url: deleteUrl,
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                if (data.IsError) {
                    displayWarningMessage(data.Message);
                }
                else {
                    $('#btnDelete').removeAttr("disabled");
                    $('#Id').val(id);
                    $('#span-del-title').text(data.Name);
                    //$('#span-del-name').text(data.Name);
                    //var txt = $('#h4text').text()
                    //$('#h4text').text(txt.toLocaleLowerCase());
                }
            },
            error: function (data) {
                displayErrorMessage(data.Message);
            }
        });
    }
}

$(function () {

    $(".DeleteLink").click(function () {
        //debugger;
        Delete(this);
    });

    $("#btnDelete").click(function () {
        debugger;
        var id = $("#Id").val();

        if (id != '') {

            $.ajax({
                type: "delete",
                url: deleteUrl,
                data: {
                    "Id": id
                },
                dataType: "json",
                success: function (data) {
                    if (data.IsError!="0") {
                        $('#Delete').modal('toggle');
                        displayWarningMessage(data.Message);
                    }
                    else {
                        debugger;
                        $('#Delete').modal('toggle');
                        
                           displaySuccessMessage(data.Message);
                           window.location.reload(true);

                  
                        
                    }
                },
                error: function (data) {
                    
                    $('#Delete').modal('toggle');
                    displayWarningMessage(data.Message);
                }
            });
        }
    });
});