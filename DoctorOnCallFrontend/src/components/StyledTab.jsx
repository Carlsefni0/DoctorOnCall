import { styled } from "@mui/material/styles";
import Tab from "@mui/material/Tab";

const StyledTab = styled(Tab)(({ selectedColor, defaultColor, fontSize }) => ({
    fontSize: fontSize || "1rem",
    color: defaultColor || "white", 
    textTransform: "none", 
    transition: "color 0.3s ease", 
    "&.Mui-selected": {
        fontFamily: 'Josefin Sans, serif',
        color: selectedColor || "#30EBD5", 
        fontWeight: "bold",
    },
    "&:hover": {
        color: selectedColor || "#30EBD5", 
    },
   
    
}));

export default StyledTab;
