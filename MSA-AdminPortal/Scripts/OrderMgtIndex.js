﻿var oGroupTable;
var totalRows = 0;

$(document).ready(function () {
    $('#searchdll').select2();
    $('#ActiveList').select2();
    jQuery("#SearchStr").focus();
    if ($("#searchdll").val() == 2) {
        $("#ActiveFilter").show();
    } else {
        $("#ActiveFilter").hide();
    }
    ///group data table
    oGroupTable = $('#groupTable').dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-5 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100],
            [10, 25, 50, 100] // change per page values here
        ],
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": "/OrdersMgt/AjaxHandler",
        //"deferLoading": 57,
        // set the initial value
        "iDisplayLength": 10,
        "sPaginationType": "bootstrap_full_number",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push(
                    { "name": "SearchBy_Id", "value": getsearchDll() },
                    { "name": "SearchStr", "value": getsearstr() },
                    { "name": "DateStart", "value": getstartDate() },
                    { "name": "DateEnd", "value": getendDate() },
                    { "name": "CustomerID", "value": getCustomerID() },
                    { "name": "IsActive", "value": getActiveListValue() }

                );
            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json)
            });
        },
        "fnDrawCallback": function () {
            populateDetail();
        },
        "fnPreDrawCallback": function (oSettings) {
            //debugger;
            var fLoad = $('#firstLoad').val();
            if (fLoad == 'True') {
                $("#groupTable_length").css({ "display": "none" });
                $(".dataTables_paginate").css({ "display": "none" });
                return false;
            } else {
                $("#groupTable_length").css({ "display": "block" });
                $(".dataTables_paginate").css({ "display": "block" });
                return true;
            }
        },
        "oLanguage": {
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
            "sInfoEmpty": 'No records',
            "sEmptyTable": "No records found currently",
            //"sProcessing": '<i class="fa fa-coffee"></i>&nbsp;Please wait...',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": {
                "sPrevious": "Prev",
                "sNext": "Next"
            }
        },
        "aoColumns": [
                    {
                        "sName": "groupID",
                        "sWidth": "4%",
                        "bSearchable": false,
                        "bSortable": false,
                        "bVisible": false,
                        "mRender": function (data, type, row) {
                            return '<span class="row-details row-details-close"></span>';
                        }
                    },
                    {
                        "sName": "groupID",
                        "sWidth": "0%",
                        "bVisible": false,
                        "mRender": function (data, type, row) {
                            return row[1] + "|" + row[2];
                        }
                    },
                    {
                        "sName": "groupName",
                        "sWidth": "96%",
                        "bSortable": false,
                        "bSearchable": false,
                        "mRender": function (data, type, row) {
                            return '<div class="portlet box blue"><div class="portlet-title"> <div class="caption\"> ' + getTitle(row[3]) + ' </div> <div class="tools"> <a class="forclick expand showresult" href="javascript:;"></a> </div> </div> <div class="portlet-body" style="display: none;"> <div id=detailsDiv' + row[4] + ' class=\'waitClass\' >&nbsp;</div> </div></div>'

                        }

                    }
        ]
    });
    $('#groupTable_wrapper .dataTables_filter').hide(); //.addClass("form-control input-medium"); // modify table search input
    $('#groupTable_wrapper .dataTables_length select').addClass("form-control input-small"); // modify table per page dropdown


    ///

    $('#groupTable').on('click', ' tbody td .row-details', function () {
        //debugger;
        var nTr = $(this).parents('tr')[0];
        if (oGroupTable.fnIsOpen(nTr)) {
            /* This row is already open - close it */
            $(this).addClass("row-details-close").removeClass("row-details-open");
            oGroupTable.fnClose(nTr);
        }
        else {
            /* Open this row */
            $(this).addClass("row-details-open").removeClass("row-details-close");
            //oGroupTable.fnOpen(nTr, fnFormatDetails(oGroupTable, nTr), 'details');
            oGroupTable.fnOpen(nTr, getDetails(oGroupTable, nTr, fnFormatDetails), 'details');

        }
    });


    ////
    $('#groupTable').on('click', ' tbody td .showresult', function () {
        var nTr = $(this).parents('tr')[0];
        //debugger;
        if ($(this).hasClass('expand')) {
            getDetails(oGroupTable, nTr, fnFormatDetails);
        }
    });
    ///
    $('#reportrange').daterangepicker({
        opens: (Metronic.isRTL() ? 'left' : 'right'),
        startDate: moment().subtract('days', 29),
        endDate: moment(),
        minDate: '01/01/2009',
        maxDate: '12/30/' + (parseInt(moment().format('YYYY')) + 5),
        dateLimit: {
            days: 90000
        },
        showDropdowns: true,
        showWeekNumbers: true,
        timePicker: false,
        timePickerIncrement: 1,
        timePicker12Hour: true,
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
            'Last 7 Days': [moment().subtract('days', 6), moment()],
            'Last 30 Days': [moment().subtract('days', 29), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
        },
        buttonClasses: ['btn'],
        applyClass: 'green',
        cancelClass: 'default',
        format: 'MM/DD/YYYY',
        separator: ' to ',
        locale: {
            applyLabel: 'Apply',
            fromLabel: 'From',
            toLabel: 'To',
            customRangeLabel: 'Custom Range',
            daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
            monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
            firstDay: 1
        }
    },
function (start, end) {
    console.log("Callback has been called!");
    //debugger;
    $('#startDate').val(start);
    $('#EndDate').val(end);
    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
}
);
    //Set the initial state of the picker label
    $('#reportrange span').html(moment().subtract('days', 29).format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));

    //zubair update date search on page load
    $('#startDate').val(moment().subtract('days', 29)); //moment().subtract('days', 29)
    $('#EndDate').val(moment()); //moment()

    $("#SearchStr").keydown(function (e) {
        if (e.which === 13) {
            var searchdllval = $('#searchdll').val();
            if (searchdllval == '') {
                displayWarningMessage('The "Search by" value must be selected to search for activity.');
            } else {
                //SearchCustomer();
                $('#CustomerID').val('');
                $('#firstLoad').val("False");
                var stateObj = { foo: "OrdersMgt" };
                history.pushState(stateObj, "OrdersMgt", "OrdersMgt");
                oGroupTable.fnDraw();
            }
        }
    });

    $("#SearchBtn").click(function (e) {
        var searchdllval = $('#searchdll').val();
        if (searchdllval == '') {
            displayWarningMessage('The "Search by" value must be selected to search for activity.');
        } else {
            //SearchCustomer();
            //debugger;
            $('#CustomerID').val('');
            $('#firstLoad').val("False");
            var stateObj = { foo: "OrdersMgt" };
            history.pushState(stateObj, "OrdersMgt", "OrdersMgt");
            oGroupTable.fnDraw();
        }
    });


    ////////////
    var orderitemTable;
    orderitemTable = $('#orderitemTable').dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'><'col-md-3 col-sm-12'><'col-md-5 col-sm-12'>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100],
            [10, 25, 50, 100] // change per page values here
        ],
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": "/OrdersMgt/ItemDetailsAjaxHandler",
        // set the initial value
        "iDisplayLength": 10,
        "sPaginationType": "bootstrap_full_number",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push(
                    { "name": "orderId", "value": $("#orderId").val() },
                    { "name": "ordertype", "value": $("#ordertype").val() }
                );
            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json)
            });
        },

        "fnInitComplete" : function() {
            if ($(this).find('tbody tr').length<=1) {
                $(this).parent().hide();
            }
        },
            
        "fnDrawCallback": function () {
            //alert('DataTables has redrawn the table
            $('#hdnFldSelectedValues').val('');
            restorecheckBoxes(this);
            totalRows = orderitemTable.fnGetData().length;
            if (totalRows < 1) {
                $('#voiditemsBtn').hide();
            } else {
                $('#voiditemsBtn').show();
            }
        },
        "oLanguage": {
            //"sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
            "sInfoEmpty": 'No records',
            "bPaginate": false,
            "sEmptyTable": "No records found currently",
            "sLengthMenu": "_MENU_ records",
            "sInfo": false,
            "oPaginate": {
                "sPrevious": "Prev",
                "sNext": "Next"
            }
        },
        "aoColumns": [
                    {
                        "sName": "id",
                        "sWidth": "5%",
                        "bVisible": false,
                        "sClass": "centerCSS",
                        "bSortable": false,
                        "sClass": "centerCSS",
                        "mRender": function (data, type, row) {
                            return "<input type=\"checkbox\" id=\"" + row[0] + "\" onclick=\"ChildClick(this," + row[0] + ");\" name=\"" + row[0] + "\" />";
                        }
                    },
                    {
                        "sName": "Qty",
                        "sWidth": "10%",
                        "sClass": "centerCSS",
                        "bSearchable": false,
                        "bSortable": false
                    },
                    {
                        "sName": "ItemName",
                        "sWidth": "20%",
                        "sClass": "itemCSS",
                        "bSearchable": false,
                        "bSortable": false

                    },
                    {
                        "sName": "PaidPrice",
                        "sWidth": "15%",
                        "sClass": "centerCSS",
                        "bSearchable": false,
                        "bSortable": false

                    },
                    {
                        "sName": "Void",
                        "sWidth": "10%",
                        "sClass": "centerCSS",
                        "bSearchable": false,
                        "bSortable": false

                    },
                    {
                        "sName": "ServingDate",
                        "sWidth": "20%",
                        "sClass": "centerCSS",
                        "bSearchable": false,
                        "bSortable": false

                    },
                    {
                        "sName": "PickedupDate",
                        "sWidth": "20%",
                        "sClass": "centerCSS",
                        "bSearchable": false,
                        "bSortable": false

                    }


        ]
    });
    $('#orderitemTable_wrapper .dataTables_filter').hide(); //.addClass("form-control input-medium"); // modify table search input
    $('#orderitemTable_wrapper .dataTables_length select').addClass("form-control input-small"); // modify table per page dropdown


    $('#VoidpopupModel').on('shown.bs.modal', function () {
        orderitemTable.fnAdjustColumnSizing();
        orderitemTable.fnDraw();
        //debugger;

        //if order type items then hide date columns
        if ($("#ordertype").val() == "0") {
            orderitemTable.fnSetColumnVis(6, false);
            orderitemTable.fnSetColumnVis(5, false);
        }
    });

    $('#stack2').on('show.bs.modal', function () {
        $('#Order_Notes').val('');
        $('#voidPayment').prop('checked', false);
        $("#uniform-voidPayment span").removeClass("checked");

        $("#closePopup").attr("disabled", "disabled");
        $("#closePopup").addClass("disabled");

    });

    $('#stack2').on('hidden.bs.modal', function () {
        $("#closePopup").removeAttr("disabled");
        $("#closePopup").removeClass("disabled");
    })

    $(".modal").on("show.bs.modal", function () {
        //$("#orderitemTable_wrapper").html("Loading");
    });
    var notesVal = $("#Order_Notes").val();
    $("#Order_Notes").replaceWith('<textarea id="Order_Notes" name="Order_Notes" class="form-control">' + notesVal + '</textarea>');

    $('#OkButton').click(function () {
        VoidOrder();
    });

    $('#voidOrderBtn').click(function () {
        //debugger;
        var orderDateValue = $("#OrderDate").text();
        var today = moment();//.format('MM/DD/YYYY');
        var ordDate = moment(orderDateValue.substring(0, 10)); //.format('MM/DD/YYYY');
        var daysDiff = today.diff(ordDate, 'days');

        if (daysDiff > 30) {
            displayErrorMessage('Voiding is not possible past 30 days.');
            return false;
        } else {
            if ($('#VoidType').val() == "ADJ") {
                displayErrorMessage('Adjustment can not be voided');
                return false;
            }
            else if ($('#isVoid').val() == "true") {
                displayErrorMessage('Order is already void');
                return false;
            }
        }
        $('#voidPaymentDiv').css("display", "block");
        $("#VoidType").val('order');
    });

    $('#voiditemsBtn').click(function () {
        //debugger;
        checkSelectedItems();
        return checkInput();
    });

    setCustomerID();
    ShowCustomersSearch();
    jQuery("#SearchStr").focus();

});// ready

function populateDetail() {
    //debugger;
    if (getCustomerID() != 0) {
        $(".forclick").trigger("click");
    }

}

function checkInput() {
    if (SelectedItems.length == 0) {
        displayErrorMessage('No item is selected yet to void');
        return false;
    }
    else {
        $("#VoidType").val('items');
        if (totalRows == SelectedItems.length) {
            $("#VoidType").val('order');
        }
        return true;
    }
}

function checkSelectedItems() {
    //debugger;
    if (totalRows == SelectedItems.length) {
        $('#voidPaymentDiv').css("display", "block");

    }
    else {
        $('#voidPaymentDiv').css("display", "none");
    }
}

function getstartDate() {
    //debugger;
    var frmdval = $('#startDate').val();
    if (frmdval != '') {
        var fromdate = moment(frmdval);
        var retval = fromdate.format('MM/DD/YYYY');
        return retval;
    } else {
        return '2/2/2000';
    }
}

function ShowCustomersSearch() {
    //debugger;
    var searchBy = getParameterByName('searchByCust');

    if (searchBy == "yes") {
        $('#topDiv h2').text('Orders');

        $('#firstLoad').val('false');
        //oGroupTable.fnDraw();
        $('#searchdll').val('2');
        //$("#SearchBtn").trigger("click");
        searchCustomerData();
        //populateDetail();
    }

}

function searchCustomerData() {
    $('#firstLoad').val("False");
    var stateObj = { foo: "OrdersMgt" };
    history.pushState(stateObj, "OrdersMgt", "OrdersMgt");
    oGroupTable.fnDraw();
}


function getsearchDll() {
    //debugger;
    var searchBy = getParameterByName('searchByCust');
    if (searchBy != "") {
        $('#searchdll').val('2');
        return 2;
    }
    else {
        var searchdllval = $('#searchdll').val();
        if (searchdllval != '') {
            return searchdllval;
        } else {
            return 0;
        }
    }
}

function setCustomerID() {
    //debugger;
    var customerID = getParameterByName('cID');
    if (customerID != '') {
        $("#CustomerID").val(customerID);
    } else {
        $("#CustomerID").val("-999");
    }
}


function getCustomerID() {
    //debugger;
    var customerID = getParameterByName('cID');
    if (customerID != '') {
        $("#CustomerID").val(customerID);
        return customerID;
    } else {
        if ($("#CustomerID").val() != "-999") {
            return $("#CustomerID").val();
        } else {
            return 0;
        }
    }
}

//get customer id from querystring in URL

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function getsearstr() {
    //debugger;
    var searchstr = $('#SearchStr').val();
    if (searchstr != '') {
        return searchstr;
    } else {
        return " ";
    }
}


function getendDate() {
    var td = $('#EndDate').val();
    if (td != '') {
        var todate = moment(td);
        var retval = todate.format('MM/DD/YYYY');
        return retval;
    } else {
        return '2/2/2030';
    }
}


function getTitle(title) {
    //debugger;
    var searchdllval = $('#searchdll').val();
    var tempStr = "Cashier";
    switch (searchdllval) {
        case "0":
            tempStr = "Cashier";
            break;
        case "1":
            tempStr = "POS";
            break;
        case "2":
            tempStr = "Customer";
            break;
        case "3":
            tempStr = "School Name";
            break;
        default:
            tempStr = "Cashier";
    }

    return tempStr + ': ' + title;
}

function getTitleForPopup(title) {
    //debugger;
    var searchdllval = $('#searchdll').val();
    var tempStr = "Cashier";
    if (title.trim().indexOf("UserID") >= 0) {
        temptitle = title.split('UserID');
        title = temptitle[0];
    }
    switch (searchdllval) {
        case "0":
            tempStr = "Cashier Name";
            break;
        case "1":
            tempStr = "POS";
            break;
        case "2":
            tempStr = "Customer Name";
            break;
        case "3":
            tempStr = "School Name";
            break;
        default:
            tempStr = "Cashier Name";
    }

    return tempStr + ': ' + '<b>' + title + '</b>';
}

function StringifydetailFilters() {

    var GroupFilters = new Object();
    // Assigning values to the properties
    GroupFilters.SearchBy_Id = getsearchDll();
    GroupFilters.SearchStr = getsearstr();

    GroupFilters.DateStart = getstartDate();
    GroupFilters.DateEnd = getendDate();

    var data = JSON.stringify(GroupFilters);
    return data;
}



function fnFormatDetails(result, groupID, groupName) {
    //debugger;
    var sOut = '';
    var sOut = "<table id='detailOrderResult_" + groupID + "'  class='table innertable table-striped table-hover table-bordered'>";

    var len = result.length;
    var validLength = 0;
    if (len >= 1) {
        sOut += "<thead><tr><th>" + getHeaderText() + "</th><th>Order Date</th><th  class =\"centerCSS\" >Item</th><th class =\"centerCSS\" >Sales Tax</th><th class =\"centerCSS\">Total</th><th class =\"centerCSS\" >Payment</th><th class =\"centerCSS\">Type</th><th class =\"centerCSS\">Check</th><th class =\"centerCSS\" >POS</th><th></th></tr></thead>";
        for (var i = 0; i < len; i++) {
            //if (i == 0)
            //{
            //    sOut = sOut.replace("detailOrderResult", "detailOrderResult" + result[i].OrderID);
            //}

            var tempOrderDate = result[i].OrderDate;
            if (result[i].OrderDateLocal != '') {
                tempOrderDate = result[i].OrderDateLocal;
            }

            if (result[i].Name && tempOrderDate) {
                sOut += "<tr>";
                //sOut += "<td><label> <input type=\"checkbox\" id=\"Student\" name=\"Student\" /></label></td>";
                sOut += "<td>" + result[i].Name + "</td>";
                sOut += "<td>" + formatDate(tempOrderDate) + "</td>";
                sOut += "<td class =\"centerCSS\" >" + formatDecimal(result[i].Item) + "</td>";
                sOut += "<td class =\"centerCSS\" >" + formatDecimal(result[i].SalesTax) + "</td>";
                sOut += "<td class =\"centerCSS\" >" + formatDecimal(result[i].Total) + "</td>";
                sOut += "<td class =\"centerCSS\" >" + formatDecimal(result[i].Payment) + "</td>";
                sOut += "<td class =\"centerCSS\" >" + formatstr(result[i].Type) + "</td>";
                sOut += "<td class =\"centerCSS\" >" + formatstr(result[i].Check) + "</td>";
                sOut += "<td class =\"centerCSS\" >" + result[i].POS + "</td>";
                sOut += "<td class =\"centerCSS\" ><a data-toggle=\"modal\" data-backdrop=\"static\" data-keyboard=\"false\" onclick=\"showOrder(" + result[i].OrderID + "," + result[i].OrderType + ",'" + groupName + "'," + result[i].isVoid + ", '" + result[i].Type + "');\" role=\"button\" href=\"#VoidpopupModel\">Show Order</a></td>";
                sOut += "</tr>";
                validLength = 1;
            }
        }
    }
    else {
        sOut += "<tr><td>No activity found between " + getstartDate() + " and " + getendDate() + ".</td></tr>";
    }

    if (validLength === 0) {
        sOut = "<table id='detailOrderResult_" + groupID + "'  class='table innertable table-striped table-hover table-bordered'>";
        sOut += "<tr><td>No activity found between " + getstartDate() + " and " + getendDate() + ".</td></tr>";
        sOut += '</table>';
        $('#detailsDiv' + groupID).html(sOut);

        $('#detailsDiv' + groupID).removeClass('waitClass');
        $('#detailsDiv' + groupID).addClass('datablockcss');
        jQuery("#SearchStr").focus();
        return;
    }
    //debugger;
    sOut += '</table>';

    $('#detailsDiv' + groupID).html(sOut);

    $('#detailsDiv' + groupID).removeClass('waitClass');
    $('#detailsDiv' + groupID).addClass('datablockcss');
    ConvertToDataTable(groupID);
    jQuery("#SearchStr").focus();

}

function ConvertToDataTable(groupID) {
    oDetailResultTable = $('#detailOrderResult_' + groupID).dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-5 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "bProcessing": true,
        "bServerSide": false,
        "bPaginate": false,
        "sinfo": false,
        "bDestroy": true,
        //"iDisplayLength": 100,
        "aoColumns": [
                    {
                        "bSearchable": false,
                        "bSortable": true,
                    },
                    {
                        "bSortable": true,
                        "bSearchable": false
                    },
                    {
                        "bSortable": false,
                        "bSearchable": false
                    },
                    {
                        "bSortable": false,
                        "bSearchable": false
                    },
                    {
                        "bSortable": false,
                        "bSearchable": false
                    },
                    {
                        "bSortable": true,
                        "bSearchable": false
                    },
                    {
                        "bSortable": true,
                        "bSearchable": false
                    },
                    {
                        "bSortable": false,
                        "bSearchable": false
                    },
                    {
                        "bSortable": false,
                        "bSearchable": false
                    },
                    {
                        "bSortable": false,
                        "bSearchable": false
                    }


        ]

    });
    $('#detailOrderResult_' + groupID + '_info').hide();
}

function formatstr(val) {
    if (val == "") {
        return '-';
    }
    else {
        return val;
    }
}



function formatDate1(isodate) {
    var date = new Date(parseInt(isodate.substr(6)));
    return moment(date).format('MM/DD/YYYY @ hh:mm A');
}
function formatDate(isodate) {
    return moment(isodate).format('MM/DD/YYYY @ hh:mm A');
}

function formatDecimal(amount) {
    var outAmount = parseFloat(Math.round(amount * 100) / 100).toFixed(2);
    if (outAmount == 0.00) {
        return '<span class=\'ZeroColor\'>$0.00</span>';
    }
    else {
        //debugger;
        return '$' + outAmount;

    }
}

function formatDecimalWitoutSpan(amount) {
    var outAmount = parseFloat(Math.round(amount * 100) / 100).toFixed(2);
    if (outAmount == 0) {
        return '0.00';
    }
    else {
        //debugger;
        return outAmount;

    }
}

function getDetails(oTable, nTr, callback) {
    //debugger;
    var aData = oTable.fnGetData(nTr);
    var tempGroupData = aData[1].split('|');
    var groupID = tempGroupData[0];
    var GroupName = tempGroupData[1];
    var allDataa = groupID + "*" + getstartDate() + "*" + getendDate() + "*" + getsearchDll();

    var jsonGroupID = JSON.stringify({ allData: allDataa });

    //debugger;
    $.ajax({
        type: 'POST',
        url: '/OrdersMgt/GetDetail',
        data: jsonGroupID,
        contentType: "application/json; charset=utf-8",
        dataType: "json", //to parse string into JSON object,
        success: function (data) {
            //debugger;
            if (data.result != -1) {
                var result = $.parseJSON(data.result);
                $('#isVoid').val(data.isVoid);
                var yposition = window.pageYOffset;
                callback(result, groupID, GroupName);
                window.scrollTo(0, yposition);
            }
            else {
                var outMessage = "No activity found between " + getstartDate() + " and " + getendDate() + ".";
                var sOut = '<table class="table innertable table-striped table-hover table-bordered">';
                sOut += "<tr><td>" + outMessage + "</td></tr>";
                sOut += '</table>';
                $('#detailsDiv' + groupID).html(sOut);
                $('#detailsDiv' + groupID).removeClass('waitClass');
                $('#detailsDiv' + groupID).addClass('datablockcss');
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            displayErrorMessage('Error during display of details');
        }
    });
    //debugger;
    var divId = 'detailDiv' + groupID;
    return '<div id=' + divId + ' class=\'waitClass\' >&nbsp;</div>';
}

function showOrder(orderid, ordertype, name, isvoid, orderTypeName) {
    //debugger;

    if (isvoid === false) {
        $('#voidOrderBtn').show();
    } else {
        $('#voidOrderBtn').hide();
    }

    if (orderTypeName === 'REF') {
        $("#paymentType").html("Refund:");
    } else if (orderTypeName === 'ADJ') {
        $("#paymentType").html("Adjustment:");
    } else {
        $("#paymentType").html("Payment:");
    }

    if (orderTypeName === 'ADJ') {
        $('#voidOrderBtn').show();
        $('#voidOrderBtn').attr("disabled", "disabled");
        $('#adjNote').show();
    } else {
        $('#adjNote').hide();
        $('#voidOrderBtn').removeAttr("disabled");
    }

    if (orderid != '') {
        $("#orderId").val(orderid);
        $("#ordertype").val(ordertype);

        $.ajax({
            type: "get",
            url: '/OrdersMgt/PopupData',
            data: { "orderid": orderid, "ordertype": ordertype },
            dataType: "json",
            success: function (data) {
                if (data.IsError) {
                    displayWarningMessage(data.ErrorMessage);
                }
                else {
                    //debugger;
                    $('#customerName').html(getTitleForPopup(name));
                    $('#OrderNumber').text(data.OrderNumber);
                    $('#OrderDate').text(data.OrderDateString);
                    $('#OrderTotal').text('$' + formatDecimalWitoutSpan(data.OrderTotal));
                    $('#POSName').text(data.POSName);
                    $('#CashierName').text(data.CashierName);
                    $('#Payment').text('$' + formatDecimalWitoutSpan(data.Payment));
                    $('#CashierID').val(data.CashierID);
                    $('#OrdersLogID').val(data.OrdersLogID);
                    $('#isVoid').val(isvoid);
                    $('#closePopup').removeAttr('disabled');
                }
            },

            complete: function () {
                $('#Loaderdiv').hide();
            },
            error: function () {
            }
        });
        //
    }

}


//////

var TargetBaseControl = null;
//Total no of checkboxes in a particular column inside the GridView.
var CheckBoxes;
//Total no of checked checkboxes in a particular column inside the GridView.
var CheckedCheckBoxes;
//Array of selected item's Ids.
var SelectedItems;
//Hidden field that wil contain string of selected item's Ids separated by '|'.
var SelectedValues;

function restorecheckBoxes(obj) {

    //Get hidden field that wil contain string of selected item's Ids separated by '|'.
    SelectedValues = document.getElementById('hdnFldSelectedValues');

    //Get an array of selected item's Ids.
    if (SelectedValues.value == '')
        SelectedItems = new Array();
    else
        SelectedItems = SelectedValues.value.split('|');

    RestoreState(obj);
}

function ChildClick(CheckBox, Id) {
    //debugger;
    //Modifiy Counter;            
    if (CheckBox.checked && CheckedCheckBoxes < CheckBoxes)
        CheckedCheckBoxes++;
    else if (CheckedCheckBoxes > 0)
        CheckedCheckBoxes--;
    //Modify selected items array.
    if (CheckBox.checked)
        SelectedItems.push(Id);
    else
        DeleteItem(Id);
    //Update Selected Values. 
    SelectedValues.value = SelectedItems.join('|');
}
function RestoreState(obj) {
    //debugger;
    //Get all the control of the type INPUT.
    var Inputs = $('input', obj.fnGetNodes());

    //Restore previous state of the all checkBoxes in side the GridView.
    for (var n = 0; n < Inputs.length; ++n)
        if (Inputs[n].type == 'checkbox' && IsItemExists(Inputs[n].id) > -1) {
            Inputs[n].checked = true;
            CheckedCheckBoxes++;
        }
        else
            Inputs[n].checked = false;
}

function DeleteItem(Text) {
    var n = IsItemExists(Text);
    if (n > -1)
        SelectedItems.splice(n, 1);
}

function IsItemExists(Text) {
    for (var n = 0; n < SelectedItems.length; ++n)
        if (SelectedItems[n] == Text)
            return n;

    return -1;
}

function VoidOrder() {

    var OrderID = $('#orderId').val();
    var OrderType = $('#ordertype').val();

    var employeeID = $('#CashierID').val();
    var OrderLogId = $('#OrdersLogID').val();
    var OrderLogNote = $('#Order_Notes').val();
    var VoidType = $('#VoidType').val();
    var voidPayment = $('#voidPayment').is(':checked');
    var allDataa = OrderID + "*" + OrderType + "*" + SelectedItems + "*" + employeeID + "*" + OrderLogId + "*" + OrderLogNote + "*" + VoidType + "*" + voidPayment;

    var jsonData = JSON.stringify({ allData: allDataa });

    //debugger;
    $.ajax({
        type: 'POST',
        url: '/OrdersMgt/VoidOrder',
        data: jsonData,
        contentType: "application/json; charset=utf-8",
        dataType: "json", //to parse string into JSON object,
        success: function (data) {
            //debugger;
            if (data.result != -1) {
                if (data.vtype == "items") {
                    displaySuccessMessage('Selected items voided');
                    populateDetail();
                } else if (data.vtype == "order") {
                    populateDetail();
                    populateDetail();
                    displaySuccessMessage('Order voided');

                }
                $(".modal").modal('hide');
            }
            else {
                displayErrorMessage('Error during voiding');
            }
        }
    });
}

function getHeaderText() {
    var searchdllval = $('#searchdll').val();
    if (searchdllval != '') {
        if (searchdllval == "2") {
            return "Cashier Name ";
        }
        else {
            return "Customer Name";
        }
    } else {
        return "Customer Name";
    }


}


$("#searchdll").on("change", function () {
    if ($("#searchdll").val() == 2) {
        $("#ActiveFilter").show();
    } else {
        $("#ActiveFilter").hide();
    }
});


function getActiveListValue() {
    if ($("#searchdll").val() == 2) {
        return $('#ActiveList').val();
    } else
        return "";
}