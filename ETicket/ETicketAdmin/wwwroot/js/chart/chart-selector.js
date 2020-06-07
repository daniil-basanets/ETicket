var activeCharts = [];

function ManageChart(checkBox, chartURL, chartDivId) {
    var isOn = checkBox.checked;
    if (isOn) {
        $.ajax({
            type: "GET",
            url: chartURL,
            success: function (result) {
                activeCharts.push(chartDivId);
                $(chartDivId).html(result);
                $("#empty-chart-list").attr("hidden", "true");
            },
        });
    }
    else {
        var index = activeCharts.indexOf(chartDivId);
        if (index > -1) {
            activeCharts.splice(index, 1);
        }
        if (activeCharts.length == 0) {
            $("#empty-chart-list").removeAttr("hidden");
        }

        $(chartDivId).html("");
    }
}

$(document).ready(function () {
    $("#check-box-list input:checked").each(function () {
        $(this).trigger("change");
    });

    //Delete container from loyout only for Index
    $('.container').removeClass('container');
})