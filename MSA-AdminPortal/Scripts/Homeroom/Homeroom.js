
$(document).ready(function () {

    disableCreationRights("CreateHomerooms", "AddNewButton", "aAddNewButton", "disabled", "ActionLink", "You don’t have rights to create a Homeroom.");
    disableEditLinksTile("UpdateHomerooms", "EditSecurityClass", "ActionLink", "You don’t have rights to edit a Homeroom.");
    disableDeleteLinksTile("DeleteHomerooms", "DeleteSecurityClass", "You don’t have rights to delete a Homeroom.");
});

function disableEditDeleteLinks() {

    disableEditLinksTile("UpdateHomerooms", "HomeRoomEdit", "ActionLink", "You don’t have rights to edit a Homeroom.");
    disableDeleteLinksTile("DeleteHomerooms", "HomeRoomDelete", "You don’t have rights to delete a Homeroom.");

}