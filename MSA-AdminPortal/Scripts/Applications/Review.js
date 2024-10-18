/// <reference path="C:\Food Service\POS\Admin\DEV-FORMS_View_Print\MSA-AdminPortal\Content/themes/assets/global/plugins/datatables/all.min.js" />

var tblAppStudentsReview;
var tblAppMembersReview;
var AppId = 0;

$(document).ready(function () {
    AppId = $("#appId").val();
    tblAppStudentsReview = $("#tblAppstudentsReviwe").DataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'>r>t", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "bDestroy": true,
        "bProcessing": true,
        "bServerSide": true,
        "cache": false,
        //"iDeferLoading": 0,
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


    tblAppMembersReview = $("#tblAppMembersReview").DataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'>r>t", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "bDestroy": true,
        "bProcessing": true,
        "bServerSide": true,
        "cache": false,
        //"iDeferLoading": 0,
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

    $('#tblAppstudentsReviwe tbody').on('click', 'td.details-control', function () { incomeTable(tblAppStudentsReview, this); });
    $('#tblAppMembersReview tbody').on('click', 'td.details-control', function () { incomeTable(tblAppMembersReview, this); });

});


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

$("#btnAccept").click(function () {

    $("#acceptStringMsg").html("accept");
    $("#btnSubmit").attr('data-id', 1);
    $("#approvalConfirmation").modal({ keyboard: false, backdrop: 'static' });
    $("#approvalConfirmation").modal('show');
});

$("#btnReject").click(function () {

    $("#acceptStringMsg").html("reject");
    $("#btnSubmit").attr('data-id', 2);
    $("#approvalConfirmation").modal({ keyboard: false, backdrop: 'static' });
    $("#approvalConfirmation").modal('show');
});

$("#btnSubmit").click(function () {
    
    var status = $(this).attr('data-id');
    ToggleAppStatus(status);
});


function ToggleAppStatus(status) {
    var appId = $("#appId").val();
    var stringData = JSON.stringify({ appId: appId, status: status, comment: $("#txtAreaComments").val() });

    $.ajax({
        type: "POST",
        url: "/Application/ToggleAppStatus",
        contentType: 'application/json',
        dataType: 'json',
        data: stringData,
        success: function (data) {
            if (data == true) {
                if (status == 1)
                    displaySuccessMessage("Application accepted successfully.");
                else
                    displaySuccessMessage("Application rejected successfully.");
                //setTimeout(function () { }, 3000);
                window.location.href = '/Application/'
            }
            else {
                if(status == 1)
                    displayErrorMessage("Error occured while accepting application.");
                else
                    displayErrorMessage("Error occured while rejecting application.");
            }
        },
        error: function (request, status, error) {
            displayErrorMessage("Error occured while accepting/rejecting application.");
        }
    });
}

$("#txtAreaComments").keydown(function (event) {
    var key = event.keyCode;
    // If the user has pressed enter
    if (key === 13) {
        $("#txtAreaComments").val($("#txtAreaComments").val() + "\n");
        return false;
    }
    else {
        return true;
    }
});
