/*
-- Author:		Waqar Q.
-- Create date: 2017-04-15
-- Description:	Java Script file for Void functionality for Order & Item

-- Modification History     

*/
//Script level variables
var selectedItemForVoid         = [];
var selectedOrderForVoid        = [];
var selectedOrderVoidPayment    = [];
var objLoadOrderDataTable       = null;
var objLoadItemDataTable        = null;
var currentOrderDataRows        = null;
var currentItemDataRows         = null;
var selectedPreorder            = null;
var voidPaymentObj              = null;
var voidPaymentEle              = null;
var allVoidProcess              = null;
var voidErrorList               = [];
var voidType                    = null;
var voidConfirmationMsgType = null;
var isVoidProcess = null;
$(document).ready(function () {


    $('.collapse')
        .on('shown.bs.collapse', function () {
            //debugger;
            $(this)
                .parent()
                .find("span.glyphicon")
                .removeClass("glyphicon-menu-left")
                .addClass("glyphicon-menu-down");
        })
        .on('hidden.bs.collapse', function () {
            //debugger;
            $(this)
                .parent()
                .find("span.glyphicon")
                .removeClass("glyphicon-menu-down")
                .addClass("glyphicon-menu-left");
        });

  
    $('#LoadVoidOrderDataTable tbody').on('change', 'input[type="checkbox"]', function (e) {
        //debugger;
        var state = this.checked;
        var id = $(this).attr('id');
        setVoidOrderList(id, state);


    });

    $('#ResetOrderBtnID').click(function () {
       // debugger;
        // $('#LoadVoidOrderDataTable tbody').find(':checkbox').prop('checked', false);
        $('#LoadVoidOrderDataTable tbody').find(':checked').each(function () {
           // debugger;
            var id = $(this).attr('id');
            $(this).prop('checked', false);
            selectedOrderForVoid = $.grep(selectedOrderForVoid, function (e) {
                        return e.Id != id;
                        });
           
            selectedOrderVoidPayment = $.grep(selectedOrderVoidPayment, function (e) {
                            return e.id != Id;
            });

            voidPaymentEle = $('#LoadVoidOrderDataTable tbody').find('span#' + id);
            voidPaymentEle.html('');
                            

            });
        
        });                            
    
    $('#ResetItemBtnID').click(function () {
        //debugger;
        // $('#LoadVoidOrderDataTable tbody').find(':checkbox').prop('checked', false);
        $('#LoadVoidItemDataTable tbody').find(':checked').each(function () {
           // debugger;
            var id = $(this).attr('id');
            $(this).prop('checked', false);
            selectedItemForVoid = $.grep(selectedItemForVoid, function (e) {
                return e.Id != id;
            });


        });
    });

    $('#LoadVoidItemDataTable tbody').on('change', 'input[type="checkbox"]', function (e) {
        //debugger;
        var state = this.checked;
        var id = $(this).attr('id');
        setVoidItemList(id, state);


    });

    $('#VoidModalDiv').on('shown.bs.modal', function () {
        //debugger;
        // Reset Script level variable on popup open
        selectedItemForVoid         = [];
        selectedOrderForVoid        = [];
        selectedOrderVoidPayment    = [];
        objLoadOrderDataTable       = null;
        objLoadItemDataTable        = null;
        currentOrderDataRows        = null;
        currentItemDataRows         = null;
        selectedPreorder            = null;
        voidPaymentObj              = null;
        voidPaymentEle              = null;
        allVoidProcess              = null;
        voidErrorList               = [];
        voidType                    = null;
        voidConfirmationMsgType     = null;
        isVoidProcess               = false;
        $('#processingVoidItemDiv').show();
        $('#processingVoidOrderDiv').show();

        selectedPreorder = $("#hdSelectedPreorderValues").val();
        //Load Order Data
        isVoidOrderProcess = false;
        LoadOrderData();
        //Load Item Data
        LoadItemData();
       
    });
    $('#VoidModalDiv').on('hidden.bs.modal', function () {
                
        $("#hdSelectedPreorderValues").val('');
        $('#orderBadge').html('0');
        $('#itemBadge').html('0');
        //Destroy the Datatable of close of popup
        $('#LoadVoidOrderDataTable').dataTable().fnDestroy();
        $('#LoadVoidItemDataTable').dataTable().fnDestroy();

        if (isVoidProcess == true) {

            //Refesh pickup data 
            refreshLoadedData();
        }


    });

    // Confirmation popup for void payment regarding order
    $('#ConfirmModalDiv').on('hidden.bs.modal', function () {
        //debugger;
        if (voidConfirmationMsgType == 'payment') {
            if (voidPaymentObj && voidPaymentEle.html() != 'Yes') {
                voidPaymentEle.html('No');
                voidPaymentObj.voidPayment = false;
                selectedOrderVoidPayment.push(voidPaymentObj);

            }
        }
            voidConfirmationMsgType = null;
           

    });

    $('#ConfirmOk').click(function () {
        //debugger;
        if (voidConfirmationMsgType == 'payment')
          {
            if (voidPaymentObj) {
                voidPaymentEle.html('Yes');
                voidPaymentObj.voidPayment = true;
                selectedOrderVoidPayment.push(voidPaymentObj);
            }
        }
        else if (voidConfirmationMsgType == 'order') {
                $('#VoidOrderBtnID').trigger('click');

        }
        else if (voidConfirmationMsgType == 'item') {
            $('#VoidItemBtnID').trigger('click');

        }

        
        $('#ConfirmModalDiv').modal('hide');

    });

    $('#VoidOrderBtnID').click(function () {
       // debugger;
        if (selectedOrderForVoid.length < 1) {
            displayWarningMessage("Please select at least one order");
            return;

        }

        if (voidConfirmationMsgType == null) {
            $('#confirmationMsgDiv').html('Would you like to proceed to void for the selected order(s)?');
            voidConfirmationMsgType = 'order';
            $('#ConfirmModalDiv').modal('toggle');
        }
        
        else if (voidConfirmationMsgType == 'order') {

            //Start ajax for selected void Orders
            allVoidProcess = true;
            voidErrorList = [];
            voidType = 'Order';

            $('#processingVoidOrderDiv').show();
            $('#processingVoidItemDiv').show();
            for (var i = 0; i < selectedOrderForVoid.length; i++) {

                var orderDataObj = selectedOrderForVoid[i];
                var voidOrderObj = {};

                voidOrderObj.callingType = 'Void';
                voidOrderObj.callingParam = 'Order';
                voidOrderObj.orderId = orderDataObj.Id;
                voidOrderObj.orderType = orderDataObj.PreSaleTrans_Id == null ? '0' : '1';
                if (orderDataObj.HasPayment == 1) {

                    var obj = $.grep(selectedOrderVoidPayment, function (e) {
                        return e.id == orderDataObj.Id;
                    });

                    if (obj) {
                        voidOrderObj.voidPayment = obj.voidPayment;
                    }
                }

                //Call function which makes ajax call to server DB update
                UpdateVoid(voidOrderObj);

            } // each loop end
        }//end of else if
    });

    $('#VoidItemBtnID').click(function () {
        //debugger
        if (selectedItemForVoid.length < 1) {
            displayWarningMessage("Please select at least one item");
            return;

        }
        if (voidConfirmationMsgType == null) {
            $('#confirmationMsgDiv').html('Would you like to proceed to void for the selected item(s)?');
            voidConfirmationMsgType = 'item';
            $('#ConfirmModalDiv').modal('toggle');
        }
        else if(voidConfirmationMsgType == 'item'){

        //Start ajax for selected void Orders
        allVoidProcess = true;
        voidErrorList = [];
        voidType = 'Item';
        $('#processingVoidItemDiv').show();
        for (var i = 0; i < selectedItemForVoid.length; i++) {

            var itemDataObj = selectedItemForVoid[i];
            var voidItemObj = {};

            voidItemObj.callingType     = 'Void';
            voidItemObj.callingParam    = 'Item';
            voidItemObj.itemId          = itemDataObj.Id;
            voidItemObj.orderId         = itemDataObj.PreorderId == null ? itemDataObj.orderId : itemDataObj.PreorderId;
            voidItemObj.customerId      = itemDataObj.customerId
            voidItemObj.orderType       = itemDataObj.PreorderId == null ? '0' : '1';     

            //Call function which makes ajax call to server DB update
            UpdateVoid(voidItemObj);

        }       // each loop end
       }
    });
    
        // callback, on completion of all ajax calls specific to Item / Order voiding
        $(document).ajaxStop(function () {
           // debugger;
            if (voidType == null) { return; }

            if (voidType == 'Item') {

                if (allVoidProcess == true && voidErrorList.length == 0) {
                    isVoidProcess = true;
                    displaySuccessMessage("Selected Item(s) have been successfully voided")

                }
                else if (allVoidProcess == false && voidErrorList.length > 0) {

                    for (var i = 0; i < voidErrorList.length; i++) {

                        displayErrorMessage(voidErrorList[i]);
                    }

                }

                //Reset variables
                allVoidProcess = null;
                voidErrorList = [];
                voidType = null;
              //  ReloadOrderForVoid();
                ReloadItemForVoid();
                voidConfirmationMsgType = null;
                $('#processingVoidItemDiv').hide();
            }
            else if (voidType == 'Order') {
               
                if (allVoidProcess == true && voidErrorList.length == 0) {
                    isVoidProcess = true;
                    displaySuccessMessage("Selected Order(s) have been successfully voided")

                }
                else if (allVoidProcess == false && voidErrorList.length > 0) {

                    for (var i=0; i < voidErrorList.length; i++) {

                        displayErrorMessage(voidErrorList[i]);
                    }

                }

                //Reset variables
                allVoidProcess = null;
                voidErrorList = [];
                voidType = null;
                ReloadOrderForVoid();
                ReloadItemForVoid();
                $('#processingVoidOrderDiv').hide();
                $('#processingVoidItemDiv').hide();
                voidConfirmationMsgType = null;
            }


        });

  
});//End of document ready function

//Load Order DataTable
function LoadOrderData() {
   // debugger;

    objLoadOrderDataTable = null;
    if ($.fn.DataTable.isDataTable('#LoadVoidOrderDataTable')) {

        $('#LoadVoidOrderDataTable').dataTable().fnDestroy();

       }


    objLoadOrderDataTable = $('#LoadVoidOrderDataTable').dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-5 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10,25, 50, 100, -1],
            [10,25, 50, 100, 'All'] // change per page values here
        ],
        "paging": true,
        "ajax": "data.json",
        "bProcessing": false,
        "bServerSide": false,
        "cache": true,
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollAutoCss": true,
        "sScrollXInner": "110%",
      

        "sAjaxSource": "/PreorderPickup/LoadVoidOrderData",
        // set the initial value
        "iDisplayLength": 10,

        "fnInitComplete":
            function (oSettings, json) {
                // $('.dataTable').wrap('<div class="dataTables_scroll" />');
                //debugger;
                //alert('DataTables has finished its initialisation.' + json.status);
                var oSettings = objLoadOrderDataTable.fnSettings();
                var iTotalOrderRecords = oSettings.fnRecordsTotal();
                $('#orderBadge').html(iTotalOrderRecords);
                $('#processingVoidOrderDiv').hide();

                $('<div style="overflow: auto"></div>').append($('#LoadVoidOrderDataTable')).insertAfter($('#LoadVoidOrderDataTable' + '_wrapper div').first());

            },

        "sPaginationType": "bootstrap_full_number",

        "fnServerData":
            function (sSource, aoData, fnCallback) {
                aoData.push(

                { "name": 'callingType', "value": 'LoadOrder' },
                { "name": 'callingParam', "value": selectedPreorder }

                );

                $.getJSON(sSource, aoData, function (json) {

                    //  console.log(json);
                    fnCallback(json);
                    // jQuery("#SearchStr").focus();
                });
            },

        'rowCallback': function (row, data, dataIndex) {
            // console.log(row);
            // console.log(data);
        }
        ,
        "fnDrawCallback": function () {
            debugger;

            var table = new $.fn.dataTable.Api('#LoadVoidOrderDataTable');
            currentOrderDataRows = table.rows().data();

            //checkUnCheckAll();
            // restorecheckBoxes(this);
            // disableEditDeleteLinks();
        },
        "oLanguage": {
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
            "sInfoEmpty": 'No Order(s) found',
            "sEmptyTable": "No records found currently",
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": {
                "sPrevious": "Prev",
                "sNext": "Next"
            }
        },
        "aoColumnDefs": [

                   {
                       mData: "Id",
                       sType: "numeric",
                       aTargets: [0],
                       bSortable: false,
                       sClass: 'centered-cell',
                       mRender: function (data, type, full) {
                           //debugger;
                           var disable = ' ';
                           if (full.CanVoid == 'No') {
                               disable = ' disabled = true';
                           }
                          
                           return '<div style="text-align: center"><input type="checkbox" id="' + data + '"' + disable + '/></div>';
                       }

                   },
                   {
                       mData: "CustomerName",
                       sType: "string",
                       aTargets: [1]
                   },
                     {
                         mData: "UserID",
                         sType: "string",
                         aTargets: [2]

                     },

                   {

                       mData: "PreSaleTrans_Id",
                       sType: "numeric",
                       aTargets: [3]
                       
                   },
                   {
                       mData: "OrderId",
                       sType: "numeric",
                       aTargets: [4]
                   },


                    {
                        mData: "Grade",
                        sType: "string",
                        aTargets: [5]
                    },
                                                      

                     {
                         mData: "PurchasedDate",
                         sType: "string",
                         //format: 'MM/DD/YYYY',
                         aTargets: [6],
                         sClass: 'centered-cell',
                         mRender: function (data, type) {

                             if (data === null) return "";

                             return data;
                             //var pattern = /Date\(([^)]+)\)/;
                             //var results = pattern.exec(data);
                             //var dt = new Date(parseFloat(results[1]));
                             //var dateStr = formatDate(dt, 'd');
                             //return dateStr
                            // return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                         }
                     },

                                     
                    {
                        "mData": "Void",
                        "sType": "string",
                        aTargets: [7],
                        mRender: function (data, type) {

                            if (data === null) {
                                return "";
                            }
                            else if (data == 'VOID') {

                                //return '<span style = color:red>' + data + '<span>'
                                return '<div class="glyphicon glyphicon-ok" style ="color:red"></div>';
                            }

                            return data;
                        }

                    },
                    {
                        "mData": "CanVoid",
                        "sType": "string",
                        aTargets: [8],
                        mRender: function (data, type) {
                           // debugger;
                            if (data === null) {
                                return "";
                            }
                            else if (data == 'No') {

                                return '<span style = color:red>' + data + '<span>'
                            }

                            return data;
                        }

                    },
                    {
                    mData: "Id",
                sType: "numeric",
                aTargets: [9],
                
                sClass: 'centered-cell',
                mRender: function (data, type, full) {
                    
                   
                    //if (full.HasPayment == 1) {
                       
                    //    return ' <span id="'+data+'">Confirmation Required</span>'
                    //}
                          
                    return '<span id="'+data+'"></span>';
                   }

                }

                                    
        ]


    });


}
function setVoidOrderList(id, state) {
    debugger;

    voidPaymentEle = null;
    voidPaymentObj = null;

    voidPaymentEle = $('#LoadVoidOrderDataTable tbody').find('span#' + id);
    
    var result = $.grep(currentOrderDataRows, function (e) {
        return e.Id == id;
    });
    
    if (state == true) {

        selectedOrderForVoid.push(result[0]);
    }
    else {

        //selectedOrderForVoid.pop(result[0])
        //Remove
        selectedOrderForVoid = $.grep(selectedOrderForVoid, function (e) {
            return e.Id != result[0].Id;
        })
    }
    //Check for payment
    if (result[0].HasPayment == 1){
    
        if (state == true) {
               
            voidPaymentObj = { id: id, voidPayment: null};
            $('#confirmationMsgDiv').html('This order has a payment. Would you like to void the payment as well?');
            $('#ConfirmModalDiv').modal('toggle');
     
        }
        else if (state == false) {

            voidPaymentEle.html('');
            selectedOrderVoidPayment = $.grep(selectedOrderVoidPayment, function (e) {
                return e.id != id;
            });

          
        }
    }
    
 }

//Load Item DataTable
function LoadItemData() {
    //debugger;

    objLoadItemDataTable = null;
    if ($.fn.DataTable.isDataTable('#LoadVoidItemDataTable')) {

        $('#LoadVoidItemDataTable').dataTable().fnDestroy();
    }


    objLoadItemDataTable = $('#LoadVoidItemDataTable').dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-5 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10,25, 50, 100, -1],
            [10,25, 50, 100, 'All'] // change per page values here
        ],
        "paging": true,
        "ajax": "data.json",
        "bProcessing": false,
        "bServerSide": false,
        "cache": true,
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollAutoCss": true,
        "sScrollXInner": "110%",
          
        "sAjaxSource": "/PreorderPickup/LoadVoidItemData",
        // set the initial value
        "iDisplayLength": 10,

        "fnInitComplete":
            function (oSettings, json) {
                // $('.dataTable').wrap('<div class="dataTables_scroll" />');
                //debugger;
                //alert('DataTables has finished its initialisation.' + json.status);
                var oSettings = objLoadItemDataTable.fnSettings();
                var iTotalItemRecords = oSettings.fnRecordsTotal();
                $('#itemBadge').html(iTotalItemRecords);
                $('#processingVoidItemDiv').hide();
                
                $('<div style="overflow: auto"></div>').append($('#LoadVoidItemDataTable')).insertAfter($('#LoadVoidItemDataTable' + '_wrapper div').first());

            },

        "sPaginationType": "bootstrap_full_number",

        "fnServerData":
            function (sSource, aoData, fnCallback) {
                aoData.push(

                { "name": 'callingType', "value": 'LoadOrder' },
                { "name": 'callingParam', "value": selectedPreorder }

                );

                $.getJSON(sSource, aoData, function (json) {

                    //  console.log(json);
                    fnCallback(json);
                    // jQuery("#SearchStr").focus();
                });
            },

        'rowCallback': function (row, data, dataIndex) {
            // console.log(row);
            // console.log(data);
        }
        ,
        "fnDrawCallback": function () {
            //debugger;

            var table = new $.fn.dataTable.Api('#LoadVoidItemDataTable');
            currentItemDataRows = table.rows().data();
            console.log(table.page.info());

            //checkUnCheckAll();
            // restorecheckBoxes(this);
            // disableEditDeleteLinks();
        },
        "oLanguage": {
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
            "sInfoEmpty": 'No Item(s) found',
            "sEmptyTable": "No records found currently",
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": {
                "sPrevious": "Prev",
                "sNext": "Next"
            }
        },
        "aoColumnDefs": [

                   {
                       mData: "Id",
                       sType: "numeric",
                       aTargets: [0],
                       bSortable: false,
                       sClass: 'centered-cell',
                       mRender: function (data, type, full) {
                          // debugger;
                           var disable = ' ';
                           if (full.CanVoid == 'No') {
                               disable = ' disabled = true';
                           }

                           return '<div style="text-align: center"><input type="checkbox" id="' + data + '"' + disable + '/></div>';
                       }

                   },
                   {

                       mData: "CustomerName",
                       sType: "string",
                       aTargets: [1]
                      
                   },
                   {
                       mData: "UserID",
                       sType: "numeric",
                       aTargets: [2]
                   },


                    {
                        mData: "Grade",
                        sType: "string",
                        aTargets: [3]
                    },
                    {
                        mData: "PreSaleTrans_Id",
                        sType: "numeric",
                        aTargets: [4]
                    },

                    {
                        mData: "orderId",
                        sType: "numeric",
                        aTargets: [5]

                    },

                    {
                        mData: "ItemName",
                        sType: "string",
                        aTargets: [6]

                    },
                     {
                         mData: "purchasedDate",
                         sType: "string",
                         //format: 'MM/DD/YYYY',
                         aTargets: [7],
                         sClass: 'centered-cell',
                         mRender: function (data, type) {

                             if (data === null) return "";

                             return data;
                             //var pattern = /Date\(([^)]+)\)/;
                             //var results = pattern.exec(data);
                             //var dt = new Date(parseFloat(results[1]));

                             //var dateStr = formatDate(dt, 'd');
                             //return dateStr;
                             //return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                         }
                     },

                    {
                        mData: "ServingDate",
                        sType: "string",
                        aTargets: [8],
                        sClass: 'centered-cell',

                        mRender: function (data, type) {

                            if (data === null) return "";

                            return data;

                            //var pattern = /Date\(([^)]+)\)/;
                            //var results = pattern.exec(data);
                            //var dt = new Date(parseFloat(results[1]));
                            //var dateStr = formatDate(dt, 'd');
                            //return dateStr;
                            //return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                        }

                    },
                  

                    {
                        mData: "Qty",
                        sType: "numeric",
                        aTargets: [9]

                    },
                    {
                        "mData": "isVoid",
                        "sType": "string",
                        aTargets: [10],
                        mRender: function (data, type) {

                            if (data === null){
                                return "";
                            }
                            else if (data == 'VOID') {

                                // return '<span style = color:red>'+data+'<span>'
                                return '<div class="glyphicon glyphicon-ok" style ="color:red"></div>';
                            }

                            return data;
                        }
                    },

                    {
                        "mData": "CanVoid",
                        "sType": "string",
                        aTargets: [11],
                        mRender: function (data, type) {
                                //debugger;
                                if (data === null) {
                                    return "";
                                }
                                else if (data == 'No') {

                                    return '<span style = color:red>' + data + '<span>'
                                }

                                return data;
                            }

                    }
             ]


    });


}

function setVoidItemList(id, state) {
   // debugger;

    var result = $.grep(currentItemDataRows, function (e) {
        return e.Id == id;
    });

    if (state == true) {

        selectedItemForVoid.push(result[0]);
    }
    else {

        selectedItemForVoid.pop(result[0]);
    }

  }

function UpdateVoid(objVoid) {

    var voidStringifyObject = JSON.stringify(objVoid);
       
    $.ajax({
        url: '/PreorderPickup/UpdateVoidStatus',
        data: { 'voidRequestParm': voidStringifyObject }, //dataString,
        type: 'GET',
        success: function (res) {
           // debugger;
            var data = res.aaData;

            if (data.Result != 0) {
                allVoidProcess = false;
                voidErrorList.push(data.ErrorMessage);

            }
           
            },
        complete: function () {
          
        },
        error: function (xhr, status, error) {
           
            allVoidProcess = false;
            voidErrorList.push('Error during retrieving Data:' + error);
        }
    });

}

function ReloadOrderForVoid() {
    
    var table = $('#LoadVoidOrderDataTable').DataTable();
    table.ajax.reload(null, false);
    selectedOrderForVoid = [];
    selectedOrderVoidPayment = [];
}

function ReloadItemForVoid() {

    var table = $('#LoadVoidItemDataTable').DataTable();
    table.ajax.reload(null, false);
    selectedItemForVoid = [];
}







