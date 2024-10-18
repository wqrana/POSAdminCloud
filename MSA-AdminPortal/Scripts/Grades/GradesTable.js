//Onload of Grades View
$(document).ready(function () {

    /* GENERATE DATATABLE HERE */
    ///////
    var oTable = $('#dtGrid').dataTable({
        "sDom": "<'row'<'col-md-6 col-sm-12'><'col-md-6 col-sm-12'><'col-md-6 col-sm-12'>r>t<'row'<'col-md-3 col-sm-12'i><'col-md-5 col-sm-12'l><'col-md-4 col-sm-12'p>>", //default layout without horizontal scroll(remove this setting to enable horizontal scroll for the table)
        "aLengthMenu": [
            [10, 25, 50, 100, -1],
            [10, 25, 50, 100, "All"] // change per page values here
        ],
        "bProcessing": true,
        "bServerSide": true,
        "aaSorting": [[1,'asc']],
        "bSortable": true,
        "sAjaxSource": "/Grade/AjaxHandler?rand=" + Math.random(),
        // set the initial value
        "iDisplayLength": 25,
        "sPaginationType": "bootstrap_full_number",
        "fnServerData": function (sSource, aoData, fnCallback) {

            $.getJSON(sSource, aoData, function (json) {
                fnCallback(json)
            });
        },

        "oLanguage": {
            "sProcessing": '<img src="/Images/ajax-loader.gif" />',
            "sLengthMenu": "_MENU_ records",
            "oPaginate": { "sPrevious": "Prev", "sNext": "Next" },
            "sInfo": 'Showing _START_ to _END_ of _TOTAL_ Grades.',
            "sInfoEmpty": 'No records.',
            "sEmptyTable": "No records found.",
        },
        "fnDrawCallback": function () {
            disableEditDeleteLinks();
        },

        "aoColumns": [
                    {
                        "bSearchable": false,
                        "bSortable": false,
                        "sClass": "center",
                        "sWidth": "8%",
                        "mRender" : function (data, type, row) {
                            var string = "<a title=\"Edit\" class=\"GradesEdit\" href=\"/Grade/Edit/" + row[0] + "\"><i class=\"fa fa-pencil-square-o fasize\"></i></a>";
                            string += " <span class=\"faseparator\"> | </span>  ";
                            string += "<a  title=\"Delete\" onclick=\"javascript:Delete(this);\" href=\"#Delete\" data-toggle=\"modal\" class=\"DeleteLink GradesDelete \" data-id=\"" + row[0] + "\"><i class=\"fa fa-trash fasize\"></i></a>";
                            return string;
                        }
                    },
                    {
                        "bSortable": true,
                    }

        ]
    });

    // modify table per page dropdown
    $('#dtGrid_wrapper .dataTables_length select').addClass("form-control input-small");
});