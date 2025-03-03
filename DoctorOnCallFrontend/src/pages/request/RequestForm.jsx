import StyledPaper from "../../components/StyledPaper.jsx";
import styles from "../../styles/record_form.module.css";
import requestStyles from "./request_form.module.css";
import {Divider, RadioGroup, FormControl, FormControlLabel, Stack, Snackbar, Alert} from "@mui/material";
import StyledTextField from "../../components/StyledTextField.jsx";
import StyledAutoComplete from "../../components/StyledAutoComplete.jsx";
import {useEffect, useState} from "react";
import StyledRadio from "../../components/StyledRadio.jsx";
import performRequest from "../../utils/performRequest.js";
import {useMutation, useQuery} from "@tanstack/react-query";
import StyledButton from "../../components/StyledButton.jsx";
import {queryClient} from "../../utils/queryClient.js";
import {useLoaderData} from "react-router-dom";

const visitTypes = [
    { label: "Сonsultation", value: 0 },
    { label: "Examination", value: 1 },
    { label: "Procedures", value: 2 },
    { label: "Remote Visit", value: 3 },
];

export default function RequestForm() {
    const initialData = useLoaderData();
    const isEditMode = Boolean(initialData);
    const [date, setDate] = useState("");
    const [time, setTime] = useState("");
    const [interval, setInterval] = useState(1);
    const [occurrences, setOccurrences] = useState(1);
    const [visitType, setVisitType] = useState(null);
    const [selectedValue, setSelectedValue] = useState("false");
    const [searchTerm, setSearchTerm] = useState("");
    const [selectedMedicine, setSelectedMedicine] = useState(null)
    const [selectedMedicines, setSelectedMedicines] = useState({})
    const [quantity, setQuantity] = useState(1);
    const [validationErrors, setValidationErrors] = useState({});
    const [description, setDescription] = useState('')
    
    const baseUrl = import.meta.env.VITE_BACKEND_URL;
    const saveRequestEndpoint = `${baseUrl}/visit-request/${isEditMode ? `${initialData.id}` : "create"}`;
    const savePatientMethod = isEditMode ? "PUT" : "POST";
    
    const { data: medications = [], isLoading } = useQuery({
            queryKey: ["medicines", searchTerm],
            queryFn: () => performRequest(`${baseUrl}/medicine/find/${searchTerm}`),
            enabled: !!searchTerm,
            staleTime: 1000 * 60 * 5,
        }
    );

    const mutation = useMutation( {
        mutationFn: (data) => performRequest(saveRequestEndpoint, savePatientMethod, data),
        onSuccess: () => {
            if(!isEditMode) resetForm(); 
            queryClient.invalidateQueries(['visit-request'])
            setValidationErrors({})
        },
        onError: (error) => {
            console.log(error);
            if (error.errors) {
                setValidationErrors(error.errors);
            } else {
                setValidationErrors({ general: ["An unexpected error occurred"] });
            }
        },
    });
    const cancelMutation = useMutation( {
        mutationFn: (data) => performRequest(`${baseUrl}/visit-request/cancel/${initialData.id}`, 'PATCH'),
        onSuccess: () => {
            queryClient.invalidateQueries(['visit-request'])
            window.history.back();
        },
        onError:(e) => console.log(e),

    });
  
    const { data: medicines} = useQuery({
        queryKey: ["medicines", initialData?.id],
        queryFn: () => performRequest(`${baseUrl}/medicine/${initialData.id}`),
        enabled: !!initialData,
        staleTime: 1000 * 60 * 5,
    });

    useEffect(() => {
        if (initialData) {
            const [initialDate, initialTime] = initialData.requestedDateTime.split("T");
            setDate(initialDate);
            setTime(initialTime.substring(0, 5));
            setVisitType(visitTypes.find((type) => type.label === initialData.requestType));
            setSelectedValue(initialData.isRegularVisit ? "true" : "false");
            setInterval(initialData.interval || 1);
            setOccurrences(initialData.occurrences || 1)
            setDescription(initialData.requestDescription || '')
        }
    }, [initialData]);
    
    console.log(visitType)

    useEffect(() => {
        if (medicines && medicines.length) {
            const initialSelectedMedicines = {};
            medicines.forEach((medicine) => {
                initialSelectedMedicines[medicine.id] = {
                    ...medicine,
                    quantity: medicine.quantity,
                };
            });
            setSelectedMedicines(initialSelectedMedicines);
        }
    }, [medicines]);

    const resetForm = () => {
        setDate("");
        setTime("");
        setInterval(0);
        setOccurrences(0);
        setVisitType(null);
        setDescription('');
        setSelectedValue("false");
        setSearchTerm("");
        setSelectedMedicine(null)
        setSelectedMedicines({})
        setQuantity(1);
        
    }
    
    
    const handleSubmit = (e) => {
        e.preventDefault();

        const requestedMedicines = {};
        Object.entries(selectedMedicines).forEach(([id, medicine]) => {
            requestedMedicines[id] = medicine.quantity;
        });

        const requestData = {
            requestedDateTime: `${date}T${time}`,
            requestType: visitType?.value || 0,
            isRegularVisit: selectedValue === "true",
            regularVisitIntervalDays: interval,
            regularVisitOccurrences: occurrences,
            requestDescription: description,
            requestedMedicines,
        };

        mutation.mutate(requestData);
    };

    const handleSelectRadio = (e) => {
        setSelectedValue(e.target.value);
    };

    const handleAddMedicine = () => {
        if (selectedMedicine && quantity > 0) {
            setSelectedMedicines((prev) => ({
                ...prev,
                [selectedMedicine.id]: {
                    ...selectedMedicine,
                    quantity,
                },
            }));
            setSelectedMedicine(null);
            setQuantity(1);
        }
    };
    
    const handleRemoveMedicine = (id) => {
        setSelectedMedicines((prev) => {
            const updated = { ...prev };
            delete updated[id];
            return updated;
        });
    };

    const handleCloseSnackbar = () => {
        mutation.reset();
        cancelMutation.reset();
    }
    
    return (
        <StyledPaper width="50%" marginLeft="15rem" height="95%">
            <form className={styles.form_wrapper} onSubmit={handleSubmit}>
                <div className={styles.fields_wrapper}>
                    <Divider>Visit Information</Divider>
                    <Stack direction='row' spacing={4}>
                        <StyledTextField
                            label="Date"
                            name="requestedDate"
                            type="date"
                            required
                            InputLabelProps={{ shrink: true }}
                            inputProps={{min: new Date(Date.now()).toISOString().split("T")[0]}}
                            value={date}
                            onChange={(e) => setDate(e.target.value)}
                            error={Boolean(validationErrors.RequestedDateTime)}
                            helperText={validationErrors.RequestedDateTime?.[0]}
                        />
                        <StyledTextField
                            label="Time"
                            name="requestedTime"
                            type="time"
                            required
                            InputLabelProps={{ shrink: true }}
                            value={time}
                            onChange={(e) => setTime(e.target.value)}
                        />
                    </Stack>
                    <StyledAutoComplete
                        options={visitTypes}
                        getOptionLabel={(option) => option.label}
                        value={visitType}
                        onChange={(event, newValue) => setVisitType(newValue)}
                        renderInput={(params) => (
                            <StyledTextField
                                {...params}
                                name="type"
                                label="Visit type"
                                variant="outlined"
                                required
                            />
                        )}
                    />
                    <Stack justifyContent='center' alignItems="center">
                        <FormControl component="fieldset">
                            <p>Is it a regular visit?</p>
                            <RadioGroup defaultValue={'false'} value={selectedValue} onChange={handleSelectRadio} row>
                                <FormControlLabel
                                    value="true"
                                    control={<StyledRadio/>}
                                    label="Yes"
                                />
                                <FormControlLabel
                                    value="false"
                                    control={<StyledRadio/>}
                                    label="No"
                                />
                            </RadioGroup>
                            
                        </FormControl>
                    </Stack>
                    {selectedValue === "true" && <Stack direction='row' spacing={2}>
                        <StyledTextField
                            type="number"
                            required
                            label="Interval"
                            value={interval}
                            inputProps={{min: 1, max: 31}}
                            onChange={(e) => setInterval(e.target.value)}
                        />
                        <StyledTextField
                            type="number"
                            required
                            value={occurrences}
                            label="Occurrences"
                            inputProps={{min: 1, max: 8}}
                            onChange={e =>setOccurrences(e.target.value)}
                        />
                    </Stack>
                    }
                    <StyledTextField
                        height="10vh"
                        placeholder="Type here"
                        multiline
                        rows={3}
                        label={description?.length === 0 ? 'Description' : `${description?.length}/200`}
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        error={description.length >= 200}
                        required
                    />
                    
                    <Divider>Medicines</Divider>
                    <Stack direction='row' spacing={2}>
                        <StyledAutoComplete
                            options={medications}
                            getOptionLabel={(option) => option.name}
                            value={selectedMedicine}
                            onChange={(event, newValue) => setSelectedMedicine(newValue)}
                            renderInput={(params) => (
                                <StyledTextField
                                    {...params}
                                    label="Select medicine"
                                    variant="outlined"
                                    onChange={(e) => setSearchTerm(e.target.value)}
                                />
                            )}
                            renderOption={(props, option) => (
                                <div
                                    {...props}
                                    className={requestStyles.medicine_option}
                                >
                                    <Stack direction='row' alignItems='center' spacing={1}>
                                        <img
                                            src={option.imageUrl}
                                            alt={option.name}
                                            style={{width: 40, height: 40, borderRadius: 4}}
                                        />
                                        <p>{option.name}</p>
                                    </Stack>
                                    <p>{option.unitPrice}$</p>
                                </div>
                            )}
                            loading={isLoading}
                        />
                        <StyledTextField
                            label="Quantity"
                            type="number"
                            value={quantity}
                            width='30%'
                            onChange={(e) => setQuantity(Math.max(1, Number(e.target.value)))}
                            inputProps={{min: 1}}
                        />
                        <StyledButton variant="contained" onClick={handleAddMedicine} disabled={!selectedMedicine}>
                            Add
                        </StyledButton>
                    </Stack>

                    <ul className={requestStyles.medicine_container}>
                        {Object.entries(selectedMedicines).map(([id, medicine]) => (
                            <li
                                className={requestStyles.medicine_item}
                                key={id}
                                onClick={() => handleRemoveMedicine(id)}
                            >
                                <Stack direction='row' alignItems='center' spacing={1}>
                                    <img src={medicine?.imageUrl || ''} alt="medicine image"/>
                                    <Stack spacing="0.1rem">
                                        <p className={requestStyles.medicine_name}>{medicine?.name}</p>
                                        <p className={requestStyles.medicine_price}>{medicine?.unitPrice}$</p>
                                    </Stack>
                                </Stack>
                                <div className={requestStyles.medicine_quantity}>{medicine.quantity}</div>
                            </li>
                        ))}
                    </ul>

                </div>
                <Stack direction='row' spacing={4} justifyContent='center'>
                    <StyledButton onClick={()=>window.history.back()}>Cancel</StyledButton>
                    {isEditMode && initialData.status !== 'Cancelled' &&
                        <StyledButton onClick={()=>cancelMutation.mutate()} loading={cancelMutation.isPending}>
                            {cancelMutation.isPending ? 'Cancelling..' : 'Cancel request'}
                        </StyledButton>
                    }
                    <StyledButton type='submit' loading={mutation.isPending} disabled={description.length >= 200}>{mutation.isPending ? 'Saving..' : 'Save'}</StyledButton>
                    
                </Stack>
            </form>
            <Snackbar open={mutation.isSuccess} autoHideDuration={3000} onClose={handleCloseSnackbar}>
                <Alert severity="success">The visit request was saved successfully!</Alert>
            </Snackbar>
            <Snackbar open={cancelMutation.isSuccess} autoHideDuration={3000} onClose={handleCloseSnackbar}>
                <Alert severity="success">The visit request was cancelled successfully!</Alert>
            </Snackbar>
            <Snackbar open={mutation.isError || cancelMutation.isError} autoHideDuration={3000} onClose={handleCloseSnackbar}>
                <Alert severity="error">{mutation.error?.message || 'An unexpected error occured!'}</Alert>
            </Snackbar>
        </StyledPaper>
    );
}
