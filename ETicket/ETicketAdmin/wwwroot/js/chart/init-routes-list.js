let routesList = []; //global var for all charts
let selectedRoutesPassengersByRoutesByHoursChart = [];

function initRoutesList() {
    if (routesList.length != 0) {
        return;
    }

    var slimSelectRoutesPassengersByRoutesByHours = new SlimSelect({
        select: '#routes-for-passengers-by-hours-by-routes',
        searchingText: 'Searching...', // Optional - Will show during ajax request
        hideSelectedOption: true,
        placeholder: 'Select routes',
        data: routesList,
        onChange: (info) => {
            selectedRoutesPassengersByRoutesByHoursChart = [];
            for (let i = 0; i < info.length; i++) {
                selectedRoutesPassengersByRoutesByHoursChart.push(info[i].value);
            }
            refreshPassengersByRoutesByHoursChart();
        }
    })

    var actionUrl = '/routes/GetRoutesList';
    $.getJSON(actionUrl, function (response) {
        if (response != null) {
            for (let i = 0; i < response.length; i++) {
                routesList.push({ value: response[i].Id, text: response[i].Number })
            }
        } 
        slimSelectRoutesPassengersByRoutesByHours.setData(routesList);
    });
}