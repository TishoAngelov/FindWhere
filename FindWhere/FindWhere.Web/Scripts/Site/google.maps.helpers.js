// Returns an object with the set marker and additional information about the selected location (latitude, neighbourhood, placeId etc.).
function setLocationOnMap(map, location, oldMarker, callback) {
    var geocoder = new google.maps.Geocoder;
    var infowindow = new google.maps.InfoWindow;
    var marker = null;

    if (oldMarker != null) {
        oldMarker.setMap(null);
    }

    geocoder.geocode({ 'location': location }, function (results, status) {
        if (status === google.maps.GeocoderStatus.OK) {
            var normalizedLocation = normalizeLocation(results);
            var errorMarkerIcon = '/Content/Images/GoogleMapsMarkers/close.png';
            var infoWindowAppearTimeout = 1200;
            var animationTimeout = 200;

            var locationDetails = {
                fullAddress: null,
                latitude: null,
                longitude: null,
                neighbourhood: null,
                placeId: null
            };

            // TODO: Add validation for allowed city and country.
            // If the selected location is allowed location.
            if (normalizedLocation.country && normalizedLocation.city && normalizedLocation.neighbourhood) {
                window.setTimeout(function () {
                    marker = new google.maps.Marker({
                        map: map,
                        position: location,
                        animation: google.maps.Animation.DROP,
                        clickable: true
                    });

                    var latitude = results[0].geometry.location.lat();
                    var longitude = results[0].geometry.location.lng();

                    locationDetails.fullAddress = results[0].formatted_address;
                    locationDetails.latitude = latitude;
                    locationDetails.longitude = longitude;
                    locationDetails.neighbourhood = normalizedLocation.neighbourhood;
                    locationDetails.placeId = normalizedLocation.placeId;

                    infowindow.setContent('Formated addres: ' + results[0].formatted_address +
                                       '<br />Country: ' + normalizedLocation.country +
                                       '<br />City: ' + normalizedLocation.city +
                                       '<br />Neighbourhood: ' + normalizedLocation.neighbourhood +
                                       '<br />Latitude: ' + latitude +
                                       '<br />Longitude: ' + longitude +
                                       '<br />Place ID: ' + normalizedLocation.placeId);

                    marker.addListener('click', function (event) {
                        infowindow.open(map, marker);
                    });

                    if (callback != null) {
                        callback(marker, locationDetails);
                    }
                }, animationTimeout);
            } else {
                marker = new google.maps.Marker({
                    map: map,
                    position: location,
                    icon: errorMarkerIcon,
                    animation: google.maps.Animation.BOUNCE,
                    clickable: true
                });

                infowindow.setContent('Incorrect location!');

                marker.addListener('click', function (event) {
                    infowindow.open(map, marker);
                });

                callback(marker, locationDetails);
            }

            window.setTimeout(function () {
                infowindow.open(map, marker);
            }, infoWindowAppearTimeout);
        } else {
            callback(marker, locationDetails);
        }
    });
}

// Returns an object { country: "", city: "", neighbourhood: "", placeId: "" }
function normalizeLocation(locations) {
    var result = {
        country: null,
        city: null,
        neighbourhood: null,
        placeId: null
    }

    if (locations == null || locations.length == 0) {
        return result;
    }

    result.placeId = locations[0].place_id;

    for (var i = 0; i < locations.length; i++) {
        var currLocation = locations[i];
        var components = currLocation.address_components;

        for (var c = 0; c < components.length; c++) {
            var currComponent = components[c];
            var componentType = currComponent.types[0];

            switch (componentType) {
                case "country": result.country = result.country || components[c].long_name; break;
                case "locality": result.city = result.city || components[c].long_name; break;
                case "neighborhood": result.neighbourhood = result.neighbourhood || components[c].long_name; break;
            }
        }
    }

    return result;
}