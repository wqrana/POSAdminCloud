var controller = '/CategoryType/';
var CreateAction = controller + 'Create';
var EditAction = controller + 'Edit';



$(document).ready(function () {
  

    // Document.ready -> link up remove event handler
    disableCreationRights("CreateCategoryTypes", "AddNewButton", "aAddNewButton", "disabled", "ActionLink", "You don’t have rights to create a Category Type.");

    //disableLink("viewCategories", "ViewSecurityCategoryClass", "You don’t have rights to view a Category.");
    //disableLink("viewMenu", "ViewSecurityMenuClass", "You don’t have rights to view a Menu item.");

    disableEditDeleteLinks();

    $(".ActionLink").click(function () {
        CreateOrEdit(this);
    });
    /*
    $("input[type=checkbox]").change(function (e) {
        debugger;
        if (this.id == "CanReduce" || this.id == "CanFree") {
            if (this.checked) {
                $('#uniform-IsMealPlan span').prop('class', '');
                $('#IsMealPlan').prop('checked', '');

                $('#uniform-IsMealEquiv span').prop('class', '');
                $('#IsMealEquiv').prop('checked', '');
            }
        }
        else {
            $('#uniform-CanReduce span').prop('class', '');
            $('#CanReduce').prop('checked', '');

            $('#uniform-CanFree span').prop('class',  '');
            $('#CanFree').prop('checked', '');

            if (this.id == "IsMealPlan" && this.checked) {
                $('#uniform-IsMealEquiv span').prop('class', '');
                $('#IsMealEquiv').prop('checked', '');
            }
            else if (this.id == "IsMealEquiv" && this.checked) {
                $('#uniform-IsMealPlan span').prop('class', '');
                $('#IsMealPlan').prop('checked', '');
            }
        }
    });*/

    $("input[type=checkbox]").change(function (e) {
        debugger;
        if (this.id == "CanReduce" || this.id == "CanFree") {
            if (this.checked) {
                $('#uniform-IsMealPlan span').prop('class', '');
                $('#IsMealPlan').prop('checked', '');

                $('#uniform-IsMealEquiv span').prop('class', '');
                $('#IsMealEquiv').prop('checked', '');
            }
        }
    });

    $("input[type=radio]").change(function (e) {

        $('#uniform-CanReduce span').prop('class', '');
        $('#CanReduce').prop('checked', '');

        $('#uniform-CanFree span').prop('class', '');
        $('#CanFree').prop('checked', '');

        if (this.id == "IsMealPlan" && this.checked) {
            $('#uniform-IsMealEquiv span').prop('class', '');
            $('#IsMealEquiv').prop('checked', '');
        }
        else if (this.id == "IsMealEquiv" && this.checked) {
            $('#uniform-IsMealPlan span').prop('class', '');
            $('#IsMealPlan').prop('checked', '');
        }
    });


    $("#btnSave").click(function () {
        // Get the id from the link
        //debugger;
        var id = $("#Id").val();

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
                "Name": $("#Name").val(),
                "CanReduce": $('#CanReduce').is(':checked'),
                "CanFree": $('#CanFree').is(':checked'),
                "IsMealPlan": $('#IsMealPlan').is(':checked'),
                "IsMealEquiv": $('#IsMealEquiv').is(':checked'),
            },
            dataType: "json",
            success: function (data) {
                if (data.IsError) {
                    displayWarningMessage(data.Message);
                }
                else {
                    // comment if window.location.reload(true); called
                    /*  if (id == '0') {
                          id = data.Id;
                          $("#Id").val(data.Id);

                          // create category box/partial view

                      }
                      else {
                          $("#name-" + data.Id).text(data.Name);
                      }*/

                    if (create) {
                        displaySuccessMessage('The category type record has been created successfully.');
                    }
                    else {
                        displaySuccessMessage('The category type record has been updated successfully.');
                    }
                    window.location.reload(true);
                }
            },
            error: function () {
                displayWarningMessage(data.ErrorMessage);
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
        "sAjaxSource": "/CategoryType/AjaxHandler",
        // set the initial value
        "iDisplayLength": 25,
        "sPaginationType": "bootstrap_full_number",
        "fnDrawCallback": function (oSettings) {
            disableEditDeleteLinks();
        },
        //"fnServerData": function (sSource, aoData, fnCallback) {
        //    aoData.push(
        //            { "name": "SearchBy", "value": $("#SearchBy").val() },
        //            { "name": "SearchBy_Id", "value": $("#SearchBy_Id").val() },
        //            { "name": "Category_Id", "value": $("#Category_Id").val() },
        //            { "name": "CategoryType_Id", "value": $("#CategoryType_Id").val() },
        //            { "name": "Taxable_Id", "value": $("#Taxable_Id").val() },
        //            { "name": "KitchenItem_Id", "value": $("#KitchenItem_Id").val() },
        //            { "name": "ScaleItem_Id", "value": $("#ScaleItem_Id").val() }
        //        );
        //    $.getJSON(sSource, aoData, function (json) {
        //        fnCallback(json)
        //    });
        //},
        "oLanguage": {
            //"sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": { "sPrevious": "Prev", "sNext": "Next" },
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Categories types.',
            "sInfoEmpty": 'No records.',
            "sEmptyTable": "No records found.",
        },
       
        "aoColumns": [
                   
                   {
                       "bSortable": false,
                       "sWidth": "8%",

                       "mRender" : function (data, type, row) {
                           return '<a  title="Edit" onclick="javascript:CreateOrEdit(this);" class=\"EditSecurityClass\" href="#CreateOrEdit" data-backdrop="static" data-keyboard="false" data-toggle="modal" class="ActionLink" data-id="' + row[0] + '"><i class="fa fa-pencil-square-o fasize"></i></a>' +
                               ' | <a  title="Delete" onclick="javascript:Delete(this);" href="#Delete" data-toggle="modal" data-backdrop="static" data-keyboard="false"  class="DeleteLink DeleteSecurityClass " data-id="' + row[1] + '"><i class="fa fa-trash fasize"></i></a>';
                       }
                   },
                    {
                        "bSortable": true,
                        "sWidth": "auto",
                        "mRender" : function (data, type, row) {
                            return '<div> '+ row[2] + '</div>';
                        }
                    },
                    {
                        "bSortable": false,
                        "sWidth": "auto",
                        "mRender" : function (data, type, row) {
                            //return '<div> ' + row[3] + '</div>';
                            return '<a href=/Category?id=' + row[5] + ' class=\"ViewSecurityCategoryClass\" >' + row[3] + '</a>'
                        }
                    },
                    {
                        "bSortable": false,
                        "sWidth": "auto",
                        "mRender" : function (data, type, row) {
                            //return '<div> ' + row[3] + '</div>';
                            return '<a href=/Menu?id=' + row[5] + ' class=\"ViewSecurityMenuClass\" >' + row[4] + '</a>'
                        }
                    }

        ]
    });

    $('#dtGrid_wrapper .dataTables_length select').addClass("form-control input-small"); // modify table per page dropdown
    /////////////////////////////////
    
    $('#Delete').on('shown.bs.modal', function () {
        //$('#btnDelete').addClass('defaultBtnClass');
        $('#btnSave').removeClass('defaultBtnClass');
    });
    $('#Delete').on('hidden.bs.modal', function () {
       // $('#btnDelete').removeClass('defaultBtnClass');
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
                if (data.IsError) {
                    displayWarningMessage(data.Message);
                }
                else {
                    $('#Id').val(id);
                    $('#span-title').text(data.Title);
                    $('#Name').val(data.Name);

                    $('#savebtnSpan').text(data.savebtnCaption);

                    $('#uniform-CanReduce span').prop('class', data.CanReduce ? 'checked' : '');
                    $('#CanReduce').prop('checked', data.CanReduce ? 'checked' : '');

                    $('#uniform-CanFree span').prop('class', data.CanFree ? 'checked' : '');
                    $('#CanFree').prop('checked', data.CanFree ? 'checked' : '');

                    $('#uniform-IsMealPlan span').prop('class', data.IsMealPlan ? 'checked' : '');
                    $('#IsMealPlan').prop('checked', data.IsMealPlan ? 'checked' : '');

                    $('#uniform-IsMealEquiv span').prop('class', data.IsMealEquiv ? 'checked' : '');
                    $('#IsMealEquiv').prop('checked', data.IsMealEquiv ? 'checked' : '');
                }
            },
            error: function () {
                displayWarningMessage(data.ErrorMessage);
            }
        });
    }


}

function disableEditDeleteLinks() {

    disableEditLinksTile("UpdateCategoryTypes", "EditSecurityClass", "ActionLink", "You don’t have rights to edit a Category Type.");
    disableDeleteLinksTile("DeleteCategoryType", "DeleteSecurityClass", "You don’t have rights to delete a Category Type.");
    disableLink("viewCategories", "ViewSecurityCategoryClass", "You don’t have rights to view a Category.");
    disableLink("viewMenu", "ViewSecurityMenuClass", "You don’t have rights to view a Menu item.");

}


