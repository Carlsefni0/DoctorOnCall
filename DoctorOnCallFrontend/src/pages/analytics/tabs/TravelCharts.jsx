import CustomLineChart from "../../../components/charts/CustomLineChart.jsx";
import {Divider, Stack} from "@mui/material";
import styles from "./travel_charts.module.css";
import StatisticItem from "../../../components/statistics_item/StatisticsItem.jsx";

export default function TravelCharts({ data}) {
    if (!data || data.groupedStats.length === 0) {
        return (
            <Stack width="100%" height="45vh" justifyContent="center" alignItems="center">
                <h2>No Data Available</h2>
            </Stack>
        );
    }

    const formatNumber = (number) => number.toLocaleString();
    
    return (
        <>
            <Stack direction="row" justifyContent="center" spacing={2}>
                <StatisticItem value={`${formatNumber(data?.travelCostSum || 0)}$`} label="Total travel cost" />
                <Divider orientation="vertical" flexItem />
                <StatisticItem value={`${data?.travelDistanceSum || 0}km`} label="Total travel distance" />
                <Divider orientation="vertical" flexItem />
                <StatisticItem value={`${data?.travelTimeSum || 0}h`} label="Total travel time" />
            </Stack>
            <div className={styles.charts_container}>
                <div className={styles.line_chart_container}>
                    <CustomLineChart data={data.groupedStats} property= "totalTravelCost" title="Travel cost"/>
                </div>
                <div className={styles.line_chart_container}>
                    <CustomLineChart data={data.groupedStats} property="totalDistance" title="Distance"/>
                </div>
                <div className={styles.line_chart_container}>
                    <CustomLineChart data={data.groupedStats} property="totalTravelTime" title="Travel time"/>
                </div>
            </div>
        </>
    );
}