


$(document).ready(function () {

    //old code
    //setTimeout(oldCode(), 0);

    //new optimized code
    setTimeout(combineResult(), 0);

});

function oldCode() {
    var urlPayments = "/Home/GetPayments";
    var urlSales = "/Home/GetSales";

    setTimeout(loadGraphData(urlPayments, "divMSAPaymentChart", "divMSAPaymentChartLoading", "divMSAPaymentChartContent", "#8dc63f"), 0);
    setTimeout(loadGraphData(urlSales, "divMSASalesChart", "divMSASalesChartLoading", "divMSASalesChartContent", "#9ACAE6"), 0);
    // showTotalSales(); This function shows Total Sales, but its commented on Neils Request
    setTimeout(showAcountInfo(), 0);
    setTimeout(showCashierSessions(), 0);
    setTimeout(showParticipationPercentage(), 0);
}


function combineResult() {
    var timestart;
    var urlTotalSales = "/Home/CombineResult";
    $.ajax({
        url: urlTotalSales,
        type: "GET",
        data: { "viewType": "Index" },
        beforeSend: function () { console.log('start combineResult', new Date()); timestart = new Date(); },
        error: function (response) {

        },
        success: function (response) {

            setTimeout(popGraphData("divMSAPaymentChart", "divMSAPaymentChartLoading", "divMSAPaymentChartContent", "#8dc63f", response.Payments), 0);
            setTimeout(popGraphData("divMSASalesChart", "divMSASalesChartLoading", "divMSASalesChartContent", "#9ACAE6", response.Sales), 0);
            // showTotalSales(); This function shows Total Sales, but its commented on Neils Request
            setTimeout(popAcountInfo(response.AccountInfo), 0);
            setTimeout(popCashierSessions(response.CashierSession), 0);
            setTimeout(popParticipationPercentage(response.ParticipationPercentage), 0);

            var end = new Date();
            console.log('end combineResult', end);
            console.log("combineResult", (end - timestart) / 1000);

        }
    });
}


function popGraphData(DivChartID, DivChartLoaddingID, DivChartContentID, ChartColor, data) {
    var res = getPaymentGraphData(data);

    loadDashboardChart(res, DivChartID, DivChartLoaddingID, DivChartContentID, ChartColor);
}

function popAcountInfo(dataresponse) {
    $("#divAccountInfoLoading").hide();
    var data = [];

    var dataresponse = JSON.parse(dataresponse);

    var totalAccounts = getTotalAccountsCombine(dataresponse);

    var i = 0;
    for (var key in dataresponse) {

        if (key == "CountOfNegativeAccounts" || key == "CountOfPositiveAccounts" || key == "CountOfZeroAccounts") {
            var chartLabel;

            var percentage = 0;

            if (key == "CountOfNegativeAccounts") {
                var negativeAmount = Math.round(Math.abs(dataresponse["NegativeAmount"]));
                percentage = calcultePercentage(totalAccounts, dataresponse["CountOfNegativeAccounts"]);
                chartLabel = "<b>" + percentage.toString() + "</b><br /> Negative Amount: $" + negativeAmount.toString() + "<br /> Negative Accounts: " + dataresponse["CountOfNegativeAccounts"] + "";

            }

            if (key == "CountOfPositiveAccounts") {
                var positiveAmount = Math.round(dataresponse["PositiveAmount"]);
                percentage = calcultePercentage(totalAccounts, dataresponse["CountOfPositiveAccounts"]);
                chartLabel = "<b>" + percentage.toString() + "</b><br /> Positive Amount: $" + positiveAmount.toString() + "<br /> Positive Accounts: " + dataresponse["CountOfPositiveAccounts"] + "";
            }

            if (key == "CountOfZeroAccounts") {
                percentage = calcultePercentage(totalAccounts, dataresponse["CountOfZeroAccounts"]);
                chartLabel = "<b>" + percentage.toString() + "</b><br /> Zero Accounts: " + dataresponse["CountOfZeroAccounts"] + "";
            }
            var sliceColor = pickColorCombine(key);


            data[i] = {
                label: chartLabel,
                data: Math.round(Math.abs(dataresponse[key])),
                color: sliceColor

            }

            i = i + 1;
        }
    }


    $.plot($("#divAccountInfoPieChart"), data, {
        series: {
            pie: {
                show: true
            }
        }
    });
}

function popCashierSessions(data) {
    $("#divCashierSessionsLoading").hide();

    var response = JSON.parse(data);

    var tblCashier = '<table class="table table-hover"><thead><tr><th>POS</th><th>User</th><th>Open Date</th></tr></thead>';

    for (var key in response) {

        var openDate = moment(response[key]["POS_Open_Session_Date"]).format('MM/DD/YY')

        tblCashier += '<tbody><tr><td>' + response[key]["POS_Name"] + '</td><td>' + response[key]["POS_Open_Cashier"] + '</td><td>' + openDate + '</td></tr></tbody>';
    }

    tblCashier += '</table>';

    $("#divCashier").html(tblCashier);
}

function popParticipationPercentage(data) {
    var data = JSON.parse(data);
    $("#divParticipationPercentageLoading").hide();
    var yesterdayHTML = '<i>%</i>' + Math.round(data.YesterdayParticipation) + '<span>Yesterday</span>';
    var todayHTML = '<i>%</i>' + Math.round(data.TodayParticipation) + '<span>Today</span>'
    var lastWeekHTML = '<i>%</i>' + Math.round(data.LastWeekParticipation) + '<span>Last Week</span>'

    $("#divYesterdayParticipation").html(yesterdayHTML);
    $("#divTodayParticipation").html(todayHTML);
    $("#divLastWeekParticipation").html(lastWeekHTML);
}

function getTotalAccountsCombine(response) {
    var totalAccounts = 0;
    for (var key in response) {

        if (key == "CountOfNegativeAccounts" || key == "CountOfPositiveAccounts" || key == "CountOfZeroAccounts") {

            totalAccounts = totalAccounts + response[key];
        }

    }

    return Math.round(totalAccounts);
}

function pickColorCombine(AccountKey) {

    var color = '';

    if (AccountKey == "CountOfNegativeAccounts") {
        color = ' #ed1d34'; //'#A23C3C'; //red
    }
    else if (AccountKey == "CountOfPositiveAccounts") {
        color = ' #8dc63f'; //'#3D853D'; //green
    }
    else if (AccountKey == "CountOfZeroAccounts") {
        color = '#989898'; // Grey //'#7633BD' //purple
    }

    return color;
}

// This function is used to get Total Sales and show sales on dashboard
function showTotalSales() {


    var urlTotalSales = "/Home/GetTotalSales";
    $.ajax({
        url: urlTotalSales,
        type: "GET",
        data: { "viewType": "Index" },
        error: function (response) {

        },
        success: function (response) {

            $("#divTotalSalesLoading").hide();
            var yesterdayHTML = '<i>$</i>' + response.yesterdatSales + '<span>Yesterday</span>';
            var todayHTML = '<i>$</i>' + response.todaySales + '<span>Today</span>'
            var lastWeekHTML = '<i>$</i>' + response.lastWeekSales + '<span>Last Week</span>'

            $("#divYesterdaySales").html(yesterdayHTML);
            $("#divTodaySales").html(todayHTML);
            $("#divLastWeekSales").html(lastWeekHTML);

        }
    });
}
// This function is used to get Participation Percentage and show Participation Percentage on dashboard
function showParticipationPercentage() {
    var urlTotalSales = "/Home/GetParticipationPercentage";
    var timestart;
    $.ajax({
        url: urlTotalSales,
        type: "GET",
        data: { "viewType": "Index" },
        beforeSend: function () { console.log('start showParticipationPercentage', new Date()); timestart = new Date(); },
        error: function (response) {
            //  debugger;
        },
        success: function (response) {
            //   debugger;

            $("#divParticipationPercentageLoading").hide();
            var yesterdayHTML = '<i>%</i>' + Math.round(response.yesterdayParticipation) + '<span>Yesterday</span>';
            var todayHTML = '<i>%</i>' + Math.round(response.todayParticipation) + '<span>Today</span>'
            var lastWeekHTML = '<i>%</i>' + Math.round(response.lastWeekParticipation) + '<span>Last Week</span>'

            $("#divYesterdayParticipation").html(yesterdayHTML);
            $("#divTodayParticipation").html(todayHTML);
            $("#divLastWeekParticipation").html(lastWeekHTML);
            var end = new Date();
            console.log('end showParticipationPercentage', end);
            console.log("showParticipationPercentage", (end - timestart) / 1000);
        }
    });
}
// This function is used to get Account information and show data in form of PIE chart on dashboard
function showAcountInfo() {
    var urlAccountInfo = "/Home/GetAccountInfo";
    var timestart;
    $.ajax({
        url: urlAccountInfo,
        type: "GET",
        data: { "viewType": "Index" },
        beforeSend: function () { console.log("start showAcountInfo", new Date()); timestart = new Date(); },
        error: function (response) {

        },
        success: function (response) {

            $("#divAccountInfoLoading").hide();
            var data = [];

            var totalAccounts = getTotalAccounts(response);

            var i = 0;
            for (var key in response) {

                if (key == "countOfNegativeAccounts" || key == "countOfPositiveAccounts" || key == "countOfZeroAccounts") {
                    var chartLabel;

                    var percentage = 0;

                    if (key == "countOfNegativeAccounts") {
                        var negativeAmount = Math.round(Math.abs(response["negativeAmount"]));
                        percentage = calcultePercentage(totalAccounts, response["countOfNegativeAccounts"]);
                        chartLabel = "<b>" + percentage.toString() + "</b><br /> Negative Amount: $" + negativeAmount.toString() + "<br /> Negative Accounts: " + response["countOfNegativeAccounts"] + "";

                    }

                    if (key == "countOfPositiveAccounts") {
                        var positiveAmount = Math.round(response["positiveAmount"]);
                        percentage = calcultePercentage(totalAccounts, response["countOfPositiveAccounts"]);
                        chartLabel = "<b>" + percentage.toString() + "</b><br /> Positive Amount: $" + positiveAmount.toString() + "<br /> Positive Accounts: " + response["countOfPositiveAccounts"] + "";
                    }

                    if (key == "countOfZeroAccounts") {
                        percentage = calcultePercentage(totalAccounts, response["countOfZeroAccounts"]);
                        chartLabel = "<b>" + percentage.toString() + "</b><br /> Zero Accounts: " + response["countOfZeroAccounts"] + "";
                    }
                    var sliceColor = pickColor(key);


                    data[i] = {
                        label: chartLabel,
                        data: Math.round(Math.abs(response[key])),
                        color: sliceColor

                    }

                    i = i + 1;
                }
            }


            $.plot($("#divAccountInfoPieChart"), data, {
                series: {
                    pie: {
                        show: true
                    }
                }
            });
            var end = new Date();
            console.log("end showAcountInfo", end);
            console.log("showAcountInfo", (end - timestart) / 1000);
        }
    });
}
// This function is used to Calculate the Total of Accounts, that helps to calulate percentage
function getTotalAccounts(response) {
    var totalAccounts = 0;
    for (var key in response) {

        if (key == "countOfNegativeAccounts" || key == "countOfPositiveAccounts" || key == "countOfZeroAccounts") {

            totalAccounts = totalAccounts + response[key];
        }

    }

    return Math.round(totalAccounts);
}
// This function is used to get Open Cashier Sessions and show data in form table
function showCashierSessions() {
    var urlAccountInfo = "/Home/GetCasheirSessions";
    var timestart;
    $.ajax({
        url: urlAccountInfo,
        type: "GET",
        data: { "viewType": "Index" },
        beforeSend: function () { console.log('start showCashierSessions', new Date()); timestart = new Date(); },
        error: function (response) {

        },
        success: function (response) {

            $("#divCashierSessionsLoading").hide();

            var response = JSON.parse(response);

            var tblCashier = '<table class="table table-hover"><thead><tr><th>POS</th><th>User</th><th>Open Date</th></tr></thead>';

            for (var key in response) {

                var openDate = moment(response[key]["POS_Open_Session_Date"]).format('MM/DD/YY')

                tblCashier += '<tbody><tr><td>' + response[key]["POS_Name"] + '</td><td>' + response[key]["POS_Open_Cashier"] + '</td><td>' + openDate + '</td></tr></tbody>';
            }

            tblCashier += '</table>';

            $("#divCashier").html(tblCashier);
            var end = new Date();
            console.log('end showCashierSessions', end);
            console.log("showCashierSessions", (end - timestart) / 1000);
        }
    });
}
// This function is used to Calculate percentage for the Account Information
function calcultePercentage(totalAccounts, accounts) {
    //  debugger;
    var percentage = (accounts / totalAccounts * 100).toFixed(2);

    return percentage.toString() + '%';
}
//This function is used to pick color for Account information PIE chart
function pickColor(AccountKey) {

    var color = '';

    if (AccountKey == "countOfNegativeAccounts") {
        color = ' #ed1d34'; //'#A23C3C'; //red
    }
    else if (AccountKey == "countOfPositiveAccounts") {
        color = ' #8dc63f'; //'#3D853D'; //green
    }
    else if (AccountKey == "countOfZeroAccounts") {
        color = '#989898'; // Grey //'#7633BD' //purple
    }

    return color;
}
//This function is used to Load the Payment and Sales data, also plot the fetched data on graph
function loadGraphData(url, DivChartID, DivChartLoaddingID, DivChartContentID, ChartColor) {
    var timestart;
    $.ajax({
        url: url,
        type: "GET",
        data: { "viewType": "Index" },
        beforeSend: function () { console.log('start loadGraphData' + url, new Date()); timestart = new Date(); },
        error: function (response) {

        },
        success: function (response) {

            var res = getPaymentGraphData(response);

            loadDashboardChart(res, DivChartID, DivChartLoaddingID, DivChartContentID, ChartColor);
            var end = new Date();
            console.log('end loadGraphData' + url, end);
            console.log("loadGraphData" + url, (end - timestart) / 1000);
        }
    });

}
//This function is used to get JSON format of Payment and Sales in order to PLOT data on Graph
function getPaymentGraphData(paymentDataString) {

    var paymentData = paymentDataString.replace(/"PaymentDate"/g, '');
    paymentData = paymentData.replace(/"TotalPayment"/g, '');
    paymentData = paymentData.replace(/:/g, '');
    paymentData = paymentData.replace(/{/g, '[');
    paymentData = paymentData.replace(/}/g, ']');
    return JSON.parse(paymentData);
}
//This function Plot the Payment and Sales data on Graph
function loadDashboardChart(ChartData, DivChartID, DivChartLoaddingID, DivChartContentID, ChartColor) {

    if ($('#' + DivChartID).size() != 0) {
        //site activities
        var previousPoint2 = null;
        $('#' + DivChartLoaddingID).hide();
        $('#' + DivChartContentID).show();

        var plot_statistics = $.plot($("#" + DivChartID),

            [{
                data: ChartData,
                points: {
                    show: true,
                    fill: true,
                    radius: 4,
                    fillColor: ChartColor
                },
                color: [ChartColor],
                shadowSize: 1

            }, {
                data: ChartData,
                lines: {
                    fill: false,
                    lineWidth: 1
                },
                color: [ChartColor]

            }, {
                data: ChartData,
                lines: {
                    fill: 0.2,
                    lineWidth: 1
                },
                color: [ChartColor]

            }],
            {

                xaxis: {
                    tickLength: 0,
                    tickDecimals: 0,
                    mode: "categories",
                    min: 0,
                    font: {
                        lineHeight: 18,
                        style: "normal",
                        variant: "small-caps",
                        color: "#6F7B8A"
                    }
                },
                yaxis: {
                    ticks: 5,
                    tickDecimals: 0,
                    tickColor: "#eee",
                    font: {
                        lineHeight: 14,
                        style: "normal",
                        variant: "small-caps",
                        color: "#6F7B8A"
                    }
                },
                grid: {
                    hoverable: true,
                    clickable: true,
                    tickColor: "#eee",
                    borderColor: "#eee",
                    borderWidth: 1
                }
            });

        $("#" + DivChartID).bind("plothover", function (event, pos, item) {
            $("#x").text(pos.x.toFixed(2));
            $("#y").text(pos.y.toFixed(2));
            if (item) {
                if (previousPoint2 != item.dataIndex) {
                    previousPoint2 = item.dataIndex;
                    $("#tooltip").remove();
                    var x = item.datapoint[0].toFixed(2),
                    y = item.datapoint[1].toFixed(2);
                    showChartTooltip(item.pageX, item.pageY, item.datapoint[0], '$' + item.datapoint[1]);
                }
            }
        });

        $('#' + DivChartID).bind("mouseleave", function () {
            $("#tooltip").remove();
        });
    }
}
//This function is used to show the tooltip on Graph Nodes
function showChartTooltip(x, y, xValue, yValue) {
    $('<div id="tooltip" class="chart-tooltip">' + yValue + '<\/div>').css({
        position: 'absolute',
        display: 'none',
        top: y - 40,
        left: x - 40,
        border: '0px solid #ccc',
        padding: '2px 6px',
        'background-color': '#fff'
    }).appendTo("body").fadeIn(200);
}


function formatDate(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear() + " " + strTime;
}