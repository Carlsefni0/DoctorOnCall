import {useLoaderData, useNavigation} from "react-router-dom";
import { useEffect, useState } from "react";
import { TabContext, TabPanel} from "@mui/lab";
import styles from "./request_details.module.css";
import StyledTab from "../../components/StyledTab.jsx";
import StyledTabList from "../../components/StyledTabList.jsx";
import StyledPaper from "../../components/StyledPaper.jsx";
import {CircularProgress, Stack} from "@mui/material";
import StyledButton from "../../components/StyledButton.jsx";
import arrow from "../../assets/arrow.svg";
import RequestInfoTab from "./tabs/RequestInfoTab.jsx";
import RejectTab from "./tabs/RejectTab.jsx";
import DoctorSearchTab from "./tabs/DoctorSearchTab.jsx";
import MedicinesTab from "./tabs/MedicinesTab.jsx";
import MapTab from "./tabs/MapTab.jsx";
import VisitInfoTab from "./tabs/VisitInfoTab.jsx";
import {useQuery} from "@tanstack/react-query";
import performRequest from "../../utils/performRequest.js";
import {getUser} from "../../context/UserContext.jsx";
import EditButton from "../../components/edit_button/EditButton.jsx";

export default function RequestDetailsPage() {
    const {user} = getUser();
    const initialData = useLoaderData();
    const {isLoading} = useNavigation();
    const [activeTab, setActiveTab] = useState("request");
    const [activeAction, setActiveAction] = useState('overview');
    const [displayedActions, setDisplayedActions] = useState([]);
    const baseUrl = import.meta.env.VITE_BACKEND_URL;


    const { data: requestData} = useQuery({
        queryKey: ['visit-request', initialData.id],
        queryFn: () => performRequest(`${baseUrl}/visit-request/${initialData.id}`),
        initialData,
        keepPreviousData: true,
    });


    const visitStatusColors = {
        Pending: "orange",
        Approved: "green",
        Rejected: "red",
        Completed: "#2bcc56",
        Cancelled: "#2b4bcc",
    };
    
    let isDoctorTabDisabled = false;
    let isRejectTabDisabled = false;
    let isVisitTabDisabled = false
    
    if (requestData.status === 'Cancelled' || requestData.status === 'Rejected') isDoctorTabDisabled = true;
    if (requestData.status === 'Pending' && user.role === 'Patient') isDoctorTabDisabled = true;
    
    if (requestData.status === 'Approved' || requestData.status === 'Cancelled') isRejectTabDisabled = true;
    if (requestData.status === 'Pending' && user.role === 'Patient') isRejectTabDisabled = true;
    
    if(requestData.status === 'Approved' || requestData.status === 'Completed') isVisitTabDisabled = true;
    
    const actions = [
        { buttonText: "Rejection", action: "reject", disableIf: isRejectTabDisabled},
        { buttonText: "Doctor", action: "searchingDoctor", disableIf: isDoctorTabDisabled},
        { buttonText: "Overview", action: "overview" }
    ];
    
    useEffect(() => {
        const actionsToDisplay = actions.filter(action => action.action !== activeAction);
        setDisplayedActions(actionsToDisplay);
    }, [activeAction]);
    
    const handleChange = (event, newValue) => {
        setActiveTab(newValue);
    };

    const handleChangeAction = (activeAction) => {
        setActiveAction(activeAction);
    };

    const handleReturnBack = ()=> {
        window.history.back()
    }
    
  
   
    return (
        <StyledPaper width='70%' marginLeft='10rem' height='95%'>
            <TabContext value={activeTab}>
                <div className={styles.header}>
                    <StyledTabList onChange={handleChange} centered>
                        <button className={styles.return_btn} onClick={handleReturnBack}>
                            <img src={arrow} alt="arrow"/>
                        </button>)
                        <StyledTab label="Request" value="request" fontSize='1.25rem' />
                        <StyledTab label="Location" value="location" fontSize='1.25rem' />
                        <StyledTab label="Medicines" value="medicines" fontSize='1.25rem' />
                        {isVisitTabDisabled && <StyledTab label="Visit" value='visit' fontSize='1.25rem'/>}
                    </StyledTabList>
                    {user.role === 'Patient' && requestData.status === 'Pending' && <EditButton recordId={requestData.id}/>}
                </div>

                <TabPanel value="request"  >
                    {isLoading ? 
                        <Stack alignItems='center' justifyContent='center' width='100%' height='100%'>
                            <CircularProgress sx={{color: '#088172'}} size='5rem'/>
                        </Stack> : 
                        <>
                            <Stack direction='row' justifyContent='end'>
                                <span style={{ color: visitStatusColors[requestData.status] }} className={styles.status}>
                                    {requestData.status}
                                </span>
                            </Stack>

                            {activeAction === 'overview' && <RequestInfoTab requestData={requestData}/>}
                            {activeAction === 'reject' && <RejectTab requestData={requestData}/>}
                            {activeAction === "searchingDoctor" && <DoctorSearchTab requestData={requestData}/>}

                            <Stack direction='row' spacing={4} justifyContent='center'>
                                {displayedActions && displayedActions.map(action =>
                                    <StyledButton key={action.action} variant="text" onClick={() => handleChangeAction(action.action)}
                                                  size='small' disabled={action.disableIf} color='#25AF9DFF'>
                                        {action.buttonText}
                                    </StyledButton>)}
                            </Stack>
                        </>
                    }
                </TabPanel>
                <TabPanel value="location" sx={{ padding: 0}}>
                    <MapTab visitCoords={requestData.visitCoords}/>
                </TabPanel>
                <TabPanel value="medicines" sx={{ height: '73vh', padding: '0' }}>
                    <MedicinesTab activeTab={activeTab} requestDataId={requestData.id} />
                </TabPanel>
                <TabPanel value='visit' sx={{height: '72vh'}}>
                    <Stack direction='row' justifyContent='end'>
                        <span style={{ color: visitStatusColors[requestData.status] }} className={styles.status}>
                            {requestData.status}
                        </span>
                    </Stack>
                    <VisitInfoTab requestData={requestData}/>
                </TabPanel>
            </TabContext>
        </StyledPaper>
    );
}