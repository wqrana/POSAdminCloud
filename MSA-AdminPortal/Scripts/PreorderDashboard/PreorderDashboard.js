$(document).ready(function () {
    $('#PeriodType').select2();

    $('#PeriodType').change(function () {
        debugger;
        loadTopSellingItemData();

    })


});

function loadTopSellingItemData() {
    debugger;
    $('#divTopPreordersLoading').show();
    $.ajax({
        url: '/PreorderDashboard/LoadTopSellingItemData',
        type: "GET",
        data: { "PeriodTypeID": $('#PeriodType').val() },
        error: function (response) {

        },
        success: function (response) {

            if (response != null) {

               var tbody = $("#LoadTSITable tBody");
               $(tbody).find('tr').remove();
               $(response).each(function () {

                   var tr = '<tr><td>' + this.ItemName + '</td>' + '<td>' + this.qtySold + '</td></tr>';

                   tbody.append(tr);
               });
               
               
                
            }
            $('#divTopPreordersLoading').hide();
        }
    });

}
