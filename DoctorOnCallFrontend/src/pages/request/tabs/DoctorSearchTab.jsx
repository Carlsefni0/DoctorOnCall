import {useCallback, useState} from "react";
import { Stack, CircularProgress, Snackbar, Alert } from "@mui/material";
import { useQuery, useMutation } from "@tanstack/react-query";
import StyledButton from "../../../components/StyledButton.jsx";
import DoctorItem from "../../../components/doctor_item/DoctorItem.jsx";
import performRequest from "../../../utils/performRequest.js";
import { queryClient } from "../../../utils/queryClient.js";
import styles from "../request_details.module.css";
import {getUser} from "../../../context/UserContext.jsx";

export default function DoctorSearchTab({ requestData}) {
    const [selectedDoctor, setSelectedDoctor] = useState(null);
    const baseUrl = import.meta.env.VITE_BACKEND_URL;
    const {user} = getUser();


    const { data: doctors, isLoading: isLoadingDoctors } = useQuery({
        queryKey: ["doctors", requestData.id],
        queryFn: () => performRequest(`${baseUrl}/doctor/find/${requestData.id}`),

    });

    const { data: assignedDoctor, isLoading: isLoadingAssignedDoctor, refetch: refetchAssignedDoctor } = useQuery({
        queryKey: ["assignedDoctor", requestData.id],
        queryFn: () => performRequest(`${baseUrl}/doctor/assigned/${requestData.id}`),
        enabled: !!requestData.id, 
    });




    const assignDoctor = useMutation({
        mutationFn: () => performRequest(`${baseUrl}/visit-request/approve/${requestData.id}/doctor/${selectedDoctor}`, "POST"),
        onSuccess: (approvedRequestData) => {
            queryClient.invalidateQueries(["visit-request", requestData.id, "assignedDoctor", "doctors"]);
            console.log(approvedRequestData)
        },
    });
    
    const resolveRequest = useMutation({
        mutationFn: (action) => {
            const url = `${baseUrl}/visit-request/${action}/${requestData.id}`;
            return performRequest(url, 'PATCH');
        }, 
        onSuccess: () => {
            queryClient.invalidateQueries(["visit-request", "assignedDoctor", "doctors",requestData.id])
        }
        
    })
    
    const handleResolveRequest = (action) => {
        resolveRequest.mutate(action);
        if(action === 'decline') window.history.back();
        
    }
    
    const handleSelectDoctor = (doctorId) => {
        setSelectedDoctor(prev => (prev === doctorId ? null : doctorId));
    };

    const handleAssignDoctor = () => {
        assignDoctor.mutate();
    };

    const handleSnackbarClose = () => {
        assignDoctor.reset();
    };

    return (
        <Stack padding='2rem' height='55vh'>
            <h2>
                {!assignedDoctor && doctors && doctors.length > 0 && "The system has found suitable doctors. Choose a doctor to visit the patient."}
                {assignedDoctor && user.role !== 'Doctor' && "Assigned doctor for this visit request:"}
                {assignedDoctor && user.role === 'Doctor' && "You was assigned to this visit request"}
                
            </h2>

            {(isLoadingDoctors) && (
                <Stack alignItems="center" justifyContent="center" height='100%'>
                    <CircularProgress sx={{color: "#088172"}} size="5rem"/>
                </Stack>
            )}

            {!assignedDoctor && doctors && doctors.length > 0 && (
                <ul className={styles.items_container__doctors} type="none">
                    {doctors.map((doctor) => (
                        <li key={doctor.id} onClick={() => handleSelectDoctor(doctor.id)}>
                            <DoctorItem doctor={doctor} isSelected={doctor.id === selectedDoctor}/>
                        </li>
                    ))}
                </ul>
            )}

            {!assignedDoctor && doctors && doctors.length === 0 && 
            <div className={styles.message}>
                There are no doctors available for this visit request.
            </div>}

            {assignedDoctor && (
               <Stack direction='column' alignItems='center' justifyContent='center' height='45vh' spacing='10rem'>
                   <DoctorItem doctor={assignedDoctor} isSelected big/>
                   {requestData.status !== 'Approved' && user.role === 'Doctor' && user.id === assignedDoctor?.userId &&
                       <Stack direction='row' spacing={4}>
                           <StyledButton onClick={()=>handleResolveRequest("approve")}>Accept</StyledButton>
                           <StyledButton onClick={()=>handleResolveRequest("decline")}>Decline</StyledButton>
                       </Stack>
                   }
               </Stack>
            )}


            {user.role === 'Admin' && !assignedDoctor && doctors && doctors.length > 0 && (
                <Stack alignItems="center" marginTop="0.5rem">
                    <StyledButton disabled={!selectedDoctor} size="small" loading={assignDoctor.isPending}
                                  onClick={handleAssignDoctor}>
                        Assign Doctor
                    </StyledButton>
                </Stack>
            )}


            <Snackbar open={assignDoctor.isSuccess} autoHideDuration={3000} onClose={handleSnackbarClose}>
                <Alert severity="success">The doctor was assigned successfully!</Alert>
            </Snackbar>
            <Snackbar open={resolveRequest.isSuccess} autoHideDuration={3000} onClose={handleSnackbarClose}>
                <Alert severity="success">The visit request was resolved successfully!</Alert>
            </Snackbar>
        </Stack>
    );
}
