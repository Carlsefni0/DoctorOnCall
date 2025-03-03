import styles from "./auth_page_styles.module.css";
import { useState } from "react";
import StyledTextField from "../../components/StyledTextField.jsx";
import { Stack } from "@mui/material";
import StyledButton from "../../components/StyledButton.jsx";
import { useMutation } from "@tanstack/react-query";
import {NavLink, useNavigate} from "react-router-dom";
import performRequest from "../../utils/performRequest.js";
import { getUser } from "../../context/UserContext.jsx";
import {jwtDecode} from "jwt-decode";
export default function LoginPage() {
    const navigate = useNavigate();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [errors, setErrors] = useState({});
    const baseUrl = import.meta.env.VITE_BACKEND_URL;
    const { dispatch } = getUser();

    const logIn = useMutation({
        mutationFn: (data) => performRequest(`${baseUrl}/auth/login`, 'POST', data),
        onSuccess: (data) => {

            localStorage.setItem("accessToken", data.token);

            const decodedToken = jwtDecode(data.token);

            dispatch({
                type: "LOGIN",
                payload: {
                    id: parseInt(decodedToken.nameid),
                    email: decodedToken.email,
                    firstName: decodedToken.given_name,
                    lastName: decodedToken.family_name,
                    role: decodedToken.role,
                },
            });
            
            if(decodedToken.role === 'Admin')
                navigate("../../dashboard/analytics");
            if(decodedToken.role === 'Doctor')
                navigate("../../dashboard/assigned-visits");
            if(decodedToken.role === 'Patient')
                navigate("../../dashboard/visits");
        },
        onError: (error) => {
            if (error.message.includes("email")) {
                setErrors((prev) => ({ ...prev, email: error.message }));
            }
            if (error.message.includes("password")) {
                setErrors((prev) => ({ ...prev, password: error.message }));
            }
        },
    });

    const handleSubmit = (e) => {
        e.preventDefault();
        setErrors({});
        logIn.mutate({ email, password });
    };

    return (
        <div className={styles.auth_page_container}>
            <div className={styles.form_wrapper}>
                <h1>Welcome!</h1>
                <form onSubmit={handleSubmit} className={styles.auth_form}>
                    <Stack spacing={4}>
                        <StyledTextField
                            required
                            label="Email"
                            type="email"
                            name="email"
                            borderColor="white"
                            placeholder="Email"
                            labelColor="#326976"
                            color="#326976"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            error={!!errors.email}
                            helperText={errors.email}
                        />
                        <StyledTextField
                            required
                            label="Password"
                            type="password"
                            name="password"
                            borderColor="white"
                            placeholder="Password"
                            labelColor="#326976"
                            color="#326976"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            error={!!errors.password}
                            helperText={errors.password}
                        />
                    </Stack>

                    <NavLink className={styles.nav_link} to="../forgot">
                        Forgot Password?
                    </NavLink>

                    <StyledButton type="submit" disabled={logIn.isPending}>
                        {logIn.isPending ? "Logging in..." : "Log In"}
                    </StyledButton>
                </form>
            </div>
        </div>
    );
}
