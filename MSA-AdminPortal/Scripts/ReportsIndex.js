
$(document).ready(function () {

    $('.datepicker').datepicker();
    $('#LocationList').select2();
    $('#LocationList').select2();
    $('#HomeRoomList').select2();
    $('#GradesList').select2();
    $('#CustomersList').select2();
    $('#AccountStatusList').select2();
    $('#QualificationList').select2();
    $('#FirstSortList').select2();
    $('#SecondSortList').select2();
    $('#ThirdSortList').select2();
    $('#FourthSortList').select2();
    $('#SortNameList').select2();
    $('#ZeroBalanceList').select2();
    $('#BATList').select2();
    $('#DateRangeTypesList').select2(); //Date Types (by farrukh m (allshore) on 05/10/16 (PA-519)
    $('#FormatNameList').select2({ placeholder: "No Selections", allowClear: true });
    $('#searchdll').select2();
    $('#searchtypedll').select2();
    $('#SessionTypeList').select2();
    $('#DepositTypeList').select2();
    $('#MsaSchool').select2();
    $('#SingleColumnSorting').select2();
    $('#AccountTypesList').select2();

    $('#processingItemListDiv').hide();
    $('#itemSelectionType').select2();
    $('#CategoryType').select2();
    $('#Category').select2();
    $('#itemList').multiSelect({ keepOrder: false });

    $('#rangeSliderDiv').hide();
    $('#LessThanDiv').slideUp(10);
    $('#GreaterThanDiv').slideUp(10);

    $('#PurchasedQtyList').select2();
    $('#qtyRangeSliderDiv').hide();
    $('#qtyLessThanDiv').slideUp(10);
    $('#qtyGreaterThanDiv').slideUp(10);

    //selectionHeader: "<div class='custom-header'>Order By</div>",
    $('#SortingColumnsList').multiSelect({ keepOrder: false });
    // $('.SlectBox').SumoSelect({ csvDispCount: 3, selectAll: true, captionFormatAllSelected: "All Selected" });
    //Added by Waqar Q.
    //Bug 1567
    $('.SlectBox').SumoSelect({ csvDispCount: 2, selectAll: false, captionFormatAllSelected: '{0} All selected!', search: true, isClickAwayOk: true });

    $('#RunReportBtn').click(function () {
        RunReport();
    })

    var selItems = document.getElementById('hdnFldSelectedValues');
    selItems.value = '';
    $('#uniform-allCustomers span').prop('class', 'checked');
    $('#allCustomers').prop('checked', 'checked');

    //
    // changed by farrukh m (allshore) on 05/06/16: from startDate: moment().subtract('days', 29) -> startDate: moment() to fix PA-509 
    $('#reportrange').daterangepicker({
        //opens: (Metronic.isRTL() ? 'left' : 'right'),
        //startDate: moment(),
        //endDate: moment(),
        //minDate: '01/01/2008',
        //maxDate: '12/30/' + (parseInt(moment().format('YYYY')) + 1),
        //dateLimit: {
        //    days: 90000
        //},
        //showDropdowns: true,
        //showWeekNumbers: true,
        //timePicker: false,
        //timePickerIncrement: 1,
        //timePicker12Hour: true,
        //ranges: {
        //    'Today': [moment(), moment()],
        //    'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
        //    'Last 7 Days': [moment().subtract('days', 6), moment()],
        //    'Last 30 Days': [moment().subtract('days', 29), moment()],
        //    'This Month': [moment().startOf('month'), moment().endOf('month')],
        //    'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
        //},
        //buttonClasses: ['btn'],
        //applyClass: 'green',
        //cancelClass: 'default',
        //format: 'MMM/DD/YYYY',
        //separator: ' to ',
        //locale: {
        //    applyLabel: 'Apply',
        //    fromLabel: 'From',
        //    toLabel: 'To',
        //    customRangeLabel: 'Custom Range',
        //    daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
        //    monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
        //    firstDay: 1
        //}

        //startDate: moment().subtract(29, 'days'),
        startDate: moment(),
        endDate: moment(),
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        buttonClasses: ['btn'],
        applyClass: 'green',
        cancelClass: 'default',
        showDropdowns: true,
        linkedCalendars:false

    },
function (start, end) {
    console.log("Callback has been called!");
   // debugger;
    $('#startDate').val(start);
    $('#EndDate').val(end);
    $('#reportrange span').html(start.format('MMM D, YYYY') + ' - ' + end.format('MMM D, YYYY'));
}
);
    //Set the initial state of the picker label
    // changed by farrukh m (allshore) on 05/06/16:  to fix PA-509
    //$('#reportrange span').html(moment().subtract('days', 29).format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
    //--------------------------------------------------------------------------------------------------
    // Added by farrukh m (allshore) on 05/06/16:  to fix PA-509 --------------------------
    $('#reportrange span').html(moment().format('MMM D, YYYY') + ' - ' + moment().format('MMM D, YYYY'));
    //---------------------------------------------------------------------
    
    $("#slider-range").slider({
        isRTL: Metronic.isRTL(),
        range: true,
        values: [0, 3000],
        slide: function (event, ui) {
            $("#range_slider_input_start").val("$" + ui.values[0]);

            $("#range_slider_input_end").val("$" + ui.values[1]);

            //$("#slider-range-amount").text("$" + ui.values[0] + " - $" + ui.values[1]);
        }
    });
    ///
    //range max

    $("#slider-range-max").slider({
        range: "max",
        min: 1,
        max: 3000,
        value: 2,
        slide: function (event, ui) {
            $("#slider_range_min_amount_input").val("$" + ui.value);
        }
    });

    $("#slider_range_max_amount_input").val("$" + $("#slider-range-max").slider("value"));

    // range min
    $("#slider-range-min").slider({
        range: "min",
        value: 100,
        min: 1,
        max: 3000,
        slide: function (event, ui) {
            $("#slider_range_min_amount_input").val("$" + ui.value);
        }
    });

    $("#slider_range_max_amount_input").val("$" + $("#slider-range-min").slider("value"));

    //------------------------------------------

    $("#qty-slider-range").slider({
        isRTL: Metronic.isRTL(),
        range: true,
        values: [0, 200],
        min: 1,
        max:200,
        slide: function (event, ui) {
            $("#qty_range_slider_input_start").val(ui.values[0]);

            $("#qty_range_slider_input_end").val(ui.values[1]);
        }
    });

    //range max
    $("#qty-slider-range-max").slider({
        range: "max",
        min: 1,
        max: 200,
        value: 2,
        slide: function (event, ui) {
            $("#qty_slider_range_min_amount_input").val(ui.value);
        }
    });

    $("#qty_slider_range_min_amount_input").val($("#qty-slider-range-max").slider("value"));


    // range min
    $("#qty-slider-range-min").slider({
        range: "min",
        value: 10,
        min: 1,
        max: 200,
        slide: function (event, ui) {
            $("#qty_slider_range_max_amount_input").val(ui.value);
        }
    });

    $("#qty_slider_range_max_amount_input").val($("#qty-slider-range-min").slider("value"));

    ////// customer search popup

    InitializeSearch();
    
    // commented by farrukh m (allshore) on 05/06/16:  to fix PA-509 --------------------------
    //$('#startDate').val(moment().subtract('days', 29));

    // Added by farrukh m (allshore) on 05/06/16:  to fix PA-509 --------------------------
    $('#startDate').val(moment());
    //-----------------------------------------------------------------------
    $('#EndDate').val(moment());


    $("#FirstSortList").on("change", function () {
        if ($("#FirstSortList").val().trim() === 'Customer Name') {
            $('#secondList').hide();
            $('#thirdList').hide();
            $('#fourthList').hide();
        } else {
            $('#secondList').show();
            $('#thirdList').show();
            $('#fourthList').show();
            $("#SecondSortList").change();
        }
    });

    $("#SecondSortList").on("change", function () {
        if ($("#SecondSortList").val().trim() === 'Customer Name') {
            $('#thirdList').hide();
            $('#fourthList').hide();
        } else {
            $('#thirdList').show();
            $('#fourthList').show();
            $("#ThirdSortList").change();
        }
    });

    $("#ThirdSortList").on("change", function () {
        if ($("#ThirdSortList").val().trim() === 'Customer Name') {
            $('#fourthList').hide();
        } else {
            $('#fourthList').show();
        }
    });

    $("#btnClose").on("click", function () {

        //$("#reportsDiv").html('');
        //$("#exportDiv").hide();
        
        CloseReport();


    });

    $('#select-all').click(function () {
        $('#itemList').multiSelect('select_all');
        return false;
    });

    $('#deselect-all').click(function () {
        $('#itemList').multiSelect('deselect_all');
        return false;
    });
    
    //$("[id*=AccountStatusList] option[value='']").each(function () {
    //    $(this).attr("disabled", "true");
    //    $(this).css("color", "Red");
    //    $(this).css("font-weight", "bolder");
    //});
});//document ready function

function CloseReport()
{
    //var viewerDiv = $("#reportsIframe").contents().find("#viewerContainer");
    //var numPages = $("#reportsIframe").contents().find("#numPages");
    //viewerDiv.html('');
    //numPages.html('of 1');
    //$("#exportDiv").hide();

    $("#reportsDiv").html('');
    $("#exportDiv").hide();
    $("#closeDiv").hide();

}

function StringifyRptFilters() {

   // debugger;
    var fromdate = moment(document.getElementById("startDate").value);
    var todate = moment(document.getElementById("EndDate").value); //.set({ hour: 0, minute: 0, second:  0 }).toDate();
  
    var SelectedCustomersList = document.getElementById('hdnFldSelectedValues');
    //var location = getSelectedItemsList('locationListDiv'); //$("#LocationList").val();
    //Added by Waqar Q. 1567
    var location = getSelectedItemsList('LocationListId');

    var aCustomers = $('#allCustomers').is(':checked');
    var spCustomers = $('#specificCustomers').is(':checked');

    //var homeRoom = getSelectedItemsList('HoomeroomDiv');  //$("#HomeRoomList").val();
    //Added by Waqar Q. 1567
    var homeRoom = getSelectedItemsList('HoomeroomListId');

    //var grade = getSelectedItemsList('gradesDiv');  //$("#GradesList").val();
    //Added by Waqar Q. 1567
    var grade = getSelectedItemsList('GradesListId');

    var accountStatus = $("#AccountStatusList").val() || [];
    var qualificationTypes = $("#QualificationList").val() || [];
    var balanceActTypes = $("#BATList").val(); 
    var dateRangeTypes = $("#DateRangeTypesList").val(); //Date Types (by farrukh m (allshore) on 05/10/16 (PA-519)
    var accountype = $("#AccountTypesList").val();

    //var sortOrder1 = $("#FirstSortList").val();
    //var sortOrder2 = $("#SecondSortList").val();
    //var sortOrder3 = $("#ThirdSortList").val();
    //var sortOrder4 = $("#FourthSortList").val();

    var multipleValues = $("#SortingColumnsList").val() || [];
    var sortingColumnsList = multipleValues.join(", ");

    var formatName = $("#FormatNameList").val() || [];


    var SessionTypeList = $("#SessionTypeList").val();
    var DepositTypeList = $("#DepositTypeList").val();
    var MsaSchool = $("#MsaSchool").val();
    var MsaSchoolMultiSelect = "";
    $('#MsaSchoolMultiSelect option:selected').each(
        function () {
            MsaSchoolMultiSelect = MsaSchoolMultiSelect + $(this).val() + ",";
        });
    if (MsaSchoolMultiSelect != "") {
        MsaSchoolMultiSelect = removeLastComma(MsaSchoolMultiSelect);
    }
    else {
        MsaSchoolMultiSelect = "-1";
    }
    var Date = $("#DateMsa").val();
    var DateEnd = $("#DateEnd").val();
    var IncludeSchool = $('#IncludeSchool').is(":checked");
    var SingleColumnSorting = $("#SingleColumnSorting").val();

    var range_slider_input_start = $("#range_slider_input_start").val();
    var range_slider_input_end = $("#range_slider_input_end").val();
    var slider_range_max_amount = $("#slider_range_max_amount_input").val();
    var slider_range_min_amount = $("#slider_range_min_amount_input").val();

    var selectedQtyType = $("#PurchasedQtyList").val();
    var qty_range_slider_input_start = $("#qty_range_slider_input_start").val();
    var qty_range_slider_input_end = $("#qty_range_slider_input_end").val();
    var qty_slider_range_max_amount = $("#qty_slider_range_max_amount_input").val();
    var qty_slider_range_min_amount = $("#qty_slider_range_min_amount_input").val();
    var insertPageBreak = $('#PagebreakCheckbox').is(":checked");

    var minQty = null;
    var maxQty = null;

    if (selectedQtyType == "Range") {
        minQty = qty_range_slider_input_start;
        maxQty = qty_range_slider_input_end;
    }
    else if (selectedQtyType == "Less Than") {
        maxQty = qty_slider_range_max_amount;
    }
    else if (selectedQtyType == "Greater Than") {
        minQty = qty_slider_range_min_amount;
    }

    //Item Filters
    var itemSelectionType = $('#itemSelectionType').val();
    var selectedTypeList = $('#itemList').val();
    //selectedTypeList = selectedTypeList.length > 1 ? selectedTypeList.join(',') : selectedTypeList[0];


    var ReportsFilters = new Object();
    // Assigning values to the properties

    ReportsFilters.fromDate = fromdate.format("MM/DD/YYYY");
    ReportsFilters.toDate = todate.format("MM/DD/YYYY 23:59:59.000");

    ReportsFilters.SelectedCustomersList = SelectedCustomersList.value;
    ReportsFilters.location = location;
    ReportsFilters.homeRoom = homeRoom;
    ReportsFilters.grade = grade;
    ReportsFilters.accountStatus = accountStatus;
    ReportsFilters.qualificationTypes = qualificationTypes;
    ReportsFilters.balanceActTypes = balanceActTypes; 
    ReportsFilters.dateRangeTypes = dateRangeTypes; //Date Types (by farrukh m (allshore) on 05/10/16 (PA-519)
    

    

    ReportsFilters.sortOrder = sortingColumnsList;

    //ReportsFilters.sortOrder2 = sortOrder2;
    //ReportsFilters.sortOrder3 = sortOrder3;
    //ReportsFilters.sortOrder4 = sortOrder4;

    ReportsFilters.formatName = formatName;
    ReportsFilters.allCustomers = aCustomers;
    ReportsFilters.specificCustomers = spCustomers;
    ReportsFilters.DepositTypeList = DepositTypeList;
    ReportsFilters.MsaSchool = MsaSchool;
    ReportsFilters.MsaSchoolMultiSelect = MsaSchoolMultiSelect;
    ReportsFilters.Date = Date;
    ReportsFilters.DateEnd = DateEnd; 
    ReportsFilters.IncludeSchool = IncludeSchool; 
    ReportsFilters.SingleColumnSorting = SingleColumnSorting;
    ReportsFilters.SessionTypeList = SessionTypeList;

    ReportsFilters.accountype = accountype;
    ReportsFilters.range_slider_input_start = range_slider_input_start;
    ReportsFilters.range_slider_input_end = range_slider_input_end;
    ReportsFilters.slider_range_max_amount = slider_range_max_amount;
    ReportsFilters.slider_range_min_amount = slider_range_min_amount;

    ReportsFilters.selectedQtyType = selectedQtyType;
    ReportsFilters.minQty = minQty;
    ReportsFilters.maxQty = maxQty;

    ReportsFilters.itemSelectionType = itemSelectionType;
    ReportsFilters.selectedTypeList = selectedTypeList;
    ReportsFilters.insertPageBreak = insertPageBreak;

    var data = JSON.stringify(ReportsFilters);
    return data;

}

function removeLastComma(str) {
    return str.replace(/,$/, "");
}

function getQueryStrings() {
    var assoc = {};
    var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
    var queryString = location.search.substring(1);
    var keyValues = queryString.split('&');

    for (var i in keyValues) {
        var key = keyValues[i].split('=');
        if (key.length > 1) {
            assoc[decode(key[0])] = decode(key[1]);
        }
    }

    return assoc;
}

function RunReport() {
    //debugger;
    dataString = StringifyRptFilters();
    var qs = getQueryStrings();
    var idval = qs["id"];
    $('#loadingSpinnerimg').show();
    $.ajax({
        url: '/Reports/ShowReport',
        data: { 'filterData': dataString, 'id': idval }, //dataString,
        type: 'GET',
        success: function (response) {
            //debugger;
            if (response.result == "-2") {
                displayErrorMessage('Error occurred during display of report.');
            }
            else if (response.result == "-3") {
                displayErrorMessage('This report is not added in the system yet.');
            }
            else {
                $('#reportsDiv').load('Reports/RefreshReport?id=0');

                $('#exportDiv').show();
                $('#closeDiv').show();
                //var rand = Math.floor((Math.random() * 1000000) + 1);
                //var iframe = document.getElementById('reportsIframe');
                //iframe.src = "/reports/ReportView?uid=" + rand;

            }
        },
        complete: function () {
            $('#loadingSpinnerimg').hide()
        },
        error: function (xhr, status, error) {
            displayErrorMessage('Error during display of report');
        }
    });
}

function showHideRangeSlider() {
    var bat = $('#AccountTypesList').val();
    if (bat == "Range") {
        $('#rangeSliderDiv').slideDown(10);
        $('#LessThanDiv').slideUp(10);
        $('#GreaterThanDiv').slideUp(10);
    }
    else if (bat == "Less Than") {
        $('#rangeSliderDiv').slideUp(10);
        $('#GreaterThanDiv').slideUp(10);
        $('#LessThanDiv').slideDown(10);


    }
    else if (bat == "Greater Than") {
        $('#rangeSliderDiv').slideUp(10);
        $('#GreaterThanDiv').slideDown(10);
        $('#LessThanDiv').slideUp(10);
    }
    else {
        $('#rangeSliderDiv').slideUp(10);
        $('#GreaterThanDiv').slideUp(10);
        $('#LessThanDiv').slideUp(10);
    }
}

function showHideQtyRangeSlider() {
    var bat = $('#PurchasedQtyList').val();
    if (bat == "Range") {
        $('#qtyRangeSliderDiv').slideDown(10);
        $('#qtyLessThanDiv').slideUp(10);
        $('#qtyGreaterThanDiv').slideUp(10);
    }
    else if (bat == "Less Than") {
        $('#qtyRangeSliderDiv').slideUp(10);
        $('#qtyGreaterThanDiv').slideUp(10);
        $('#qtyLessThanDiv').slideDown(10);
    }
    else if (bat == "Greater Than") {
        $('#qtyRangeSliderDiv').slideUp(10);
        $('#qtyGreaterThanDiv').slideDown(10);
        $('#qtyLessThanDiv').slideUp(10);
    }
    else {
        $('#qtyRangeSliderDiv').slideUp(10);
        $('#qtyGreaterThanDiv').slideUp(10);
        $('#qtyLessThanDiv').slideUp(10);
    }
}

$(".exportClick").click(function () {


    var exportButtonID = this.id;
    var exportFormat = 'PortableDocFormat';
    var firstExe = true;

    switch (exportButtonID) {
        case 'btnExportToExcel':
            exportFormat = 'Excel';
            break;
        case 'btnExportToPDF':
            exportFormat = 'PortableDocFormat';
            break;
        case 'btnExportToWord':
            exportFormat = 'WordForWindows';
            break;
        case 'btnExportToCSV':
            exportFormat = 'CharacterSeparatedValues';
            break;
        default:
            exportFormat = 'PortableDocFormat';
    }


    $.ajax({
        type: 'GET',
        url: '/Reports/GetExcel',
        data: { 'ExportFormat': exportFormat, 'FirstExe': firstExe },
        success: function (response) {
            //debugger;
            if (response.result == "-2") {
                displayErrorMessage('Error occurred during display of report.');
            }
            else if (response.result == "-3") {
                displayErrorMessage('This report is not added in the system yet.');
            }
            else {
                firstExe = false;
                window.location = '/Reports/GetExcel/?exportFormat=' + exportFormat + '&firstExe=' + firstExe;

            }
        },
        error: function (xhr, status, error) {
            displayErrorMessage('Error during display of report');
        }
    });

});

/* commented
Waqar Q Bug 1567
function getSelectedItemsList(divid) {
    //debugger;
    var retStr = "";
    var targetDiv = '#' + divid + ' input:checked';
    var selectedList = [];

    //$("#locationListDiv input:checked").each(function () {

    $(targetDiv).each(function () {
        selectedList.push($(this).attr('id'));
    });

    if (selectedList.length > 0) {
        retStr = selectedList.join(',');
    }

    return retStr;

}
*/
//Added
//Waqar Q Bug 1567
function getSelectedItemsList(MultiSelectID) {
    // debugger;
    var retStr = "";
    var targetDDL = '#' + MultiSelectID + '  option:selected';
    var selectedList = [];

    //$("#locationListDiv input:checked").each(function () {

    $(targetDDL).each(function () {
        selectedList.push($(this).val());
    });

    if (selectedList.length > 0) {
        retStr = selectedList.join(',');
    }

    return retStr;

}

function getSelectedItemsArray(divid) {
    var targetDiv = '#' + divid + ' input:checked';
    var selectedList = [];

    $(targetDiv).each(function () {
        selectedList.push($(this).attr('id'));
    });

    return selectedList;

}

function checkChecked(topDiv, innerSpan, displayText) {
    //debugger;
    var checkboxesList = '';
    checkboxesList = getSelectedItemsList(topDiv);
    var targetSpan = '#' + innerSpan;
    if (checkboxesList.length > 0) {
        $(targetSpan).text(" One or many selected");
        $(targetSpan).addClass("fa fa-check-square fa-1");
    } else {
        $(targetSpan).text(displayText);
        $(targetSpan).removeClass("fa fa-check-square fa-1");
    }


}

// Selection Type Dropdown
$('#itemSelectionType').change(function (e) {

    var selectionType = $('#itemSelectionType').val();
    //debugger;
    switch (selectionType) {

        case "1":
            $('#CategoryTypeDiv').hide();
            $('#CategoryDiv').hide();
            break;
        case "2":
            $('#CategoryDiv').hide();
            $('#CategoryTypeDiv').show();
            resetDefaultOption('#CategoryType');
            break;
        case "3":
            $('#CategoryTypeDiv').show();
            $('#CategoryDiv').show();
            resetDefaultOption('#CategoryType');
            resetDefaultOption('#Category');
            break;
    }
    LoadItemDropdownData('#itemList');
});

//On Category Type Selection
$('#CategoryType').change(function (e) {
    //debugger;
    var selectionType = $('#itemSelectionType').val();

    if (selectionType == '3') {
        LoadItemDropdownData('#Category');
    }

    LoadItemDropdownData('#itemList');
});

//On Category Selection
$('#Category').change(function (e) {

    LoadItemDropdownData('#itemList');
});

function resetDefaultOption(dropdownID) {

    if (dropdownID == '#Category') {
        $(dropdownID).find('option:not(:first)').remove();
    }

    $(dropdownID).val("");
    $(dropdownID).select2('destroy');
    $(dropdownID).select2();
}

//Load Item Filter Dropdowns Data (Category Type/Category/ Item) 
function LoadItemDropdownData(elementID) {

    if (elementID == '#itemList') $('#processingItemListDiv').show();

    var selectionType = elementID == '#Category' ? '2' : $('#itemSelectionType').val();

    fitlerObject = { 'selectionType': selectionType, 'categoryType': $('#CategoryType').val(), 'category': $('#Category').val() };

    var filterStringifyObject = JSON.stringify(fitlerObject);

    $.ajax({
        url: '/Reports/GetRptFiltersDropdownData',
        data: { 'filterData': filterStringifyObject }, //dataString,
        type: 'GET',
        success: function (res) {
            $('#processingItemListDiv').hide();

            var data = res;
            $(elementID + ' option').remove();
            if (elementID == '#Category') {
                var option = '<option selected value> All </option>';
                $(elementID).append(option);

            }

            $(data).each(function () {

                var option = '<option value=' + this.Value + '>' + this.Text + '</option>';
                $(elementID).append(option);


            });

            if (elementID == '#itemList') {
                $(elementID).multiSelect('destroy');
                $(elementID).multiSelect({ keepOrder: false });

            }
            else {

                $(elementID).select2('destroy');
                $(elementID).select2();
            }


        },
        complete: function () {

        },
        error: function (xhr, status, error) {
            $('#processingItemListDiv').hide();
            displayErrorMessage('Error during retrieving Data:' + error);
        }
    });
}
