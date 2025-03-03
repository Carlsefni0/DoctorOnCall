import styles from "../styles/error_page_styles.module.css";
import {useLocation, useRouteError} from "react-router-dom";
import StyledButton from "../components/StyledButton.jsx";

export default function ErrorPage() {
    const error = useRouteError();
    const location = useLocation();
    const errorMessage = error?.message || location.state?.message || "Something went wrong.";
    const errorDetails =
        error?.data && typeof error.data === "string"
            ? JSON.parse(error.data)
            : null;

    return (
        <div className={styles.error_page_container}>
            <div className={styles.error_message_wrapper}>
                <h1>Ooops!</h1>
                <p>{errorMessage}</p>
                {errorDetails?.details && (
                    <div className={styles.error_details}>
                        <h3>Details:</h3>
                        <pre>{errorDetails.details}</pre>
                    </div>
                )}
                <StyledButton onClick={() => window.history.back()} width='40%' marginTop='2rem'>
                    Go Back
                </StyledButton>
            </div>
        </div>
    );
}
