/// <reference path="jquery-2.0.3.min.js" />
/// <reference path="jquery-2.0.3.intellisense.js" />
/// <reference path="C:\Food Service\POS\Admin\DEV-Preorder_Management\MSA-AdminPortal\Content/themes/assets/global/plugins/datatables/all.min.js" />

var nEditing = null;
var pickupItemsTable;
var selectedPickupIdsInPopup = new Array();
var pickupItemsQuantities = new Array();
var isProcess = null;

$(document).ready(function () {

    //var preorderIdsList = "1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15";
    pickupItemsTable = $('#pickupItemsTable').dataTable({
       "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-5 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100],
            [10, 25, 50, 100] // change per page values here
        ],
        "ajax": "data.json",
        "bProcessing": true,
        "bServerSide": true,
        "cache": true,
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollAutoCss": true,
        "sScrollXInner": "110%",


       // "bProcessing": true,
       // "scrollX": true,
        //"sScrollXInner": "110%",
        //"bScrollCollapse": true,
       // "bServerSide": true,
      //  "bAutoWidth": false,
        "order": [[1, "asc"]],
        "sAjaxSource": "/PreorderPickup/AjaxPickupItemsHandler",
        // set the initial value
        "iDisplayLength": 10,
        "sPaginationType": "bootstrap_full_number",
        "fnInitComplete": function () {
            // Move Page Length dropdown to the bottom of the popup.
            /*
            var parentDiv = $(".dataTables_paginate").parent("div");
            $("#pickupItemsTable_length").each(function () {
                $(this).appendTo($(parentDiv));
            });
            */
            $('<div style="overflow: auto"></div>').append($('#pickupItemsTable')).insertAfter($('#pickupItemsTable' + '_wrapper div').first());
        },
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push(
                    { "name": "PreorderIdsList", "value": selectedPreorderItemValues.toString() }
                );
            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json);
            });
        },
        "fnDrawCallback": function () {
            StoreAllPickupItemsQty(this);
            RestorePickupItemsQty(this);
            RestoreCheckboxes(this);
            nEditing = null;
        },
        "oLanguage": {
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
            "sInfoEmpty": 'No founds',
            "sEmptyTable": "No records found currently",
            //"sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": {
                "sPrevious": "Prev",
                "sNext": "Next"
            }
        },
        "aoColumns": [
            {
                "sName": "id",
                "sWidth": "3%",
                "sClass": "centerClass",
                "bSortable": false,
                "mRender": function (data, type, row) {
                    if(row[12] == "0" || row[11] == "VOID"){
                        return "<input type=\"checkbox\" id=\"" + row[0] + "\" onclick=\"CheckPickupItemClick(this," + row[0] + ")\" name=\"" + row[0] + "\" disabled=\"disabled\" />";
                    }
                    else{
                        return "<input type=\"checkbox\" id=\"" + row[0] + "\" onclick=\"CheckPickupItemClick(this," + row[0] + ")\" name=\"" + row[0] + "\" />";
                    }
                }
            },            
            {
                "sName": "MSA_Transaction_Id",
                "sWidth": "11%"
            },
            {
                "sName": "Grade",
                //"sWidth": "7%"
            },
            {
                "sName": "Customer_Name",
                //"sWidth": "12%"
            },
            {
                "sName": "User_Id",
                //"sWidth": "9%"
            },
            {
                "sName": "Item_Name",
                //"sWidth": "9%"
            },
            {
                "sName": "Date_Purchased",
                //"sWidth": "10%"
            },
            {
                "sName": "Date_To_Serve",
                "sWidth": "9%"
            },
            {
                "sName": "Date_Picked_Up",
                //"sWidth": "10%"
            },
            {
                "sName": "Ordered",
                //"sWidth": "5%"
            },
            {
                "sName": "Received",
                //"sWidth": "5%"
            },
            {
                "sName": "Void",
                //"sWidth": "5%",
                "bSortable": false,
                "mRender": function (data, type, row) {
                    if (row[11] == "VOID") return "<i class=\"fa fa-check\" style=\"margin-left:0px;\"></i>"; else return "";
                }
            },
            {
                "sName": "Qty_Being_Picked_Up",
                "sWidth": "11%",
                "bSortable": false
            }
        ]
    })


    //$(".dataTables_scroll").css("width", "110%");
    //$("#pickupItemsTable").css("min-width", "1500px");
    //$('.dataTables_scrollBody thead tr').addClass('hidden');
    $("#pickupItemsTable_filter").hide();

    $(window).on('resize', function () {
        pickupItemsTable.fnAdjustColumnSizing();
    });

    $('#pickupItemsTable tbody').on('click', 'tr td:nth-child(13)', function () {

        if (this.innerHTML.indexOf('input') > 0) {
            return;
        }

        var nRow = $(this).closest('tr');

        if (nEditing !== null && nEditing != nRow) {
            /* Currently editing - but not this row - restore the old before continuing to edit mode */
            RestoreRow(pickupItemsTable, nEditing);
            EditRow(pickupItemsTable, nRow);
            nEditing = nRow;
        } else {
            /* No edit in progress - let's start one */
            EditRow(pickupItemsTable, nRow);
            nEditing = nRow;
        }
    });
});


$("#model_PickupItems_Popup").on("shown.bs.modal", function () {
    pickupItemsTable.fnDraw();
    isProcess = 0;
});

$('#model_PickupItems_Popup').on('hidden.bs.modal', function () {
   if(isProcess == 1){
   // var table = $('#LoadDataTable').DataTable();
       // table.ajax.reload(null, false);
       //Refesh pickup data 
       refreshLoadedData();
   }
});

$("#btnProcess").click(function () {
    debugger;
    var selectedItemsToProcess = new Array();
    var gridData = pickupItemsTable.fnGetData();

    if (selectedPickupIdsInPopup.length == 0) {
        displayErrorMessage("Please select an item to process");
        return;
    }

    for (var i = 0; i < selectedPickupIdsInPopup.length; i++) {
        var obj = PickupItemQtyExits(selectedPickupIdsInPopup[i]);
        if (obj != null) {
            if (parseInt(obj.Qty) != 0) {
                var item = { Id: obj.Id, Qty: obj.Qty, DatePickedUp: moment().format() };
                obj.DatePickedUp = moment().format();
                selectedItemsToProcess.push(obj);
            }
        }
    }

    if (selectedItemsToProcess.length == 0) {
        displayErrorMessage("Please select an item to process");
        return;
    }

    var stringData = JSON.stringify({ items: selectedItemsToProcess });

    $.ajax({
        type: "POST",
        url: "/PreorderPickup/ProcessPickupItems",
        contentType: 'application/json',
        dataType: 'json',
        data: stringData,
        success: function (data) {
            if (data.result == '1') {
                displaySuccessMessage("All items processed successfully.");
                isProcess = 1;
                $('#model_PickupItems_Popup').modal('hide');
               // window.location.href = "/PreorderPickup/Index"
            }
            else {
                displayErrorMessage("Error occured while processing items.");
            }
        },
        error: function (request, status, error) {
            displayErrorMessage("Error occured while processing items.");
        }
    })
});

function OpenPickupItemsPopup() {

    if (selectedPreorderItemValues.length == 0) {
        displayErrorMessage("Please select an item to pick up");
        return;
    }

    EmptyItemsArray();
    nEditing = null;
    $("#model_PickupItems_Popup").modal({ keyboard: false, backdrop: 'static' });
    $("#model_PickupItems_Popup").modal('show');
}

function CheckPickupItemClick(checkBox, Id) {
    
    if (checkBox.checked) {
        var n = DoesPickupItemExist(Id);
        if (n == -1)
        selectedPickupIdsInPopup.push(Id);
    }
    else {
        DeletePickupItem(Id);
    }
}

function RestoreCheckboxes(obj) {

    var inputs = $('input', obj.fnGetNodes());
    for (var n = 0; n < inputs.length; n++) {
        if (inputs[n].type == 'checkbox') {
            if (DoesPickupItemExist(inputs[n].id) > -1) {
                inputs[n].checked = true;
            }
            else
            {
                inputs[n].checked = false;
            }
        }
    }
}
function EmptyItemsArray() {
    //debugger;
    if (selectedPickupIdsInPopup) {
        selectedPickupIdsInPopup.splice(0, selectedPickupIdsInPopup.length);
    }
    if (pickupItemsQuantities) {
        pickupItemsQuantities.splice(0, pickupItemsQuantities.length);
    }
}

function DeletePickupItem(Text)
{
    var n = DoesPickupItemExist(Text);
    if (n > -1)
        selectedPickupIdsInPopup.splice(n, 1);
}

function DoesPickupItemExist(text)
{
    for (var n = 0; n < selectedPickupIdsInPopup.length; n++) {
        if (selectedPickupIdsInPopup[n] == text)
            return n;
    }
    return -1;
}

function RestoreRow(oTable, nRow) {
    var aData = oTable.fnGetData(nRow);
    var jqTds = $('>td', nRow);

    for (var i = 0, iLen = jqTds.length; i < iLen; i++) {
        oTable.fnUpdate(aData[i], nRow, i, false);
    }

    // on editing cancelling and restoring Qty being pickedup column, grid unchecks the checkbox, so we need to restore it.
    RestoreCheckboxes(oTable);
}

function EditRow(oTable, nRow) {
    var aData = pickupItemsTable.fnGetData(nRow);
    var jqTds = $('>td', nRow);
    jqTds[12].innerHTML = '<input id="QtyInput' + aData[12] + '" type="text" class="form-control input-small numericOnly" maxlength = 5 value="' + aData[12] + '">';

    $("#QtyInput" + aData[12]).enterKey(function () {
        SaveRow(oTable, nRow, aData[12]);
    });

    $("#QtyInput" + aData[12]).escKey(function () {
        CancelEditRow(oTable, nRow);
    });

    $("#QtyInput" + aData[12]).numericOnly(function () {
        return;
    })

    // on editing cancelling and restoring Qty being pickedup column, grid unchecks the checkbox, so we need to restore it.
    RestoreCheckboxes(oTable);
}

function SaveRow(oTable, nRow, input) {

    var newValue = $("#QtyInput" + input).val();

    if (newValue == null || newValue == '') return;

    var aData = pickupItemsTable.fnGetData(nRow);
    if (parseInt(newValue) + parseInt(aData[10]) > parseInt(aData[9])) {
        displayErrorMessage("The quantity being picked up cannot be greater than the quantity available.")
        return;
    }

    oTable.fnUpdate(newValue, nRow, 12, false);
    UpdatePickupItemQty(aData[0], newValue);

    // if user has entered zero in Qty being picked up column then the checkbox will become uncheck and we also remove its Id
    // from our array, because this item cannot be processed.
    if (newValue == "0") {
        CheckPickupItemClick($("#" + aData[0]), aData[0]);
    }

    // on editing cancelling and restoring Qty being pickedup column, grid unchecks the checkbox, so we need to restore it.
    RestoreCheckboxes(oTable);
}

function CancelEditRow(oTable, nRow) {
    var aData = oTable.fnGetData(nRow);
    oTable.fnUpdate(aData[12], nRow, 12, false);
    
    // on editing cancelling and restoring Qty being pickedup column, grid unchecks the checkbox, so we need to restore it.
    RestoreCheckboxes(oTable);
}

function StoreAllPickupItemsQty(grid) {
    var gridData = grid.fnGetData();
    for (var i = 0; i < gridData.length; i++) {
        StorePickupItemQty(gridData[i][0], gridData[i][12]);
    }
}

function RestorePickupItemsQty(grid) {
    var gridData = grid.fnGetData();
    for (var i = 0; i < gridData.length; i++) {
        var obj = PickupItemQtyExits(gridData[i][0]);
        if (obj != null) {
            grid.fnUpdate(obj.Qty, i, 12, false);
        }
    }
}

function StorePickupItemQty(id, qty) {
    if (PickupItemQtyExits(id) == null) {
        var obj = { Id: id, Qty: qty };
        pickupItemsQuantities.push(obj);
    }
}

function UpdatePickupItemQty(id, qty) {
    var obj = PickupItemQtyExits(id);
    if (obj != null) {
        obj.Qty = qty;
    }
}

function PickupItemQtyExits(id) {
    for (var i = 0; i < pickupItemsQuantities.length; i++) {
        if (pickupItemsQuantities[i].Id == id) {
            return pickupItemsQuantities[i];
        }
    }
    return null;
}