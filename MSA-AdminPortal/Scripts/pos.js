
var posdelid = "";
var clientdelId = "";
var ccStatus = "";
var SchoolID = "";
var Name = "";
var sessionStatus = "Open";

$(document).ready(function () {

    $(".lnkItem").click(function (e) {
        //debugger;
        $.post({
            url: $(this).attr('href'),
            type: 'post',
            success: function (data) {
                // assuming you're returning a partial result
                $('#divDetail').html(data);
            }
        });
    });


    $(".settingslnk").click(function (e) {
       //   debugger;
        var height = '241px';
        var oForm = $(this).closest(".form-class");
        $("#posNamelbl").text(oForm[0]["Name"].value);
        $("#VeriFoneUserId").val(oForm[0]["UserName"].value);
        if (oForm[0]["enbStatus"].value == "True") {
            $('#CreditCardStatus').prop('checked', true);
            $('#CreditCardStatus').bootstrapSwitch('state', true);

            $("#divUserName").show();
            $("#divUserPassword").show();
            $(".scroller").height(height);
            $(".slimScrollDiv").height(height);
            
        }
        else {
           
            height = '100px';
            $('#CreditCardStatus').prop('checked', false);
            $('#CreditCardStatus').bootstrapSwitch('state', false);

            $("#divUserName").hide();
            $("#divUserPassword").hide();
            $(".scroller").height(height);
            $(".slimScrollDiv").height(height);

        }
        clientdelId = oForm[0]["ClientID"].value;
        posdelid = oForm[0]["posid"].value;

        ccStatus = oForm[0]["enbStatus"].value;
        SchoolID = oForm[0]["School_Id"].value;
    });

    $(".savaForm").click(function (e) {
        updatePOSData(this);
    });
    $(".deletepos").click(function (e) {
       // debugger;
        var oForm = $(this).closest(".form-class");
        clientdelId = oForm[0]["ClientID"].value;
        posdelid = oForm[0]["posid"].value;
        sessionStatus = oForm[0]["SessionStatus"].value;
        Name = oForm[0]["Name"].value;

        $("#span-del-title").text(Name);

    });

    $("#Confirmdelete").click(function (e) {
            DeletePOS();
    });

    disableEditLinksTile("UpdatePOS", "POSEditTile", "ActionLink", "You don’t have rights to edit a POS.");

    disableDeleteLinksTile("DeletePOS", "POSEdelTile", "You don’t have rights to delete a POS.");

    $('#deleteModal').on('shown.bs.modal', function () {
        $('#Confirmdelete').addClass('defaultBtnClass');
        $('#btnSave').removeClass('defaultBtnClass');
    });
    $('#deleteModal').on('hidden.bs.modal', function () {
        $('#Confirmdelete').removeClass('defaultBtnClass');
        $('#btnSave').addClass('defaultBtnClass');
    });

});

