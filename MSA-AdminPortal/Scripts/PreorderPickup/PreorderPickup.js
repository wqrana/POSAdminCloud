/*
-- Author:		Waqar Q.
-- Create date: 2017-04-04
-- Description:	Java Script file for Preorder Pickup functionality

-- Modification History     

*/

var selectedPreorderItemValues      = [];
var selectedPreorderAllItemValues   = null;
var selectedPreorderValues = [];
var objLoadDataTable = null;
var currentDataRows = null;
var confirmationMsgType = null;
var reportOfAllItems = true;
$(document).ready(function () {
    



    $('.datepicker').datepicker();
    // $('#LocationList').select2();
  
    $('#HomeRoomList').select2();
    $('#GradesList').select2();
    $('#CustomersList').select2();
    $('#DateRangeTypesList').select2(); 
    $('#ItemStatus').select2();
    $('#itemSelectionType').select2();
    $('#CategoryType').select2();
    $('#Category').select2();
    $('#itemList').multiSelect({ keepOrder: false });
    $('#processingItemListDiv').hide();

    $('.SlectBox').SumoSelect({ csvDispCount: 2, selectAll: false, captionFormatAllSelected: '{0} All selected!', search: true, isClickAwayOk: true, okCancelInMulti: true });
    //Waqar Q. Bug 1539
  //  $('.SlectBox').SumoSelect({ csvDispCount: 2, selectAll: false, captionFormatAllSelected: '{0} All selected!', search: true, isClickAwayOk: true});
 //   $('#reporDiv').hide();
    //All Select/De-Select
    $('#select-all').click(function(){
        $('#itemList').multiSelect('select_all');
        return false;
    });

    $("#VoidBtnID").on('click', function () {
      


        ShowVoidPopup();
    });

    //reload table  
    $('#refresh').click(function () {
       // debugger;
        var table = $('#LoadDataTable').DataTable();
        table.ajax.reload(null, false);
    });

    $('#deselect-all').click(function(){
        $('#itemList').multiSelect('deselect_all');
        return false;
    });
   
    
    $('#SelectAll').click(checkUnCheckAll);

   
    $('#LoadDataTable tbody').on('change', 'input[type="checkbox"]', function (e) {
        //debugger;
        var state = this.checked;
        var id = $(this).attr('id');
         setPreorderList(id, state);

         //SelectAllFooter
       
        
    });


   // $('#DataTableDiv').hide();

    $('#settingSection').click(function () {
        //debugger;

        switch ($('#settingSection').html()) {

            case 'Hide Filters':
                $('#filterPanel').hide();
                $('#contentPanel').removeClass('col-md-8').addClass('col-md-12');
                $('#settingSection').html('Show Filters')
                break;
            case 'Show Filters':
                $('#filterPanel').show();
                $('#contentPanel').removeClass('col-md-12').addClass('col-md-8');
                $('#settingSection').html('Hide Filters')
                break;

        }

    });


    $('#LoadDataBtn').click(function () {
        //var filter = StringifyPreorderFilters()
        //debugger;
        var selectedList = $('#itemList').val();
        if (selectedList == null || selectedList == "") {
           
            //alert("Please Select " + $('#itemSelectionType option:selected').text() + " List");
            var message = "Please Select " + $('#itemSelectionType option:selected').text() + " List";
            displayWarningMessage(message);
            return;

        }
        selectedPreorderItemValues = [];
        selectedPreorderValues = [];
        //Call to load data
        LoadData();
    });

    $('#ConfirmOk').click(function () {
       // debugger;
        if (confirmationMsgType == 'report') {
            reportOfAllItems = false;
            RunPreorderPickupReport();
        }
        $('#ConfirmModalDiv').modal('hide');

    });

    $('#ConfirmNo').click(function () {
        // debugger;
        if (confirmationMsgType == 'report') {
            reportOfAllItems = true;
            RunPreorderPickupReport();
        }
        $('#ConfirmModalDiv').modal('hide');

    });

      $('#ConfirmModalDiv').on('hidden.bs.modal', function () {
        //debugger;
        //if (confirmationMsgType == 'report') {
        //    RunPreorderPickupReport();
                       
        //}
        confirmationMsgType = null;


     });


    $('#RunReportBtn').click(function () {

        $('#confirmationMsgDiv').html('Would you like to generate report of selected item(s)?');
        confirmationMsgType = 'report';
        reportOfAllItems = true;
        $('#ConfirmModalDiv').modal('toggle');


     // RunPreorderPickupReport();
        
    });
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
            url: '/PreorderPickupReport/GetExcel',
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
                    window.location = '/PreorderPickupReport/GetExcel/?exportFormat=' + exportFormat + '&firstExe=' + firstExe;

                }
            },
            error: function (xhr, status, error) {
                displayErrorMessage('Error during display of report');
            }
        });

    });



    $('#closeDiv').click(function () {
        $('#DataTableDiv').show();
        $('#reporDiv').hide();
        $('#LoadDataBtn').prop('disabled', false);

    });

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


    var selItems = document.getElementById('hdnFldSelectedValues');
    selItems.value = '';
    $('#uniform-allCustomers span').prop('class', 'checked');
    $('#allCustomers').prop('checked', 'checked');

    //
    // Date Pickup Handling 
    /* implementation with old version @1.3.13
    $('#loaddatarange').daterangepicker({
        opens: (Metronic.isRTL() ? 'left' : 'right'),
        startDate: moment(),
        endDate: moment(),
        minDate: '01/01/2008',
        maxDate: '12/30/' + (parseInt(moment().format('YYYY')) + 1),
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
            'This Week (5 Days)': [moment().startOf('week').subtract(-1, 'days'), moment().endOf('week').subtract(1, 'days')],
            'This Week (7 Days)': [moment().startOf('week'), moment().endOf('week')],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'This Year (Fiscal)': [moment(), moment().subtract(1,'days')]
           
        },
      
        buttonClasses: ['btn'],
        applyClass: 'green',
        cancelClass: 'default',
        format: 'MMM/DD/YYYY',
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
    function (start, end, label) {
        console.log("Callback has been called!");
        console.log(label);

        if (label == 'This Year (Fiscal)') {
            var dateObject = new Date(start);
            console.log(dateObject);
            var startYear, endYear;
            if (dateObject.getMonth() < 6) {
                     
                startYear=endYear = dateObject.getFullYear();
                startYear--;                   
            }
            else
            {

                startYear = endYear = dateObject.getFullYear();
                endYear++;
            }

            start._d = new Date(startYear, 6, 1, 23, 1, 1, 100);
            end._d = new Date(endYear, 5, 30, 23, 1, 1, 100);

        }
          $('#startDate').val(start);
    $('#EndDate').val(end);
    $('#loaddatarange span').html(start.format('MMM D, YYYY') + ' - ' + end.format('MMM D, YYYY'));
    });
       $('#loaddatarange span').html(moment().format('MMM D, YYYY') + ' - ' + moment().format('MMM D, YYYY'));

  */
    
    $('#loaddatarange').daterangepicker({
        startDate: moment(),
        endDate: moment(),
        ranges: {
            'Today': [moment(), moment()],
            'This Week (5 Days)': [moment().startOf('week').subtract(-1, 'days'), moment().endOf('week').subtract(1, 'days')],
            'This Week (7 Days)': [moment().startOf('week'), moment().endOf('week')],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'This Year (Fiscal)': [moment(), moment().subtract(1,'days')]
        },
        buttonClasses: ['btn'],
        applyClass: 'green',
        cancelClass: 'default',
        showDropdowns: true,
        linkedCalendars: false

    },
    function (start, end, label) {
   
    if (label == 'This Year (Fiscal)') {
        var dateObject = new Date(start);

        var startYear, endYear;
        if (dateObject.getMonth() < 6) {

            startYear = endYear = dateObject.getFullYear();
            startYear--;
        }
        else {

            startYear = endYear = dateObject.getFullYear();
            endYear++;
        }

        start._d = new Date(startYear, 6, 1, 23, 1, 1, 100);
        end._d = new Date(endYear, 5, 30, 23, 1, 1, 100);
    }

    debugger;
    $('#startDate').val(start);
    $('#EndDate').val(end);
    $('#loaddatarange span').html(start.format('MMM D, YYYY') + ' - ' + end.format('MMM D, YYYY'));
    });

    // For First time init
    //Bug Fixed: 1851
    $('#startDate').val(moment());
    $('#EndDate').val(moment());
    $('#loaddatarange span').html(moment().format('MMM D, YYYY') + ' - ' + moment().format('MMM D, YYYY'));
 
    
    InitializeSearch();
            
});//End of document ready function


//Filter Setting for data retrieval
function PreorderFiltersData() {
   // debugger;
    //Location Filter
    // var location = getSelectedItemsList('locationListDiv');
    var location = getSelectedItemsList('LocationListId');
    //Date Range Type
    // var dateRangeTypes = getSelectedItemsList('DateRangeTypesListDiv');
    var dateRangeTypes = getSelectedItemsList('DateRangeTypesListId');
   
    //Date Range from and to
    var fromdate        = moment(document.getElementById("startDate").value);
    var todate          = moment(document.getElementById("EndDate").value);
    //Homeroom filter
    //var homeRoom = getSelectedItemsList('HoomeroomDiv');
    var homeRoom = getSelectedItemsList('HoomeroomListId');
    
    //Grade Filter
    var grade = getSelectedItemsList('GradesListId');
    // Customer Filter
    var aCustomers = $('#allCustomers').is(':checked');
    var spCustomers = $('#specificCustomers').is(':checked');
    var SelectedCustomersList =  document.getElementById('hdnFldSelectedValues');
    //Item Filters
    var itemStatus = $('#ItemStatus').val();
    var itemSelectionType = $('#itemSelectionType').val();
    var selectedTypeList = $('#itemList').val();
    var selectedTypeList = selectedTypeList.length > 1 ? selectedTypeList.join(',') : selectedTypeList[0];

    // Assigning values to the filter object properties    
    var PreorderFilters = {};
   //Location
    PreorderFilters.location = location;
    //Date Range
    PreorderFilters.dateRangeTypes = dateRangeTypes;
    PreorderFilters.fromDate = fromdate.format("MM/DD/YYYY");
    PreorderFilters.toDate = todate.format("MM/DD/YYYY 23:59:59.000");
    //Homeroom
    PreorderFilters.homeRoom = homeRoom;
    //Custumer
    PreorderFilters.customerSelectionType = aCustomers == true ? 'A' : 'S';
    PreorderFilters.selectedCustomersList = SelectedCustomersList.value;
   //grade    
    PreorderFilters.grade = grade;
    //item
    PreorderFilters.itemSelectionType = itemSelectionType;
    PreorderFilters.itemStatusType = itemStatus ;
    PreorderFilters.selectedTypeList = selectedTypeList;
    //Object into json format
   // var data = JSON.stringify(PreorderFilters);
    //return data;
    return PreorderFilters;
}
//Get Preorder pickup items count
function GetPreorderPickupItemsCount() {

    var fitlerObject = PreorderFiltersData();
    
    var filterStringifyObject = JSON.stringify(fitlerObject);

    $.ajax({
        url: '/PreorderPickup/GetPreorderPickupItemsCount',
        data: { 'filterData': filterStringifyObject }, //dataString,
        type: 'GET',
        success: function (res) {
          //  debugger;
           selectedPreorderAllItemValues = res.recordIdStr;
          
        },
        complete: function () {

        },
        error: function (xhr, status, error) {
            displayErrorMessage('Error during retrieving Data:' + error);
        }
    });


}


//Load Item Filter Dropdowns Data (Category Type/Category/ Item) 
function LoadItemDropdownData(elementID) {
   
    if (elementID == '#itemList') $('#processingItemListDiv').show();
     
    var selectionType = elementID == '#Category' ? '2' : $('#itemSelectionType').val();

    fitlerObject = { 'selectionType': selectionType, 'categoryType': $('#CategoryType').val(), 'category': $('#Category').val() };

    var filterStringifyObject = JSON.stringify(fitlerObject);
    
    $.ajax({
        url: '/PreorderPickup/GetDropdownData',
        data: { 'filterData': filterStringifyObject }, //dataString,
        type: 'GET',
        success: function (res) {
            $('#processingItemListDiv').hide();

            var data = res;        
            $(elementID+' option').remove();
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
            displayErrorMessage('Error during retrieving Data:' + error);
        }
    });

}

//Load Preorder Data
function LoadData() {
   // debugger;
    var filterData = PreorderFiltersData();
    objLoadDataTable = null;
    if ($.fn.DataTable.isDataTable('#LoadDataTable')){

        $('#LoadDataTable').dataTable().fnDestroy();
        }

    $('#DataTableDiv').show();

    GetPreorderPickupItemsCount();

       objLoadDataTable = $('#LoadDataTable').dataTable({
           "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-5 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
           "aLengthMenu": [
               [25, 50, 100,-1],
               [25, 50, 100, 'All'] // change per page values here
           ],
           "ajax": "data.json",
           "bProcessing": true,
           "bServerSide": true,
           "cache": true,
           "bDestroy": true,
           "bAutoWidth": false,
           "bScrollAutoCss": true,
           "sScrollXInner": "110%",
         
          
           "sAjaxSource": "/PreorderPickup/LoadPreorderPickupData",
           // set the initial value
           "iDisplayLength": 25,

           "fnInitComplete":
               function (oSettings, json) {
                  // $('.dataTable').wrap('<div class="dataTables_scroll" />');
                   //debugger;
                   //alert('DataTables has finished its initialisation.' + json.status);
                   $('<div style="overflow: auto"></div>').append($('#LoadDataTable')).insertAfter($('#LoadDataTable' + '_wrapper div').first());
                   //Bug 1538
                   $('#thSelectAll').removeClass('sorting_asc');
                  },

           "sPaginationType": "bootstrap_full_number",

           "fnServerData":
               function (sSource, aoData, fnCallback) {
                   aoData.push(

                   { "name": 'location', "value": filterData.location },
                   { "name": 'dateRangeTypes', "value": filterData.dateRangeTypes },
                   { "name": 'fromDate', "value": filterData.fromDate },
                   { "name": 'toDate', "value": filterData.toDate },
                   { "name": 'grade', "value": filterData.grade },
                   { "name": 'homeRoom', "value": filterData.homeRoom },
                   { "name": 'customerSelectionType', "value": filterData.customerSelectionType },
                   { "name": 'SelectedCustomersList', "value": filterData.selectedCustomersList },
                   { "name": 'itemSelectionType', "value": filterData.itemSelectionType },
                   { "name": 'itemStatusType', "value": filterData.itemStatusType },
                   { "name": 'selectedTypeList', "value": filterData.selectedTypeList }

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
              
               $('#filterPanel').hide();
               $('#contentPanel').removeClass('col-md-8').addClass('col-md-12');
               $('#settingSection').html('Show Filters');
                var table = new $.fn.dataTable.Api('#LoadDataTable');
                currentDataRows = table.rows().data();
               
               //checkUnCheckAll();
                restorePickupCheckBoxes(this);
               // disableEditDeleteLinks();
           },
           "oLanguage": {
               "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
               "sInfoEmpty": 'No Preorder record found',
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
                          mData: "PreOrderItemId",
                          sType: "numeric",
                          aTargets: [0],
                          bSortable: false,
                          sClass  : 'centered-cell' ,
                          mRender: function (data, type) {
                            
                              return '<div style="text-align: center"><input type="checkbox" id="' + data + '" /></div>';
                          }

                      },
                      {
                          
                          mData: "preOrderId",
                          sType: "numeric",
                          aTargets: [1],
                          visible: false
                      },
                      {
                          mData: "transactionId",
                          sType: "numeric",
                          aTargets: [2]
                      },


                       {
                           mData: "Grade",
                           sType: "string",
                           aTargets: [3]
                       },
                       {
                           mData: "customerName",
                           sType: "string",
                           aTargets: [4]
                       },

                       {
                           mData: "userId",
                           sType: "string",
                           aTargets: [5]

                       },

                       {
                           mData: "itemName",
                           sType: "string",
                           aTargets: [6]

                       },
                        {
                            mData: "datePurchased",
                            sType: "string",
                           // format: 'MM/DD/YYYY',
                            aTargets: [7],
                            sClass   : 'centered-cell',
                            mRender: function (data, type) {
                                debugger;
                                if (data === null) return "";

                              //  var pattern = /Date\(([^)]+)\)/;
                               // var results = pattern.exec(data);
                                //var dt = new Date(parseFloat(results[1]));
                              //  var dt = new Date(parseInt(results[1], 10));


                                //  return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                                // Bug 1548 Waqar Q.
                                //var utcDate = moment.utc(dt.toUTCString());
                                //var dateStr = utcDate.toLocaleString([], { hour12: true });
                                // var dateStr = dt.toLocaleString([], { hour12: true });
                             //   var dateStr = formatDate(dt,'dt');
                                //return (dt.getUTCMonth() + 1) + "/" + dt.getUTCDate() + "/" + dt.getUTCFullYear();
                                return data;
                            }
                        },

                       {
                           mData: "dateToServe",
                           sType: "string",
                           aTargets: [8],
                           sClass: 'centered-cell',

                           mRender: function (data, type) {
                               debugger;
                           if (data === null) return "";

                          // var pattern = /Date\(([^)]+)\)/;
                          // var results = pattern.exec(data);
                          // var dt = new Date(parseFloat(results[1]));

                           // return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                           // Bug 1548 Waqar Q.
                          // var dateStr = formatDate(dt, 'd');
                               //return (dt.getUTCMonth() + 1) + "/" + dt.getUTCDate() + "/" + dt.getUTCFullYear();
                           return data;
                       }

                       },
                       {
                           mData: "datePickedUp",
                           sType: "string",
                           aTargets: [9],
                           sClass: 'centered-cell',
                           mRender: function (data, type) {

                               if (data === null) return "";

                             //  var pattern = /Date\(([^)]+)\)/;
                             //  var results = pattern.exec(data);
                              // var dt = new Date(parseFloat(results[1]));

                               //return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                               // Bug 1548 Waqar Q.
                               //var dateStr = dt.toLocaleString([], { hour12: true });
                              // var dateStr = formatDate(dt, 'dt');
                               return data;
                               //return (dt.getUTCMonth() + 1) + "/" + dt.getUTCDate() + "/" + dt.getUTCFullYear();
                           }

                       },

                       {
                           mData: "qty",
                           sType: "numeric",
                           aTargets: [10]

                       },

                       {
                           "mData": "received",
                           "sType": "numeric",
                           aTargets: [11]

                       },
                       {
                           "mData": "void",
                           "sType": "string",
                           aTargets: [12],
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

                       }

           ]

        
       });   
     
    
}
// Work Check all and un-check all
function checkUnCheckAll() {
    //debugger;
    var state = $('#SelectAll').is(":checked");
   
    $('#LoadDataTable tbody').find(':checkbox').each(function () {

        $(this).prop('checked', state);
        var id = $(this).attr('id');
        if (id != 'SelectAll') {
            setPreorderList(id, state);
        }
    });
}

function setPreorderList(id, state) {
    //debugger;
        var preOrderId = null;
      
     //  var table = new $.fn.dataTable.Api('#LoadDataTable');
     //  var dataArray = table.rows().data();
        var result = $.grep(currentDataRows, function (e) {
            return e.PreOrderItemId == id;
       });

       if (result.length==1) {
           preOrderId = result[0].preOrderId;
       }
      
               
       var index = selectedPreorderItemValues.indexOf(id);
       if (index >= 0 && state == false) {
           selectedPreorderItemValues.splice(index, 1);
           selectedPreorderValues.splice(index, 1);
       }
       else if (index < 0 && state == true) {
           selectedPreorderItemValues.push(id);
           selectedPreorderValues.push(preOrderId);
       }

  }

//Restore Checkbox state upon page change
function restorePickupCheckBoxes(obj) {
    // RestoreState(obj);
    var checkedCount = 0;
    var totalCount = 0;
    $(':checkbox', obj.fnGetNodes()).each(function () {
        var id = $(this).attr('id');
        if (id != 'SelectAll') {

            totalCount++;

            var index = selectedPreorderItemValues.indexOf(id);
            if (index >= 0) {
                $(this).prop('checked', true);
                checkedCount++;
            }
            else {

                $(this).prop('checked', false);
            }
        }
    });
    //debugger;
    if (checkedCount == totalCount && totalCount > 0) {
        $('#SelectAll').prop('checked', true).parent("span").addClass('checked');

    }
    else {

        $('#SelectAll').prop('checked', false).parent(".checked").removeClass('checked');

    }

}

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



/* old logic to get multiselect dropdown values
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
function resetDefaultOption(dropdownID) {

    if (dropdownID == '#Category') {
        $(dropdownID).find('option:not(:first)').remove();
    }

    $(dropdownID).val("");
    $(dropdownID).select2('destroy');
    $(dropdownID).select2();
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

function ShowVoidPopup() {
    
    if (selectedPreorderValues.length < 1) {
        //alert("Please select at least one item");
        var message = "Please select at least one item";
        displayWarningMessage(message);
        return;
    }

    $("#hdSelectedPreorderValues").val(selectedPreorderValues.join(','));
    
    $('#VoidModalDiv').modal('toggle');
}

// Preorder Report Section

function refreshLoadedData() {

    var state = false;

    $('#LoadDataTable tbody').find(':checkbox').attr("checked",state)
    $('#SelectAll').prop('checked', false).parent(".checked").removeClass('checked');
    selectedPreorderItemValues = [];
    selectedPreorderValues = [];

    var table = $('#LoadDataTable').DataTable();
    table.ajax.reload(null, false);


}

function RunPreorderPickupReport() {
    debugger;
    //var today = new Date();
    //var dd = today.getDate();
    //var mm = today.getMonth() + 1; //January is 0!
    //var yyyy = today.getFullYear();
    //var toDateStr = dd+'/'+mm+'/'+yyyy;
    //Waqar Q. Bug 1556
    var toDateStr ='';
    var fromDateStr = '';
    var dateRangeTypes = getSelectedItemsList('DateRangeTypesListId');
    if (dateRangeTypes != "") {
        //Date Range from and to
        fromDateStr = moment(document.getElementById("startDate").value).format("DD/MM/YYYY");
        toDateStr = moment(document.getElementById("EndDate").value).format("DD/MM/YYYY");

     }

    var selectedPreorderitemStr = reportOfAllItems ==true? selectedPreorderAllItemValues : selectedPreorderItemValues.join(',');
    var dataObj = { location: selectedPreorderitemStr, fromDate: fromDateStr, toDate: toDateStr }
    var dataFilterString = JSON.stringify(dataObj);
    var idVal = 0;
    //Hide Preorder Div
    $('#DataTableDiv').hide();
    $('#reporDiv').show();
    $('#LoadDataBtn').prop('disabled',true);
    //dataString = StringifyRptFilters();
    //var qs = getQueryStrings();
    //var idval = qs["id"];
    $('#exportDiv').hide();
    $('#closeDiv').hide();
    $('#loadingSpinnerimg').show();
    $('#exportDiv').show();
    $('#closeDiv').show();
   // return;
    $.ajax({
        url: '/PreorderPickupReport/ShowReport',
        data: { 'dataFilterString': dataFilterString, 'id': idVal }, //dataString,
        type: 'GET',
        success: function (response) {
          //  debugger;
            if (response.result == "-2") {
                displayErrorMessage('Error occurred during display of report.');
            }
            else if (response.result == "-3") {
                displayErrorMessage('This report is not added in the system yet.');
            }
            else {
                $('#DataTableDiv').hide();
                $('#reportContentDiv').load('/PreorderPickupReport/RefreshReport?id=0');

                $('#exportDiv').show();
                $('#closeDiv').show();
               

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




