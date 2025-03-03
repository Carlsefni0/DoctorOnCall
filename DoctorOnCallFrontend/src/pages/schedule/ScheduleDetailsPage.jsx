import StyledPaper from "../../components/StyledPaper.jsx";
import styles from './schedule.module.css';
import {Divider, Snackbar, Stack} from "@mui/material";
import { useState } from "react";
import StyledTextField from "../../components/StyledTextField.jsx";
import StyledButton from "../../components/StyledButton.jsx";
import {useLoaderData, useNavigate} from "react-router-dom";
import { useMutation } from "@tanstack/react-query";
import ScheduleForm from "./ScheduleForm.jsx";
import ReturnButton from "../../components/return-button/ReturnButton.jsx";
import {Alert} from "@mui/lab";
import {queryClient} from "../../utils/queryClient.js";
import performRequest from "../../utils/performRequest.js";

export default function ScheduleDetailsPage() {
    const scheduleData = useLoaderData();
    const [scheduleName, setScheduleName] = useState(scheduleData.scheduleName || "");
    const [scheduleDays, setScheduleDays] = useState(scheduleData.scheduleDays || []);
    const [isEditing, setIsEditing] = useState(false);
    const baseUrl = import.meta.env.VITE_BACKEND_URL;
    const navigate = useNavigate();

    const updateMutation = useMutation({
        mutationFn: () => performRequest(`${baseUrl}/schedule/${scheduleData.scheduleId}`, 'PUT', { scheduleName, scheduleDays }),
        onSuccess: ()=> {
            queryClient.invalidateQueries(['schedule']);
        }
    });

    const deleteMutation = useMutation({
        mutationFn: () => performRequest(`${baseUrl}/schedule/${scheduleData.scheduleId}`, 'DELETE'),
        onSuccess: ()=> {
            queryClient.invalidateQueries(['schedule']);
            navigate('..');
        },
    });
    
    const handleUpdateSchedule = () => {
        if(scheduleDays.length === 0){
            deleteMutation.mutate();
        }
        updateMutation.mutate();
    }
    const handleDeleteSchedule = () => {
        deleteMutation.mutate()
    }

    const handleSnackbarClose = () => {
        updateMutation.reset();
    };
    return (
        <StyledPaper width='50%' marginLeft='15rem' height='95%'>
            <div className={styles.header}>
                <ReturnButton/>
                <h2>Schedule details</h2>
            </div>
            <div className={styles.content}>
                <StyledTextField
                    placeholder='Schedule name'
                    width='58%'
                    value={scheduleName}
                    label={scheduleName.length === 0 || !isEditing ? "Schedule name" : `${scheduleName.length}/50`}
                    error={scheduleName.length>50}
                    onChange={(e) => setScheduleName(e.target.value)}
                    disabled={!isEditing}
                />
                <Divider><h3>Schedule days</h3></Divider>

                <ScheduleForm
                    scheduleDays={scheduleDays}
                    setScheduleDays={setScheduleDays}
                    isEditing={isEditing}
                    setIsEditing={setIsEditing}
                    displayCancel={true}
                />
                {isEditing &&
                    <Stack spacing={1}>
                        {<StyledButton width='13rem' onClick={handleUpdateSchedule} loading={updateMutation.isPending}
                                       loadingPosition="start" disabled={scheduleName.length>50}>
                            Update Schedule
                        </StyledButton>}
                        <StyledButton width='13rem' onClick={handleDeleteSchedule} loading={deleteMutation.isPending}>
                            Delete Schedule
                        </StyledButton>
                    </Stack>
                }
                <Snackbar open={updateMutation.isSuccess} autoHideDuration={3000} onClose={handleSnackbarClose} >
                    <Alert severity="success" >
                        The schedule was updated successfully!
                    </Alert>
                </Snackbar>
                <Snackbar open={updateMutation.isError || deleteMutation.isError} autoHideDuration={3000} onClose={handleSnackbarClose} >
                    <Alert severity="error" >
                        {updateMutation?.error?.message}
                    </Alert>
                </Snackbar>
            </div>
        </StyledPaper>
    );
}
