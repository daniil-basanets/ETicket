var stationList = [];

var selectStations = [];

var slimSelectStationForTheRoute = new SlimSelect({
    select: '#stations-for-the-route',
    selected: true,
    searchingText: 'Searching...', // Optional - Will show during ajax request
    hideSelectedOption: true,
    placeholder: 'Select stations',
    data: stationList,

    onChange: (info) => {
        if (info.length > selectStations.length) { //Add
            for (let i = 0; i < info.length; i++) {
                if (!selectStations.includes(info[i].value)) {
                    selectStations.push(info[i].value);
                }
            }
        }
        else {

            if (info.length == 0) {
                selectStations = [];
            }

            for (let i = 0; i < selectStations.length; i++) { //Delete

                var idForDelete = -1;

                for (let j = 0; j < info.length; j++) {
                    if (selectStations[i] == info[j].value) {
                        idForDelete = -1;
                        break;
                    }
                    else {
                        idForDelete = i;
                    }

                }

                if (idForDelete != -1) {
                    selectStations.splice(i, 1);
                }
            }
        }
        SaveStationInHiddenItems(selectStations);
    }
});

function SaveStationInHiddenItems(selectStations) {
    $("#station-hidden-items").html("");
    for (let i = 0; i < selectStations.length; i++) {
        var input = document.createElement("input");
        input.setAttribute('type', 'hidden');
        input.setAttribute('name', 'StationIds');
        input.setAttribute('value', selectStations[i]);
        $("#station-hidden-items").append(input);
    }
}