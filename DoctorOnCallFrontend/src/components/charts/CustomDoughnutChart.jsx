import React from 'react';
import { Doughnut } from 'react-chartjs-2';
import {
    Chart as ChartJS,
    CategoryScale,
    ArcElement,
    Title,
    Tooltip,
    Legend,
} from 'chart.js';

ChartJS.register(CategoryScale, ArcElement, Title, Tooltip, Legend);

// Масив з назвами місяців
const monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'June', 'July', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec'];

export default function CustomDoughnutChart({ data, labels, title }) {
    // Форматуємо мітки (labels) для відображення назв місяців
    // const labels = data.map(item => {
    //     const monthIndex = item.month - 1; // Місяці в JavaScript нумеруються з 0
    //     return `${monthNames[monthIndex]}`;
    // });

    const chartData = {
        labels,
        datasets: [
            {
                label: title,
                data: Object.values(data),
                backgroundColor: [
                    'rgba(75, 192, 192, 0.6)',
                    'rgba(54, 162, 235, 0.6)',
                    'rgba(255, 206, 86, 0.6)',
                    'rgba(231, 111, 81, 0.6)',
                    'rgba(244, 162, 97, 0.6)',
                    'rgba(233, 196, 106, 0.6)',
                    'rgba(42, 157, 143, 0.6)',
                    'rgba(38, 70, 83, 0.6)',
                    'rgba(129, 178, 154, 0.6)',
                    'rgba(145, 81, 81, 0.6)',
                    'rgba(255, 99, 132, 0.6)',
                    'rgba(153, 102, 255, 0.6)',
                ],
                borderColor: 'rgba(255, 255, 255, 1)',
                color: 'red',
                borderWidth: 1,
            },
        ],
    };

    const options = {
        animation: {
            animateScale: true,
        },
        // radius:'50%',
        responsive: true,
        plugins: {
            legend: {
                display: false,
                position: 'bottom',
            },
            title: {
                display: true,
                text: title,
                position: "bottom",
                font:{
                    family: "Josefin Sans, serif",
                    weight: 200,
                    size: 18
                },
                color: '#088172',
            },
        },
    };
// Приклад даних
    // const data = [
    //     {
    //         year: 2025,
    //         month: 2,
    //         totalDistance: 5830,
    //         averageDistance: 2915,
    //         totalTravelCost: 2915,
    //         totalTravelTime: 8,
    //         averageTravelTime: 4,
    //     },
    //     {
    //         year: 2025,
    //         month: 2,
    //         totalDistance: 7830,
    //         averageDistance: 2915,
    //         totalTravelCost: 2915,
    //         totalTravelTime: 28,
    //         averageTravelTime: 4,
    //     },
    //     {
    //         year: 2025,
    //         month: 3,
    //         totalDistance: 3830,
    //         averageDistance: 2915,
    //         totalTravelCost: 2915,
    //         totalTravelTime: 18,
    //         averageTravelTime: 4,
    //     },
    //     {
    //         year: 2025,
    //         month: 4,
    //         totalDistance: 6830,
    //         averageDistance: 2915,
    //         totalTravelCost: 2915,
    //         totalTravelTime: 14,
    //         averageTravelTime: 4,
    //     },
    //     {
    //         year: 2025,
    //         month: 4,
    //         totalDistance: 5830,
    //         averageDistance: 2915,
    //         totalTravelCost: 2915,
    //         totalTravelTime: 34,
    //         averageTravelTime: 4,
    //     },
    //     {
    //         year: 2025,
    //         month: 4,
    //         totalDistance: 1830,
    //         averageDistance: 2915,
    //         totalTravelCost: 2915,
    //         totalTravelTime: 56,
    //         averageTravelTime: 4,
    //     },
    //     {
    //         year: 2025,
    //         month: 4,
    //         totalDistance: 3830,
    //         averageDistance: 2915,
    //         totalTravelCost: 2915,
    //         totalTravelTime: 83,
    //         averageTravelTime: 4,
    //     },
    //     {
    //         year: 2025,
    //         month: 4,
    //         totalDistance: 3830,
    //         averageDistance: 2915,
    //         totalTravelCost: 2915,
    //         totalTravelTime: 45,
    //         averageTravelTime: 4,
    //     },
    //    
    //     // Додайте інші місяці за потреби
    // ];
    return (
        <Doughnut
            options={options}
            data={chartData}
            width={100}
            height={100}
        />
    );
}
