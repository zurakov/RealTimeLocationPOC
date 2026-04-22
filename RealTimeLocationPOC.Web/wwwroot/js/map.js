window.mapInterop = {
    map: null,
    markers: {},

    initMap: function (elementId) {
        if (this.map) {
            this.map.remove();
        }

        this.map = L.map(elementId).setView([25.276987, 55.296249], 13); // Dubai default

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
            maxZoom: 19
        }).addTo(this.map);

        // Fix tile rendering on late init
        setTimeout(() => this.map.invalidateSize(), 200);
    },

    upsertMarker: function (employeeId, fullName, lat, lng, heading, isOnline) {
        if (!this.map) return;

        var color = isOnline ? '#10b981' : '#6b7280';
        var pulse = isOnline ? 'marker-pulse' : '';

        var icon = L.divIcon({
            className: 'custom-marker',
            html: `<div class="marker-dot ${pulse}" style="background:${color};"></div>`,
            iconSize: [20, 20],
            iconAnchor: [10, 10],
            popupAnchor: [0, -15]
        });

        if (this.markers[employeeId]) {
            this.markers[employeeId].setLatLng([lat, lng]);
            this.markers[employeeId].setIcon(icon);
            this.markers[employeeId].setPopupContent(
                `<strong>${fullName}</strong><br/>` +
                `Status: ${isOnline ? '🟢 Online' : '⚫ Offline'}<br/>` +
                `Lat: ${lat.toFixed(6)}<br/>Lng: ${lng.toFixed(6)}` +
                (heading !== null ? `<br/>Heading: ${heading.toFixed(1)}°` : '')
            );
        } else {
            var marker = L.marker([lat, lng], { icon: icon }).addTo(this.map);
            marker.bindPopup(
                `<strong>${fullName}</strong><br/>` +
                `Status: ${isOnline ? '🟢 Online' : '⚫ Offline'}<br/>` +
                `Lat: ${lat.toFixed(6)}<br/>Lng: ${lng.toFixed(6)}` +
                (heading !== null ? `<br/>Heading: ${heading.toFixed(1)}°` : '')
            );
            this.markers[employeeId] = marker;
        }

        // If at least one marker exists, fit bounds
        var markerArray = Object.values(this.markers);
        if (markerArray.length > 0) {
            var lats = markerArray.map(m => m.getLatLng().lat).filter(l => l !== 0);
            var lngs = markerArray.map(m => m.getLatLng().lng).filter(l => l !== 0);
            if (lats.length > 0 && lngs.length > 0) {
                var bounds = L.latLngBounds(
                    markerArray
                        .map(m => m.getLatLng())
                        .filter(ll => ll.lat !== 0 && ll.lng !== 0)
                );
                this.map.fitBounds(bounds, { padding: [50, 50], maxZoom: 15 });
            }
        }
    },

    removeMarker: function (employeeId) {
        if (this.markers[employeeId]) {
            this.map.removeLayer(this.markers[employeeId]);
            delete this.markers[employeeId];
        }
    }
};

window.locationInterop = {
    getPosition: function () {
        return new Promise((resolve, reject) => {
            if (!navigator.geolocation) {
                reject('Geolocation not supported');
                return;
            }
            navigator.geolocation.getCurrentPosition(
                function (pos) {
                    resolve({
                        latitude: pos.coords.latitude,
                        longitude: pos.coords.longitude,
                        heading: pos.coords.heading
                    });
                },
                function (err) {
                    reject(err.message);
                },
                {
                    enableHighAccuracy: true,
                    timeout: 10000,
                    maximumAge: 5000
                }
            );
        });
    }
};

window.sseInterop = {
    eventSources: {},

    startSse: function (url, dotNetHelper) {
        if (this.eventSources[url]) {
            this.eventSources[url].close();
        }

        const eventSource = new EventSource(url);

        eventSource.onmessage = (event) => {
            dotNetHelper.invokeMethodAsync('OnMessageReceived', event.data);
        };

        eventSource.onerror = (err) => {
            console.error('SSE Error:', err);
        };

        this.eventSources[url] = eventSource;
    },

    stopSse: function (url) {
        if (this.eventSources[url] && this.eventSources[url].close) {
            this.eventSources[url].close();
            delete this.eventSources[url];
        }
    }
};
