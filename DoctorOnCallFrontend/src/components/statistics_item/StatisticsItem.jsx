import {Divider} from "@mui/material";
import styles from './statistics_item.module.css'

export default function StatisticItem({ value, label }) {
    return (
        <>
            <dl className={styles.data_item}>
                <dt>{value}</dt>
                <dd>{label}</dd>
            </dl>
        </>
    );
}