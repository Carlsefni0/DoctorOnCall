import { useState, useEffect } from "react";
import { Stack, Snackbar, Alert } from "@mui/material";
import StyledButton from "../../../components/StyledButton.jsx";
import StyledTextField from "../../../components/StyledTextField.jsx";
import { useMutation } from "@tanstack/react-query";
import performRequest from "../../../utils/performRequest.js";
import { queryClient } from "../../../utils/queryClient.js";
import styles from "../request_details.module.css";

export default function RejectTab({ requestData}) {
    const [rejectionReason, setRejectionReason] = useState(requestData.rejectionReason || "");
    const [canReject, setCanReject] = useState(false);

    useEffect(() => {
        setCanReject(rejectionReason.length > 0 && rejectionReason.length <= 200);
    }, [rejectionReason]);

    const baseUrl = import.meta.env.VITE_BACKEND_URL;

    const rejectMutation = useMutation({
        mutationFn: () => performRequest(`${baseUrl}/visit-request/reject/${requestData.id}`, "PATCH", { rejectionReason }),
        onSuccess: () => {
            queryClient.invalidateQueries(['visit-request', requestData.id]);
        },
        onError: (error) => {
            setValidationErrors(error.errors || { general: ["An unexpected error occurred"] });
        },
    });

    return (
        <Stack alignItems='center' gap='1.5rem' padding='2rem' height='55vh'>
            {requestData.status === 'Pending' && <>
                <h2>Are you sure you want to reject this visit request?</h2>
                <p>If you want to reject the visit request, you have to specify the reason for rejection</p>
            </>}

            {requestData.status === 'Rejected' && <h2>The visit request was rejected</h2>}

            <StyledTextField
                height="40vh"
                placeholder="Type here"
                multiline
                rows={10}
                label={rejectionReason.length === 0 ? 'Reason' : `${rejectionReason.length}/200`}
                value={rejectionReason}
                onChange={(e) => setRejectionReason(e.target.value)}
                error={rejectionReason.length >= 200}
                required
                disabled={requestData.status !== "Pending"}
            />
            {requestData.status === "Pending" && <StyledButton
                loading={rejectMutation.isLoading}
                disabled={!canReject || requestData.status === "Rejected"}
                onClick={() => rejectMutation.mutate()}
                width="20%"
            >
                Reject
            </StyledButton>}

            <Snackbar open={rejectMutation.isSuccess} autoHideDuration={3000} onClose={()=>rejectMutation.reset()}>
                <Alert severity="success">The visit request was rejected successfully!</Alert>
            </Snackbar>
            <Snackbar open={rejectMutation.isError} autoHideDuration={3000} onClose={()=>rejectMutation.reset()}>
                <Alert severity="error">An unexpected error occured!</Alert>
            </Snackbar>
        </Stack>
    );
}
