import { Stack, Divider } from "@mui/material";
import { NavLink } from "react-router-dom";
import StyledAvatar from "../../../components/StyledAvatar.jsx";
import stringAvatar from "../../../utils/stringToAvatar.js";
import styles from "./request_info_tab.module.css";

export default function RequestInfoTab({ requestData }) {
    const formatDateTime = (dateString) => {
        const date = new Date(dateString);
        return {
            date: date.toLocaleDateString(),
            time: date.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })
        };
    };

    return (
        <Stack padding='2rem' spacing={4} height='55vh'>
            <NavLink to={`../../patients/${requestData.patientId}`} className={`${styles.property_wrapper} ${styles.nav_link}`}>
                <dt>Patient</dt>
                <Stack direction='row' spacing={1} alignItems='center'>
                    <StyledAvatar {...stringAvatar(`${requestData.patientFullName}`)} width='4rem' height='4rem' />
                    <Stack direction='column' spacing={1}>
                        <dd id="patientFullName">{requestData.patientFullName}</dd>
                        <dd className={styles.email}>{requestData.patientEmail}</dd>
                    </Stack>
                </Stack>
            </NavLink>
            <Divider />
            <Stack direction='row' spacing={5} flexWrap="wrap">
                {requestData.isRegularVisit ? (<>
                        <dl className={styles.property_wrapper}>
                            <dt>Visit Dates</dt>
                            {requestData.regularVisitDates.map((visit) => {
                                const {date} = formatDateTime(visit.visitStartDateTime);
                                return <dd key={visit.id}>{date}</dd>;
                            })}
                        </dl>
                        <dl className={styles.property_wrapper}>
                            <dt>Time</dt>
                            <dd>{formatDateTime(requestData.requestedDateTime).time}</dd>
                        </dl>
                    </>
                ) : (
                    <>
                        <dl className={styles.property_wrapper}>
                            <dt>Date</dt>
                            <dd>{formatDateTime(requestData.requestedDateTime).date}</dd>
                        </dl>
                        <dl className={styles.property_wrapper}>
                            <dt>Time</dt>
                            <dd>{formatDateTime(requestData.requestedDateTime).time}</dd>
                        </dl>
                    </>
                )}
                <dl className={styles.property_wrapper}>
                    <dt>District</dt>
                    <dd>{requestData.district}</dd>
                </dl>
                <dl className={styles.property_wrapper}>
                    <dt>Type</dt>
                    <dd>{requestData.requestType}</dd>
                </dl>
                <dl className={styles.property_wrapper}>
                    <dt>Address</dt>
                    <dd>{requestData.visitAddress}</dd>
                </dl>

            </Stack>
            <Divider/>
            <dl className={styles.property_wrapper__text}>
                <dt>Description</dt>
                <dd>{requestData.requestDescription}</dd>
            </dl>
        </Stack>
    );
}
