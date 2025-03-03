import StyledPaper from "../../components/StyledPaper.jsx";
import styles from './schedule_exception_details.module.css'
import ReturnButton from "../../components/return-button/ReturnButton.jsx";
import EditButton from "../../components/edit_button/EditButton.jsx";
import {NavLink, useLoaderData} from "react-router-dom";
import {Alert, Divider, Snackbar, Stack} from "@mui/material";
import StyledAvatar from "../../components/StyledAvatar.jsx";
import stringAvatar from "../../utils/stringToAvatar.js";
import StyledButton from "../../components/StyledButton.jsx";
import {useMutation, useQuery} from "@tanstack/react-query";
import performRequest from "../../utils/performRequest.js";
import {queryClient} from "../../utils/queryClient.js";
import {getUser} from "../../context/UserContext.jsx";

const visitStatusColors = {
    Pending: "orange",
    Approved: "green",
    Rejected: "red",
    Completed: "#2bcc56",
    Cancelled: "#2b4bcc",
};
export default function ScheduleExceptionDetailPage() {
    const initialData = useLoaderData();
    const baseUrl = import.meta.env.VITE_BACKEND_URL;
    
    const endpointUrl = `${baseUrl}/schedule-exception/${initialData.id}`;
    const {user} = getUser();

    const { data: scheduleException} = useQuery({
        queryKey: ['schedule-exception', initialData.id],
        queryFn: () => performRequest(endpointUrl),
        initialData,
        keepPreviousData: true,
    });
    
    const mutation = useMutation({
        mutationFn: (status)=> performRequest(`${endpointUrl}?status=${status}`, 'PATCH'),
        onSuccess: () => queryClient.invalidateQueries(['schedule-exception', initialData.id])
    });

    const doctorFullName = `${scheduleException.doctorFirstName} ${scheduleException.doctorLastName}`
    const startDate = scheduleException?.startDate
        ? new Date(scheduleException.startDate).toISOString().split("T")[0]
        : "N/A";

    const endDate = scheduleException?.endDate
        ? new Date(scheduleException.endDate).toISOString().split("T")[0]
        : "N/A";


    return( 
    <StyledPaper width='70%' marginLeft='10rem' height='95%'>
        <div className={styles.header}>
            <ReturnButton />
            <h2>Schedule exception</h2>
            {user.role !== 'Admin' && <EditButton recordId={scheduleException.id}/>}
        </div>
        <Stack direction='row' justifyContent='end'>
                <span style={{ color: visitStatusColors[scheduleException.status] }} className={styles.status}>
                    {scheduleException.status}
                </span>
        </Stack>
        <div className={styles.content}>
            <NavLink to={`../../doctors/${scheduleException.doctorId}`}
                     className={`${styles.property_wrapper} ${styles.nav_link}`}>
                <label htmlFor="patientFullName">Doctor</label>
                <Stack direction='row' spacing={1} alignItems='center'>
                    <StyledAvatar {...stringAvatar(doctorFullName)} width='4rem' height='4rem'/>
                    <Stack direction='column' spacing={1}>
                        <dd id="patientFullName">{doctorFullName}</dd>
                        <dd className={styles.email}>{scheduleException.doctorEmail}</dd>
                    </Stack>
                </Stack>
            </NavLink>
            <Divider/>
            <Stack direction='row' spacing={5}>
                <dl className={styles.property_wrapper}>
                    <dt>Start Date</dt>
                    <dd>{startDate}</dd>
                </dl>
                <dl className={styles.property_wrapper}>
                    <dt>End Date</dt>
                    <dd>{endDate}</dd>
                </dl>
                <dl className={styles.property_wrapper}>
                    <dt>Exception Type</dt>
                    <dd>{scheduleException.type}</dd>
                </dl>
            </Stack>
            <Divider/>
            <dl className={styles.property_wrapper}>
                <dt>Reason</dt>
                <dd>{scheduleException.reason}</dd>
            </dl>
        </div>
      
        <Stack direction='row' spacing={4} justifyContent='center'>
            {user.role === 'Admin' && scheduleException.status === 'Pending' && <>
                <StyledButton width='6rem' onClick={()=> mutation.mutate(1)} loading={mutation.isPending}>
                    Approve
                </StyledButton>
                <StyledButton width='6rem' onClick={()=> mutation.mutate(2)} loading={mutation.isPending}>
                    Reject
                </StyledButton>
            </> }
            {user.role === 'Doctor' && scheduleException.status === 'Pending' && 
                <StyledButton width='6rem' onClick={()=> mutation.mutate(3)} loading={mutation.isPending}>
                    Cancel
                </StyledButton>}
        </Stack>
        <Snackbar open={mutation.isSuccess} autoHideDuration={3000} onClose={()=>mutation.reset()}>
            <Alert severity="success">The schedule exception status has been updated</Alert>
        </Snackbar>
        <Snackbar open={mutation.isError} autoHideDuration={3000} onClose={()=> mutation.reset()}>
            <Alert severity="error">An unexpected error occurred. Try again later</Alert>
        </Snackbar>
        
    </StyledPaper>)
}