function initMap() {
    var startMap = { lat: -25.363, lng: 131.044 };
    var map = new google.maps.Map(document.getElementById('map'), { zoom: 4, center: startMap });
    var marker = new google.maps.Marker({ position: startMap, map: map });
    var marker = new google.maps.Marker({ position: { lat: -25.363, lng: 151.044 }, map: map });
}

//initMap();

var geocoder;

if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(successFunction, errorFunction);
}
//Get the latitude and the longitude;
function successFunction(position) {
    var lat = position.coords.latitude;
    var lng = position.coords.longitude;
    codeLatLng(lat, lng);    
}

function getUserWeather(userLocation) {
    var trimmedStr = userLocation.replace(/\s+/g, '').replace('-', '');
    //trimmedStr = 'Eilat';
    //trimmedStr = 'RishonLetzion';

    var appid = "2e9a3b3f3b7a98e0442e3a85875a2481";
    $.get("http://api.openweathermap.org/data/2.5/weather?q=" + trimmedStr + "&units=metric&APPID=" + appid + "&units=imperial", function (response) {
        //response
        console.log(response);
        $("#name").text(response.name);
        $("#temp").text(response.main.temp);
        $("#humidity").text(response.main.humidity);
    })
}

function errorFunction() {
    alert("Geocoder failed");
}

function initialize() {
    geocoder = new google.maps.Geocoder();    
}

function codeLatLng(lat, lng) {

    var latlng = new google.maps.LatLng(lat, lng);
    var userCity = "";
    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            console.log(results)
            if (results[1]) {
                //formatted address
                //alert(results[0].formatted_address)
                //find country name
                for (var i = 0; i < results[0].address_components.length; i++) {
                    for (var b = 0; b < results[0].address_components[i].types.length; b++) {

                        //there are different types that might hold a city admin_area_lvl_1 usually does in come cases looking for sublocality type will be more appropriate
                        //if (results[0].address_components[i].types[b] == "administrative_area_level_1") {
                        if (results[0].address_components[i].types[b] == "locality") {
                            //this is the object you are looking for
                            userCity = results[0].address_components[i];
                            break;
                        }
                    }
                }
                //city data
                //alert(userCity.short_name + " " + userCity.long_name)


            } else {
                //alert("No results found");
            }
        } else {
            //alert("Geocoder failed due to: " + status);
        }

        getUserWeather(userCity.short_name);
    });
}

initialize()
