import { useMutation } from "@tanstack/react-query";
import { useRef, useState } from "react";
import {Divider,Snackbar, Stack} from "@mui/material";
import { Alert } from "@mui/lab";
import {queryClient} from "../../utils/queryClient.js";
import StyledTextField from "../../components/StyledTextField.jsx";
import StyledButton from "../../components/StyledButton.jsx";
import StyledPaper from "../../components/StyledPaper.jsx";
import StyledAutoComplete from "../../components/StyledAutoComplete.jsx";
import districts from "../../data/districts.js";
import styles from "../../styles/record_form.module.css";
import {useLoaderData, useNavigate} from "react-router-dom";
import AddressAutocomplete from "../../components/AddressAutoComplete.jsx";
import performRequest from "../../utils/performRequest.js";


const genders = [
    { label: "Male", value: 0 },
    { label: "Female", value: 1 },
];
export default function PatientForm() {
    const initialData = useLoaderData();
    const isEditMode = Boolean(initialData);
    const [patientData, setPatientData] = useState(initialData || {});
    const [passwordMismatch, setPasswordMismatch] = useState(false);
    const [gender, setGender] = useState();
    const [disease, setDisease] = useState(initialData?.disease || "");
    const [validationErrors, setValidationErrors] = useState({});
    const [address, setAddress] = useState(patientData.address || "");
    const [isDeleteMode, setIsDeleteMode] = useState(false)
    const formRef = useRef(null);
    const navigate = useNavigate();
    const baseUrl = import.meta.env.VITE_BACKEND_URL;
    
    const savePatientEndpoint = `${baseUrl}/patient/${isEditMode ? `${patientData.id}` : "register"}`;
    const savePatientMethod = isEditMode ? "PUT" : "POST";

    const mutation = useMutation({
        mutationFn: (data) => performRequest(savePatientEndpoint, savePatientMethod, data),
        onSuccess: () => {
            setValidationErrors({});
            queryClient.invalidateQueries(['/patients', patientData.id]);
            if(!isEditMode) resetForm()
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
    const deleteMutation = useMutation({
        mutationFn: (action) => {
            const deletePatientEnpoint = `${baseUrl}/patient/${action === 'disable' ? `disable${patientData.id}` : patientData.id}`
            const deletePatientMethod = action === 'disable' ? 'PUT' : 'Delete';
            
            performRequest(deletePatientEnpoint, deletePatientMethod);
        },
        onSuccess: () => {
            queryClient.invalidateQueries(["/patients"]);
            navigate('../')
        },

    })

    const handleReturnBack = () => {
        window.history.back();
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        const formData = new FormData(formRef.current);

        const password = formData.get("password");
        const repeatPassword = formData.get("repeatPassword");

        if (password !== repeatPassword) {
            setPasswordMismatch(true);
        } else {
            setPasswordMismatch(false);
        } 

        const newPatientData = {
            id: initialData?.id,
            firstName: formData.get("firstName"),
            lastName: formData.get("lastName"),
            gender: gender.value,
            email: formData.get("email"),
            password: formData.get("password") || 'ItsVeryBadDecision1@!',
            phoneNumber: formData.get("phoneNumber"),
            dateOfBirth: formData.get("dateOfBirth"),
            address: address,
            disease: disease,
            district: formData.get("district"),
        };

        setPatientData(newPatientData);
        mutation.mutate(newPatientData);
    };

    const resetForm = () => {
        setPatientData({});
        setAddress("");
        setPasswordMismatch(false);
        setValidationErrors({});
        setDisease("")
        setGender(null)
        if (formRef.current) {
            formRef.current.reset();
        }

        setPatientData({
            gender: null,
            specialization: null,
            address: null,
            district: null,
            disease: null,
        });
    };

    const handleDeleteMode = () => {
        setIsDeleteMode(!isDeleteMode);
    }

    const handleDelete = () => {
        deleteMutation.mutate('delete')
    }

    const handleDisable = () => {
        deleteMutation.mutate('disable');
    }

    return (
        <StyledPaper width="50%" marginLeft="15rem" height='95%'>
            <form key={JSON.stringify(patientData)} ref={formRef} className={styles.form_wrapper} onSubmit={handleSubmit} autocomplete="off">
                <div className={styles.fields_wrapper}>
                    <Divider>Personal information</Divider>
                    <div className={styles.form_row}>
                        <StyledTextField
                            label="First name"
                            required
                            name="firstName"
                            error={Boolean(validationErrors.FirstName)}
                            helperText={validationErrors.FirstName?.[0]}
                            defaultValue={patientData.firstName || ""}
                        />
                        <StyledTextField
                            label="Last name"
                            required
                            name="lastName"
                            error={Boolean(validationErrors.LastName)}
                            helperText={validationErrors.LastName?.[0]}
                            defaultValue={patientData.lastName || ""}
                        />
                    </div>
                    <div className={styles.form_row}>
                        <StyledAutoComplete
                            options={genders}
                            getOptionLabel={(option) => option.label}
                            value={gender}
                            // defaultValue={patientData.gender !== undefined ? (patientData.gender === 0 ? "Male" : "Female") : ""}
                            renderInput={(params) => (
                                <StyledTextField
                                    {...params}
                                    name="gender"
                                    label="Gender"
                                    variant="outlined"
                                    required
                                />
                            )}
                            onChange={(event, newValue) => setGender(newValue)}
                        />

                        <StyledTextField
                            label="Birthday"
                            required
                            type="date"
                            name="dateOfBirth"
                            InputLabelProps={{shrink: true}}
                            error={Boolean(validationErrors.DateOfBirth)}
                            helperText={validationErrors.DateOfBirth?.[0]}
                            defaultValue={patientData.dateOfBirth ? patientData.dateOfBirth.split('T')[0] : ""}
                            inputProps={{
                                max: new Date().toISOString().split('T')[0], type: "date"
                            }}
                        />
                    </div>
                    <div className={styles.form_row}>
                        <StyledAutoComplete
                            options={districts}
                            defaultValue={patientData.district || null}
                            renderInput={(params) => (
                                <StyledTextField
                                    {...params}
                                    name="district"
                                    label="District"
                                    variant="outlined"
                                    required
                                    error={Boolean(validationErrors.District)}
                                    helperText={validationErrors.District?.[0]}
                                />
                            )}
                        />
                        <AddressAutocomplete
                            defaultValue={patientData.address || ""}
                            onAddressSelect={(address) => setAddress(address)}
                            error={Boolean(validationErrors.Address)}
                            helperText={validationErrors.Address?.[0]}
                        />
                    </div>
                    
                    {!isEditMode && <div className={styles.form_row}>
                        <StyledTextField
                            label="Password"
                            required
                            type="password"
                            name="password"
                            error={Boolean(validationErrors.Password || passwordMismatch)}
                            helperText={validationErrors.Password?.[0]}
                        />
                        <StyledTextField
                            label="Repeat password"
                            required
                            type="password"
                            name="repeatPassword"
                            error={Boolean(validationErrors.Password) || passwordMismatch}
                            helperText={passwordMismatch && 'Password must match'}
                        />
                    </div>}
                    {!isEditMode && <>
                        <Divider>Contact information</Divider>
                        <div className={styles.form_row}>
                            <StyledTextField
                                label="Email"
                                required
                                type="email"
                                name="email"
                                error={Boolean(validationErrors.Email)}
                                helperText={validationErrors.Email?.[0]}
                                defaultValue={patientData.email || ""}
                            />
                            <StyledTextField
                                label="Phone number"
                                name="phoneNumber"
                                error={Boolean(validationErrors.PhoneNumber)}
                                helperText={validationErrors.PhoneNumber?.[0]}
                                defaultValue={patientData.phoneNumber || ""}
                            />
                        </div>
                    </>}
                    <Divider>Medical information</Divider>
                    <div className={styles.form_row}>
                        <StyledTextField
                            height="40vh"
                            placeholder="Type here"
                            multiline
                            rows={3}
                            label={disease.length === 0 ? 'Disease' : `${disease.length}/200`}
                            value={disease}
                            onChange={(e) => setDisease(e.target.value)}
                            error={disease.length >= 200}
                            required
                        />
                       
                    </div>

                </div>

                <div className={styles.buttons_wrapper}>
                    <Stack spacing={2} alignItems='center'>
                        {isDeleteMode && <p>You can disable user account or delete it forever.</p>}
                        <Stack direction='row' spacing={5} justifyContent='space-around'>
                            <StyledButton width="7rem"
                                          onClick={isDeleteMode ? handleDeleteMode : handleReturnBack}>Cancel</StyledButton>
                            {!isDeleteMode && <StyledButton
                                type="submit"
                                variant="contained"
                                loading={mutation.isPending}
                                loadingPosition="start"
                                width="7rem"
                                disabled={disease.length >= 200}
                            >
                                {isEditMode ? "Update" : "Register"}
                            </StyledButton>}
                            {isEditMode && <StyledButton width="7rem" backgroundColor='red' hover='#b01221'
                                                         onClick={isDeleteMode ? handleDelete : handleDeleteMode}>
                                Delete</StyledButton>}
                            {isDeleteMode && <StyledButton width="7rem" onClick={handleDisable}>Disable</StyledButton>}
                        </Stack>
                    </Stack>
                </div>
            </form>
            <Snackbar open={mutation.isSuccess} autoHideDuration={3000} onClose={() => mutation.reset()}>
                <Alert severity="success">
                    {isEditMode ? "Patient details updated successfully!" : "Patient registered successfully!"}
                </Alert>
            </Snackbar>
            <Snackbar open={deleteMutation.isError} autoHideDuration={3000} onClose={() => deleteMutation.reset()}>
                <Alert severity="error">An unexpected error occurred. Try again later</Alert>
            </Snackbar>
        </StyledPaper>
    );
}
