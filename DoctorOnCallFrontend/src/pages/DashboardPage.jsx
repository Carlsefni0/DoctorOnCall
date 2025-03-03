import styles from "../styles/dashboard_style.module.css";
import Header from "../components/header/Header.jsx";
import Sidebar from "../components/sidebar/Sidebar.jsx";
import { Outlet } from "react-router-dom";

export default function Dashboard() {
    return (
        <div className={styles.dashboard_page_container}>
            <div className={styles.dashboard_header}>
                <Header />
            </div>
            <div className={styles.dashboard_sidebar}>
                <Sidebar />
            </div>
            <div className={styles.dashboard_content}>
                <Outlet />
            </div>
        </div>
    );
}
