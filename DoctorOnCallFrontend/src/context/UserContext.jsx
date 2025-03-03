import { createContext, useContext, useReducer, useEffect } from "react";
import {jwtDecode}from "jwt-decode";

const UserContext = createContext(null);

const initialState = {
    id: null,
    email: "",
    firstName: "",
    lastName: "",
    role: "",
    isAuthenticated: false,
};

function userReducer(state, action) {
    switch (action.type) {
        case "LOGIN":
            return { ...state, ...action.payload, isAuthenticated: true };
        case "LOGOUT":
            localStorage.removeItem("accessToken");
            localStorage.removeItem("user");
            return { ...initialState };
        default:
            return state;
    }
}

export function UserProvider({ children }) {
    const [state, dispatch] = useReducer(userReducer, initialState);

    useEffect(() => {
        const token = localStorage.getItem("accessToken");
        if (token) {
            try {
                const decodedToken = jwtDecode(token);

                const currentTime = Date.now() / 1000;
                if (decodedToken.exp && decodedToken.exp < currentTime) {
                    console.warn("Token expired. Logging out.");
                    dispatch({ type: "LOGOUT" });
                } else {
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
                }
            } catch (error) {
                console.error("Invalid token:", error);
                dispatch({ type: "LOGOUT" });
            }
        }
    }, []);

    return (
        <UserContext.Provider value={{ user: state, dispatch }}>
            {children}
        </UserContext.Provider>
    );
}

export function getUser() {
    return useContext(UserContext);
}
