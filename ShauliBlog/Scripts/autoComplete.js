google.maps.event.addListenerOnce(map, 'idle', function () {
    // do something only the first time the map is loaded
    loadAutoComplete();
});
function loadAutoComplete() {
    var input = document.getElementById('pac-input');
    var autocomplete = new google.maps.places.Autocomplete(input);
}