function initMap(coords) {

    var uluru = {lat: coords[0].latitude, lng: coords[0].longitude};
    var x = document.getElementById("GeoLocation");

    var map = new google.maps.Map(
        document.getElementById('GeoLocation'), {zoom: 12, center: uluru});

    for (var i = 0; i < coords.length; i++) {
        uluru = {lat: coords[i].latitude, lng: coords[i].longitude};
        new google.maps.Marker({position: uluru, map: map});
    }

    if (x.style.display === "none") {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
}

