import {styled} from "@mui/material/styles";
import {Autocomplete} from "@mui/lab";

const StyledAutocomplete = styled(Autocomplete)(({ theme, fontSize, width}) => ({
    "& .MuiInputBase-input": {
        color: "#545454",
        fontFamily: "Josefin Sans, serif",
        fontSize: fontSize || "0.875rem",
    },
    width: width || "100%",
}));
export default StyledAutocomplete;