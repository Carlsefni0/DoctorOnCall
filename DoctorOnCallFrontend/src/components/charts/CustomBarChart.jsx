import React from 'react';
import { Bar } from 'react-chartjs-2';
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    BarElement,
    Title,
    Tooltip,
    Legend,
} from 'chart.js';

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

const monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'June', 'July', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec'];

export default function CustomBarChart({ data, title }) {
    
console.log(Object.values(data))
    const labels = Array.isArray(data) ? data.map(item => {
        const monthIndex = parseInt(item.groupIdentifier) - 1;
        const isDate = item.groupIdentifier.includes('-');
        const isWeek = item.groupIdentifier.includes('week')
        return isDate || isWeek ? `${item.groupIdentifier}` : monthNames[monthIndex];
    }) : [];
    const chartData = {
        labels,
        datasets: [
            {
                label: title,
                data: Object.values(data).map(item => item.totalMedicineCost),
                backgroundColor: 'rgba(37,175,157,1)',
                borderColor: 'rgb(24,126,113)',
                borderWidth: 1,
            },
        ],
    };

    // Опції для графіка
    const options = {
        responsive: true,
        plugins: {
            legend: {
                display: false,
                position: 'top',
            },
            title: {
                display: true,
                text: title,
                font:{
                    family: "Josefin Sans, serif",
                    weight: 200,
                    size: 18
                },
                color: '#088172',
            },
        },
        scales: {
            x: {
                title: {
                    display: false,
                    font: {
                        size: 14,
                    },
                },
            },
            y: {
                title: {
                    display: true,
                    font: {
                        size: 14,
                    },
                },
                beginAtZero: true,
            },
        },
    };

    return (
        <Bar
            options={options}
            data={chartData}
            width={1750}
            height={600}
        />
    );
}
