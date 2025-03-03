import React, { useState } from "react";
import styles from "./header.module.css";
import logo from '../../assets/logo.svg';
import StyledAvatar from "../StyledAvatar.jsx";
import stringAvatar from "../../utils/stringToAvatar.js";
import { Stack, Menu, MenuItem } from "@mui/material";
import { getUser } from "../../context/UserContext.jsx";
import {useNavigate} from "react-router-dom";

export default function Header() {
    const navigate = useNavigate();
    const { user,dispatch } = getUser();
    const userFullName = `${user.firstName} ${user.lastName}`;

    const [anchorEl, setAnchorEl] = useState(null);
    const isMenuOpen = Boolean(anchorEl);
    

    const handleMenuClose = () => {
        setAnchorEl(null);
    };

    const handleProfileClick = () => {
        navigate('/profile');
        handleMenuClose();
    };

    const handleLogoutClick = () => {
        dispatch({type: "LOGOUT"})
        navigate('/auth/login');
        handleMenuClose();
    };

    return (
        <header className={styles.header_container}>
            <div className={styles.logo_container}>
                <img src={logo} alt="logo" />
                <h1>Doctor On Call</h1>
            </div>
            <Stack direction="row" alignItems="center" spacing={2}>
                <StyledAvatar
                    {...stringAvatar(userFullName)}
                    width="2.5rem"
                    height="2.5rem"
                    onClick={(event) => setAnchorEl(event.currentTarget)} // Додаємо обробник кліку
                    style={{ cursor: "pointer" }} // Робимо аватарку клікабельною
                />
                <Stack>
                    <p>{userFullName}</p>
                    <p className={styles.email}>{user.email}</p>
                </Stack>
            </Stack>

            <Menu
                anchorEl={anchorEl}
                open={isMenuOpen}
                onClose={()=> setAnchorEl(null)}
                anchorOrigin={{
                    vertical: "bottom",
                    horizontal: "right",
                }}
                transformOrigin={{
                    vertical: "top",
                    horizontal: "right",
                }}
            >
                <MenuItem onClick={handleProfileClick}>Profile</MenuItem>
                <MenuItem onClick={handleLogoutClick}><p className={styles.log_out}>Log Out</p></MenuItem>
            </Menu>
        </header>
    );
}
