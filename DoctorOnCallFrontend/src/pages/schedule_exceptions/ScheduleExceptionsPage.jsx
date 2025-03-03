import {useLoaderData, useNavigate, useNavigation} from "react-router-dom";
import GeneralListPage from "../../components/GenericListPage.jsx";
import getRequestColumns from "../../components/table/getRequestColumns.jsx";
import getScheduleExceptionColumns from "../../components/table/getScheduleExceptionColumns.jsx";
import AddIcon from '@mui/icons-material/Add';
import FilterModal from "../../components/filter_modal/FilterModal.jsx";
import {Stack} from "@mui/material";
import styles from "../../components/filter_modal/filter_modal.module.css";
import StyledTextField from "../../components/StyledTextField.jsx";
import {LocalizationProvider} from "@mui/x-date-pickers/LocalizationProvider";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import StyledDatePicker from "../../components/StyledDatePicker.jsx";
import dayjs from "dayjs";
import districts from "../../data/districts.js";
import StyledButton from "../../components/StyledButton.jsx";
import {getUser} from "../../context/UserContext.jsx";
import {useState} from "react";
import FilterAltIcon from "@mui/icons-material/FilterAlt";


const scheduleExceptionStatus = {
    'Pending': 0,
    'Approved': 1,
    'Rejected': 2,
    'Cancelled': 3
}
const scheduleExceptionType = {
    'Vacation': 0,
    'Sick leave': 1,
    'Training': 2,
    'Emergency': 3,
    'Other':4
}
const initialFilters = {
    types: [],
    statuses: [],
    firstName: '',
    lastName: '',
}
export default function ScheduleExceptionsPage() {
    const initialScheduleExceptions = useLoaderData();
    const navigation = useNavigation();
    const navigate = useNavigate();
    const [isFilterModalOpen, setIsFilterModalOpen] = useState(false);
    const [filters, setFilters] = useState(initialFilters);
    const [doctorFullName, setDoctorFullName] = useState("");
    const {user} = getUser();


    const toggleFilterModal = () => {
        setIsFilterModalOpen(!isFilterModalOpen);
    };
    
    const actions = [
        {
            icon: <FilterAltIcon />,
            name: "Filter",
            tooltipTitle: "Open filter",
            onClick: toggleFilterModal,
        },
    ];
    
    if(user.role === 'Doctor') {
        actions.push( {
            icon: <AddIcon />,
            name: "Create new Schedule Exception",
            tooltipTitle: "Create new Schedule Exception",
            onClick: () => navigate("create"),
        },)
    }

    const updateFilters = (key, value) => {
        setFilters(prev => {
            const values = new Set(prev[key]);

            if (values.has(value)) {
                values.delete(value);
            } else {
                values.add(value);
            }

            return { ...prev, [key]: [...values] };
        });
    };
    const handleDoctorFullNameChange = (e) => {
        const fullName = e.target.value;
        setDoctorFullName(fullName);

        const [first, last] = fullName.split(" ");
        setFilters(prev => ({
            ...prev,
            firstName: first || "",
            lastName: last || "",
        }));
    };
    const handleResetFilters = () => {
        setFilters(initialFilters);
        setDoctorFullName('');
    }

    return (
        <>
            <GeneralListPage
                getColumns={getScheduleExceptionColumns}
                initialData={initialScheduleExceptions}
                filters={filters}
                actions={actions}
                controller='schedule-exception'
                isLoadingInitial={navigation.state === "loading"}

            />
            <FilterModal
                open={isFilterModalOpen}
                onClose={toggleFilterModal}
                width="30vw"
                height="45vh"
            >
                <Stack spacing={2} height="100%">
                    {user.role !== 'Doctor' && <Stack spacing={2}>
                        <p className={styles.criteria}>Doctor name</p>
                        <StyledTextField borderRadius="10rem" placeholder="Type here"
                                         onChange={handleDoctorFullNameChange} value={doctorFullName}/>
                    </Stack>}
                    <p className={styles.criteria}>Type</p>
                    <ul className={styles.options_wrapper}>
                        {
                            Object.entries(scheduleExceptionType).map(([key, value]) =>
                                <li
                                    key={key}
                                    onClick={() => updateFilters("types", value)}
                                    className={filters.types.includes(value) ? styles.option__selected : styles.option}
                                >
                                    {key}
                                </li>
                            )
                        }
                    </ul>
                    <p className={styles.criteria}>Status</p>
                    <ul className={styles.options_wrapper}>
                        {
                            Object.entries(scheduleExceptionStatus).map(([key, value]) =>
                                <li
                                    key={key}
                                    onClick={() => updateFilters("statuses", value)}
                                    className={filters.statuses.includes(value) ? styles.option__selected : styles.option}
                                >
                                    {key}
                                </li>
                            )
                        }
                    </ul>


                </Stack>
                <Stack alignItems='center' width='100%'>
                    <StyledButton marginTop='2rem' width='50%' onClick={handleResetFilters}>Reset Filters</StyledButton>
                </Stack>
            </FilterModal>
        </>

    );
}