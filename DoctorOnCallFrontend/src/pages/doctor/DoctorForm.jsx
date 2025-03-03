import { useMutation } from "@tanstack/react-query";
import { useRef, useState} from "react";
import {Divider, Snackbar, Stack} from "@mui/material";
import {useLoaderData, useNavigate} from "react-router-dom";
import { Alert } from "@mui/lab";
import { queryClient } from "../../utils/queryClient.js";
import StyledTextField from "../../components/StyledTextField.jsx";
import StyledButton from "../../components/StyledButton.jsx";
import StyledPaper from "../../components/StyledPaper.jsx";
import StyledAutoComplete from "../../components/StyledAutoComplete.jsx";
import specializations from "../../data/specializations.js";
import hospitals from "../../data/hospitals.js";
import districts from "../../data/districts.js";
import styles from "../../styles/record_form.module.css";
import performRequest from "../../utils/performRequest.js";

export default function DoctorForm() {
    const initialData = useLoaderData();
    const isEditMode = Boolean(initialData);
    const [doctorData, setDoctorData] = useState(initialData || {});
    const [passwordMismatch, setPasswordMismatch] = useState(false);
    const [isDeleteMode, setIsDeleteMode] = useState(false)
    const [validationErrors, setValidationErrors] = useState({});
    const formRef = useRef(null);
    const navigate = useNavigate();
    const baseUrl = import.meta.env.VITE_BACKEND_URL;

    const saveDoctorEndpoint = `${baseUrl}/doctor/${isEditMode ? `${doctorData.id}` : "register"}`
    const saveDoctorMethod = isEditMode ? "PUT" : "POST"
    
  
    const mutation = useMutation({
        mutationFn: (data) => performRequest(saveDoctorEndpoint, saveDoctorMethod, data),
        onSuccess: () => {
            setValidationErrors({});
            queryClient.invalidateQueries(["/doctors", doctorData.id]);
            if (!isEditMode) resetForm();
        },
        onError: (error) => {
            setValidationErrors(error.errors || { general: ["An unexpected error occurred"] });
        },
    });

    const deleteMutation = useMutation({
        mutationFn: (action) => {
            const deleteDoctorEndpoint = `${baseUrl}/doctor/${action === 'disable' ? `disable${doctorData.userId}` : doctorData.userId}`
            const deleteDoctorMethod = action === 'disable' ? 'PUT' : 'DELETE'
            
            performRequest(deleteDoctorEndpoint,deleteDoctorMethod);
        },
        onSuccess: () => {
            navigate('../');
            queryClient.invalidateQueries(["/doctors"]);
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

        const newDoctorData = {
            id: initialData?.id,
            firstName: formData.get("firstName"),
            lastName: formData.get("lastName"),
            gender: formData.get("gender") === "Male" ? 0 : 1,
            email: formData.get("email"),
            password: formData.get("password") || 'ItsVeryBadDecision1@!',
            phoneNumber: formData.get("phoneNumber"),
            specialization: formData.get("specialization"),
            hospital: formData.get("hospital"),
            workingDistrict: formData.get("workingDistrict"),
            
        };
        
        setDoctorData(newDoctorData);
        mutation.mutate(newDoctorData);
    };

    const resetForm = () => {
        setDoctorData({});
        setPasswordMismatch(false);
        setValidationErrors({});

        if (formRef.current) {
            formRef.current.reset();
        }

        setDoctorData({
            gender: null,
            specialization: null,
            hospital: null,
            workingDistrict: null,
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
            <form key={JSON.stringify(doctorData)} ref={formRef} autocomplete="off" className={styles.form_wrapper} onSubmit={handleSubmit}>
                <div className={styles.fields_wrapper}>
                    <Divider>Personal information</Divider>
                    <div className={styles.form_row}>
                        <StyledTextField
                            label="First name"
                            required name="firstName"
                            error={Boolean(validationErrors.FirstName)}
                            helperText={validationErrors.FirstName?.[0]}
                            defaultValue={doctorData.firstName || ""}/>
                        <StyledTextField
                            label="Last name"
                            required name="lastName"
                            error={Boolean(validationErrors.LastName)}
                            helperText={validationErrors.LastName?.[0]}
                            defaultValue={doctorData.lastName || ""}/>
                    </div>
                    <div className={styles.form_row}>
                        <StyledAutoComplete
                            options={['Male', 'Female']}
                            defaultValue={doctorData.gender !== undefined ? (doctorData.gender === 0 ? "Male" : "Female") : ""}
                            renderInput={(params) => (
                                <StyledTextField
                                    {...params}
                                    name="gender"
                                    label="Gender"
                                    variant="outlined"
                                    required
                                    error={Boolean(validationErrors.Gender)}
                                    helperText={validationErrors.Gender?.[0]}
                                />
                            )}
                            onChange={(_, value) => setDoctorData((prev) => ({
                                ...prev,
                                gender: value === "Male" ? 0 : 1
                            }))}
                        />
                    </div>
                    {!isEditMode && <div className={styles.form_row}>
                        <StyledTextField
                            label="Password"
                            required
                            name="password"
                            type="password"
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
                    <Divider>Contact information</Divider>
                    <div className={styles.form_row}>
                        <StyledTextField
                            label="Email"
                            required
                            type="email"
                            name="email"
                            error={Boolean(validationErrors.Email)}
                            helperText={validationErrors.Email?.[0]}
                            defaultValue={doctorData.email || ""}
                        />
                        <StyledTextField
                            label="Phone number"
                            name="phoneNumber"
                            error={Boolean(validationErrors.PhoneNumber)}
                            helperText={validationErrors.PhoneNumber?.[0]}
                            defaultValue={doctorData.phoneNumber || ""}
                        />
                    </div>
                    <Divider>Professional information</Divider>
                    <div className={styles.form_row}>
                        <StyledAutoComplete
                            options={specializations}
                            defaultValue={doctorData.specialization || null}
                            renderInput={(params) => (
                                <StyledTextField {...params}
                                           name="specialization"
                                           label="Specialization"
                                           variant="outlined"
                                           required error={Boolean(validationErrors.Specialization)}
                                           helperText={validationErrors.Specialization?.[0]}
                                />
                            )}
                        />
                        <StyledAutoComplete
                            options={hospitals}
                            defaultValue={doctorData.hospital || null}
                            renderInput={(params) => (
                                <StyledTextField {...params} name="hospital"
                                           label="Hospital" variant="outlined"
                                           required
                                           error={Boolean(validationErrors.Hospital)}
                                           helperText={validationErrors.Hospital?.[0]}/>)}/>
                    </div>
                    <div className={styles.form_row}>
                        <StyledAutoComplete
                            options={districts}
                            defaultValue={doctorData.workingDistrict || null}
                            renderInput={(params) => (
                                <StyledTextField
                                    {...params}
                                    name="workingDistrict"
                                    label="Working district"
                                    variant="outlined"
                                    required
                                    error={Boolean(validationErrors.District)}
                                    helperText={validationErrors.District?.[0]}
                                />
                            )}
                        />
                 
                    </div>
                </div>
                <div className={styles.buttons_wrapper}>
                    <Stack spacing={2} alignItems='center'>
                        {isDeleteMode && <p>You can disable user account or delete it forever.</p>}
                        <Stack direction='row' spacing={5} justifyContent='space-around'>
                            <StyledButton width="7rem" onClick={isDeleteMode ? handleDeleteMode : handleReturnBack}>Cancel</StyledButton>
                            {!isDeleteMode && <StyledButton
                                type="submit"
                                variant="contained"
                                loading={mutation.isPending}
                                loadingPosition="start"
                                width="7rem"
                            >
                                {isEditMode ? "Update" : "Register"}
                            </StyledButton>}
                            {isEditMode && <StyledButton width="7rem" backgroundColor='red' hover='#b01221' onClick={isDeleteMode ? handleDelete : handleDeleteMode}>
                                Delete
                            </StyledButton>}
                            {isDeleteMode && <StyledButton width="7rem" onClick={handleDisable}>Disable</StyledButton>}
                        </Stack>
                    </Stack>
                </div>
            </form>
            <Snackbar open={mutation.isSuccess} autoHideDuration={3000} onClose={() => mutation.reset()}>
                <Alert severity="success">{isEditMode ? "Doctor details updated successfully!" : "Doctor registered successfully!"}</Alert>
            </Snackbar>
            <Snackbar open={deleteMutation.isError} autoHideDuration={3000} onClose={() => deleteMutation.reset()}>
                <Alert severity="error">An unexpected error occurred. Try again later</Alert>
            </Snackbar>
            <Snackbar open={deleteMutation.isSuccess} autoHideDuration={3000} onClose={() => deleteMutation.reset()}>
                <Alert severity="error">An unexpected error occurred. Try again later</Alert>
            </Snackbar>
        </StyledPaper>
    );
}
