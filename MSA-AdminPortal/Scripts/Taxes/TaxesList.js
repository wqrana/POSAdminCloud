$.getScript("/Content/themes/assets/global/plugins/jquery.formatCurrency-1.4.0/jquery.formatCurrency-1.4.0.min.js");

var oTable;
$(document).ready(function () {
    $('.multicss').select2();

    oTable = $('#taxesList').dataTable({
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
            //$('.dataTables_scrollBody').scrollTop(0);

            // $('body').scrollTop(0);
            window.scrollTo(0, 0);

            $('#taxesList tbody').find('div#AssignedSchoolDiv')
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

                    var string = "<a  title=\"Edit\" class=\"TaxEditTile\" onclick=\"javascript:CreateOrEdit(this);\" href=\"#basic_modalPopup\" data-toggle=\"modal\" data-backdrop=\"static\" data-keyboard=\"false\" class=\"DeleteLink\" data-id=\"" + id + "\"><i class=\"fa fa-pencil-square-o fasize\"></i></a>";
                    string += " <span class=\"faseparator\"> | </span> ";
                    string += " <a  title=\"Delete\" class=\"TaxDeleteTile\" onclick=\"javascript:Delete(this);\" href=\"#Delete\" data-toggle=\"modal\" data-backdrop=\"static\" data-keyboard=\"false\" class=\"DeleteLink\" data-id=\"" + id + "\"><i class=\"fa fa-trash fasize\"></i></a>";
                    string += " <span class=\"faseparator\">  ";

                    return string;
                }
            },
            {
                "bSortable": true,
                "sWidth": "25%",
                "sClass": "floattop",
            },
            {
                "bSortable": true,
                "sClass": "floattop",
                "sWidth": "15%",
                "mRender": function (data, type, row) {
                    return row[2] + '%';
                }
            },
            {
                "bSortable": false,
                "sClass": "floattop",
                "sWidth": "50%",
            }
        ]


    });

    /////////////
    $('select.multicss').change(function () {

        //debugger;
        var calID = this.id;
        var listID = '#' + calID;
        var SchoolsList = $(listID).val();
        if (SchoolsList == null) {
            SchoolsList = '';
        }

        var dataString = JSON.stringify({
            allData: calID + '*' + SchoolsList
        });

        $.ajax({
            type: "POST",
            url: "/taxes/updateSchoolsList",
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
    });


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

    BindTaxRateAmount();
});

var controller = '/taxes/';
var CreateAction = controller + 'CreateTax';
var EditAction = controller + 'EditTax';

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
                    $("#calNamelbl").html(model.Title);
                    clientdelId = model.DistrictID;
                    $("#TaxName").val($.trim(model.Name));
                    $("#TaxRate").val($.trim(model.TaxRate));
                    $("#taxID").val(model.Id)
                }
            },
            error: function () {
                displayWarningMessage(model.LoadErrorMessage);
            }
        });
    }

}

$(".savaForm").click(function () {


    var id = $("#taxID").val();
    var distID = $("#distID").val();
    var name = $("#TaxName").val();
    var taxRate = removePercent($("#TaxRate").val());

    var url = '';
    var create = id == '0';
    if (create) {
        url = CreateAction;
    }
    else {
        url = EditAction;
    }


    if (name.replace(/[^\w\s]/gi, '').length != name.length) {
        displayWarningMessage("Please enter a valid Tax name.");
        return false;
    }

    if (name.trim() == "") {
        displayWarningMessage("Please enter tax name.");
        return false;
    } else if (taxRate.trim() == "" /*|| taxRate.trim() == "0"*/) {
        displayWarningMessage("Please select tax rate.");
        return false;
    }
    else {

        $.ajax({
            type: "post",
            url: url,
            data: {
                "Id": id,
                "Name": name,
                "ClientId": distID,
                "TaxRate": taxRate
            },
            dataType: "json",
            success: function (data) {

                var model = data;

                if (model.IsError) {
                    displayWarningMessage(model.ErrorMessage);
                }
                else {

                        if (create) {

                            displaySuccessMessage('Tax created successfully.');
                            window.location.reload(false);

                        }
                        else {
                            displaySuccessMessage('Tax updated successfully.');
                            window.location.reload(false);
                        }
                    //}
                    //window.location.reload(true);
                    //oTable.fnDraw();
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
    disableCreationRights("CreateTaxes", "btnAddNewTax", "aAddNewCalendar", "disabled", "ActionLink", "You don’t have rights to create a tax.");

    disableEditLinksTile("UpdateTaxes", "TaxEditTile", "ActionLink", "You don’t have rights to edit a tax.");
    //removeCrossBtns("UpdatePreorderCalendars");


    disableDeleteLinksTile("DeleteTaxes", "TaxDeleteTile", "You don’t have rights to delete a tax.")
    //disableLink("ViewPreorderCalendars", "CalendarViewCSS", "You don’t have rights to view tax.");

    ///////////////////////////////////////////////////////////////////////////////////////////////

}
$('#basic_modalPopup').on('hidden.bs.modal', function () {
    // do something…
    //window.location.reload();
});

$('#basic_modalPopup').on('shown.bs.modal', function (e) {
    $('#btnSaveTax').addClass('defaultBtnClass');
})

$('#basic_modalPopup').on('hidden.bs.modal', function () {
    $('#btnSaveTax').removeClass('defaultBtnClass');
});

//$('#Delete').on('shown.bs.modal', function (e) {
//    $('#btnDelete').addClass('defaultBtnClass');
//})

//$('#Delete').on('hidden.bs.modal', function () {
//    $('#btnDelete').removeClass('defaultBtnClass');
//});

function setTwoNumberDecimal(a) {
    this.value = parseFloat(this.value).toFixed(2);
}


function BindTaxRateAmount() {
    $('input#TaxRate').blur(function () {
        var num = parseFloat(removePercent($(this).val()));
        if (num <= 100 && num!=0) {
            var cleanNum = num.toFixed(2);
            $(this).val(addPercent(cleanNum));
        }
        else {
            $(this).val(0);
        }
    });
}

function removePercent(value)
{
    return value.replace("%", "");
}

function addPercent(value) {
    return value + "%";
}