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
    refreshPassengersByDayOfWeekChart();
});

$('#passengers-by-days-of-week-start').change(function () {
    refreshPassengersByDayOfWeekChart();
})
$('#passengers-by-days-of-week-end').change(function () {
    refreshPassengersByDayOfWeekChart();
})

var passengersByDayOfWeekChart = null;
function refreshPassengersByDayOfWeekChart() {
    var start = new Date($('#passengers-by-days-of-week-start').val());
    var end = new Date($('#passengers-by-days-of-week-end').val());

    if (isNaN(start.valueOf()) || isNaN(end.valueOf())) {
        return;
    }
   
    var actionUrl = '/metrics/GetPassengersByDaysOfWeek' + "?startPeriod=" + start.toISOString() + "&endPeriod=" + end.toISOString();
    $.getJSON(actionUrl, function (response) {
        if (response != null) {
            chartData = response;
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
                    datasets: [{
                        label: "passengers",
                        backgroundColor: "rgba(78, 115, 223, 0.05)",
                        borderColor: "rgba(78, 115, 223, 1)",
                        pointRadius: 4,
                        pointBackgroundColor: "rgba(78, 115, 223, 1)",
                        pointBorderColor: "rgba(78, 115, 223, 1)",
                        pointHoverRadius: 1,
                        pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
                        pointHoverBorderColor: "rgba(78, 115, 223, 1)",
                        pointHitRadius: 10,
                        pointBorderWidth: 2,
                        data: chartData.Data,
                    }],
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


