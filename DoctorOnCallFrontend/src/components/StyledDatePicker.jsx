import { styled } from "@mui/material/styles";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";

const StyledDatePicker = styled(DatePicker)(({ theme, borderRadius, height }) => ({
    "& .MuiOutlinedInput-root": {
        borderRadius,
        transition: "border-color 0.2s ease-in-out",
        color: "#545454",
        height,

        "& fieldset": {
            borderColor: theme.palette.grey[400], // Стандартний колір контуру
        },

        "&:hover fieldset": {
            borderColor:  "#25AF9DFF", // Колір при наведенні
        },

        "&.Mui-focused fieldset": {
            borderColor:  "#25AF9DFF", // Колір при фокусі
            borderWidth: "2px",
        },
    },
}));

export default StyledDatePicker;
