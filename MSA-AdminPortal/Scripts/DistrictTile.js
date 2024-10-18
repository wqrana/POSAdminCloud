
function CreateOrEdit(e) {

    var id = $(e).attr("data-id");

    window.location.href = "/settings/districts/" + id;
}

$(function () {

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
    
});


$(document).ready(function () {
    $('#dtGrid_wrapper .dataTables_length select').addClass("form-control input-small");

    $('#Delete').on('shown.bs.modal', function () {
       // $('#btnDelete').addClass('defaultBtnClass');
        //$('#btnSave').removeClass('defaultBtnClass');
    });
    $('#Delete').on('hidden.bs.modal', function () {
       // $('#btnDelete').removeClass('defaultBtnClass');
        //$('#btnSave').removeClass('defaultBtnClass');
    });
});

