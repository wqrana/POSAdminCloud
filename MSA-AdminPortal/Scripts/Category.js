var controller = '/Category/';
var CreateAction = controller + 'Create';
var EditAction = controller + 'Edit';


$(document).ready(function () {
    $('#CategoryModel_Color_div').colorpicker({ color: '#ff0000' });

    disableCreationRights("CreateCategories", "AddNewButton", "aAddNewButton", "disabled", "ActionLink", "You don’t have rights to create a Category.");
    disableEditDeleteLinks();

    //});

    //$(function () {

    // Document.ready -> link up remove event handler
    $(".ActionLink").click(function () {
        CreateOrEdit(this);
    });

    $("#btnSave").click(function () {

        var prefix = '#CategoryModel_';

        var id = $(prefix + "Id").val();

        var url = '';
        var create = id == '0';
        if (create) {
            url = CreateAction;
        }
        else {
            url = EditAction;
        }

        $.ajax({
            type: "post",
            url: url,
            data: {
                "Id": id,
                "Name": $(prefix + 'Name').val(),
                "CategoryType_Id": $(prefix + 'CategoryType_Id').val(),
                "Color": $(prefix + 'Color').val(),
                "IsActive": $(prefix + 'IsActive').is(':checked'),
            },
            dataType: "json",
            success: function (data) {

                var model = data.CategoryModel;

                if (model.IsError) {
                    displayWarningMessage(model.Message);
                }
                else {
                    // comment if window.location.reload(true); called
                    /*  if (id == '0') {
                          id = model.Id;
                          $("#Id").val(model.Id);

                          // create category box/partial view

                      }
                      else {
                          $("#name-" + model.Id).text(model.Name);
                      }*/

                    if (create) {
                        displaySuccessMessage('The category record has been created successfully.');
                    }
                    else {
                        displaySuccessMessage('The category record has been updated successfully.');
                    }

                    window.location.reload(true);
                }
            },
            error: function () {
                displayWarningMessage(model.Message);
            }
        });
    });

    //////////////////////////////////
    var oCategoryTypeTable = $('#dtGrid').dataTable({
        //"sDom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'i><'col-md-4 col-sm-12'l><'col-md-4 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100, -1],
            [10, 25, 50, 100, "All"] // change per page values here
        ],
        "order": [[1, "asc"]],
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": "/Category/AjaxHandler",
        // set the initial value
        "iDisplayLength": 25,

        "sPaginationType": "bootstrap_full_number",
        "oLanguage": {
            //"sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ Categories",
            "oPaginate": { "sPrevious": "Prev", "sNext": "Next" },
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Categories.',
            "sInfoEmpty": 'No records.',
            "sEmptyTable": "No records found.",
        },

        "fnDrawCallback": function (oSettings) {
            disableEditDeleteLinks();
        },

        "aoColumns": [
                        {
                            "bSortable": false,
                            "sClass": "center",
                            "sWidth": "8%",
                            "mRender" : function (data, type, row) {
                                return '<a title="Edit" onclick="javascript:CreateOrEdit(this);" href="#CreateOrEdit " data-backdrop="static" data-keyboard="false" data-toggle="modal" class="ActionLink EditSecurityClass" data-id="' + row[0] + '"><i class="fa fa-pencil-square-o fasize"></i></a>' +
                                ' <span class="faseparator"> | </span> <a  title="Delete" onclick="javascript:Delete(this);" href="#Delete" data-toggle="modal" data-backdrop="static" data-keyboard="false"  class="DeleteLink DeleteSecurityClass " data-id="' + row[1] + '"><i class="fa fa-trash fasize"></i></a>';
                            }
                        },
                    {
                        "bSortable": true,
                        "mRender" : function (data, type, row) {
                            return '<div style="background-color: ' + row[2] + ';width:16px;height:16px;float:left;margin-right:10px">&nbsp;</div><div>' + row[3] + '<div>';
                        }
                    },
                    {
                        "bSortable": true,
                        "mRender" : function (data, type, row) {
                            return '<div>' + row[4] + '<div>';
                        }
                    },
                    {
                        "bSortable": true,
                        "sClass": "center",
                        "mRender" : function (data, type, row) {
                            return '<a href="/Menu/GetItemsByCategory?id=' + row[6] + '"  class=\"ViewSecurityMenuClass\" >' + row[5] + '</a>';
                        }
                    }
        ]
    });

    $('#dtGrid_wrapper .dataTables_length select').addClass("form-control input-small"); // modify table per page dropdown
    /////////////////////////////////


    $('#Delete').on('shown.bs.modal', function () {
        $('#btnDelete').addClass('defaultBtnClass');
        $('#btnSave').removeClass('defaultBtnClass');
    });
    $('#Delete').on('hidden.bs.modal', function () {
        $('#btnDelete').removeClass('defaultBtnClass');
        $('#btnSave').removeClass('defaultBtnClass');
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

    if (id != '') {

        $.ajax({
            type: "get",
            url: url,
            data: { "id": id },
            dataType: "json",
            success: function (data) {

                var model = data.CategoryModel;

                if (model.IsError) {
                    displayWarningMessage(model.ErrorMessage);
                }
                else {

                    $('#savebtnSpan').text(model.savebtnCaption);
                    var prefix = '#CategoryModel_';

                    $(prefix + 'Title').text(model.Title);
                    $(prefix + 'Id').val(model.Id);
                    $(prefix + 'Name').val(model.Name);
                    $(prefix + 'CategoryType_Id').val(model.CategoryType_Id);

                    $(prefix + 'Color').val(model.HexaColor);
                    $(prefix + 'Color_div').prop('data-color', model.HexaColor);
                    $(prefix + 'Color_i').prop('style', 'background-color: ' + model.HexaColor);

                    $(prefix + 'Color_div').colorpicker('setValue', model.HexaColor);

                    $('#uniform-CategoryModel_IsActive span').prop('class', model.IsActive ? 'checked' : '');
                    $(prefix + 'IsActive').prop('checked', model.IsActive ? 'checked' : '');
                }
            },
            error: function () {
                displayWarningMessage(model.LoadErrorMessage);
            }
        });
    }

}

function disableEditDeleteLinks() {

    disableEditLinksTile("UpdateCategories", "EditSecurityClass", "ActionLink", "You don’t have rights to edit a Category.");
    disableDeleteLinksTile("DeleteCategories", "DeleteSecurityClass", "You don’t have rights to delete a Category.");
    disableLink("viewMenu", "ViewSecurityMenuClass", "You don’t have rights to view a Menu item.");
}