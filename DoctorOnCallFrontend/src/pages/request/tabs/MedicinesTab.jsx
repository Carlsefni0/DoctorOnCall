import {CircularProgress, Stack} from "@mui/material";
import styles from "../request_details.module.css";
import MedicineItem from "../../../components/medicine-item/MedicineItem.jsx";
import {useQuery} from "@tanstack/react-query";
import performRequest from "../../../utils/performRequest.js";

export default function MedicinesTab({requestDataId, activeTab}) {

    const baseUrl = import.meta.env.VITE_BACKEND_URL;
    

    const { data: medicines, isLoading: isLoadingMedicines } = useQuery({
        queryKey: ['medicines', requestDataId],
        queryFn: () => performRequest(`${baseUrl}/medicine/${requestDataId}`),
        enabled: activeTab === "medicines",
        staleTime: 1000 * 60 * 5,
    });

    return <>
        {isLoadingMedicines ?
            <Stack alignItems='center' justifyContent='center' width='100%' height='100%'>
                <CircularProgress sx={{color: '#088172'}} size='5rem'/>
            </Stack> :
            medicines && medicines.length !== 0 ?
                <ul className={styles.items_container__medicines} type='none'>
                    { medicines.map(medicine =>
                        <li key={medicine.id}>
                            <MedicineItem medicine={medicine}/>
                        </li>)
                    }
                </ul> :
                <div className={styles.message}>
                    The patient didn't ask for any medicines
                </div>
        }
    </>
}