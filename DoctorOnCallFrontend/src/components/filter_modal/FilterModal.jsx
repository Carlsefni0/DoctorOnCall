import {Dialog, Fade, Grow} from "@mui/material";
import styles from "./filter_modal.module.css";

export default function FilterModal({ open, onClose, children, width, height }) {
    return (
        <Dialog
            open={open}
            onClose={onClose}
            BackdropProps={{
                style: {
                    backgroundColor: "rgba(255, 255, 255, 0.2)",
                    backdropFilter: "blur(2px)"
                }
            }}
            TransitionComponent={Grow}
            transitionDuration={500}
            PaperProps={{
                style: { width, height, padding: "2rem" },
            }}
        >
            {children}
        </Dialog>
    );
};
