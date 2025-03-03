import {Map, Marker, useMap, useMapsLibrary} from "@vis.gl/react-google-maps";
import {useEffect, useRef} from "react";

export default function RouteMap({ start, waypoints }) {
    const mapLib = useMapsLibrary("routes");
    const map = useMap();
    const directionsRenderer = useRef(null);

    useEffect(() => {
        if (!mapLib || !map || !window.google) return;

        const directionsService = new window.google.maps.DirectionsService();
        if (!directionsRenderer.current) {
            directionsRenderer.current = new window.google.maps.DirectionsRenderer();
        }

        directionsService.route(
            {
                origin: start,
                destination: waypoints[waypoints.length - 1], // кінцева точка
                waypoints: waypoints.slice(0, -1).map((point) => ({ location: point, stopover: true })),
                travelMode: window.google.maps.TravelMode.DRIVING,
                optimizeWaypoints: true,
                
            },
            (result, status) => {
                if (status === "OK") {
                    directionsRenderer.current.setDirections(result);
                    directionsRenderer.current.setMap(map);
                } else {
                    console.error("Directions request failed due to " + status);
                }
            }
        );
    }, [mapLib, map, start, waypoints]);

    return (
        <Map
            style={{ width: "100%", height: "75.2vh" }}
            defaultCenter={start}
            defaultZoom={12}
            gestureHandling="greedy"
            disableDefaultUI={true}
        >
            <Marker position={start} /> {/* Початкова точка */}
            {waypoints.map((point, index) => (
                <Marker key={index} position={point} />
            ))}
        </Map>
    );
}
