jQuery(document).ready(function () {
    getMenuItems(0);

    $('#calendar2').fullCalendar({

        contentHeight: 320,
        editable: true,
        selectable: true,
        droppable: true, // this allows things to be dropped onto the calendar
       
        eventOrder: 'userOrder',

        loading: function (isLoading, view) {
            if (isLoading) {// isLoading gives boolean value
                $('#loadingDivDailyView').show();

            }
        },
        eventRender: function (event, element) {

            var updateCalHD = $("#UpdateCalendars").val();
            var delspan = $('<span />').addClass('deliconCSS').html('&nbsp;');
            if (updateCalHD != "False") {
                delspan.html('<button type="button" id="delbut2' + event.id + '" class="close crossCSS" title="delete" data-backdrop="static" data-keyboard="false" data-toggle="modal" href="#DeleteIIemModal"  aria-hidden="true"></button>');

                $(delspan).click(function () {
                    //debugger;
                    $("#menus_id").val(event.menus_id);
                    var CalID = getUrlVars()["id"];
                    $("#webCalID").val(CalID);

                });


            }

            element.find('.fc-content').append(delspan);

            element.find('.fc-title').addClass('itemNameCSS');
            //debugger;
            var span = $('<span />').addClass('priceValueCSS').html(formatDollar(event.price));
            element.find('.fc-content').append(span);

        },
        eventAfterAllRender: function (view) {
            //debugger;
            $('#loadingDivDailyView').hide();
            var $myTable = $(".DayHeaderClass");
            if ($myTable.is("html *")) {
                $myTable.remove();
            }
            var tdelement = $("#calendar2").find('.fc-view.fc-basicDay-view.fc-basic-view');
            $(tdelement).find('div.fc-row.fc-widget-header').append('<table id="DayHeaderTable"><tr><td><span>Assigned Items</span><span class="pricespan"> Price</span><span class="deviderspan"> | </span> </td>  </tr></table>');
            $("#DayHeaderTable").addClass('DayHeaderClass');



        },
        eventOverlap: function (stillEvent, movingEvent) {
            //debugger;
            var selecteddatevar = $("#startDate").val();
            //var selecteddateNew = moment(selecteddatevar).format('YYYY-MM-DD');
            var selecteddateNew = moment.utc(selecteddatevar).format('YYYY-MM-DD');
            var currentDate = moment().utc().format('YYYY-MM-DD');
            if (selecteddateNew <= currentDate) {
                displayWarningMessage('Items cannot be added to a current or past date.');
                return false;
            } else {


                var allow = allowOverlap(movingEvent.title)
                if (!allow) {
                    displayWarningMessage(movingEvent.title + ' already exist');
                    
                    
                }
                return allow;
               
            }
        },
        drop: function (date, jsEvent, ui, resourceId) {
            //debugger;

            var originalEventObject = $(this).data('event');

            var selecteddatevar = date;
            // var selecteddateNew = moment(selecteddatevar).format('YYYY-MM-DD');
            var selecteddateNew = moment.utc(selecteddatevar).format('YYYY-MM-DD');
            var currentDate = moment().utc().format('YYYY-MM-DD');

            if (selecteddateNew <= currentDate) {

                displayWarningMessage('Items cannot be added to a current or past date.');
                $('#calendar2').fullCalendar('removeEvents', function (event) {
                    return originalEventObject = event;
                });

            } else {
                var allow = allowOverlapInitially(originalEventObject.title)
                if (!allow) {
                    displayWarningMessage(originalEventObject.title + ' already exist');
                                    
                }

            }
        },
     
        eventReceive: function (event) {
            //debugger;
            var eventExistsAtParent = isEventAlreadyExistsAtParent(event.title);
            if (!isPastDate() && !eventExistsAtParent) {
                saveEventDataToDatabase(event);
            }
            
            if (eventExistsAtParent)
            {
                getEventsFromParent();
            }

        },
        viewRender: function (view, element) {
            //debugger;
            var selecteddatevar = $('#calendar2').fullCalendar('getDate');
            $("#startDate").val(selecteddatevar);

            var selecteddateNew = moment(selecteddatevar).format('YYYY-MM-DD');
            var items = $('#calendar').fullCalendar('clientEvents', function (event) {
                //debugger;
                if (moment(event.start).format("YYYY-MM-DD") == selecteddateNew) {
                    return true;
                }
                return false;

            });

            //debugger;
            $('#calendar2').fullCalendar('removeEvents', function (event) {
                return true;
            });
            if (items.length > 0) {
                //debugger;
                var str = items[0].color;
                $('#SelectedColor').val(str);
                str = items[0].textColor;
                $('#SelectedtextColor').val(str);
            } else {

                //default set Color
                $('#SelectedColor').val("#dbdbda");
                $('#SelectedtextColor').val("#121212");

            }
            $('#calendar2').fullCalendar('addEventSource', items);
            //alert('a');
            GetCutOffSettings();
        },


        eventClick: function (calEvent, jsEvent, view) {
            //debugger;
            makeEventSelected(calEvent.id);
            $('#SelectedEventID').val(calEvent.id);

        }
    });

    $("#moveUpDiv").click(function () {
        MoveUpItem();
    });

    $("#moveDownDiv").click(function () {
        MoveDownItem();
    });


    $("#ddlCategoryType").select2();

    $("#btnimgAllowOrders").click(function () {
        UpdateCalByOrderStatusDay('AllowOrders', 'AllowOrdersModal');

    });

    $("#btnimgViewonly").click(function () {
        UpdateCalByOrderStatusDay('ViewOnly', 'AllowOrdersModal');

    });

    $("#btnimgAdminonly").click(function () {
        UpdateCalByOrderStatusDay('AdminOnly', 'AllowOrdersModal');

    });
    $("#btnimgItemscheduler").click(function () {
        $("ItemSchedulerModal").show();

    });
    $("#btnimgbtnCutoff").click(function () {
        //
        OpenOverrideCutoffModal();

        //alert('a')

    });

    $(".OverrideCutoffBtn").click(function () {
        UpdateOverrideCutOff();
    });

    //date picker
    //$('#itemSchedulerCal').multiDatesPicker('destroy');
    //$('#itemSchedulerCal').multiDatesPicker({
    //    showWeek: true,
    //    dateFormat: "mm-dd-yy",
    //    dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
    //    defaultDate: getDateForDatePicker()
    //});

    initializeDatePicker();

    $('.ItemSchedulerBtn').click(function () {
        //save dates 
        //debugger;
        SaveItemsForSelectedDates();
    });


    $('#imgbtnSearch').click(function () {
        getMenuItems(1);
    });
    $("#ddlCategoryType").change(function () {
        getMenuItems(1);
    });


    //////
    $.datepicker._updateDatepicker_original = $.datepicker._updateDatepicker;
    $.datepicker._updateDatepicker = function (inst) {
        $.datepicker._updateDatepicker_original(inst);
        var afterShow = this._get(inst, 'afterShow');
        if (afterShow) {
            //alert('tttt');
            afterShow.apply((inst.input ? inst.input[0] : null));  // trigger custom callback
        }
    }

    disableClicksbasedOnRole();

});

function getMenuItems(isInitial) {
    //debugger;

    var SearchModel = new Object();
    if (isInitial == 0) {
        SearchModel.CategoryID = -1;
        SearchModel.searchStr = '';
        SearchModel.searchType = -1;

    } else {
        //debugger;
        var tempStr = $("#ddlCategoryType").val();
        var indexValue = -1;
        var searchType = -1;

        if (tempStr != -1) {
            indexValue = parseInt(tempStr);
            searchType = 1;
        }

        SearchModel.CategoryID = indexValue;
        SearchModel.searchStr = $("#txtSearch").val();
        SearchModel.searchType = searchType;
    }


    $.ajax({
        url: "/PreorderCal/AjaxHandlerMenuItems",
        type: "POST",
        dataType: "json",
        data: JSON.stringify(SearchModel),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            //debugger;
            $("#external-events").empty("");
            $("#external-events").append("<p id=\"eventsPara\"></p>");
            if (data.length > 0) {
                $(data).each(function (i, item) {
                    //alert(item.id);
                    $("#eventsPara").before($("<div class='fc-event'></div>")
                        .html(item.ItemName)
                        .attr("id", item.MenuID)
                        .attr("userOrder", 0)
                        .attr("color", '#CCC')
                        .attr("textColor", '#000')
                        .attr("price", item.StudentFullPrice)
                        .attr("menus_id", item.MenuID)
                        );
                });
            }
            else {
                $("#eventsPara").before($("<div></div>").html('<br/><div style="text-align:center;">No matches found.</div>'));
            }

            //////
            $('#external-events .fc-event').each(function () {

                // store data so the calendar knows to render an event upon drop
                $(this).data('event', {
                    title: $.trim($(this).text()), // use the element's text as the event title
                    id: $(this).attr("id"),
                    color: $(this).attr("color"),
                    userOrder: $(this).attr("userOrder"),
                    textColor: $(this).attr("textColor"),
                    price: $(this).attr("price"),
                    menus_id: $(this).attr("menus_id"),

                    stick: true // maintain when user navigates (see docs on the renderEvent method)
                });

                // make the event draggable using jQuery UI
                $(this).draggable({
                    scroll: false,
                    helper: 'clone',
                    //containment: 'body',
                    start: function (event, ui) {
                        $(this).addClass("ui-helper");
                        $(this).data("startingScrollTop", $(this).parent().scrollTop());

                    },
                    drag: function (event, ui) {
                        var st = parseInt($(this).data("startingScrollTop"));
                        ui.position.top -= $(this).parent().scrollTop() - st;
                    },
                    stop:function(event,ui)
                    {
                        $(this).removeClass('ui-helper');
                    },
                    revert: true,      // will cause the event to go back to its
                    revertDuration: 0  //  original position after the drag
                });

            });
            //$("#external-events").prepend("<div class=\"searchResultHeaderCSS\">Available Items</div>");

            ////
        },
        beforeSend: function () {
            $("#external-events").empty("");
            $("#external-events").append("<img class=\"loaderimgCSS\" src=\"../Content/themes/assets/img/input-spinner.gif\" />");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //debugger;
            alert(textStatus);
        }





    });


}

function getSelectedTextColor() {
    return $('#SelectedtextColor').val();
}


function getSelectedColor() {
    return $('#SelectedColor').val();
}


function MoveDownItem() {
    //debugger;
    var selectedIndex = parseInt($('#selectedEventIndex').val(), 0);

       var newPos = 0;
    var itemsBefore = $('#calendar2').fullCalendar('clientEvents', function (event) {
        return true;
    });

    if (itemsBefore.length == 0 || selectedIndex == 1000) {
        displayWarningMessage('Select an item to reorder.');
        return;
    }

    if (selectedIndex + 1 == itemsBefore.length) {
        displayWarningMessage('Cannot be moved further.');
        return;
    }

    var checkInt = selectedIndex + 1;
    if (checkInt < itemsBefore.length) {

        newPos = selectedIndex + 1;

        var temObject = itemsBefore[newPos];
        temObject.userOrder = temObject.userOrder - 1;
        var movingAheadObj = itemsBefore[selectedIndex];
        movingAheadObj.userOrder = movingAheadObj.userOrder + 1;
        itemsBefore[newPos] = movingAheadObj;
        itemsBefore[selectedIndex] = temObject;

        var itemsAfter = itemsBefore;

        $('#calendar2').fullCalendar('removeEvents');
        $('#calendar2').fullCalendar('removeEventSources');

        $('#calendar2').fullCalendar('addEventSource', itemsAfter);
        $('#calendar2').fullCalendar('refetchEvents');
        $('#selectedEventIndex').val(newPos);
        postItemsToServer(itemsAfter);
    }
}

function MoveUpItem() {
   // debugger;
    var selectedIndex = parseInt($('#selectedEventIndex').val(), 0); 
    var newPos = 0;
    var itemsBefore = $('#calendar2').fullCalendar('clientEvents', function (event) {
        return true;
    });

    if (itemsBefore.length == 0 || selectedIndex == 1000) {
        displayWarningMessage('Select an item to reorder.');
        return;
    }

    if (selectedIndex == 0) {
        displayWarningMessage('Cannot be moved further.');
        return;
    }


    if ((selectedIndex > 0) && (itemsBefore.length > 0) && (selectedIndex < 700)) {

        newPos = selectedIndex - 1;

        var temObject = itemsBefore[newPos];
        temObject.userOrder = temObject.userOrder + 1;
        var movingbackObj = itemsBefore[selectedIndex];
        movingbackObj.userOrder = movingbackObj.userOrder - 1;
        itemsBefore[newPos] = movingbackObj;
        itemsBefore[selectedIndex] = temObject;

        var itemsAfter = itemsBefore;

        $('#calendar2').fullCalendar('removeEvents');
        $('#calendar2').fullCalendar('removeEventSources');

        $('#calendar2').fullCalendar('addEventSource', itemsAfter);
        $('#calendar2').fullCalendar('refetchEvents');
        $('#selectedEventIndex').val(newPos);
        postItemsToServer(itemsAfter);
    }


}

function makeEventSelected(id) {
    //debugger;
    var AllEventitems = $('#calendar2').fullCalendar('clientEvents', function (event) {
        return true;
    });

    $.each(AllEventitems, function (key, value) {

        if (value.id == id) {
            value.color = "#c9d7ec"; //getSelectedColor();
            value.textColor = "#0c0c0d"; //getSelectedTextColor();
            $('#calendar2').fullCalendar('updateEvent', value);
            $('#selectedEventIndex').val(key);
            //alert(key);

        } else {
            value.color = getSelectedColor();
            value.textColor = getSelectedTextColor();
            $('#calendar2').fullCalendar('updateEvent', value);

        }

    });



}

function makeEventUnSelected() {
    //debugger;
    $('#selectedEventIndex').val(1000);

    var AllEventitems = $('#calendar2').fullCalendar('clientEvents', function (event) {
        return true;
    });

    $.each(AllEventitems, function (key, value) {
        {
            value.color = getSelectedColor();
            value.textColor = getSelectedTextColor();
            $('#calendar2').fullCalendar('updateEvent', value);

        }
    });

}

function GetNextUseOrder() {
    var retValue = 0;

    var AllEventitems = $('#calendar2').fullCalendar('clientEvents', function (event) {
        return true;
    });

    if (AllEventitems.length > 0) {
        retValue = Math.max.apply(Math, AllEventitems.map(function (o) { return o.userOrder; }));
        retValue = retValue + 1;
    } else {
        retValue = 1;
    }


    return retValue;


}

function reLoadEvents() {
    // debugger;
    var AllEventitems = $('#calendar2').fullCalendar('clientEvents', function (event) {
        return true;
    });

    $('#calendar2').fullCalendar('removeEvents');
    $('#calendar2').fullCalendar('removeEventSources');

    $('#calendar2').fullCalendar('addEventSource', AllEventitems);
    $('#calendar2').fullCalendar('refetchEvents');


}

function allowOverlap(title) {
    // debugger;
    var AllEventitems = $('#calendar2').fullCalendar('clientEvents', function (event) {
        return true;
    });
    var count = 0;
    var retValue = false;
    $.each(AllEventitems, function (key, value) {
        if (value.title == title) {
            count++;
        }

    });
    if (count == 0) {
        retValue = true;
    }
    return retValue;
}

function allowOverlapInitially(title) {
    // debugger;
    var AllEventitems = $('#calendar2').fullCalendar('clientEvents', function (event) {
        return true;
    });
    var count = 0;
    var retValue = false;
    $.each(AllEventitems, function (key, value) {
        if (value.title == title) {
            count++;
        }

    });
    if ((count == 0) || (count == 1)) {
        retValue = true;
    }
    return retValue;
}

function ItemsExists() {
   // debugger;
    var AllEventitems = $('#calendar2').fullCalendar('clientEvents', function (event) {
        return true;
    });
    var count = 0;
    var retValue = false;
    $.each(AllEventitems, function (key, value) {
        count++;

    });
    if (count > 0) {
        retValue = true;
    }
    return retValue;
}


function DeleteEventFromSource(id) {
    //debugger;
    var AllEventitems = $('#calendar2').fullCalendar('clientEvents', function (event) {
        return true;
    });

    for (var i = 0; i < AllEventitems.length; i++) {
        var obj = AllEventitems[i];

        if (parseInt(obj.menus_id) == parseInt(id)) {
            AllEventitems.splice(i, 1);
            i--;
        }
    }

    $('#calendar2').fullCalendar('removeEvents');
    $('#calendar2').fullCalendar('removeEventSources');

    $('#calendar2').fullCalendar('addEventSource', AllEventitems);
    $('#calendar2').fullCalendar('refetchEvents');


}


function formatDollar(numIn) {
    //debugger;
    var num = 0;
    if (numIn) {
        num = parseFloat(numIn);
    }
    var p = num.toFixed(2).split(".");
    return "$" + p[0].split("").reverse().reduce(function (acc, num, i, orig) {
        return num == "-" ? acc : num + (i && !(i % 3) ? "," : "") + acc;
    }, "") + "." + p[1];
}

function saveEventDataToDatabase(obj) {
    //debugger;
    var fulldate = $("#startDate").val();
    //var startDatevar = moment(fulldate).format("MM/DD/YYYY");
    var startDatevar = moment.utc(fulldate).format("MM/DD/YYYY");
    var WebCalItem = new Object();

    WebCalItem.menus_id = obj.menus_id;
    WebCalItem.calItemDate = startDatevar;
    WebCalItem.webCalID = getUrlVars()["id"];
    WebCalItem.mealType = $("#CalendarType").val();
    WebCalItem.userOrder = GetNextUseOrder();


    $.ajax({
        url: "/PreorderCal/SaveCalMenuItem",
        type: "POST",
        data: JSON.stringify(WebCalItem),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",

        success: function (data) {
            //debugger;
            if (data.result == 'ok') {
                var temp = data.recordID;
                obj.color = getSelectedColor();
                obj.textColor = getSelectedTextColor();
                obj.userOrder = WebCalItem.userOrder;

                $('#calendar2').fullCalendar('updateEvent', obj);
                makeEventUnSelected();
                reLoadEvents();
                $('#calendar').fullCalendar('refetchEvents');
            }


        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //debugger;
            alert(textStatus);
        }
    });
}

function postItemsToServer(menuItems) {
    //debugger;
    var formattedMenuItems = [];
    var len = menuItems.length;
    for (var i = 0; i < len; i++) {

        var userOrderLocal = i + 1;
        var WebCalItem = new Object();
        WebCalItem.menus_id = parseInt(menuItems[i].menus_id);
        var fulldate = menuItems[i].start;
        WebCalItem.calItemDate = moment(fulldate).format("MM/DD/YYYY");
        WebCalItem.webCalID = parseInt(getUrlVars()["id"]);
        WebCalItem.useOrder = parseInt(userOrderLocal);

        formattedMenuItems.push(WebCalItem);
    }

    newMenuItems = JSON.stringify({ 'items': formattedMenuItems });

    $.ajax({


        url: '/PreorderCal/UpdateMenuItems',
        type: "POST",
        data: newMenuItems,
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function () {
            $('#calendar').fullCalendar('refetchEvents');

        },
        failure: function (response) {

        }
    });

}

function UpdateCalByOrderStatusDay(status, modalName) {

    if (ItemsExists()) {
        var fulldate = $("#calendar2").fullCalendar('getDate');
        var SelectedDatevar = moment(fulldate).format("MM/DD/YYYY")
        var WebCalItemStatus = new Object();

        WebCalItemStatus.Status = status;
        WebCalItemStatus.SelectedDate = SelectedDatevar;
        WebCalItemStatus.WebCalID = getUrlVars()["id"];

        //debugger;
        $.ajax({
            type: "POST",
            url: "/PreorderCal/UpdateCalItemStatusDay",
            data: JSON.stringify(WebCalItemStatus),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data) {
                if (data.result == 'ok') {
                    displaySuccessMessage("Items updated successfully.");
                    $('#calendar').fullCalendar('refetchEvents');

                    var divlement = $("#calendar2").find('.fc-toolbar.fc-header-toolbar .fc-center');
                    //divlement.html('aa');
                    divlement.children().remove();
                    divlement.children('img').remove();
                    divlement.append("<img class=\"loaderimgCSS\" src=\"../Content/themes/assets/img/input-spinner.gif\" />");
                    //debugger;
                    setTimeout(function () {

                        getEventsFromParent();
                        divlement.children('img').remove();
                    }, 1000);


                    $('#' + modalName).modal('hide');
                }
            },
            error: function (request, status, error) {
                displayErrorMessage("Error occurred during update.");
                return false;
            }
        });
    } else {
        displayWarningMessage('There is no item in selected date.');
    }
}

function getEventsFromParent() {
   debugger;
    var selecteddatevar = $('#calendar2').fullCalendar('getDate');
    $("#startDate").val(selecteddatevar);

    var selecteddateNew = moment(selecteddatevar).format('YYYY-MM-DD');
    var retryTime = 2000;
    var interval = 1000;
    for (i = 0; i < 3; i++) {

        var items = $('#calendar').fullCalendar('clientEvents', function (event) {
            //debugger;
            //if (moment(event.start).format("YYYY-MM-DD") <= selecteddateNew && moment(event.end).format("YYYY-MM-DD") >= selecteddateNew) {
            if (moment(event.start).format("YYYY-MM-DD") == selecteddateNew) {
                return true;
            }
            return false;

        });

        setTimeout(function () {

        }, retryTime);


        if (items.length > 0) {
            //debugger;
            $('#calendar2').fullCalendar('removeEvents', function (event) {
                return true;
            });
            if (items.length > 0) {

                var str = items[0].color;
                $('#SelectedColor').val(str);
                str = items[0].textColor;
                $('#SelectedtextColor').val(str);
            } else {

                //default set Color
                $('#SelectedColor').val("#dbdbda");
                $('#SelectedtextColor').val("#121212");

            }
            $('#calendar2').fullCalendar('addEventSource', items);

            break;
        }
        retryTime = retryTime + interval;

    }





}

function isEventAlreadyExistsAtParent(title) {
    //debugger;
    var selecteddatevar = $('#calendar2').fullCalendar('getDate');

    var selecteddateNew = moment(selecteddatevar).format('YYYY-MM-DD');

    var items = $('#calendar').fullCalendar('clientEvents', function (event) {
        //debugger;
        //if (moment(event.start).format("YYYY-MM-DD") <= selecteddateNew && moment(event.end).format("YYYY-MM-DD") >= selecteddateNew) {
        if (moment(event.start).format("YYYY-MM-DD") == selecteddateNew) {
            return true;
        }
        return false;

    });

    var count = 0;

    $.each(items, function (key, value) {
        if (value.title == title) {
            count++;
        }

    });
    if (count == 0) {
        retValue = false;
    } else
        if (count > 0) {
            retValue = true;
        }

    return retValue;
}


function SaveItemsForSelectedDates() {

    if (ItemsExists()) {

        var fulldate = $("#calendar2").fullCalendar('getDate');
        var SelectedDatevar = moment(fulldate).format("MM/DD/YYYY")
        var ItemScheduler = new Object();
        var dates = $('#itemSchedulerCal').multiDatesPicker('getDates');
        datesStr = dates.join()


        ItemScheduler.WebCalID = getUrlVars()["id"];
        ItemScheduler.SourceDate = SelectedDatevar;
        ItemScheduler.dateList = datesStr;


        //debugger;
        $.ajax({
            type: "POST",
            url: "/PreorderCal/UpdateCalForSelectedDates",
            data: JSON.stringify(ItemScheduler),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data) {
                if (data.result == 'ok') {
                    displaySuccessMessage("Items updated successfully.");
                    $('#calendar').fullCalendar('refetchEvents');
                    getEventsFromParent();
                } else {
                    displayErrorMessage('Error occurred during update.');
                }
            },
            error: function (request, status, error) {
                displayErrorMessage("Error occurred during update.");
                return false;
            }
        });
    } else {
        displayWarningMessage('There is no item in selected date.');


    }

}


function UpdateOverrideCutOff() {
    debugger;
    checkdaysValue();
    var str = $("#txtcutoffday2").val()
    if (str.length == 0) {

        displayWarningMessage('Please fill cutoff date.');
        return false;
    }

    var fulldate = $("#calendar2").fullCalendar('getDate');
    var SelectedDatevar = moment(fulldate).format("MM/DD/YYYY")
    var OverrideCutOffData = new Object();


    OverrideCutOffData.pWebcalid = getUrlVars()["id"];
    OverrideCutOffData.pDate = SelectedDatevar;
    OverrideCutOffData.cutOffType = 0;
    OverrideCutOffData.cutOffValue = $("#txtcutoffday2").val();


    //debugger;
    $.ajax({
        type: "POST",
        url: "/PreorderCal/OverrideCutOffDate",
        data: JSON.stringify(OverrideCutOffData),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            if (data.result == 'ok') {
                displaySuccessMessage("Items updated successfully.");
                $('#calendar').fullCalendar('refetchEvents');

                setTimeout(function () {

                    getEventsFromParent();

                }, 3000);
                $('#OverrideCutoffModal').modal('hide');


            } else {
                displayErrorMessage('Error occurred during update.');
            }
        },
        error: function (request, status, error) {
            displayErrorMessage("Error occurred during update.");
            return false;
        }
    });

}


function getDateForDatePicker() {
    var fulldate = $("#calendar2").fullCalendar('getDate');
    var SelectedDatevar = moment(fulldate).format("M-D-Y")
    return SelectedDatevar;


}

$('#ItemSchedulerModal').on('shown.bs.modal', function () {

    initializeDatePicker();

});

function initializeDatePicker() {
    //debugger;
    var defdate = getDateForDatePicker()
    //debugger;
    $('#itemSchedulerCal').multiDatesPicker('destroy');
    $('#itemSchedulerCal').multiDatesPicker({
        showWeek: true,
        minDate: 0,
        dateFormat: "m-d-yy",
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        defaultDate: defdate,

        afterShow: function (inst) {
            //alert('after show');
            BindClicks();
        }
    });


    $('#itemSchedulerCal').multiDatesPicker('resetDates', 'picked');
    $('#itemSchedulerCal').multiDatesPicker('toggleDate', defdate);
    //BindClicks();
}

function BindClicks() {
    setTimeout(function () {
        //debugger;

        //var tem = $(".ui-datepicker-week-col");
        //var temp = $("table.ui-datepicker-calendar > thead > th");
        //var list = $("table.ui-datepicker-calendar > thead").find("th[scope='col']");
        $("table.ui-datepicker-calendar > thead").find("th[scope='col']").each(function () {
            //debugger;
            var ColTh = $(this);
            //ColTh.append("<span>aa</span>");
            if (!ColTh.hasClass('pointerCSS')) {
                ColTh.addClass('pointerCSS');

            }



            $(ColTh).click(function () {
                //alert($(ColTh).index())
                var colIndex = $(ColTh).index();
                var dateList = [];
                $(ColTh).closest('table').find('tr').each(function () {
                    //debugger;
                    var dateTd = $(this).find('td:eq(' + colIndex + ')');
                    var month = $(dateTd).attr('data-month');//.trigger('click');
                    var year = $(dateTd).attr('data-year');
                    var day = $(dateTd).children(0).text();
                    if (typeof month != 'undefined') {
                        var monthINT = parseInt(month.trim());
                        //debugger;
                        monthINT = monthINT + 1;
                        var newDate = monthINT + '-' + day.trim() + '-' + year.trim();
                        dateList.push(newDate);
                    }
                    //dateTd.css("background-color", "#FFBBBB");
                });
                //debugger;
                $('#itemSchedulerCal').multiDatesPicker('addDates', dateList);
                dateList = [];

            });

        });

        BindRowClicks();

        //
    }, 1000);
}

function BindRowClicks() {
    setTimeout(function () {
        //debugger;

        //var tem = $(".ui-datepicker-week-col");
        //var temp = $("table.ui-datepicker-calendar > thead > th");
        //var list = $("table.ui-datepicker-calendar > thead").find("th[scope='col']");
        $("table.ui-datepicker-calendar > tbody").find("td.ui-datepicker-week-col").each(function () {
            //debugger;
            var RowWkTD = $(this);
            //ColTh.append("<span>aa</span>");
            if (!RowWkTD.hasClass('pointerCSS')) {
                RowWkTD.addClass('pointerCSS');

            }



            $(RowWkTD).click(function () {
                //alert($(ColTh).index())
                var colIndex = $(RowWkTD).index();
                var dateList2 = [];
                $(RowWkTD).closest('tr').each(function () {
                    $.each(this.cells, function () {
                        //debugger;
                        var dateTd = $(this);
                        var month = $(dateTd).attr('data-month');//.trigger('click');
                        var year = $(dateTd).attr('data-year');
                        var day = $(dateTd).children(0).text();
                        if (typeof month != 'undefined') {
                            var monthINT = parseInt(month.trim());
                            //debugger;
                            monthINT = monthINT + 1;
                            var newDate = monthINT + '-' + day.trim() + '-' + year.trim();
                            dateList2.push(newDate);
                        }
                        //dateTd.css("background-color", "#FFBBBB");
                    });

                });
                //debugger;
                $('#itemSchedulerCal').multiDatesPicker('addDates', dateList2);
                dateList2 = [];

            });

        });


        //
    }, 1000);
}
function getMondays() {
    var d = new Date(),
        month = d.getMonth(),
        mondays = [];

    d.setDate(1);

    // Get the first Monday in the month
    while (d.getDay() !== 1) {
        d.setDate(d.getDate() + 1);
    }

    // Get all the other Mondays in the month
    while (d.getMonth() === month) {
        mondays.push(new Date(d.getTime()));
        d.setDate(d.getDate() + 7);
    }

    return mondays;
}

function OpenOverrideCutoffModal() {
    debugger;
    $('#loadingDiv').show();
    var selecteddatevar = $('#calendar2').fullCalendar('getDate');
    var selecteddateNew = moment(selecteddatevar).format('YYYY-MM-DD');
    //  var currentdate = moment().format('YYYY-MM-DD');
    var currentdate = moment.utc().format('YYYY-MM-DD');
    GetCutOffSettings();
    
    // process code after 2.5 second ... waith for init activity if pending in previous action
   setTimeout(function () {
    
    $('#loadingDiv').hide();
    var items = $('#calendar').fullCalendar('clientEvents', function (event) {
       if (moment(event.start).format("YYYY-MM-DD") == selecteddateNew) {
            return true;
        }
        return false;
      });

    if (selecteddateNew <= currentdate) {
        displayWarningMessage('Cannot be applied override cutoff settings for a past date.');
    }
    else if (items.length == 0) {
        displayWarningMessage('Cannot be applied override cutoff settings. There is no order item(s).');
    }
    else {
        var hasCutoffValue = $('#hdhascutoffvalue').val();
        //debugger;
        if (hasCutoffValue.length == 0 || hasCutoffValue == '') {
            //weait for ajax call

            hasCutoffValue = $('#hdhascutoffvalue').val();
            /*
            setTimeout(function () {
                hasCutoffValue = $('#hdhascutoffvalue').val();
            }, 2500);
            */
        }

        if (hasCutoffValue == 'false') {
            displayWarningMessage('The calendar must have cutoff settings configured \n for the override function to be available.');
        } else {
            var OverrideCutOffValue = $('#hdoverridecutoffvalue').val();
            var isOverriddentCutOff = OverrideCutOffValue != "" && OverrideCutOffValue != "0";
            var cutoffDate = $('#hdcutOFFdate').val();
            var cutOffDate = moment(cutoffDate).format('YYYY-MM-DD');
            var accepdate = $('#hdacceptorderdate').val();
            var AcceptOrderDate = moment(accepdate).format('YYYY-MM-DD');

            var orderingClosed = AcceptOrderDate >= cutOffDate;

            if (!orderingClosed && !isOverriddentCutOff) {
                // displayWarningMessage('The calendar must have cutoff settings configured \n for the override function to be available.');
                displayWarningMessage('"Cutoff  Override” settings can only be applied to dates that have been closed for ordering by the system.');
            } else {
                $('#OverrideCutoffModal').modal({ backdrop: 'static', keyboard: false });
            }

        }

    }

    }, 2000);


}

function isPastDate() {

    var selecteddatevar = $('#calendar2').fullCalendar('getDate');
    var selecteddateNew = moment.utc(selecteddatevar).format('YYYY-MM-DD');
    var CurrentDate = moment().utc().format('YYYY-MM-DD');
    if (selecteddateNew <= CurrentDate) {
        return true;
    } else {
        return false;
    }
}

function disableClicksbasedOnRole() {
    var HiddenFieldVal = $("#UpdateCalendars").val();

    if (HiddenFieldVal == "False") {
        //debugger;
        $("#btnimgAllowOrders").off("click").on;
        $("#btnimgViewonly").off("click");
        $("#btnimgAdminonly").off("click");
        $(".ItemSchedulerBtn").off("click");
        $(".OverrideCutoffBtn").off("click");
        $(".AllowOrdersBtn").off("click");
        $(".ViewOnlyBtn").off("click");
        $(".AdminOnlyBtn").off("click");
        $(".orderingOptionsBtn").off("click");
    }

}


$('#btnCancelOverrideCutoff').click(function () {
    $('#txtcutoffday2').val('');
    $('#NewCutofflbl').text('');
});
$('#btnCloseOverrideCutoff').click(function () {
    $('#txtcutoffday2').val('');
    $('#NewCutofflbl').text('');
});

$('#btnCloseOrderingOptions').click(function () {
    location.reload();
});
$('#btnCancelOrderingOptions').click(function () {
    location.reload();
});


$('#OverrideCutoffModal').on('shown.bs.modal', function (e) {
    $('#btnOverrideCutoff').addClass('defaultBtnClass');
})

$('#OverrideCutoffModal').on('hidden.bs.modal', function () {
    $('#btnOverrideCutoff').removeClass('defaultBtnClass');
});

$('#PreorderDayModal').on('shown.bs.modal', function (e) {
    $("#txtSearch").focus();
});

$('#PreorderDayModal').on('hidden.bs.modal', function () {
    $('#txtSearch').val('');
    $('#ddlCategoryType').val('-1').trigger('change');
});
