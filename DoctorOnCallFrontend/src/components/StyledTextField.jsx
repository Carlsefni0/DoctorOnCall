import { styled } from "@mui/material/styles";
import { TextField } from "@mui/material";

const StyledTextField = styled(TextField)(({ theme, borderColor, focusBorderColor, fontSize, error, width, labelColor, borderRadius }) => ({
    "& .MuiOutlinedInput-root": {
        "& .MuiOutlinedInput-notchedOutline": {
            borderColor: error ? "#d32f2f" : borderColor || "#ccc",
        },
        "&:hover .MuiOutlinedInput-notchedOutline": {
            borderColor: error ? "#d32f2f" : borderColor || "#25AF9DFF",
        },
        "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
            borderColor: error ? "#d32f2f" : focusBorderColor || "#25AF9DFF",
            boxShadow: "none",
        },
        borderRadius,
        
    },
    "& .MuiInputBase-input": {
        color: "#545454",
        fontFamily: "Josefin Sans, serif",
        fontSize: fontSize || "0.875rem",
    },
    "& .MuiInputLabel-root": {
        color: error ? "#d32f2f" : labelColor || "#545454", // Використовуємо labelColor для кастомізації
        fontFamily: "Josefin Sans, serif",
    },
    "& .MuiInputLabel-root.Mui-focused": {
        color: error ? "#d32f2f" : labelColor || "#25AF9DFF", // Колір мітки у фокусі
    },
    width: width || "100%",
}));


export default StyledTextField;
