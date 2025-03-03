import {useLoaderData, useNavigate} from "react-router-dom";
import stringAvatar from "../../utils/stringToAvatar.js";
import GenericUserDetailsPage from "../../components/generic_details/GenericUserDetailsPage.jsx";
import StyledButton from "../../components/StyledButton.jsx";
import {Stack} from "@mui/material";

export default function DoctorDetailsPage() {
    const doctorData = useLoaderData();
    const navigate = useNavigate();
    const handleDoctorRequestsClick = () => {
        navigate(`../../visits?doctorId=${id}`);
    };
    const handleDoctorSchedulesClick = () => {
        navigate(`../../schedules?doctorId=${id}`);
    };
    const handleDoctorAnalyticsClick = () => {
        navigate(`../../analytics?doctorId=${id}`);
    };
    const {firstName, lastName, email, gender, specialization, phoneNumber, workingDistrict, status, hospital, id} = doctorData;
    
    return (
        <GenericUserDetailsPage
            title={`${firstName} ${lastName}`}
            avatarText={stringAvatar(`${firstName} ${lastName}`)}
            email={email}
            id={id}
            details={[
                {label: "Gender", value: gender},
                {label: "Specialization", value: specialization},
                {label: "Phone Number", value: phoneNumber},
                {label: "District", value: workingDistrict},
                {label: "Hospital", value: hospital}
            ]}>
            <Stack direction='row' justifyContent='center' marginTop='2rem'>
                <StyledButton variant='text' color='#25AF9DFF' onClick={handleDoctorRequestsClick}>Requests</StyledButton>
                <StyledButton variant='text' color='#25AF9DFF' onClick={handleDoctorSchedulesClick}>Schedules</StyledButton>
                <StyledButton variant='text' color='#25AF9DFF' onClick={handleDoctorAnalyticsClick}>Analytics</StyledButton>
                
            </Stack>
        </GenericUserDetailsPage>
    );
}
