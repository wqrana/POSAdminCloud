$(document).ready(function () {
    //getCutOffDate();
    //checkdaysValue();
    //getuseFiveDayWeekCutOff();
    //Allow users to enter numbers only
    $(".numericOnly").bind('keypress', function (e) {
        //debugger;
        // tab and shift
        if (e.keyCode == '9' || e.keyCode == '16') {
            return;
        }
        var code;
        if (e.keyCode) code = e.keyCode;
        else if (e.which) code = e.which;
        //delete , 
        if (e.which == 46)
            return false;
        //backspace 46
        if (code == 8 || code == 46 || code == 13)
            return true;
        //numbers 48 to 57
        if (code < 48 || code > 57)
            return false;
        if ((code > 47 || code < 58)) {

        }
    });

    //Disable paste
    $(".numericOnly").bind("paste", function (e) {
        e.preventDefault();
    });

    $(".numericOnly").bind('mouseenter', function (e) {
        var val = $(this).val();
        if (val != '0') {
            val = val.replace(/[^0-9]+/g, "")
            $(this).val(val);
        }
    });
});


function checkdaysValue() {
    debugger;
    var days = $('#txtcutoffday2').val();
    var retStr = "";
   // var retDate = new Date();
    var retDate = moment().utc();
    if (days == "" || days == "0") {
        $('#NewCutofflbl').text('');
    } else {
        //debugger;
       
        //var newDate = moment(new Date()).add(days, 'days'); //.format('MM/DD/YYYYY');
        var newDate = moment(moment().utc().add(days, 'days').format('MM/DD/YYYYY'));
        servDateStr = $('#lblSelectedDate').text();
        var servingdate = moment(servDateStr);
        if (newDate > servingdate) {
            days = servingdate.diff(moment(retDate.format('MM/DD/YYYYY')), 'days');
            
            retDate = servingdate.subtract(0, 'days'); //.format('MM/DD/YYYYY');
            $('#txtcutoffday2').val(days);
            

        } else {
            retDate = newDate; //.format('MM/DD/YYYYY');
        }
        //alert(retDate.day());
        var useFiveDaysWeek = $('#hduseFiveDayWeekCutOff').val();

        if (useFiveDaysWeek == "true") {
            if (retDate.day() == "0" || retDate.day() == "1") {
                if (retDate.day() == "0") {
                    retDate = retDate.subtract(1, 'days');
                }
                if (retDate.day() == "1") {
                    retDate = retDate.subtract(2, 'days');
                }
            }
        }
        retStr = retDate.format('MM/DD/YYYY');

        $('#NewCutofflbl').text(retStr);
    }
}


//setup before functions
var typingTimer;                //timer identifier
var doneTypingInterval = 1000;  //time in ms, 5 second for example

//on keyup, start the countdown
$('.numericOnly').keyup(function (e) {
   
    clearTimeout(typingTimer);
    typingTimer = setTimeout(doneTyping, doneTypingInterval);
    
});

//on keydown, clear the countdown 
$('.numericOnly').keydown(function () {
    clearTimeout(typingTimer);
});

//user is "finished typing," do something
function doneTyping() {
    checkdaysValue();
}

//function getCutOffDate() {
//    var calID = $("#hdCalID").val();

//    var jsoncalID = JSON.stringify({ allData: calID });
//    $.ajax({
//        type: "POST",
//        url: "PreorderOverrideCutOff.aspx/GetCutOffDate",
//        data: jsoncalID,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (data) {
//            if (data.d == '') {
//                //window.location = "default.aspx";
//                return false;
//            }
//            else {
//                $('#CurrentCutofflbl').text(data.d);

//            }

//        },
//        error: function (request, status, error) {
//            displayErrorMessage("Error occurred during get.");
//            return false;
//        }
//    });

//    return false;
//}


function getuseFiveDayWeekCutOff() {
    var calID = $("#calID").val();
    var jsoncalID = JSON.stringify({ allData: calID });
    $.ajax({
        type: "POST",
        url: "/PreorderCalController/GetWebLunchCutoffSettings",
        data: jsoncalID,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //debugger;
            if (data.d == '') {
                //window.location = "default.aspx";
                return false;
            }
            else {
                $('#hduseFiveDayWeekCutOff').val(data.fiveday);
                $('#hdcutOFFdate').val(data.cutOFFdate);
                $('#hdOverrideCutOffValue').val(data.OverrideCutOffValue);
                $('#hdCutoffvalue').val(data.Cutoffvalue);
                $('#hdhasCutOffvalue').val(data.hasCutOffvalue);
                $('#hdisOverriddentCutOff').val(data.isOverriddentCutOff);
                $('#hdAcceptOrderDate').val(data.AcceptOrderDate);
                
                checkdaysValue();

            }

        },
        error: function (request, status, error) {
            displayErrorMessage("Error occurred during get.");
            return false;
        }
    });

    return false;
}
