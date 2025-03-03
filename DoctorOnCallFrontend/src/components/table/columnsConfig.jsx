import EventIcon from "@mui/icons-material/Event";
import AccessTimeIcon from "@mui/icons-material/AccessTime";
import StarIcon from "@mui/icons-material/Star";
import WbSunnyIcon from "@mui/icons-material/WbSunny";
import AcUnitIcon from "@mui/icons-material/AcUnit";
import SpaIcon from "@mui/icons-material/Spa";
import FilterDramaIcon from "@mui/icons-material/FilterDrama";

const statusColors = {
    Pending: "orange",
    Approved: "green",
    Accepted: "blue",
    Rejected: "red",
    Scheduled: "purple",
    Cancelled: "gray",
};

const getSeasonIcon = (date) => {
    const month = date.getMonth() + 1;
    if ([12, 1, 2].includes(month)) return <AcUnitIcon />;
    if ([3, 4, 5].includes(month)) return <SpaIcon />;
    if ([6, 7, 8].includes(month)) return <WbSunnyIcon />;
    if ([9, 10, 11].includes(month)) return <FilterDramaIcon />;
    return null;
};

export const getColumns = (rows) => [
    {
        field: "status",
        headerName: (
            <div style={{ display: "flex", alignItems: "center", justifyContent: "center" }}>
                <StarIcon style={{ marginRight: 4 }} />
                Status
            </div>
        ),
        render: (value) => (
            <span
                style={{
                    color: statusColors[value],
                    padding: "6px 10px",
                    borderRadius: "1rem",
                    justifyContent: "center",
                    fontSize: "12px",
                    border: "1px solid lightgray",
                    backgroundColor: "#faf8f8",
                }}
            >
                {value}
            </span>
        ),
    },
    {
        field: "requestedDate",
        headerName: (
            <div style={{ display: "flex", alignItems: "center", justifyContent: "center", gap: "0.3rem" }}>
                <EventIcon style={{ marginRight: 4 }} />
                Requested Date
            </div>
        ),
        render: (value) => (
            <div style={{ display: "flex", alignItems: "center", gap: "8px", justifyContent: "center" }}>
                {getSeasonIcon(new Date(value))}
                {value}
            </div>
        ),
    },
    {
        field: "requestedTime",
        headerName: (
            <div style={{ display: "flex", alignItems: "center", justifyContent: "center", gap: "0.3rem" }}>
                <AccessTimeIcon style={{ marginRight: 4 }} />
                Requested Time
            </div>
        ),
    },
    {
        field: "isRegularVisit",
        headerName: "Is Regular Visit",
        render: (value) => (value ? "Yes" : "No"),
    },
];
