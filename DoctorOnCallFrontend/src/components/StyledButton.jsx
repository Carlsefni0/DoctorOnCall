import {styled} from "@mui/material/styles";
import {Button} from "@mui/material";

const StyledButton = styled(Button)(({ theme, backgroundColor, width, height, fontSize,hover, color, variant, marginTop }) => ({
    backgroundColor: variant === "text" ? "transparent" : backgroundColor || '#25AF9DFF',
    '&:hover': {
        backgroundColor: variant === "text" ? "rgba(0,0,0,0.08)" : hover || 'rgba(21,103,92,0.87)',
    },
    '&:disabled': {
        backgroundColor: variant === "text" ? "transparent" : '#bdbdbd',
    },
    color: color || "white",
    width,
    height,
    marginTop,
    fontSize: fontSize || '1rem',
    fontFamily: 'Josefin Sans, serif',
    textTransform: 'none',
    boxShadow: variant === "text" ? "none" : undefined,
}));

export default StyledButton;