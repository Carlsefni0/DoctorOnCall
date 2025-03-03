import styles from './doctor_item.module.css'
import StyledAvatar from "../StyledAvatar.jsx";
import {Stack} from "@mui/material";
import stringAvatar from "../../utils/stringToAvatar.js";
import {NavLink} from "react-router-dom";
import DoneIcon from '@mui/icons-material/Done';
export default function DoctorItem({doctor, isSelected, big}) {
    
    return<Stack alignItems='center' spacing={1} height='5rem'>
        <StyledAvatar {...stringAvatar(doctor.fullName)} width={`${big ? '7rem' : '3.5rem'}`} height={`${big ? '7rem' : '3.5rem'}`}/>
        <Stack alignItems='center'>
            <NavLink to={`../../doctors/${doctor.id}`} className={styles.nav_link}>{doctor.fullName}</NavLink>
            {isSelected && <DoneIcon /> }
        </Stack>
    </Stack>;
}