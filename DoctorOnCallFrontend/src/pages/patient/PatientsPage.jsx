import GeneralListPage from "../../components/GenericListPage.jsx";
import getPatientColumns from "../../components/table/getPatientColumns.jsx";
import {useLoaderData, useNavigate} from "react-router-dom";
import PersonAddIcon from "@mui/icons-material/PersonAdd";
import FilterModal from "../../components/filter_modal/FilterModal.jsx";
import {Stack} from "@mui/material";
import StyledTextField from "../../components/StyledTextField.jsx";
import styles from "../../components/filter_modal/filter_modal.module.css";
import districts from "../../data/districts.js";
import FilterAltIcon from "@mui/icons-material/FilterAlt";
import {useState} from "react";
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs from "dayjs";
import StyledDatePicker from "../../components/StyledDatePicker.jsx";
import StyledButton from "../../components/StyledButton.jsx";

const gender = {
    Male: 0,
    Female: 1,
}
const initialFilters = {
    districts: [],
    firstName: "",
    lastName: "",
    birthYear: null,
    birthMonth: null,
    gender: null,
}
export default function PatientsPage() {
    const initialPatients = useLoaderData();
    const navigate = useNavigate();
    const [isFilterModalOpen, setIsFilterModalOpen] = useState(false);
    const [filters, setFilters] = useState(initialFilters);
    const [patientFullName, setPatientFullName] = useState('');

    const toggleFilterModal = () => {
        setIsFilterModalOpen(!isFilterModalOpen);
    };

    const handlePatientFullNameChange = (e) => {
        const fullName = e.target.value;
        setPatientFullName(fullName);

        const [first, last] = fullName.split(" ");
        setFilters(prev => ({
            ...prev,
            firstName: first || "",
            lastName: last || "",
        }));
    };
    
    const handleResetFilters = () => {
        setFilters(initialFilters);
        setPatientFullName('');
    }

    const updateFilters = (key, value) => {
        setFilters(prev => {
            
            if(key === 'birthYear') {
                prev.birthYear = null;
            }
            const values = new Set(prev[key]);

            if (values.has(value)) {
                values.delete(value);
            } else {
                values.add(value);
            }

            return { ...prev, [key]: [...values] };
        });
    };

    const actions = [
        {
            icon: <FilterAltIcon />,
            name: "Filter",
            tooltipTitle: "Open filter",
            onClick: toggleFilterModal,
        },
        {
            icon: <PersonAddIcon />,
            name: "Add",
            tooltipTitle: "Register new patient",
            onClick: () => navigate("register"),
        },
    ];

    return (
        <>
            <GeneralListPage
                getColumns={getPatientColumns}
                initialData={initialPatients}
                filters={filters}
                actions={actions}
                controller='patient'
            />
            <FilterModal
                open={isFilterModalOpen}
                onClose={toggleFilterModal}
                width="30vw"
                height="55vh"
            >
                <Stack spacing={2} height="100%">
                    <Stack spacing={1}>
                        <p className={styles.criteria}>Patient name</p>
                        <StyledTextField borderRadius="10rem" placeholder="Type here"
                                         onChange={handlePatientFullNameChange}
                                         value={patientFullName}/>
                    </Stack>

                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <Stack direction="row" spacing={4}>
                            <Stack spacing={1}>
                                <p className={styles.criteria}>Birth Year</p>
                                <StyledDatePicker
                                    borderRadius='10rem'
                                    height='3rem'
                                    views={["year"]}
                                    value={filters.birthYear ? dayjs().year(filters.birthYear) : null}
                                    onChange={(newYear) => updateFilters("birthYear", newYear ? newYear.year() : null)}
                                    format="YYYY"
                                />
                            </Stack>
                            <Stack spacing={1}>
                                <p className={styles.criteria}>Birth Month</p>
                                <StyledDatePicker
                                    borderRadius='10rem'
                                    height='3rem'
                                    views={["month"]}
                                    value={filters.birthYear ? dayjs().year(filters.birthYear).month((filters.birthMonth || 1) - 1) : null}
                                    onChange={(newMonth) => updateFilters("birthMonth", newMonth ? newMonth.month() + 1 : null)}
                                    format="MM"
                                />
                            </Stack>
                        </Stack>
                    </LocalizationProvider>
                    <p className={styles.criteria}>District</p>
                    <ul className={styles.options_wrapper}>
                        {districts.map(item => (
                            <li
                                key={item}
                                onClick={() => updateFilters("districts", item)}
                                className={filters.districts.includes(item) ? styles.option__selected : styles.option}
                            >
                                {item}
                            </li>
                        ))}
                    </ul>
                    <p className={styles.criteria}>Gender</p>
                    <ul className={styles.options_wrapper}>
                        {
                            Object.entries(gender).map(([key, value]) =>
                                <li
                                    key={key}
                                    onClick={() => setFilters(prevState => ({...prevState, gender: value}))}
                                    className={filters.gender === value ? styles.option__selected : styles.option}
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
