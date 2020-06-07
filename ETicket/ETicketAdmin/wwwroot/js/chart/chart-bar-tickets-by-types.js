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
    $('#tickets-by-types-start').val(yyyy_mm_dd(dateStart));
    $('#tickets-by-types-end').val(yyyy_mm_dd(dateEnd));
    refreshTicketsByTypesChart();
});

$('#tickets-by-types-start').change(function () {
    refreshTicketsByTypesChart();
})
$('#tickets-by-types-end').change(function () {
    refreshTicketsByTypesChart();
})


var ticketsByTypesChart = null;
var chartData;
function refreshTicketsByTypesChart() {
    var start = new Date($('#tickets-by-types-start').val());
    var end = new Date($('#tickets-by-types-end').val());

    if (isNaN(start.valueOf()) || isNaN(end.valueOf())) {
        return;
    }

    var actionUrl = '/metrics/GetTicketsByTicketTypes' + "?startPeriod=" + start.toISOString() + "&endPeriod=" + end.toISOString();
    $.getJSON(actionUrl, function (response) {
        if (response != null) {
            chartData = response;
            
            var ctx = document.getElementById("tickets-by-types");

            if (ticketsByTypesChart != null) {
                ticketsByTypesChart.destroy();
            }

            if (chartData.ErrorMessage) {
                $("#tickets-by-types-error").html(chartData.ErrorMessage);
            }
            else {
                $("#tickets-by-types-error").html("");
            }

            ticketsByTypesChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: chartData.Labels,
                    datasets: [{
                        label: "Ticket count",
                        backgroundColor: "rgba(78, 115, 223, 0.9)", 
                        hoverBackgroundColor: "#2e59d9",
                        borderColor: "#4e73df",
                        data: chartData.Data,
                    }],
                },

                options: {
                    maintainAspectRatio: false,
                    layout: {
                        padding: {
                            left: 10,
                            right: 25,
                            top: 25,
                            bottom: 0
                        }
                    },
                    scales: {
                        xAxes: [{
                            scaleLabel: {
                                display: true,
                                labelString: 'ticket types'
                            },
                            time: {
                                unit: 'month'
                            },
                            gridLines: {
                                display: false,
                                drawBorder: false
                            },
                            ticks: {
                                maxTicksLimit: 50
                            },
                            maxBarThickness: 25,
                        }],
                        yAxes: [{
                            scaleLabel: {
                                display: true,
                                labelString: 'tickets count'
                            },
                            ticks: {
                                min: 0,
                                maxTicksLimit: 100,
                                padding: 10,
                                callback: function (value, index, values) {
                                    return number_format(value);
                                }
                            },
                            gridLines: {
                                color: "rgb(234, 236, 244)",
                                zeroLineColor: "rgb(234, 236, 244)",
                                drawBorder: false,
                                borderDash: [2],
                                zeroLineBorderDash: [2]
                            }
                        }],
                    },
                    legend: {
                        display: false
                    },
                    tooltips: {
                        titleMarginBottom: 10,
                        titleFontColor: '#6e707e',
                        titleFontSize: 14,
                        backgroundColor: "rgb(255,255,255)",
                        bodyFontColor: "#858796",
                        borderColor: '#dddfeb',
                        borderWidth: 1,
                        xPadding: 15,
                        yPadding: 15,
                        displayColors: false,
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
    })
};

