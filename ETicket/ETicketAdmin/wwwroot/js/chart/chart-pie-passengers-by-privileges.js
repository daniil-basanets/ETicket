// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

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
    $('#passengers-by-privilege-start').val(yyyy_mm_dd(dateStart));
    $('#passengers-by-privilege-end').val(yyyy_mm_dd(dateEnd));
    refreshPassengersByPrivilege();
});

$('#passengers-by-privilege-start').change(function () {
    refreshPassengersByPrivilege();
})
$('#passengers-by-privilege-end').change(function () {
    refreshPassengersByPrivilege();
})

var passengersByPrivilegeChart = null;
function refreshPassengersByPrivilege() {
    var start = new Date($('#passengers-by-privilege-start').val());
    var end = new Date($('#passengers-by-privilege-end').val());

    if (isNaN(start.valueOf()) || isNaN(end.valueOf())) {
        return;
    }

// Pie Chart Example
    var chartData;

    var actionUrl = '/metrics/PassengersByPrivileges' + "?startPeriod=" + start.toISOString() + "&endPeriod=" + end.toISOString();
    $.getJSON(actionUrl, function (response) {
        if (response != null) {
            chartData = response;
            
            var ctx = document.getElementById("passengers-by-privilege");

            if (passengersByPrivilegeChart != null) {
                passengersByPrivilegeChart.destroy();
            }

            if (chartData.ErrorMessage) {
                $("#passengers-by-privilege-error").html(chartData.ErrorMessage);
            }
            else {
                $("#passengers-by-privilege-error").html("");
            }

            passengersByPrivilegeChart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: chartData.Labels,
                    datasets: [{
                        data: chartData.Data,
                        backgroundColor: ['#808080', '#4e73df', '#1cc88a', '#36b9cc', '#FFBF40', '#7109AA'],
                        hoverBorderColor: "rgba(234, 236, 244, 1)",
                    }],
                },
                options: {
                    maintainAspectRatio: false,
                    tooltips: {
                        backgroundColor: "rgb(255,255,255)",
                        bodyFontColor: "#858796",
                        borderColor: '#dddfeb',
                        borderWidth: 1,
                        xPadding: 15,
                        yPadding: 15,
                        displayColors: false,
                        caretPadding: 10,
                    },
                    legend: {
                        display: true
                    },
                    cutoutPercentage: 50,
                },

            })
        }
    })
};
