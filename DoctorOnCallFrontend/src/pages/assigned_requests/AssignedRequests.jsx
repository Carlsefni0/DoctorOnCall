import { useState } from "react";
import { useCalendarApp, ScheduleXCalendar } from "@schedule-x/react";
import {
    createViewDay,
    createViewMonthAgenda,
    createViewMonthGrid,
    createViewWeek, viewMonthGrid,
    viewWeek,
} from "@schedule-x/calendar";
import { createEventModalPlugin } from "@schedule-x/event-modal";
import { createEventsServicePlugin } from "@schedule-x/events-service";
import "@schedule-x/theme-default/dist/index.css";
import StyledPaper from "../../components/StyledPaper.jsx";
import { useLoaderData, useNavigate } from "react-router-dom";
import performRequest from "../../utils/performRequest.js";
import {queryClient} from "../../utils/queryClient.js";

const formatDateTime = (isoString) => {
    const date = new Date(isoString);
    return date.toISOString().slice(0, 16).replace("T", " ");
};

export default function AssignedRequests() {
    const assignedVisitRequests = useLoaderData();
    const navigate = useNavigate();
    const eventsService = useState(() => createEventsServicePlugin())[0]
    
    const events = assignedVisitRequests.map(request => ({
        id: String(request.visitRequestId),
        title: request.visitRequestType,
        start: formatDateTime(request.startDateTime),
        end: formatDateTime(request.endDateTime),
        location: request.address,
        description: "Double click on the visit to get more information",
        calendarId: "event",
    }))
    
    const calendar = useCalendarApp({
        views: [createViewDay(), createViewWeek(), createViewMonthGrid(), createViewMonthAgenda()],
        defaultView: viewMonthGrid.name,
        events,
        callbacks: {
            onRangeUpdate: async (range) => {
                try {
                    const apiBaseUrl = import.meta.env.VITE_BACKEND_URL;
                    
                    console.log(range);

                    const filters = {
                        doctorId: 2, // або отримуй його динамічно
                        dateRangeStart: range.start,
                        dateRangeEnd: range.end,
                    };

                    const url = new URL(`${apiBaseUrl}/visit-request/assigned`);
                    Object.entries(filters).forEach(([key, value]) => {
                        if (value) url.searchParams.append(key, value);
                    });

                    const newEvents = await performRequest(url.toString());
                    
                    
                    
                    eventsService.set(newEvents.map(request => ({
                        id: String(request.visitRequestId),
                        title: request.visitRequestType,
                        start: formatDateTime(request.startDateTime),
                        end: formatDateTime(request.endDateTime),
                        location: request.address,
                        description: "Double click on the visit to get more information",
                        calendarId: "event",
                    })))
                    
                } catch (error) {
                    console.error("Failed to fetch new events:", error);
                }
            },

            onDoubleClickEvent: (calendarEvent) => {
                navigate(`../visits/${calendarEvent.id}`);
            },
          
        },
        calendars: {
            event: {
                colorName: "event",
                lightColors: {
                    main: "#1cf9b0",
                    container: "#dafff0",
                    onContainer: "#004d3d",
                },
                darkColors: {
                    main: "#c0fff5",
                    onContainer: "#e6fff5",
                    container: "#42a297",
                },
            },
        },
        plugins: [eventsService, createEventModalPlugin()],
    });
    
    

    return (
        <StyledPaper width="90%" height="83vh" marginLeft="2.5rem">
            <ScheduleXCalendar calendarApp={calendar} />
        </StyledPaper>
    );
}
