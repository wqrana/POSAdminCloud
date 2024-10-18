

var parent_Id = null;
var oParentGrid = null;
var nEditing = null;
var lowBalSettingsGrid = null;
$(document).ready(function () {

    $("#searchdll").select2();

    oParentGrid = $('#dtParentsGrid').DataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-4 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100],
            [10, 25, 50, 100] // change per page values here
        ],
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": "/Parents/AjaxParentList",
        // set the initial value
        "iDisplayLength": 25,
        "sPaginationType": "bootstrap_full_number",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push(
                    { "name": "SearchValue", "value": $("#SearchStr").val() },
                    { "name": "SearchBy", "value": $("#searchdll").val() }
                );
            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json);
                jQuery("#SearchStr").focus();
            });
        },

        "oLanguage": {
            //"sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": { "sPrevious": "Prev", "sNext": "Next" },
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records.',
            "sInfoEmpty": 'No records.',
            "sEmptyTable": "No records found.",
        },
        "aoColumns": [
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.UserID + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div><a href=\"#parentDetailPopup\"  class=\"EditSecurityClass\" ' + 'onclick=loadParentPopupData(\"' + row.Id + '\");  data-toggle=\"modal\" data-backdrop=\"static\" data-keyboard=\"false\" >' + row.LastName + ', ' + row.FirstName + '</a></div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.Email + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    var s2 = ("" + row.Phone).replace(/\D/g, '');
                    var m = s2.match(/^(\d{3})(\d{3})(\d{4})$/);
                    var phone = (!m) ? "" : "(" + m[1] + ") " + m[2] + "-" + m[3];
                    return '<div>' + phone + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                //"mRender": function (data, type, row) {
                //    return '<div>' + row.NumberOfStudents + '</div>';
                //},
                "mRender": function (data, type, row) {
                    return '<div><a href=\"#studentDetailPopup\"  class=\"EditSecurityClass\" ' + 'onclick=loadStudentPopupData(\"' + row.Id + '\");  data-toggle=\"modal\" data-backdrop=\"static\" data-keyboard=\"false\" >' + row.NumberOfStudents + '</a></div>';
                }
            }

        ],

        //initComplete: function () {
        //    var input = $('.dataTables_filter input').unbind(),
        //    self = this.api(),
        //    $searchButton = $('<button>')
        //                .text('search')
        //                .addClass('btn yellow')
        //                .attr("style", "width: 120px;")
        //                .click(function () {
        //                    self.search(input.val()).draw();
        //                   })
        //    $('.dataTables_filter').append($searchButton);

        //}
    });

    

    $("#SearchBtn").click(function (e) {
        oParentGrid.draw();
    });

    $('#SearchStr').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            oParentGrid.draw();
            return false;
        }
    });

    $('#dtBalancenotification tbody').on('click', 'tr td:nth-child(4)', function () {

        if (this.innerHTML.indexOf('input') > 0) {
            return;
        }

        var nRow = $(this).closest('tr');

        if (nEditing !== null && nEditing != nRow) {
            /* Currently editing - but not this row - restore the old before continuing to edit mode */
            RestoreRow(lowBalSettingsGrid, nEditing);
            EditRow(lowBalSettingsGrid, nRow);
            nEditing = nRow;
        } else {
            /* No edit in progress - let's start one */
            EditRow(lowBalSettingsGrid, nRow);
            nEditing = nRow;
        }
    });

});

function loadParentPopupData(parentId)
{
    parent_Id = parentId;
    loadInformationTabData(parentId);
    loadStudentTabData(parentId);
    loadDepositHistoryTabData(parentId);
    loadLowBalSettingsTabData(parentId);
}

function loadInformationTabData(parentId)
{
    /* Information Tab Data */
    $.ajax({
        url: "/Parents/AjaxParent",
        // The data to send (will be converted to a query string)
        data: {
            parentId: parentId
        },
        type: "GET",
        // The type of data we expect back
        dataType: "json",
    })
      // Code to run if the request succeeds (is done). The response is passed to the function
      .done(function (json) {
          var setupDate = "";
          var lastLoginDate = "";
          if (json.aaData.SetupDate != null && json.aaData.SetupDate != "") {
              setupDate = new Date(parseInt(json.aaData.SetupDate.substr(6)));
              setupDate = formatDate(setupDate, 'dt');
             // var month = setupDate.getMonth() + 1;
              // setupDate = setupDate.getDate() + "/" + month + "/" + setupDate.getFullYear() + ' ' + setupDate.getHours() + ':' + setupDate.getMinutes();
              
          }
          if (json.aaData.LastLogin != null && json.aaData.LastLogin != "") {
              lastLoginDate = new Date(parseInt(json.aaData.LastLogin.substr(6)));
             // var month = lastLoginDate.getMonth() + 1;
              // lastLoginDate = lastLoginDate.getDate() + "/" + month + "/" + lastLoginDate.getFullYear() + ' ' + lastLoginDate.getHours() + ':' + lastLoginDate.getMinutes();
              lastLoginDate = formatDate(lastLoginDate,'dt')
          }

          $('#parentName').text(json.aaData.FirstName + ' ' + json.aaData.LastName);
          $('#userId').text(json.aaData.UserID);

          $('#address').html(json.aaData.Address + "<br />" + json.aaData.City + ", " + json.aaData.State + " " + json.aaData.Zip);
          $('#setupDate').text(setupDate);
          $('#phone').text(json.aaData.Phone);
          $('#loginDate').text(lastLoginDate);
          $('#email').text(json.aaData.Email);
          $('#verificationCode').text(json.aaData.VerificationCode);
          if (json.aaData.Verified) {
              if (json.aaData.BadParent) {
                  $('#accountStatus').text('Inactive');
                  $('#activeCirle1').attr("src", "../Images/circle-gray.png");
              }
              else {
                  $('#accountStatus').text('Active');
                  $('#activeCirle1').attr("src", "../Images/circle-green.png");
              }
          }
          else {
              $('#accountStatus').text('Inactive');
              $('#activeCirle1').attr("src", "../Images/circle-gray.png");
          }

          if (json.aaData.BalNotify) {
              $('#lowBalanceNotification').text('Active');
              $('#activeCirle2').attr("src", "../Images/circle-green.png");
          }
          else {
              $('#lowBalanceNotification').text('Inactive');
              $('#activeCirle2').attr("src", "../Images/circle-gray.png");
          }

          // MISC Tab Data
          $("#oldEmailAddress").text(json.aaData.Email);

      })
      // Code to run if the request fails; the raw request and
      // status codes are passed to the function
      .fail(function (xhr, status, errorThrown) {
          //alert("Sorry, there was a problem!");
          console.log("Error: " + errorThrown);
          console.log("Status: " + status);
          console.dir(xhr);
      })
      // Code to run regardless of success or failure;
      .always(function (xhr, status) {
          //alert("The request is complete!");
      });
}

var oStudentGrid = null;
function loadStudentTabData(parentId)
{
    /* Student Tab Data */
    oStudentGrid = $('#dtStudentGrid').DataTable({
        "sDom": "t",
        "bDestroy": true,
        "sAjaxSource": "/Parents/AjaxStudentList?parentId=" + parentId,

        "aoColumns": [
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.StudentId + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.FirstName + ' ' + row.LastName + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.SchoolName + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    // return '<div>' + row.Balance + '</div>';
                    return '<div>' + formatDecimal(row.Balance) + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    if (row.Active) {
                        return '<div class="success">Active</div>';
                    }
                    else {
                        return '<div class="danger">Inactive</div>';
                    }
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div><a title="Delete" href="#studentDeleteWarning" onclick=setStudentName(' + row.StudentId + ',' + parentId + ',"' + row.FirstName + '","' + row.LastName + '"); role="button" class="DeleteSecurityClass" data-toggle="modal" data-backdrop="static" data-keyboard="false"><i class="fa fa-trash fasize"></i></a></div>';
                    //return '<div><a title="Delete" href="#studentDeleteWarning" onclick=alert(\"usman\"); class="DeleteSecurityClass" data-toggle="modal"><i class="fa fa-trash fasize"></i></a></div>';
                }
            }
        ]
    });
}
function loadDepositHistoryTabData(parentId) {
    /* Deposit History Tab */

    $('#dtDepositHistoryGrid').DataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-4 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100],
            [10, 25, 50, 100] // change per page values here
        ],
        "bDestroy": true,
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": "/Parents/AjaxDepositHistory",
        // set the initial value
        "iDisplayLength": 10,
        "sPaginationType": "bootstrap_full_number",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push(
                    { "name": "ParentId", "value": parentId }
                );
            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json);
                //jQuery("#SearchStr").focus();
            });
        },

        "oLanguage": {
            //"sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": { "sPrevious": "Prev", "sNext": "Next" },
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records.',
            "sInfoEmpty": 'No records.',
            "sEmptyTable": "No records found.",
        },
        "aoColumns": [
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.TransactionID + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "25%",
                "sClass": 'centered-cell',
                "mRender": function (data, type, row) {
                    //debugger;
                    var date = new Date(parseInt(row.TransactionDate.substr(6)));
                   // var dateStr =  moment(date).format('MM/DD/YYYY  hh:mm A');
                    var dateStr = formatDate(date, 'dt');
                   // var month = date.getMonth() + 1;
                    return  dateStr;
                }
            },
            {
                "bSortable": true,
                "sWidth": "18%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.PaymentType + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "sClass": 'right-cell',
                "mRender": function (data, type, row) {

                    
                    return formatDecimal(row.TransactionTotal) ;
                }
            },
            {
                "bSortable": true,
                "sWidth": "19%",
                "sClass": 'right-cell',
                "mRender": function (data, type, row) {
                    return formatDecimal(row.NsfFee );
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.ReturnReason + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "18%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.PaymentStatus + '</div>';
                }
            }

        ]

        //initComplete: function () {
        //    var input = $('.dataTables_filter input').unbind(),
        //    self = this.api(),
        //    $searchButton = $('<button>')
        //                .text('search')
        //                .addClass('btn yellow')
        //                .attr("style", "width: 120px;")
        //                .click(function () {
        //                    self.search(input.val()).draw();
        //                   })
        //    $('.dataTables_filter').append($searchButton);

        //}
    });
}



function loadStudentPopupData(parentId)
{
    $('#dtStudentDetailGrid').DataTable({
        "sDom": "t",
        "bDestroy": true,
        //"sAjaxSource": "/Parents/AjaxStudentDetail?parentId=" + parentId,
        "sAjaxSource": "/Parents/AjaxStudentList?parentId=" + parentId,

        "aoColumns": [
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.StudentId + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.FirstName + ' ' + row.LastName + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    if (row.DOB != null) {
                        var date = new Date(parseInt(row.DOB.substr(6)));
                        //var month = date.getMonth() + 1;
                        //return '<div>' + date.getDate() + "/" + month + "/" + date.getFullYear() + '</div>';

                        return formatDate(date, 'd');
                    }
                    return '';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.SchoolName == null ? '' : row.SchoolName + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.Grade == null ? '' : row.Grade + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.HomeRoom == null ? '' : row.HomeRoom + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    //debugger;
                    return '<div>' +formatDecimal(row.Balance) + '</div>';
                }
            }

        ]
    });
}



var student_Id = null;

function setStudentName(std_id, parentId, firstName, lastName) {

    student_Id = std_id;
    //parent_Id = parentId;
    $("#studentNameH").html(firstName + " " + lastName);
}

$("#btnConfirmDelete").click(function (e) {
    DeleteStudent();
});

function DeleteStudent() {
    
    $.ajax({
        type: "POST",
        url: "/Parents/AjaxDeleteStudent",
        data: JSON.stringify({
            
            studentId: student_Id,
            parentId: parent_Id
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {
            if (json.Data == '-1') {
                displaySuccessMessage("The student record has been deleted successfully.");
                //oStudentGrid.draw();
                loadStudentTabData(parent_Id);
            }
            else {
                alert(json.Data);
            }
        },
        error: function (request, status, error) {
            displayErrorMessage('Error in deleting student data');
            return false;
        }
    });
}

$("#btnChangeEmail").click(function (e) {

    $("#btnChangeEmail").prop("disabled", true);

    if (($("#newEmailAddress").val() == "") || ($("#confirmEmailAddress").val() == "")) {
        //alert('You must provide the new email address in both "New Email Address" and also "Confirm Email address".');
        displayWarningMessage('New Email & Confirm Email Addresses are required fields.');
        $("#btnChangeEmail").prop("disabled", false);
        return false;
        //document.getElementById('spnError').style.display = 'none';
        //document.getElementById('EmailDiv').style.display = 'none';

    }
    else if ((!isValidEmailAddress($("#newEmailAddress").val())) || (!isValidEmailAddress($("#confirmEmailAddress").val()))) {
        //alert('New Email/Confirm Email Address is not valid.');
        displayWarningMessage('New Email/Confirm Email Address is not valid.');
        $("#btnChangeEmail").prop("disabled", false);
        return false;
        //document.getElementById('spnError').style.display = 'none';
        //document.getElementById('EmailDiv').style.display = 'none';


    }
    else if ($("#newEmailAddress").val() != $("#confirmEmailAddress").val()) {
        //alert('Both email addresses are not equal.');
        displayWarningMessage('New Email & Confirm Email Addresses are not same.');
        $("#btnChangeEmail").prop("disabled", false);
        return false;
    }
    else {

        $.ajax({
            type: "POST",
            url: "/Parents/AjaxUpdateParentEmail",
            data: JSON.stringify({

                parentId: parent_Id,
                email: $("#newEmailAddress").val()
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (json) {

                if (json.Data == '1') {

                    $("#oldEmailAddress").html($("#newEmailAddress").val());
                    $("#newEmailAddress").val('');
                    $("#confirmEmailAddress").val('');
                    displaySuccessMessage('Email changed successfully.');
                }
                else {

                    displayErrorMessage('Email not changed.');
                }
            },
            error: function (request, status, error) {
                displayErrorMessage('Error occurred');
                return false;
            }
        })
        .always(function (xhr, status) {
            $("#btnChangeEmail").prop("disabled", false);
        });
    }
});

$("#btnResetPassword").click(function (e) {
    $("#btnResetPassword").prop("disabled", true);
    $.ajax({
        type: "POST",
        url: "/Parents/AjaxChangePassword",
        data: JSON.stringify({

            parentId: parent_Id
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {
            if (json.Data == '1') {
                displaySuccessMessage('Password reset successfully.');
            }
            else {
                displayErrorMessage('Password did not reset. Something went wrong.');
            }
        },
        error: function (request, status, error) {
            displayErrorMessage('Error occurred');
            return false;
        },
        
    })
    .always(function (xhr, status) {
        $("#btnResetPassword").prop("disabled", false);
    });

});

function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
    return pattern.test(emailAddress);
};

$('#parentDetailPopup').on('hidden.bs.modal', function () {
    nEditing = null;
    oParentGrid.draw();
});

function formatDecimal(amount) {
    //debugger;
    if (!isNaN(amount)) {
        var outAmount = parseFloat(Math.round(amount * 100) / 100).toFixed(2);
        if (outAmount == 0.00) {
            return '$0.00';
        }
        else {

            return '$' + outAmount;

        }
    }
    else {
        return amount;
    }
            
}

function formatDate(date, format)
{
    if (format == 'dt') {
        return moment(date).format('MM/DD/YYYY  hh:mm A');
    }
    else {
        return moment(date).format('MM/DD/YYYY');
    }

}

function loadLowBalSettingsTabData(parentId) {

    $(".lowBalCheck").parent().removeClass('checked');

    $.ajax({
        type: "GET",
        url: "/Parents/GetLowBalanceSettings",
        data: { parentId: parentId },
        ContentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(result){
            $("#cbxPaymentNotify").parent().addClass(result.PaymentNotify ? 'checked' : '');
            $("#cbxVIPNotify").parent().addClass(result.VIPNotify ? 'checked' : '');
            $("#cbxPreorderNotify").parent().addClass(result.PreorderNotify? 'checked' : '');
            $('#cbxLBN').parent().addClass(result.BalNotify ? 'checked' : '');
            $('#txtEmail').val(result.Email);
        },
        error: function (request, status, error) {
            displayErrorMessage('Error occurred while getting low balance settings.');
            return false;
        }
    })

    // Data for Low balance Settings/Notification tab
    lowBalSettingsGrid = $('#dtBalancenotification').dataTable({
        "sDom": "t",
        "bDestroy": true,
        "oLanguage":
        {
            "sEmptyTable": 'Currently, there are no students assigned to this parent account. To add a student, go to <a href=https://secure.myschoolaccount.com target=_blank>myschoolaccount.com</a> website, and select [Add Student] from the main menu and follow the screen prompts.'
        },
        "sAjaxSource": "/Parents/AjaxLowBalStudentsList?parentId=" + parentId,
        "aoColumns": [
            {
                "sName": "StudentId",
                "className": 'hidden'
            },
            {
                "sName": "StudentName",
                "sWidth": "40%",
            },
            {
                "sName": "CurrentBalance",
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return formatDecimal(row[2]);
                }
            },
            {
                "sName": "MinimumBalance",
                "sWidth": "20%",
                "mRender": function (data, type, row) {
                    return formatDecimalWithoutDollarSign(row[3]);
                }
            },
            {
                "sName": "IsNotifyEnabled",
                "sWidth": "20%",
                "sClass": "centerClass",
                "bSortable": false,
                "mRender": function (data, type, row) {
                    if (row[4] == 'True') {
                        return "<input type=\"checkbox\" id=\"chk" + row[0] + "\" onclick=\"CheckStudentLowBal(this, " + row[0] + ")\" name=\"" + row[0] + "\" checked=\"checked\" />";
                    }
                    else {
                        return "<input type=\"checkbox\" id=\"chk" + row[0] + "\" onclick=\"CheckStudentLowBal(this, " + row[0] + ")\" name=\"" + row[0] + "\" />";
                    }
                }
            }
        ]
    });
};

function CheckStudentLowBal(checkBox, studentId) {
    var nRow = $(checkBox).closest('tr');
    if (checkBox.checked) {
        lowBalSettingsGrid.fnUpdate('True', nRow, 4, false);
    }
    else {
        lowBalSettingsGrid.fnUpdate('False', nRow, 4, false);
    }
    // Updating a row re-generates a row and checkboxes loses their states. therefore, we restore them.
    var aData = lowBalSettingsGrid.fnGetData(nRow);
    restoreLowBalCheckbox(aData);
}

// Save low balance settings
$("#btnSaveChanges").click(function () {
    var Email = '#txtEmail';
    var EmailVer = ValidateEmail(Email, "Email is not valid.");
    if (!EmailVer) return;

    var mainSettings = new Object();
    mainSettings.Id = parent_Id;
    mainSettings.PaymentNotify = $("#cbxPaymentNotify").parent().hasClass("checked") ? true : false;
    mainSettings.VIPNotify = $("#cbxVIPNotify").parent().hasClass("checked") ? true : false;
    mainSettings.PreorderNotify = $("#cbxPreorderNotify").parent().hasClass("checked") ? true : false;
    mainSettings.BalNotify = $("#cbxLBN").parent().hasClass("checked") ? true : false;
    mainSettings.Email = $("#txtEmail").val();

    var students = new Array();
    
    var gridData = lowBalSettingsGrid.fnGetData();
    for (var i = 0; i < gridData.length; i++) {
            console.log(gridData[i]);
            var student = new Object();
            student.StudentId = gridData[i][0];
            student.MinimumBalance = gridData[i][3];
            student.IsNotifyEnabled = gridData[i][4];

            students.push(student);
    }

    var stringData = JSON.stringify({ mainSettings: mainSettings, students: students });

    $.ajax({
        type: "POST",
        url: "/Parents/SaveLowBalanceSettings",
        data: stringData,
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            if (parseInt(result) > 0) {
                displaySuccessMessage('Changes saved successfully.');
                //oParentGrid.draw();
                //$("#parentDetailPopup").modal().hide();
            }
            else {
                displayErrorMessage('An error occurred while saving low balance settings.');
            }
        },
        error: function (request, status, error) {
            displayErrorMessage('An error occurred while saving low balance settings.');
            return false;
        }
    });
});

function RestoreRow(oTable, nRow) {
    var aData = oTable.fnGetData(nRow);
    var jqTds = $('>td', nRow);
    
    for (var i = 0, iLen = jqTds.length; i < iLen; i++) {
        oTable.fnUpdate(aData[i], nRow, i, false);
    }
}

function EditRow(oTable, nRow) {
    var aData = lowBalSettingsGrid.fnGetData(nRow);
    var jqTds = $('>td', nRow);
    var minValue = formatDecimalWithoutDollarSign(aData[3]);
    jqTds[3].innerHTML = '<input id="QtyInput' + aData[0] + '" type="text" class="form-control" maxlength= 10 value="' + minValue + '")/>';

    $("#QtyInput" + aData[0]).enterKey(function () {
        SaveRow(oTable, nRow, aData[0]);
    });

    $("#QtyInput" + aData[0]).escKey(function () {
        CancelEditRow(oTable, nRow);
    });

    restoreLowBalCheckbox(aData);
}

function SaveRow(oTable, nRow, input) {

    var newValue = $("#QtyInput" + input).val();

    if (newValue == null || newValue == '') return;

    newValue = formatDecimalWithoutDollarSign(newValue);

    var aData = oTable.fnGetData(nRow);
    if (newValue == '') {
        displayErrorMessage("The minimum balance cannot be empty.")
        return;
    }

    oTable.fnUpdate(newValue, nRow, 3, false);
}

function CancelEditRow(oTable, nRow) {
    var aData = oTable.fnGetData(nRow);
    oTable.fnUpdate(aData[3], nRow, 3, false);
}

function formatDecimalWithoutDollarSign(newValue) {
    val = new Number(newValue);
    if (val > Number(0.0))
        return val.toFixed(2);
    else
        return ".00";
}

function restoreLowBalCheckbox(rowData) {
    if (rowData[4] == 'True') {
        $("#chk" + rowData[0]).attr("checked", true);
    }
    else {
        $("#chk" + rowData[0]).attr("checked", false);
    }
}

$("#btnSendTestEmail").click(function () {

    var Email = '#txtEmail';
    var EmailVer = ValidateEmail(Email, "Email is not valid.");
    if (!EmailVer) return;

    var stringData = JSON.stringify({ email: $("#txtEmail").val() });
    $.ajax({
        type: "POST",
        url: "/Parents/SendLowBalTestEmail",
        data: stringData,
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            if (result == true) {
                displaySuccessMessage('Test email sent successfully.');
            } else {
                displayErrorMessage('An error occurred while sending test email.');
            }
        },
        error: function (request, status, error) {
            displayErrorMessage('An error occurred while sending test email.');
            return false;
        }
    });

});


function ValidateEmail(elem, message) {

    var element = $(elem);
    if (element.val() == "" || element.val().length == 0) {
        displayWarningMessage('Email address is a required field.');
        return false;
    }
    else if ((!isValidEmailAddress(element.val()))) {

        if (element.val().indexOf(';') > 0) {
            var allEmailsValid = true;
            element.val().split(';').forEach(function (item) {
                if (!isValidEmailAddress(item)) {
                    allEmailsValid = false;
                }
            });

            if (allEmailsValid) {
                return true;
            }
        }
        displayWarningMessage('invalid Email address(es).');
        return false;
    }

    return true;
}