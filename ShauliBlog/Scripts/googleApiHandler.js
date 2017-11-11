function initMap() {
    //var startMap = { lat: -25.363, lng: 131.044 };
    //var map = new google.maps.Map(document.getElementById('map'), { zoom: 4, center: startMap });
    //var marker = new google.maps.Marker({ position: startMap, map: map });
    //var marker = new google.maps.Marker({ position: { lat: -25.363, lng: 151.044 }, map: map });

    //initialize();
    initializeAutoComplete();
}

// initializes google's auto complete component
function initializeAutoComplete() {
    var input = document.getElementById('pac-input');

    if (input != null) {
        var autocomplete = new google.maps.places.Autocomplete(input);

        autocomplete.addListener('place_changed', function () {
            var place = autocomplete.getPlace();
            console.log(place);
            console.log(input);           
        })
    }
}

// gets all users addresses list, formatted as a string.
// shows address on map
function showUserAddresses(addressList) {
    //addressList = "Spain;" + addressList;
    addressStrList = addressList.split(";");
    
    var map;
    var bounds = new google.maps.LatLngBounds();
    for (address of addressStrList) {
        if (address != "") {            
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {

                    var latitude = results[0].geometry.location.lat();
                    var longitude = results[0].geometry.location.lng();

                    if (map == null) {
                        var mapOptions = { center: new google.maps.LatLng(latitude, longitude), zoom: 8, mapTypeId: google.maps.MapTypeId.ROADMAP };
                        map = new google.maps.Map(document.getElementById("map"), mapOptions);
                    }

                    var marker = new google.maps.Marker({ position: new google.maps.LatLng(latitude, longitude), map: map });

                    bounds.extend(marker.getPosition());
                    map.fitBounds(bounds);
                } else {
                    console.log('invalid address: ' + address);
                }
            });
        }
    }
}