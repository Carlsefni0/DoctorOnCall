import styles from './medicine_item.module.css'
export default function MedicineItem({medicine}) {
    return <div className={styles.medicine_item_container}>
        <img src={medicine.imageUrl} alt="Medicine" className={styles.img} />
        <div className={styles.medicine_quantity}>x{medicine.quantity}</div>
        <div className={styles.medicine_info}>
            <div>
                <p className={styles.medicine_title}>{medicine.name}</p>
                <p className={styles.medicine_description}>{medicine.description}</p>
            </div>
            <p className={styles.medicine_price}>{medicine.unitPrice}$</p>
        </div>
    </div>
}