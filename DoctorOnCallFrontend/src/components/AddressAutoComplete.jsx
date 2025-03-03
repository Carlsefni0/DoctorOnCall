import React from 'react';
import TextField from "@mui/material/TextField";
import usePlacesAutocomplete, { getGeocode } from "use-places-autocomplete";
import StyledAutocomplete from "./StyledAutoComplete.jsx";

const LVIV_BOUNDS = {
    north: 49.93,  
    south: 49.775,
    east: 23.899, 
    west: 24.204, 
};

export default function AddressAutocomplete({ defaultValue, onAddressSelect, error, helperText }) {
    const { ready, value, setValue, suggestions: { status, data }, clearSuggestions } = usePlacesAutocomplete({
        debounce: 300,
        requestOptions: {
            bounds: LVIV_BOUNDS, 
            componentRestrictions: { country: "UA" }
        },
        defaultValue,
    });

    const handleSelect = async (address) => {
        setValue(address, false);
        clearSuggestions();

        try {
            const results = await getGeocode({ address });
            onAddressSelect(address, results[0]);
        } catch (error) {
            console.error("Error getting geocode:", error);
        }
    };

    const options = status === "OK" ? data.map(({ place_id, description }) => ({
        label: description,
        value: place_id,
    })) : [];

    return (
        <StyledAutocomplete
            options={options}
            value={value}
            onChange={(event, newValue) => {
                if (newValue) {
                    handleSelect(newValue.label);
                }
            }}
            onInputChange={(event, newInputValue) => {
                setValue(newInputValue);
            }}
            renderInput={(params) => (
                <TextField
                    {...params}
                    fullWidth
                    label="Address"
                    error={error}
                    helperText={helperText}
                />
            )}
            disabled={!ready}
        />
    );
}