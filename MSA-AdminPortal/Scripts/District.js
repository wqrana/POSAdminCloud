
function CreateOrEdit(e) {

    var id = $(e).attr("data-id");
  
    window.location.href = "/settings/districts/" + id;

}

function SchoolByDistrict(e) {
    alert(e);
    var id = $(e).attr("data-id");
    alert(id);
}

$(document).ready(function () {

    ///////////////////////////////////////////////////////////////////////////////////////////////
    //Security handler
    ///////////////////////////////////////////////////////////////////////////////////////////////
    disableCreationRights("CreateDistricts", "btnAddNewDistrict", "aAddNewDistrict", "disabled", "ActionLink", "You don’t have rights to create a district.");

    disableEditLinksTile("UpdateDistricts", "DistrictEditTile", "ActionLink", "You don’t have rights to edit a district.");

    disableDeleteLinksTile("DeleteDistricts", "DistrictDeleteTile", "You don’t have rights to delete a district.")
    ///////////////////////////////////////////////////////////////////////////////////////////////

    // Document.ready -> link up remove event handler
    $(".ActionLink").click(function () {

        CreateOrEdit(this);

    });

    ///////
    var oTable = $('#dtGrid').dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-4 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100, -1],
            [10, 25, 50, 100, "All"] // change per page values here
        ],
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": "/District/AjaxHandler",
        // set the initial value
        "iDisplayLength": 25,
        "sPaginationType": "bootstrap_full_number",
        "fnDrawCallback": function (oSettings) {
            disableEditDeleteLinks();
        },
        //"fnServerData": function (sSource, aoData, fnCallback) {

        //    $.getJSON(sSource, aoData, function (json) {
        //        fnCallback(json);
        //        window.scrollTo(0, 0);
        //    });
        //},

        "oLanguage": {
            //"sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": { "sPrevious": "Prev", "sNext": "Next" },
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Districts.',
            "sInfoEmpty": 'No records.',
            "sEmptyTable": "No records found.",
        },
        "aoColumns": [
            {
                "bSortable": false,
                "sWidth": "8%",
                "sClass": "center",
                "mRender": function (data, type, row) {
                    
                    return '<a onclick="javascript:CreateOrEdit(this);" href="#CreateOrEdit" data-toggle="modal" data-backdrop="static" data-keyboard="false" title="Edit" class="ActionLink DistrictEdit" data-id="' + row[0] + '"><i class="fa fa-pencil-square-o fasize"></i></a>' +
                        ' <span class="faseparator"> | </span> <a onclick="javascript:Delete(this);" href="#Delete" data-toggle="modal" data-backdrop="static" data-keyboard="false"  class="DeleteLink DistrictDelete" title="Delete" data-id="' + row[1] + '"><i class="fa fa-trash fasize"></i></a>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "24%",
                "bVisible": false,
                "mRender": function (data, type, row) {
                    return '<div>' + row[2] + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "24%",
                "mRender": function (data, type, row) {
                    return '<div>' + row[2] + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "11%",
                "mRender": function (data, type, row) {
                    return '<div>' + row[3] + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "10%",
                "mRender" : function (data, type, row) {
                    return '<div>' + row[4] + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "9%",
                "mRender" : function (data, type, row) {
                    return '<div>' + row[5] + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "8%",
                "mRender" : function (data, type, row) {
                    return '<div>' + row[6] + '</div>';
                }
            },
            {
                "bSortable": true,
                "sWidth": "13%",
                "mRender" : function (data, type, row) {
                    if (row[7] == "0") {
                        return '<div class="zeroCount">' + row[7] + '</div>';
                    } else {
                        return '<div>' + row[7] + '</div>';
                    }
                }
            }

        ]
    });

    // modify table per page dropdown
    $('#dtGrid_wrapper .dataTables_length select').addClass("form-control input-small");

    $('#Delete').on('shown.bs.modal', function () {
        //$('#btnDelete').addClass('defaultBtnClass');
        //$('#btnSave').removeClass('defaultBtnClass');
    });
    $('#Delete').on('hidden.bs.modal', function () {
      //  $('#btnDelete').removeClass('defaultBtnClass');
        //$('#btnSave').removeClass('defaultBtnClass');
    });

});

function disableEditDeleteLinks() {

    disableEditLinksTile("UpdateDistricts", "DistrictEdit", "ActionLink", "You don’t have rights to edit a district.");

    disableDeleteLinksTile("DeleteDistricts", "DistrictDelete", "You don’t have rights to delete a district.")
}

$("Schools").click(function () {

    SchoolByDistrict(this);

});