import {useEffect, useState} from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import {CircularProgress, Stack} from "@mui/material";
import CustomTable from "./table/CustomTable.jsx";
import CustomSpeedDial from "./CustomSpeedDial.jsx";
import StyledPagination from "./StyledPagination.jsx";
import {getUser} from "../context/UserContext.jsx";
import performRequest from "../utils/performRequest.js";

export default function GeneralListPage({ getColumns, initialData, filters, controller, isLoadingInitial, actions = []}) {
    const navigate = useNavigate();
    const [searchParams, setSearchParams] = useSearchParams();

    const {user} = getUser(); // Отримуємо авторизованого користувача

    // Перевіряємо, чи є doctorId у глобальному кеші
    let doctorId;

    // Якщо лікар і doctorId ще не збережено – зберігаємо його
    useEffect(() => {
        if (user?.role === "Admin" && !doctorId) {
            doctorId = user.id;
        }
    }, [user, doctorId]);

    // Витягуємо параметри запиту
    const filtersFromUrl = Object.fromEntries(searchParams.entries());

   
    const currentPage = parseInt(searchParams.get("page") || "1");

    const { data, isLoading } = useQuery({
        queryKey: [controller, currentPage, filtersFromUrl, filters],
        queryFn: () => {
            const apiBaseUrl = import.meta.env.VITE_BACKEND_URL;
            const queryParams = new URLSearchParams({
                PageNumber: currentPage,
                ...filtersFromUrl
            });

            Object.entries(filters).forEach(([key, values]) => {
                if (Array.isArray(values)) {
                    values.forEach(value => queryParams.append(key, value));
                } else if (values) {
                    queryParams.append(key, values);
                }
            });

            const url = `${apiBaseUrl}/${controller}?${queryParams.toString()}`;
            return performRequest(url, "GET");
        },
        initialData: currentPage === 1 ? initialData : undefined,
        keepPreviousData: true,
    });





    const columns = getColumns();

    const handleRowClick = (row) => {
        navigate(`${row.id}`);
    };

    const handlePageChange = (event, newPage) => {
        setSearchParams((prev) => {
            prev.set("page", newPage.toString());
            return prev;
        });
    };
    
    return (
        <>
            {isLoadingInitial ?   <Stack alignItems='center' justifyContent='center' width='100%' height='100%'>
                    <CircularProgress sx={{color: '#088172'}} size='5rem'/>
                </Stack> :
                <>
                    <CustomTable
                        columns={columns}
                        rows={data?.items || []}
                        onRowClick={handleRowClick}
                    />

                    <Stack spacing={2} alignItems="center" marginTop='2rem' marginLeft='2.5rem' width='90%'>
                        <StyledPagination
                            count={data?.totalPages || 1}
                            page={currentPage}
                            onChange={handlePageChange}
                            color="primary"
                            showFirstButton
                            showLastButton
                            siblingCount={1}
                            boundaryCount={1}
                        />
                    </Stack>

                    <CustomSpeedDial actions={actions} />
                    
                </>}
        </>
    );
}

