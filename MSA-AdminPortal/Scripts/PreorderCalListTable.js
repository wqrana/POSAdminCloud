var oTable;
$(document).ready(function () {
   

    debugger;

    $('.multicss').select2();
    oTable = $('#PreorderCalList').dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-4 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100],
            [10, 25, 50, 100] // change per page values here
        ],
        "bProcessing": true,
        "order": [[1, "asc"]],

        "iDisplayLength": 10,
        "sPaginationType": "bootstrap_full_number",
        "oLanguage": {
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
            "sInfoEmpty": 'No records found',
            "sEmptyTable": "No records found",
            //"sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": {
                "sPrevious": "Prev",
                "sNext": "Next"
            }
        },
        "fnDrawCallback": function () {
            debugger;
            //$('.dataTables_scrollBody').scrollTop(0);

            // $('body').scrollTop(0);

            window.scrollTo(0, 0);

            $('#PreorderCalList tbody').find('div#AssignedSchoolDiv')
           .each(function () {
                //debugger;
                $(this).removeAttr('hidden');

            });
            
            disableEditDeleteLinks();

        },

        "aoColumns": [
            {
                "bSearchable": false,
                "sWidth": "10%",
                "bSortable": false,
                "sClass": "center",
                "mRender": function (data, type, row) {
                   // debugger;
                    var innerHML = row[0];
                    var id = $(innerHML).text();

                    var string = "<a  title=\"Edit\" class=\"CalendarEditTile\" onclick=\"javascript:CreateOrEdit(this);\" href=\"#basic_modalPopup\" data-toggle=\"modal\" data-backdrop=\"static\" data-keyboard=\"false\" class=\"DeleteLink\" data-id=\"" + id + "\"><i class=\"fa fa-pencil-square-o fasize\"></i></a>";
                    string += " <span class=\"faseparator\"> | </span> ";
                    string += " <a  title=\"Delete\" class=\"CalendarDeleteTile\" onclick=\"javascript:Delete(this);\" href=\"#Delete\" data-toggle=\"modal\" data-backdrop=\"static\" data-keyboard=\"false\" class=\"DeleteLink\" data-id=\"" + id + "\"><i class=\"fa fa-trash fasize\"></i></a>";
                    string += " <span class=\"faseparator\"> | </span> ";
                    string += "<a  title=\"Calendar\" class=\"CalendarViewCSS\" href=\"/PreorderCal/index?id=" + id + "\"><i class=\"fa fa-calendar fasize\"></i></a>";

                    return string;
                }
            },
            {
                "bSortable": true,
                "sWidth": "25%",
                "sClass": "floattop",
                //Calendar Name
            },
            {
                "bSortable": true,
                "sClass": "floattop",
                "sWidth": "15%",
                //Calendar type
            },
            {
                "bSortable": false,
                "sClass": "floattop",
                "sWidth": "50%",
            }
        ]


    });

    /////////////
    /*
    $('select.multicss').change(function () {

        debugger;
        var calID = this.id;
        var listID = '#' + calID;
        var SchoolsList = $(listID).val();
        //$('select.multicss').val();
        if (SchoolsList == null)
        {
            SchoolsList = '';
        }

        var dataString = JSON.stringify({
            allData: calID + '*' + SchoolsList
        });

        $.ajax({
            type: "POST",
            url: "/PreorderCalList/updateSchoolsList",
            data: dataString,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.result == '-1') {

                    displaySuccessMessageOnce("Changes have been saved successfully.");
                }
            },
            error: function (request, status, error) {
                displayErrorMessage("Error occurred during saving the data.");
                return false;
            }
        });
        */
   
   

    $("#calTypes").select2();
    //var $input = $('#refresh');
    //var txt = $input.val();
    //debugger;
    //if (txt == "yes") {
    //    location.reload(true);
    //} else {
    //    $input.val('yes');
    //}
    if (!!window.performance && window.performance.navigation.type == 2) {
        window.location.reload();
    }
    

});

function CalendarAssignSchools(calID, SchoolsList) {

        if (SchoolsList == null) {
            SchoolsList = '';
        }

        var dataString = JSON.stringify({
            allData: calID + '*' + SchoolsList
        });

        $.ajax({
            type: "POST",
            url: "/PreorderCalList/updateSchoolsList",
            data: dataString,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.result == '-1') {

                    displaySuccessMessageOnce("Changes have been saved successfully.");
                }
            },
            error: function (request, status, error) {
                displayErrorMessage("Error occurred during saving the data.");
                return false;
            }
        });

    }
var controller = '/PreorderCalList/';
var CreateAction = controller + 'CreateCalendar';
var EditAction = controller + 'EditCalendar';

function CreateOrEdit(e) {
    // Get the id from the link
    var id = $(e).attr("data-id");

    var url = '';
    if (id == '0') {
        url = CreateAction;
    }
    else {
        url = EditAction;
    }
    
    if (id != '') {

        $.ajax({
            type: "get",
            url: url,
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                
                var model = data;

                if (model.IsError) {
                    displayWarningMessage(model.ErrorMessage);
                }
                else {
                    
                    var height = '241px';
                    $('#savebtnSpan').text(model.savebtnCaption);
                    $("#calNamelbl").text(model.Title);
                    clientdelId = model.DistrictID;
                    $("#CalendarName").val($.trim(model.CalendarName));
                    $("#distID").val(model.DistrictID)
                    $("#calID").val(model.WebCalID)
                    if (id == '0') {
                        $('#divCalendarType').show();
                        $("#calSpan").hide();

                    } else {
                        $("#calLink").attr("href", "/PreorderCal/index?id=" + model.WebCalID);
                        $("#calSpan").show();
                        $('#divCalendarType').hide();

                    }

                }
            },
            error: function () {
                displayWarningMessage(model.LoadErrorMessage);
            }
        });
    }

}

$(".savaForm").click(function () {
    

    var id = $("#calID").val();
    var distID = $("#distID").val();
    var name = $("#CalendarName").val();
    var calType = $("#calTypes").val();

    var url = '';
    var create = id == '0';
    if (create) {
        url = CreateAction;
    }
    else {
        url = EditAction;
    }

    if (name.trim() == "") {
        displayWarningMessage("Please enter calendar name.");
        return false;
    } else if (calType == "0" && id == '0') {
        displayWarningMessage("Please select calendar type.");
        return false;
    }
    else {

        $.ajax({
            type: "post",
            url: url,
            data: {
                "WebCalID": id,
                "CalendarName": name,
                "DistrictID": distID,
                "CalendarType": calType
            },
            dataType: "json",
            success: function (data) {

                var model = data;

                if (model.IsError) {
                    displayWarningMessage(model.ErrorMessage);
                }
                else {
                    
                    if (model.WebCalID == -999) {
                        displayWarningMessage('The calendar name already exists.');
                        $("#calSpan").hide();

                    }
                    else {
                        if (create) {

                            displaySuccessMessage('Calendar created  successfully.');
                            $("#calLink").attr("href", "/PreorderCal/index?id=" + model.WebCalID);
                            $("#calSpan").show();
                            window.location.reload(false);

                        }
                        else {
                            displaySuccessMessage('Calendar name updated successfully.');
                            $("#calLink").attr("href", "/PreorderCal/index?id=" + model.WebCalID);
                            $("#calSpan").show();
                            window.location.reload(false);
                        }
                    }
                    //window.location.reload(true);
                    oTable.fnDraw();
                }
            },
            error: function () {
                displayWarningMessage(model.ErrorMessage);
            }
        });
    }
});

function disableEditDeleteLinks() {

    ///////////////////////////////////////////////////////////////////////////////////////////////
    //Security handler
    ///////////////////////////////////////////////////////////////////////////////////////////////
    disableCreationRights("CreatePreorderCalendars", "btnAddNewCalendar", "aAddNewCalendar", "disabled", "ActionLink", "You don’t have rights to create a calendar.");

    disableEditLinksTile("UpdatePreorderCalendars", "CalendarEditTile", "ActionLink", "You don’t have rights to edit a calendar.");
    removeCrossBtns("UpdatePreorderCalendars");
    

    disableDeleteLinksTile("DeletePreorderCalendars", "CalendarDeleteTile", "You don’t have rights to delete a calendar.")
    disableLink("ViewPreorderCalendars", "CalendarViewCSS", "You don’t have rights to view calendar.");
   
    ///////////////////////////////////////////////////////////////////////////////////////////////

}
$('#basic_modalPopup').on('hidden.bs.modal', function () {
    // do something…
    window.location.reload();
});

$('#basic_modalPopup').on('shown.bs.modal', function (e) {
    $('#btnSaveCal').addClass('defaultBtnClass');
    $('#CalendarName').focus();
})

$('#basic_modalPopup').on('hidden.bs.modal', function () {
    $('#btnSaveCal').removeClass('defaultBtnClass');
});

$('#Delete').on('shown.bs.modal', function (e) {
   // $('#btnDelete').addClass('defaultBtnClass');
})

$('#Delete').on('hidden.bs.modal', function () {
  //  $('#btnDelete').removeClass('defaultBtnClass');
});