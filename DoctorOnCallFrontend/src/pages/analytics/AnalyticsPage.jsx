import styles from "./analytics.module.css";
import StyledPaper from "../../components/StyledPaper.jsx";
import {TabContext, TabPanel} from "@mui/lab";
import StyledTabList from "../../components/StyledTabList.jsx";
import StyledTab from "../../components/StyledTab.jsx";
import { useState } from "react";
import {CircularProgress, Divider, Stack} from "@mui/material";
import StyledTextField from "../../components/StyledTextField.jsx";
import {useQuery} from "@tanstack/react-query";
import performRequest from "../../utils/performRequest.js";
import TravelCharts from "./tabs/TravelCharts.jsx";
import VisitCharts from "./tabs/VisitCharts.jsx";
import MedicineCharts from "./tabs/MedicineCharts.jsx";
import arrow from "../../assets/arrow.svg";
import {useLoaderData, useSearchParams} from "react-router-dom";

export default function AnalyticsPage() {
    const initialData = useLoaderData();
    const [activeTab, setActiveTab] = useState("travel");
    const [searchParams, setSearchParams] = useSearchParams();
    
    const now = new Date();

    const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 2);
    const startOfMonthISO = startOfMonth.toISOString().split("T")[0];

    const endOfMonth = new Date(now.getFullYear(), now.getMonth() + 1, 0, 23, 59, 59);
    const endOfMonthISO = endOfMonth.toISOString().split("T")[0];
    
    const [startDate, setStartDate] = useState(startOfMonthISO);
    const [endDate, setEndDate] = useState(endOfMonthISO);
    const baseUrl = import.meta.env.VITE_BACKEND_URL;
    
    const analyticsEndpoints = {
        travel: "/travel",
        visits: "/visit",
        medicines: "/medicine",
    }
    
    const handleChange = (event, newValue) => {
        setActiveTab(newValue);
    };

    const { data, isLoading } = useQuery({
        queryKey: [activeTab, startDate, endDate],
        queryFn: async () => {
            const filtersFromUrl = Object.fromEntries(searchParams.entries());
            const params = new URLSearchParams();

            const ISOStartDate = new Date(startDate).toISOString();
            const ISOEndDate = new Date(endDate).toISOString();

            if (startDate) params.append("StartDate", ISOStartDate.split(".")[0]);
            if (endDate) params.append("EndDate", ISOEndDate.split(".")[0]);

            const start = new Date(startDate);
            const end = new Date(endDate);

                
            const diffInMonths = (end.getFullYear() - start.getFullYear()) * 12 + (end.getMonth() - start.getMonth()) || 0;

            let groupInterval = 0;
            if (diffInMonths > 1 && diffInMonths <= 6) {
                groupInterval = 1;
            } else if (diffInMonths > 6) {
                groupInterval = 2;
            }

            params.append("GroupInterval", groupInterval);
            if(filtersFromUrl.doctorId) params.append("DoctorId", filtersFromUrl.doctorId)
            
            const url = `${baseUrl}/analytics${analyticsEndpoints[activeTab]}?${params.toString()}`;
            return performRequest(url);
        },
        initialData
    });
    
    return (
        <StyledPaper width='90%' marginLeft='2.5rem' height='95%'>
            <TabContext value={activeTab}>
                <div className={styles.header}>
                   
                    <StyledTabList onChange={handleChange} centered>
                      
                        <StyledTab label="Travel" value="travel" fontSize='1.25rem'/>
                        <StyledTab label="Visits" value="visits" fontSize='1.25rem'/>
                        <StyledTab label="Medicines" value="medicines" fontSize='1.25rem'/>
                    </StyledTabList>
                </div>
                <Stack direction="row" height='8rem' justifyContent='center' alignItems='center' spacing={4}>
                    <StyledTextField
                        label="Start date"
                        type="date"
                        name="startDate"
                        InputLabelProps={{ shrink: true }}
                        width='20%'
                        value={startDate}
                        onChange={(e)=>setStartDate(e.target.value)}
                        
                    />
                    <StyledTextField
                        label="End date"
                        type="date"
                        name="enedDate"
                        InputLabelProps={{ shrink: true }}
                        width='20%'
                        value={endDate}
                        onChange={(e)=>setEndDate(e.target.value)}
                        
                    />
                </Stack>
               
                <TabPanel value="travel" sx={{padding: 0}}>
                    {isLoading ?
                        <Stack width='100%' height='45vh' justifyContent='center' alignItems='center'>
                            <CircularProgress sx={{color: '#088172'}} size='5rem'/> 
                        </Stack> :
                        <TravelCharts data={data}/>}
                </TabPanel>
                <TabPanel value="visits" sx={{padding: 0}}>
                    {isLoading ?
                        <Stack width='100%' height='45vh' justifyContent='center' alignItems='center'>
                            <CircularProgress sx={{color: '#088172'}} size='5rem'/>
                        </Stack> :
                        <VisitCharts data={data}/>}
                </TabPanel>
                <TabPanel value="medicines" sx={{padding: '0'}}>
                    {isLoading ?
                        <Stack width='100%' height='45vh' justifyContent='center' alignItems='center'>
                            <CircularProgress sx={{color: '#088172'}} size='5rem'/>
                        </Stack> :
                        <MedicineCharts data={data}/>}
                </TabPanel>
            </TabContext>
        </StyledPaper>
    );
}