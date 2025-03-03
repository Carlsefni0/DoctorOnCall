import styles from './visit_request_item.module.css'
import StyledAvatar from "../StyledAvatar.jsx";
import stringAvatar from "../../utils/stringToAvatar.js";
import {Divider, Stack} from "@mui/material";
import {NavLink} from "react-router-dom";
export default function VisitRequestItem({visitRequest}) {
    const patientFullName = `${visitRequest.patientFirstName} ${visitRequest.patientLastName}`
    return( 
        <NavLink to={`../visits/${visitRequest.id}`} className={styles.visit_request_item_container}>
            <StyledAvatar {...stringAvatar(patientFullName)} width='3rem' height='3rem'/>
            <Stack justifyContent='center' spacing={1} width='25%'>
                <p>{patientFullName}</p>
                <p className={styles.email}>{visitRequest.patientEmail}</p>
            </Stack>
            <Divider orientation='vertical'/>
            <p className={styles.address}>{visitRequest.visitAddress}</p>
            <Divider orientation='vertical'/>
        </NavLink>)
      
}