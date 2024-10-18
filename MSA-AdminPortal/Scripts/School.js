var controller = '/School/';
var CreateAction = controller + 'Create';
var EditAction = controller + 'Edit';

function CreateNewSchool() {
    window.location.href = '/School/Create';
}

function deleteSchool() {
    
}

$(document).ready(function () {
   
    //Security handler
    disableCreationRights("CreateSchools", "btnAddNewSchool", "aAddNewSchool", "disabled", "ActionLink", "You don’t have rights to create a school.");
    disableEditLinksTile("UpdateSchools", "SchoolEditTile", "ActionLink", "You don’t have rights to edit a School.");
    disableDeleteLinksTile("DeleteSchools", "SchoolDeleteTile", "You don’t have rights to delete a School.");

    $('#Delete').on('shown.bs.modal', function () {
      //  $('#btnDelete').addClass('defaultBtnClass');
     //   $('#btnSave').removeClass('defaultBtnClass');
    });
    $('#Delete').on('hidden.bs.modal', function () {
      //  $('#btnDelete').removeClass('defaultBtnClass');
      //  $('#btnSave').removeClass('defaultBtnClass');
    });
});

function disableEditDeleteLinks()
{
    disableEditLinksTile("UpdateSchools", "SchoolEditTile", "ActionLink", "You don’t have rights to edit a School.");
    disableDeleteLinksTile("DeleteSchools", "SchoolDeleteTile", "You don’t have rights to delete a School.")
}