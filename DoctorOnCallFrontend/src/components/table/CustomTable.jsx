import React, { useState } from "react";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import { styled } from "@mui/material/styles";
import {RiseLoader} from "react-spinners";
import StyledPaper from "../StyledPaper.jsx";

export default function CustomTable({columns, rows, onRowClick, pagination, isLoading}) {
    const [localPage, setLocalPage] = useState(0);
    const [localRowsPerPage, setLocalRowsPerPage] = useState(10);
   
    const StyledTableContainer = styled(TableContainer)(({ theme }) => ({
        borderTopRightRadius: "1rem",
        borderTopLeftRadius: "1rem",
        overflow: "auto",
        boxShadow: "0 4px 10px rgba(0, 0, 0, 0.1)",
        width: "100%",
        height: "72vh"
    }));

    const StyledTableRow = styled(TableRow)(({ theme }) => ({
        "&:hover": {
            backgroundColor: "#eef1f1",
            cursor: onRowClick ? "pointer" : "default",
        },
        "& .MuiTableCell-root": {
            borderBottom: "none",
        },
        border: "none",
    }));

    const StyledTableCell = styled(TableCell)(({ theme }) => ({
        fontSize: "0.8rem",
        fontFamily: "Josefin Sans, serif",
        textAlign: "center",
    }));

    const StyledTableHeadCell = styled(StyledTableCell)(({ theme }) => ({
        backgroundColor: "#088172",
        color: "#fff",
        fontWeight: "bold",
        position: "sticky",
        top: 0,
        zIndex: 1,
        textAlign: "center",
        verticalAlign: "middle",
    }));
    
    const displayedRows = pagination
        ? rows
        : rows.slice(
            localPage * localRowsPerPage,
            localPage * localRowsPerPage + localRowsPerPage
        );

    return (
        <StyledPaper width='90%'  marginLeft='2.5rem'>
            <StyledTableContainer>
                <Table stickyHeader>
                    <TableHead>
                        <TableRow>
                            {columns.map((column) => (
                                <StyledTableHeadCell key={column.field}>
                                    {column.headerName}
                                </StyledTableHeadCell>
                            ))}
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {displayedRows.length > 0 ? (
                            displayedRows.map((row) => (
                                <StyledTableRow
                                    key={row.id}
                                    onClick={() => onRowClick && onRowClick(row)}
                                >
                                    {columns.map((column) => (
                                        <StyledTableCell key={column.field}>
                                            {column.render ? column.render(row[column.field]) : row[column.field]}

                                        </StyledTableCell>
                                    ))}
                                </StyledTableRow>
                            ))
                        ) : (
                            <StyledTableRow>
                                <StyledTableCell colSpan={columns.length} align="center">
                                    No data available
                                </StyledTableCell>
                            </StyledTableRow>
                        )}
                    </TableBody>
                </Table>
            </StyledTableContainer>
        </StyledPaper>
    );
};


