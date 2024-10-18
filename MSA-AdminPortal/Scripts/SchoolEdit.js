$(document).ready(function () {

    $('#taxList').select2();

    $('.datepicker').datepicker();
    $("#dllState").select2({
        placeholder: "Select State",
        allowClear: true
    });
    $("#dllDistrict").select2();
    $("#dllDirector").select2();
    $("#dllAdmin").select2();
    $("#dllBarCodeLength").select2();
    $("#dllBarCodeLength").select2();

    $('.make-switch').bootstrapSwitch();
    $('.make-switch').bootstrapSwitch();

    $('.pin-switch').bootstrapSwitch({
        onText: 'Enable',
        offText: 'Disable',
        labelWidth: 'auto'
    });




    $('#Phone1').inputmask("(999)999-9999");
    $('#Phone2').inputmask("(999)999-9999");

    $('#AlaCarteLimit').blur(function () {
        limit('AlaCarteLimit');
    });
    limit('AlaCarteLimit');

    $('#MealPlanLimit').blur(function () {
        limit('MealPlanLimit');
    });
    limit('MealPlanLimit');

    if (!document.getElementById("DoPinPreFix").checked) {
        $('#PinPreFix').slideUp();
    }

    if (document.getElementById("distStartDate").value == document.getElementById("schlStartDate").value &&
        document.getElementById("distEndDate").value == document.getElementById("schlEndDate").value) {
        document.getElementById("DisSchoolYear").checked = true;
        $('.dates').slideUp();
    }
    else {
        document.getElementById("NewSchoolYear").checked = true;
        $('.dates').slideDown();
    }
    //debugger;
    if (document.getElementById("distExecId").value == "True") {
        document.getElementById("distExec").checked = true;
        $('.exec').slideUp();
    }
    else {
        document.getElementById("newExec").checked = true;
        $('.exec').slideDown();
    }

    if (document.getElementById("schlId").value == '0') {
        distChange();
    }

    $('#SchoolID').blur(function () {
        ValidateElement(this, 1, "SchoolID is required.");
    });

    $('#SchoolName').blur(function () {
        ValidateElement(this, 1, "SchoolName is required.");
    });


    $('#Zip').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });

    $('#Zip').blur(function () {
        ValidateZipElement('#Zip', 5, "Zip code should be 5 or 9 digits long.");
    });

    //attach keypress to input
    $('.numericOnly').keydown(function (event) {
        // Allow special chars + arrows 
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9
            || event.keyCode == 27 || event.keyCode == 13
            || (event.keyCode == 65 && event.ctrlKey === true)
            || (event.keyCode >= 35 && event.keyCode <= 39)) {
            return;
        } else {
            // If it's not a number stop the keypress
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });

    $('.floatOnly').numeric();
    //attach keypress to input
    //$('.floatOnly').keypress(function (event) {

    //    if (event.keyCode == 45 || event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9
    //        || event.keyCode == 27 || event.keyCode == 13
    //        || (event.keyCode == 65 && event.ctrlKey === true)
    //        || (event.keyCode >= 35 && event.keyCode <= 39)) {
    //        return;
    //    }

    //    if (event.which < 45 || event.which > 59) {
    //        event.preventDefault();
    //    } // prevent if not number/dot

    //    if (event.which == 46
    //    && $(this).val().indexOf('.') != -1) {
    //        event.preventDefault();
    //    } // prevent if 
    //    //alert($(this).val().indexOf('-'));
    //    if (event.which == 45 && $(this).val().indexOf('-') != -1) {
    //        event.preventDefault();
    //    } // prevent if 

    //    if (event.which === 47 || event.which === 58) {
    //        event.preventDefault();
    //    }


    //});

    $('#StartSchoolYear')
    .datepicker()
    .on('changeDate', function (ev) {
        var endDate = $('#EndSchoolYear').val();
        if (endDate !== '') {
            if (ev.date.valueOf() > new Date(endDate).valueOf()) {
                displayInfoMessage('Start date must be before the end date.');
                $('#StartSchoolYear').val("")
            } else {
            }
            $('#StartSchoolYear').datepicker('hide');
        }
    });
    $('#EndSchoolYear')
        .datepicker()
        .on('changeDate', function (ev) {
            var startDate = $('#StartSchoolYear').val();
            if (startDate !== '') {
                if (ev.date.valueOf() < new Date(startDate).valueOf()) {
                    displayInfoMessage("End date must be after the start date.");
                    $('#EndSchoolYear').val("")
                } else {
                }
                $('#EndSchoolYear').datepicker('hide');
            }
        });

    togglePinSize();

});

var controller = '/School/';
var CreateAction = controller + 'Create';
var EditAction = controller + 'Edit';

$("#btnSave").click(function () {
    debugger;
    var id = document.getElementById("schlId").value;

    var url = '';
    var create = id == '0';
    if (create) {
        url = CreateAction;
    }
    else {
        url = EditAction;
    }

    var errorbar = $('.alert-danger');
    errorbar.hide();

    var clientId = document.getElementById("ClientId").value;
    var id = document.getElementById("schlId").value;
    var SchoolID = document.getElementById("SchoolID").value;
    var SchoolName = document.getElementById("SchoolName").value;
    var distid = $('#dllDistrict').val();
    var Addr1 = document.getElementById("Address1").value;
    var Addr2 = document.getElementById("Address2").value;
    var city = document.getElementById("City").value;
    var state = $('#dllState').val();
    var zip = document.getElementById("Zip").value;
    var phone1 = document.getElementById("Phone1").value;
    var phone2 = document.getElementById("Phone2").value;
    var mealPlanLimit = document.getElementById("MealPlanLimit").value;
    var alaCarteLimit = document.getElementById("AlaCarteLimit").value;
    var severeNeed = $('#isSevereNeed').is(':checked');
    var stripZero = $('#StripZeros').is(':checked');
    var photoLogging = $('#PhotoLogging').is(':checked');
    var useDistExec = $('#distExec').is(':checked');
    var doPinPreFix = $('#DoPinPreFix').is(':checked');
    var isPinEnable = $('#IsPinEnable').is(':checked');
    var PinLength = 0;
    var schoolDateVar = true;
    var taxList = $("#taxList").val();

    if (taxList == null) {
        taxList = '';
    }

    if (isPinEnable) {
        PinLength = $('#dllBarCodeLength').val();
    }
    var pinPreFix = document.getElementById("PinPreFix").value;

    if ($('#DisSchoolYear').is(':checked') == true) {
        var startDate = document.getElementById("distStartDate").value;
        var endDate = document.getElementById("distEndDate").value;
    }
    else {
        var startDate = document.getElementById("StartSchoolYear").value;
        var endDate = document.getElementById("EndSchoolYear").value;
        if (startDate === '' || endDate === '') {
           // displayWarningMessage("Both Start and End School date required.");
            // return false;
            schoolDateVar = false;
        }
    }
    if ($('#distExec').is(':checked') == true) {
        var DirectorId = document.getElementById("distDirectorId").value;
        var AdminId = document.getElementById("distAdminId").value;
    }
    else {
        var DirectorId = $('#dllDirector').val();
        var AdminId = $('#dllAdmin').val();
    }
    var Notes = document.getElementById("Comments").value;
    var schoolIdVar = ValidateElement('#SchoolID', 1, "SchoolID is required.");
    var schoolNameVar = ValidateElement('#SchoolName', 1, "SchoolName is required.");

    var phone1Ver = ValidatePhone('#Phone1', "Enter a valid phone number");
    var phone2Ver = ValidatePhone('#Phone2', "Enter a valid phone number");

    var zipVer = ValidateZipElement('#Zip', 5, "Zip code should be 5 or 9 digits.");

   

    if (phone1Ver && phone2Ver && zipVer && schoolDateVar && schoolIdVar && schoolNameVar) {
        errorbar.hide();

        $.ajax({
            type: "post",
            url: url,
            data: {
                "ClientID": clientId,
                "Id": id,
                "District_Id": distid,
                "Emp_Director_Id": DirectorId,
                "Emp_Administrator_Id": AdminId,
                "SchoolID": SchoolID,
                "SchoolName": SchoolName,
                "Address1": Addr1,
                "Address2": Addr2,
                "City": city,
                "State": state,
                "Zip": zip,
                "Phone1": phone1,
                "Phone2": phone2,
                "Comment": Notes,
                "isSevereNeed": severeNeed,
                "UseDistDirAdmin": useDistExec,
                "SchoolYearStartDate": startDate,
                "SchoolYearEndDate": endDate,
                "PhotoLogging": photoLogging,
                "StripZeros": stripZero,
                "PinPreFix": pinPreFix,
                "DoPinPreFix": doPinPreFix,
                "AlaCarteLimit": alaCarteLimit,
                "MealPlanLimit": mealPlanLimit,
                "BarCodeLength": PinLength,
                "TaxesList": taxList.toString()
            },
            dataType: "json",
            success: function (data) {

                var model = data;

                if (model.IsError) {
                    displayWarningMessage(model.ErrorMessage);
                }
                else {
                    if (create) {
                        displaySuccessMessage('The school record has been created successfully.');
                    }
                    else {
                        displaySuccessMessage('The school record has been updated successfully.');
                    }

                    window.location.replace("/School");
                }
            },
            error: function (data) {
                var model = data.SchoolModel;

                displayWarningMessage(model.ErrorMessage);
            }
        });
    }
    else {
        errorbar.show();
        // errorbar.focus();
        $('#SchoolID').focus();
    }
});

function togglePinPrefix() {
    if (document.getElementById("DoPinPreFix").checked) {
        $('#PinPreFix').slideDown();
        return false;
    }
    else {
        $('#PinPreFix').slideUp();
        return false;
    }
}

function togglePinSize() {
    if (document.getElementById("IsPinEnable").checked) {
        $('#ddlBarCodeDiv').slideDown();
        return false;
    }
    else {
        $('#ddlBarCodeDiv').slideUp();
        return false;
    }
}

function hideDates() {
    $('.dates').slideUp();
    return false;
}

function showDates() {
    $('.dates').slideDown();
    return false;
}

function hideExec() {
    $('.exec').slideUp();
    return false;
}

function showExec() {
    $('.exec').slideDown();
    return false;
}

function distChange() {
    //change the values for the district admin and director

    $.ajax({
        type: "get",
        url: "/School/GetDistrictData",
        data: { "Id": $('#dllDistrict').val() },
        dataType: "json",
        success: function (data) {

            var model = data;

            if (model.IsError) {
                displayWarningMessage(model.ErrorMessage);
            }
            else {
                var milliStart = model.StartDate.replace(/\/Date\((-?\d+)\)\//, '$1');
                var dStart = new Date(parseInt(milliStart));
                var stuff = new Date(dStart.getFullYear(), dStart.getMonth(), dStart.getDate());
                var mystuff = dStart.toLocaleDateString();
                $('#distStartDate').attr("value", dStart.toLocaleDateString());
                var milliEnd = model.EndDate.replace(/\/Date\((-?\d+)\)\//, '$1');
                var dEnd = new Date(parseInt(milliEnd));
                var newstuff = dEnd.toLocaleDateString();
                $('#distEndDate').attr("value", dEnd.toLocaleDateString());
                document.getElementById('schoolText').innerHTML = 'Use district school year (' + model.StartDateString + ' - ' + model.EndDateString + ')';
                if (model.AdminId) {
                    $('#dllAdmin').val(model.AdminId).attr("selected", "selected");
                } else {
                    $('#distAdminId').attr("value", '');
                }
                if (model.DirectorId) {
                    $('#dllDirector').val(model.DirectorId).attr("selected", "selected");
                } else {

                    $('#distDirectorId').attr("value", '');
                }

                $('#dllDirector').loadSelect(model.Directors);
                $('#dllAdmin').loadSelect(model.Admins);
                if (model.Directors) {
                    $('#dllDirector').select2("val", '');
                }
                if (model.Admins) {
                    $('#dllAdmin').select2("val", '');
                }
                if (model.DirectorName || model.AdminName) {
                    document.getElementById('DirAdminText').innerHTML = 'Use district director/admin (' + model.DirectorName + '/' + model.AdminName + ')';
                } else {
                    document.getElementById('DirAdminText').innerHTML = 'Use district director/admin';
                }



            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            
        }
    });
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
        icon.removeClass("fa-warning").addClass("fa-check").removeAttr("data-original-title");
        element.closest('.form-group').removeClass('has-error').addClass('has-success');
        return true;
    }
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

function ValidateZipElement(elem, minlen, message) {
    var element = $(elem);
    if (element.val().length != 5 && element.val().length != 9 && element.val().length != 0) {
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
        if (element.val() == "") {
            icon.removeClass("fa-check");
            element.closest('.form-group').removeClass('has-success');
        }
        return true;
    }
}

function validateSchoolDates() {
    if ($('#distExec').is(':checked') == true) {
        return true;
    }
    else {
        var DirectorId = $('#dllDirector').val();
        var AdminId = $('#dllAdmin').val();
    }
}

(function ($) {
    $(function () {
        $.fn.loadSelect = function (data) {
            return this.each(function () {
                this.options.length = 0;
                var select = this;
                $.each(data, function (index, itemData) {
                    var option = new Option(itemData.Text, itemData.Value);
                    $(select).append(option);
                });
            });
        };
    });
})(jQuery);

function limit(elemName) {

    //debugger;
    var oldVal = $('#' + elemName).val()
    if (oldVal != "") {
        var elemVal = parseFloat(oldVal.replace(/[^0-9-.]/g, '')).toFixed(2);
        $('#' + elemName).val(elemVal);
    }
    return;
}