import GeneralListPage from "../../components/GenericListPage.jsx";
import getRequestColumns from "../../components/table/getRequestColumns.jsx";
import {useLoaderData, useNavigate, useNavigation} from "react-router-dom";
import AddIcon from "@mui/icons-material/Add";
import {getUser} from "../../context/UserContext.jsx";
import {Stack} from "@mui/material";
import StyledTextField from "../../components/StyledTextField.jsx";
import styles from "../../components/filter_modal/filter_modal.module.css";
import specializations from "../../data/specializations.js";
import districts from "../../data/districts.js";
import FilterModal from "../../components/filter_modal/FilterModal.jsx";
import {useState} from "react";
import FilterAltIcon from "@mui/icons-material/FilterAlt";
import PersonAddIcon from "@mui/icons-material/PersonAdd";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import StyledDatePicker from "../../components/StyledDatePicker.jsx";
import dayjs from "dayjs";
import {LocalizationProvider} from "@mui/x-date-pickers/LocalizationProvider";
import StyledButton from "../../components/StyledButton.jsx";

const visitRequestStatus = {
    'Pending': 0,
    'Approved': 1,
    'Accepted': 2,
    'Rejected': 3,
    'Cancelled': 4,
    'Completed': 5,
}

const visitRequestType = {
    'Сonsultation': 0,
    'Examination': 1,
    'Procedures': 2,
    'Remote Visit': 3,
}
const visitRequestFrequency = {
    'Regular': 'true',
    'One-time': 'false',
}
const initialFilters = {
    statuses: [],
    types: [],
    isRegular: null,
    year: null,
    month: null,
}

export default function RequestsPage() {
    const navigate= useNavigate();
    const initialRequests = useLoaderData();
    const navigation = useNavigation();
    const [isFilterModalOpen, setIsFilterModalOpen] = useState(false);
    const [filters, setFilters] = useState(initialFilters);
    const {user} = getUser();

    const toggleFilterModal = () => {
        setIsFilterModalOpen(!isFilterModalOpen);
    };

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
       
    ];
    
    if(user.role === 'Patient') {
        actions.push({
            icon: <AddIcon />,
            name: "Create visit request",
            tooltipTitle: "Create visit request",
            onClick: () => {navigate("create")}
        })
    }
    
    return (
        <>
            <GeneralListPage
                getColumns={getRequestColumns}
                initialData={initialRequests}
                filters={filters}
                controller='visit-request'
                isLoadingInitial={navigation.state === "loading"}
                actions={actions}

            />
            <FilterModal
                open={isFilterModalOpen}
                onClose={toggleFilterModal}
                width="30vw"
                height="50vh"
            >
                <Stack spacing={2} height="100%">
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <Stack direction="row" spacing={4}>
                            <Stack spacing={1}>
                                <p className={styles.criteria}>Year</p>
                                <StyledDatePicker
                                    borderRadius='10rem'
                                    views={["year"]}
                                    height='3rem'
                                    value={filters.year ? dayjs().year(filters.year) : null}
                                    onChange={(newYear) => updateFilters("year", newYear ? newYear.year() : null)}
                                    format="YYYY"
                                />
                            </Stack>
                            <Stack spacing={1}>
                                <p className={styles.criteria}>Month</p>
                                <StyledDatePicker
                                    borderRadius='10rem'
                                    height='3rem'
                                    views={["month"]}
                                    value={filters.year ? dayjs().year(filters.year).month((filters.month || 1) - 1) : null}
                                    onChange={(newMonth) => updateFilters("month", newMonth ? newMonth.month() + 1 : null)}
                                    format="MM"
                                />
                            </Stack>
                        </Stack>
                    </LocalizationProvider>
                    <p className={styles.criteria}>Status</p>
                    <ul className={styles.options_wrapper}>
                        {
                            Object.entries(visitRequestStatus).map(([key, value]) =>
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
                    <p className={styles.criteria}>Visit type</p>
                    <ul className={styles.options_wrapper}>
                        {
                            Object.entries(visitRequestType).map(([key, value]) =>
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
                    <p className={styles.criteria}>Frequency</p>
                    <ul className={styles.options_wrapper}>
                        {
                            Object.entries(visitRequestFrequency).map(([key, value]) =>
                                <li
                                    key={key}
                                    onClick={() => setFilters(prevState => ({...prevState, isRegular: value}))}
                                    className={filters.isRegular === value ? styles.option__selected : styles.option}
                                >
                                    {key}
                                </li>
                            )
                        }
                    </ul>
                    <Stack alignItems='center' width='100%'>
                        <StyledButton  marginTop='2rem' width='50%' onClick={()=>setFilters(initialFilters)}>Reset Filters</StyledButton>
                    </Stack>
                </Stack>
            </FilterModal>
        </>

    );
}
