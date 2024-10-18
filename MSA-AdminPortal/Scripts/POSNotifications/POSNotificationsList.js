var oTable;
$(document).ready(function () {


    InitializeSearch();

    (function ($) {
        $.fn.focusTextToEnd = function () {
            var $initialVal = this.val();
            this.val($initialVal);
        };
    })(jQuery);

    oTable = $('#POSNotificationsList').dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-4 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100],
            [10, 25, 50, 100] // change per page values here
        ],
        "bProcessing": true,
        "order": [[1, "asc"]],

        "iDisplayLength": 10,
        "sPaginationType": "bootstrap_full_number",
        "oLanguage": {
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
            "sInfoEmpty": 'No records found',
            "sEmptyTable": "No records found",
            //"sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": {
                "sPrevious": "Prev",
                "sNext": "Next"
            }
        },
        "fnDrawCallback": function () {
            //$('.dataTables_scrollBody').scrollTop(0);

            // $('body').scrollTop(0);
            window.scrollTo(0, 0);

            $('#POSNotificationsList tbody').find('div#AssignedSchoolDiv')
           .each(function () {
               //debugger;
               $(this).removeAttr('hidden');

           });
            
            disableEditDeleteLinks();

        },

        "aoColumns": [
            {
                "bSearchable": false,
                "sWidth": "10%",
                "bSortable": false,
                "sClass": "center",
                "mRender": function (data, type, row) {
                    // debugger;
                    var innerHML = row[0];
                    var id = $(innerHML).text();

                    var string = "<a  title=\"Edit\" class=\"POSNotificationsEditTile\" onclick=\"javascript:CreateOrEdit(this);\" href=\"#basic_modalPopup\" data-toggle=\"modal\" data-backdrop=\"static\" data-keyboard=\"false\" class=\"DeleteLink\" data-id=\"" + id + "\"><i class=\"fa fa-pencil-square-o fasize\"></i></a>";
                    string += " <span class=\"faseparator\"> | </span> ";
                    string += " <a  title=\"Delete\" class=\"POSNotificationsDeleteTile\" onclick=\"javascript:Delete(this);\" href=\"#Delete\" data-toggle=\"modal\" data-backdrop=\"static\" data-keyboard=\"false\" class=\"DeleteLink\" data-id=\"" + id + "\"><i class=\"fa fa-trash fasize\"></i></a>";
                    string += " <span class=\"faseparator\"> | </span> ";
                    string += "<a  title=\"Add student to POS Notification \" href=\"#\"  onclick=\"javascript:OpenCustomerSearchPopUp('" + id + "','" + row[6] + "')\" data-toggle=\"modal\" data-backdrop=\"static\" data-keyboard=\"false\"><span class=\"fa-stack fa-size\" style=\"font-size: 9px;\"><i class=\"fa fa-bell fa-stack-2x\" style=\"font-size: 17px;\"></i><i class=\"fa fa-plus fa-stack-1x fa-inverse\" style=\"font-size: 9px;\"></i></span></a>";

                    return string;
                }
            },
            {
                "bSortable": true,
                "sWidth": "25%",
                "sClass": "floattop",
                "visible": true
            },
            {
                "bSortable": true,
                "sClass": "floattop",
                "sWidth": "15%",
                "visible": true
            },
            {
                "bSortable": true,
                "sClass": "floattop",
                "sWidth": "50%",
                "mRender": function (data, type, row) {
                    var style = "color:" + row[4] + ";background-color:" + row[5];
                    return "<div style=\"" + style + "\">" + data + "<div>";
                }
            },
            {
                "bSortable": false,
                "sClass": "floattop",
                "sWidth": "50%",
                "visible": false
            },
            {
                "bSortable": false,
                "sClass": "floattop",
                "sWidth": "50%",
                "visible": false
            },
            {
                "bSortable": false,
                "sClass": "floattop",
                "sWidth": "50%",
                "visible": false
            }
        ]


    });

    $('#popupokBtn').click(function () {
        var selectedCustomers = $('#hdnFldSelectedValues').val();
        var posNotificationId = $('#hdnPOSNotificationId').val();

        if (posNotificationId != "") {


            if (selectedCustomers == null) {
                selectedCustomers = '';
            }
            else {
                selectedCustomers = selectedCustomers.split("|").toString();
            }

            var dataString = JSON.stringify({
                allData: posNotificationId + '*' + selectedCustomers
            });

            //alert(dataString);

            $.ajax({
                type: "POST",
                url: "/POSNotifications/AddNotificationToCustomer",
                data: dataString,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.result == '-1') {

                        displaySuccessMessageOnce("Changes have been saved successfully.");
                        window.location.reload(true);
                    }
                },
                error: function (request, status, error) {
                    displayErrorMessage("Error occurred during saving the data.");
                    return false;
                }
            });
        }
    });


});

var controller = '/POSNotifications/';
var CreateAction = controller + 'CreatePOSNotification';
var EditAction = controller + 'EditPOSNotification';

function CreateOrEdit(e) {
    // Get the id from the link
    var id = $(e).attr("data-id");

    var url = '';
    if (id == '0') {
        url = CreateAction;
    }
    else {
        url = EditAction;
    }

    if (id != '') {

        $.ajax({
            type: "get",
            url: url,
            data: { "id": id },
            dataType: "json",
            success: function (data) {

                var model = data;

                if (model.IsError) {
                    displayWarningMessage(model.ErrorMessage);
                }
                else {
                    var height = '441px';
                    $('#savebtnSpan').text(model.savebtnCaption);
                    $("#posNamelbl").text(model.Title);
                    clientdelId = model.DistrictID;
                    $("#posNotificationName").val(model.Name);
                    $("#posNotificationCode").val(model.Code);
                    $("#posNotificationTextColor").val(model.TextColor);
                    $("#posNotificationBackColor").val(model.BackColor);
                    $("#posNotificationTextColorH").val(model.TextColor);
                    $("#posNotificationBackColorH").val(model.BackColor);
                    $("#posNotificationDescr").val(model.Description);

                    $("#posNotificationID").val(model.Id);
                }

                $("#posNotificationTextColorH").val($("#posNotificationTextColorH").val() == "" ? "#ffffff" : $("#posNotificationTextColorH").val());
                $("#posNotificationBackColorH").val($("#posNotificationBackColorH").val() == "" ? "#000000" : $("#posNotificationBackColorH").val());

                $("#posNotificationTextColor").spectrum({
                    color: $("#posNotificationTextColorH").val() == "" ? "#ffffff" : $("#posNotificationTextColorH").val(),
                    move: function (color) {
                        //console.log("change called: " + color.toHexString());
                        $('#posNotificationTextColorH').val(color.toHexString());
                        $("#posNotificationDescr").css({ 'color': color.toHexString(), 'background-color': $("#posNotificationBackColorH").val() == "" ? "#000000" : $("#posNotificationBackColorH").val() });
                    },
                    show: function () {
                        $('.sp-choose').hide();
                        $('.sp-cancel').hide();
                    },
                });
                $("#posNotificationBackColor").spectrum({
                    color: $("#posNotificationBackColorH").val() == "" ? "#000000" : $("#posNotificationBackColorH").val(),
                    move: function (color) {
                        //console.log("change called: " + color.toHexString());
                        $('#posNotificationBackColorH').val(color.toHexString());
                        $("#posNotificationDescr").css({ 'color': $("#posNotificationTextColorH").val() == "" ? "#ffffff" : $("#posNotificationTextColorH").val(), 'background-color': color.toHexString() });
                    },
                    show: function () {
                        $('.sp-choose').hide();
                        $('.sp-cancel').hide();
                    },
                });

                $("#posNotificationDescr").css({ 'color': $("#posNotificationTextColorH").val() == "" ? "#ffffff" : $("#posNotificationTextColorH").val(), 'background-color': $("#posNotificationBackColorH").val() == "" ? "#000000" : $("#posNotificationBackColorH").val() });
            },
            error: function () {
                displayWarningMessage(model.LoadErrorMessage);
            }
        });
    }

}

$(".savaForm").click(function () {


    var id = $("#posNotificationID").val();
    var distID = $("#distID").val();
    var name = $("#posNotificationName").val();
    var code = $("#posNotificationCode").val();
    var textColor = $("#posNotificationTextColorH").val();
    var backColor = $("#posNotificationBackColorH").val();
    var descr = $("#posNotificationDescr").val();

    var url = '';
    var create = id == '0';
    if (create) {
        url = CreateAction;
    }
    else {
        url = EditAction;
    }

    
    if (name.trim() == "") {
        displayWarningMessage("Please enter POS Notification name.");
        return false;
    }
    else if (name.replace(/[^\w\s]/gi, "").length != name.length && name.replace(/[^\w\s]/gi, "").length == 0) {
        displayWarningMessage("Please enter a valid POS Notification name.");
        return false;
    }
    else if (code.trim() == "") {
        displayWarningMessage("Please enter POS Notification code.");
        return false;
    }
    else if (code.replace(/[^\w\s]/gi, "").length != code.length && code.replace(/[^\w\s]/gi, "").length == 0) {
        displayWarningMessage("Please enter a valid POS Notification code.");
        return false;
    }
    else if (descr.trim() == "") {
        displayWarningMessage("Please enter POS Notification description.");
        return false;
    }
    else if (descr.replace(/[^\w\s]/gi, "").length != descr.length && descr.replace(/[^\w\s]/gi, "").length == 0) {
        displayWarningMessage("Please enter a valid POS Notification description.");
        return false;
    }
    else {

        $.ajax({
            type: "post",
            url: url,
            data: {
                "Id": id,
                "Name": name,
                "ClientId": distID,
                "Code": code,
                "TextColor": textColor,
                "BackColor": backColor,
                "Description": descr
            },
            dataType: "json",
            success: function (data) {

                var model = data;

                if (model.IsError) {
                    displayWarningMessage(model.ErrorMessage);
                }
                else {

                    if (create) {

                        displaySuccessMessage('POS Notification created successfully.');
                        window.location.reload(false);

                    }
                    else {
                        displaySuccessMessage('POS Notification updated successfully.');
                        window.location.reload(false);
                    }
                    //}
                    //window.location.reload(true);
                    //oTable.fnDraw();
                }
            },
            error: function () {
                displayWarningMessage(model.ErrorMessage);
            }
        });
    }
});

function OpenCustomerSearchPopUp(POSNotificationId, sSelectedCustomers) {
    //resetUrl();
    //InitializeSearch();
    //alert(sSelectedCustomers);
    $('#hdnPOSNotificationId').val(POSNotificationId);
    $('#hdnFldSelectedValues').val(sSelectedCustomers);
    $('#CustomerSearcRptmodalcontent').css({ 'width': '1200px', 'margin-left': '-282px' });
    $('#CustomerSearcRpt').modal('show');
    //$('#specificCustomer').trigger('click');
}

function resetUrl() {
    window.history.pushState("", "", "/POSNotifications/Index");
}

function disableEditDeleteLinks() {

    ///////////////////////////////////////////////////////////////////////////////////////////////
    //Security handler
    ///////////////////////////////////////////////////////////////////////////////////////////////
    disableCreationRights("CreatePOSNotifications", "btnAddNewPOSNotifications", "aAddNewPOSNotifications", "disabled", "ActionLink", "You don’t have rights to create a POS Notifications.");

    disableEditLinksTile("UpdatePOSNotifications", "POSNotificationsEditTile", "ActionLink", "You don’t have rights to edit a POS Notifications.");


    disableDeleteLinksTile("DeletePOSNotifications", "POSNotificationsDeleteTile", "You don’t have rights to delete a POS Notifications.")

    ///////////////////////////////////////////////////////////////////////////////////////////////

}
$('#basic_modalPopup').on('hidden.bs.modal', function () {
    // do something…
    //window.location.reload();
});

$('#basic_modalPopup').on('shown.bs.modal', function (e) {
    $('#btnSavePOSNOtifications').addClass('defaultBtnClass');
    $('#posNotificationName').focus();
})

$('#basic_modalPopup').on('hidden.bs.modal', function () {
    $('#btnSavePOSNOtifications').removeClass('defaultBtnClass');
});

//due to bug#2015
//$('#Delete').on('shown.bs.modal', function (e) {
//    $('#btnDelete').addClass('defaultBtnClass');
//})

//$('#Delete').on('hidden.bs.modal', function () {
//    $('#btnDelete').removeClass('defaultBtnClass');
//});