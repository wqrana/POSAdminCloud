var controller = '/settings/';
var CreateAction = controller + 'CreatePOS';
var EditAction = controller + 'EditPOS';

$(document).ready(function () {

    $(".savaForm").click(function (e) {
        updatePOSData(this);
    });

    var oPOSTable = $('#POSTable').dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-4 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100, -1],
            [10, 25, 50, 100, "All"] // change per page values here
        ],
        "bProcessing": false,
        "bServerSide": true,
        "sAjaxSource": "/settings/AjaxHandler",
        // set the initial value
        "iDisplayLength": 25,
        "order": [[1, "asc"]],
        "sPaginationType": "bootstrap_full_number",
        "fnServerData": function (sSource, aoData, fnCallback) {

            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json);
                window.scrollTo(0, 0);
            });
        },
        "fnDrawCallback": function (oSettings) {
            disableEditDeleteLinks();
        },

        "oLanguage": {
            //"sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": { "sPrevious": "Prev", "sNext": "Next" },
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ POS.',
            "sInfoEmpty": 'No records.',
            "sEmptyTable": "No records found.",
        },
        "aoColumns": [
            {
                "bSortable": false,
                "sClass": "center",
                "sWidth": "8%",
                "mRender": function (data, type, row) {
                    return '<a onclick="javascript:CreateOrEdit(this);" href="#basic_modalPopup" data-toggle="modal"  title="Edit" data-backdrop="static" data-keyboard="false" class="ActionLink POSEdit " data-id="' + row[0] + '"><i class="fa fa-pencil-square-o fasize"></i></a>' +
                        '<span class="faseparator"> | </span> <a onclick="javascript:Delete(this);" href="#Delete" data-toggle="modal" data-backdrop="static" data-keyboard="false"   title="Delete" class="DeleteLink POSDelete" data-id="' + row[1] + '"><i class="fa fa-trash fasize"></i></a>';
                    //return '<a href="#basic_modalPopup" data-toggle="modal" data-backdrop="static" data-keyboard="false"  class=" settingslnk " data-id="' + row[5] + '">Edit</a>';

                }
            },
            {
                "sName": "Name",
                "bSortable": true,
                "sWidth": "24%",
                "bVisible": true,
                "mRender": function (data, type, row) {
                    return '<div>' + row[2] + '</div>';
                }
            },
                   {
                       "bSortable": true,
                       "sWidth": "24%",
                       "mRender": function (data, type, row) {
                           return '<div>' + row[3] + '</div>';
                       }
                   },
                    {
                        "bSortable": true,
                        "sWidth": "22%",
                        "mRender": function (data, type, row) {
                            if (row[4].trim() == 'Closed')
                                return '<div style="color:red;">' + row[4] + '</div>';
                            else
                                return '<div style="color:green;">' + row[4] + '</div>';
                        }
                    },
                     {
                         "bSortable": true,
                         "bVisible": false,
                         "mRender": function (data, type, row) {
                             return '<div>' + row[5] + '</div>';
                         }
                     },
                    {
                        "bSortable": false,
                        "sWidth": "22%",
                        "sClass": "textCenterCSS",
                        "mRender": function (data, type, row) {
                            if (row[6] == 'True') return "<i class=\"fa fa-check  \" style=\"margin-left:0px;\"></i>"; else return "";
                        }
                    }

        ]
    });

    // modify table per page dropdown
    $('#oPOSTable_wrapper .dataTables_length select').addClass("form-control input-small");


    $('#Delete').on('shown.bs.modal', function () {
        $('#btnDelete').addClass('defaultBtnClass');
        $('#btnSave').removeClass('defaultBtnClass');
    });
    $('#Delete').on('hidden.bs.modal', function () {
        $('#btnDelete').removeClass('defaultBtnClass');
        $('#btnSave').addClass('defaultBtnClass');
    });

});


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
    //debugger;
    if (id != '') {

        $.ajax({
            type: "get",
            url: url,
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                var model = data;

                if (model.IsError) {
                    displayWarningMessage(model.ErrorMessage);
                }
                else {
                    // debugger;
                    var height = '241px';
                    $('#savebtnSpan').text(model.savebtnCaption);
                    $("#posNamelbl").text(model.Name);
                    $("#VeriFoneUserId").val(model.VeriFoneUserId);
                    if (model.enbCCCCProcessing) {
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
                    clientdelId = model.ClientID;
                    posdelid = model.Id;
                    //ccStatus = oForm[0]["enbStatus"].value;
                    SchoolID = model.School_Id;
                }
            },
            error: function () {
                displayWarningMessage(model.LoadErrorMessage);
            }
        });
    }

}
function disableEditDeleteLinks() {

    disableEditLinksTile("UpdatePOS", "POSEdit", "ActionLink", "You don’t have rights to edit a POS.");
    disableDeleteLinksTile("DeletePOS", "POSDelete", "You don’t have rights to delete a POS.");

}