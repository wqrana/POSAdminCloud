var currentpage = 0;
var pageChnaged = false;
var isSortingAction = false;
var count = 0;
var ocustomerSearchTable;

function InitializeSearch() {

    $("#hidefilters").text("Show Filters");
    $('#filterDiv').slideUp();
    //$('.checkNum').slideUp();
    $("#SchoolFilter").select2();
    //$("#searchdll").select2();
    $("#GradeFilter").select2();
    //$("#adultdll").select2();
    $("#homeroomdll").select2();


    $("#hidefilters").click(function () {


        toggleText(this.id);
        /* commented by Waqar Q.
        ClearCustomerFilters();
        setTimeout(foo, 3000);
        oCustomerTable.fnDraw();
        */
    });
    var newUserPage = false;
    if ($('#newUser').length > 0) {
        if ($('#newUser').val() === 'true') {
            newUserPage = true;
        }
    }


    ocustomerSearchTable = $('#customerSearchTable').DataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-4 col-sm-12'i><'col-md-3 col-sm-12'l><'col-md-5 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100],
            [10, 25, 50, 100] // change per page values here
        ],
        "bProcessing": true,
        "bServerSide": true,
        //"bStateSave": true,
        //"fnStateSave": function (oSettings, oData) {
        //    localStorage.setItem('offersDataTables', JSON.stringify(oData));
        //},
        //"fnStateLoad": function (oSettings) {
        //    return JSON.parse(localStorage.getItem('offersDataTables'));
        //},
        "sAjaxSource": "/Customer/AjaxSearchHandler",
        // set the initial value
        //"iDisplayLength": 10,
        "sPaginationType": "bootstrap_full_number",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push(
                    { "name": "SearchBy", "value": $("#SearchStrCustPopUp").val() },
                    { "name": "SearchBy_Id", "value": $("#searchdll").val() },
                    { "name": "SearchTypedll", "value": ""/*$("#searchtypedll").val()*/ },
                    { "name": "newUser", "value": newUserPage },
                    { "name": "SchoolStr", "value": ($('#SchoolFilter').val()!=undefined && $('#SchoolFilter').val().trim() == '') ? '' : $('#SchoolFilter').select2('data').text },
                    { "name": "HomeRoomStr", "value": ($('#homeroomdll').val() == null || $('#homeroomdll').val().trim() == '-9999' || $('#homeroomdll').val().trim() == '') ? '' : $('#homeroomdll').select2('data').text },
                    { "name": "GradeStr", "value": $("#GradeFilter").val() }
                );
            $.getJSON(sSource, aoData, function (json) {
                // debugger;
                fnCallback(json);

            });
        },
        "fnDrawCallback": function () {
            restorecheckBoxes(this);
            jQuery("#SearchStrCustPopUp").focus();
            if (isSortingAction) {
                isSortingAction = false;
            }
            currentpage = this.fnPagingInfo().iPage;
        },


        "oLanguage": {
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Records',
            "sInfoEmpty": 'No records',
            "sEmptyTable": "No record found",
            "sProcessing": ' <img src="/Content/themes/assets/img/ajax-loading.gif" alt="Loading..." height="45" width="45">',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": {
                "sPrevious": "Prev",
                "sNext": "Next"
            }
        },
        "aoColumns": [
                    {
                        "sName": "id",
                        "sWidth": "5%",
                        "sClass": "centerClass",
                        "bSortable": false,
                        "sClass": "centerClass",
                        "mRender": function (data, type, row) {
                            var firstName = resolveName(row[3]);
                            var lastName = resolveName(row[2]);
                            return "<input type=\"checkbox\" id=\"" + row[0] + "\" onclick=\"ChildClick(this," + row[0] + ",\'" + row[1] + "\',\'" + firstName + "\',\'" + lastName + "\',\'" + row[9] + "\',\'" + row[10] + "\',\'" + row[11] + "\')\" name=\"" + row[0] + "\" />";
                        }
                    },

                    {
                        "sName": "UserID",
                        "sWidth": "9%",
                        "bSearchable": false,
                        "bSortable": true,
                    },
                    {
                        "sName": "Last_Name",
                        "sWidth": "13%"
                    },
                    {
                        "sName": "First_Name",
                        "sWidth": "13%"
                    },
                    {
                        "sName": "Middle_Initial",
                        "sWidth": "10%",
                    },
                    {
                        "sName": "Adult",
                        "sWidth": "7%",
                        "sClass": "centerClass",
                        "mRender": function (data, type, row) {
                            if (row[5] == 'True') return "<i class=\"fa fa-check\" style=\"margin-left:0px;\"></i>"; else return "";
                        }

                    },
                    {
                        "sName": "Grade",
                        "sWidth": "12%",
                    },
                    { "sName": "Homeroom", "sWidth": "12%", },
                    { "sName": "School_Name", "sWidth": "19%" }


        ]
    })

    $('#customerSearchTable_wrapper .dataTables_filter').hide(); //.addClass("form-control input-medium"); // modify table search input
    $('#customerSearchTable_wrapper .dataTables_length select').addClass("form-control input-small"); // modify table per page dropdown


    $('#CustomerSearcRpt').on('shown.bs.modal', function () {
        $('#btnSave').removeClass('defaultBtnClass');
        SearchCustomer();
    });
    $('#CustomerSearcRpt').on('hidden.bs.modal', function () {

        var noRecordsRow = "<tr><td colspan=\"9\" style=\"text-align: center;\">No record found</td></tr>";

        $('#customerSearchTable tbody').empty();
        $('#customerSearchTable_info').html('No Records');
        $("#customerSearchTable tbody").append(noRecordsRow);

        $('#searchdll').val(0);
        $('#SearchStrCustPopUp').val('');
        $('#btnSave').addClass('defaultBtnClass');

        SelectedValues = document.getElementById('hdnFldSelectedValues');
        if (SelectedValues.value != '') {
            //debugger;
            $('label[for=specificCust]').html('Specific Customers <span class="fa fa-check-square fa-1"></span>');
            $('#specificCustomers').prop('checked', true);
            $('#allCustomers').prop('checked', false);
        }
        else {
            //debugger;
            $('label[for=specificCust]').html('Specific Customers');
            $('#uniform-allCustomers span').prop('class', 'checked');
            $('#allCustomers').prop('checked', true);


            $('#uniform-specificCustomers span').prop('class', '');
            $('#specificCustomers').prop('checked', false);

        }
    });

    $("#SearchBtnCustPopUp").click(function (e) {
        SearchCustomer();
    });

    $('#SearchStrCustPopUp').keyup(function (e) {
        if (e.keyCode === 13) {
            SearchCustomer();
        }
    });

    jQuery("#SearchStrCustPopUp").focus();

    function resolveName(name) {
        return name.replace("'", "\\'");
    }

    function SearchCustomer() {
        if ($('#searchdll').val() == 0)//LN,FN and Userid=1,2 and 3
            ocustomerSearchTable.columns([1, 2, 3]).search($('#SearchStrCustPopUp').val()).draw();

        if ($('#searchdll').val() == 1)//last name =2
            ocustomerSearchTable.columns([2]).search($('#SearchStrCustPopUp').val()).draw();

        if ($('#searchdll').val() == 2)//first name=3
            ocustomerSearchTable.columns([3]).search($('#SearchStrCustPopUp').val()).draw();

        if ($('#searchdll').val() == 3)//user id=1
            ocustomerSearchTable.columns([1]).search($('#SearchStrCustPopUp').val()).draw();

        if ($('#searchdll').val() == 4)//grade=6
            ocustomerSearchTable.columns([6]).search($('#SearchStrCustPopUp').val()).draw();

        if ($('#searchdll').val() == 5)//homeroom=7
            ocustomerSearchTable.columns([7]).search($('#SearchStrCustPopUp').val()).draw();
    }
}

    var TargetBaseControl = null;
    //Total no of checkboxes in a particular column inside the GridView.
    var CheckBoxes;
    //Total no of checked checkboxes in a particular column inside the GridView.
    var CheckedCheckBoxes;
    //Array of selected item's Ids.
    var SelectedItems = new Array();
    //Hidden field that wil contain string of selected item's Ids separated by '|'.
    var SelectedValues;

    function restorecheckBoxes(obj) {
        //debugger;
        //Get hidden field that wil contain string of selected item's Ids separated by '|'.
        SelectedValues = document.getElementById('hdnFldSelectedValues');
        //Get an array of selected item's Ids.
        var SingleSelectValue = document.getElementById('SingleSelect').value;
        if (SelectedValues.value == '') {
            if (SingleSelectValue == "1" || SingleSelectValue == "2" || SingleSelectValue == "3") {
                SelectedItems = new Array();
            }
        }
        else
            if (SingleSelectValue == "3") {
                //debugger;
                //district page check
                var adminOrDirector = document.getElementById('adminOrDirector').value;
                if (adminOrDirector == "admin") {
                    var adminID = document.getElementById('hdadminID').value;
                    SelectedItems = new Array();
                    SelectedItems.push(adminID);
                }
                else if (adminOrDirector == "Director") {
                    var directorID = document.getElementById('hddirectorID').value;
                    SelectedItems = new Array();
                    SelectedItems.push(directorID);
                }


            } else {
                SelectedItems = SelectedValues.value.split('|');
            }

        RestoreState(obj);
        //code commented to fix PA-251
        //if (document.getElementById('SingleSelect').value == "1") {
        //    document.getElementById('CustomerId').value = 0;
        //    //document.getElementById('CustomerName').innerHTML = "";
        //}
        //if (document.getElementById('SingleSelect').value == "2") {
        //    document.getElementById('CustomerId').value = 0;
        //    //document.getElementById('UserNameEdit').value = "";
        //}
    }

    function ChildClick(CheckBox, Id, userid, fname, lname, amount, abalance, mbalance) {
        //debugger;
        //Modifiy Counter;
        var singleSelectVal = document.getElementById('SingleSelect').value;
        var pageName = $("#PageName").val();
        if (pageName === 'refund') {
            if (parseFloat(amount) <= 0) {
                $(CheckBox).prop('checked', false);
                displayInfoMessage('Cannot apply refund to customer having zero or Negative balance.');
                return;
            }
        }
        $("#MBalance").html("$" + mbalance);
        $("#ABalance").html("$" + abalance);
        $("#UserID").html(userid);

        if (document.getElementById('SingleSelect').value == "1" || document.getElementById('SingleSelect').value == "2") {
            document.getElementById('CustomerId').value = Id;
            document.getElementById('CustomerName').innerHTML = fname + " " + lname;
            $("#tableDiv").slideDown();
            //debugger;
            if ($('#UserNameEdit').length) {
                $('#UserNameEdit').val(fname + " " + lname);
            }
            SelectedItems = new Array();
            SelectedItems.push(Id);
            $('#hdnFldSelectedValues').val(SelectedItems);
            $('#CustomerSearcRpt').modal('hide');
        }
        else if (singleSelectVal == "3") {
            //district page
            //debugger;
            var adminOrDirectorVal = document.getElementById('adminOrDirector').value;

            if (adminOrDirectorVal == "admin") {
                SelectedItems = new Array();
                SelectedItems.push(Id);

                $('#hdadminID').val(Id);
                $('#hdnFldSelectedValues').val(SelectedItems);
                $('#CustomerSearcRpt').modal('hide');
                $('#adminName').val(fname + " " + lname);

            } else if (adminOrDirectorVal == "Director") {
                SelectedItems = new Array();
                SelectedItems.push(Id);

                $('#hddirectorID').val(Id);
                $('#hdnFldSelectedValues').val(SelectedItems);
                $('#CustomerSearcRpt').modal('hide');
                $('#directorName').val(fname + " " + lname);
            }

        }
            //if (document.getElementById('SingleSelect').value == "2") {
            //    document.getElementById('CustomerId').value = Id;
            //    document.getElementById('UserNameEdit').value = fname + " " + lname;
            //    $('#CustomerSearcRpt').modal('hide');
            //}
        else {
            if (CheckBox.checked && CheckedCheckBoxes < CheckBoxes)
                CheckedCheckBoxes++;
            else if (CheckedCheckBoxes > 0)
                CheckedCheckBoxes--;
            //Modify selected items array.
            if (CheckBox.checked)
                SelectedItems.push(Id);
            else
                DeleteItem(Id);
            //Update Selected Values. 
            $('hdnFldSelectedValues').val(SelectedItems);
            SelectedValues.value = SelectedItems.join('|');
        }
    }
    function RestoreState(obj) {
        //debugger;
        //Get all the control of the type INPUT.
        var Inputs = $('input', obj.fnGetNodes());

        //Restore previous state of the all checkBoxes in side the GridView.
        for (var n = 0; n < Inputs.length; ++n)
            if (Inputs[n].type == 'checkbox' && IsItemExists(Inputs[n].id) > -1) {
                Inputs[n].checked = true;
                CheckedCheckBoxes++;
            }
            else
                Inputs[n].checked = false;
    }

    function DeleteItem(Text) {
        var n = IsItemExists(Text);
        if (n > -1)
            SelectedItems.splice(n, 1);
    }

    function IsItemExists(Text) {
        //debugger;
        for (var n = 0; n < SelectedItems.length; ++n)
            if (SelectedItems[n] == Text)
                return n;

        return -1;
    }


    //jQuery.fn.dataTableExt.oApi.fnStandingRedraw = function (oSettings) {
    //    if (oSettings.oFeatures.bServerSide === false) {
    //        var before = oSettings._iDisplayStart;
    //        oSettings.oApi._fnReDraw(oSettings);
    //        // iDisplayStart has been reset to zero - so lets change it back        
    //        oSettings._iDisplayStart = before;
    //        oSettings.oApi._fnCalculateEnd(oSettings);
    //    }
    //    // draw the 'current' page    
    //    oSettings.oApi._fnDraw(oSettings);
    //};

    //$.fn.dataTableExt.oApi.fnPagingInfo = function (oSettings) {
    //    return {
    //        "iStart": oSettings._iDisplayStart,
    //        "iEnd": oSettings.fnDisplayEnd(),
    //        "iLength": oSettings._iDisplayLength,
    //        "iTotal": oSettings.fnRecordsTotal(),
    //        "iFilteredTotal": oSettings.fnRecordsDisplay(),
    //        "iPage": Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
    //        "iTotalPages": Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
    //    };
    //};

    function HideFilterDiv() {

        $('#filterDiv').slideUp();
        var el = document.getElementById('hidefilters');
        el.firstChild.data = "Show Filters";
        return false;
    }

    function ShowFilterDiv() {

        $('#filterDiv').slideDown();
        var el = document.getElementById('hidefilters');
        el.firstChild.data = "Hide Filters";
        return false;
    }

    function toggleText(button_id) {
        var el = document.getElementById(button_id);
        if (el.firstChild.data == "Hide Filters") {
            el.firstChild.data = "Show Filters";
            HideFilterDiv();

        }
        else {
            el.firstChild.data = "Hide Filters";
            ShowFilterDiv();
        }
    }

    $("#applyFilterBtn").click(function (e) {
        //ApplyCustomerFilters();
        //setTimeout(foo, 3000);

        //Save filters in session;
        // saveFilters();
        ocustomerSearchTable.draw();
    });

    $("#Clearfilters").click(function (e) {
        ClearCustomerFilters();
        setTimeout(foo, 3000);
        ocustomerSearchTable.draw();
    });


    function ClearCustomerFilters() {
        $("#SearchStrCustPopUp").val("");
        $("#searchdll").val("0");
        $("#searchdll").select2("val", "0");

        $("#homeroomdll").select2("val", "-9999");
        //var theSpan = $("#s2id_searchdll").find("span").first();
        //theSpan.text("Search By...");
        $("#SchoolFilter").val("");
        $("#SchoolFilter").select2();

        $("#GradeFilter").val("");
        $("#GradeFilter").select2();

        //$("#adultdll").val("");
        //$("#adultdll").select2();

        //$("#activedll").val("");
        //$("#activedll").select2("val", "", "placeholder", "Both");

        bindHomeroomDll(0);

    }

    function bindHomeroomDll(schoolId) {

        var jsonSchoolID = JSON.stringify({ SchoolId: schoolId });
        $('#homeroomdll').select2("enable", false);

        $.ajax({
            type: "POST",
            url: "/Customer/getHomeRoomsList",
            data: jsonSchoolID,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                $('#homeroomdll').select2("enable", true);
                if (data.result != "-1") {

                    $('#homeroomdll').get(0).options.length = 0;
                    $("#homeroomdll").get(0).options[$("#homeroomdll").get(0).options.length] = new Option("", "-9999");
                    $("#homeroomdll").select2("val", "-9999");
                    $.each(data.result, function (index, item) {
                        $("#homeroomdll").get(0).options[$("#homeroomdll").get(0).options.length] = new Option(item.Text, item.Value);
                    });
                } else {
                    $('#homeroomdll').get(0).options.length = 0;
                    $("#homeroomdll").get(0).options[$("#homeroomdll").get(0).options.length] = new Option("", "-9999");
                    $("#homeroomdll").select2("val", "-9999");
                }

                if (filterObj) {

                    $("#homeroomdll").select2("val", filterObj.homeroomdll);
                    filterObj = null;
                }
            },
            error: function (request, status, error) {
                //displayErrorMessage("Error occurred during saving the data.");
                //return false;
            }
        });
    }

    function foo() {
        //some delay
    }