
function initMap(z,y) {
    var uluru = {lat: z, lng: y};
    var x = document.getElementById("GeoLocation");

    var map = new google.maps.Map(
        document.getElementById('GeoLocation'), {zoom: 12, center: uluru});
    // The marker, positioned at Uluru
    
    new google.maps.Marker({position: uluru, map: map});
    
    if (x.style.display === "none") {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
}
