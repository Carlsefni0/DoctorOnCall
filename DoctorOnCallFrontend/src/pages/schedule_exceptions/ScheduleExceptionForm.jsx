import { useState, useRef } from "react";
import { useMutation } from "@tanstack/react-query";
import { Divider, Stack, Snackbar } from "@mui/material";
import { Alert } from "@mui/lab";
import StyledPaper from "../../components/StyledPaper.jsx";
import StyledTextField from "../../components/StyledTextField.jsx";
import StyledAutoComplete from "../../components/StyledAutoComplete.jsx";
import StyledButton from "../../components/StyledButton.jsx";
import styles from "../../styles/record_form.module.css";
import performRequest from "../../utils/performRequest.js";
import { queryClient } from "../../utils/queryClient.js";
import { useLoaderData } from "react-router-dom";

const exceptionTypes = [
    { label: "Vacation", value: 0 },
    { label: "Sick leave", value: 1 },
    { label: "Training", value: 2 },
    { label: "Emergency", value: 3 },
    { label: "Other", value: 4 }
];

const emptyData = {
    startDate: "",
    endDate: "",
    type: null,
    reason: ""
};

export default function ScheduleExceptionForm() {
    const initialData = useLoaderData();

    const transformedInitialData = initialData
        ? {
            ...initialData,
            type: exceptionTypes.find((type) => type.label === initialData.type) || null,
        }
        : emptyData;

    const [exceptionData, setExceptionData] = useState(transformedInitialData);
    const [validationErrors, setValidationErrors] = useState({});
    const formRef = useRef(null);
    const baseUrl = import.meta.env.VITE_BACKEND_URL;
    const saveExceptionEndpoint = `${baseUrl}/schedule-exception/create`;
    const updateExceptionStatusEndpoint = `${baseUrl}/schedule-exception/${exceptionData.id}`;

    const formatDateForInput = (date) => {
        return date ? new Date(date).toISOString().split('T')[0] : "";
    };

    const formatDateForServer = (date) => {
        return date ? new Date(date).toISOString() : "";
    };

    const saveExceptionMutation = useMutation({
        mutationFn: (data) => {
            const formattedData = {
                ...data,
                startDate: formatDateForServer(data.startDate),
                endDate: formatDateForServer(data.endDate),
                type: data.type.value,
            };
            return performRequest(saveExceptionEndpoint, "POST", formattedData);
        },
        onSuccess: () => {
            setValidationErrors({});
            queryClient.invalidateQueries(["/schedule-exception"]);
            resetForm();
        },
        onError: (error) => {
            setValidationErrors(error.errors || { general: ["An unexpected error occurred"] });
        },
    });

    const updateExceptionStatusMutation = useMutation({
        mutationFn: (status)=> performRequest(`${updateExceptionStatusEndpoint}?status=${status}`, 'PATCH'),
        onSuccess: () => queryClient.invalidateQueries(['/schedule-exception', initialData.id])
    });

    const handleSubmit = (e) => {
        e.preventDefault();
        saveExceptionMutation.mutate(exceptionData);
    };
    
    const handleCloseSnackBar = () => {
        saveExceptionMutation.reset();
        updateExceptionStatusMutation.reset();
    }

    const resetForm = () => {
        setExceptionData(emptyData);
        setValidationErrors({});
        if (formRef.current) {
            formRef.current.reset();
        }
    };

    return (
        <StyledPaper width="50%" marginLeft="15rem" height="95%">
            <form ref={formRef} className={styles.form_wrapper} onSubmit={handleSubmit}>
                <div className={styles.fields_wrapper}>
                    <Divider>Exception Info</Divider>
                    <div className={styles.form_row}>
                        <StyledTextField
                            label="Start Date"
                            type="date"
                            required
                            name="startDate"
                            value={formatDateForInput(exceptionData.startDate)}
                            onChange={(e) =>
                                setExceptionData({ ...exceptionData, startDate: e.target.value })
                            }
                            InputLabelProps={{ shrink: true }}
                            inputProps={{ min: new Date().toISOString().split('T')[0] }}
                            error={Boolean(validationErrors.startDate)}
                            helperText={validationErrors.startDate?.[0]}
                        />
                        <StyledTextField
                            label="End Date"
                            type="date"
                            required
                            name="endDate"
                            value={formatDateForInput(exceptionData.endDate)}
                            onChange={(e) =>
                                setExceptionData({ ...exceptionData, endDate: e.target.value })
                            }
                            InputLabelProps={{ shrink: true }}
                            inputProps={{ min: new Date().toISOString().split('T')[0] }}
                            error={Boolean(validationErrors.endDate)}
                            helperText={validationErrors.endDate?.[0]}
                        />

                    </div>
                    <div className={styles.form_row}>
                        <StyledAutoComplete
                            options={exceptionTypes}
                            getOptionLabel={(option) => option.label}
                            value={exceptionData.type}
                            onChange={(event, newValue) =>
                                setExceptionData({ ...exceptionData, type: newValue })
                            }
                            renderInput={(params) => (
                                <StyledTextField
                                    {...params}
                                    name="type"
                                    label="Exception Type"
                                    variant="outlined"
                                    required
                                    error={Boolean(validationErrors.type)}
                                    helperText={validationErrors.type?.[0]}
                                />
                            )}
                        />
                    </div>
                    <StyledTextField
                        height="40vh"
                        required
                        multiline
                        rows={10}
                        label={exceptionData.reason.length === 0 ? "Reason" : `${exceptionData.reason.length}/200`}
                        value={exceptionData.reason}
                        onChange={(e) =>
                            setExceptionData({ ...exceptionData, reason: e.target.value })
                        }
                        error={exceptionData.reason.length >= 200}
                        helperText={
                            exceptionData.reason.length >= 200 ? "Reason cannot exceed 200 characters" : ""
                        }
                    />
                </div>
                <Stack direction="row" justifyContent="center" spacing={2}>
                    <StyledButton type="submit" loading={saveExceptionMutation.isPending} disabled={exceptionData.reason.length > 200}>
                        Submit
                    </StyledButton>
                    <StyledButton onClick={()=>window.history.back()}>Cancel</StyledButton>
                    {initialData && 
                        <StyledButton onClick={()=>updateExceptionStatusMutation.mutate(3)} disabled={updateExceptionStatusMutation.isPending}>
                        Undo
                    </StyledButton>}
                    
                </Stack>
            </form>
            <Snackbar open={saveExceptionMutation.isSuccess} autoHideDuration={3000} onClose={handleCloseSnackBar}>
                <Alert severity="success">Exception was created successfully!</Alert>
            </Snackbar>
            <Snackbar open={updateExceptionStatusMutation.isSuccess} autoHideDuration={3000} onClose={handleCloseSnackBar}>
                <Alert severity="success">Exception was canceled successfully!</Alert>
            </Snackbar>
            <Snackbar open={saveExceptionMutation.isError || updateExceptionStatusMutation.isError} autoHideDuration={3000} onClose={handleCloseSnackBar}>
                <Alert severity="error">An unexpected error occurred. Try again later</Alert>
            </Snackbar>
        </StyledPaper>
    );
}