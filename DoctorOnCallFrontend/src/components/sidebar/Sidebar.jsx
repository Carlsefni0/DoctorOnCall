import {NavLink} from "react-router-dom";
import styles from './sidebar.module.css'
import medicine from '../../assets/medicin.svg'
import calendar from '../../assets/calendar.png'
import doctor from '../../assets/doctor.png'
import patient from '../../assets/patient.png'
import mail from '../../assets/mail.png'
import list from '../../assets/list.png'
import assignment from '../../assets/assignment.png'
import scheduleException from '../../assets/schedule_exception.png'
import analytics from '../../assets/analytics.png'
import visit from '../../assets/visit.png'
import {getUser} from "../../context/UserContext.jsx";


const TAB_CONFIG = {
    Admin: [
        { path: "analytics", tabName: "Analytics", icon: analytics },
        { path: "doctors", tabName: "Doctors", icon: doctor },
        { path: "patients", tabName: "Patients", icon: patient },
        { path: "schedules", tabName: "Schedules", icon: calendar },
        { path: "schedule-exceptions", tabName: "Schedule exceptions", icon: scheduleException },
        { path: "visits", tabName: "Visits", icon: visit },
        
    ],
    Doctor: [
        { path: 'assigned-visits', tabName: 'Assigned visits', icon: assignment},
        { path: 'current-visits', tabName: 'Current visits', icon: list},
        { path: "visits", tabName: "Visits", icon: visit },
        { path: "schedules", tabName: "Schedules", icon: calendar },
        { path: "schedule-exceptions", tabName: "Schedule exceptions", icon: scheduleException },
        { path: "analytics", tabName: "Analytics", icon: analytics },
        
    ],
    Patient: [
        { path: "visits", tabName: "Visits", icon: visit }
    ],
}
export default function Sidebar() {
    const {user} = getUser();
    
    const tabs = TAB_CONFIG[user.role] || [];
    
    return <aside className={styles.sidebar_container}>
        <nav>
            <ul type="none">
                {tabs.map((tab) =>
                    <NavLink key={tab.path} to={`${tab.path}`}  className={({ isActive }) =>
                        isActive ? styles.tab__active : styles.tab
                    }>
                        <img src={tab.icon} alt={tab.path}/>
                        <div>{tab.tabName}</div>
                    </NavLink>
                )}
            </ul>
        </nav>
        
    </aside>
}