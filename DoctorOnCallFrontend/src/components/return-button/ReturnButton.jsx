import arrow from "../../assets/arrow.svg";
import styles from './return_button.module.css'

export default function ReturnButton() {

    const handleReturnBack = ()=> {
        window.history.back()
    }
    
    return (
        <button className={styles.return_btn} onClick={handleReturnBack}>
            <img src={arrow} alt="arrow"/>
        </button>)
}