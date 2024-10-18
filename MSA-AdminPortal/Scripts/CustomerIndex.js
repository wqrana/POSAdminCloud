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

var ocustomerLogTable;
var oCustomerTable;

var TargetBaseControl = null;
//Total no of checkboxes in a particular column inside the GridView.
var CheckBoxes;
//Total no of checked checkboxes in a particular column inside the GridView.
var CheckedCheckBoxes;
//Array of selected item's Ids.
var SelectedItems;
//Hidden field that wil contain string of selected item's Ids separated by '|'.
var SelectedValues;

var filterObj;
var CustSortCol;
var CustSortDir;
$(document).ready(function () {
    //debugger;
    filterObj = null;
    //Default Customer sort data setting
    CustSortCol = 1;
    CustSortDir = "asc";

    $("#hidefilters").text("Show Filters");
    $('#filterDiv').slideUp();
    $('.checkNum').slideUp();
    $("#SchoolFilter").select2();
    $("#searchdll").select2();
    $("#GradeFilter").select2();
    $("#adultdll").select2();



    $("#activedll").select2();
    $("#activedll").select2("val", "");


    $("#homeroomdll").select2();


    CheckBoxes = 0;
    CheckedCheckBoxes = 0;

    $("#hidefilters").click(function () {


        toggleText(this.id);
        /* commented by Waqar Q.
        ClearCustomerFilters();
        setTimeout(foo, 3000);
        oCustomerTable.fnDraw();
        */
    });

    $("#SchoolFilter").change(function () {
        var schoolID = this.value;
        bindHomeroomDll(schoolID);
    });

    $('#SearchStr').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            if (filtersSelected()) {
                oCustomerTable.fnDraw();
            } else {
                displayWarningMessage('Please choose a "Search by..." value before trying to use the search functionality.');
            }
            return false;
        }
    });

    $("#ViewActivity").click(function () {

        var cID = $("#CustomerID").val();
        var cUserID = $("#cUserID").text();
        var CustomerIDString = JSON.stringify({
            allData: cID
        });
        if (cID != "") {
            $.ajax({
                type: "POST",
                url: "/OrdersMgt/GetActivityCount",
                data: CustomerIDString,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.result == 'no') {
                        displayWarningMessage("The customer doesn’t have any activity.");
                    }
                    else {
                        window.location = "/OrdersMgt/Activity?cID=" + cID + "&cUserID=" + cUserID + "&searchByCust=yes";
                    }
                },
                error: function (request, status, error) {
                    displayErrorMessage('Error occurred during redirection to the activity page.');
                    return false;
                }
            });

        } else {
            displayErrorMessage("Error occurred during redirection to the activity page.");
        }
    });

    restoreFilters();


    ///////
    //old settings sdom
    //"<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>"
    oCustomerTable = $('#customerTable').dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-5 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10,25, 50, 100],
            [10,25, 50, 100] // change per page values here
        ],
        "ajax": "data.json",
        "bProcessing": true,
        "bServerSide": true,
        "order": [[CustSortCol, CustSortDir]],
        "cache": true,
        "sAjaxSource": "/Customer/AjaxHandler",
        // set the initial value
        "iDisplayLength": 10,
        "fnInitComplete": function (oSettings, json) {

            //alert('DataTables has finished its initialisation.' + json.status);
        },

        "sPaginationType": "bootstrap_full_number",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push(
                    { "name": "SearchBy", "value": $("#SearchStr").val() },
                    { "name": "SearchBy_Id", "value": $("#searchdll").val() },
                    { "name": "SchoolStr", "value": ($('#SchoolFilter').val().trim() == '') ? '' : $('#SchoolFilter').select2('data').text },
                    { "name": "HomeRoomStr", "value": ($('#homeroomdll').val() == null || $('#homeroomdll').val().trim() == '-9999' || $('#homeroomdll').val().trim() == '') ? '' : $('#homeroomdll').select2('data').text },
                    { "name": "GradeStr", "value": $("#GradeFilter").val() },
                    { "name": "ActiveStr", "value": $("#activedll").val() },
                    { "name": "AdultStr", "value": $("#adultdll").val() },
                    { "name": "radomNumber", "value": Math.random() }
                );
            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json);
                jQuery("#SearchStr").focus();
            });
        },
        "fnDrawCallback": function () {
            //debugger;
            restorecheckBoxes(this);
            disableEditDeleteLinks();
        },
        "oLanguage": {
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
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
        "aoColumns": [
                    {
                        "sWidth": "12%",
                        "sClass": "leftClass",
                        "bSortable": false,
                        "mRender": function (data, type, row) {

                            return '<a  title="Edit" href="/Customer/Edit/' + row[12] + '" role=\"button\" class=\"EditSecurityClass\" ><i id="edit" class="fa fa-pencil-square-o fasize"></i></a>'
                               //+ ' <span class="faseparator"> | </span>  <a  title="Delete"' + 'href="#" onclick="return setCustomer(' + row[12] + ',"' + row[4] + '","' + row[3] + '");"  data-backdrop="static" data-keyboard="false" class="DeleteSecurityClass" data-toggle="modal" ><i class="fa fa-trash fasize"></i></a>'
                                + '<span class="faseparator">| </span><a title="Delete" href="#" onclick="return setCustomer(' + row[12] + ',\'' + row[4] + '\',\''+row[3]+'\')" data-toggle="modal" data-keyboard="false" data-backdrop="static" class="deleteUser DeleteSecurityClass"><i class="fa fa-trash  fasize"></i></a>'
                               + ' <span class="faseparator"> | </span>  <a  title="Activate/Deactivate" onclick="javascript:Activate(this);" href="#Activate" data-toggle="modal" data-backdrop="static" data-keyboard="false" class="ActivateLink disableSecurityClass" data-id="' + row[12] + '">' + row[0] + '</a>';
                            ;
                        }
                    },
                    {
                        "sName": "id",
                        "sWidth": "3%",
                        "sClass": "centerClass",
                        "bVisible": false,
                        "bSortable": false,
                        "sClass": "centerClass",
                        "mRender": function (data, type, row) {
                            return "<input type=\"checkbox\" id=\"" + row[12] + "\" onclick=\"ChildClick(this," + row[12] + ",\'" + row[12] + "\',\'" + row[12] + "\')\" name=\"" + row[12] + "\" />";
                        }
                    },
                    {
                        "sName": "UserID",
                        "sWidth": "9%",
                        "bSearchable": false,
                        "bSortable": true,
                        "mRender": function (data, type, row) {
                            return '<a href=\"#CustomerModal\"  class=\"EditSecurityClass \" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + row[2] + '</a>';

                        }
                    },
                    {
                        "sName": "Last_Name",
                        "sWidth": "13%",
                        "mRender": function (data, type, row) {
                            return '<a href=\"#CustomerModal\"    class=\"EditSecurityClass\" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + row[3] + '</a>';

                        }

                    },
                    {
                        "sName": "First_Name",
                        "sWidth": "13%",
                        "bSortable": true,
                        "mRender": function (data, type, row) {
                            //return '<a class=\" default\" data-target=\"#stack1\" data-toggle=\"modal\">Edit</a>';
                            return '<a href=\"#CustomerModal\"    class=\"EditSecurityClass\" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + row[4] + '</a>';

                        }
                    },
                    {
                        "sName": "Middle_Initial",
                        "sWidth": "10%",
                        "mRender": function (data, type, row) {
                            return '<a href=\"#CustomerModal\"    class=\"EditSecurityClass\" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + row[5] + '</a>';

                        }
                    },
                    {
                        "sName": "Adult",
                        "sWidth": "6%",
                        "sClass": "centerClass",
                        "mRender": function (data, type, row) {
                            if (row[6] == 'True') return "<i class=\"fa fa-check\" style=\"margin-left:0px;\"></i>"; else return "";
                        }

                    },
                    {
                        "sName": "Grade",
                        "sWidth": "7%",
                        "mRender": function (data, type, row) {
                            return '<a href=\"#CustomerModal\"    class=\"EditSecurityClass\" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + row[7] + '</a>';

                        }
                    },
                    {
                        "sName": "Homeroom",
                        "sWidth": "6%",
                        "mRender": function (data, type, row) {
                            return '<a href=\"#CustomerModal\"    class=\"EditSecurityClass\" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + row[8] + '</a>';

                        }
                    },
                    {
                        "sName": "School_Name",
                        "sWidth": "10%",
                        "mRender": function (data, type, row) {
                            return '<a href=\"#CustomerModal\"    class=\"EditSecurityClass\" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + trimifLong(row[9]) + '</a>';

                        }
                    },
                    {
                        "sName": "PIN",
                        "sWidth": "6%",
                        "mRender": function (data, type, row) {
                            return '<a href=\"#CustomerModal\"    class=\"EditSecurityClass\" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + trimifLong(row[10]) + '</a>';

                        }
                    },
                    {
                        "sName": "Total_Balance",
                        "sWidth": "7%",
                        "mRender": function (data, type, row) {
                            return '<a href=\"#CustomerModal\"    class=\"EditSecurityClass\" role=\"button\" data-backdrop="static" data-keyboard="false"  ' + 'onclick=editClicked(\"' + row[12] + '\");  data-toggle=\"modal\" >' + row[11] + '</a>';

                        }
                    },

        ]
    });
    $('#customerTable_wrapper .dataTables_filter').hide(); //.addClass("form-control input-medium"); // modify table search input
    $('#customerTable_wrapper .dataTables_length select').addClass("form-control input-small"); // modify table per page dropdown

    //oCustomerTable.fnSort([[1, 'asc']]);

    $("#Confirmdelete").click(function (e) {
        DeleteCustomer();
    });
    $("#SearchBtn").click(function (e) {
        //SearchCustomer();
        if (filtersSelected()) {
            //Save filters in session;
            // saveFilters();
            oCustomerTable.fnDraw();
        } else {
            displayWarningMessage('Please choose a "Search by..." value before trying to use the search functionality.');
        }
    });

    $("#applyFilterBtn").click(function (e) {
        //ApplyCustomerFilters();
        //setTimeout(foo, 3000);

        //Save filters in session;
        // saveFilters();
        oCustomerTable.fnDraw();
    });

    $("#Clearfilters").click(function (e) {
        ClearCustomerFilters();
        setTimeout(foo, 3000);
        oCustomerTable.fnDraw();
    });

    /////tab chnaged
    $(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
        ocustomerLogTable.fnAdjustColumnSizing();
        $('#customerLogTable_wrapper').slimScroll({
            height: '160px',
            size: '10px',
            position: 'right',
            color: '#a1a0a0',
            opacity: 1,
            alwaysVisible: true,
            railVisible: true,
            railColor: '#fff',
            railOpacity: 1,
            wheelStep: 10,
            allowPageScroll: false,
            disableFadeOut: false
        });
    });
    //////


    //On Add new Customer
    $("#AddNewButton").click(function () {
        //debugger;
        saveFilters();

    });
    //On edit Customer
    $(document).on("click", "#edit", function () {
        //debugger;
        saveFilters();
    });






    //
    $("#editBtn").click(function () {
        debugger;
        saveFilters();
        var updatecustomer = $('#UpdateCustomers').val();

        if (updatecustomer == "False") {
            disableDeleteLinksTile("UpdateCustomers", "disableSecurityClass", "You don’t have rights to edit a customer.");
            alert("You don’t have rights to edit a customer.");
        } else {
            // saveFilters();
            var cID = $('#CustomerID').val();
            window.location.href = '/Customer/Edit/' + cID;
        }
    });
    //////////other table
    ocustomerLogTable = $('#customerLogTable').dataTable({
        "sDom": "t",
        "aLengthMenu": [
            [10, 25, 50, 100, -1],
            [10, 25, 50, 100, "All"] // change per page values here
        ],
        "bProcessing": false,
        "bServerSide": true,
        "bAutoWidth": false,
        "sAjaxSource": "/Customer/LogAjaxHandler",
        "iDisplayLength": 10000,
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) { //added
            //oSettings.jqXHR = $.ajax({
            //    "dataType": 'json',
            //    "type": "POST",
            //    "url": sSource, //"/Customer/LogAjaxHandler",
            //    "data": aoData,
            //    "success": fnCallback,
            //    "error": function (xhr, error, thrown) {
            //        if (error == "parsererror") {
            //            alert("DataTables warning:: JSON data from server could not be parsed. " +
            //                "This is caused by a JSON formatting error.");
            //        }
            //    }
            //});
        },


        "aoColumns": [
                    {
                        "sName": "CDate",
                        "sWidth": "20%",
                        "bSearchable": false,
                        "bSortable": false
                    },
                    {
                        "sName": "ChangedTime",
                        "sWidth": "14%",
                        "bSortable": false
                    },
                    {
                        "sName": "Note",
                        "sWidth": "46%",
                        "bSortable": false
                    },
                    {
                        "sName": "Employee_Name",
                        "sWidth": "20%",
                        "bSortable": false
                    }
        ]
    });

    //////////////
    BindPaymentAmount();
    /////////
    checkCustomersSelected();

    //$('select[name^="activedll"] option:selected').attr("selected", null);
    //$('select[name^="activedll"] option[value="Active"]').attr("selected", "selected");

    disableCreationRights("CreateCustomers", "AddNewButton", "aAddNewButton", "disabled", "ActionLink", "You don’t have rights to create a customer.");


});

//Restore filters from session object
function restoreFilters() {

    var filterObjStr = sessionStorage.getItem("savedFilters");
    if (filterObjStr) {
        filterObj = JSON.parse(filterObjStr);

        $("#SearchStr").val(filterObj.SearchStr).change();
        $("#searchdll").val(filterObj.searchdll).change();
        $('#SchoolFilter').val(filterObj.SchoolFilter).change();
        $('#homeroomdll').val(filterObj.homeroomdll).change();
        //$("#homeroomdll").select2("val", filterObj.homeroomdll);
        $("#GradeFilter").val(filterObj.GradeFilter).change();
        $("#activedll").val(filterObj.activedll).change();
        $("#adultdll").val(filterObj.adultdll).change();

        //Set saved filters 
        CustSortCol = filterObj.sortCol;
        CustSortDir = filterObj.sortDir;


        sessionStorage.removeItem("savedFilters");
    }

}

//Save Filter Values in session
function saveFilters() {

    filterObj = new Object();
    filterObj.SearchStr = $("#SearchStr").val();
    filterObj.searchdll = $("#searchdll").val();
    filterObj.SchoolFilter = $('#SchoolFilter').val()//($('#SchoolFilter').val().trim() == '') ? '' : $('#SchoolFilter').select2('data').text;
    filterObj.homeroomdll = $('#homeroomdll').val()//($('#homeroomdll').val() == null || $('#homeroomdll').val().trim() == '-9999' || $('#homeroomdll').val().trim() == '') ? '' : $('#homeroomdll').select2('data').text;
    filterObj.GradeFilter = $("#GradeFilter").val();
    filterObj.activedll = $("#activedll").val();
    filterObj.adultdll = $("#adultdll").val();
    //Save current current column sort info.


    var sortCol = $("#customerTable").dataTable().fnSettings().aaSorting[0][0];
    var sortDir = $("#customerTable").dataTable().fnSettings().aaSorting[0][1];



    filterObj.sortCol = sortCol; // counting from left, starting with 0
    filterObj.sortDir = sortDir;

    filterObjStr = JSON.stringify(filterObj);

    sessionStorage.setItem("savedFilters", filterObjStr);

}


function bindHomeroomDll(schoolId) {

    var jsonSchoolID = JSON.stringify({ SchoolId: schoolId });
    $('#homeroomdll').select2("enable", false);

    $.ajax({
        type: "POST",
        url: "/Customer/getHomeRoomsList",
        data: jsonSchoolID,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $('#homeroomdll').select2("enable", true);
            if (data.result != "-1") {

                $('#homeroomdll').get(0).options.length = 0;
                $("#homeroomdll").get(0).options[$("#homeroomdll").get(0).options.length] = new Option("", "-9999");
                $("#homeroomdll").select2("val", "-9999");
                $.each(data.result, function (index, item) {
                    $("#homeroomdll").get(0).options[$("#homeroomdll").get(0).options.length] = new Option(item.Text, item.Value);
                });
            } else {
                $('#homeroomdll').get(0).options.length = 0;
                $("#homeroomdll").get(0).options[$("#homeroomdll").get(0).options.length] = new Option("", "-9999");
                $("#homeroomdll").select2("val", "-9999");
            }

            if (filterObj) {

                $("#homeroomdll").select2("val", filterObj.homeroomdll);
                filterObj = null;
            }
        },
        error: function (request, status, error) {
            //displayErrorMessage("Error occurred during saving the data.");
            //return false;
        }
    });
}

$.fn.dataTableExt.oApi.fnReloadAjax = function (oSettings, sNewSource, fnCallback, bStandingRedraw) {


    if (sNewSource !== undefined && sNewSource !== null) {
        oSettings.sAjaxSource = sNewSource;
    }
    this.oApi._fnClearTable(oSettings);
    // Server-side processing should just call fnDraw
    if (oSettings.oFeatures.bServerSide) {
        this.fnDraw();
        return;
    }

    this.oApi._fnProcessingDisplay(oSettings, true);
    var that = this;
    var iStart = oSettings._iDisplayStart;
    var aData = [];

    this.oApi._fnServerParams(oSettings, aData);

    oSettings.fnServerData.call(oSettings.oInstance, oSettings.sAjaxSource, aData, function (json) {
        /* Clear the old information from the table */
        that.oApi._fnClearTable(oSettings);

        /* Got the data - add it to the table */
        var aData = (oSettings.sAjaxDataProp !== "") ?
that.oApi._fnGetObjectDataFn(oSettings.sAjaxDataProp)(json) : json;

        for (var i = 0; i < aData.length; i++) {
            that.oApi._fnAddData(oSettings, aData[i]);
        }

        oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();

        that.fnDraw();

        if (bStandingRedraw === true) {
            oSettings._iDisplayStart = iStart;
            that.oApi._fnCalculateEnd(oSettings);
            that.fnDraw(false);
        }

        that.oApi._fnProcessingDisplay(oSettings, false);

        /* Callback user function - for event handlers etc */
        if (typeof fnCallback == 'function' && fnCallback !== null) {
            fnCallback(oSettings);
        }
    }, oSettings);
};


//coomented old function 12 March, 2015
//$.fn.dataTableExt.oApi.fnReloadAjax = function (oSettings, sNewSource, fnCallback, bStandingRedraw) {

//    if (typeof sNewSource != 'undefined' && sNewSource != null) {
//        oSettings.sAjaxSource = sNewSource;
//    }
//    this.oApi._fnProcessingDisplay(oSettings, true);
//    var that = this;
//    var iStart = oSettings._iDisplayStart;

//    oSettings.fnServerData(oSettings.sAjaxSource, [], function (json) {
//        /* Clear the old information from the table */
//        that.oApi._fnClearTable(oSettings);

//        /* Got the data - add it to the table */
//        var aData = (oSettings.sAjaxDataProp !== "") ?
//            that.oApi._fnGetObjectDataFn(oSettings.sAjaxDataProp)(json) : json;

//        for (var i = 0 ; i < json.aaData.length ; i++) {
//            that.oApi._fnAddData(oSettings, json.aaData[i]);
//        }

//        oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
//        that.fnDraw();

//        if (typeof bStandingRedraw != 'undefined' && bStandingRedraw === true) {
//            oSettings._iDisplayStart = iStart;
//            that.fnDraw(false);
//        }

//        that.oApi._fnProcessingDisplay(oSettings, false);

//        /* Callback user function - for event handlers etc */
//        if (typeof fnCallback == 'function' && fnCallback != null) {
//            fnCallback(oSettings);
//        }
//    }, oSettings);
//}

function trimifLong(str) {
    if (str != null) {
        if (str.length > 20 && str.indexOf(' ') == -1) {
            return str.substr(0, 17) + "..."
        } else {
            return str;
        }
    }
}

var customerId = '';
function setCustomer(id, lastName, firstName) {
    customerId = id;
    $("#customerNameH").html(firstName + " " + lastName);
    $("#customerName").html(firstName + " " + lastName);
    $('#deleteModal').modal('show');
    return true;
}

function foo() {
    //some delay
}
function DeleteCustomer() {

    var posdataString = JSON.stringify({
        allData: customerId
    });
    $.ajax({
        type: "POST",
        url: "/Customer/DeleteCustomer",
        data: posdataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.result == '-1') {
                displaySuccessMessage("The customer record has been deleted successfully.");
                oCustomerTable.fnDraw();
            }
            else {
                displayWarningMessage(data.result);
            }
        },
        error: function (request, status, error) {
            displayErrorMessage('Error in deleting customer data');
            return false;
        }
    });
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
function filtersSelected() {
    var sval = $("#searchdll").val();
    if (sval == null) {
        $("#searchdll").val(0)
        return true;
    }
    if (sval.length == 0) {
        return false
    } else {
        return true;
    }


}

//function SearchCustomer() {
//    var searchstr = document.getElementById('SearchStr').value;
//    var dllsearch = $('#searchdll').val();

//    var posdataString = JSON.stringify({
//        allData: searchstr + '*' + dllsearch
//    });
//    $.ajax({
//        type: "POST",
//        url: "/Customer/SearchCustomer",
//        data: posdataString,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (data) {
//            if (data.result == '-1') {
//                displaySuccessMessage("Searching.........");
//            }
//            else {
//                alert(data.result);
//            }
//        },
//        error: function (request, status, error) {
//            displayErrorMessage('Error in applying filters on customer data');
//            return false;
//        }
//    });
//}

//function ApplyCustomerFilters() {

//    var schooltext = document.getElementById('schoolText').value;
//    var homeroomtext = document.getElementById('homeroomText').value;

//    var gradefilterval = $('#GradeFilter').val();
//    var adultdllval = $('#adultdll').val();
//    var activedllval = $('#activedll').val();

//    //debugger;
//    var posdataString = JSON.stringify({
//        allData: schooltext + '*' +
//                    homeroomtext + '*' +
//                    gradefilterval + '*' +
//                    adultdllval + '*' +
//                    activedllval
//    });
//    $.ajax({
//        type: "POST",
//        url: "/Customer/ApplyCustomerFilters",
//        data: posdataString,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (data) {
//            if (data.result == '-1') {
//                displaySuccessMessage("Applying Filters.........");

//            }
//            else {
//                alert(data.result);
//            }
//        },
//        error: function (request, status, error) {
//            displayErrorMessage('Error deleting customer data');
//            return false;
//        }
//    });
//}


function ClearCustomerFilters() {
    $("#SearchStr").val("");
    $("#searchdll").val("0");
    $("#searchdll").select2("val", "0");

    $("#homeroomdll").select2("val", "-9999");
    //var theSpan = $("#s2id_searchdll").find("span").first();
    //theSpan.text("Search By...");
    $("#SchoolFilter").val("");
    $("#SchoolFilter").select2();

    $("#GradeFilter").val("");
    $("#GradeFilter").select2();

    $("#adultdll").val("");
    $("#adultdll").select2();

    $("#activedll").val("");
    $("#activedll").select2("val", "", "placeholder", "Both");

    bindHomeroomDll(0);
   // $("#activedll").val("Both");
    //$("#GradeFilter").val("");
    //var thegradSpan = $("#s2id_GradeFilter").find("span").first();
    //thegradSpan.text("Select Grade");


    //$("#activedll").val("");
    //var theactiveSpan = $("#s2id_activedll").find("span").first();
    //theactiveSpan.text("Select Active Type");



    //$("#adultdll").val("");
    //var theadultSpan = $("#s2id_adultdll").find("span").first();
    //theadultSpan.text("Select Adult Type");


    //var clearstr = JSON.stringify({
    //    allData: 0
    //});
    //$.ajax({
    //    type: "POST",
    //    url: "/Customer/ClearCustomerFilters",
    //    data: clearstr,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        if (data.result == '-1') {
    //            displaySuccessMessage("Clearing filters.........");

    //        }
    //        else {
    //            alert(data.result);
    //        }
    //    },
    //    error: function (request, status, error) {
    //        displayErrorMessage('Error clearing filters');
    //        return false;
    //    }
    //});

}

// VIEW
function editClicked(id) {
    //debugger;

    if (id != '') {
        $('#Loaderdiv').show();
        //
        $.ajax({
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                //Upload progress
                xhr.upload.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        var percentComplete = evt.loaded / evt.total;
                        //Do something with upload progress
                        console.log(percentComplete);
                    }
                }, false);
                //Download progress
                xhr.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        var percentComplete = evt.loaded / evt.total;
                        //Do something with download progress
                        //console.log(percentComplete);
                        $('#Loaderdiv').show();
                    }
                }, false);
                return xhr;
            },
            type: "get",
            url: '/Customer/Popup/' + id,
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                if (data.IsError) {
                    displayWarningMessage(data.ErrorMessage);
                }
                else {
                    // debugger;
                    $('#customerDetail').text(data.Customer.LastName + ' ' + data.Customer.FirstName);
                    $('#customernamebelow').text(data.Customer.LastName + ' ' + data.Customer.FirstName);
                    $('#CustomerID').val(data.Customer.Customer_Id);
                    $('#Cgender').text(data.FullGender);
                    $('#cSSN').text(data.Customer.SSN);
                    $('#cdob').text(data.CustomerDOB);
                    $('#CAddress').text(data.FullAddress);
                    $('#cPhoto').attr('src', data.uri);
                    $('#CPhon').text(data.Customer.Customer_Phone);
                    $('#CEmail').text(data.Customer.Email);
                    $('#Clanguage').text(data.Customer.Language);
                    $('#cNotes').text(data.Customer.Customer_Notes);
                    $('#cUserID').text(data.Customer.UserID);
                    $('#cPIN').text(data.Customer.PIN);
                    $('#CSchool').text(trimifLong(data.Customer.School_Name));
                    $('#CGrade').text(data.Customer.Grade);

                    $('#CHomeroom').text(data.Customer.Homeroom);
                    $('#CDistrict').text(data.Customer.District_Name);
                    $('#CMStatus').text(data.Customer.LunchType_String);
                    $('#customertotalbalance').text('$' + data.Customer.TotalBalance);
                    $('#customerMealPlanBalance').text('$' + data.Customer.MealPlanBalance);
                    $('#customerAlaCartebalance').text('$' + data.Customer.AlaCarteBalance);
                    $('#COptions').html(data.cOptions);
                    $('#CAccountrest').html(data.cRestrictions);
                    $('#CeatingAssign').html(data.CustomerAssignedSchools);
                    $('#userCreationDate').text(ifEmpty(data.Customer.ConvertedCreationDate));
                    //activeStatusImg Active

                    populatePOSNotificationCombo(data);

                    ocustomerLogTable.fnReloadAjax("/Customer/LogAjaxHandler/" + data.Customer.Customer_Id);
                    ocustomerLogTable.fnAdjustColumnSizing();
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

function populatePOSNotificationCombo(data) {
    var strComboStart = '<select id="pOSNotificationsList" style="width: 100%; height: 100%;" class="multicss" multiple="multiple" size="100">';
    var strComboEnd = '</select>';
    var strComboContent = "";
    for (var i = 0; i < data.POSNotifications.length; i++) {
        strComboContent = strComboContent + '<option value="' + data.POSNotifications[i].Id.toString() + '"' + (data.POSNotifications[i].IsSelected ? 'selected="selected"' : '') + '>' + data.POSNotifications[i].Name.toString() + '</option>';
    }
    var finalContent = strComboStart + strComboContent + strComboEnd
    $('#divpOSNotificationsList').html(finalContent);
    $('#pOSNotificationsList').select2();

    $('#pOSNotificationsList').change(function () {
        var selectedNotifications = $('#pOSNotificationsList').val();
        var customerId = $('#CustomerID').val();

        if (customerId != "") {


            if (selectedNotifications == null) {
                selectedNotifications = '';
            }

            var dataString = JSON.stringify({
                allData: customerId + '*' + selectedNotifications
            });

            //alert(dataString);

            $.ajax({
                type: "POST",
                url: "/POSNotifications/AddCustomerToNotificaion",
                data: dataString,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.result == '-1') {

                        displaySuccessMessageOnce("Changes have been saved successfully.");
                        //window.location.reload(true);
                    }
                },
                error: function (request, status, error) {
                    displayErrorMessage("Error occurred during saving the data.");
                    return false;
                }
            });
        }
    });
}

function ifEmpty(str) {

    if (!str || 0 === str.length || str == "null") {
        return "Not specified";
    }
}


function ChildClick(CheckBox, Id, fname, lname) {


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
    $('hdnFldSelectedValues').val(SelectedItems);
    SelectedValues.value = SelectedItems.join('|');
    checkCustomersSelected();


}

function checkCustomersSelected() {

    if (oCustomerTable) {
        restorecheckBoxes(oCustomerTable);
    }
    if ($('#hdnFldSelectedValues').val() == "") {
        disableActionButton();
    } else {
        enableActionButton();
    }
}

function disableActionButton() {
    $("#ActionButton").attr("disabled", true);
    $("#ActionButton").addClass("disabled");
}

function enableActionButton() {
    if ($("#ActionButton").hasClass("disabled")) {
        $("#ActionButton").removeAttr("disabled");
        $("#ActionButton").removeClass("disabled");
    }
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


function restorecheckBoxes(obj) {

    //Get hidden field that wil contain string of selected item's Ids separated by '|'.
    SelectedValues = document.getElementById('hdnFldSelectedValues');

    SelectedItems = SelectedValues.value.split('|');

    RestoreState(obj);

}
function RestoreState(obj) {

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
function PayByCheck() {

    if (document.getElementById("CheckPayment").checked == true)
        $('.checkNum').slideDown();
    else
        $('.checkNum').slideUp();
}

function disableEditDeleteLinks() {
    disableEditLinksTile("UpdateCustomers", "EditSecurityClass", "ActionLink", "You don’t have rights to edit a customer.");
    disableDeleteLinksTile("DeleteCustomers", "DeleteSecurityClass", "You don’t have rights to delete a customer.");
    disableDeleteLinksTile("UpdateCustomers", "disableSecurityClass", "You don’t have rights to deactivate a customer.");
    disableDeleteLinksTile("viewCustomers", "ViewSecurityClass", "You don’t have rights to view a customer.");
}
