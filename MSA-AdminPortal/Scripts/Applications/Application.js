/// <reference path="C:\Food Service\POS\Admin\DEV-FORMS_View_Print\MSA-AdminPortal\Content/themes/assets/global/plugins/datatables/all.min.js" />
var tblApplications;
var tblAppStudents;
var tblAppMembers;
var CustSortCol;
var CustSortDir;
var AppId = 0;

$(document).ready(function () {

    CustSortCol = 1;
    CustSortDir = "asc";

    $("#dlSearchBy").select2();
    $("#dpApprovalStatus").select2();
    $("#dpEnteredOptions").select2();
    $("#dpUpdatedOptions").select2();
    $('#dpSigneddate').datepicker({
        showButtonPanel: true
    });

    $("#hidefilters").text("Show Filters");
    $('#filterDiv').slideUp();
    $("#hidefilters").click(function () {
        toggleText(this.id);
    });

    $("#applyFilterBtn").click(function (e) {
        tblApplications.fnDraw();
    });

    $("#Clearfilters").click(function (e) {
        clearApplicationFilters();
        setTimeout(foo, 3000);
        tblApplications.fnDraw();
    });

    tblApplications = $("#tblApplications").dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-5 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100],
            [10, 25, 50, 100] // change per page values here
        ],
        "ajax": "data.json",
        "bProcessing": true,
        "bServerSide": true,
        "order": [[CustSortCol, CustSortDir]],
        "cache": true,
        "sAjaxSource": "/Application/AjaxHandler",
        // set the initial value
        "iDisplayLength": 10,
        "fnInitComplete": function (oSettings, json) {
            //alert('DataTables has finished its initialisation.' + json.status);
        },
        "sPaginationType": "bootstrap_full_number",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push(
                    { "name": "SearchBy", "value": $("#txtSearch").val() },
                    { "name": "SearchBy_Id", "value": $("#dlSearchBy").val() },
                    { "name": "SignedDate", "value": $("#txtSignedDate").val() },
                    { "name": "ApprovalStatus", "value": $("#dpApprovalStatus").val() },
                    { "name": "Entered", "value": $("#dpEnteredOptions").val() },
                    { "name": "Updated", "value": $("#dpUpdatedOptions").val() }
                );
            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json);
                jQuery("#txtSearch").focus();
            });
        },
        "fnDrawCallback": function () {
        },
        "oLanguage": {
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
            "sInfoEmpty": 'No records',
            "sEmptyTable": "No records found",
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": {
                "sPrevious": "Prev",
                "sNext": "Next"
            }
        },
        "aoColumns": [
            {
                "sWidth": "1%",
                "sClass": "leftClass",
                "bSortable": false,
                "mRender": function (data, type, row) {
                    return '<a  title="Print" href="/Application/Print?appId=' + row[1] + '" target="_blank" role=\"button\" class=\"EditSecurityClass\" ><i id="print" class="fa fa-print fasize"></i></a>'
                        + '<span class="faseparator"> | </span> <a title="Review" onclick=ReviewApp(\"' + row[1] + '\"); href="/Application/Review?appId=' + row[1] + '" ><i class="fa fa-book fasize"></i></a>';
                    ;
                }
            },
            {
                "sName": "Application_Id",
                "sWidth": "3%",
                "sClass": "centerClass",
                "bVisible": true,
                "bSortable": true,
                "sClass": "centerClass"
            },
            {
                "sName": "Student_Name",
                "sWidth": "3%",
                "sClass": "centerClass",
                "bVisible": false,
                "bSortable": true,
                "sClass": "centerClass"
            },
            {
                "sName": "Member_Name",
                "sWidth": "3%",
                "sClass": "centerClass",
                "bVisible": false,
                "bSortable": true,
                "sClass": "centerClass"
            },
            {
                "sName": "District_Name",
                "sWidth": "3%",
                "sClass": "centerClass",
                "bVisible": true,
                "bSortable": true,
                "sClass": "centerClass"
            },
            {
                "sName": "Household_Size",
                "sWidth": "3%",
                "sClass": "centerClass",
                "bVisible": true,
                "bSortable": true,
                "sClass": "centerClass"
            },
            {
                "sName": "No_Of_Students",
                "sWidth": "3%",
                "sClass": "centerClass",
                "bVisible": true,
                "bSortable": true,
                "sClass": "centerClass",
                "mRender": function (data, type, row) {
                    return '<a href=\"#\" role=\"button\" ' + 'onclick=ShowStudentsPopup(\"' + row[1] + '\"); >' + row[6] + '</a>';
                    ;
                }
            },
            {
                "sName": "No_Of_Members",
                "sWidth": "3%",
                "sClass": "centerClass",
                "bVisible": true,
                "bSortable": true,
                "sClass": "centerClass",
                "mRender": function (data, type, row) {
                    return '<a href=\"#\" role=\"button\" ' + 'onclick=ShowMembersPopup(\"' + row[1] + '\"); >' + row[7] + '</a>';
                    ;
                }
            },
            {
                "sName": "App_Signer_Name",
                "sWidth": "3%",
                "sClass": "centerClass",
                "bVisible": true,
                "bSortable": true,
                "sClass": "centerClass"
            },
            {
                "sName": "Approval_Status",
                "sWidth": "3%",
                "sClass": "centerClass",
                "bVisible": true,
                "bSortable": true,
                "sClass": "centerClass",
                "mRender": function (data, type, row) {
                    switch (row[9]) {
                        case "1":
                            return 'Accepted';
                            break;
                        case "2":
                            return 'Rejected';
                            break;
                        default:
                            return 'No Status';
                            break;
                    }
                }
            }
        ]
    });


    
    tblAppStudents = $("#tblAppstudents").DataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'>r>t", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "bDestroy": true,
        "bProcessing": true,
        "bServerSide": true,
        "cache": false,
        "iDeferLoading" : 0,
        "sAjaxSource": "/Application/AjaxHandlerStudentsList",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "appId", "value": AppId });
            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json)
            });
        },
        "oLanguage": {
            "sProcessing": '<img src="/Images/ajax-small-loader.gif" />',
            "sInfoEmpty": 'No records.',
            "sEmptyTable": "No records found.",
        },
        "aoColumns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            {
                data: "Id",
                "orderable": false,
            },
            {
                data: "StudentName",
                "orderable": false,
            },
            {
                data: "Homeroom",
                "orderable": false,
            },
            {
                data: "DateOfBirth",
                "orderable": false,
            },
            {
                data: "HasIncome",
                "orderable": false,
                "mRender": function (data, type, row) {
                    return row.HasIncome == "Yes" ? "<i class=\"fa fa-check\" style=\"margin-left:0px;\"></i>" : "";
                }
            },
            {
                data: "POS_Status",
                "orderable": false,
                "mRender": function (data, type, row) {
                    switch (row.POS_Status) {
                        case 1:
                            return 'Paid';
                            break;
                        case 2:
                            return 'Reduced';
                            break;
                        case 3:
                            return 'Free';
                            break;
                        default:
                            return 'No Status';
                            break;
                    }
                }
            },
            {
                data: "Status",
                "orderable": false,
                "mRender": function (data, type, row) {
                    switch (row.Status) {
                        case 1:
                            return 'Accepted';
                            break;
                        case 2:
                            return 'Rejected';
                            break;
                        default:
                            return 'No Status';
                            break;
                    }
                }
            },
            {
                data: "DirectCert",
                "orderable": false,
                "mRender": function (data, type, row) {
                    switch (row.DirectCert) {
                        case 1:
                            return 'Cert 1';
                            break;
                        case 2:
                            return 'Cert 2';
                            break;
                        default:
                            return 'No Cert';
                            break;
                    }
                }
            },
            {
                data: "Precertified",
                "orderable": false,
                "mRender": function (data, type, row) {
                    return (row.Precertified) ? "<i class=\"fa fa-check\" style=\"margin-left:0px;\"></i>" : "";
                }
            }
        ]
    });

    tblAppMembers = $("#tblAppMembers").DataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'>r>t", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "bDestroy": true,
        "bProcessing": true,
        "bServerSide": true,
        "cache": false,
        "iDeferLoading": 0,
        "sAjaxSource": "/Application/AjaxHandlerMembersList",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "appId", "value": AppId });
            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json)
            });
        },
        "oLanguage": {
            "sProcessing": '<img src="/Images/ajax-small-loader.gif" />',
            "sInfoEmpty": 'No records.',
            "sEmptyTable": "No records found.",
        },
        "aoColumns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            {
                data: "Id",
                "orderable": false,
            },
            {
                data: "MemberName",
                "orderable": false,
            },
            {
                data: "DateOfBirth",
                "orderable": false,
            },
            {
                data: "Email",
                "orderable": false,
            },
            {
                data: "SSN",
                "orderable": false,
            },
            {
                data: "FosterChild",
                "orderable": false,
            },
            {
                data: "HasIncome",
                "orderable": false,
                "mRender": function (data, type, row) {
                    return row.HasIncome == "Yes" ? "<i class=\"fa fa-check\" style=\"margin-left:0px;\"></i>" : "";
                }
            },
            {
                data: "IsStudent",
                "orderable": false,
                "mRender": function (data, type, row) {
                    return row.IsStudent ? "<i class=\"fa fa-check\" style=\"margin-left:0px;\"></i>" : "";
                }
            },
            {
                data: "Status",
                "orderable": false,
                "mRender": function (data, type, row) {
                    switch (row.Status) {
                        case "1":
                            return 'Accepted';
                            break;
                        case "2":
                            return 'Rejected';
                            break;
                        default:
                            return 'No Status';
                            break;
                    }
                }
            },
            {
                data: "DirectCert",
                "orderable": false,
                "mRender": function (data, type, row) {
                    switch (row.DirectCert) {
                        case "1":
                            return 'Cert 1';
                            break;
                        case "2":
                            return 'Cert 2';
                            break;
                        default:
                            return 'No Cert';
                            break;
                    }
                }
            },
            {
                data: "Precertified",
                "orderable": false,
                "mRender": function (data, type, row) {
                    return (row.Precertified) ? "<i class=\"fa fa-check\" style=\"margin-left:0px;\"></i>" : "";
                }
            }
        ]
    });


    $('#tblAppstudents tbody').on('click', 'td.details-control', function () { incomeTable(tblAppStudents, this); });
    $('#tblAppMembers tbody').on('click', 'td.details-control', function () { incomeTable(tblAppMembers, this); });

    $("#aapStudentsPopup").on("shown.bs.modal", function () {
        tblAppStudents.draw();
    });

    $("#aapMembersPopup").on("shown.bs.modal", function () {
        tblAppMembers.draw();
    });
});

$("#dlSearchBy").change(function () {
    HideShowGridColumns();
});

function foo() {
    //some delay
}

$("#SearchBtn").click(function () {
    tblApplications.fnDraw();
});

function HideShowGridColumns() {
    var searchById = $("#dlSearchBy").val();
    switch (searchById) {
        case "0": {
            tblApplications.fnSetColumnVis(2, false);
            tblApplications.fnSetColumnVis(3, false);
            tblApplications.fnSetColumnVis(6, true);
            tblApplications.fnSetColumnVis(7, true);
        }
            break;
        case "1": {
            tblApplications.fnSetColumnVis(2, false);
            tblApplications.fnSetColumnVis(3, true);
            tblApplications.fnSetColumnVis(6, false);
            tblApplications.fnSetColumnVis(7, false);
        }
            break;
        case "2": {
            tblApplications.fnSetColumnVis(2, true);
            tblApplications.fnSetColumnVis(3, false);
            tblApplications.fnSetColumnVis(6, false);
            tblApplications.fnSetColumnVis(7, false);
        }
            break;
        default: {
            tblApplications.fnSetColumnVis(2, false);
            tblApplications.fnSetColumnVis(3, false);
            tblApplications.fnSetColumnVis(6, false);
            tblApplications.fnSetColumnVis(7, false);
        }
            break;
    }
}

function ShowStudentsPopup(appId)
{
    AppId = appId;

    $("#aapStudentsPopup").modal({ keyboard: false, backdrop: 'static' });
    $("#aapStudentsPopup").modal('show');    
}

function ShowMembersPopup(appId)
{
    AppId = appId;

    $("#aapMembersPopup").modal({ keyboard: false, backdrop: 'static' });
    $("#aapMembersPopup").modal('show');
}

function ReviewApp(appId)
{
    AppId = appId;
    tblAppStudentsReview.draw();
}

function formatIncome(d) {

    if (d.HasIncome != 'Yes') return '<p>No income</p>';

    // `d` is the original data object for the row
    var ret = '<table class="incomeDetail" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">';
    ret += '<tr><th>Job Type</th><th>Frequency</th><th>Income</th></tr>';

    // Job 1
    if (d.Job1FrequencyName && d.Job1Income) {
        ret += '<tr><td>Job 1</td><td>' + d.Job1FrequencyName + '</td><td>$ ' + d.Job1Income + '</td></tr>';
    }
    // Job 2
    if (d.Job2FrequencyName && d.Job2Income) {
        ret += '<tr><td>Job 2</td><td>' + d.Job2FrequencyName + '</td><td>$ ' + d.Job2Income + '</td></tr>';
    }
    // Job 3
    if (d.Job3FrequencyName && d.Job3Income) {
        ret += '<tr><td>Job 3</td><td>' + d.Job3FrequencyName + '</td><td>$ ' + d.Job3Income + '</td></tr>';
    }
    // Welfare, Alimony, Child Support
    if (d.WelfareFrequencyName && d.WelfareIncome) {
        ret += '<tr><td>Welfare, Alimony, Child Support</td><td>' + d.WelfareFrequencyName + '</td><td>$ ' + d.WelfareIncome + '</td></tr>';
    }

    // Pensions, retirement, Social Security, SSI, VA benefits
    if (d.PensionFrequencyName && d.PensionIncome) {
        ret += '<tr><td>Pensions, retirement, Social Security, SSI, VA benefits:</td><td>' + d.PensionFrequencyName + '</td><td>$ ' + d.PensionIncome + '</td></tr>';
    }
    // All Other income
    if (d.OtherFrequencyName && d.OtherIncome) {
        ret += '<tr><td>Other income</td><td>' + d.OtherFrequencyName + '</td><td>$ ' + d.OtherIncome + '</td></tr>';
    }
    ret += '</table>';

    return ret;
}

function incomeTable(table, ths) {
    var tr = $(ths).closest('tr');
    var row = table.row(tr);

    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        // Open this row
        row.child(formatIncome(row.data())).show();
        tr.addClass('shown');
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

function clearApplicationFilters()
{
    $("#txtSearch").val("");
    $("#dlSearchBy").val("0");
    $("#dlSearchBy").select2("val", "0");

    $("#txtSignedDate").val("");
    $("#dpApprovalStatus").select2("val", "-9999");

    $("#dpEnteredOptions").val("");
    $("#dpEnteredOptions").select2();

    $("#dpUpdatedOptions").val("");
    $("#dpUpdatedOptions").select2();
}