import {styled} from "@mui/material/styles";
import Paper from "@mui/material/Paper";

const StyledPaper = styled(Paper)(({ theme, width , marginLeft, height, padding, paddingTop}) => ({
    backgroundColor: "#fdfdfd",
    width,
    marginLeft,
    height,
    borderTopRightRadius: "1rem",
    borderTopLeftRadius: "1rem",
    border: "1px solid lightgray",
    padding: padding || "0",
    paddingTop: paddingTop || "0",
}));

export default StyledPaper;