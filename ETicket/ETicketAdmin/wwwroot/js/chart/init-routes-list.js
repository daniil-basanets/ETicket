let routesList = []; //global var for all charts
let selectedRoutesPassengersByRoutesByHoursChart = [];

function initRoutesList() {
    if (routesList.length != 0) {
        return;
    }

    var actionUrl = '/routes/GetRoutesList';
    $.getJSON(actionUrl, function (response) {
        if (response != null) {
            for (let i = 0; i < response.length; i++) {
                routesList.push({ value: response[i].Id, text: response[i].Number })
            }
        }

        new SlimSelect({
            select: '#routes-for-passengers-by-hours-by-routes',
            searchingText: 'Searching...', // Optional - Will show during ajax request
            data: routesList,
            onChange: (info) => {
                selectedRoutesPassengersByRoutesByHoursChart = [];
                for (let i = 0; i < info.length; i++) {
                    selectedRoutesPassengersByRoutesByHoursChart.push(info[i].value);
                }
                refreshPassengersByRoutesByHoursChart();
            }
        })
    });

}