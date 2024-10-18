//Onload of Grades View
$(document).ready(function () {

    disableCreationRights("CreateGrades", "AddNewButton", "aAddNewButton", "disabled", "ActionLink", "You don’t have rights to create a Grade.");
    disableEditLinksTile("UpdateGrades", "EditSecurityClass", "ActionLink", "You don’t have rights to edit a Grade.");
    disableDeleteLinksTile("DeleteGrades", "DeleteSecurityClass", "You don’t have rights to delete a Grade.");

});


function disableEditDeleteLinks() {

    disableEditLinksTile("UpdateGrades", "GradesEdit", "ActionLink", "You don’t have rights to edit a Grade.");
    disableDeleteLinksTile("DeleteGrades", "GradesDelete", "You don’t have rights to delete a Grade.");

}