import React, { useRef, useEffect } from 'react';
import { Line } from 'react-chartjs-2';
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
    Filler,
} from 'chart.js';

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
    Filler
);

const monthNames = ['Jan',	'Feb',	'Mar','Apr','May','June','July','Aug','Sept','Oct','Nov','Dec'];

export default function CustomLineChart({ data, property, title }) {
    const chartRef = useRef(null);

    useEffect(() => {
        if (chartRef.current) {
            const ctx = chartRef.current.canvas.getContext('2d');
            const gradient = ctx.createLinearGradient(0, 0, 0, 400);
            gradient.addColorStop(0, 'rgba(56,228,205,0.6)');
            gradient.addColorStop(0, 'rgba(169,227,220,0.74)');
            gradient.addColorStop(1, 'rgba(255,255,255,0.46)'); 

            chartRef.current.data.datasets[0].backgroundColor = gradient;
            chartRef.current.update();
        }
    }, [data]);

    const labels = Array.isArray(data) ? data.map(item => {
        const monthIndex = parseInt(item.groupIdentifier) - 1;
        const isDate = item.groupIdentifier.includes('-');
        const isWeek = item.groupIdentifier.includes('week')
        return isDate || isWeek ? `${item.groupIdentifier}` : monthNames[monthIndex];
    }) : [];

    const chartData = {
        labels: labels,
        datasets: [
            {
                label: title,
                data: Array.isArray(data) ? data.map(item => item[property]) : [],
                borderColor: 'rgba(37,175,157,1)',
                backgroundColor: 'rgb(250,248,248)',
                fill: true,
                tension: 0.4,
            },
        ],
    };


    const options = {
        responsive: true,
        plugins: {
            legend: {
                display: false,
                position: 'bottom',
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
                grid: {
                    display: false,
                },
                border: {
                    display: false, 
                },
                ticks: {
                    display: true, 
                },
            },
            y: {
                beginAtZero: true,
                grid: {
                    display: false, 
                },
                border: {
                    display: false, 
                },
                ticks: {
                    display: true, 
                },
            },
        },
    };

    return (
        <Line
            ref={chartRef}
            options={options}
            data={chartData}
            width={1450}
            height={450}
        />
    );
}