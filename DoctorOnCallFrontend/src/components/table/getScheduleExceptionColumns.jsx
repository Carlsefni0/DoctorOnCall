import ErrorOutlineIcon from "@mui/icons-material/ErrorOutline";
import CalendarTodayIcon from "@mui/icons-material/CalendarToday";
import PersonIcon from "@mui/icons-material/Person";
import NotesIcon from "@mui/icons-material/Notes";
import styles from "./table.module.css";

const exceptionStatusColors = {
    Pending: "orange",
    Approved: "green",
    Rejected: "red",
    Expired: "gray",
    Cancelled: "#2b4bcc",
};

const getScheduleExceptionColumns = () => [
    // {
    //     field: "id",
    //     headerName: (
    //         <div className={styles.header_column}>
    //             <div className={styles.column_title}>Exception ID</div>
    //         </div>
    //     ),
    // },
    // {
    //     field: "doctorFirstName",
    //     headerName: (
    //         <div className={styles.header_column}>
    //             <PersonIcon />
    //             <div className={styles.column_title}>Doctor First Name</div>
    //         </div>
    //     ),
    // },
    // {
    //     field: "doctorLastName",
    //     headerName: (
    //         <div className={styles.header_column}>
    //             <PersonIcon />
    //             <div className={styles.column_title}>Doctor Last Name</div>
    //         </div>
    //     ),
    // },
    {
        field: "fullName",
        headerName: (
            <div className={styles.header_column}>
                <PersonIcon />
                <div className={styles.column_title}>Full name</div>
            </div>
        ),
    },
    {
        field: "type",
        headerName: (
            <div className={styles.header_column}>
                <ErrorOutlineIcon />
                <div className={styles.column_title}>Type</div>
            </div>
        ),
    },
    
    {
        field: "startDate",
        headerName: (
            <div className={styles.header_column}>
                <CalendarTodayIcon />
                <div className={styles.column_title}>Start date</div>
            </div>
        ),
        render: (value) => (value ?  new Date(value).toLocaleString().split(',')[0] : "-"),
    },
    {
        field: "endDate",
        headerName: (
            <div className={styles.header_column}>
                <CalendarTodayIcon />
                <div className={styles.column_title}>End date</div>
            </div>
        ),
        render: (value) => (value ?  new Date(value).toLocaleString().split(',')[0] : "-"),
    },
    {
        field: "status",
        headerName: (
            <div className={styles.header_column}>
                <ErrorOutlineIcon />
                <div className={styles.column_title}>Status</div>
            </div>
        ),
        render: (value) => (
            <span
                style={{
                    color: exceptionStatusColors[value],
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

export default getScheduleExceptionColumns;
