import { useState } from "react";
import { Divider, IconButton, Stack } from "@mui/material";
import StyledTextField from "../../components/StyledTextField.jsx";
import StyledButton from "../../components/StyledButton.jsx";
import StyledAutocomplete from "../../components/StyledAutocomplete.jsx";
import DeleteIcon from '@mui/icons-material/Delete';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import styles from './schedule.module.css';
import {getUser} from "../../context/UserContext.jsx";

const daysOfWeekMap = {
    0: "Sunday",
    1: "Monday",
    2: "Tuesday",
    3: "Wednesday",
    4: "Thursday",
    5: "Friday",
    6: "Saturday",
};


export default function ScheduleForm({ scheduleDays, setScheduleDays, isEditing, setIsEditing, displayCancel }) {
    const [newDay, setNewDay] = useState({ dayOfWeek: null, startTime: "08:00:00", endTime: "18:00:00" });
    const [startIndex, setStartIndex] = useState(0);
    const {user} = getUser();

    const handleAddDay = () => {

        if (scheduleDays.some(day => day.dayOfWeek === newDay.dayOfWeek)) {
            alert("This day is already added!");
            return;
        }

        setScheduleDays((prev) => [...prev, newDay].sort((a, b) => a.dayOfWeek - b.dayOfWeek));
        setNewDay({ dayOfWeek: null, startTime: "08:00:00", endTime: "18:00:00" });
    };

    const handleDeleteDay = (dayToDelete) => {
        setScheduleDays((prev) => prev.filter(day => day.dayOfWeek !== dayToDelete));
    };

    const handleNext = () => {
        if (startIndex + 3 < scheduleDays.length) setStartIndex(startIndex + 1);
    };

    const handlePrev = () => {
        if (startIndex > 0) setStartIndex(startIndex - 1);
    };
    
    const handleCancel = () => {
        setNewDay({ dayOfWeek: null, startTime: "08:00:00", endTime: "18:00:00" });
        setIsEditing(false);
    }

    const handleEditing = () => {
        setIsEditing(true);
    }


    return (
        <>
            {scheduleDays.length !== 0 ? (
                <Stack direction='row' alignItems='center'>
                    <IconButton onClick={handlePrev} disabled={startIndex === 0}>
                        <ArrowBackIcon/>
                    </IconButton>
                    <div className={styles.days_container}>
                        <ul className={styles.days_list}>
                            {scheduleDays.slice(startIndex, startIndex + 3).map((day, index) => (
                                <li key={index} className={styles.day}>
                                    <div className={styles.day_name}>
                                        {daysOfWeekMap[day.dayOfWeek]}
                                        {isEditing && <IconButton size="small" onClick={() => handleDeleteDay(day.dayOfWeek)}>
                                            <DeleteIcon/>
                                        </IconButton>}
                                    </div>
                                    <p className={styles.working_hours}>{day.startTime.slice(0,5)} - {day.endTime.slice(0,5)}</p>
                                </li>
                            ))}

                        </ul>
                    </div>
                    <IconButton onClick={handleNext} disabled={startIndex + 3 >= scheduleDays.length}>
                        <ArrowForwardIcon/>
                    </IconButton>
                </Stack>
            ) : (
                <div>The list of days is empty. The schedule must contain at least 1 day</div>
            )}

            <Divider />
            {isEditing &&  <Stack spacing={2} sx={{ width: '60%' }}>
                <StyledAutocomplete
                    options={Object.keys(daysOfWeekMap)
                        .map(Number)
                        .filter(day => !scheduleDays.some(d => d.dayOfWeek === day))}
                    value={newDay.dayOfWeek}
                    onChange={(_, value) => setNewDay((prev) => ({ ...prev, dayOfWeek: value }))}
                    renderInput={(params) => <StyledTextField {...params} label="Day of week" />}
                    getOptionLabel={(option) => daysOfWeekMap[option]}
                />
                <Stack direction='row' spacing={4}>
                    <StyledTextField type="time" label="Start time" value={newDay.startTime} onChange={(e) => setNewDay(prev => ({...prev, startTime: e.target.value +':00'}))} />
                    <StyledTextField type="time" label="End time" value={newDay.endTime} onChange={(e) => setNewDay(prev => ({...prev, endTime: e.target.value+':00'}))} />
                </Stack>
            </Stack>}
           
            
            <Stack direction='row' spacing={2}>

                {isEditing && <StyledButton onClick={handleAddDay} width='6rem' disabled={newDay.dayOfWeek === null}>Add</StyledButton>}
                {displayCancel && isEditing && <StyledButton onClick={handleCancel} width='6rem'>Cancel</StyledButton>}
                {!isEditing && user.role === 'Admin' && <StyledButton onClick={handleEditing} width='6rem'>Edit</StyledButton>}
                
            </Stack>
           
        </>
    );
}
