import PersonIcon from "@mui/icons-material/Person";
import WorkIcon from "@mui/icons-material/Work";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import BadgeIcon from "@mui/icons-material/Badge";
import styles from "./table.module.css";

const doctorStatusColors = {
    0: "gray", // Неактивний
    1: "green", // Активний
};

const getDoctorColumns = () => [
    // {
    //     field: "id",
    //     headerName: (
    //         <div className={styles.header_column}>
    //             <BadgeIcon />
    //             <div className={styles.column_title}>Doctor ID</div>
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
        field: "specialization",
        headerName: (
            <div className={styles.header_column}>
                <WorkIcon />
                <div className={styles.column_title}>Specialization</div>
            </div>
        ),
    },
    {
        field: "workingDistrict",
        headerName: (
            <div className={styles.header_column}>
                <LocationOnIcon />
                <div className={styles.column_title}>District</div>
            </div>
        ),
    },
    // {
    //     field: "status",
    //     headerName: "Status",
    //     render: (value) => (
    //         <span
    //             style={{
    //                 color: doctorStatusColors[value],
    //                 padding: "6px 10px",
    //                 borderRadius: "1rem",
    //                 fontSize: "12px",
    //                 border: "1px solid lightgray",
    //                 backgroundColor: "#f9f9f9",
    //             }}
    //         >
    //             {value === 1 ? "Active" : "Inactive"}
    //         </span>
    //     ),
    // },
];

export default getDoctorColumns;
