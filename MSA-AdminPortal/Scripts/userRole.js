
$(document).ready(function () {

    var oTableUserRole = $('#dtUserRoleTable').DataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-4 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100],
            [10, 25, 50, 100] // change per page values here
        ],
        "order": [[1, "asc"]],
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": "/Security/AjaxHandler",
        // set the initial value
        "iDisplayLength": 10,
        "sPaginationType": "bootstrap_full_number",
        "fnDrawCallback": function (oSettings) {
            disableEditDeleteLinks();
        },
        "oLanguage": {
            //"sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": { "sPrevious": "Prev", "sNext": "Next" },
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
            "sInfoEmpty": 'No records',
            "sEmptyTable": "No records found",
        },
        "aoColumns": [
            {
                "bSortable": false,
                "sWidth": "10%",
                "sClass": "center",
                "mRender": function (data, type, row) {
                    if (row[5] == "0") {
                        return '<a title="Edit" href="/security/UserRoles?id=' + row[0] + '" role=\"button\" class=\"EditSecurityClass\" ><i class="fa fa-pencil-square-o fasize"></i></a>'
                                + ' <span class="faseparator"> | </span> '
                                + ' <a  title="Delete" href=\"#Delete\"' + ' onclick=Delete(this); role=\"button\" data-backdrop=\"static\" data-keyboard=\"false\" class="DeleteLink DeleteSecurityClass " data-id="' + row[1] + '" data-toggle=\"modal\"><i class=\"fa fa-trash fasize\"></i></a>'
                                + ' <span class="faseparator"> | </span>'
                                + '<a  title="click for details" href=\"#UserRolePopup\"' + ' onclick=ShowUserRoleDetails(this); role=\"button\" data-backdrop=\"static\" data-keyboard=\"false\" class="aa"  data-id="' + row[1] + '" data-toggle=\"modal\"><i class=\"fa fa-align-justify\"></i></a>';

                                
                                
                    }
                    else {
                        return '<a title="Edit" href="/security/UserRoles?id=' + row[0] + '" role=\"button\" class=\"EditSecurityClass\" ><i class="fa fa-pencil-square-o fasize"></i></a>'
                                + ' <span class="faseparator"> | </span> '
                                + '<a  title="Delete" href=\"#userattached\"' + ' onclick=setValue(' + row[5] + '); role=\"button\" data-backdrop=\"static\" data-keyboard=\"false\" class="DeleteLink DeleteSecurityClass " data-id="' + row[1] + '" data-toggle=\"modal\"><i class=\"fa fa-trash fasize\"></i></a>'
                                + ' <span class="faseparator"> | </span>'
                                + '<a  title="click for details" href=\"#UserRolePopup\"' + ' onclick=ShowUserRoleDetails(this); role=\"button\" data-backdrop=\"static\" data-keyboard=\"false\" class="aa"  data-id="' + row[1] + '" data-toggle=\"modal\"><i class=\"fa fa-align-justify\"></i></a>';


                    }
                }
            },
            {
                "bSortable": true,
                "sWidth": "30%",
                "bVisible": true,
                "mRender": function (data, type, row) {
                    return '<div>' + row[3] + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "30%",
                "mRender": function (data, type, row) {
                    return '<div>' + row[2] + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "30%",
                "mRender": function (data, type, row) {
                    if (row[5] == "0") {
                        return '<div class="zeroUsersCSS">' + row[5] + '</div>';
                    } else {
                        return '<div><a  href="/security/users?id=' + row[0] + '">' + row[5] + '</a></div>';

                    }
                }
            },
        ]
    });

    $('#UserRolePopup').on('shown.bs.modal', function () {
        $('#btnCloseUserRoleTable').addClass('defaultBtnClass');
    });
    $('#UserRolePopup').on('hidden.bs.modal', function () {
        $('#permissionDiv').html('');
        $('#btnCloseUserRoleTable').removeClass('defaultBtnClass');
    });

    Delete

    $('#Delete').on('shown.bs.modal', function () {
        $('#btnDelete').addClass('defaultBtnClass');
    });
    $('#Delete').on('hidden.bs.modal', function () {
        $('#btnDelete').removeClass('defaultBtnClass');
    });

    $('#userattached').on('shown.bs.modal', function () {
        $('#btnCloseUserRole').addClass('defaultBtnClass');
    });
    $('#userattached').on('hidden.bs.modal', function () {
        $('#btnCloseUserRole').removeClass('defaultBtnClass');
    });
    // modify table per page dropdown
    $('#dtUserRoleTable_wrapper .dataTables_length select').addClass("form-control input-small");

    ////////
    disableCreationRights("CreateUserRoles", "AddNewButton", "aAddNewButton", "disabled", "ActionLink", "You don’t have rights to create a user role.");

});

function disableEditDeleteLinks() {

    disableEditLinksTile("UpdateUserRoles", "EditSecurityClass", "ActionLink", "You don’t have rights to edit a user role.");
    disableDeleteLinksTile("DeleteUserRoles", "DeleteSecurityClass", "You don’t have rights to delete a user role.");

}

function setValue(val)
{
    $("#spanUserscount").text(val);

}

function ShowUserRoleDetails(obj) {
    //debugger;
    var userRole_id = -1;
    if (obj) {
        userRole_id = $(obj).attr("data-id");
    }

    var dataString = JSON.stringify({
        allData: userRole_id
    });


    $.ajax({
        type: "POST",
        url: "/Security/getModuleAndRights",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#permissionDiv").children().remove();
            $("#permissionDiv").children('img').remove();
            $("#permissionDiv").append("<img src=\"../Content/themes/assets/img/input-spinner.gif\" />");
        },
        success: function (data) {
            if (data.result == '-1') {
                $("#permissionDiv").html("User role detail could not be found.");
                //displaySuccessMessage("No permissions are set for this module.");
            }
            else {
                //debugger;
                $("#permissionDiv").children().remove();
                var arr_from_json = JSON.parse(data.result);
                $("#permissionDiv").append("<table>");
                for (var key in arr_from_json) {
                    var value = arr_from_json[key];
                    $("#permissionDiv").append("<tr><td class='firstCol'>" + key + "</td><td>" + value + " </td></tr>");
                    //$("#permissionDiv").append(" " + value + "");
                }
                $("#permissionDiv").append("</table>");
                //$.each(data.result, function (index, item) {
                //    var temp = item;
                //    $("#permissionDiv").append("<label> " + item.ActionID + item.DisplayActionText + "   </label>");
                //    //alert(item.ID);
                //});
            }
        },
        error: function (request, status, error) {
            displayErrorMessage("Error occurred during getting permissions data.");
            return false;
        }
    });
}

$('#Delete').on('shown.bs.modal', function (e) {
    $('#btnDelete').addClass('defaultBtnClass');
})

$('#Delete').on('hidden.bs.modal', function () {
    $('#btnDelete').removeClass('defaultBtnClass');
});


