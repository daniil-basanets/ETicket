function ManageChart(checkBox, chartURL, chartDivId) {
    var isOn = checkBox.checked;
    if (isOn) {
        $.ajax({
            type: "GET",
            url: chartURL,
            success: function (result) {
                $(chartDivId).html(result);
            },
        });
    }
    else {
        $(chartDivId).html("");
    }
}