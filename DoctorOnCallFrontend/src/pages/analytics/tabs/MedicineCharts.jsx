import {Stack} from "@mui/material";
import styles from "./medicine_charts.module.css";
import StatisticItem from "../../../components/statistics_item/StatisticsItem.jsx";
import CustomBarChart from "../../../components/charts/CustomBarChart.jsx";

export default function MedicineCharts({ data}) {
    if (!data || data.groupedStats.length === 0) {
        return (
            <Stack width="100%" height="45vh" justifyContent="center" alignItems="center">
                <h2>No Data Available</h2>
            </Stack>
        );
    }
    return (
        <>
            <Stack direction="row" justifyContent="center" spacing={2}>
                <StatisticItem value={`${data?.medicineCostSum || 0}$`} label="Total medicine cost" />
            </Stack>
            <div className={styles.bar_chart_container}>
                <CustomBarChart data={data.groupedStats} title="Medicine cost"/>
                
            </div>
        </>
    );
}