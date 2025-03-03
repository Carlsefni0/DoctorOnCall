import PendingActionsIcon from "@mui/icons-material/PendingActions";
import CalendarTodayIcon from "@mui/icons-material/CalendarToday";
import EventRepeatIcon from "@mui/icons-material/EventRepeat";
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import styles from "./table.module.css";

const visitStatusColors = {
    Pending: "orange",
    Approved: "green",
    Rejected: "red",
    Completed: "#2bcc56",
    Cancelled: "#2b4bcc",
};

const getRequestColumns = () => [
    // {
    //     field: "id",
    //     headerName: (
    //         <div className={styles.header_column}>
    //             <div className={styles.column_title}>Visit ID</div>
    //         </div>
    //     ),
    // },
   
    {
        field: "requestedDateTime",
        headerName: (
            <div className={styles.header_column}>
                <CalendarTodayIcon />
                <div className={styles.column_title}>Requested date</div>
            </div>
        ),
        render: (value) => new Date(value).toLocaleString().split(',')[0], // Formats the date for readability
    },
    {
        field: "requestedDateTime",
        headerName: (
            <div className={styles.header_column}>
                <AccessTimeIcon />
                <div className={styles.column_title}>Requested time</div>
            </div>
        ),
        render: (value) => new Date(value).toLocaleString().slice(11,17), // Formats the date for readability
    },
    {
        field: "type",
        headerName: (
            <div className={styles.header_column}>
                <AccessTimeIcon />
                <div className={styles.column_title}>Visit type</div>
            </div>
        ),
    },
    {
        field: "isRegularVisit",
        headerName: (
            <div className={styles.header_column}>
                <EventRepeatIcon />
                <div className={styles.column_title}>Regular visit</div>
            </div>
        ),
        render: (value) => (value ? "Yes" : "No"), // Maps boolean to readable text
    },
    {
        field: "status",
        headerName: (
            <div className={styles.header_column}>
                <PendingActionsIcon />
                <div className={styles.column_title}>Status</div>
            </div>
        ),
        render: (value) => (
            <span
                style={{
                    width: "2rem",
                    color: visitStatusColors[value],
                    padding: "6px 10px",
                    borderRadius: "1rem",
                    fontSize: "12px",
                    border: "1px solid lightgray",
                    backgroundColor: "#f9f9f9",
                }}
            >
                {value}
            </span>
        ),
    },
];

export default getRequestColumns;
