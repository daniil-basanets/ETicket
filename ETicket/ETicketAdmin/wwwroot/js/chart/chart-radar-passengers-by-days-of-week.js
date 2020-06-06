// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

function number_format(number, decimals, dec_point, thousands_sep) {
    // *     example: number_format(1234.56, 2, ',', ' ');
    // *     return: '1 234,56'
    number = (number + '').replace(',', '').replace(' ', '');
    var n = !isFinite(+number) ? 0 : +number,
        prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
        sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
        dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
        s = '',
        toFixedFix = function (n, prec) {
            var k = Math.pow(10, prec);
            return '' + Math.round(n * k) / k;
        };
    // Fix for IE parseFloat(0.55).toFixed(0) = 0;
    s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');
    if (s[0].length > 3) {
        s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
    }
    if ((s[1] || '').length < prec) {
        s[1] = s[1] || '';
        s[1] += new Array(prec - s[1].length + 1).join('0');
    }
    return s.join(dec);
}

function yyyy_mm_dd(date) {
    //var now = new Date();
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return '' + y + "-" + (m < 10 ? '0' : '') + m + "-" + (d < 10 ? '0' : '') + d;
}

$(document).ready(function () {
    var dateStart = new Date(new Date().setMonth(new Date().getMonth() - 1));
    var dateEnd = new Date();
    $('#passengers-by-days-of-week-start').val(yyyy_mm_dd(dateStart));
    $('#passengers-by-days-of-week-end').val(yyyy_mm_dd(dateEnd));
    $('#passengers-by-days-of-week-start').attr('max', new Date());
    $('#passengers-by-days-of-week-end').attr('max', new Date());
    refreshPassengersByDayOfWeekChart();
});

$('#passengers-by-days-of-week-start').change(function () {
    refreshPassengersByDayOfWeekChart();
})
$('#passengers-by-days-of-week-end').change(function () {
    refreshPassengersByDayOfWeekChart();
})

var selectedRoutesPassengerByWeekDaysChart = [];

$(document).ready(function () {
    var slimSelectPassengerByWeekDays = new SlimSelect({
        select: '#routes-for-passengers-by-days-of-week',
        searchingText: 'Searching...', // Optional - Will show during ajax request
        hideSelectedOption: true,
        placeholder: 'Select routes',
        onChange: (info) => {
            selectedRoutesPassengerByWeekDaysChart = [];
            for (let i = 0; i < info.length; i++) {
                selectedRoutesPassengerByWeekDaysChart.push(info[i].value);
            }
            refreshPassengersByDayOfWeekChart();
        }
    });

    var actionUrl = '/routes/GetRoutesList';
    $.getJSON(actionUrl, function (response) {
        let responseResult = [];
        if (response != null) {
            for (let i = 0; i < response.length; i++) {
                responseResult.push({ value: response[i].Id, text: response[i].Number })
            }
        }

        slimSelectPassengerByWeekDays.setData(responseResult);
    });
})

var collor = [
    "rgba(0, 117, 220, 0.5)"
    , "rgba(153, 63, 0, 0.5)"
    , "rgba(76, 0, 92, 0.5)"
    , "rgba(25, 25, 25, 0.5)"
    , "rgba(0, 92, 49, 0.5)"
    , "rgba(255, 204, 72, 0.5)"
    , "rgba(255, 204, 153, 0.5)"
    , "rgba(128, 128, 128, 0.5)"
    , "rgba(148, 255, 181, 0.5)"
    , "rgba(240, 163, 255, 0.5)"
    , "rgba(143, 124, 0, 0.5)"
    , "rgba(194, 0, 136, 0.5)"
    , "rgba(0, 51, 128, 0.5)"
    , "rgba(255, 164, 5, 0.5)"
    , "rgba(255, 168, 187, 0.5)"
    , "rgba(66, 102, 0, 0.5)"
    , "rgba(255, 0, 16, 0.5)"
    , "rgba(94, 241, 242, 0.5)"
    , "rgba(0, 153, 143, 0.5)"
    , "rgba(224, 255, 102, 0.5)"
    , "rgba(116, 10, 255, 0.5)"
    , "rgba(153, 0, 0, 0.5)"
    , "rgba(255, 255, 128, 0.5)"
    , "rgba(255, 255, 0, 0.5)"
    , "rgba(255, 80, 5, 0.5)"
    , "rgba(255, 255, 128, 0.5)"
]

function getCollor(id) {
    if (id > collor.length) {
        return "rgba(255, 255, 255, 0.5)"
    }
    return collor[id];
}

var passengersByDayOfWeekChart = null;
function refreshPassengersByDayOfWeekChart() {
    var start = new Date($('#passengers-by-days-of-week-start').val());
    var end = new Date($('#passengers-by-days-of-week-end').val());

    if (isNaN(start.valueOf()) || isNaN(end.valueOf())) {
        return;
    }
    var selectedRoutesUrl = "";
    for (var key in selectedRoutesPassengerByWeekDaysChart) {
        selectedRoutesUrl += "selectedRoutesId=" + selectedRoutesPassengerByWeekDaysChart[key] + "&";
    }
    var actionUrl = "";
    var isMultiLines = false;
    if (selectedRoutesUrl) {
        isMultiLines = true;
        actionUrl = '/metrics/GetMultiRoutesPassengersByDaysOfWeek' + "?startPeriod=" + start.toISOString() + "&endPeriod=" + end.toISOString() + "&scale=" + chartScale + '&' + selectedRoutesUrl.slice(0, -1);
    } else {
        isMultiLines = false;
        actionUrl = '/metrics/GetPassengersByDaysOfWeek' + "?startPeriod=" + start.toISOString() + "&endPeriod=" + end.toISOString() + "&scale=" + chartScale;
    }
    $.getJSON(actionUrl, function (response) {
        if (response != null) {
            chartData = response;

            var datasetsChart = [];
            if (!isMultiLines) {
                datasetsChart[0] = {
                    label: "total passengers",
                    data: chartData.Data, //routes data
                    borderColor: getCollor(0),
                    backgroundColor: "rgba(0, 0, 0, 0)",
                    pointRadius: 3,
                    pointBackgroundColor: getCollor(0).replace("0.5", "1"),
                    pointBorderColor: getCollor(0).replace("0.5", "1"),
                    pointHoverRadius: 3,
                    pointHoverBackgroundColor: getCollor(0).replace("0.5", "1"),
                    pointHoverBorderColor: getCollor(0).replace("0.5", "1"),
                    pointHitRadius: 10,
                    pointBorderWidth: 2
                }
            }
            else {
                chartData = response;
                for (var i = 0; i < chartData.LineLable.length; i++) {
                    var items = [];
                    for (var j = 0; j < chartData.Data[i].length; j++) {
                        items[j] = chartData.Data[i][j];
                    }
                    //alert(chartData.Labels[i]);
                    var temp = {
                        label: chartData.LineLable[i],
                        data: items, //routes data
                        borderColor: getCollor(i),
                        backgroundColor: "rgba(0, 0, 0, 0)",
                        pointRadius: 3,
                        pointBackgroundColor: getCollor(i).replace("0.5", "1"),
                        pointBorderColor: getCollor(i).replace("0.5", "1"),
                        pointHoverRadius: 3,
                        pointHoverBackgroundColor: getCollor(i).replace("0.5", "1"),
                        pointHoverBorderColor: getCollor(i).replace("0.5", "1"),
                        pointHitRadius: 10,
                        pointBorderWidth: 2
                    }
                    datasetsChart[i] = temp
                }
            }
            //chartData = response;
            var maxY = Math.max.apply(null, chartData.Data);

            var ctx = document.getElementById("passengers-by-days-of-week");

            if (passengersByDayOfWeekChart != null) {
                passengersByDayOfWeekChart.destroy();
            }

            if (chartData.ErrorMessage) {
                $("#passengers-by-days-of-week-error").html(chartData.ErrorMessage);
            }
            else {
                $("#passengers-by-days-of-week-error").html("");
            }

            passengersByDayOfWeekChart = new Chart(ctx, {
                type: 'radar',
                data: {
                    labels: chartData.Labels,
                    datasets: datasetsChart,
                },
                options: {
                    maintainAspectRatio: false,
                    tooltips: {
                        backgroundColor: "rgb(255,255,255)",
                        bodyFontColor: "#858796",
                        titleMarginBottom: 10,
                        titleFontColor: '#6e707e',
                        titleFontSize: 14,
                        borderColor: '#dddfeb',
                        borderWidth: 1,
                        xPadding: 15,
                        yPadding: 15,
                        displayColors: false,
                        intersect: false,
                        mode: 'index',
                        caretPadding: 10,
                        callbacks: {
                            title: function (tooltipItems, chart) {
                                //Return value for title
                                return chart.labels[tooltipItems[0].index];
                            },
                            label: function (tooltipItem, chart) {
                                var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                                return datasetLabel + ': ' + number_format(tooltipItem.yLabel);
                            }
                        }
                    },
                }
            });
        }
    });
}


