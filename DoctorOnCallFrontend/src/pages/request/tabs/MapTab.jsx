import {APIProvider, Map, Marker} from "@vis.gl/react-google-maps";

export default function MapTab({visitCoords}) {
    const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY;

    const position = {
        lat: parseFloat(visitCoords.lat),
        lng: parseFloat(visitCoords.lng),
    };
    
    return   <APIProvider apiKey={apiKey}>
        <Map
            style={{ width: "100%", height: "75.2vh" }}
            defaultCenter={position}
            defaultZoom={15}
            gestureHandling="greedy"
            disableDefaultUI={true}
        >
            <Marker position={position} />
        </Map>
    </APIProvider>
}