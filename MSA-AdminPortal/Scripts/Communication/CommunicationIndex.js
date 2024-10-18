var parentAlert_Id = null;
var oParentAlertGrid = null;
var oDIstrictList = null;
$(document).ready(function () {
    // UF Review: replace 1 == 0 with Communication.aspx.cs line number 28 condition (!DistrictUsers.CreateAlertBanners).
   // $('#districtList').select2();
  //  oDIstrictList = $('.SlectBox').SumoSelect({ placeholder: "Select District(s)", csvDispCount: 2, captionFormatAllSelected: '{0} All selected!', search: true, isClickAwayOk: true, });

    $('#processingDiv').hide();
    if (1 == 0) {
        $("#btnOpenAlertPopup").prop("disabled", "disabled");
    }

    loadParentAlertGridData();
    loadDistrictList();

    $("#txtStartDate").datepicker({
        minDate: -1,
        maxDate: -2,
        autoclose: true,
        format: 'mm/dd/yyyy',
        multidate: false,
        multidateSeparator: 'Ad33L',
    }).change(function () {
        console.log($('#txtStartDate').val());
        //$("#txtEndDate").datepicker({ minDate: $('#txtStartDate').val() });
        $('#txtEndDate').datepicker('setStartDate', $('#txtStartDate').val());
    });

    $("#txtEndDate").datepicker({
        minDate: -1,
        maxDate: -2,
        autoclose: true,
        format: 'mm/dd/yyyy',
        multidate: false,
        multidateSeparator: 'Ad33L',
    }).change(function () {
        console.log($('#txtEndDate').val());
        //$("#txtEndDate").datepicker({ minDate: $('#txtStartDate').val() });
        $('#txtStartDate').datepicker('setEndDate', $('#txtEndDate').val());
    })

   
    //$('#districtList').multiSelect({
    //    selectableHeader: "<div><!--<b>Available</b>--></div>",
    //    selectionHeader: "<div><!--<b>Selected</b>--></div>"
    //});
   // $('#districtList').select2();

   // $('.SlectBox').SumoSelect({ csvDispCount: 2, selectAll: true, captionFormatAllSelected: '{0} All selected!', search: true, isClickAwayOk: true });

});

$("#btnOpenAlertPopup").click(function () {
    $('#hdnAlertId').val('');
});

$("#btnConfirmDelete").click(function (e) {
    DeleteParentAlert();
});

function loadParentAlertGridData() {
    oParentAlertGrid = $('#dtParentAlertGrid').DataTable({
       /* 'scrollX': true,*/
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-5 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "bDestroy": true,
        "responsive": true,
        "iSortCol_0": 1,
        "drawCallback": function (settings) {
            $('body').scrollTop(0);
            $('.parent-alert-active').parent().prop("style", "background-color: #3dae48");
            $('.parent-alert-inactive').parent().prop("style", "background-color: #949599");
        },
        "aLengthMenu": [
            [10, 25, 50, 100],
            [10, 25, 50, 100] // change per page values here
        ],
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": "/Communication/AjaxParentAlertList",
        // set the initial value
        "iDisplayLength": 10,
        "sPaginationType": "bootstrap_full_number",
        "fnServerData": function (sSource, aoData, fnCallback) {
            //aoData.push(
            //        { "name": "SearchValue", "value": $("#SearchStr").val() },
            //        { "name": "SearchBy", "value": $("#searchdll").val() }
            //    );
            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json);
                window.scrollTo(0, 0);
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
                "bSortable": false,
                "bVisible": false,
                "sWidth": "12.5%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.ID + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "12.5%",
                "mRender": function (data, type, row) {
                    if (row.Status == "Active") {
                        return '<div class="parent-alert-active" style="color: white">' + row.Status + '</div>';
                    }
                    else {
                        return '<div class="parent-alert-inactive" style="color: white">' + row.Status + '</div>';
                    }
                }
            },
            {
                "bSortable": true,
                "sWidth": "12.5%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.MessageCreated + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "12.5%",
                "mRender": function (data, type, row) {
                    var editPermission = true;
                    var href = "#parentAlertPopup";

                    // UF Review: replace 1 == 1 with Communication.aspx.cs line number 140 condition (!DistrictUsers.ModifyAlertBanners).
                    if (row.USERID == "0" && !($('#hdnUpdateMSAAlerts').val() == "True")) {
                        editPermission = false;
                        href = "#";
                    }
                    else if ($('#hdnUpdateMSAAlerts').val() == "False") {
                        editPermission = false;
                        href = "#";
                    }
                    return '<a href=\"' + href + '\" class=\"EditSecurityClass\" onclick=loadParentAlertPopupData(' + row.ID + ',' + editPermission + '); data-toggle=\"modal\" data-backdrop=\"static\" data-keyboard=\"false\" >' + row.MessageName + '</a>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "12.5%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.MessageText + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "12.5%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.MessageStart + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "12.5%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.MessageEnd + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "12.5%",
                "mRender": function (data, type, row) {
                    return '<div>' + row.userid2 + '</div>';
                }
            },
            {
                "bSortable": false,
                "sWidth": "12.5%",
                "mRender": function (data, type, row) {
                    // UF Review: replace 1 == 0 with Communication.aspx.cs line number 164 condition (!DistrictUsers.DeleteAlertBanners).
                    // if (row.District_Id == 0 || !($('#hdnDeleteMSAAlerts').val() == "True")) comment against bug 1763
                    if (!($('#hdnDeleteMSAAlerts').val() == "True"))

                    {
                        // can't delete
                        return '<div><i title="No delete permission" class="fa fa-trash fasize" style:"color:gray;"></i></a></div>';
                    }
                    else {
                        return '<div><a title="Delete" href="#parentAlertDeleteWarning" onclick="setParentAlertName(' + row.ID + ",\'" + row.MessageName.trim() + '\');" role="button" class="DeleteSecurityClass" data-toggle="modal" data-backdrop="static" data-keyboard="false"><i class="fa fa-trash fasize"></i></a></div>';
                    }
                }
            }

        ],

        //initComplete: function () {
        //var input = $('.dataTables_filter input').unbind(),
        //self = this.api(),
        //$searchButton = $('<button>')
        //            .text('search')
        //            .addClass('btn yellow')
        //            .attr("style", "width: 120px;")
        //            .click(function () {
        //                self.search(input.val()).draw();
        //               })

        //}
    });
}

function loadParentAlertPopupData(alertId, editPermission) {

    

    if (editPermission == false) {
        //displayWarningMessage('MSA generated system alerts cannot be edited. Please contact Food Service Solutions to request a change if needed.');
        displayWarningMessage("You don't have rights to edit MSA Alert.");
        //$('#infoTitle').html('Message');
        //$('#infoMessage').html('MSA generated system alerts cannot be edited. Please contact Food Service Solutions to request a change if needed.');
        //$('#informationModal').modal('show');
        return false;
    }
    else {
        $('#hdnAlertId').val(alertId);

        $('#alertPopupHeading').html('Edit Alert');

        $.ajax({
            url: "/Communication/AjaxGetParentAlert",
            data: {
                parentAlertId: alertId // The data to send (will be converted to a query string)
            },
            type: "GET",
            dataType: "json", // The type of data we expect back
        })
          // Code to run if the request succeeds (is done). The response is passed to the function
          .done(function (json) {
              //debugger;
              $('#districtList').attr('disabled', 'disabled');
              $('#isForEdit').val('true');

              var startDate = "";
              var endDate = "";
              if (json.aaData.MessageStart != null && json.aaData.MessageStart != "") {
                  startDate = new Date(parseInt(json.aaData.MessageStart.substr(6)));
                  var month = startDate.getMonth() + 1;
                  startDate = month + "/" + startDate.getDate() + "/" + startDate.getFullYear();
              }
              if (json.aaData.MessageEnd != null && json.aaData.MessageEnd != "") {
                  endDate = new Date(parseInt(json.aaData.MessageEnd.substr(6)));
                  var month = endDate.getMonth() + 1;
                  endDate = month + "/" + endDate.getDate() + "/" + endDate.getFullYear();
              }

              $('#txtTitle').val(json.aaData.MessageName);
              $('#txtMessage').val(json.aaData.MessageText);

              //$('#address').html(json.aaData.Address + "<br />" + json.aaData.City + ", " + json.aaData.State + " " + json.aaData.Zip);
              $('#txtStartDate').val(startDate);

              if (json.aaData.MessageEnd == null || json.aaData.MessageEnd == "") {
                  $("#chkNoEndDate").attr("checked", true);
                  $("#chkNoEndDate").parent().addClass("checked");
                  $("#txtEndDate").attr('disabled', 'disabled');
                  $('#txtEndDate').val('');
              }
              else {
                  $("#chkNoEndDate").removeAttr("checked");
                  $("#chkNoEndDate").parent().removeClass("checked");
                  $("#txtEndDate").removeAttr("disabled");
                  $('#txtEndDate').val(endDate);
              }
              if (json.aaData.Enabled == null || json.aaData.Enabled == "" || json.aaData.Enabled == false) {
                  $("#chkActive").removeAttr("checked");
                  $("#chkActive").parent().removeClass("checked");
                  document.getElementById("chkActive").checked = false;
              }
              else {
                  $("#chkActive").attr("checked", true);
                  $("#chkActive").parent().addClass("checked");
                  document.getElementById("chkActive").checked = true;
              }

              if (json.aaData.DistrictGroup == "AllDistricts" ||
                  json.aaData.DistrictGroup == "AllCreditCardDistricts" ||
                  json.aaData.DistrictGroup == "AllShoppingCartDistricts" ||
                  json.aaData.DistrictGroup == "AllPreorderDistricts" ||
                  json.aaData.DistrictGroup == "AllLiveDistricts")
              {
                  $("#chkMultipleAlerts").attr("checked", true);
                  $("#chkMultipleAlerts").parent().addClass("checked");
              }
              else {
                  $("#chkMultipleAlerts").removeAttr("checked");
                  $("#chkMultipleAlerts").parent().removeClass("checked");
              }

              if (json.aaData.DistrictGroup == "AllDistricts") {
                  $("input[name='districtGroup']")[0].checked = true;
                  $("input[name='districtGroup']").button("refresh");
                  $('#txtDistrictGroupSelection').val('AllDistricts');
              }
              else if (json.aaData.DistrictGroup == "AllCreditCardDistricts") {
                  $("input[name='districtGroup']")[1].checked = true;
                  $("input[name='districtGroup']").button("refresh");
                  $('#txtDistrictGroupSelection').val('AllCreditCardDistricts');
              }
              else if (json.aaData.DistrictGroup == "AllShoppingCartDistricts") {
                  $("input[name='districtGroup']")[2].checked = true;
                  $("input[name='districtGroup']").button("refresh");
                  $('#txtDistrictGroupSelection').val('AllShoppingCartDistricts');
              }
              else if (json.aaData.DistrictGroup == "AllPreorderDistricts") {
                  $("input[name='districtGroup']")[3].checked = true;
                  $("input[name='districtGroup']").button("refresh");
                  $('#txtDistrictGroupSelection').val('AllPreorderDistricts');

              }
              else if (json.aaData.DistrictGroup == "AllLiveDistricts") {
                  $("input[name='districtGroup']")[4].checked = true;
                  $("input[name='districtGroup']").button("refresh");
                  $('#txtDistrictGroupSelection').val('AllLiveDistricts');

              }

              else {

                  $('input[name=districtGroup]').removeAttr("checked");
              }
          })
          // Code to run if the request fails; the raw request and status codes are passed to the function
          .fail(function (xhr, status, errorThrown) {
              console.log("Error: " + errorThrown);
              console.log("Status: " + status);
              console.dir(xhr);
          })
          // Code to run regardless of success or failure;
          .always(function (xhr, status) {
          });
    }//else
}

$.fn.addItems = function (data) {
    return this.each(function () {
       
        var list = this;
        $.each(data, function (index, itemData) {
            var option = new Option(itemData.Name, itemData.Id);
            list.add(option);
        });

        //$('#districtList').multiSelect('refresh');
       // loadDistrictListUI();

    });
};

$('#select-all').click(function () {
   
    $('select.SlectBox')[0].sumo.selectAll();
    enableDisableGroupSection();
});

$('#deselect-all').click(function () {
  //  $('#districtList').multiSelect('deselect_all');
    //$('#districtGroup').find('label').removeAttr('disabled');
  
    $('select.SlectBox')[0].sumo.unSelectAll();
    enableDisableGroupSection();
    
});


$('#uncheckdistrictgroups').click(function () {
    
      $("input:radio").attr("checked", false);

});




$('select.SlectBox').on('sumo:closed', function (sumo) {
   
    enableDisableGroupSection();

});

function enableDisableGroupSection() {

    var count = $("#districtList :selected").length;
    if (count > 0) {
        $("input:radio").attr("checked", false);
        $("input:radio").attr('disabled', 'disabled');
    }
    else {

        $("input:radio").removeAttr('disabled');
    }
}
function loadDistrictListUI() {
   // debugger;
    /*
    if ($('#isForEdit').val() == 'true') {
        //$('#districtList').multiSelect('destroy');
      //  $('#districtList').attr('disabled', 'disabled');
    }
    $('#districtList').multiSelect('destroy');
    $('#districtList').multiSelect({
        selectableHeader: "<div><!--<b>Available</b>--></div>",
        selectionHeader: "<div><!--<b>Selected</b>--></div>"  ,
        afterSelect: function (values) {
            debugger;
            if ($('#ms-districtList').children('.ms-selection').find('.ms-selected').length > 0) {
                // One or more items are selected
                $('#districtGroup').find('label').attr('disabled', 'disabled');
                $('#districtGroup').find('label').removeClass('active');
            }
            else {
                // Nothing is selected
                $('#districtGroup').find('label').removeAttr('disabled');
            }
        },
        afterDeselect: function (values) {
            debugger;
            if ($('#ms-districtList').children('.ms-selection').find('.ms-selected').length > 0) {
                // One or more items are selected
                $('#districtGroup').find('label').attr('disabled', 'disabled');
                $('#districtGroup').find('label').removeClass('active');
            }
            else {
                // Nothing is selected
                $('#districtGroup').find('label').removeAttr('disabled');
            }
        }
       
        
    });
     */
    if ($('#txtDistrictList').val() == '' || $('#isForEdit').val() == 'true') {
       // $("#districtList").val('');
        $('select.SlectBox')[0].sumo.unSelectAll();
    }
    else {
        $('select.SlectBox')[0].sumo.unSelectAll();

        //$('select.SlectBox')[0].sumo.selectItem('volo');

        $("#districtList").val(jQuery.unique($('#txtDistrictList').val().split(',')));
        $('select.SlectBox')[0].sumo.reload();
    }

   // if ($('#isForEdit').val() != 'true') {
       var txtDistrictGroupSelection=  $('#txtDistrictGroupSelection').val();
       if (txtDistrictGroupSelection != '') {
         //  $("input:radio").attr("checked", false);
           var index = 0;

           switch (txtDistrictGroupSelection) {

               case 'AllCreditCardDistricts':
                   index = 1;
                   break;
                   
               case 'AllShoppingCartDistricts':
                   index = 2;
                   break;

               case 'AllPreorderDistricts':
                   index = 3;
                   break;
               case 'AllLiveDistricts':
                   index = 4;
                   break;

                   
            }
                    
           $("input[name='districtGroup']")[index].checked = true;
           $("input[name='districtGroup']").button("refresh");

       }
       else
       {
           $("input:radio").attr("checked", false);
       }
   // }

    

    if ($('#hdnAlertId').val() == '') {
        //Create Alert
        $('#districtList').removeAttr('disabled');
        //$('#ms-districtList').find('#ms-list').removeAttr('disabled');
        //$("#districtList").multiSelect({ enable: "enable" });
    }
    else {
        // Edit Alert
        $('#districtList').attr('disabled', 'disabled');
        //$('#ms-districtList').find('#ms-list').attr('disabled', 'disabled');
        //$("#districtList").multiSelect({ disable: "disable" });
    }

    enableDisableGroupSection();
}

function loadDistrictList() {

    $('#processingDiv').show();
  //  $("#districtList").hide();
  
    $.ajax({
        url: "/Communication/AjaxGetDistrictList",
        type: "GET",
        dataType: "json", // The type of data we expect back
    })
      // Code to run if the request succeeds (is done). The response is passed to the function
      .done(function (json) {
          //debugger;
          $("#districtList").val('');
                   
          $("#districtList").addItems(json.aaData);
         
          // $("#districtList").show();
          setTimeout(function () {
              $('#districtList').select2();
              $('.SlectBox').SumoSelect({ placeholder: "Select District(s)", csvDispCount: 2, captionFormatAllSelected: '{0} All selected!', search: true, isClickAwayOk: true /*, selectAll: true*/, triggerChangeCombined: true });
                                      
              $('#processingDiv').hide();
            //  $("#districtList").show();
          }, 3000);
         
          //$('.SlectBox').sumo.reload();
         
        
      })
      // Code to run if the request fails; the raw request and status codes are passed to the function
      .fail(function (xhr, status, errorThrown) {
          $('#processingDiv').hide();
          console.log("Error: " + errorThrown);
          console.log("Status: " + status);
          console.dir(xhr);
      })
      // Code to run regardless of success or failure;
      .always(function (xhr, status) {
      });
}

function setParentAlertName(parentAlertId, parentAlertName) {
    parentAlert_Id = parentAlertId;
    $("#parentAlertNameH").html(parentAlertName);
}

function DeleteParentAlert() {
    $.ajax({
        type: "POST",
        url: "/Communication/AjaxDeleteParentAlert",
        data: JSON.stringify({
            parentAlerId: parentAlert_Id
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {
            if (json.Data == '1') {
                displaySuccessMessage("The parent alert has been deleted successfully.");
                //loadParentAlertGridData();
                oParentAlertGrid.draw();
            }
            else {
                alert(json.Data);
            }
        },
        error: function (request, status, error) {
            displayErrorMessage('Error in deleting parent alert data');
            return false;
        }
    });
}

$('#btnCalnedar').click(function (event) {
    event.preventDefault();
    $('#txtEndDate').click();
});

$('#btnPreview').click(function (event) {
    var alertText = $('#txtMessage').val();
    $("#alertTextspan").html(alertText)
});
//waqar
$('#chkMultipleAlerts').click(function (event) {
    //debugger;
    var state = $('#chkMultipleAlerts').is(":checked");

    if ($('#hdnAlertId').val() != '') {

        if (state)
            $('#hdnDistrictCheckbox').val("true");
        else
            $('#hdnDistrictCheckbox').val("false");
    }
    //Open popup window
    $('#districtListPopup').modal('toggle');
    //loadDistrictList();
   
});

$('#chkNoEndDate').click(function () {
    // assuming the textarea is inside <div class="controls /">
    if ($('#chkNoEndDate').is(':checked')) {
        $("#txtEndDate").attr("disabled", "disabled");
        $("#txtEndDate").val('');

        //Remove validation formatting.
        var icon = $("#txtEndDate").parent('.input-icon').children('i');
        icon.removeClass("fa-warning").removeClass("fa-check").removeAttr("data-original-title");
        $("#txtEndDate").closest('.form-group').removeClass('has-error').removeClass('has-success');
    }
    else {
        $("#txtEndDate").removeAttr("disabled");
    }
});

$('#chkNoEndDate').click(function () {
    // assuming the textarea is inside <div class="controls /">
    if ($('#chkNoEndDate').is(':checked')) {
        $("#txtEndDate").attr("disabled", "disabled");
        $("#txtEndDate").val('');

        //Remove validation formatting.
        var icon = $("#txtEndDate").parent('.input-icon').children('i');
        icon.removeClass("fa-warning").removeClass("fa-check").removeAttr("data-original-title");
        $("#txtEndDate").closest('.form-group').removeClass('has-error').removeClass('has-success');
    }
    else {
        $("#txtEndDate").removeAttr("disabled");
    }
});

function countChar(textAreaMessage) {
    var len = textAreaMessage.value.length;
    if (len > 300) {
        textAreaMessage.value = textAreaMessage.value.substring(0, 300);
    } else {
        $('#charNum').text(300 - len);
    }
};

$('.open-datetimepicker').click(function (event) {
    //event.preventDefault();
    $('#datetimepicker').click();
});

function okDistrictList() {
    //debugger;
    /*
    if ($('#ms-districtList').children('.ms-selection').find('.ms-selected').length > 0 ||
    $('#districtGroup').find('.active').length > 0) {
        $('#chkMultipleAlerts').attr('checked', true);
        $('#chkMultipleAlerts').parent().addClass('checked');
    }
    else {
        $('#chkMultipleAlerts').removeAttr('checked');
        $('#chkMultipleAlerts').parent().removeClass('checked');
    }
    if ($("#districtList").val() != null) {
        $('#txtDistrictList').val($("#districtList").val().toString());
    }
    */
    var isGroupSelection = $("input[type='radio']:checked");
    var selectedDistictCount = $("#districtList :selected").length;

    if (isGroupSelection.length > 0 || selectedDistictCount > 0) {
        $('#chkMultipleAlerts').attr('checked', true);
        $('#chkMultipleAlerts').parent().addClass('checked');
    }
    else {

        $('#chkMultipleAlerts').removeAttr('checked');
        $('#chkMultipleAlerts').parent().removeClass('checked');
    }

    if ($("#districtList").val() != null) {
        $('#txtDistrictList').val($("#districtList").val().toString());
    }
    else
    {
        $('#txtDistrictList').val('');
    }

    if (isGroupSelection.length > 0)
    {
        var txtid = $(isGroupSelection).attr('id');
        $('#txtDistrictGroupSelection').val(txtid);
    }
    else
    {
        $('#txtDistrictGroupSelection').val('');
    }
    

    $('#districtListPopup').modal('hide');
}

function closeDistrictList() {
    /*
    if ($('#districtList').val() != null) {
        $('#districtList').multiSelect('deselect', $('#districtList').val());
    }
    $('#txtDistrictList').val('');
    loadDistrictListUI();
    $('#districtList').multiSelect('deselect_all');
    */

    resetchkMultipleAlerts();


}






function ClearParentAlertUI() {
    $('.alert-danger').hide();

    $('hdnAlertId').val('');
    $('#alertPopupHeading').html('Create Alert');

    $('#txtTitle').val('');
    removeValidationFormatting($('#txtTitle'));

    $('#txtMessage').val('');
    removeValidationFormatting($('#txtMessage'));

    $('#txtStartDate').val('');
    removeValidationFormatting($('#txtStartDate'));

    $("#txtEndDate").removeAttr("disabled");
    $('#txtEndDate').val('');
    removeValidationFormatting($('#txtEndDate'));

    $("#chkNoEndDate").removeAttr("checked");
    $("#chkNoEndDate").parent().removeClass("checked");

    $("#chkActive").removeAttr("checked");
    $("#chkActive").parent().removeClass("checked");

    $("#chkMultipleAlerts").removeAttr("checked");
    $("#chkMultipleAlerts").parent().removeClass("checked");

   // $('#districtList').multiSelect('deselect_all');
    // $('#districtGroup').find('label').removeClass('active');
    $("input:radio").attr("checked", false);


}

function removeValidationFormatting(element) {
    //Remove validation formatting.
    var icon = element.parent('.input-icon').children('i');
    icon.removeClass("fa-warning").removeClass("fa-check").removeAttr("data-original-title");
    element.closest('.form-group').removeClass('has-error').removeClass('has-success');
}

$('#txtTitle').blur(function () {
    ValidateElement(this, 1, "Alert title is required.");
});
$('#txtMessage').blur(function () {
    ValidateElement(this, 1, "Message is required.");
});
$('#txtStartDate').change(function () {
    ValidateElement(this, 1, "Start Date is required.");
});
$('#txtEndDate').change(function () {
    ValidateElement(this, 1, "End Date is required.");
});

function ValidateElement(elem, minlen, message) {
    
    var element = $(elem);
    if (element.val().trim().length < minlen) {
        var icon = $(element).parent('.input-icon').children('i');
        icon.removeClass('fa-check').addClass("fa-warning");
        icon.attr("data-original-title", message).tooltip({ 'container': element.parent() });
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

function ValidateParentAlertData() {
    var errorbar = $('.alert-danger');

    var txtTitle = '#txtTitle';
    var txtMessage = '#txtMessage';
    var txtStartDate = '#txtStartDate';
    var txtEndDate = '#txtEndDate';

    var isValidTitle = ValidateElement(txtTitle, 1, "Title is a required field.");
    var isValidMessage = ValidateElement(txtMessage, 1, "Message is a required field.");
    var isValidStartDate = ValidateElement(txtStartDate, 1, "Start Date is required.");
    var isValidEndDate = $('#chkNoEndDate').parent().hasClass('checked') ? true : ValidateElement(txtEndDate, 1, "End Date is a required field.");

    if (isValidTitle && isValidMessage && isValidStartDate && isValidEndDate) {
        errorbar.hide();
        return true;
    }
    else {
        errorbar.show();
        return false;
    }
}

$('#btnSave').click(function () {
    if (!ValidateParentAlertData()) {
        //invalid data
        return;
    }
    else {
        // Save

        var dataString = GetDataString();

        $.ajax({
            type: "POST",
            url: "/Communication/AjaxCreateUpdateAlert",
            data: dataString,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //if (($('#hdnAlertId').val() == "" && data == -1) ||
                //    ($('#hdnAlertId').val() != "" && data == 1))
                //{
                displaySuccessMessage("The parent alert has been saved successfully.");
                $('#parentAlertPopup').modal('toggle'); // close modal
                oParentAlertGrid.draw();
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
    }
});

function GetDataString() {
    var title = document.getElementById("txtTitle").value;
    var message = document.getElementById("txtMessage").value;
    var startDate = document.getElementById("txtStartDate").value;
    var endDate = document.getElementById("txtEndDate").value;
    var active = document.getElementById('chkActive').checked;
    var alertId = document.getElementById("hdnAlertId").value;
    var chkNoEndDate = $("#chkNoEndDate");
    if (chkNoEndDate.parent().hasClass('checked') || chkNoEndDate.prop('checked')) {
        endDate = "";
    }
    //if (alertId != "")
    //    isUpdateRequest = true;

    //var AllDistricts = jQuerylatest('#AllDistricts').is(":checked");
    //var AllPreorderDistricts = jQuerylatest('#AllPreorderDistricts').is(":checked");
    //var AllShoppingCartDistricts = jQuerylatest('#AllShoppingCartDistricts').is(":checked");
    //var AllCreditCardDistricts = jQuerylatest('#AllCreditCardDistricts').is(":checked");
    //debugger;
    var DistrictGroup = null;

   // var selectedElemet = $('input:radio').find('checked');
    var selectedElemet = $("input[type=radio]:checked");

    if (selectedElemet.length > 0)
        //DistrictGroup = selectedElemet[0].children[0].getAttribute('id');
        DistrictGroup = selectedElemet.attr('id');
    var selectedDistrictsList = GetSelectedDistricts();

    var dataString = JSON.stringify({
        dataToUpload: title + "*" +
                            message + "*" +
                            startDate + "*" +
                            endDate + "*" +
                            active + "*" +
                            alertId + "*" +
                            selectedDistrictsList + "*" +
                            DistrictGroup
    });

    return dataString;
}

function GetSelectedDistricts() {
    //debugger;
    var defaultVal = '-1';
    var districtCount = $("#districtList option").length;
    var SelectedDistrictCount = $("#txtDistrictList").val().split(',').length;

    if (districtCount == 0) {
        return defaultVal;
    }
    else if (districtCount == SelectedDistrictCount) {
        return 'All';
    }
    else {

        var disList = $("#txtDistrictList").val() // jQuery.unique($('#txtDistrictList').val().split(','));
        if (disList == "") return defaultVal;
        return disList;
    }

    /*
    if ($('#ms-districtList').length == 0)
        return defaultVal;
  
    if ($('#ms-districtList').children('.ms-selectable').find('li').length ==
        $('#ms-districtList').children('.ms-selection').find('.ms-selected').length) {
        return 'all';
    }
    else {
        var ids = $('select#districtList').val()
        var items = $('#ms-districtList').children('.ms-selection').find('.ms-selected');
        if (ids != null && ids.length > 0) {
            var commaSeparatedIds = "";
            for (var i = 0; i < items.length; i++) {
                commaSeparatedIds += ids[i] + ",";
            }

            commaSeparatedIds = removeLastComma(commaSeparatedIds);
            return commaSeparatedIds;
        }
        else {
            return defaultVal;
        }
    }
    */
}

function removeLastComma(str) {
    return str.replace(/,$/, "");
}



$('#alertPreviewPopup').on('shown.bs.modal', function (e) {
    $('#btnClosePreview').addClass('defaultBtnClass');
    $('#btnSave').removeClass('defaultBtnClass');
});
$('#alertPreviewPopup').on('hidden.bs.modal', function () {
    $('#btnClosePreview').removeClass('defaultBtnClass');
    $('#btnSave').addClass('defaultBtnClass');
});

$('#districtListPopup').on('shown.bs.modal', function (e) {
    $('#btnOk').addClass('defaultBtnClass');
    $('#btnSave').removeClass('defaultBtnClass');
   // debugger;
    //Load Distict pop-up data
    // loadDistrictList();
    
    loadDistrictListUI();
  
    
});

$('#districtListPopup').on('hidden.bs.modal', function () {
    $('#btnOk').removeClass('defaultBtnClass');
    $('#btnSave').addClass('defaultBtnClass');
        //resetchkMultipleAlerts();
    //$('#districtList').multiSelect('deselect_all');

    
});


$('#parentAlertPopup').on('hidden.bs.modal', function () {
    $('#isForEdit').val('');

    $('#txtDistrictList').val('');
    $('#txtDistrictGroupSelection').val('');
   // debugger;
    ClearParentAlertUI();
    oParentAlertGrid.draw();

   

    $('#districtList').removeAttr('disabled');
    //  $("#districtList").multiSelect('destroy');

    
});

$('#parentAlertPopup').on('shown.bs.modal', function (e) {
    $("#txtTitle").focus();
    $('#btnSave').addClass('defaultBtnClass');
    $('#charNum').text(300);

    // loadDistrictList();
   
})

$('#parentAlertPopup').on('hidden.bs.modal', function () {
    $('#btnSave').removeClass('defaultBtnClass');
});

function chkMultipleAlertsChanged(chkBox) {
     //set value for district check box in text box
    if ($(chkBox).attr('checked') != undefined)
        $('#hdnDistrictCheckbox').val("true");
    else
        $('#hdnDistrictCheckbox').val("false");
}


function chkMultipleAlertsFocus(chkBox) {
    // set value for district check box in text box
    if ($('#hdnAlertId').val() != '') {
        if ($(chkBox).attr('checked') != undefined)
            $('#hdnDistrictCheckbox').val("true");
        else
            $('#hdnDistrictCheckbox').val("false");
    }
}

function resetchkMultipleAlerts()
{
    //if ($('#hdnAlertId').val() != '') {
        if ($('#hdnDistrictCheckbox').val() == "true") {
            $('#chkMultipleAlerts').attr('checked', true);
            $('#chkMultipleAlerts').parent().addClass('checked');
        }
        else {
            $('#chkMultipleAlerts').removeAttr('checked');
            $('#chkMultipleAlerts').parent().removeClass('checked');
        }
    //}
}