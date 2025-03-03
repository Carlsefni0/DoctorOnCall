import StyledPaper from "../../components/StyledPaper.jsx";
import styles from './schedules.module.css';
import { NavLink, useLoaderData, useNavigate, useSearchParams } from "react-router-dom";
import CustomSpeedDial from "../../components/CustomSpeedDial.jsx";
import AddIcon from '@mui/icons-material/Add';
import { useMutation, useQuery } from "@tanstack/react-query";
import StyledPagination from "../../components/StyledPagination.jsx";
import { CircularProgress, Dialog, IconButton, Snackbar, Stack } from "@mui/material";
import RemoveCircleOutlineIcon from '@mui/icons-material/RemoveCircleOutline';
import {useEffect, useState} from "react";
import { queryClient } from "../../utils/queryClient.js";
import StyledButton from "../../components/StyledButton.jsx";
import performRequest from "../../utils/performRequest.js";
import Alert from "@mui/material/Alert";
import StyledTextField from "../../components/StyledTextField.jsx";
import StyledAutoComplete from "../../components/StyledAutoComplete.jsx";
import {getUser} from "../../context/UserContext.jsx";

const daysOfWeekMap = {
    0: "Sun", 1: "Mon", 2: "Tue", 3: "Wed", 4: "Thu", 5: "Fri", 6: "Sat"
};

export default function SchedulesPage() {
    const initialData = useLoaderData();
    const navigate = useNavigate();
    const [searchParams, setSearchParams] = useSearchParams();
    const [scheduleToRemove, setScheduleToRemove] = useState(null);
    const [selectedSchedule, setSelectedSchedule] = useState(null);
    const currentPage = parseInt(searchParams.get("page") || "1", 10);
    const baseUrl = import.meta.env.VITE_BACKEND_URL;


    const {user} = getUser();

    let doctorId;

    useEffect(() => {
        if (user?.role === "Admin" && !doctorId) {
            doctorId = user.id;
        }
    }, [user, doctorId]);

    const filtersFromUrl = Object.fromEntries(searchParams.entries());

    if (doctorId && !filtersFromUrl.doctorId) {
        filtersFromUrl.doctorId = doctorId;
    }


    const { data, isLoading } = useQuery({
        queryKey: ['schedule', currentPage, filtersFromUrl],
        queryFn: () => {
            const apiBaseUrl = import.meta.env.VITE_BACKEND_URL;
            const queryParams = new URLSearchParams({
                PageNumber: currentPage,
                PageSize: 8,
                ...filtersFromUrl,
            });
            const url = `${apiBaseUrl}/schedule?${queryParams.toString()}`;
            return performRequest(url, "GET");
        },
        initialData: currentPage === 1 ? initialData : undefined,
        keepPreviousData: true,
    });

    const adminActions = [{
        icon: <AddIcon />, name: "New Schedule", tooltipTitle: "Create new schedule", onClick: () => navigate("create")
    }];
    
    const { data: schedulesToAssign } = useQuery({
        queryKey: ['schedulesToAssign', filtersFromUrl.doctorId],
        queryFn: () => performRequest(`${baseUrl}/schedule/names`),
        enabled: Boolean(filtersFromUrl.doctorId)
    });

    const assignedSchedules = data?.items?.map(schedule => schedule.scheduleId) || [];
    const availableSchedules = schedulesToAssign?.filter(schedule => !assignedSchedules.includes(schedule.id)) || [];

    const removeMutation = useMutation({
        mutationFn: () => performRequest(`${baseUrl}/schedule/remove/${scheduleToRemove}/doctor/${filtersFromUrl.doctorId}`, 'POST'),
        onSuccess: () => {
            queryClient.invalidateQueries(['schedule', currentPage, filtersFromUrl]);
            setScheduleToRemove(null);
        }
    });

    const assignMutation = useMutation({
        mutationFn: () => performRequest(`${baseUrl}/schedule/assign/${selectedSchedule}/doctor/${filtersFromUrl.doctorId}`, 'POST'),
        onSuccess: () => {
            queryClient.invalidateQueries(['schedule', currentPage, filtersFromUrl]);
            setSelectedSchedule(null);
        }
    });

    const handlePageChange = (event, newPage) => {
        setSearchParams({ page: newPage.toString() });
    };
    
    
    const handleRemoveSchedule = () => {
        if (scheduleToRemove) {
            removeMutation.mutate();
        }
    };
   
    const handleSnackbarClose = () => {
        removeMutation.reset();
        assignMutation.reset();
    };

    return (
        <>
            <StyledPaper width='90%' marginLeft='2.5rem' height="80%">
                <div className={styles.header}><h2>Schedules</h2></div>

                {isLoading ? (
                    <Stack alignItems="center" marginTop='2rem'><CircularProgress /></Stack>
                ) : (
                    <>
                        <Stack justifyContent="center" spacing={2} direction='row' marginTop='1.5rem'>
                            {user.role ==='Admin' && availableSchedules.length > 0 && (
                                <>
                                    <StyledAutoComplete
                                        width='25%'
                                        height='2.75rem'
                                        options={availableSchedules.map(schedule => ({ label: schedule.scheduleName, value: schedule.id }))}
                                        onChange={(event, newValue) => setSelectedSchedule(newValue?.value || null)}
                                        renderInput={(params) => <StyledTextField {...params} name="schedule" label="Schedule" variant="outlined" required />}
                                    />
                                    <StyledButton width='6rem' size='small' onClick={()=>assignMutation.mutate()} disabled={!selectedSchedule}>Assign</StyledButton>
                                </>
                            )}
                        </Stack>
                        <ul className={styles.schedules_list}>
                            {data?.items?.length > 0 ? data.items.map((schedule) => (
                                <li key={schedule.scheduleId} className={styles.schedule}>
                                    <NavLink to={`${schedule.scheduleId}`} className={styles.nav_link}>
                                        <p className={styles.schedule_header}>{schedule.scheduleName}</p>
                                        <ul className={styles.days_list}>{schedule.scheduleDays.map((day) => (
                                            <li key={day.dayOfWeek} className={styles.day}>{daysOfWeekMap[day.dayOfWeek]}</li>
                                        ))}</ul>
                                    </NavLink>
                                    {filtersFromUrl.doctorId && (
                                        <Stack alignItems='center' marginTop={2}>
                                            <IconButton size="small" onClick={() => setScheduleToRemove(schedule.scheduleId)} sx={{ height: '1rem', width: '1rem' }}>
                                                <RemoveCircleOutlineIcon/>
                                            </IconButton>
                                        </Stack>
                                    )}
                                </li>
                            )) : <Stack width="100%" justifyContent="center" height='35vh' alignItems="center">
                                <h2>{filtersFromUrl.doctorId ? "The doctor doesn't have any assigned schedule": "No schedules available"}</h2>
                            </Stack>}
                        </ul>
                    </>
                )}

                <CustomSpeedDial actions={user.role === 'Admin' ? adminActions : []} />
            </StyledPaper>

            <Stack spacing={2} alignItems="center" marginTop='2rem' marginLeft='2.5rem' width='90%'>
                <StyledPagination count={data?.totalPages || 1} page={currentPage} onChange={handlePageChange} color="primary" showFirstButton showLastButton />
            </Stack>

            <Dialog open={Boolean(scheduleToRemove)}>
                <div className={styles.dialog_wrapper}>
                    <h2>Are you sure you want to remove this schedule from the doctor?</h2>
                    <Stack direction='row' spacing={4}>
                        <StyledButton onClick={()=>setScheduleToRemove(null)} width='6rem'>Cancel</StyledButton>
                        <StyledButton onClick={handleRemoveSchedule} backgroundColor='red' hover='#b01221' width='6rem' loading={removeMutation.isPending}>Remove</StyledButton>
                    </Stack>
                </div>
            </Dialog>
            <Snackbar open={removeMutation.isSuccess} autoHideDuration={3000} onClose={handleSnackbarClose}>
                <Alert severity="success">The schedule was removed successfully</Alert>
            </Snackbar>
            <Snackbar open={assignMutation.isSuccess} autoHideDuration={3000} onClose={handleSnackbarClose}>
                <Alert severity="success">The schedule was assigned successfully</Alert>
            </Snackbar>
            <Snackbar open={assignMutation.isError} autoHideDuration={3000} onClose={handleSnackbarClose}>
                <Alert severity="error">{assignMutation.error?.message}</Alert>
            </Snackbar>
        </>
    );
}
