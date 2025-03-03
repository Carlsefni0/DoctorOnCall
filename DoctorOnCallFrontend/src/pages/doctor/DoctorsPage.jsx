import styles from '../../components/filter_modal/filter_modal.module.css'
import GeneralListPage from "../../components/GenericListPage.jsx";
import getDoctorColumns from "../../components/table/getDoctorColumns.jsx";
import {useLoaderData, useNavigate, useNavigation} from "react-router-dom";
import PersonAddIcon from "@mui/icons-material/PersonAdd";
import FilterModal from "../../components/filter_modal/FilterModal.jsx";
import FilterAltIcon from "@mui/icons-material/FilterAlt";
import {useState} from "react";
import StyledTextField from "../../components/StyledTextField.jsx";
import specializations from '../../data/specializations.js'
import districts from "../../data/districts.js"
import {Stack} from "@mui/material";
import StyledButton from "../../components/StyledButton.jsx";

const initialFilters = {
    specializations: [],
    districts: [],
    firstName: "",
    lastName: "",

}
export default function DoctorsPage() {
    const initialDoctors = useLoaderData();
    const navigation = useNavigation();
    const navigate = useNavigate();
    const [isFilterModalOpen, setIsFilterModalOpen] = useState(false);
    const [filters, setFilters] = useState(initialFilters);
    const [doctorFullName, setDoctorFullName] = useState("");
    
    const toggleFilterModal = () => {
        setIsFilterModalOpen(!isFilterModalOpen);
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
            tooltipTitle: "Register new doctor",
            onClick: () => navigate("register"),
        },
    ];

    return (
        <>
            <GeneralListPage
                getColumns={getDoctorColumns}
                initialData={initialDoctors}
                actions={actions}
                controller="doctor"
                filters={filters}
                isLoadingInitial={navigation.state === "loading"}
            />
            <FilterModal
                open={isFilterModalOpen}
                onClose={toggleFilterModal}
                width="30vw"
                height="62vh"
            >
                <Stack spacing={2} height="100%">
                    <Stack spacing={2}>
                        <p className={styles.criteria}>Doctor name</p>
                        <StyledTextField borderRadius="10rem" placeholder="Type here"
                                         onChange={handleDoctorFullNameChange} value={doctorFullName}/>
                    </Stack>
                    <p className={styles.criteria}>Specialization</p>
                    <ul className={styles.options_wrapper}>
                        {specializations.map(item => (
                            <li
                                key={item}
                                onClick={() => updateFilters("specializations", item)}
                                className={filters.specializations.includes(item) ? styles.option__selected : styles.option}
                            >
                                {item}
                            </li>
                        ))}
                    </ul>
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
                    <Stack alignItems='center' width='100%'>
                        <StyledButton width='50%' marginTop='2rem' onClick={handleResetFilters}>Reset Filters</StyledButton>
                    </Stack>
                </Stack>
               
            </FilterModal>
        </>
    );
}
