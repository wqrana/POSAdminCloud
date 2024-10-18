
var dataposted = false;
$(document).ready(function () {
    $('#LastName').focus();

    $('.datepicker').datepicker({
        autoClose: true,
        onSelect: function (dateText, inst) {
            validateDOB(this, "Date of birth is ahead of current date.");
        }
    });

    $('.datepicker').on('changeDate', function (ev) {
        $(this).datepicker('hide');
    });

    $('#UserID').keydown(function (e) {
        if (e.keyCode === 32) {
            return false;
        }
    });

    $('#PIN').keydown(function (e) {
        if (e.keyCode === 32) {
            return false;
        }
    });

    $('#genderList').select2();
    $('#stateList').select2();
    $('#languageList').select2();
    $('#gradeList').select2();
    $('#EthnicityList').select2();
    $('#districtList').select2();
    $('#schoolsList').select2();
    $('#homeroomList').select2();

    $("#SSN").inputmask("999-99-9999", { showMaskOnFocus: true });
    $("#Customer_Phone").inputmask("(999) 999-9999", { showMaskOnFocus: true });

    checkIsStudentOnLoad();
    $("#Student").click(function () {
        checkIsStudent();
    });

    $("#graduationDateSet").click(function () {
        // debugger;
        ShowHideGraduationTextBox();
    });

    ShowHideGraduationTextBox();

    var isStudentCheked = $('#Student').is(':checked');
    if (!isStudentCheked) {
        $("#Student_Worker").attr("disabled", true);

    } else {
        $("#Student_Worker").removeAttr("disabled");


    }



    $('#Customer_Zip').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });

    $('#Customer_Zip').blur(function () {
        ValidateZipElement(this, 5, "Zip code should be 5 or 9 digits.");
    });

    $('#Date_Of_Birth').blur(function () {
        validateDOB(this, "Date of birth is ahead of current date.");
    });

    $('#Date_Of_Birth').change(function () {
        validateDOB(this, "Date of birth is ahead of current date.");
    });

    $('#LastName').blur(function () {
        ValidateElement(this, 1, "Last Name is required.");
    });

    $('#FirstName').blur(function () {
        ValidateElement(this, 1, "First Name is required.");
    });

    $('#UserID').blur(function () {
        ValidateUserID(this, 1);

    });

    $('#PIN').blur(function () {
        //ValidateElement(this, 1, "PIN is required.");
        ValidatePINID(this, 1);
    });




    $('#Customer_Phone').blur(function () {
        //this.value = this.value.replace(/[^0-9\.]/g, '');
        ValidatePhone(this, "Phone is not valid.");
    });





    $('#AssignedSchoolsList').multiSelect();
    $("#enbBtnpos").removeClass('disablebtn').addClass('enablebtn');
    $("#disBtnpos").removeClass('enablebtn').addClass('disablebtn');
    var notesVal = $("#Customer_Notes").val();
    $("#Customer_Notes").replaceWith('<textarea id="Customer_Notes" name="Customer_Notes" class="form-control" maxlength="30"> ' + notesVal + '</textarea>');

    $("#districtList").change(function () {
        var districtID = this.value;
        CheckIfDistrictNotSelected();
        if (districtID == "") {
            $('#schoolsList').select2("enable", false);
            return;
        }
        getSchoolsList(districtID);

    });
    ////http://loudev.com/
    $("#schoolsList").change(function () {
        var schoolID = this.value;
        if (schoolID == "") {
            $('#districtName').text("");
            return;
        }

        //getDistrictInfo(schoolID);
        //alert(schoolID);

        var jsonSchoolID = JSON.stringify({ allData: schoolID });
        //debugger;
        $.ajax({
            type: "POST",
            url: "/Customer/getHomeRooms",
            data: jsonSchoolID,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.result != "-1") {
                    $('#homeroomList').get(0).options.length = 0;
                    $("#homeroomList").get(0).options[$("#homeroomList").get(0).options.length] = new Option("-- Select --", "-9999");
                    $("#homeroomList").select2("val", "-9999");
                    $.each(data.result, function (index, item) {
                        $("#homeroomList").get(0).options[$("#homeroomList").get(0).options.length] = new Option(item.Text, item.Value);
                    });

                    $("#AssignedSchoolsList").find("option").remove();

                    $.each(data.Schoollist, function (index, item) {

                        $('#AssignedSchoolsList').append($('<option>', { value: item.id }).text(item.name));

                    });
                    $('#AssignedSchoolsList').multiSelect('refresh');
                    //debugger;
                    var selecteddistrictID = $("#districtList").val();
                    if (selecteddistrictID == "-1" || selecteddistrictID == "") {
                        $("#districtList").val(data.DistrictID);
                        var id = $('#districtList').val();
                        var idstr = $('#districtList option:selected').text();
                        var distSpan = $("#s2id_districtList").find("span").first();
                        distSpan.text(idstr);
                        CheckIfDistrictNotSelected();
                    }

                } else {
                    $('#homeroomList').get(0).options.length = 0;
                    $("#homeroomList").get(0).options[$("#homeroomList").get(0).options.length] = new Option("-- Select --", "-9999");
                    $("#homeroomList").select2("val", "-9999");
                }

            },
            error: function (request, status, error) {
                //displayErrorMessage("Error occurred during saving the data.");
                //return false;
            }
        });

        removeSchoolFromEating(schoolID, true);
    });


    ///


    $('#fileupload').fileupload({
        dataType: 'json',
        url: '/Customer/updateCustomer',
        add: function (e, data) {
            acceptFileTypes: /(\.|\/)(jpe?g|png)$/i,
            $('#target').attr('src', URL.createObjectURL(data.files[0]));
            dataposted = true;
            $('.submitClass').click(function () {
                if (ValidateCustomerData()) {
                    data.formData = { customerData: CreatstringifyData() };
                    data.submit();
                }
            });
        },

        done: function (e, data) {
            AfterSaveCustomer(data.jqXHR.responseJSON.status, data.jqXHR.responseJSON.CustomerID);
        }
    });
    ////

    $("#AssignedSchoolsList").change(function () {
        checkPrimarySchool();
    });
    //CheckIfDistrictEmpty();
    CheckIfDistrictNotSelected();
    showHideFreeReducedDiv();
}); //document ready function ends


function checkIsStudentOnLoad() {

    var isStudentCheked = $('#Student').is(':checked');
    if (!isStudentCheked) {
        $("#ReducedAndFree").slideUp();
        $("#Student_Worker").attr("disabled", true);
        $("#uniform-Student_Worker").addClass("disabled");
        $("#EmployeeAdultParent").show();
        $("#EmployeeAdult").attr("disabled", false);
        $("#uniform-EmployeeAdult").removeClass("disabled");
    } else {
        //debugger;
        $("#ReducedAndFree").slideDown();
        $("#Student_Worker").removeAttr("disabled");
        $("#uniform-Student_Worker").removeClass("disabled");
        $("#EmployeeAdultParent").hide();
        $("#EmployeeAdult").attr("disabled", true);
        $("#uniform-EmployeeAdult").addClass("disabled");
    }
}



function checkIsStudent() {

    var isStudentCheked = $('#Student').is(':checked');
    var reducedLunch = $("#ReducedLunch").attr("checked", false);
    var FreeLunch = $("#FreeLunch").attr("checked", false);
    var MealPlanLunch = $("#MealPlanLunch").attr("checked", false);
    // Change by farrukh m (allshore) on 05/05/16 to fix PA-505
    var EmployeeAdult = $("#PiadLunch").attr("checked", false);
    var PiadLunch = $("#EmployeeAdult").prop("checked", true);
    //------ end ------------
    $.uniform.update(reducedLunch);
    $.uniform.update(FreeLunch);
    $.uniform.update(MealPlanLunch);
    $.uniform.update(EmployeeAdult);
    $.uniform.update(PiadLunch);

    if (!isStudentCheked) {
        //zubair M 

        $("#ReducedAndFree").slideUp();
        $("#Student_Worker").attr("disabled", true);
        $("#uniform-Student_Worker").addClass("disabled");
        $("#EmployeeAdultParent").show();
        $("#EmployeeAdult").attr("disabled", false);
        $("#uniform-EmployeeAdult").removeClass("disabled");
    } else {
        //debugger;
        $("#ReducedAndFree").slideDown();
        $("#Student_Worker").removeAttr("disabled");
        $("#uniform-Student_Worker").removeClass("disabled");
        $("#EmployeeAdultParent").hide();
        $("#EmployeeAdult").attr("disabled", true);
        $("#uniform-EmployeeAdult").addClass("disabled");
    }
}




function CheckIfDistrictEmpty() {
    //debugger;
    var txt = $('#districtName').text();
    if (txt == "") {
        var schoolID = $("#schoolsList").val();
        getDistrictInfo(schoolID)
    }
}


function CheckIfDistrictNotSelected() {
    var districtID = $("#districtList").val();
    if (districtID == "-1" || districtID == "") {
        DisableAccountInfo();
    } else {
        EnableAccountInfo();
    }

}



function getDistrictInfo(schoolID) {
    var jsonSchoolID = JSON.stringify({ allData: schoolID });
    //debugger;
    $.ajax({
        type: "POST",
        url: "/Customer/getDistrict",
        data: jsonSchoolID,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //debugger;
            if (data.result != "-1") {
                //$('#districtList').val(data.result);
                //var id = $('#districtList').val();
                //var idstr = $('#districtList option:selected').text();
                //var distSpan = $("#s2id_districtList").find("span").first();
                //distSpan.text(idstr);
                $('#districtName').text(data.districtName);
                $('#HDdistrictID').val(data.result);

            }

        },
        error: function (request, status, error) {
            //displayErrorMessage("Error occurred during saving the data.");
            //return false;
        }
    });
}

function getSchoolsList(districtID) {
    var jsondistrictID = JSON.stringify({ allData: districtID });

    $("#homeroomList").get(0).options[$("#homeroomList").get(0).options.length] = new Option("-- Select --", "-9999");
    $("#homeroomList").select2("val", "-9999");

    //debugger;
    $.ajax({
        type: "POST",
        url: "/Customer/getSchools",
        data: jsondistrictID,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //debugger;
            if (data.result != "-1") {
                if (data.count == "0") {
                    var schoolSpan = $("#s2id_schoolsList").find("span").first();
                    schoolSpan.text('');
                }

                $('#schoolsList').get(0).options.length = 0;
                $("#schoolsList").get(0).options[0] = new Option("Select school", "-1");
                $.each(data.result, function (index, item) {
                    $("#schoolsList").get(0).options[$("#schoolsList").get(0).options.length] = new Option(item.data, item.value);
                });
                $('#schoolsList').select2();
                $('#schoolsList').select2("enable", true)

                /////////fill eatings schools too
                $("#AssignedSchoolsList").find("option").remove();

                $.each(data.result, function (index, item) {

                    $('#AssignedSchoolsList').append($('<option>', { value: item.value }).text(item.data));

                });
                $('#AssignedSchoolsList').multiSelect('refresh');
                //////////////////

                //var id = $('#districtList').val();
                //var idstr = $('#districtList option:selected').text();
                //var distSpan = $("#s2id_districtList").find("span").first();
                //distSpan.text(idstr);
            }
        },
        error: function (request, status, error) {
            //displayErrorMessage("Error occurred during saving the data.");
            //return false;
        }
    });
}



function checkPrimarySchool() {
    var SchooID = $("#schoolsList").val();
    removeSchoolFromEating(SchooID, false);
}

function removeSchoolFromEating(schoolID, bool) {

    //debugger;
    var selectedOpts = $('select#AssignedSchoolsList :selected');
    var sel = $("select#AssignedSchoolsList option[value='" + schoolID + "']").prop("selected");
    if (sel) {
        $("select#AssignedSchoolsList option[value='" + schoolID + "']").prop("selected", false);

        $("#" + schoolID + "-selection").removeClass('ms-selected');
        $("#" + schoolID + "-selection").css({ 'display': 'none' });
        $("#" + schoolID + "-selectable").css({ 'display': 'block' });
        if (bool) {
            displayWarningMessage('You have chosen this school as primary, so unselected from eating schools.');
        }
        else {
            displayWarningMessage('School selected as a primary can’t be included in eating schools.');
        }
    }



}
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#target').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}



function CreatstringifyData() {
    var clientId = document.getElementById("ClientId").value;
    var custId = document.getElementById("CustomerId").value;

    var Active = $('#CustomerActive').is(':checked');
    var lastname = document.getElementById("LastName").value;
    var firstname = document.getElementById("FirstName").value;
    var middle = document.getElementById("Middle").value;
    var dob = document.getElementById("Date_Of_Birth").value;
    var gender = $('#genderList').val();
    var ssn = document.getElementById("SSN").value;
    var Customer_Notes = document.getElementById("Customer_Notes").value;
    var Addr1 = document.getElementById("Customer_Addr1").value;
    var Addr2 = document.getElementById("Customer_Addr2").value;
    var city = document.getElementById("Customer_City").value;
    var state = $('#stateList').val();
    var zip = document.getElementById("Customer_Zip").value;
    var phone = document.getElementById("Customer_Phone").value;
    var email = document.getElementById("Email").value;
    var lang = $('#languageList').val();
    var ethnicity = $('#EthnicityList').val();
    var userid = document.getElementById("UserID").value;
    var pin = document.getElementById("PIN").value;
    var schoolid = $('#schoolsList').val();
    var distid = $('#districtList').val(); //$('#HDdistrictID').val(); //$('#districtList').val();
    var notinDist = $('#NotInDistrict').is(':checked');
    var grad = $('#gradeList').val();
    var homeroom = ($('#homeroomList').val().trim() == '-9999' || $('#homeroomList').val().trim() == '') ? '' : $('#homeroomList').val(); $('#homeroomList').val();
    var graduationDateSet = $('#graduationDateSet').is(':checked');
    var GraduationDate = $('#GraduationDate').val();
    var paidlunch = $('#PiadLunch').is(':checked');
    var ReducedLunch = $('#ReducedLunch').is(':checked');
    var FreeLunch = $('#FreeLunch').is(':checked');
    var MealPlanLunch = $('#MealPlanLunch').is(':checked');
    var EmployeeAdult = $('#EmployeeAdult').is(':checked');

    var Student = $('#Student').is(':checked');
    var snackP = $('#Snack_Participant').is(':checked');
    var stuWork = $('#Student_Worker').is(':checked');
    var alacarte = $('#AllowAlaCarte').is(':checked');
    var nocredit = $('#No_Credit_On_Account').is(':checked');

    var multipleValues = $("#AssignedSchoolsList").val() || [];
    var schoolsList = multipleValues.join(", ");

    var PictureExtension = document.getElementById("PictureExtension").value;
    var StorageAccountName = document.getElementById("StorageAccountName").value;
    var ContainerName = document.getElementById("ContainerName").value;
    var PictureFileName = document.getElementById("PictureFileName").value;




    //Creating a object to serialize from javascript 
    var customerData = new Object();

    // Assigning values to the properties
    customerData.clientId = clientId;
    customerData.custId = custId;
    customerData.Active = Active;
    customerData.lastname = lastname;
    customerData.firstname = firstname;
    customerData.middle = middle;
    customerData.dob = dob;
    customerData.gender = gender;
    customerData.ssn = ssn;
    customerData.Customer_Notes = Customer_Notes;
    customerData.Addr1 = Addr1;
    customerData.Addr2 = Addr2;
    customerData.city = city;
    customerData.state = state;
    customerData.zip = zip;
    customerData.phone = phone;
    customerData.email = email;
    customerData.lang = lang;
    customerData.ethnicity = ethnicity;
    customerData.userid = userid;
    customerData.pin = pin;
    customerData.schoolid = schoolid;
    customerData.distid = distid;
    customerData.notinDist = notinDist;
    customerData.grad = grad;
    customerData.homeroom = homeroom;
    customerData.graduationDateSet = graduationDateSet;
    customerData.GraduationDate = GraduationDate;


    customerData.paidlunch = paidlunch;
    customerData.ReducedLunch = ReducedLunch;
    customerData.FreeLunch = FreeLunch;
    customerData.MealPlanLunch = MealPlanLunch;
    customerData.EmployeeAdult = EmployeeAdult;
    customerData.Student = Student;
    customerData.snackP = snackP;
    customerData.stuWork = stuWork;
    customerData.alacarte = alacarte;
    customerData.nocredit = nocredit;

    customerData.PictureExtension = PictureExtension;
    customerData.StorageAccountName = StorageAccountName;
    customerData.ContainerName = ContainerName;
    customerData.PictureFileName = PictureFileName;

    customerData.schoolsList = schoolsList;

    var data = JSON.stringify(customerData);
    return data;

}

function SaveCustomerData() {
    //debugger;
    var CustomerId = 0;
    if (!dataposted) {
        if (ValidateCustomerData()) {
            //debugger;
            dataString = CreatstringifyData();
            $.ajax({
                type: "POST",
                url: "/Customer/updateCustomer",
                data: dataString,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    AfterSaveCustomer(data.status, data.CustomerID);

                },
                error: function (request, status, error) {
                    displayErrorMessage("Error occurred during saving the data.");
                    return false;
                }
            });
        }
    }
    return CustomerId;
}

function AfterSaveCustomer(status, cID) {
    //debugger;
    if (status == 'new') {
        displaySuccessMessage("The customer record has been created successfully.");
        //window.location.href = '/Customer/Edit/' + cID
        window.location.href = '/Customer/';
    }
    else if (status == 'old') {
        displaySuccessMessage("The customer record has been updated successfully."); //alert(data.result);
        window.location.href = '/Customer/';
        //window.history.back();
    }
    else if (status == 'error') {
        displayErrorMessage("Error occurred during saving the data.");
    } else if (status == 'duplicateUserid') {
        var UserIDVer = ValidateElement('#UserID', 1000, "User ID already exists.");
        displayErrorMessage("User ID already exists , please chose other one.");
    } else if (status == 'duplicatePIN') {
        var PINVer = ValidateElement('#PIN', 1000, "PIN already exists.");
        displayErrorMessage("PIN exists , please chose other one.");
    }
}

//function ValidateCustomerData() {
//    return true;
//    var errorbar = $('.alert-danger');

//    var LName = '#LastName';

//    var CustomerZip = '#Customer_Zip';
//    var CustomerPhone = '#Customer_Phone';

//    if (ValidateElement(LName, 6, "Length of last name sholud not be less than 6.")
//        && ValidateElement(CustomerZip, 5, "Zip code sholud not be less than 5 digits.")
//        && ValidateElement(CustomerPhone, 9, "Phone number sholud not be less than 9 digits.")
//        ) {
//        errorbar.hide();
//        return true;
//    }
//    else {
//        errorbar.show();
//        return false;
//    }
//}


function ValidateCustomerData() {
    debugger;
    var errorbar = $('.alert-danger');
    errorbar.hide();

    var lastname = '#LastName';
    var FirstName = '#FirstName';
    var UserID = '#UserID';
    var PIN = '#PIN';

    //optional data verification
    var Email = '#Email';
    var ssn = '#SSN';

    var phone = '#Customer_Phone';

    var CustomerZip = '#Customer_Zip';
    var DOB = '#Date_Of_Birth';

    var lNameVer = ValidateElement(lastname, 1, "Last Name is a required field.");
    var fNameVer = ValidateElement(FirstName, 1, "First Name is a required field.");
    var UserIDVer = ValidateElement(UserID, 1, "User ID is required.");
    var PinVer = ValidateElement(PIN, 1, "PIN is a required field.");
    var EmailVer = ValidateEmail(Email, "Email is not valid.");
    var ssnVer = ValidateSSN(ssn, "SSN is not valid.");
    var customerZip = ValidateZipElement(CustomerZip, 5, "Zip code sholud not be less than 5 digits.");
    var CustomerPhone = ValidatePhone(phone, "Phone is not valid.");
    var mealStat = validateMealStatus();
    var schoolSelected = validateSchoolId();
    var graduationBool = validateGraduationDate();

    // var phoneVer = ValidatePhone(phone, "Phone is not valid.");
    //var DateOB = validateDOB(DOB, "Date of birth is ahead of current date");

    //var Zipcode = ValidateElement(CustomerZip, 5, "Zip code sholud not be less than 5 digits.");

    //if ((ValidateElement(lastname, 1, "Last Name is a required field.")) &&
    //    (ValidateElement(FirstName, 1, "First Name is a required field.")) &&
    //(ValidateElement(UserID, 1, "User ID is a required field.")) &&
    // (ValidateElement(PIN, 1, "PIN is a required field.")) &&
    //    (ValidateEmail(Email, "Email is not valid."))) {
    if (lNameVer && fNameVer && UserIDVer && PinVer && EmailVer && ssnVer && mealStat && schoolSelected && customerZip && CustomerPhone && graduationBool) {
        errorbar.hide();
        return true;
    }
    else {
        errorbar.show();
        $(lastname).focus();
        return false;
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
        icon.removeClass("fa-warning").addClass("fa-check").removeAttr("data-original-title");
        element.closest('.form-group').removeClass('has-error').addClass('has-success');
        return true;
    }
}

function ValidateElementEmpty(elem, minlen, message) {
    var element = $(elem);
    if (element.val().length < minlen && element.val().length != 0) {
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

function ValidateEmail(elem, message) {
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    var element = $(elem);

    if (element.val().length == 0) {
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass("fa-warning");
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
        icon.removeClass("fa-warning").addClass("fa-check");
        element.closest('.form-group').removeClass('has-error').addClass('has-success');
        return true;
    }
}

function RedirectToIndex(path) {
    window.location.href = path;
}

function ValidateSSN(elem, message) {
    var filter = /^\d{3}-\d{2}-\d{4}$/;
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

function validateMealStatus() {
    //debugger;
    var paidlunch = $('#PiadLunch').is(':checked');
    var ReducedLunch = $('#ReducedLunch').is(':checked');
    var FreeLunch = $('#FreeLunch').is(':checked');
    var MealPlanLunch = $('#MealPlanLunch').is(':checked');
    var EmployeeAdult = $('#EmployeeAdult').is(':checked');

    var isStudentCheked = $('#Student').is(':checked');
    if (!isStudentCheked) {


        if (!paidlunch && !MealPlanLunch && !EmployeeAdult) {
            displayErrorMessage("Meal Status is required.");
            return false;
        } else {
            return true;
        }
    } else {
        if (!paidlunch && !ReducedLunch && !FreeLunch && !MealPlanLunch) {
            displayErrorMessage("Meal Status is required.");
            return false;
        } else {
            return true;
        }
    }

}

function ValidatePhone(elem, message) {
    //debugger;
    var filter = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    var element = $(elem);
    //if (element.val() == "") return;
    //if (element.val().length == 0) {
    //    var icon = $(element).parent('.input-icon').children('i');
    //    icon.removeClass("fa-warning").removeAttr("data-original-title");
    //    element.closest('.form-group').removeClass('has-error');
    //    return true;
    //}

    if (!filter.test(element.val()) && element.val() != "") {

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

function validateDOB(elem, message) {
    //debugger;
    var element = $(elem);
    //if (element.val() == "") return;

    if (new Date(element.val()) > new Date() && element.val != "") {
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

function validateSchoolId() {
    //debugger;
    var schoolID = $("#schoolsList").val();
    if (schoolID == "-1" || schoolID == "") {
        //displayErrorMessage("Please select school.");
        return false;
    } else
        return true;

}
function ValidateUserID(elem, minlen) {
    // debugger;
    var message = "User ID is required.";

    var element = $(elem);
    if (element.val().trim().length < minlen) {
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass('fa-check').addClass("fa-warning");
        icon.attr("data-original-title", message).tooltip({ 'container': 'body' });
        element.closest('.form-group').removeClass('has-success').addClass('has-error');

        return false;
    }
    else {
        message = "Users ID already exists, please  chose other one.";
        var districtID = $('#districtList').val();
        var customerID = $('#CustomerId').val();

        var jsonUserID = JSON.stringify({ allData: element.val() + '*' + customerID + '*' + districtID });
        //debugger;
        $.ajax({
            type: "POST",
            url: "/Customer/CheckUserID",
            async: false,
            data: jsonUserID,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.result != "-1") {
                    if (data.result == "yes") {
                        var icon = $(element).parent('.input-icon').children('i');
                        icon.removeClass('fa-check').addClass("fa-warning");
                        icon.attr("data-original-title", message).tooltip({ 'container': 'body' });
                        element.closest('.form-group').removeClass('has-success').addClass('has-error');
                        return false;
                    } else {
                        var icon = $(element).parent('.input-icon').children('i');
                        icon.removeClass("fa-warning").addClass("fa-check").removeAttr("data-original-title");
                        element.closest('.form-group').removeClass('has-error').addClass('has-success');
                        return true;
                    }
                }
            },
            error: function (request, status, error) {
                return true;
            }
        });
    }
}


function ValidatePINID(elem, minlen) {
    // debugger;
    var message = "PIN ID is required.";

    var element = $(elem);
    if (element.val().trim().length < minlen) {
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass('fa-check').addClass("fa-warning");
        icon.attr("data-original-title", message).tooltip({ 'container': 'body' });
        element.closest('.form-group').removeClass('has-success').addClass('has-error');

        return false;
    }
    else {
        message = "PIN ID already exists, please  chose other one.";
        var districtID = $('#districtList').val();
        var customerID = $('#CustomerId').val();
        var jsonUserID = JSON.stringify({ allData: element.val() + '*' + customerID + '*' + districtID });
        //debugger;
        $.ajax({
            type: "POST",
            url: "/Customer/CheckPINID",
            async: false,
            data: jsonUserID,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.result != "-1") {
                    if (data.result == "yes") {
                        var icon = $(element).parent('.input-icon').children('i');
                        icon.removeClass('fa-check').addClass("fa-warning");
                        icon.attr("data-original-title", message).tooltip({ 'container': 'body' });
                        element.closest('.form-group').removeClass('has-success').addClass('has-error');
                        return false;
                    } else {
                        var icon = $(element).parent('.input-icon').children('i');
                        icon.removeClass("fa-warning").addClass("fa-check").removeAttr("data-original-title");
                        element.closest('.form-group').removeClass('has-error').addClass('has-success');
                        return true;
                    }
                }
            },
            error: function (request, status, error) {
                return true;
            }
        });
    }
}

function DisableAccountInfo() {


    $("#NotInDistrict").attr("disabled", true);
    $("#uniform-NotInDistrict").addClass("disabled");


    $("#UserID").attr("disabled", true);

    $("#PIN").attr("disabled", true);
    $("#gradeList").select2("enable", false);
    $("#graduationDateSet").attr("disabled", true);
    $("#homeroomList").select2("enable", false);


    $("#PiadLunch").attr("disabled", true);
    $("#uniform-PiadLunch").addClass("disabled");

    $("#ReducedLunch").attr("disabled", true);
    $("#uniform-ReducedLunch").addClass("disabled");

    $("#FreeLunch").attr("disabled", true);
    $("#uniform-FreeLunch").addClass("disabled");

    $("#MealPlanLunch").attr("disabled", true);
    $("#uniform-MealPlanLunch").addClass("disabled");

    $("#EmployeeAdult").attr("disabled", true);
    $("#uniform-EmployeeAdult").addClass("disabled");

    $("#Student").attr("disabled", true);
    $("#uniform-Student").addClass("disabled");

    $("#Snack_Participant").attr("disabled", true);
    $("#uniform-Snack_Participant").addClass("disabled");

    //$("#Student_Worker").attr("disabled", true);
    //$("#uniform-Student_Worker").addClass("disabled");

    $("#AllowAlaCarte").attr("disabled", true);
    $("#uniform-AllowAlaCarte").addClass("disabled");

    $("#No_Credit_On_Account").attr("disabled", true);
    $("#uniform-No_Credit_On_Account").addClass("disabled");

    //$('#AssignedSchoolsList').multiSelect("disable");
    $("#AssignedSchoolsList").find("option").remove();
    $("#AssignedSchoolsList").attr("disabled", "disabled");
    $("#AssignedSchoolsList").multiSelect('refresh');

}

function EnableAccountInfo() {


    $("#NotInDistrict").removeAttr("disabled");
    $("#uniform-NotInDistrict").removeClass("disabled");

    $("#UserID").attr("disabled", false);

    $("#PIN").attr("disabled", false);
    $("#gradeList").select2("enable", true);

    $("#graduationDateSet").removeAttr("disabled");
    $("#uniform-graduationDateSet").removeClass("disabled");

    $("#homeroomList").select2("enable", true);


    $("#PiadLunch").removeAttr("disabled");
    $("#uniform-PiadLunch").removeClass("disabled");

    $("#ReducedLunch").removeAttr("disabled");
    $("#uniform-ReducedLunch").removeClass("disabled");

    $("#FreeLunch").removeAttr("disabled");
    $("#uniform-FreeLunch").removeClass("disabled");

    $("#MealPlanLunch").removeAttr("disabled");
    $("#uniform-MealPlanLunch").removeClass("disabled");

    $("#EmployeeAdult").removeAttr("disabled");
    $("#uniform-EmployeeAdult").removeClass("disabled");

    $("#Student").removeAttr("disabled");
    $("#uniform-Student").removeClass("disabled");

    $("#Snack_Participant").removeAttr("disabled");
    $("#uniform-Snack_Participant").removeClass("disabled");

    //$("#Student_Worker").attr("disabled", true);
    //$("#uniform-Student_Worker").addClass("disabled");

    $("#AllowAlaCarte").removeAttr("disabled");
    $("#uniform-AllowAlaCarte").removeClass("disabled");

    $("#No_Credit_On_Account").removeAttr("disabled");
    $("#uniform-No_Credit_On_Account").removeClass("disabled");

    //$('#AssignedSchoolsList').multiSelect("disable");

    $("#AssignedSchoolsList").removeAttr("disabled");
    $("#AssignedSchoolsList").multiSelect('refresh');

}

function ShowHideGraduationTextBox() {
    var graduationDateSet = $('#graduationDateSet').is(':checked');
    if (graduationDateSet) {
        $("#GraduationDateDiv").slideDown();
    } else {
        $("#GraduationDateDiv").slideUp();
    }
}


function removePhoto() {
    var id = $("#CustomerId").val();

    if (id != '') {
        $.ajax({
            type: "get",
            url: "/Customer/RemovePhoto",
            data: { "CustomerID": id },
            dataType: "json",
            success: function (data) {
                if (data) {
                    displaySuccessMessage('Photo removed successfully.');
                    $('#target').attr('src', '../../images/defaultpic.jpg');
                    $("#removePhotoContainer").hide();
                    $('#removeImageModal').modal('toggle');

                }
                else {
                    displayErrorMessage('Error in remove photo. Please try again');
                    $('#removeImageModal').modal('toggle');
                }
            },
            error: function (data) {
                displayErrorMessage('Error in remove photo. Please try again');
                $('#removeImageModal').modal('toggle');
            }
        });
    }
}

function validateGraduationDate() {
    var graduationDateSet = $('#graduationDateSet').is(':checked');
    if (graduationDateSet) {
        var GraduationDateValue = $('#GraduationDate').val();
        if (GraduationDateValue != "") {
            return true;
        } else {
            displayErrorMessage("Please enter graduation date.");
            return false;
        }
    } else {
        return true;
    }

}

function showHideFreeReducedDiv() {
    var Count = $('#FreeReducedAppCount').val();
    var countINT = parseInt(Count);
    if (countINT > 0) {
        $('#FreeReducedDIV').removeClass('rowFreeRedCSS');
    } else {
        if (!$('#FreeReducedDIV').hasClass('rowFreeRedCSS'))
            $('#FreeReducedDIV').addClass('rowFreeRedCSS');
    }

}

