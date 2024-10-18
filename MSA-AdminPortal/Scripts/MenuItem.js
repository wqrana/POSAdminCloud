
var controller = '/Menu/';
var CreateAction = controller + 'Create';
var EditAction = controller + 'Edit';
var oCustomerTable;

function SearchAndFilterClick(t) {

    t.fnDraw();
}

function CheckCategoryType() {

    var id = $('#MenuItem_Category_Id').val();

    $.ajax({
        type: "get",
        url: "/Menu/CheckCategory",
        data: { "id": id },
        dataType: "json",
        success: function (data) {
            if (data == true) {
                $('#ReducedPrice').slideDown();
            }
            else {
                $('#ReducedPrice').slideUp();
            }
        },
        error: function (data) { }
    });
}

function SlideUpFilterBox(t) {

    $("#hideFilter").text("Use Filters");
    $('#filterDiv').slideUp();

    $("#Category_Id").val(null);
    $("#CategoryType_Id").val(null);
    $("#Taxable_Id").val(null);
    $("#KitchenItem_Id").val(null);
    $("#ScaleItem_Id").val(null);

    t.fnDraw();
}

function SlideDownFilterBox() {

    $("#hideFilter").text("Hide Filters");
    $('#filterDiv').slideDown();

}

function CreateOrEdit(e) {
    //debugger;
    var id = $(e).attr("data-id");

    var url = '';
    if (id == '0') {
        url = CreateAction;
        $("#MenuItem_Title_Edit").hide();
        $("#MenuItem_Title_Create").show();
    }
    else {
        url = EditAction;
        $("#MenuItem_Title_Create").hide();
        $("#MenuItem_Title_Edit").show();
    }

    if (id != '') {

        $.ajax({
            type: "get",
            url: url,
            data: { "id": id },
            dataType: "json",
            success: function (data) {

                var model = data.MenuItem;
                //debugger;


                if (model.IsError) {
                    displayWarningMessage(model.ErrorMessage);
                }
                else {
                    $('#savebtnSpan').text(model.savebtnCaption);

                    var prefix = '#MenuItem_';

                    if (id == '0') {
                        $(prefix + 'Title_Create').prop('style', '');
                        $(prefix + 'Title_Edit').attr('style', 'display:none');
                        $(prefix + 'Title-Name').text('');
                    }
                    else {
                        $(prefix + 'Title_Create').attr('style', 'display:none');
                        $(prefix + 'Title_Edit').prop('style', '');

                        $(prefix + 'Title-Name').text(model.ItemName);
                    }

                    $(prefix + 'Category_Id').val(model.Category_Id != 0 ? model.Category_Id : "");
                    $(prefix + 'Id').val(model.Id);
                    $(prefix + 'ItemName').val(model.ItemName);
                    $(prefix + 'PreOrderDesc').val(model.PreOrderDesc);

                    $(prefix + 'UPC').val(model.UPC);
                    $(prefix + 'ButtonCaption').val(model.ButtonCaption);

                    //Item Type
                    $('#uniform-ItemType_NA span').prop('class', model.ItemType == 0 ? 'checked' : '');
                    $('#ItemType_NA').prop('checked', model.ItemType == 0 ? 'checked' : '');

                    $('#uniform-ItemType_LunchItem span').prop('class', model.ItemType == 1 ? 'checked' : '');
                    $('#ItemType_LunchItem').prop('checked', model.ItemType == 1 ? 'checked' : '');

                    $('#uniform-ItemType_Breakfast span').prop('class', model.ItemType == 2 ? 'checked' : '');
                    $('#ItemType_Breakfast').prop('checked', model.ItemType == 2 ? 'checked' : '');

                    //Menu Items Specifications
                    $('#uniform-isTaxable span').prop('class', model.isTaxable ? 'checked' : '');
                    $('#isTaxable').prop('checked', model.isTaxable ? 'checked' : '');

                    $('#uniform-isScaleItem span').prop('class', model.isScaleItem ? 'checked' : '');
                    $('#isScaleItem').prop('checked', model.isScaleItem ? 'checked' : '');

                    $('#uniform-isOnceDay span').prop('class', model.isOnceDay ? 'checked' : '');
                    $('#isOnceDay').prop('checked', model.isOnceDay ? 'checked' : '');

                    $('#uniform-KitchenItem span').prop('class', model.KitchenItem == 1 ? 'checked' : '');
                    $('#KitchenItem').prop('checked', model.KitchenItem ? 'checked' : '');

                    $(prefix + 'StudentFullPrice').val(model.StudentFullPrice == null ? "0.00" : model.StudentFullPrice.toFixed(2));
                    $(prefix + 'StudentRedPrice').val(model.StudentRedPrice == null ? "0.00" : model.StudentRedPrice.toFixed(2));
                    $(prefix + 'EmployeePrice').val(model.EmployeePrice == null ? "0.00" : model.EmployeePrice.toFixed(2));
                    $(prefix + 'GuestPrice').val(model.GuestPrice == null ? "0.00" : model.GuestPrice.toFixed(2));
                    if (model.displayReducedPrice) {
                        $('#ReducedPrice').show();
                    } else {
                        $('#ReducedPrice').hide();
                    }
                    openCreateEditPopUp();
                    //debugger;
                }
            },
            error: function () {
                displayWarningMessage(model.LoadErrorMessage);
            }
        });
    }
}
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

$(document).ready(function () {

    $("#hideFilter").text("Use Filters");
    $('#filterDiv').slideUp();
    $('.floatOnly').numeric();

    disableCreationRights("CreateMenuObjects", "AddNewButton", "aAddNewButton", "disabled", "ActionLink", "You don’t have rights to create a Menu item.");
    disableEditDeleteLinks();



    $(".SearchAndFilter").click(function (e) {

        SearchAndFilterClick(oCustomerTable);

    });

    // Document.ready -> link up remove event handler
    $(".ActionLink").click(function () {

        CreateOrEdit(this);

    });

    $("#btnSave").click(function () {
       // debugger;
        var prefix = '#MenuItem_';

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
                "ItemName": $(prefix + 'ItemName').val(),
                "Category_Id": $(prefix + 'Category_Id').val(),
                "UPC": $(prefix + 'UPC').val(),
                "ButtonCaption": $(prefix + 'ButtonCaption').val(),

                //Item Type : N/A=0, Lunch Item = 1 and Breakfast=2
                "ItemType": $('#ItemType_LunchItem').is(':checked') ? 1 : $('#ItemType_Breakfast').is(':checked') ? 2 : 0,

                //Menu Items Specifications
                "isTaxable": $('#isTaxable').is(':checked'),
                "isScaleItem": $('#isScaleItem').is(':checked'),
                "isOnceDay": $('#isOnceDay').is(':checked'),
                "KitchenItem": $('#KitchenItem').is(':checked'),

                // Prices
                "StudentFullPrice": $(prefix + 'StudentFullPrice').val(),
                "StudentRedPrice": $(prefix + 'StudentRedPrice').val(),
                "EmployeePrice": $(prefix + 'EmployeePrice').val(),
                "GuestPrice": $(prefix + 'GuestPrice').val(),
                "PreOrderDesc": $(prefix + 'PreOrderDesc').val()
            },
            dataType: "json",
            success: function (data) {

                var model = data.MenuItem;

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
                          $("#name-" + model.Id).text(model.ItemName);
                      }*/

                    if (create) {
                        displaySuccessMessage('The item record has been created successfully.');
                    }
                    else {
                        displaySuccessMessage('The item record has been updated successfully.');
                    }
                    window.location.reload(true);
                }
            },
            error: function () {
                displayWarningMessage(model.ErrorMessage);
            }
        });
    });

    $("#closeFilter").click(function () {
        SlideUpFilterBox(oCustomerTable);
    });

    $("#hideFilter").click(function () {

        if ($("#hideFilter").text() == "Hide Filters") {

            SlideUpFilterBox(oCustomerTable);
        }
        else {
            SlideDownFilterBox();
        }

    });

    ///////
    oCustomerTable = $('#dtGrid').dataTable({
        //"sDom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-3 col-sm-12'i><'col-md-5 col-sm-12'l><'col-md-4 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100, -1],
            [10, 25, 50, 100, "All"] // change per page values here
        ],
        "order": [[1, "asc"]],
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": "/Menu/AjaxHandler",
        // set the initial value
        "iDisplayLength": 25,
        "sPaginationType": "bootstrap_full_number",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push(
                    { "name": "SearchBy", "value": $("#SearchBy").val() },
                    { "name": "SearchBy_Id", "value": $("#SearchBy_Id").val() },
                    { "name": "Category_Id", "value": $("#Category_Id").val() },
                    { "name": "CategoryType_Id", "value": $("#CategoryType_Id").val() },
                    { "name": "Taxable_Id", "value": $("#Taxable_Id").val() },
                    { "name": "KitchenItem_Id", "value": $("#KitchenItem_Id").val() },
                    { "name": "ScaleItem_Id", "value": $("#ScaleItem_Id").val() },
                    { "name": "hdCategory_Id", "value": $("#hdCategoryID").val() }

                );
            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json)
                $("#hdCategoryID").val(null);
            });
        },
        "fnDrawCallback": function (oSettings) {
            disableEditDeleteLinks();
            //Fixed Bug 1919
            $('#SearchBy').focus();
        },
        "oLanguage": {
            //"sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": { "sPrevious": "Prev", "sNext": "Next" },
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Items.',
            "sInfoEmpty": 'No records.',
            "sEmptyTable": "No records found.",
        },
        //"oLanguage": {
        //    "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Items.',
        //    "sInfoEmpty": 'No records.',
        //    "sEmptyTable": "No records found.",
        //},
        "aoColumns": [
            {
                "sWidth": "8%",
                "bSortable": false,
                "sClass": "center",
                "mRender": function (data, type, row) {
                    return '<a  title="Edit" href="#" onclick="CreateOrEdit(this)" data-backdrop="static" data-keyboard="false" data-toggle="modal" class="ActionLink EditSecurityClass" data-id="' + row[0] + '"><i class="fa fa-pencil-square-o fasize"></i></a>' +
                        ' | <a  title="Delete" onclick="javascript:Delete(this);" href="#Delete" data-toggle="modal" data-backdrop="static" data-keyboard="false"  class="DeleteLink DeleteSecurityClass" data-id="' + row[1] + '"><i class="fa fa-trash fasize"></i></a>';
                }
            },
            {
                "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<div>' + row[2] + '</div>';
                }
            },
            {

                "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<div style="background-color: ' + row[3] + ';width:16px;height:16px;float:left;margin-right:10px">&nbsp;</div><div>' + row[4] + '<div>';
                }
            },
            {
                "bSortable": false,
                "mRender": function (data, type, row) {
                    return '<div>' + (row[5] !== 'null') ? row[5] : '' + '</div>';
                }
            },
        ]
    });

    $('#dtGrid_wrapper .dataTables_length select').addClass("form-control input-small"); // modify table per page dropdown

    var CategoryName = getParameterByName('Cat');
    if (CategoryName !== "") {
        var stateObj = { foo: "Menu" };
        history.pushState(stateObj, "Menu", "Table");

        $("#SearchBy_Id").val("1");
        $("#hdCategoryID").val(CategoryName);

        SearchAndFilterClick(oCustomerTable);
    }

    $('#CreateOrEdit').on('shown.bs.modal', function () {
        $('#btnDelete').removeClass('defaultBtnClass');
        $('#btnSave').addClass('defaultBtnClass');
    });
    $('#CreateOrEdit').on('hidden.bs.modal', function () {
        $('#btnDelete').removeClass('defaultBtnClass');
        $('#btnSave').removeClass('defaultBtnClass');
    });


    $('#Delete').on('shown.bs.modal', function () {
        $('#btnDelete').addClass('defaultBtnClass');
        $('#btnSave').removeClass('defaultBtnClass');
    });
    $('#Delete').on('hidden.bs.modal', function () {
        $('#btnDelete').removeClass('defaultBtnClass');
        $('#btnSave').removeClass('defaultBtnClass');
    });


    $('#SearchBy').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            oCustomerTable.fnDraw();
        }
    });
    /*
    //Fixed Bug 1919
    $('#SearchBy').focus();

    $('#SearchBy').focusout(function () {
        $('#SearchBy').focus();
    });
    */
});


function foo() {
    //some delay
}


$("#Clearfilters").click(function (e) {
    ClearMenuItemsFilters();
    setTimeout(foo, 3000);
    oCustomerTable.fnDraw();
});

function ClearMenuItemsFilters() {

    $("#SearchBy").val("");

    $("#SearchBy_Id").val("0");
    $("#SearchBy_Id").select2("val", "0");



    $("#CategoryType_Id").val("");
    $("#CategoryType_Id").select2("val", "");

    $("#Category_Id").val("");
    $("#Category_Id").select2("val", "");

    $("#Taxable_Id").val("");
    $("#Taxable_Id").select2();

    $("#ScaleItem_Id").val("");
    $("#ScaleItem_Id").select2();


    $("#KitchenItem_Id").val("");
    $("#KitchenItem_Id").select2();

}


function disableEditDeleteLinks() {

    disableEditLinksTile("UpdateMenuDetails", "EditSecurityClass", "ActionLink", "You don’t have rights to edit a Menu item.");
    disableDeleteLinksTile("DeleteMenuObjects", "DeleteSecurityClass", "You don’t have rights to delete a Menu Item.");

}

function openCreateEditPopUp() {
    if ($("#UpdateMenuDetails").val() != "True") {
        displayWarningMessage("You don’t have rights to edit a Menu item.");
    }
    else {
        $("#CreateOrEdit").modal('show');
    }
}