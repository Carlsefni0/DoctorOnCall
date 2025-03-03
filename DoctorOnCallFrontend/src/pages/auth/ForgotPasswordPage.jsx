import styles from "../../styles/auth/forgot_page.module.css"
import {Form, NavLink, useActionData} from "react-router-dom";
import StyledButton from "../../components/StyledButton.jsx";
import StyledTextField from "../../components/StyledTextField.jsx";

export default function ForgotPasswordPage() {
    const error = useActionData() || null;
    const isError = error !== undefined && error !== null;

    return (
        <div className={styles.forgot_page_container}>
            <div className={styles.forgot_wrapper}>
                <h1>Forgot Password</h1>
                <h2>Enter the email which is linked to this account. We will send a link to reset the password</h2>
                <Form method="post" className={styles.form_wrapper}>
                    <StyledTextField
                        required
                        label="Email"
                        type="email"
                        name="email"
                        borderColor="white"
                        placeholder="Email"
                        labelColor="#326976"
                        color="#326976"
                        error={isError}
                        helperText={isError && error.message}
                        marginTop={"1.5rem"}
                        width={"90%"}
                    />
                    <StyledButton width='50%' marginTop='2rem'>Send Email</StyledButton>
                </Form>
                <NavLink className={styles.nav_link} to="../login">Log In</NavLink>
                
            </div>
        </div>
    )
}
export async function action({ request }) {
    const formData = await request.formData();
    const email = formData.get("email");
    console.log(email)
    

    try {
        const response = await fetch("http://localhost:5083/api/auth/forgot-password", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email }),
        });
        console.log("dsfsdf")

        if (!response.ok) {

            const errorData = await response.json();

            if (response.status >= 500) {
                throw new Error("Server error. Please try again later.");
            }
            
            return { message: errorData.message};
        }

        return { success: true };
    } catch (error) {
        console.error(error);
        throw new Error("Unable to connect to the server. Please try again later.", { status: 500 });
    }
}
