import StyledPaper from "../../components/StyledPaper.jsx";
import { useState} from "react";
import { TabContext, TabPanel } from "@mui/lab";
import StyledTabList from "../../components/StyledTabList.jsx";
import styles from "./current_visits.module.css";
import arrow from "../../assets/arrow.svg";
import StyledTab from "../../components/StyledTab.jsx";
import { APIProvider} from "@vis.gl/react-google-maps";
import RouteMap from "../../components/RouteMap.jsx";
import {useLoaderData} from "react-router-dom";
import VisitRequestItem from "../../components/visit_request_item/VisitRequestItem.jsx";
import {Stack} from "@mui/material";


export default function CurrentVisits() {
    const {hospitalCoords, currentVisitRequests} = useLoaderData();
    const [activeTab, setActiveTab] = useState("visits");
    const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY;

    const start = hospitalCoords;

    const waypoints = currentVisitRequests.map(visitRequest => visitRequest.visitCoords)
    const handleChange = (event, newValue) => {
        setActiveTab(newValue);
    };

    const handleReturnBack = () => {
        window.history.back();
    };

    return (
        <StyledPaper width="70%" height="90%" marginLeft="10rem">
            <TabContext value={activeTab}>
                <div className={styles.header}>
                    <StyledTabList onChange={handleChange} centered>
                        <button className={styles.return_btn} onClick={handleReturnBack}>
                            <img src={arrow} alt="arrow" />
                        </button>
                        <StyledTab label="Visits" value="visits" fontSize="1.25rem" />
                        <StyledTab label="Route" value="route" fontSize="1.25rem" />
                    </StyledTabList>
                </div>
                <TabPanel value="visits" sx={{height: '90%'}}>
                    <ul type='none' className={styles.visit_requests_list}>
                        {currentVisitRequests.length !== 0 ? currentVisitRequests.map(visitRequest => <li key={visitRequest.id}>
                            <VisitRequestItem visitRequest={visitRequest} />
                        </li>):
                            <Stack width="100%" justifyContent="center" height='100%' alignItems="center">
                                <h2>You have no visits for today </h2>
                            </Stack>}
                    </ul>
                </TabPanel>
                <TabPanel value="route" sx={{ padding: 0 }}>
                    <APIProvider apiKey={apiKey}>
                        <RouteMap start={start} waypoints={waypoints} />
                    </APIProvider>
                </TabPanel>
              
            </TabContext>
        </StyledPaper>
    );
}

