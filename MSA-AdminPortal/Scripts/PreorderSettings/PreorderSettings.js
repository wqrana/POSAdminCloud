/**
 * Module Description
 * Preorder Management
 * Functionality
 * Preorder Handling Type
 * Version    Date             Author           Remarks
 * 1.00       21 March 2017     Waqar           Javascript for clientside handling
 *
 */

$(document).ready(function () {
    
    disableUpdateRights("UpdateSettings", "ApplyBtnID", "You don’t have rights to update the Preorder settings")

    $('#POPickMode').select2();

    $('#POPickMode').change(function () {
        var opt = $('#POPickMode :selected').val();
        var PickModeDescription = '';
        switch (opt) {

            case '0':
                PickModeDescription = 'In this mode, a user is responsible for manually marking each preorder item as picked up.';
                break;
            case '1':
                PickModeDescription = "In this mode, the server will automatically mark all preorder items that were to be served today.  A User may come back into this screen and manually mark them as not picked up or picked up.";
                break;

            case '2':
                PickModeDescription = "In this mode, the cashier at the POS will be able to mark the preordered items as the customers come through the line ";
                PickModeDescription+="to pickup their items.  A User may come back into this screen and manually mark the items as picked up.";
                break;

        }

        if (opt != '') {

            $('#HandlingTypeDescription p').text(PickModeDescription);
        }

        

    });



});



function SavePreorderSettings()
{
    var url = "/PreorderSettings/SaveSettings";
    var id = $('#Id').val();
    var POPickMode = $('#POPickMode :selected').val();

    if (id != null)
    {
            $.ajax({
                type: "post",
                url: url,
                data: {                    
                    'Id': id,
                    'POPickMode': POPickMode
                     },
                dataType: "json",
                success: function (data) {

                    var res = data;

                    if (res.result == 1) {
                        console.log(res);
                        displaySuccessMessage('Setting applied successfully.');
                       // window.location.reload(true);
                    }
                    else {
                        displayErrorMessage('Error applying Settings.');
                    }
                },
                error: function () {
                    displayErrorMessage("Error applying Settings.");
                }
            });
        } 
    
    else {
        displayErrorMessage("Select the Handling Type.");
    }
}

function disableUpdateRights(HiddednFielID, updateButtonID , warningMessage) {
    var UpdateHiddenField = $("#" + HiddednFielID).val();

    if (UpdateHiddenField == "False") {

        //create new school

        $("#" + updateButtonID).prop("disabled", true);
             
        $('#sub-div').click(function (e) {
            displayWarningMessage(warningMessage);
        });
    }
}

