import { useNavigate } from "react-router-dom";
import { getUser } from "../context/UserContext.jsx";

const RouterGuard = ({ children, requiredRoles }) => {
     const { user } = getUser();
     const navigate = useNavigate();

     if (!user || user.isAuthenticated === false) {
          return null;
     }

     if (!user.role || !requiredRoles.includes(user.role)) {
          throw new Error("You don't have permission to visit this page.");
     }

     return children;
};

export default RouterGuard;
