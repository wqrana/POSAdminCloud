jQuery(document).ready(function () {

    $('#calendar').fullCalendar({
        //defaultDate: '2017-03-13',
       fixedWeekCount: false,
        editable: false,
        eventLimit: true, // allow "more" link when too many events
        dayRender: function( date, cell ) {
            // It's an example, do your own test here
            if(cell.hasClass("fc-other-month")) {
                cell.addClass('disabled');
                if (cell.hasClass('fc-today')) {
                    cell.removeClass('fc-today');
                }
            } 

        },
      
        eventRender: function (event, element) {
            //element.find('span.fc-title').html(element.find('span.fc-title').text());

            //element.attr('title', event.tooltip);
            var tooltip = event.tooltip;
            if (tooltip) {
                if (tooltip.length > 1) {
                    $(element).attr("data-original-title", tooltip)
                    $(element).tooltip({ container: "body" })
                }
            }

        },

        eventOrder: 'userOrder',

        loading: function (isLoading, view) {
            if (isLoading) {// isLoading gives boolean value
                $('#loadingDiv').show();

            }
        },
        eventAfterAllRender: function (view) {
            $('#loadingDiv').hide();

        },
        dayClick: function (date, jsEvent, view) {
            
            //debugger;
            var ClickedMonth = moment(date).format("MM");
            var CurrentDate = $("#calendar").fullCalendar('getDate');
            var renderedMonth = moment(CurrentDate).format("MM");

           
            if ($('#UpdateCalendars').val() == "True") {
                if (ClickedMonth == renderedMonth) {
                    GetCutOffSettings(true, date);
                } else {

                    return false;
                  //  displayWarningMessage('Please open dates from the current month only.');


                }
            }
            else {
                displayWarningMessage("You don't have rights to edit calendar");
            }

            //var ClickedMonth = moment(date).format("MM");
            //var CurrentDate = $("#calendar").fullCalendar('getDate');
            //var renderedMonth = moment(CurrentDate).format("MM");

            //if (ClickedMonth == renderedMonth) {


            //    $('#PreorderDayModal').modal({ backdrop: 'static', keyboard: false });
            //    $('#PreorderDayModal').on('shown.bs.modal', function () {
            //        $('#PreorderDayModal').off('shown.bs.modal');
            //        $('#calendar2').fullCalendar('gotoDate', date);

            //        //var event = { id: 1, title: 'New event', start: new Date() };

            //        var startDateNew = moment(date).format("MM/DD/YYYY");
            //        var endDateNew = moment(date).format("MM/DD/YYYY");
            //        var cache = new Date().getTime();

            //        $('#calendar2').fullCalendar('changeView', 'basicDay');//agendaDay
            //        getEventsFromParent();
            //        //debugger;
            //        //$('#calendar2').fullCalendar('refetchEvents');
            //        //var URL = "/PreorderCal/AjaxHandler?CalId=" + getUrlVars()['id'] + "&start=" + startDateNew + "&end=" + endDateNew + "";
            //        makeEventUnSelected();

            //    })

            //} else {
            //    displayWarningMessage('Please open dates from the current month only.');


            //}

        },
        //eventClick: function (event) {
        //    $('#calendar').fullCalendar('removeEvents', event._id);
        //},
        eventMouseover: function (event, domEvent) {
            //debugger;

            var eventMonth = moment(event.start).format("MM");
            var CurrentDate = $("#calendar").fullCalendar('getDate');
            var renderedMonth = moment(CurrentDate).format("MM");

            if (eventMonth != renderedMonth) {
                return false;
            }
            var updateCalHD = $("#UpdateCalendars").val();
            if (updateCalHD != "False") {

                var layer = '<div id="events-layer" class="fc-transparent tohide" style="position:absolute;' +
                'width:100%; height:100%; top:-1px; text-align:right; z-index:100">' +
                //'<a><img src="../../Content/themes/assets/img/remove-icon-small.png" title="delete" width="14" ' +
                //'id="delbut' + event.id + '" border="0" class="crossCSS"  /></a>' +
                '<button type="button" id="delbut' + event.id + '" class="close crossCSS" title="delete" data-dismiss="modal" aria-hidden="true"></button>' +
                '</div>';
                $(this).append(layer);
                $("#delbut" + event.id).hide();
                $("#delbut" + event.id).fadeIn(300);
                $("#delbut" + event.id).click(function () {
                    //$.post("DeleteCalItem", { eventId: event.id, showOrder: event.showOrder });
                    //deleteCalItem(event.menus_id, event.start, event.webCalID);
                    $("#menus_id").val(event.menus_id);
                    $("#startDate").val(event.start);
                    $("#webCalID").val(event.webCalID);
                    $('#DeleteIIemModal').modal({ backdrop: 'static', keyboard: false });


                });
                $("#edbut" + event.id).hide();
                $("#edbut" + event.id).fadeIn(300);
            }
        },
        eventMouseout: function (event, domEvent) {
            $('.tohide').remove();
            $('.tooltip').remove();

        },
        views: {
            agendaFourWeeks: {
                type: 'month',
                duration: { weeks: 4 },
                buttonText: '4 Weeks',
                fixedWeekCount: false
            }
        },


        events: {
            url: '/PreorderCal/AjaxHandler?CalId=' + getUrlVars()["id"],
            error: function () {
                $('#script-warning').show();
            }
        }

    });

    if ($('#UpdateCalendars').val() == "False") {
        $('#btnAction').addClass("disabled");
    }

    $("#AllowOrdersImg").click(function () {
        $('#AllowOrdersModal').modal({ backdrop: 'static', keyboard: false });
    });

    $("#ViewOnlyIMg").click(function () {
        $('#ViewOnlyModal').modal({ backdrop: 'static', keyboard: false });
    });

    $("#AdminOlnyimg").click(function () {
        $('#AdminOlnyModal').modal({ backdrop: 'static', keyboard: false });
    });

    $("#orderingOptionsImg").click(function () {
       // debugger;
        $('#orderingOptionsModal').on('show.bs.modal', function (e) {
            $('#btnOrderingOptions').addClass('defaultBtnClass');
        });
        $('#orderingOptionsModal').on('hidden.bs.modal', function () {
            $('#btnOrderingOptions').removeClass('defaultBtnClass');
        });

        $('#orderingOptionsModal').modal({ backdrop: 'static', keyboard: false });


    });





    $('#AllowOrdersModal').on('show.bs.modal', function (e) {
        $('#btnAllowOrders').addClass('defaultBtnClass');
    });
    $('#AllowOrdersModal').on('hidden.bs.modal', function () {
        $('#btnAllowOrders').removeClass('defaultBtnClass');
    });
    $('#btnAllowOrders').click(function () {
        showCurrentMonthMessag();
    });



    $('#ViewOnlyModal').on('show.bs.modal', function (e) {
        $('#btnViewOnly').addClass('defaultBtnClass');
    });
    $('#ViewOnlyModal').on('hidden.bs.modal', function () {
        $('#btnViewOnly').removeClass('defaultBtnClass');
    });
    $('#btnViewOnly').click(function () {
        showCurrentMonthMessag();
    });



    $('#AdminOlnyModal').on('show.bs.modal', function (e) {
        $('#btnAdminOnly').addClass('defaultBtnClass');
    });
    $('#AdminOlnyModal').on('hidden.bs.modal', function () {
        $('#btnAdminOnly').removeClass('defaultBtnClass');
    });
    $('#btnAdminOnly').click(function () {
        showCurrentMonthMessag();
    });






    $('#DeleteIIemModal').on('shown.bs.modal', function (e) {
        $('#btnConfirmDelete').addClass('defaultBtnClass');
    });
    $('#DeleteIIemModal').on('hidden.bs.modal', function () {
        $('#btnConfirmDelete').removeClass('defaultBtnClass');
    });



    $('#PreorderDayModal').on('show.bs.modal', function () {
        $('#imgbtnSearch').addClass('defaultBtnClass');
    });
    $('#PreorderDayModal').on('hidden.bs.modal', function () {
        $('#imgbtnSearch').removeClass('defaultBtnClass');
    });


    $("#close").click(function () {
        window.location.href = "/PreorderCalList/Table"
    });

    $(".savaForm").click(function (e) {
        //debugger;
        // skip ordering options from current month events check
        if ($(e.target).attr('id') == 'btnOrderingOptions') {
            deleteCalItem();
        }
        else {
            var events = $('#calendar').fullCalendar('clientEvents', function (evt) {
                return isCurrentMonthEvent(evt);
            });

           // if (events.length > 0) { //comment if to resolve Bug 1708

                var isShowDeleteMessage = $(e.target).attr('id') == 'btnConfirmDelete';
                deleteCalItem(isShowDeleteMessage);
            //}
        }
    });
    $(".AllowOrdersBtn").click(function () {
        var events = $('#calendar').fullCalendar('clientEvents', function (evt) {
            return isCurrentMonthEvent(evt);
        });

        if (events.length > 0) {
            UpdateCalByOrderStatusMonth('AllowOrders', 'AllowOrdersModal');
        }
    });

    $(".AdminOnlyBtn").click(function () {
        var events = $('#calendar').fullCalendar('clientEvents', function (evt) {
            return isCurrentMonthEvent(evt);
        });

        if (events.length > 0) {
            UpdateCalByOrderStatusMonth('AdminOnly', 'AdminOlnyModal');
        }
    });

    $(".ViewOnlyBtn").click(function () {
        var events = $('#calendar').fullCalendar('clientEvents', function (evt) {
            return isCurrentMonthEvent(evt);
        });

        if (events.length > 0) {
            UpdateCalByOrderStatusMonth('ViewOnly', 'ViewOnlyModal');
        }
    });

    $(".orderingOptionsBtn").click(function () {
        UpdateCalOrderingOptions();
    });







    $("#DDCutOffSelection").select2();
    $("#DropDownList2").select2();
    $("#sameDayAmPm").select2();
    $("#sameDayMinutes").select2();
    $("#sameDayHours").select2();
    //$("#sameDaySettings").select2();

    /////////////////
    enableDisableControls()
    $('#orderingoption').change(enableDisableControls);

    $("#none").change(ChangeValue);
    $("#cutoffsettings").change(ChangeValue);
    $("#sameDaySettings").change(ChangeValue);
    ShowHideSamedayRows();


});


function deleteCalItem(isShowDeleteMessage) {

    var fulldate = $("#startDate").val();
    // var startDatevar = moment(fulldate).format("MM/DD/YYYY");
    var startDatevar = moment.utc(fulldate).format("MM/DD/YYYY");
    var WebCalItem = new Object();

    WebCalItem.menus_id = $("#menus_id").val();
    WebCalItem.calItemDate = startDatevar;
    WebCalItem.webCalID = $("#webCalID").val();

    //debugger;
    $.ajax({
        type: "POST",
        url: "/PreorderCal/DeleteCalItem",
        //data: "{ 'calItemID': id, 'calItemDate': start, 'webCalID': webCalID }",
        data: JSON.stringify(WebCalItem),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            if (data.result == 'ok') {
                if (isShowDeleteMessage) {
                    displaySuccessMessage("Item removed successfully.");
                }
                $('#calendar').fullCalendar('refetchEvents');
                DeleteEventFromSource(WebCalItem.menus_id);
                $('#DeleteIIemModal').modal('hide');
            }
        },
        error: function (request, status, error) {
            if (isShowDeleteMessage) {
                displayErrorMessage("Error occurred during deleting the data.");
            }
            return false;
        }
    });


}

function UpdateCalByOrderStatusMonth(status, modalName) {
    var fulldate = $("#calendar").fullCalendar('getDate');
    var SelectedDatevar = moment(fulldate).format("MM/DD/YYYY")
    var WebCalItemStatus = new Object();

    WebCalItemStatus.Status = status;
    WebCalItemStatus.SelectedDate = SelectedDatevar;
    WebCalItemStatus.WebCalID = getUrlVars()["id"];

    //debugger;
    $.ajax({
        type: "POST",
        url: "/PreorderCal/UpdateCalItemStatus",
        data: JSON.stringify(WebCalItemStatus),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            if (data.result == 'ok') {
                displaySuccessMessage("Items updated successfully.");
                $('#calendar').fullCalendar('refetchEvents');
                $('#' + modalName).modal('hide');
            }
        },
        error: function (request, status, error) {
            displayErrorMessage("Error occurred during update.");
            return false;
        }
    });
}


function UpdateCalOrderingOptions() {

    var fulldate = $("#calendar").fullCalendar('getDate');
    var SelectedDatevar = moment(fulldate).format("MM/DD/YYYY")

    var orderinOptionsvar = 0;

    var noneSelected = $('#none').is(':checked');
    var cutoffsettingsSelected = $('#cutoffsettings').is(':checked');
    var sameDaySettingsSelected = $('#sameDaySettings').is(':checked');

    if (noneSelected) {
        orderinOptionsvar = 0;
    } else if (cutoffsettingsSelected) {
        orderinOptionsvar = 1;

    } else if (sameDaySettingsSelected) {
        orderinOptionsvar = 2;
    }


    var OrderingOptionsModel = new Object();

    OrderingOptionsModel.WebCalID = $("#hdWEBCalIDOOP").val(); //getUrlVars()["id"];
    OrderingOptionsModel.CutOffType = $("#DropDownList2").val();
    OrderingOptionsModel.CutOffValue = $("#txtcutoffday").val();
    OrderingOptionsModel.CutOffSelection = $("#DDCutOffSelection").val();
    OrderingOptionsModel.OrderingOption = orderinOptionsvar;

    OrderingOptionsModel.sameDayHours = $("#sameDayHours").val();
    OrderingOptionsModel.sameDayMinutes = $("#sameDayMinutes").val();
    OrderingOptionsModel.sameDayAmPm = $("#sameDayAmPm").val();
    OrderingOptionsModel.useSameDayOrdering = $("#hdusesamedayordering").val();
    OrderingOptionsModel.DistrictID = $("#hdDistrictIDOOP").val();

    //debugger;
    $.ajax({
        type: "POST",
        url: "/PreorderCal/UpdateOrderingOptions",
        data: JSON.stringify(OrderingOptionsModel),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            if (data.result == 'ok') {

                displaySuccessMessage("Ordering options updated successfully.");
                $('#calendar').fullCalendar('refetchEvents');
                $('#orderingOptionsModal').modal('hide');
            }
        },
        error: function (request, status, error) {
            displayErrorMessage("Error occurred during updating the data.");
            return false;
        }
    });
}

function isCurrentMonthEvent(evt) {
    var isCurrentMonthEvt = false;

    if (evt == null) {
        return false;
    }
    else {
        var today = new Date();
        var currentYear = today.getFullYear();
        var currentMonth = today.getMonth() + 1;
               
        var eventDate = evt.start._d;
        //Event Month and Year
        var eventCurrentYear = moment.utc(eventDate).format("YYYY");
        var eventCurrentMonth = moment.utc(eventDate).format("MM");


        return currentYear == eventCurrentYear && currentMonth == eventCurrentMonth;
    }
}


function showCurrentMonthMessag() {
    var events = $('#calendar').fullCalendar('clientEvents', function (evt) {
        return isCurrentMonthEvent(evt);
    });
    if (events.length == 0) {
        displayWarningMessage('There is no item in selected date.');
    }
}


function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function getSelectedMonth() {
    var CurrentDate = $("#calendar").fullCalendar('getDate');
    var date1 = moment(CurrentDate).format("MM/DD/YYYY")
    return date1;
}


function disableCutoffControls() {
    $('#DDCutOffSelection').attr('disabled', 'disabled');
    $('#txtcutoffday').attr('disabled', 'disabled');
    $('#DropDownList2').attr('disabled', 'disabled');
    $('#cutoffsettings').attr('checked', false);
}

function enableCutoffControls() {
    $('#DDCutOffSelection').removeAttr('disabled');
    $('#txtcutoffday').removeAttr('disabled');
    $('#DropDownList2').removeAttr('disabled');
    //$('#cutoffsettings').attr('checked', 'checked');
    $('#cutoffsettings').prop('checked', true);
}

function disableSameDayControls() {
    $('#sameDayHours').attr('disabled', 'disabled');
    $('#sameDayMinutes').attr('disabled', 'disabled');
    $('#sameDayAmPm').attr('disabled', 'disabled');
    $('#sameDaySettings').prop('checked', false);
}

function enableSameDayControls() {
    $('#sameDayHours').removeAttr('disabled');
    $('#sameDayMinutes').removeAttr('disabled');
    $('#sameDayAmPm').removeAttr('disabled');
    //$('#sameDaySettings').attr('checked', 'checked');
    $('#sameDaySettings').prop('checked', true);
}

function enableDisableControls() {
    //debugger;
    var orderingOptoion = $("#HDOrderingOption").val();
    //none
    if (orderingOptoion == "0") {
        disableCutoffControls();
        disableSameDayControls();
    } else if (orderingOptoion == "1") {
        disableSameDayControls();
        enableCutoffControls();
    }
    else if (orderingOptoion == "2") {
        disableCutoffControls();
        enableSameDayControls();
    }
}

function ChangeValue() {

    //debugger;
    if ($("#none").is(":checked")) {
        $("#HDOrderingOption").val("0");
    } else
        if ($("#cutoffsettings").is(":checked")) {
            $("#HDOrderingOption").val("1");
        } else
            if ($("#sameDaySettings").is(":checked")) {
                $("#HDOrderingOption").val("2");
            }

    enableDisableControls();
}

function ShowHideSamedayRows() {
    var sameDayOrdering = $("#hduseSameDayOrdering").val();

    if (sameDayOrdering == "true") {
        $("#samedaysettingsDiv").show();
        $("#samedaysettingsLbl").show();
        $("#scrollerDiv").height(220);


    } else {
        $("#samedaysettingsDiv").hide();
        $("#samedaysettingsLbl").hide();
        $("#scrollerDiv").height(120);

    }
}


var myCustomFetchFunction = function (start, end) {
    //debugger;
    var retevent = [];
    $.ajax({
        url: '/PreorderCal/AjaxHandler',
        dataType: 'json',
        data: {
            CalId: getUrlVars()['id'],
            // our hypothetical feed requires UNIX timestamps
            start: moment(start).format("MM/DD/YYYY"),
            end: moment(end).format("MM/DD/YYYY")
            // additional parameters can be added here
        },
        success: function (events) {
            //debugger;
            //var event = [];
            $.each(events, function (key, val) {
                var newEvent = new Object();

                newEvent.title = "some text" + key;
                newEvent.start = new Date();
                newEvent.allDay = false;

                retevent.push(newEvent);
            });
            //callback(event);
            // this is where I do some custom stuff with the events
            //myCustomFunction(events);
            //retevent = events;
            $('#calendar2').fullCalendar('addEventSource', retevent);
            $('#calendar2').fullCalendar({
                events: retevent
            });

            $('#calendar2').fullCalendar('render');
        }
    });
    return retevent;
};

var myCustomFunction = function (events) {
    // do something
}


function GetCutOffSettings(showModal,date) {

    debugger;
    var fulldate = $("#calendar2").fullCalendar('getDate');
    var SelectedDatevar = moment(fulldate).format("MM/DD/YYYY")
    var WebCalItemStatus = new Object();

    WebCalItemStatus.SelectedDate = SelectedDatevar;
    WebCalItemStatus.WebCalID = getUrlVars()["id"];

    var requestNew = false;
    var sDateCutoffSgs = $('#sDateCutoffSgs').val();
    if (sDateCutoffSgs == "0" || sDateCutoffSgs != SelectedDatevar) {
        requestNew = true;
        $('#sDateCutoffSgs').val(SelectedDatevar);
    } else {

        requestNew = false;
    }


    $.ajax({
        type: "POST",
        url: "/PreorderCal/GetWebLunchCutoffSettings",
        data: JSON.stringify(WebCalItemStatus),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            //debugger;
            if (data.result == 'ok') {
                //debugger;
                $('#hduseFiveDayWeekCutOff').val(data.fiveday);
                $('#CurrentCutofflbl').text(data.cutOFFdate);
                //added waqar q
                $('#lblSelectedDate').text(SelectedDatevar);
                //
                $('#txtcutoffday2').val(data.OverrideCutOffValue)

                $('#hdoverridecutoffvalue').val(data.OverrideCutOffValue);
                $('#hdhascutoffvalue').val(data.hasCutOffvalue);
                $('#hdcutOFFdate').val(data.cutOFFdate);
                $('#hdacceptorderdate').val(data.AcceptOrderDate);

                $('#hdcutoffvalue').val(data.Cutoffvalue);
                $('#hdisoverriddentcutoff').val(data.isOverriddentCutOff);

                checkdaysValue();
                //$('#' + modalName).modal('hide');

                if (showModal) {
                    $('#PreorderDayModal').modal({ backdrop: 'static', keyboard: false });
                    $('#PreorderDayModal').on('shown.bs.modal', function () {
                        $('#PreorderDayModal').off('shown.bs.modal');
                        $('#calendar2').fullCalendar('gotoDate', date);

                        //var event = { id: 1, title: 'New event', start: new Date() };

                        var startDateNew = moment(date).format("MM/DD/YYYY");
                        var endDateNew = moment(date).format("MM/DD/YYYY");
                        var cache = new Date().getTime();

                        $('#calendar2').fullCalendar('changeView', 'basicDay');//agendaDay
                        getEventsFromParent();
                        makeEventUnSelected();

                    })
                }
            }
        },
        error: function (request, status, error) {
            displayErrorMessage("Error occurred during update.");
            return false;
        }
    });
}



