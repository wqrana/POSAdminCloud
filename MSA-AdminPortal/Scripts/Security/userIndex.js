var oUserTable;
var userTable;
var UserSortCol = 1;
var UserSortDir = "asc";






$(document).ready(function () {
    var oldStart = 0;
    $('#SearchStr').focus();
    //oUserTable = $('#UserTable').DataTable(
    //    {
    //        "oLanguage": {
    //            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
    //            "sInfoEmpty": 'No records',
    //            "sEmptyTable": "No records found",
    //            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
    //            "sLengthMenu": "_MENU_ records",
    //            "oPaginate": {
    //                "sPrevious": "Prev",
    //                "sNext": "Next"
    //            }
    //        },
    //        "aLengthMenu": [
    //            [10, 25, 50, 100],
    //            [10, 25, 50, 100] // change per page values here
    //        ],
    //        "sPaginationType": "bootstrap_full_number",
    //        "searching": true,
    //        "iDisplayLength": 10,
    //        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-4 col-sm-12'p>>",
    //        "order": [[1, "asc"]],
    //        "fnDrawCallback": function (oSettings) {
    //            oldStart = oSettings._iDisplayStart;
    //            $('html, body').animate({
    //                scrollTop: 0
    //            }, 100);
    //            disableEditDeleteLinks();
    //        },
    //        "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
    //    }
    //);


    // from here for UserTable2
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //

    userTable = $('#UserTable2').dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-5 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100],
            [10, 25, 50, 100] // change per page values here
        ],

        "ajax": "data.json",
        "bProcessing": true,
        "bServerSide": true,
        //"paging": true,
        //"bDefaultContent": "-",
        "order": [[UserSortCol, UserSortDir]],
        "sAjaxSource": "/Security/AjaxHandlerUser",
        "iDisplayLength": 10,
        "bPaginate": true,
        "fnInitComplete": function (oSettings, json) {
        },

        "sPaginationType": "bootstrap_full_number",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push(
                    { "name": "SearchBy", "value": $("#SearchStr").val() },
                    { "name": "UserRoleId", "value": $("#dlluserRoles2").val() },
                    { "name": "ActiveStr", "value": $("#activedll").val() },
                    { "name": "PrimaryStr", "value": $("#primarydll").val() },
                    {
                        "name": "radomNumber", "value": Math.random()
                    }
                );
            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json);
                jQuery("#SearchStr").focus();
            });
        },
        "fnDrawCallback": function () {
            //restorecheckBoxes(this);
            disableEditDeleteLinks();
        },
        "oLanguage": {
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
            "sInfoEmpty": 'No records',
            "sEmptyTable": "No records found",
            //"sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": {
                "sPrevious": "Prev",
                "sNext": "Next"
            }
        },
        "aoColumns": [
                      //action column
                    {
                        "sName": "UserId",
                        "sWidth": "3%",
                        "sClass": "leftClass",
                        "bSearchable": false,
                        "bSortable": false,
                        "mRender": function (data, type, row) {
                           // var htmlActionColumn = '' + '<span class="faseparator">| </span><a title="Delete" href="#" onclick="return showDeleteModal(' + row[6] + ',\'' + row[2] + '\'); " data-toggle="modal" data-keyboard="false" data-backdrop="static" class="deleteUser DeleteSecurityClass"><i class="fa fa-trash  fasize"></i></a>';
                            var htmlActionColumn = '' + '<span class="faseparator">| </span><a onclick="javascript:Delete(this);" href="#Delete" data-toggle="modal" data-backdrop="static" data-keyboard="false"  class="DeleteLink DeleteSecurityClass" title="Delete" data-id="' + row[6] + '"><i class="fa fa-trash fasize"></i></a>';
                            if (row[4] == "false")
                                htmlActionColumn = '<i class="fa fa-pencil-square-o fasize" style:"color:gray;"></i>' + htmlActionColumn + '<span class="faseparator">|' + '<a title="Activate/Deactivate" onclick="javascript:Activate(this);return false;" href="#" data-toggle="modal" data-backdrop="static" data-keyboard="false" class="ActivateLink disableSecurityClass" data-id="'+row[6]+'"><i title="Activate" class="fa fa-user-plus fasize faActiveColr "></i></a>';
                            else
                                htmlActionColumn = '<a class="EditSecurityClass" title="Edit" onclick="OpenModalPopupForEdit(' + row[6] + '); " data-toggle="modal" data-keyboard="false" data-backdrop="static"><i class="fa fa-pencil-square-o fasize" ></i></a>' + htmlActionColumn + '<span class="faseparator">|' + '<a title="Activate/Deactivate" onclick="javascript:Activate(this);" href="#Activate" data-toggle="modal" data-backdrop="static" data-keyboard="false" class="ActivateLink disableSecurityClass" data-id="'+row[6]+'"><i title="Deactivate" class="fa fa-user-times fasize fadeactiveColr"></i></a>';

                            return htmlActionColumn;
                        }
                    },
                    //Login Name
                    {
                        "sName": "LoginName",
                        "sWidth": "9%",
                        "bSearchable": false,
                        "bSortable": true,
                        "mRender": function (data, type, row) {
                            //return '<a href=\"#CustomerModal\"  class=\"EditSecurityClass \" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + row[2] + '</a>';
                            var htmlLoginName = '<div><a class="EditSecurityClass" title="Edit" onclick="OpenModalPopupForEdit(' + row[6] + ')" data-toggle="modal" data-keyboard="false" data-backdrop="static">' + row[2] + '</a></div>';

                            if (row[4] == "false")
                                htmlLoginName = '<div>' + row[2] + '</div>';
                            else
                                htmlLoginName = '<div><a class="EditSecurityClass" title="Edit" onclick="OpenModalPopupForEdit(' + row[6] + ')" data-toggle="modal" data-keyboard="false" data-backdrop="static">' + row[2] + '</a></div>';

                            return htmlLoginName
                        }
                    },
                    //User Name
                    {
                        
                        "sWidth": "13%",
                        "mRender": function (data, type, row) {
                            //return '<a href=\"#CustomerModal\"    class=\"EditSecurityClass\" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + row[3] + '</a>';
                            var htmlUserName = '<div><a class="EditSecurityClass" title="Edit" onclick="OpenModalPopupForEdit(' + row[6] + ')" data-toggle="modal" data-keyboard="false"  data-backdrop="static">' + row[1] + '</a></div>';

                            if (row[4] == "false")
                                htmlUserName = '<div>' + row[1] + '</div>';
                            else
                                htmlUserName = '<div><a class="EditSecurityClass" title="Edit" onclick="OpenModalPopupForEdit(' + row[6] + ')" data-toggle="modal" data-keyboard="false"  data-backdrop="static">' + row[1] + '</a></div>';

                            return htmlUserName;

                        }

                    },
                    //User Role Name
                    {
                        
                        "sWidth": "13%",
                        "bSortable": true,
                        "mRender": function (data, type, row) {
                            return row[3];
                        }

                    },
                    //Is Primary
                    {
                        "sWidth": "6%",
                        "sClass": "centerClass",
                        "mRender": function (data, type, row) {
                            if (row[5] == "false")
                                return "<div><i class=\"fa fa-times\" style=\"margin-left:0px;\"></i></div>";

                            else
                                return "<div><i class=\"fa fa-check\" style=\"margin-left:0px;\"></i></div>";
                        }

                    },
                    //Is Active
                    {
                        "sWidth": "6%",
                        "sClass": "centerClass",
                        "mRender": function (data, type, row) {
                            if (row[4] == "false")
                                return "<div><i class=\"fa fa-times\" style=\"margin-left:0px;\"></i></div>";
                            else
                                return "<div><i class=\"fa fa-check\" style=\"margin-left:0px;\"></i></div>";

                        }

                    }

        ]
    });

    $('#UserTable2_wrapper .dataTables_filter').hide(); //.addClass("form-control input-medium"); // modify table search input
    $('#UserTable2_wrapper .dataTables_length select').addClass("form-control input-small"); // modify table per page dropdown

    $("#hidefilter").text("Show Filters");

    $("#dlluserRoles2").select2();
    $("#primarydll").select2();
    $("#activedll").select2();

    $("#hidefilter").click(function () {

        toggleTextUser(this.id);
        /* commented by Waqar Q.
        ClearUserFilters();
        setTimeout(foo, 3000);
        oCustomerTable.fnDraw();
        */
    });

    $("#Clearfilter").click(function (e) {
        ClearUserFilter();
        setTimeout(foo, 3000);
        userTable.fnDraw();
    });

    $("#applyFilterBtnUser").click(function (e) {
        //ApplyCustomerFilters();
        //setTimeout(foo, 3000);

        //Save filters in session;
        // saveFilters();
        userTable.fnDraw();
    });


    //$("#SearchBtn").click(function (e) {
    //    //SearchCustomer();
    //    if (filtersSelected()) {
    //        //Save filters in session;
    //        // saveFilters();
    //        userTable.fnDraw();
    //    } else {
    //        displayWarningMessage('Please choose a "Search by..." value before trying to use the search functionality.');
    //    }
    //});


    $('#dtGrid_wrapper .dataTables_length select').addClass("form-control input-small");

    //Commented to use delete.js functionalilty
    /*
    $(".Confirmdelete").click(function (e) {
        DeleteUser();
    });
    */
    //
    disableCreationRights("CreateUsers", "AddNewButton", "aAddNewButton", "disabled", "ActionLink", "You don’t have rights to create a user.");

    $("#SearchBtn").click(function (e) {
        userTable.fnDraw();
    });

    $('#SearchStr').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            userTable.fnDraw();
        }
    });
  //  $('#SearchStr').focus();

    //$('#SearchStr').focusout(function () {
    //    $('#SearchStr').focus();
    //});


    });

function foo() {
    //some delay
}

function ClearUserFilter() {
    $("#SearchStr").val("");


    $("#dlluserRoles2").val("");
    $("#dlluserRoles2").select2();

    $("#primarydll").val("");
    $("#primarydll").select2();

    $("#activedll").val("");
    $("#activedll").select2();

}

function toggleTextUser(button_id) {
    var el = document.getElementById(button_id);
    if (el.firstChild.data == "Hide Filters") {
        el.firstChild.data = "Show Filters";
        HideFilterDivUser();

    }
    else {
        el.firstChild.data = "Hide Filters";
        ShowFilterDivUser();
    }
}

function HideFilterDivUser() {
    $('#filterDivUser').slideUp();
    var el = document.getElementById('hidefilter');
    el.firstChild.data = "Show Filters";
    return false;
}

function ShowFilterDivUser() {
    $('#filterDivUser').slideDown();
    var el = document.getElementById('hidefilter');
    el.firstChild.data = "Hide Filters";
    return false;
}

function DeleteUser(customerId) {
    var userdataString = JSON.stringify({
        allData: customerId
    });
    $.ajax({
        type: "POST",
        url: "/Security/DeleteUser",
        data: userdataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.result == '-1') {
                displaySuccessMessage("user deleted successfully");
                window.location.reload(true);
            }
            else if (data.result == '-5') {
                displayWarningMessage("This user can’t be deleted because it has some associated orders.");
            }
            else {
                alert(data.result);
            }
        },
        error: function (request, status, error) {
            displayErrorMessage("Error occurred during deleting the user.");

            return false;
        }
    });
}

function OpenModalPopupForEdit(id) {

    var controller = '/Security/';
    var CreateAction = controller + 'CreateUser';
    var EditAction = controller + 'EditUser';
    var SaveAction = controller + 'SaveUser';

    //var id = $(this).attr("data-id");


    if (id == '0') {
        document.getElementById('specificCustomer').innerHTML = "Select User";
    }
    else {
        document.getElementById('specificCustomer').innerHTML = "";
    }

    var url = '';
    if (id == '0') {
        url = CreateAction;
        $('#newUser').val(true);
    }
    else {
        url = EditAction;
        $('#newUser').val(false);
    }

    if (id != '') {
        $('#loading-image').show();
        $.ajax({
            type: "get",
            url: url,
            data: { "id": id },
            dataType: "json",
            success: function (data) {

                var model = data;

                $('#savebtnSpan').text(model.savebtnCaption);

                if (id == '0') {
                    $('#UserDetails').html('Add New User');
                }
                else
                {
                    $('#UserDetails').html('Edit User : ' + model.UserName);
                }

                $('#UserNameEdit').val(model.UserName);
                $('#LoginNameEdit').val(model.LoginName);
                $('#Password').val(model.Password);
                $('#ConfirmPassword').val(model.Password);
                $('#dllSecurity').val(model.SecurityGroupId);
                $('#CustomerId').val(model.EmployeeId);
                $('#dlluserRoles').val(model.UserRolesID);
                $('#dlluserRoles').select2();
                $("#basic_modalPopup").modal('show');
            },
            complete: function () {
                $('#loading-image').hide();
            },
            error: function () {
                displayErrorMessage("Error loading data.");
            }
        });
    }
}

function OpenModalPopup() {
    $('#newUser').val(true);
    $("#hdnFldSelectedValues").val('');
    $('#CustomerName').html("");
    $('#UserDetails').html('Add New User');
    $('#loading-image').hide();

    //$('#UserNameEdit').val('');
    //$('#LoginNameEdit').val('');
    //$('#Password').val('');
    //$('#ConfirmPassword').val('');
    //$('#dllSecurity').val('');
    //$('#CustomerId').val('');
    //$('#dlluserRoles').val('');

    //$('#savebtnSpan').text('Save');
    OpenModalPopupForEdit('0');
    //$("#basic_modalPopup").modal('show');
}



function disableEditDeleteLinks() {
    disableEditLinksTile("UpdateUsers", "EditSecurityClass", "ActionLink", "You don’t have rights to edit a user.");
    disableDeleteLinksTile("DeleteUsers", "DeleteSecurityClass", "You don’t have rights to delete a user.");
}

function showDeleteModal(customerId, name) {

    $('#Confirmdelete').attr('onclick', 'DeleteUser(' + customerId + ')');

    $('#userName').html(' ' + name)

    $("#deleteModal").on('shown.bs.modal', function (e) {
        $('#Confirmdelete').addClass('defaultBtnClass');
        $('#btnSave').removeClass('defaultBtnClass');
    }).on('hidden.bs.modal', function () {
        $('#Confirmdelete').removeClass('defaultBtnClass');
        $('#btnSave').removeClass('defaultBtnClass');
    }).modal('show');
}