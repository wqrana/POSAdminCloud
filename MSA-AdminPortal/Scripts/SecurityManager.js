

function disableCreationRights(HiddednFielID, createButtonID, createAnchorButtonID, ClassNameTobeAdded, ClassNameTobeRemoved, warningMessage) {
    var CreateHiddenField = $("#" + HiddednFielID).val();
    
    if (CreateHiddenField == "False") {

        //create new school
      
        $("#" + createButtonID).addClass(ClassNameTobeAdded);
        $("#" + createAnchorButtonID).removeClass(ClassNameTobeRemoved);
        $("#" + createAnchorButtonID).attr("href", "#");
        $("#" + createAnchorButtonID).removeAttr("onclick");
        /*$("#" + createAnchorButtonID).click(function (e) {
            debugger;
            displayWarningMessage(warningMessage);
        });*/

        $('#sub-div').click(function (e) {
            displayWarningMessage(warningMessage);
        });
    }
}

function disableEditLinksTile(HiddednFielID, TargetClassName, ClassNameTobeRemoved, warningMessage) {
    var EditHiddenField = $("#" + HiddednFielID).val();
    if (EditHiddenField == "False") {
        $("." + TargetClassName).removeClass(ClassNameTobeRemoved);

        $("." + TargetClassName).attr("href", "#");
        $("." + TargetClassName).removeAttr("onclick");
        $("." + TargetClassName).click(function (e) {
            displayWarningMessage(warningMessage);
            e.preventDefault();
            return false;
        });
    }
}

function disableDeleteLinksTile(HiddednFielID, TargetClassName, warningMessage) {
    //debugger;
    var delHiddenField = $("#" + HiddednFielID).val();

    if (delHiddenField == "False") {
        //handle delete district
        $("." + TargetClassName).attr("href", "");
        $("." + TargetClassName).removeAttr("onclick");
        $("." + TargetClassName).click(function (e) {
            displayWarningMessage(warningMessage);
            e.preventDefault();
            return false;
        });
    }
}

function disableButton(HiddednFielID, ButtonID, ClassNameTobeAdded, ClassNameTobeRemoved, warningMessage) {
    var hiddenFieldVal = $("#" + HiddednFielID).val();
    //debugger;
    if (hiddenFieldVal == "False") {

        //create new school
        $("#" + ButtonID).addClass(ClassNameTobeAdded);
        $("#" + ButtonID).removeClass(ClassNameTobeRemoved);
        $("#" + ButtonID).removeAttr("onclick");
        $("#" + ButtonID).wrap('<a id="aAddNewButton" style="display:inline-block;" ></a>');

        $("#aAddNewButton").click(function (e) {
            displayWarningMessage(warningMessage);
        });
    }
}

function disableLink(HiddednFielID, TargetClassName, warningMessage) {
    var HiddenFieldVal = $("#" + HiddednFielID).val();

    if (HiddenFieldVal == "False") {
        //handle delete district
        $("." + TargetClassName).attr("href", "");
        $("." + TargetClassName).removeAttr("onclick");
        $("." + TargetClassName).click(function (e) {
            displayWarningMessage(warningMessage);
            e.preventDefault();
            return false;
        });
    }
}

// this function is for only preorder page
function removeCrossBtns(HiddednFielID) {
    var HiddenFieldVal = $("#" + HiddednFielID).val();

    if (HiddenFieldVal == "False") {
        $('.select2-choices').find('li.select2-search-choice').find('.select2-search-choice-close').remove();
        $('.multicss').select2("enable", false);
    }
}

function HideLinksSidebar() {
    //
    //debugger;
    var HiddenFeildLists = "viewDashboard, viewDistricts, viewSchools, viewPOS, viewUsers, viewUserRoles, viewCustomers, "+ 
                            "viewHomerooms, viewGrades, viewActivity, AllowNewPayments, AllowRefunding, AllowAccountAdjustments,  viewCategoryTypes, viewCategories, viewMenu, ViewCustomerReports, ViewFinancialReports, viewParent, viewCommunication";

    var hiddenFeildArrayList = HiddenFeildLists.split(',');
    for (var i = 0; i < hiddenFeildArrayList.length; i++) {
        var hdnItem = hiddenFeildArrayList[i].trim();
        var LinkName = "#" + hdnItem + "Link";
        var HiddenFieldval = $("#" + hdnItem).val();

        if (HiddenFieldval == "False") {
            if (hdnItem == "ViewCustomerReports") {
                LinkName = "#RGP_CUSTOMERS";
            }
            if (hdnItem == "ViewFinancialReports") {
                LinkName = "#RGP_FINANCIAL";
            }
            

            $(LinkName).css({ "display": "none" });

        }

        

    }
}