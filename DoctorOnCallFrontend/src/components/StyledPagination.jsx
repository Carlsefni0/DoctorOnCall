import {styled} from "@mui/material/styles";
import {Pagination} from "@mui/material";

const StyledPagination = styled(Pagination)(({ theme }) => ({
    "& .MuiPaginationItem-root": {
        "&:hover": {
            backgroundColor: theme.palette.action.hover,
        },
        "&.Mui-selected": {
            backgroundColor: "#25AF9DFF",
            color: theme.palette.common.white,
            "&:hover": {
                backgroundColor: "#158173",
            },
        },
    },
}));

export default StyledPagination