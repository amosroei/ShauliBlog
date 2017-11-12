
function initMap() {
    // initializes google map component
    var startMap = { lat: -25.363, lng: 131.044 };
    var map = new google.maps.Map(document.getElementById('map'), { zoom: 4, center: startMap });
    var marker = new google.maps.Marker({ position: startMap, map: map });
    var marker = new google.maps.Marker({ position: { lat: 32.1663133, lng: 34.843311 }, map: map });
}

//initMap();

var geocoder;

if (navigator.geolocation) {
    // gets the user location
    navigator.geolocation.getCurrentPosition(getPositionSuccessFunction, getPositionErrorFunction);
}
//Get the latitude and the longitude;
function getPositionSuccessFunction(position) {
    var lat = position.coords.latitude;
    var lng = position.coords.longitude;

    // converts the user location's lat and long to city
    convertLatLongToCity(lat, lng);    
}

function getPositionErrorFunction() {
    //alert("Geocoder failed");
}

function convertLatLongToCity(lat, lng) {

    var latlng = new google.maps.LatLng(lat, lng);
    var userCity = "";
    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            console.log(results)
            if (results[1]) {
                                
                //find country name
                for (var i = 0; i < results[0].address_components.length; i++) {
                    for (var b = 0; b < results[0].address_components[i].types.length; b++) {

                        //there are different types that might hold a city admin_area_lvl_1 usually does in come cases looking for sublocality type will be more appropriate                        
                        if (results[0].address_components[i].types[b] == "locality") {
                            //this is the object you are looking for
                            userCity = results[0].address_components[i];
                            break;
                        }
                    }
                }            
            }
        } 
        getUserWeather(userCity.long_name);
    });


    
}

// gets user's location, call openweather api and gets the weather in his location
//function getUserWeather(userLocation) {
//    var trimmedStr = userLocation.replace(/\s+/g, '').replace('-', '');
//    //trimmedStr = 'Eilat';
//    //trimmedStr = 'RishonLetzion';

//    var appid = "2e9a3b3f3b7a98e0442e3a85875a2481";
//    $.get("http://api.openweathermap.org/data/2.5/weather?q=" + trimmedStr + "&units=metric&APPID=" + appid + "&units=imperial", function (response) {
//        //response
//        console.log(response);

//        // sets the relevant html controls
//        $("#name").text(response.name);
//        $("#temp").text(response.main.temp);
//        $("#humidity").text(response.main.humidity);
//    })
//}

function getUserWeather(userLocation) {
    var worldWeatherAppId = "e31e72c85ae94d9991b105529171011";
    var formattedUserLocation = userLocation.replace(" ", "+");
    var url = "http://api.worldweatheronline.com/premium/v1/weather.ashx?key=" + worldWeatherAppId +
        "&q=" + userLocation + "&format=json";

    $.get(url, function (response) {
        //response
        console.log(response);

        var weatherObj = response.data.current_condition[0];
        var temprature = weatherObj.temp_C;
        var humidity = weatherObj.humidity;

        // sets the relevant html controls
        $("#name").text(userLocation);
        $("#temp").text(temprature);
        $("#humidity").text(humidity);
    })
}

function initialize() {
    geocoder = new google.maps.Geocoder();
}

initialize()

