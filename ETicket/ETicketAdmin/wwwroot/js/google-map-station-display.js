function initMap(z,y) {
    
    var uluru = {lat: z, lng: y};
    
    var x = document.getElementById("GeoLocation");

    if (x.style.display === "none") {
        x.style.display = "block";
        var map = new google.maps.Map(
            document.getElementById('GeoLocation'), {zoom: 12, center: uluru});
        
        var marker = new google.maps.Marker({position: uluru, map: map});
    } else {
        x.style.display = "none";
    }
}