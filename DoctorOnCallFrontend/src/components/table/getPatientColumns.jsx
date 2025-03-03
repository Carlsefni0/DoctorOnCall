import PersonIcon from "@mui/icons-material/Person";
import BadgeIcon from "@mui/icons-material/Badge";
import CalendarTodayIcon from "@mui/icons-material/CalendarToday";
import WcIcon from "@mui/icons-material/Wc";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import HealingIcon from "@mui/icons-material/Healing";
import styles from "./table.module.css";

const getPatientColumns = () => [
    // {
    //     field: "id",
    //     headerName: (
    //         <div className={styles.header_column}>
    //             <BadgeIcon />
    //             <div className={styles.column_title}>Patient ID</div>
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
        field: "dateOfBirth",
        headerName: (
            <div className={styles.header_column}>
                <CalendarTodayIcon />
                <div className={styles.column_title}>Date of birth</div>
            </div>
        ),
        render: (value) => (
            <span>
               {new Date(value).toLocaleString().split(',')[0]}
            </span>
        ),
    },
    {
        field: "gender",
        headerName: (
            <div className={styles.header_column}>
                <WcIcon />
                <div className={styles.column_title}>Gender</div>
            </div>
        ),
    },
    {
        field: "district",
        headerName: (
            <div className={styles.header_column}>
                <LocationOnIcon />
                <div className={styles.column_title}>District</div>
            </div>
        ),
    },
    // {
    //     field: "disease",
    //     headerName: (
    //         <div className={styles.header_column}>
    //             <HealingIcon />
    //             <div className={styles.column_title}>Disease</div>
    //         </div>
    //     ),
    // },
];

export default getPatientColumns;
