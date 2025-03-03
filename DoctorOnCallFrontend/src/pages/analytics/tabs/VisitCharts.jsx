import { Stack } from "@mui/material";
import styles from "./visit_charts.module.css";
import CustomDoughnutChart from "../../../components/charts/CustomDoughnutChart.jsx";
import CustomLineChart from "../../../components/charts/CustomLineChart.jsx";
import { useEffect, useRef } from "react";

export default function VisitCharts({ data }) {
    const containerRef = useRef(null);
    const scrollPosition = useRef(0);

    useEffect(() => {
        if (containerRef.current) {
            scrollPosition.current = containerRef.current.scrollTop;
        }
    }, [data]);

    useEffect(() => {
        if (containerRef.current) {
            setTimeout(() => {
                containerRef.current.scrollTop = scrollPosition.current;
            }, 0);
        }
    }, [data]);

    if (!data) {
        return (
            <Stack width="100%" height="45vh" justifyContent="center" alignItems="center">
                <h2>No Data Available</h2>
            </Stack>
        );
    }

    const visitRequestTypes = [...Object.values(data).slice(2)];
    const isTypesEmpty = visitRequestTypes.every(item => +item === 0);
    const visitRequestLabels = ["Consultations", "Examinations", "Procedures", "Remote"];

    const visitsStatus = [...Object.values(data).slice(0, 2)];
    const isVisitsEmpty = visitsStatus.every(item => +item === 0)
    const visitLabels = ["Completed", "Cancelled"];

    return (
        <div ref={containerRef} className={styles.charts_container}>
            <Stack direction="row" justifyContent="center" spacing="10rem" marginTop={2}>
                <div className={styles.doughnut_container}>
                    {!isTypesEmpty ?
                        <CustomDoughnutChart
                            data={visitRequestTypes}
                            labels={visitRequestLabels}
                            property="totalTravelTime"
                            title="Visits type"
                        /> : <Stack width="100%" height="25rem" justifyContent="center" alignItems="center" border = '3px solid lightgray' borderRadius={100}>
                            <h2>No data available</h2>
                        </Stack>}
                </div>
                <div className={styles.doughnut_container}>
                    {!isVisitsEmpty ? <CustomDoughnutChart data={visitsStatus} labels={visitLabels} title="Visits status"/>:
                        <Stack width="100%" height="25rem" justifyContent="center" alignItems="center" border = '3px solid lightgray' borderRadius={100}>
                    <h2>No data Available</h2>
            </Stack>}
                </div>
            </Stack>
            <CustomLineChart data={data.delayStats} property={"totalDelayTime"} title="Delay time" />
        </div>
    );
}
