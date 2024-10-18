$.getScript("/Content/themes/assets/global/plugins/jquery.formatCurrency-1.4.0/jquery.formatCurrency-1.4.0.min.js");

var obeginningBalanceTable;

var filterObj;
var CustSortCol;
var CustSortDir;
var textboxarr = [];


$(document).ready(function () {

    $('#btncomplete').attr('disabled', 'disabled');

    filterObj = null;
    //Default Customer sort data setting
    CustSortCol = 1;
    CustSortDir = "asc";

    //$("#hidefilters").text("Show Filters");
    //$('#filterDiv').slideUp();
    $("#SchoolFilter").select2();
    $("#searchdll").select2();
    $("#GradeFilter").select2();
    $("#cmbHomeroomFilter").select2();
    $("#DistrictFilter").select2();

    jQuery("#SearchStr").focus();

    $("#hidefilters").click(function () {
        toggleText(this.id);
    });

    $("#Clearfilters").click(function (e) {
        ClearBeginningBalanceFilters(true);


    });

    $("#applyFilterBtn").click(function (e) {
        if (obeginningBalanceTable == undefined) {
            drawBeginningBalanceTable();
        }
        else {

            obeginningBalanceTable.fnDraw();
        }
    });

    $("#SearchBtn").click(function (e) {
        if (obeginningBalanceTable == undefined) {
            drawBeginningBalanceTable();
        }
        else {

            obeginningBalanceTable.fnDraw();
        }
    });

    $('#SearchStr').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            if (obeginningBalanceTable == undefined) {
                drawbeginningBalanceTable();
            }
            else {

                obeginningBalanceTable.fnDraw();
            }
        }
    });

    $('#GradeFilter').on('change', function () {
        $('#cmbHomeroomFilter').html('');
        $("#cmbHomeroomFilter").select2();

        fetchHomeRoom(this.value, $('#SchoolFilter').val(), $('#DistrictFilter').val());
    })

    $('#SchoolFilter').on('change', function () {
        $('#GradeFilter').html('');
        $('#cmbHomeroomFilter').html('');
        $("#GradeFilter").select2();
        $("#cmbHomeroomFilter").select2();

        fetchGrades(this.value, $('#DistrictFilter').val());
    })

    $('#DistrictFilter').on('change', function () {

        $('#GradeFilter').html('');
        $("#GradeFilter").select2();

        $('#cmbHomeroomFilter').html('');
        $("#cmbHomeroomFilter").select2();

        $('#SchoolFilter').html('');
        $("#SchoolFilter").select2();

        fetchSchools(this.value);
    })

});

function drawBeginningBalanceTable() {

    $('#btncomplete').attr('disabled', 'disabled');

    obeginningBalanceTable = $('#beginningBalanceTable').dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r><'row'<'col-md-4 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-5 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "ajax": "data.json",
        "bProcessing": true,
        "bServerSide": true,
        "bPaginate": false,
        "bInfo": false,
        "paging": false,
        "order": [[CustSortCol, CustSortDir]],
        "cache": true,
        "sAjaxSource": "/BeginningBalance/AjaxSearchHandler",
        // set the initial value
        "iDisplayLength": 10,
        "fnInitComplete": function (oSettings, json) {

            //alert('DataTables has finished its initialisation.' + json.status);
        },

        "sPaginationType": "bootstrap_full_number",

        "fnServerData": function (sSource, aoData, fnCallback) {
            //debugger;
            aoData.push(
                    { "name": "SearchString", "value": $("#SearchStr").val() },
                    { "name": "SchoolFilter", "value": $('#SchoolFilter').val() },//.trim() == '') ? '' : $('#SchoolFilter').select2('data').text },
                    { "name": "GradeFilter", "value": $("#GradeFilter").val() },
                    { "name": "DistrictFilter", "value": $('#DistrictFilter').val() },
                    { "name": "HomeRoomFilter", "value": $("#cmbHomeroomFilter").val() },
                    { "name": "radomNumber", "value": Math.random() }
                );
            $.getJSON(sSource, aoData, function (json) {
                //debugger;
                fnCallback(json);
                $('#btncomplete').attr('disabled', 'disabled');
                jQuery("#SearchStr").focus();
            });
        },
        "fnDrawCallback": function () {
            //debugger;
            //restorecheckBoxes(this);
            //disableEditDeleteLinks();
            $("#thTotalLbl").attr('colspan', 3);
            //calculateTotal();
            //calculateGrandTotal();
            formatBoxes();
        },
        "oLanguage": {
            //"sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
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
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                    i : 0;
            };

            // For Meal Balance 6
            // Total over all pages 4
            var mbid = 4;
            total = api
                .column(mbid)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal = api
                .column(mbid - 1, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            //if (total >= 0)
            $(api.column(mbid - 1).footer()).html("<input type=\"text\" style=\"text-align: right; width: 100%;border:none\" id=\"thTotalM\" value=" + total + " readonly />");
            //else
            //$(api.column(4).footer()).html("<div style=\"text-align:right;color:red\">($" + total + ")</div>");


            // For Ala Carte Balance 5
            // Total over all pages 5
            var abid = 5;
            total = api
                .column(abid)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal = api
                .column(abid, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            $(api.column(abid - 1).footer()).html("<input type=\"text\" style=\"text-align: right; width: 100%;border:none\" id=\"thTotalA\" value=" + total + " readonly />");

            // For Total Balance 8
            // Total over all pages 6
            var tbid = 6;
            total = api
                .column(tbid)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal = api
                .column(tbid, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            $(api.column(tbid - 1).footer()).html("<input type=\"text\" style=\"text-align: right; width: 100%;border:none\" id=\"thTotalT\" value=" + total + " readonly />");

        },
        "aoColumns": [
                    {
                        "sName": "id",
                        "sWidth": "3%",
                        "sClass": "centerClass",
                        "bVisible": false,
                        "bSortable": false,
                        "sClass": "centerClass",
                        "mRender": function (data, type, row) {
                            return "<input type=\"checkbox\" id=\"" + row[0] + "\" onclick=\"ChildClick(this," + row[12] + ",\'" + row[12] + "\',\'" + row[12] + "\')\" name=\"" + row[12] + "\" />";
                            return row[0];
                        }
                    },
                    {
                        "sName": "UserId",
                        "sWidth": "9%",
                        "bSearchable": false,
                        "bSortable": true,
                        "mRender": function (data, type, row) {
                            //return '<a href=\"#CustomerModal\"  class=\"EditSecurityClass \" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + row[2] + '</a>';
                            return row[1];

                        }
                    },
                    {
                        "sName": "CustomerName",
                        "sWidth": "13%",
                        "mRender": function (data, type, row) {
                            //return '<a href=\"#CustomerModal\"    class=\"EditSecurityClass\" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + row[3] + '</a>';
                            return row[2];

                        }

                    },
                    {
                        "sName": "Grade",
                        "sWidth": "10%",
                        "mRender": function (data, type, row) {
                            //return '<a href=\"#CustomerModal\"    class=\"EditSecurityClass\" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + row[5] + '</a>';
                            return row[3];

                        }
                    },
                    {
                        "sName": "Meal Balance",
                        "sWidth": "10%",
                        "mRender": function (data, type, row) {
                            //if (Number(row[4]) >= 0)
                            //    return "<div style=\"text-align: right\">" + row[4] + "</div>";
                            //else
                            //    return "<div style=\"text-align: right;color:red\">(" + row[4] + ")</div>";
                            var field = row[4] == "" ? "$0.00" : row[4];
                            var field2 = row[4] == "" ? "0.00" : row[4];
                            var id = row[0] + "txt" + "MB";
                            var MBalance = (row[7] == "0" || row[7] == "") ? "0" : row[7];
                            textboxarr.push(id);
                            return "<input class=\"MB\" type=\"text\" data-id=" + row[0] + " id=" + id + " style=\"text-align:right;width: 100%\" data-MBalance=" + MBalance + " data-MBalanceActual=" + field2 + " onfocus=\"initAmountTextBoxes(this," + row[0] + ")\" onclick=\"initAmountTextBoxes(this," + row[0] + ")\" maxlength=\"7\" value=" + field + " />";
                        }
                    },
                    {
                        "sName": "Ala Carte Balance",
                        "sWidth": "10%",
                        "mRender": function (data, type, row) {
                            //if (Number(row[5]) >= 0)
                            //    return "<div style=\"text-align: right\">" + row[5] + "</div>";
                            //else
                            //    return "<div style=\"text-align: right;color:red\">(" + row[5] + ")</div>";
                            var field = row[5] == "" ? "$0.00" : row[5];
                            var field2 = row[5] == "" ? "0.00" : row[5];
                            var id = row[0] + "txt" + "ACB";
                            var AlaCarteBalance = (row[8] == "0" || row[8] == "") ? "0" : row[8];
                            textboxarr.push(id);
                            return "<input class=\"ACB\" type=\"text\" data-id=" + row[0] + " id=" + id + " style=\"text-align:right;width: 100%\"  data-AlaCarteBalance=" + AlaCarteBalance + " data-AlaCarteBalanceActual=" + field2 + " onfocus=\"initAmountTextBoxes(this," + row[0] + ")\" onclick=\"initAmountTextBoxes(this," + row[0] + ")\" maxlength=\"7\" value=" + field + " />";
                        }
                    },
                    {
                        "sName": "Total Balance",
                        "sWidth": "10%",
                        "mRender": function (data, type, row) {
                            //if (Number(row[6]) >= 0)
                            //    return "<div style=\"text-align: right\">" + row[6] + "</div>";
                            //else
                            //    return "<div style=\"text-align: right;color:red\">(" + row[6] + ")</div>";
                            var field = row[6] == "" ? "$0.00" : row[6];
                            var id = row[0] + "txt" + "TB";
                            textboxarr.push(id);
                            return "<input class=\"TB\" type=\"text\" id=" + id + "  style=\"text-align:right;width: 100%;border:none;background-color: inherit;\" tabindex=\"-1\" value=" + field + " readonly />";
                        }
                    },

        ]
    });
}

function formatBoxes() {
    if (textboxarr.length > 0) {
        for (var i = 0; i < textboxarr.length; i++) {
            $("#" + textboxarr[i]).formatCurrency({ colorize: true, negativeFormat: '-%s%n', roundToDecimalPlace: 2 });
        }
        //$("#" + thTotalM).formatCurrency({ colorize: true, negativeFormat: '-%s%n', roundToDecimalPlace: 2 });
        calculateGrandTotal();
    }
}

function toggleText(button_id) {
    var el = document.getElementById(button_id);
    if (el.firstChild.data == "Hide Filters") {
        el.firstChild.data = "Show Filters";
        HideFilterDiv();

    }
    else {
        el.firstChild.data = "Hide Filters";
        ShowFilterDiv();
    }
}

function HideFilterDiv() {

    $('#filterDiv').slideUp();
    var el = document.getElementById('hidefilters');
    el.firstChild.data = "Show Filters";
    return false;
}

function ShowFilterDiv() {

    $('#filterDiv').slideDown();
    var el = document.getElementById('hidefilters');
    el.firstChild.data = "Hide Filters";
    return false;
}

function ClearBeginningBalanceFilters(loadtable) {
    $('#btncomplete').attr('disabled', 'disabled');

    $("#SearchStr").val("");

    $("#DistrictFilter").val("");
    $("#DistrictFilter").select2();

    $("#SchoolFilter").html('');
    $("#SchoolFilter").val("");
    $("#SchoolFilter").select2();

    $("#GradeFilter").html('');
    $("#GradeFilter").val("");
    $("#GradeFilter").select2();

    $("#cmbHomeroomFilter").html('');
    $("#cmbHomeroomFilter").val("");
    $("#cmbHomeroomFilter").select2();

    if (loadtable) {
        setTimeout(foo, 3000);
        if (obeginningBalanceTable == undefined) {
            drawBeginningBalanceTable();
        }
        else {

            obeginningBalanceTable.fnDraw();
        }
    }
    else {
        //ograduateSeniorsTable.fnClearTable();
        //ograduateSeniorsTable.fnDraw();
        //ograduateSeniorsTable.fnDestroy();
        $('#beginningBalanceTable tbody').empty();
        $("#thTotalM").val("");
        $("#thTotalA").val("");
        $("#thTotalT").val("");
    }
}

function foo() {
    //some delay
}

function fetchGrades(schoolId, districtId) {
    var schooldata = JSON.stringify({
        schoolId: schoolId,
        districtId: districtId
    });
    $.ajax({
        type: "POST",
        url: "/BeginningBalance/GetDistinctGrades",
        data: schooldata,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            var htmlstring = "<option value=\"\"></option>";
            for (var i = 0; i < data.Data.length; i++) {
                htmlstring = htmlstring + "<option value=\"" + data.Data[i].GradeId + "\">" + data.Data[i].GradeName + "</option>";
            }
            $('#GradeFilter').html('');
            $('#GradeFilter').html(htmlstring);
            $("#GradeFilter").select2();
        },
        error: function (request, status, error) {
            displayErrorMessage('Error occurred.');
            return false;
        }
    });
}

function fetchSchools(districtId) {
    var districtdata = JSON.stringify({
        districtId: districtId
    });
    $.ajax({
        type: "POST",
        url: "/BeginningBalance/GetDistinctSchools",
        data: districtdata,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            var htmlstring = "<option value=\"\"></option>";
            for (var i = 0; i < data.Data.length; i++) {
                htmlstring = htmlstring + "<option value=\"" + data.Data[i].SchoolId + "\">" + data.Data[i].SchoolName + "</option>";
            }
            $('#SchoolFilter').html('');
            $('#SchoolFilter').html(htmlstring);
            $("#SchoolFilter").select2();
        },
        error: function (request, status, error) {
            displayErrorMessage('Error occurred.');
            return false;
        }
    });
}



function fetchHomeRoom(gradeId, schoolId, districtId) {
    var schooldata = JSON.stringify({
        schoolId: schoolId,
        gradeId: gradeId,
        districtId: districtId
    });
    $.ajax({
        type: "POST",
        url: "/BeginningBalance/GetDistinctHomeRoom",
        data: schooldata,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            var htmlstring = "<option value=\"\"></option>";
            for (var i = 0; i < data.Data.length; i++) {
                htmlstring = htmlstring + "<option value=\"" + data.Data[i].HomeRoomId + "\">" + data.Data[i].HomeRoomName + "</option>";
            }
            $('#cmbHomeroomFilter').html('');
            $('#cmbHomeroomFilter').html(htmlstring);
            $("#cmbHomeroomFilter").select2();
        },
        error: function (request, status, error) {
            displayErrorMessage('Error occurred.');
            return false;
        }
    });
}

//function initAmountTextBoxesfirst(txtBox, id) {
//    initAmountTextBoxes($('#' + id), id);
//}

function initAmountTextBoxes(txtBox, id) {
    InitializeAmount(txtBox, id);
}

function InitializeAmount(txtBox, id) {
    $(txtBox).blur(function () {
        //limit(this,id);
        $(this).formatCurrency({ colorize: true, negativeFormat: '-%s%n', roundToDecimalPlace: 2 });
    })
.keyup(function (e) {
    var e = window.event || e;
    var keyUnicode = e.charCode || e.keyCode;
    if (e !== undefined) {
        console.log('key up' + e.charCode + ' ' + e.keyCode);
        console.log('key up' + e.charCode || e.keyCode);
        switch (keyUnicode) {
            case 16: break; // Shift
            case 17: break; // Ctrl
            case 18: break; // Alt
            case 27: this.value = ''; break; // Esc: clear entry
            case 35: break; // End
            case 36: break; // Home
            case 37: break; // cursor left
            case 38: break; // cursor up
            case 39: break; // cursor right
            case 40: break; // cursor down
            case 78: break; // N (Opera 9.63+ maps the "." from the number key section to the "N" key too!) (See: http://unixpapa.com/js/key.html search for ". Del")
            case 110: break; // . number block (Opera 9.63+ maps the "." from the number block to the "N" key (78) !!!)
            case 190: break; // .
            default: $(this).formatCurrency({ colorize: true, negativeFormat: '-%s%n', roundToDecimalPlace: -1, eventOnDecimalsEntered: true });
        }
    }
    calculateTotal(id);
})
.keydown(function (e) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190, 109]) !== -1 ||
        // Allow: Ctrl+A
        (e.keyCode == 65 && e.ctrlKey === true) ||
        // Allow: home, end, left, right
        (e.keyCode >= 35 && e.keyCode <= 39)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }

});


}

function calculateTotal(id) {
    var idMealBalance = id + "txt" + "MB";
    var idAlaCarteBalance = id + "txt" + "ACB";
    var idTotalBalance = id + "txt" + "TB";

    var mbValue = parseFloat($("#" + idMealBalance).val().replace(/[^0-9-.]/g, ''));
    var acbValue = parseFloat($("#" + idAlaCarteBalance).val().replace(/[^0-9-.]/g, ''));
    var tbValue = mbValue + acbValue;

    if (isNaN(tbValue)) {
        tbValue = 0;
    }

    $("#" + idTotalBalance).val(tbValue);
    $("#" + idTotalBalance).formatCurrency({ colorize: true, negativeFormat: '-%s%n', roundToDecimalPlace: 2 });

    calculateGrandTotal();

    $('#btncomplete').removeAttr('disabled');
}


function calculateGrandTotal() {
    var totalMB = 0;
    var totalACB = 0;
    var totalTB = 0;

    $('.MB').each(function () {
        var value = this.value;

        value = parseFloat(value.replace(/[^0-9-.]/g, ''));
        totalMB = value + totalMB;
    });

    $('.ACB').each(function () {
        var value = this.value;

        value = parseFloat(value.replace(/[^0-9-.]/g, ''));
        totalACB = value + totalACB;
    });

    $('.TB').each(function () {
        var value = this.value;

        value = parseFloat(value.replace(/[^0-9-.]/g, ''));
        totalTB = value + totalTB;
    });

    if (isNaN(totalMB)) {
        totalMB = 0;
    }
    if (isNaN(totalACB)) {
        totalACB = 0;
    }
    if (isNaN(totalTB)) {
        totalTB = 0;
    }

    $("#thTotalM").val(totalMB);
    $("#thTotalA").val(totalACB);
    $("#thTotalT").val(totalTB);

    $("#thTotalM").formatCurrency({ colorize: true, negativeFormat: '-%s%n', roundToDecimalPlace: 2 })
    $("#thTotalA").formatCurrency({ colorize: true, negativeFormat: '-%s%n', roundToDecimalPlace: 2 })
    $("#thTotalT").formatCurrency({ colorize: true, negativeFormat: '-%s%n', roundToDecimalPlace: 2 })

}

function savePayments() {
    showConfirmDialog('The adjustments will be applied to the customer(s) account(s). Are you sure you want to proceed?', 'Yes', 'No', savePaymentsFinal, foo,'Alert!');
}

function savePaymentsFinal() {
    //CustomerId 
    //MPAmount 
    //ACAmount 

    var customerids = [];
    var dataArray = [];

    //get all the id for the customer to save payments
    $('.MB').each(function () {
        var value = this.value;
        customerids.push($(this).attr("data-id"));
    });
    console.log(customerids);

    //run the loop through all the ids to create objects
    if (customerids.length > 0) {
        for (var i = 0; i < customerids.length; i++) {

            var temp = new Object();

            var mbId = "#" + customerids[i] + "txt" + "MB";
            var acbId = "#" + customerids[i] + "txt" + "ACB";

            var mbValue = parseFloat(($(mbId).val()).replace(/[^0-9-.]/g, ''));
            var acbValue = parseFloat(($(acbId).val()).replace(/[^0-9-.]/g, ''));

            var mbBalanceValue = $(mbId).attr("data-MBalance")
            var acbBalanceValue = $(acbId).attr("data-AlaCarteBalance")

            var mbBalanceValueActual = $(mbId).attr("data-MBalanceActual")
            var acbBalanceValueActual = $(acbId).attr("data-AlaCarteBalanceActual")

            //so if the meal plan or alacarte changes
            //(ex. $0.00 MP to $5.00 MP)
            //(ex. $2.00 AC to $3.00 AC)
            //Do not create order record where there is no change
            //(ex. $0.00 MP to $0.00 MP)
            //(ex. $5.00 AC to $5.00 AC)

            if (mbValue != mbBalanceValueActual || acbValue != acbBalanceValueActual) {

                temp.MPAmount = 0;
                temp.ACAmount = 0;

                if (mbValue != mbBalanceValueActual) {
                    temp.CustomerId = customerids[i];
                    temp.MPAmount = mbValue - mbBalanceValue;
                    //temp.ACAmount = 0;//acbValue - acbBalanceValue;
                }
                if (acbValue != acbBalanceValueActual) {
                    temp.CustomerId = customerids[i];
                    //temp.MPAmount = 0;//mbValue - mbBalanceValue;
                    temp.ACAmount = acbValue - acbBalanceValue;
                }

                dataArray.push(temp);
            }
        }
        console.log(dataArray);
    }
    else {
        displayErrorMessage('Please apply the filter first, before saving the payments.');
        return;
    }

    //send object to api for saving payments
    if (dataArray.length > 0) {
        var data = JSON.stringify({
            olstBeginningBalancePaymentData: dataArray
        });

        $.ajax({
            type: "POST",
            url: "/BeginningBalance/SavePayment",
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                var res = data;
                if (res.result == '-1') {
                    displaySuccessMessage('Payments saved successfully.');
                    ClearBeginningBalanceFilters(false);
                }
                else {
                    displayErrorMessage('Error saving payments.');
                }
            },
            error: function (request, status, error) {
                displayErrorMessage('Error occurred.');
                return false;
            }
        });
    }
    else {
        displaySuccessMessage('Payments saved successfully.');
        ClearBeginningBalanceFilters(false);
    }
}