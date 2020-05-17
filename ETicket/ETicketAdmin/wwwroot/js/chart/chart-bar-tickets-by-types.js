// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

var actionUrl = '/metrics/GetTicketsByTicketTypes';
$.getJSON(actionUrl, function (response) {
    if (response != null) {
        var chartData = response;
        var maxY = Math.max.apply(null, chartData.data);

        var ctx = document.getElementById("tickets-by-types");
        var myBarChart = new Chart(ctx, {
            type: 'horizontalBar',
            data: {
                labels: chartData.labels,
                datasets: [{
                    label: "Ticket count",
                    backgroundColor: "#4e73df",
                    hoverBackgroundColor: "#2e59d9",
                    borderColor: "#4e73df",
                    data: chartData.data,
                }],
            },
            options: {}
        });

    }
});

