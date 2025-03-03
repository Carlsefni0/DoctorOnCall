import { styled } from "@mui/material/styles";
import { SpeedDial, SpeedDialAction, SpeedDialIcon } from "@mui/material";

const StyledSpeedDial = styled(SpeedDial)(({ theme }) => ({
    "& .MuiSpeedDial-fab": {
        backgroundColor: "#25AF9DFF",
        color: theme.palette.common.white,
        width: "4rem",
        height: "4rem",
        
        "&:hover": {
            backgroundColor: "#158173",
        },
    },
}));

const StyledSpeedDialAction = styled(SpeedDialAction)(({ theme }) => ({
    "& .MuiSpeedDialAction-fab": {
        backgroundColor: "#ffffff",
        color: "#000000",
        "&:hover": {
            backgroundColor: "#f5f5f5",
        },
    },
}));

export default function CustomSpeedDial({ actions }) {
    return (
        <StyledSpeedDial
            ariaLabel="Actions"
            sx={{ position: "fixed", bottom: 16, right: 16 }}
            icon={<SpeedDialIcon />}
        >
            {actions.map((action) => (
                <StyledSpeedDialAction
                    key={action.name}
                    icon={action.icon}
                    tooltipTitle={action.name}
                    onClick={action.onClick}
                />
            ))}
        </StyledSpeedDial>
    );
}
