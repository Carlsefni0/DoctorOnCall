import { Divider } from "@mui/material";
import styles from "./generic_user_details.module.css";
import StyledPaper from "../StyledPaper.jsx";
import ReturnButton from "../return-button/ReturnButton.jsx";
import StyledAvatar from "../StyledAvatar.jsx";
import EditButton from "../edit_button/EditButton.jsx";

export default function GenericUserDetailsPage({title, avatarText, email, details = [], children, id}) {
    return (
        <StyledPaper elevation={3} width="60%" marginLeft="15rem" height="95%">
            <div className={styles.header}>
                <ReturnButton />
                <div className={styles.avatar_wrapper}>
                    <StyledAvatar {...avatarText} width="6rem" height="6rem" />
                    <p>{title}</p>
                    <p className={styles.email}>{email}</p>
                </div>
                <EditButton recordId={id}/>
            </div>
            <div className={styles.details_info}>
                <Divider>Basic Information</Divider>
                <div className={styles.info}>
                    {details.map(({ label, value}) => (
                        <dl className={styles.property_wrapper}>
                            <dt>{label}</dt>
                            <dd>{value}</dd>
                        </dl>
                    ))}
                </div>
            </div>
            {children}
        </StyledPaper>
    );
}
