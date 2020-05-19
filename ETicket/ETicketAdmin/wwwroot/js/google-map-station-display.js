function initMap(z,y) {
    // The location of Uluru
    var uluru = {lat: z, lng: y};
    // The map, centered at Uluru
    var x = document.getElementById("GeoLocation");

    if (x.style.display === "none") {
        x.style.display = "block";
        var map = new google.maps.Map(
            document.getElementById('GeoLocation'), {zoom: 12, center: uluru});
        // The marker, positioned at Uluru
        var marker = new google.maps.Marker({position: uluru, map: map});
    } else {
        x.style.display = "none";
        var map = new google.maps.Map(
            document.getElementById('GeoLocation'), {zoom: 12, center: uluru});
        // The marker, positioned at Uluru
        var marker = new google.maps.Marker({position: uluru, map: map});
    }
}