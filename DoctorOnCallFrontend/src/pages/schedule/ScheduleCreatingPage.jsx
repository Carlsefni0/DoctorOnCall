import StyledPaper from "../../components/StyledPaper.jsx";
import styles from './schedule.module.css';
import {Divider, Snackbar, Stack} from "@mui/material";
import { useState } from "react";
import StyledTextField from "../../components/StyledTextField.jsx";
import StyledButton from "../../components/StyledButton.jsx";
import { useMutation } from "@tanstack/react-query";
import ScheduleForm from "./ScheduleForm.jsx";
import ReturnButton from "../../components/return-button/ReturnButton.jsx";
import {Alert} from "@mui/lab";
import {queryClient} from "../../utils/queryClient.js";
import performRequest from "../../utils/performRequest.js";

export default function ScheduleCreatingPage() {
    const [scheduleName, setScheduleName] = useState("");
    const [scheduleDays, setScheduleDays] = useState([]);
    const [isEditing, setIsEditing] = useState(false);
    const baseUrl = import.meta.env.VITE_BACKEND_URL;
    

    const createMutation = useMutation({
        mutationFn: () => performRequest(`${baseUrl}/schedule/create`, 'POST', { scheduleName, scheduleDays }),
        onError: (error) => {
            console.log(error);
            
        },
        onSuccess: () => {
            queryClient.invalidateQueries(['/schedule']);
            setScheduleName('');
            setScheduleDays([]);
        }
    });
    
    const handleCreateSchedule = () => {
        createMutation.mutate()
    }

    const handleSnackbarClose = () => {
        createMutation.reset();
    };

    return (
        <StyledPaper width='50%' marginLeft='15rem' height='90%'>
            <div className={styles.header}>
                <ReturnButton/>
                <h2>Creating new schedule</h2>
            </div>
            <div className={styles.content}>
                <StyledTextField
                    placeholder='Schedule name'
                    width='58%'
                    value={scheduleName}
                    label="Schedule name"
                    onChange={(e) => setScheduleName(e.target.value)}
                    
                />
                <Divider><h3>Schedule days</h3></Divider>

                <ScheduleForm
                    scheduleDays={scheduleDays}
                    setScheduleDays={setScheduleDays}
                    isEditing
                    setIsEditing={setIsEditing}
                    displayCancel={false}
                />

                <Stack direction='row' spacing={2}>
                    {scheduleDays.length !== 0 && <StyledButton width='13rem' onClick={handleCreateSchedule} disabled={!scheduleName}>
                        Save Schedule
                    </StyledButton>}
                    
                </Stack>
            </div>
            <Snackbar open={createMutation.isSuccess} autoHideDuration={3000} onClose={handleSnackbarClose} >
                <Alert severity="success" >
                    The schedule was created successfully!
                </Alert>
            </Snackbar>
            <Snackbar open={createMutation.isError} autoHideDuration={3000} onClose={handleSnackbarClose} >
                <Alert severity="error" >
                    {createMutation?.error?.message}
                </Alert>
            </Snackbar>
        </StyledPaper>
    );
}
