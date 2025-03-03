import {useLoaderData, useNavigate} from "react-router-dom";
import stringAvatar from "../../utils/stringToAvatar.js";
import GenericUserDetailsPage from "../../components/generic_details/GenericUserDetailsPage.jsx";
import StyledButton from "../../components/StyledButton.jsx";
import {Stack} from "@mui/material";

export default function PatientDetailsPage() {
    const patientData = useLoaderData();
    const navigate = useNavigate();
    const { firstName, lastName, email, gender, phoneNumber, address, district, dateOfBirth, disease, id } = patientData;
    const birthdate = new Date(dateOfBirth).toLocaleString().split(',')[0];
    const handlePatientRequestsClick = () => {
        navigate(`../../visits?PatientId=${id}`);
    };

    return (
        <GenericUserDetailsPage
            title={`${firstName} ${lastName}`}
            avatarText={stringAvatar(`${firstName} ${lastName}`)}
            email={email}
            id={id}
            details={[
                { label: "Gender", value: gender},
                { label: "Birth Date", value: birthdate},
                { label: "Phone Number", value: phoneNumber},
                { label: "District", value: district},
                { label: "Disease", value: disease},
                { label: "Address", value: address},
            ]}
        >
            <Stack direction='row' justifyContent='center' marginTop='2rem'>
                <StyledButton variant='text' color='#25AF9DFF' width='20%'  onClick={handlePatientRequestsClick}>Patient Requests</StyledButton>
            </Stack>
        </GenericUserDetailsPage>
    );
}
