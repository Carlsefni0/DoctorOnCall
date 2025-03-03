import {styled} from "@mui/material/styles";
import {Pagination, Radio} from "@mui/material";

const StyledRadio = styled(Radio)(({ theme }) => ({
    color: "#25AF9DFF",
    "&.Mui-checked": { color: "#25AF9DFF" },
}));

export default StyledRadio