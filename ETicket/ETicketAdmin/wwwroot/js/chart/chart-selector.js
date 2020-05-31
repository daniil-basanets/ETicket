function ManageTicketsByTicketTypesChart() {
    var isOn = $("#TicketsByTicketTypesChartCheckbox").val();
}

function ManageTicketsByTicketTypesChart() {
    var isOn = $("#TicketsByTicketTypesChartCheckbox").is(':checked');
    if (isOn) {
        $.ajax({
            type: "GET",
            url: "/metrics/TicketsByTicketTypesChart",
            success: function (result) {
                $("#TicketsByTicketTypesChartPlace").html(result);
            },
        });
    }
    else {
        $("#TicketsByTicketTypesChartPlace").html("");
    }
}

function ManagePassengerByTimeChart() {
    var isOn = $("#PassengerByTimeChartCheckbox").is(':checked');
    if (isOn) {
        $.ajax({
            type: "GET",
            url: "/metrics/PassengersByTimeChart",
            success: function (result) {
                $("#PassengerByTimeChartPlace").html(result);
            },
        });
    }
    else {
        $("#PassengerByTimeChartPlace").html("");
    }   
}

function ManagePassengersByPrivilegeChart() {
    var isOn = $("#PassengersByPrivilegeChartCheckbox").is(':checked');
    if (isOn) {
        $.ajax({
            type: "GET",
            url: "/metrics/PassengersByPrivilegesChart",
            success: function (result) {
                $("#PassengersByPrivilegeChartPlace").html(result);
            },
        });
    }
    else {
        $("#PassengersByPrivilegeChartPlace").html("");
    }
}