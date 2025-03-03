import { Alert, Dialog, Snackbar, Stack } from "@mui/material";
import StyledTextField from "../../../components/StyledTextField.jsx";
import { useEffect, useState } from "react";
import StyledButton from "../../../components/StyledButton.jsx";
import styles from "./visit_info_tab.module.css";
import { useMutation, useQuery } from "@tanstack/react-query";
import performRequest from "../../../utils/performRequest.js";
import { queryClient } from "../../../utils/queryClient.js";
import VisitInfo from "./VisitInfo.jsx";
import {getUser} from "../../../context/UserContext.jsx";

const formatDateTimeForServer = (date) => {
    return date.getFullYear() +
        "-" + String(date.getMonth() + 1).padStart(2, '0') +
        "-" + String(date.getDate()).padStart(2, '0') +
        "T" + String(date.getHours()).padStart(2, '0') +
        ":" + String(date.getMinutes()).padStart(2, '0') +
        ":" + String(date.getSeconds()).padStart(2, '0');
};

export default function VisitInfoTab({ requestData }) {
    const { id: visitRequestId } = requestData;
    const {user} = getUser();
    const baseUrl = import.meta.env.VITE_BACKEND_URL;
    const [message, setMessage] = useState('');
    const [notifyAction, setNotifyAction] = useState('');
    const [activeAction, setActiveAction] = useState(user.role === 'Doctor' ? 'report' : 'visits');
    const [validationErrors, setValidationErrors] = useState({});


    const today = new Date().toDateString();

    let visitStartDateTime = null;
    let isTodayReported;
    
    if (requestData.isRegularVisit) {
        const nextUnreportedVisit = requestData.regularVisitDates.find(visit => !visit.isReported);
        if (nextUnreportedVisit) {
            visitStartDateTime =nextUnreportedVisit.visitStartDateTime;
        }
        
        isTodayReported = Boolean(requestData.regularVisitDates.find((visit) => new Date(visit.visitStartDateTime).toDateString() === today && visit.isReported));
        
        
    } else {
        visitStartDateTime = requestData.requestedDateTime;
    }

    const canReport = new Date(visitStartDateTime).toDateString() === today;
    const [startTime, setStartTime] = useState(new Date(visitStartDateTime).toLocaleTimeString());
    const [endTime, setEndTime] = useState();
    const [notes, setNotes] = useState("");
    const canComplete = notes?.length !== 0 && notes?.length <= 200;
    const isCompleted = requestData.status === 'Completed';
    const isCancelled = requestData.status === 'Cancelled';
    

    const completeMutation = useMutation({
        mutationFn: async () => {
            const actualStartDateTime = new Date(visitStartDateTime);
            if (startTime) {
                const [startHours, startMinutes] = startTime.split(":").map(Number);
                actualStartDateTime.setHours(startHours, startMinutes, 0, 0);
            }

            const actualEndDateTime = new Date(visitStartDateTime);
            if (endTime) {
                const [endHours, endMinutes] = endTime.split(":").map(Number);
                actualEndDateTime.setHours(endHours, endMinutes, 0, 0);
            }
            
            return await performRequest(`${baseUrl}/visit/complete/${visitRequestId}`, "PATCH", {
                ActualVisitStartDateTime: formatDateTimeForServer(actualStartDateTime),
                ActualVisitEndDateTime: formatDateTimeForServer(actualEndDateTime),
                Notes: notes.trim()
            });
        },
        onSuccess: () => {
            queryClient.invalidateQueries(['visit', 'visit-request', visitRequestId]);
        },
        onError: (error) => {
            setValidationErrors(error.errors || { general: ["An unexpected error occurred"] });
        },
    });


    const notifyMutation = useMutation({
        mutationFn: async () => {
            const endpoint = notifyAction === 'delay' ? `delay/${visitRequestId}` : `cancel/${visitRequestId}`;
            const payload = notifyAction === 'delay'
                ? { DelayReason: message }
                : { CancellationReason: message };

            return await performRequest(`${baseUrl}/visit/${endpoint}`, notifyAction === 'delay' ? "POST" : "PATCH", payload);
        },
        onSuccess: () => {
            setNotifyAction('');
            setMessage('');
            queryClient.invalidateQueries(['visit', visitRequestId]);
        },
    });
    
    const canSend = message.length < 100;

    return (
        <Stack  alignItems='center' width='100%'>
            {activeAction === 'visits' && <VisitInfo visitRequestId={visitRequestId} />}
            {activeAction === 'report' &&
            <Stack  width='100%' spacing={3} alignItems='center' height='63vh'>
                <h2>
                    {canReport && !isCompleted &&  "Fill the form about the visit"} 
                    {!canReport && !isTodayReported &&  "You can report about the visit only in the day of it"}
                    {isTodayReported || isCompleted && "You have already reported"}
                </h2>
                <Stack direction='row' justifyContent='center' alignItems='start' spacing={6} width='80%' height='10vh'>
                    <StyledTextField type="time" label="Start Time" value={startTime} InputLabelProps={{ shrink: true }}
                                     disabled={!canReport || isCompleted || isCancelled} onChange={(e) => setStartTime(e.target.value)} required/>
                    <StyledTextField type="time" label="End Time" value={endTime} InputLabelProps={{ shrink: true }}
                                     disabled={!canReport || isCompleted || isCancelled} onChange={(e) => setEndTime(e.target.value)}
                                     error={Boolean(validationErrors.ActualVisitEndDateTime)}
                                     helperText={validationErrors.ActualVisitEndDateTime?.[0]} required/>
                </Stack>

                <StyledTextField
                    width='80%'
                    height="50vh"
                    required
                    multiline
                    rows={8}
                    label={notes.length === 0 ? 'Notes' : `${notes.length}/200`}
                    value={notes}
                    onChange={(e) => setNotes(e.target.value)}
                    error={notes.length >= 200}
                    disabled={isCompleted || !canReport || isCancelled}

                />

                {!isCompleted && canReport && <Stack direction='row' spacing={2}>
                    <StyledButton loading={completeMutation.isPending} onClick={() => completeMutation.mutate()}
                                  disabled={!canComplete}>
                        Complete Visit
                    </StyledButton>
                    <StyledButton onClick={() => setNotifyAction('delay')}>
                        I will be late
                    </StyledButton>
                    <StyledButton backgroundColor='red' hover='#b01221' onClick={() => setNotifyAction('cancel')}>
                        Cancel Visit
                    </StyledButton>
                </Stack>}
            </Stack>}
            {user.role === 'Doctor' &&
            <StyledButton variant='text'  color='#25AF9DFF' size='small' width='4rem' onClick={()=>setActiveAction(activeAction === 'report' ? 'visits' : 'report')}>
                {activeAction === 'report' ? 'Visits' : 'Report'}
            </StyledButton>}
            <Dialog open={!!notifyAction} width='40rem'>
                <div className={styles.dialog_wrapper}>
                    <h2>{notifyAction === 'delay'
                        ? 'Leave a message for the patient and we will notify him'
                        : "Are you sure you want to cancel the visit? The patient will be notified"}
                    </h2>
                    <StyledTextField
                        height="20rem"
                        label={message.length === 0 ? 'Message' : `${message.length}/100`}
                        multiline
                        rows={5}
                        value={message}
                        onChange={(e) => setMessage(e.target.value)}
                        error={message.length > 100}
                    />
                    <Stack direction='row' spacing={4}>
                        <StyledButton onClick={() => notifyMutation.mutate()} width='6rem' 
                                      disabled={!canSend || message.length === 0 || notifyMutation.isPending }>Send</StyledButton>
                        <StyledButton onClick={() => setNotifyAction('')} width='6rem'>Cancel</StyledButton>
                    </Stack>
                </div>
            </Dialog>

            <Snackbar open={completeMutation.isSuccess} autoHideDuration={3000}
                      onClose={() => completeMutation.reset()}>
                <Alert severity="success">The visit was completed successfully!</Alert>
            </Snackbar>
            <Snackbar open={notifyMutation.isSuccess} autoHideDuration={3000}
                      onClose={() => notifyMutation.reset()}>
                <Alert severity="success">The patient will be notified</Alert>
            </Snackbar>
    </Stack>
    );
}