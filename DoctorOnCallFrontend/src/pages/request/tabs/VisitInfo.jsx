import { Accordion, AccordionSummary, AccordionDetails, Divider, Stack } from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import styles from "./request_info_tab.module.css";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import performRequest from "../../../utils/performRequest.js";

const formatTime = (dateTime) => {
    if (!dateTime) return "N/A";
    const date = new Date(dateTime);
    return date.toISOString().slice(11, 16);
};

export default function VisitInfo({ visitRequestId }) {
    const baseUrl = import.meta.env.VITE_BACKEND_URL;
    const [expanded, setExpanded] = useState(null);

    const { data: visitData, isLoading } = useQuery({
        queryKey: ["/visit", visitRequestId],
        queryFn: () => performRequest(`${baseUrl}/visit/${visitRequestId}`),
    });

    const handleAccordionChange = (panel) => (event, isExpanded) => {
        setExpanded(isExpanded ? panel : null);
    };

    return (
            <ul className={styles.visits_list}>
                {visitData?.length > 0 ? (
                    visitData.map((visit, index) => (
                        <li><Accordion
                            key={index}
                            sx={{ width: "80%" }}
                            expanded={expanded === index}
                            onChange={handleAccordionChange(index)}
                        >
                            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                                <p className={styles.visit_accordion_summary}>
                                    {`Visit ${index + 1}: ${new Date(visit.actualStartDateTime).toLocaleDateString().split('T')[0]}`}
                                </p>
                            </AccordionSummary>
                            <AccordionDetails>
                                <Stack>
                                    <Divider />
                                    <Stack direction="row" spacing={5} marginTop={2}>
                                        <dl className={styles.property_wrapper}>
                                            <dt>Visit Start Time</dt>
                                            <dd>{visit.actualStartDateTime?.slice(11, 16) || 'N/A'}</dd>
                                        </dl>
                                        <dl className={styles.property_wrapper}>
                                            <dt>Visit End Time</dt>
                                            <dd>{visit.actualEndDateTime?.slice(11, 16) || 'N/A'}</dd>
                                        </dl>
                                        <dl className={styles.property_wrapper}>
                                            <dt>Delay Time</dt>
                                            <dd>{visit.delayTime || "N/A"}</dd>
                                        </dl>
                                    </Stack>
                                    <Divider sx={{ my: 1 }} />
                                    <dl className={styles.property_wrapper__text}>
                                        <dt>{visit.notes ? "Notes" : "Cancellation Reason"}</dt>
                                        <dd>{visit.notes || visit.CancellationReason || "N/A"}</dd>
                                    </dl>
                                </Stack>
                            </AccordionDetails>
                        </Accordion> </li>
                    ))
                ) : (
                    <h2>No visits available</h2>
                )}
            </ul>
    );
}
