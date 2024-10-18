
$(document).ready(function () {
    //ClearUI();
    
    // UF: Review Settings.aspx.cs Line 128
    //if (1 == 0) {
    //    $('#hdnEditPrefrences').val('false');

    //    $('#contactName').attr('disabled', 'disabled');
    //    $('#contactNumber').attr('disabled', 'disabled');
    //    $('#contactEmail').attr('disabled', 'disabled');
    //    $('#updateSettings').attr('disabled', 'disabled');
    //}
    //else {
    //    $('#hdnEditPrefrences').val('true');

    //    $('#contactName').removeAttr('disabled');
    //    $('#contactNumber').removeAttr('disabled');
    //    $('#contactEmail').removeAttr('disabled');
    //    $('#updateSettings').removeAttr('disabled');
    //}

    //loadDistrictData($('#hdnDistrictId').val());
    //$("#my-checkbox").bootstrapSwitch();
    $('.SlectBox').SumoSelect({
        //csvDispCount: 3,
        selectAll: true,
        captionFormatAllSelected: "All Selected"
    });

    if ($('#UpdateMSASettings').val() == 'False')
    {
        $('.SlectBox').html('');
        $('.SlectBox').attr('disabled', 'disabled');
        $('.SlectBox').removeAttr('multiple');
        $('.SlectBox')[0].sumo.disable();
        $('.SlectBox')[1].sumo.disable();
    }

    //$('#requestAttentionPopup').on('shown.bs.modal', function () {
    //    $('#proceed').addClass('defaultBtnClass');
    //});
    //$('#requestAttentionPopup').on('hidden.bs.modal', function () {
    //    $('#proceed').removeClass('defaultBtnClass');
    //});

});



//$("#mybtn").click(function () {


//    if (true)
//    {
//        $('##my-checkbox').html('Edit Alert');
//    }
//    var val = $("#my-checkbox").is(':checked');
//    alert(val);
//});

function ClearUI() {
    $('#DName').html('');
    $('#DAddress').html('');
    $('#bankName').html('');
    $('#routingNumber').html('');
    $('#accountNumber').html('');
    $('#achSaving').html('');
    $('#creditCard').html('');

    $('#transferFee').html('');
    $('#lblConvFee').html('');
    $('#lastUpdateDateTime').html('');
    $('#preorderPurchasing').html('');
    $('#shoppingCart').html('');

}
function GetStatusHtml(isAllowed)
{
    var status = "";
    var img = "";

    if (isAllowed)
    {
        status = "ENABLED";
        img = "../Images/circle-green.png";
    }
    else
    {
        status = "DISABLED";
        img = "../Images/circle-gray.png";
    }

    return "<span><img height='10' width='10' style='margin-bottom:2px' src='" + img + "' /></span>  " + status;
}

function loadDistrictData(districtId) {

    $.ajax({
        url: "/Settings/AjaxGetDistrict",
        data: { districtId: districtId }, // query string
        type: "GET",
        dataType: "json", // type of expected data
    })
      
      .done(function (json) { // request succeeds

          if (json != null || json !== '') {

              $('#DName').html(json.dist.Name);
              $('#DAddress').html(json.dist.Address);

              $('#bankName').html(json.dist.BankName);
              $('#routingNumber').html(json.dist.BankRouting);
              $('#accountNumber').html(json.dist.BankAccount);

              $('#achSaving').html(GetStatusHtml(json.dist.allowACH));
              $('#creditCard').html(GetStatusHtml(json.dist.allowCreditCard));
              
              if (json.UseCCFee == false) {
                  $('#transferFeeHeading').html('Student Account Transfer Fee:');
              }
              else {
                  $('#transferFeeHeading').html('ACH Transfer Fee:');
                  $('#feeDiv').append('<div class="row"><label class="col-md-offset-1 col-md-6 label-heading">Credit Card Processing Fee:</label> <label class="col-md-5"><label>' + json.dist.VariableCCFee + '</label>%</label> </div>');
              }
              $('#transferFee').html(formatDollar(json.dist.CreditFee));
              if (json.dist.AllowStudentUsageFee != null && (json.dist.AllowStudentUsageFee == 1 || json.dist.AllowStudentUsageFee == true))
              {
                  $('#feeDiv').append('<div class="row"><label class="col-md-offset-1 col-md-6 label-heading">Student Usage Fee Amount:</label> <label class="col-md-5"><label>' + formatDollar(json.dist.StudentUsageFee) + '</label></label> </div>');
              }

              if (json.dist.LastUpdate != null) {

                  var date = new Date(parseInt(json.dist.LastUpdate.substr(6)));

                  $('#lastUpdateDateTime').html(date.toLocaleString());
              }

              $('#preorderPurchasing').html(GetStatusHtml(json.dist.allowWebLunch));
              $('#shoppingCart').html(GetStatusHtml(json.dist.ShoppingCart_Id));

          }

          //var setupDate = "";
          //var lastLoginDate = "";
          //if (json.aaData.SetupDate != null && json.aaData.SetupDate != "") {
          //    setupDate = new Date(parseInt(json.aaData.SetupDate.substr(6)));
          //    var month = setupDate.getMonth() + 1;
          //    setupDate = setupDate.getDate() + "/" + month + "/" + setupDate.getFullYear() + ' ' + setupDate.getHours() + ':' + setupDate.getMinutes();
          //}
          //if (json.aaData.LastLogin != null && json.aaData.LastLogin != "") {
          //    lastLoginDate = new Date(parseInt(json.aaData.LastLogin.substr(6)));
          //    var month = lastLoginDate.getMonth() + 1;
          //    lastLoginDate = lastLoginDate.getDate() + "/" + month + "/" + lastLoginDate.getFullYear() + ' ' + lastLoginDate.getHours() + ':' + lastLoginDate.getMinutes();
          //}

          //$('#parentName').text(json.aaData.FirstName + ' ' + json.aaData.LastName);
          //$('#userId').text(json.aaData.UserID);

          //$('#address').html(json.aaData.Address + "<br />" + json.aaData.City + ", " + json.aaData.State + " " + json.aaData.Zip);
          //$('#setupDate').text(setupDate);
          //$('#phone').text(json.aaData.Phone);
          //$('#loginDate').text(lastLoginDate);
          //$('#email').text(json.aaData.Email);
          //$('#verificationCode').text(json.aaData.VerificationCode);
          //if (json.aaData.Verified) {
          //    if (json.aaData.BadParent) {
          //        $('#accountStatus').text('Inactive');
          //        $('#activeCirle1').attr("src", "../Images/circle-gray.png");
          //    }
          //    else {
          //        $('#accountStatus').text('Active');
          //        $('#activeCirle1').attr("src", "../Images/circle-green.png");
          //    }
          //}
          //else {
          //    $('#accountStatus').text('Inactive');
          //    $('#activeCirle1').attr("src", "../Images/circle-gray.png");
          //}

          //if (json.aaData.BalNotify) {
          //    $('#lowBalanceNotification').text('Active');
          //    $('#activeCirle2').attr("src", "../Images/circle-green.png");
          //}
          //else {
          //    $('#lowBalanceNotification').text('Inactive');
          //    $('#activeCirle2').attr("src", "../Images/circle-gray.png");
          //}

          //// MISC Tab Data
          //$("#oldEmailAddress").text(json.aaData.Email);

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




$('#updateSettings').click(function () {

    // Save

    var dataString = GetDataString();
    document.getElementById("contactNameInDialog").innerHTML = document.getElementById("contactName").value;

    $.ajax({
        type: "POST",
        url: "/Settings/AjaxUpdateDistrictSettings",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //if (($('#hdnAlertId').val() == "" && data == -1) ||
            //    ($('#hdnAlertId').val() != "" && data == 1))
            //{

            //$('#infoTitle').html('Message');
            //$('#infoMessage').html('Settings updated successfully');
            //$('#informationModal').modal('show');
            //return false;
            if (data != 0) {
                document.getElementById("contactNameInDialog").innerHTML = document.getElementById("contactName").value;
                displaySuccessMessage("Settings updated successfully");
            }
            else {
                displayErrorMessage('Something went wrong');
            }
                //ChangeUpdatedDate();
                //$('#parentAlertPopup').modal('toggle'); // close modal

                //}
                //else {
                //    displayErrorMessage('Something went wrong');
                //    return false;
                //}
        },
        error: function (request, status, error) {
            displayErrorMessage('Error in deleting parent alert data');
            return false;
        }
    });
});

function ChangeUpdatedDate() {
    var d = new Date();
    var weekday = new Array(7);
    weekday[0] = "Sunday";
    weekday[1] = "Monday";
    weekday[2] = "Tuesday";
    weekday[3] = "Wednesday";
    weekday[4] = "Thursday";
    weekday[5] = "Friday";
    weekday[6] = "Saturday";

    var dayName = weekday[d.getUTCDay()];

    var monthNames = ["January", "February", "March", "April", "May", "June",
                                "July", "August", "September", "October", "November", "December"];

    var monthName = monthNames[d.getMonth()];
    var day = d.getUTCDate();
    var year = d.getUTCFullYear();
    var hour = d.getUTCHours();
    var min = d.getUTCMinutes();
    var sec = d.getUTCSeconds();
    $("#lastUpdateDateTime").html(dayName + ", " + monthName + " " + day + ", " + year + " (" + hour + ":" + min + ":" + sec + ")");
}


$('#updateCartOptions').click(function () {

    // Save

    var validatePreorderItemStatus = $('#validatePreorderItemStatus').is(':checked') ? "true" : "false";
    var allowPreorderNegBalances = $('#allowPreorderNegBalances').is(':checked') ? "true" : "false";

    var dataString = JSON.stringify({
        dataToUpload: validatePreorderItemStatus + "*" + allowPreorderNegBalances
    });


    $.ajax({
        type: "POST",
        url: "/Settings/AjaxUpdateDistrictOptions",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            

            if (data != 0) {
                displaySuccessMessage("Cart options updated successfully");
            }
            else {
                displayErrorMessage("Something went wrong.");
            }
                
        },
        error: function (request, status, error) {
            displayErrorMessage('Error in deleting parent alert data');
            return false;
        }
    });
});

$('#updateOptions').click(function () {

    // Save

    var msaAlert = $('#displayMsaAlertFirst').is(':checked') ? "true" : "false";

    var dataString = JSON.stringify({ dataToUpload: msaAlert });
   // debugger;
    
    $.ajax({
        type: "POST",
        url: "/Settings/AjaxUpdateCommunicationOptions",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (data != 0) {
              //  debugger;
                displaySuccessMessage("Communication Option updated successfully");
            }
            else {
                displayErrorMessage("Something went wrong.");
            }

        },
        error: function (request, status, error) {
            displayErrorMessage('Error in deleting parent alert data');
            return false;
        }
    });
});


function GetDataString() {

    var contactName = document.getElementById("contactName").value;
    var contactNumber = document.getElementById("contactNumber").value;
    var contactEmail = document.getElementById("contactEmail").value;
    var lowBalanceNotification = $('#lowBalanceNotification').is(':checked') ? "true" : "false";
    var allowStudentTransfers = $('#allowStudentTransfers').is(':checked') ? "true" : "false";

    var studentAttachment;
    if ($('#studentAttachment').length !== 0) {
        var studentAttachment = $('#studentAttachment').is(':checked') ? "true" : "false";
    }
    else {
        studentAttachment = "N/A";
    }

    var displayVoids = $('#displayVoids').is(':checked') ? "true" : "false";
    var displayAdjustments = $('#displayAdjustments').is(':checked') ? "true" : "false";

    var preorderTax_SelectedValues = "";
    var i = 0;
    $('#preorderTax option:selected').each(
        function () {
            preorderTax_SelectedValues = preorderTax_SelectedValues + $(this).val() + ",";
            i++;
        });
    if (preorderTax_SelectedValues != "") {
        preorderTax_SelectedValues = removeLastComma(preorderTax_SelectedValues);
    }
    else {
        preorderTax_SelectedValues = "-1";
    }

    var easyPayTax_SelectedValues = "";
    var j = 0;
    $('#easyPayTax option:selected').each(
        function () {
            easyPayTax_SelectedValues = easyPayTax_SelectedValues + $(this).val() + ",";
            j++;
        });
    if (easyPayTax_SelectedValues != "") {
        easyPayTax_SelectedValues = removeLastComma(easyPayTax_SelectedValues);
    }
    else {
        easyPayTax_SelectedValues = "-1";
    }

    var cutOff_5;
    var forcePaymentNegBalance;
    if ($('#cutOff_5').length !== 0) {
        cutOff_5 = $('#cutOff_5').is(':checked') ? "true" : "false";
        //$('#cutOff_7').is(':checked') ? "true" : "false"; No need to check 7 because we have boolean at backend for isFive
        forcePaymentNegBalance = $('#forcePaymentNegBalance').is(':checked') ? "true" : "false";
    }
    else {
        cutOff_5 = "N/A";
        forcePaymentNegBalance = "N/A";
    }

    var dataString = JSON.stringify({
        dataToUpload: contactName + "*" +
                            contactNumber + "*" +
                            contactEmail + "*" +
                            lowBalanceNotification + "*" +
                            allowStudentTransfers + "*" +
                            studentAttachment + "*" +
                            displayVoids + "*" +
                            displayAdjustments + "*" +
                            preorderTax_SelectedValues + "*" +
                            easyPayTax_SelectedValues + "*" +
                            cutOff_5 + "*" +
                            forcePaymentNegBalance 

    });

    return dataString;
}

function removeLastComma(str) {
    return str.replace(/,$/, "");
}


$('#proceed').click(function () {
    $('hdnProceedClicked').val('1');
    var type = $('#hdnType').val();
    RequestAttention(type);

    });

$('#paymentType').click(function () {
    
    $('#hdnType').val('');
    if ($('#paymentType').is(':checked')) {
        $('#hdnType').val('payment');
        //console.log("payment");
        //$('#requestAttentionPopup').modal('show');
    }
});

$('#bankInfo').click(function () {

    $('#hdnType').val('');
    if ($('#bankInfo').is(':checked')) {
        $('#hdnType').val('bankInfo');

        //console.log("bankInfo");
        //$('#requestAttentionPopup').modal('show');
    }
    
});

$('#requestAttentionPopup').on('hidden.bs.modal', function () {

    $('#paymentType').removeAttr('checked');
    $('#paymentType').parent().removeClass('checked');

    $('#bankInfo').removeAttr('checked');
    $('#bankInfo').parent().removeClass('checked');

});


function RequestAttention(requestType) {

    var txtName = document.getElementById('contactName').value;
    var txtEmail = document.getElementById('contactEmail').value;
    if (txtName == "")
        txtName = "NA";
    if (txtEmail == "")
        txtEmail = "NA";

    var changeTypeClient = $("#paymentType").is(':checked') ? '1' : '0'
    var changeBankClient = $("#bankInfo").is(':checked') ? '1' : '0'
    
    var dataString = JSON.stringify({
        allData: txtName + "*" +
                            txtEmail + "*" +
                            requestType + "*" +
                            changeTypeClient + "*" +
                            changeBankClient
    });
    
    $.ajax({
        type: "POST",
        url: "/Settings/AjaxRequestAttention",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data == 1) {
                ChangeUpdatedDate();
                displaySuccessMessage("Email sent successfully.");
                //if (requestType == 'payment')
                //{ 
                //    $('#paymentType').attr('checked', true);
                //    $('#paymentType').parent().addClass('checked');

                //}
                //else if (attentionType == "bank")
                //{
                //    $('#bankInfo').attr('checked', true);
                //    $('#bankInfo').parent().addClass('checked');
                //}
            }
            else {
                displayErrorMessage("Something went wrong.");
            }
        },
        error: function (request, status, error) {
            
            displayErrorMessage("An error occurred while sending email.");
            return false;
        }
    });
}
/*
Bug: 1642
$('#requestAttentionPopup').on('shown.bs.modal', function (e) {
    $('#proceed').addClass('defaultBtnClass');
})

$('#requestAttentionPopup').on('hidden.bs.modal', function () {
    $('#proceed').removeClass('defaultBtnClass');
});

*/