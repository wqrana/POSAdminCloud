function updateDistrict() {
    //debugger;
    if (ValidateDistrictData()) {
        var clientId = document.getElementById("ClientId").value;
        var distid = document.getElementById("districtId").value;

        var distName = document.getElementById('DistrictName').value;
        var addr1 = document.getElementById('Address1').value;
        var addr2 = document.getElementById('Address2').value;
        var cityname = document.getElementById('City').value;
        var statename = $('#dllState').val();
        var zip = document.getElementById('zip').value;
        var PhoneNo1 = document.getElementById('Phone1').value;
        var PhoneNo2 = document.getElementById('Phone2').value;

        var isStudentPaidTaxable = $('#isStudentPaidTaxable').is(':checked');
        var isStudentRedTaxable = $('#isStudentRedTaxable').is(':checked');
        var isStudentFreeTaxable = $('#isStudentFreeTaxable').is(':checked');
        var isEmployeeTaxable = $('#isEmployeeTaxable').is(':checked');
        var isMealPlanTaxable = $('#isMealPlanTaxable').is(':checked');
        var isGuestTaxable = $('#isGuestTaxable').is(':checked');
        var isStudCashTaxable = $('#isStudCashTaxable').is(':checked');

        var StartSchoolYear = document.getElementById('StartSchoolYear').value;
        var EndSchoolYear = document.getElementById('EndSchoolYear').value;

        var BankRoute = document.getElementById('BankRoute').value;

        var BankAccount = document.getElementById('BankAccount').value;
        var BankName = document.getElementById('BankName').value;
        var BankAddr1 = document.getElementById('BankAddr1').value;
        var BankAddr2 = document.getElementById('BankAddr2').value;

        var BankCity = document.getElementById('BankCity').value;
        var dllBankState = $('#dllBankState').val();
        var BankZip = document.getElementById('BankZip').value;

        var dllDirector = document.getElementById('hddirectorID').value; //$('#dllDirector').val();
        var dllAdmin = document.getElementById('hdadminID').value; //$('#dllAdmin').val();

        var phone1Ver = ValidatePhone('#Phone1', "Enter a valid phone number");
        var phone2Ver = ValidatePhone('#Phone2', "Enter a valid phone number");
        var errorbar = $('.alert-danger');

        if (phone1Ver && phone2Ver) {

            errorbar.hide();

            var dataString = JSON.stringify({
                allData: clientId + "*" +
                         distid + "*" +
                         distName + "*" +
                         addr1 + "*" +
                         addr2 + "*" +
                         cityname + "*" +
                         statename + "*" +
                         zip + "*" +
                         PhoneNo1 + "*" +
                         PhoneNo2 + "*" +

                         isStudentPaidTaxable + "*" +
                         isStudentRedTaxable + "*" +
                         isStudentFreeTaxable + "*" +
                         isEmployeeTaxable + "*" +
                         isMealPlanTaxable + "*" +
                         isGuestTaxable + "*" +
                         isStudCashTaxable + "*" +
                         StartSchoolYear + "*" +
                         EndSchoolYear + "*" +

                         BankRoute + "*" +
                         BankAccount + "*" +
                         BankName + "*" +
                         BankAddr1 + "*" +
                         BankAddr2 + "*" +
                         BankCity + "*" +
                         dllBankState + "*" +
                         BankZip + "*" + 
                         dllAdmin + "*" + 
                         dllDirector
                         
            });

            $.ajax({
                type: "POST",
                url: "/Settings/updateDistrict",
                data: dataString,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.result == '-1') {
                        displaySuccessMessage("The district record has been updated successfully.");
                        RedirectToIndex('/district/');
                    }
                    else if (data.result == '1') {
                        displaySuccessMessage("The district record has been updated successfully.");
                        RedirectToIndex('/district/');
                    }
                    else if (data.result == '-999') {
                        $('#newdistrictName').text(distName);
                        $('#ConfirmModal').modal('show');
                    }
                    else if (data.result == '-1000') {
                        displayErrorMessage("This district name already exists.");
                    }
                    else {
                        displaySuccessMessage("The district record has been created successfully.");
                        //window.location.href = '/settings/Districts/' + data.result
                        RedirectToIndex('/district/');
                    }
                },
                error: function (request, status, error) {
                    displayErrorMessage("Error occurred during saving the data.");
                    return false;
                }
            });
        }
        else {
            errorbar.show();
        }
    }


}

function ValidateDistrictData() {
    debugger;
    var errorbar = $('.alert-danger');

    var distname = '#DistrictName';
    var zip = '#zip';
    var BankZip = '#BankZip';

    if (ValidateElement(distname, 1, "District name is required.") &&
        CheckStartEnddate() &&
        checkZipCode(zip) &&
        checkZipCode(BankZip) &&
        CheckBankRouteInput() &&
        CheckBankAccountInput()
        ) {
        errorbar.hide();
        return true;
    }
    else {
        errorbar.show();
        return false;
    }
}

function CheckStartEnddate() {
    //debugger;
    var sDatelement = $("#StartSchoolYear");
    var eDateelement = $("#EndSchoolYear");
    if (sDatelement.val() == '' || eDateelement.val() == '') return true;
    var startDate = new Date(sDatelement.val());
    var endDate = new Date(eDateelement.val());

    if (endDate < startDate) {
        var icon = $(sDatelement).parent('.input-icon').children('i');
        var icon2 = $(eDateelement).parent('.input-icon').children('i');

        icon.removeClass('fa-check').addClass("fa-warning");
        icon2.removeClass('fa-check').addClass("fa-warning");

        icon.attr("data-original-title", 'Start date should be less than end date.').tooltip({ 'container': 'body' });
        icon2.attr("data-original-title", 'End date should be greater than start date.').tooltip({ 'container': 'body' });

        sDatelement.closest('.form-group').removeClass('has-success').addClass('has-error');
        eDateelement.closest('.form-group').removeClass('has-success').addClass('has-error');

        displayErrorMessage("Start date should be less than end date.");
        return false
    }
    else {
        var icon = $(sDatelement).parent('.input-icon').children('i');
        var icon2 = $(eDateelement).parent('.input-icon').children('i');

        icon.removeClass("fa-warning").addClass("fa-check");
        icon2.removeClass("fa-warning").addClass("fa-check");

        sDatelement.closest('.form-group').removeClass('has-error').addClass('has-success');
        eDateelement.closest('.form-group').removeClass('has-error').addClass('has-success');

        return true;
    }
}


function CheckBankAccountInput() {
    //alert(element.id);
    debugger;
    var fieldValidationEx = /^\d+$/;
    var element = $("#BankAccount");


    if (element.val().length != 0 && fieldValidationEx.test(element.val()) == false) {
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass('fa-check').addClass("fa-warning");
        icon.attr("data-original-title", 'Invalid routing number format').tooltip({ 'container': 'body' });
        element.closest('.form-group').removeClass('has-success').addClass('has-error');
        return false;

    }
   
    else {
        if (element.val().length != 0) {
            var icon = $(element).parent('.input-icon').children('i');
            icon.removeClass("fa-warning").addClass("fa-check");
            element.closest('.form-group').removeClass('has-error').addClass('has-success');
        } else {
            var icon = $(element).parent('.input-icon').children('i');
            icon.removeClass("fa-warning");
            icon.removeClass("fa-check");
            element.closest('.form-group').removeClass('has-error');
            element.closest('.form-group').removeClass('has-success');

       

            return true;
        }

    }
    
    return true;
}


function CheckBankRouteInput() {
    //alert(element.id);
    debugger;
    var fieldValidationEx = /^\d+$/;
    var element = $("#BankRoute");
  
   
    if (element.val().length != 0 && fieldValidationEx.test(element.val()) == false){
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass('fa-check').addClass("fa-warning");
        icon.attr("data-original-title", 'Invalid routing number format').tooltip({ 'container': 'body' });
        element.closest('.form-group').removeClass('has-success').addClass('has-error');
        return false;

    }
    else if ((element.val().length != 0 && element.val().length < 9) || (element.val().length > 9 && element.val().length != 0)) {
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass('fa-check').addClass("fa-warning");
        icon.attr("data-original-title", 'Length of routing number should be equal to 9.').tooltip({ 'container': 'body' });
        element.closest('.form-group').removeClass('has-success').addClass('has-error');
        return false;
    }
    else {
        if (element.val().length != 0) {
            var icon = $(element).parent('.input-icon').children('i');
            icon.removeClass("fa-warning").addClass("fa-check");
            element.closest('.form-group').removeClass('has-error').addClass('has-success');
        } else {
            var icon = $(element).parent('.input-icon').children('i');
            icon.removeClass("fa-warning");
            icon.removeClass("fa-check");
            element.closest('.form-group').removeClass('has-error');
            element.closest('.form-group').removeClass('has-success');

            //var accountElemnet = $("#BankAccount");
            //var icon = $(accountElemnet).parent('.input-icon').children('i');
            //icon.removeClass("fa-warning");
            //icon.removeClass("fa-check");
            //accountElemnet.closest('.form-group').removeClass('has-error');
            //accountElemnet.closest('.form-group').removeClass('has-success');

            return true;
        }

    }
    //return CheckBankAccountNo();
    return true;
}

function ValidatePhone(elem, message) {
    var filter = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    var element = $(elem);

    if (element.val().length == 0) {
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass("fa-warning").removeAttr("data-original-title");
        element.closest('.form-group').removeClass('has-error');
        return true;
    }

    if (!filter.test(element.val())) {
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass('fa-check').addClass("fa-warning");
        icon.attr("data-original-title", message).tooltip({ 'container': 'body' });
        element.closest('.form-group').removeClass('has-success').addClass('has-error');
        return false;
    }
    else {
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass("fa-warning").addClass("fa-check").removeAttr("data-original-title");
        element.closest('.form-group').removeClass('has-error').addClass('has-success');
        return true;
    }
}

function ValidateElement(elem, minlen, message) {
    var element = $(elem);
    if (element.val().trim().length < minlen) {
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass('fa-check').addClass("fa-warning");
        icon.attr("data-original-title", message).tooltip({ 'container': 'body' });
        element.closest('.form-group').removeClass('has-success').addClass('has-error');

        return false;
    }
    else {
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass("fa-warning").addClass("fa-check");
        element.closest('.form-group').removeClass('has-error').addClass('has-success');
        return true;
    }
}

function checkZipCode(elem) {
    if ($(elem).val().length) {
        var regexpr = /(^\d{5}$)|(^\d{5}-\d{4}$)/;
        if (!regexpr.test($(elem).val())) {
            var icon = $(elem).parent('.input-icon').children('i');
            icon.removeClass('fa-check').addClass("fa-warning").css("color", "#b94a48");
            icon.attr("data-original-title", 'The zip code should only be either 5 or 9 digits long.').tooltip({ 'container': 'body' });
            $(elem).closest('.form-group').removeClass('has-success').addClass('has-error');
            return false;
        }
        else {
            var icon = $(elem).parent('.input-icon').children('i');
            icon.removeClass("fa-warning").addClass("fa-check").css("color", "#468847");;
            $(elem).closest('.form-group').removeClass('has-error').addClass('has-success');
            return true;
        }
    }
    else
        return true;
}

function CheckElement(elem, minlen, message) {
    var element = $(elem);
    //debugger;
    if (element.val().length < minlen && element.val().length != 0) {
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass('fa-check').addClass("fa-warning");
        icon.attr("data-original-title", message).tooltip({ 'container': 'body' });
        element.closest('.form-group').removeClass('has-success').addClass('has-error');

        return false;
    }
    else {
        if (element.val().length != 0) {
            var icon = $(element).parent('.input-icon').children('i');
            icon.removeClass("fa-warning").addClass("fa-check");
            element.closest('.form-group').removeClass('has-error').addClass('has-success');
        }
        return true;
    }
}
function CheckBankAccountNo() {
    //alert(element.id);
    var element = $("#BankRoute");
    var accountElemnet = $("#BankAccount");
    if (element.val().length == 9) {// then check validation
        if (accountElemnet.val().length >= 4) {
            var icon = $(accountElemnet).parent('.input-icon').children('i');
            icon.removeClass("fa-warning").addClass("fa-check");
            accountElemnet.closest('.form-group').removeClass('has-error').addClass('has-success');
            return true;
        }
        else {
            var icon = $(accountElemnet).parent('.input-icon').children('i');
            icon.removeClass('fa-check').addClass("fa-warning");
            icon.attr("data-original-title", 'Bank account no is not valid').tooltip({ 'container': 'body' });
            accountElemnet.closest('.form-group').removeClass('has-success').addClass('has-error');
            return false;
        }
    }
    else {
        return false;
    }
}
function updatePOSData() {
    //debugger;
    var ClientID = clientdelId;
    var POSID = posdelid;
    var School_Id = SchoolID;
    var Name = $("#posNamelbl").text();
    var EnableCCProcessing = $('#CreditCardStatus').is(':checked');
    var VeriFoneUserId = $("#VeriFoneUserId").val();
    var VeriFonePassword = $("#VeriFonePassword").val();



    var dataString = JSON.stringify({
        allData: ClientID + "*" +
                 POSID + "*" +
                 School_Id + "*" +
                 Name + "*" +
                 EnableCCProcessing + "*" +
                 VeriFoneUserId + "*" +
                 VeriFonePassword
    });

    $.ajax({
        type: "POST",
        url: "/Settings/updatePOS",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.result == '0') {
                displaySuccessMessage("The POS record has been updated successfully.");
                window.location.reload();
            }
            else {
                alert(data.result);
            }
        },
        error: function (request, status, error) {
            displayErrorMessage("Error occurred during saving the data.");
            return false;
        }
    });
}

/*
function displayErrorMessage(message) {
    toastr.error(message).options
                                = {
                                    "closeButton": true,
                                    "debug": false,
                                    "positionClass": "toast-top-center",
                                    "onclick": null,
                                    "showDuration": "1000",
                                    "hideDuration": "1000",
                                    "timeOut": "5000",
                                    "extendedTimeOut": "1000",
                                    "showEasing": "swing",
                                    "hideEasing": "linear",
                                    "showMethod": "fadeIn",
                                    "hideMethod": "fadeOut"
                                };
}

function displaySuccessMessage(message) {
    toastr.success(message).options
                   = {
                       "closeButton": true,
                       "debug": false,
                       "positionClass": "toast-top-center",
                       "onclick": null,
                       "showDuration": "1000",
                       "hideDuration": "1000",
                       "timeOut": "5000",
                       "extendedTimeOut": "1000",
                       "showEasing": "swing",
                       "hideEasing": "linear",
                       "showMethod": "fadeIn",
                       "hideMethod": "fadeOut"
                   };

}


function displayInfoMessage(message) {
    toastr.info(message).options
                   = {
                       "closeButton": true,
                       "debug": false,
                       "positionClass": "toast-top-center",
                       "onclick": null,
                       "showDuration": "40000",
                       "hideDuration": "40000",
                       "timeOut": "40000",
                       "extendedTimeOut": "40000",
                       "showEasing": "swing",
                       "hideEasing": "linear",
                       "showMethod": "fadeIn",
                       "hideMethod": "fadeOut",
                       "width": "600"
                   };

}*/

function DeleteSchool() {
    //debugger;
    var schooldataString = JSON.stringify({
        allData: clientdelId + "*" + schooldelid
    });
    $.ajax({
        type: "POST",
        url: "/Settings/DeleteSchool",
        data: schooldataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.result == '-1') {
                displaySuccessMessage("The school record has been deleted successfully.");
            }
            else {
                alert(data.result);
            }
        },
        error: function (request, status, error) {
            displayErrorMessage("Error occurred during deleting the school.");

            return false;
        }
    });
}

function DeletePOS() {
    //debugger;
    var posdataString = JSON.stringify({
        //allData: clientdelId + "*" + posdelid

    });
    $.ajax({
        type: "delete",
        url: "/Settings/DeletePOS",
        data: { "id": posdelid },
        dataType: "json",
        success: function (data) {
            if (data.IsError) {
                displayWarningMessage(data.Message);
            }
            else {
                displaySuccessMessage("The POS record has been deleted successfully.");
                window.location.reload(true);
            }
        },
        error: function (request, status, error) {
            displayErrorMessage("Error occurred during deleting the pos.");

            return false;
        }
    });


}
function DeleteSuccessfull() {
    alert('deone');
}

function enbdisbtns(btn) {
    //debugger;
    var jbtn = $(btn);
    if (btn.id.indexOf("enb") != -1) {
        jbtn.removeClass('disablebtn').addClass('enablebtn');
        var newbtnid = getOtherBtn(btn);
        var otherBtn = $(newbtnid);
        otherBtn.removeClass('enablebtn').addClass('disablebtn');
        //set hiddedn feild 
        //var hdf = newbtnid.replace("disBtn_", "");
        ccStatus = "true"; //$(hdf).val("true");
    }
    else {
        jbtn.removeClass('disablebtn').addClass('enablebtn');
        var newbtnid = getOtherBtn(btn);
        var otherBtn = $(newbtnid);
        otherBtn.removeClass('enablebtn').addClass('disablebtn');
        //set hiddedn feild 
        var hdfname = newbtnid.replace("enbBtn_", "");
        //var hdf = $(hdfname);
        //hdf.val("false");
        ccStatus = "false"; //

    }
}

function getOtherBtn(btn) {
    var id = btn.id;
    var newid = "";
    if (btn.id.indexOf("enb") != -1) {
        newid = id.replace("enb", "dis");
    }
    else if (btn.id.indexOf("dis") != -1) {
        newid = id.replace("dis", "enb");
    }
    return "#" + newid;
}
function RedirectToIndex(path) {
    window.location.href = path;
}