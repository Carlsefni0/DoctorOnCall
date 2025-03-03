import styles from "./edit_button.module.css";
import {useNavigate} from "react-router-dom";
import edit from "../../assets/edit.svg";

export default function EditButton({recordId}) {
    const navigate = useNavigate();
    const handleEdit=()=> {
        navigate(`../edit/${recordId}`);
    }
    return (
        <button className={styles.edit_btn} onClick={handleEdit}>
            <img src={edit} alt="edit"/>
        </button>)
    
}