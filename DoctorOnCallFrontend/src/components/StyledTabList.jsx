import {TabList} from "@mui/lab";
import {styled} from "@mui/material/styles";

const StyledTabList = styled(TabList)(({ theme }) => ({
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    position: 'relative',
    '& .MuiTabs-indicator': {
        backgroundColor: '#30EBD5', 
        height: '4px', 
        borderRadius: '4px', 
        transition: 'transform 0.3s ease, width 0.3s ease, background-color 0.3s ease',
    },
    '& .MuiTab-root': {
        color: '#FFFFFF', 
        transition: 'color 0.3s ease',
        fontFamily: 'Josefin Sans, serif',
        
        
        '&.Mui-selected': {
            color: '#30EBD5', 
            fontWeight: 'bold',
        },
        '&:hover': {
            color: '#30EBD5', 
        },
    },
}));

export default StyledTabList;
