import {createBrowserRouter, Navigate, Outlet, RouterProvider} from "react-router-dom";
import { QueryClientProvider} from "@tanstack/react-query"
import { queryClient } from "./utils/queryClient.js"
import LoginPage from "./pages/auth/LoginPage.jsx";
import ForgotPasswordPage, { action as ForgotAction } from "./pages/auth/ForgotPasswordPage.jsx";
import ErrorPage from "./pages/ErrorPage.jsx";
import Dashboard from "./pages/DashboardPage.jsx";
import DoctorsPage from "./pages/doctor/DoctorsPage.jsx";
import {loader as DataLoader} from "./utils/loaders.js"
import DoctorDetailsPage from "./pages/doctor/DoctorDetailsPage.jsx";
import PatientsPage from "./pages/patient/PatientsPage.jsx";
import RequestsPage from "./pages/request/RequestsPage.jsx";
import RequestDetailsPage from "./pages/request/RequestDetailsPage.jsx";
import ScheduleCreatingPage from "./pages/schedule/ScheduleCreatingPage.jsx";
import ScheduleDetailsPage from "./pages/schedule/ScheduleDetailsPage.jsx";
import SchedulesPage from "./pages/schedule/SchedulesPage.jsx";
import PatientDetailsPage from "./pages/patient/PatientDetailsPage.jsx";
import PatientForm from "./pages/patient/PatientForm.jsx";
import DoctorForm from "./pages/doctor/DoctorForm.jsx";
import AssignedRequests from "./pages/assigned_requests/AssignedRequests.jsx";
import CurrentVisits from "./pages/current_visits/CurrentVisits.jsx";
import ScheduleExceptionsPage from "./pages/schedule_exceptions/ScheduleExceptionsPage.jsx";
import ScheduleExceptionForm from "./pages/schedule_exceptions/ScheduleExceptionForm.jsx";
import ScheduleExceptionDetailPage from "./pages/schedule_exceptions/ScheduleExceptionDetailsPage.jsx";
import AnalyticsPage from "./pages/analytics/AnalyticsPage.jsx";
import {UserProvider} from "./context/UserContext.jsx";
import RouterGuard from "./components/RouterGuard.jsx";
import RequestForm from "./pages/request/RequestForm.jsx";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Navigate to="/auth/login" replace />,
  },
  {
    path: "auth",
    errorElement: <ErrorPage />,
    children: [
      { path: "login", element: <LoginPage /> },
      { path: "forgot", element: <ForgotPasswordPage />, action: ForgotAction },
    ],
  },
  {
    path: "dashboard",
    element: <Dashboard />,
    errorElement: <ErrorPage />,
    children: [
      {
        path: "doctors",
        element: <RouterGuard requiredRoles={['Admin']}><Outlet/></RouterGuard>,
        children: [
          {
            path: "",
            element: <DoctorsPage />,
            loader: () => DataLoader({ endpoint: "/doctor" }),
          },
          {
            path: ":doctorId",
            element: <DoctorDetailsPage/>,
            loader: ({ params }) =>
                DataLoader({ endpoint: "/doctor", params: params.doctorId }),
          },
          { path:'register', element: <DoctorForm/>},
          { path:'edit/:doctorId', element: <DoctorForm/>, loader: ({ params }) => DataLoader({ endpoint: "/doctor", params: params.doctorId }) }
        ],
      },
      { path: "patients",
        element: <RouterGuard requiredRoles={['Admin', 'Doctor']}><Outlet/></RouterGuard>,
        children: [
          {
            path: '',
            element: <PatientsPage />,
            loader: ()=> DataLoader({ endpoint: "/patient" }),
          },
          {
            path:':patientId',
            element: <PatientDetailsPage/>,
            loader: ({ params }) => DataLoader({ endpoint: "/patient", params: params.patientId }),
          },
          { path:'register', element: <RouterGuard requiredRoles={['Admin']}><PatientForm/></RouterGuard> },
          { 
            path:'edit/:patientId', 
            element: <PatientForm/>, 
            loader: ({ params }) => DataLoader({ endpoint: "/patient", params: params.patientId }) 
          }
        ]
      },
      {
        path: "visits", 
        children: [
          {
            path: '',
            element: <RouterGuard requiredRoles={['Admin', 'Doctor', 'Patient']}><RequestsPage /></RouterGuard>,
            loader: ({ request }) => {
              const url = new URL(request.url);
              const filters = Object.fromEntries(url.searchParams.entries());
              return DataLoader({ endpoint: "/visit-request", filters});
            },
          },
          {
            path: ':requestId', element: <RequestDetailsPage />,
            loader: ({ params }) => DataLoader({ endpoint: "/visit-request", params: params.requestId }),
          },
          { path: 'create', element: <RouterGuard requiredRoles={['Patient']}><RequestForm/></RouterGuard>},
          { 
            path: 'edit/:requestId', 
            element: <RouterGuard requiredRoles={['Patient']}><RequestForm /></RouterGuard>,
            loader: ({ params }) => DataLoader({ endpoint: "/visit-request", params: params.requestId })
          }
        ]
      },
      {
        path: "schedules", 
        element: <RouterGuard requiredRoles={['Admin', 'Doctor']}><Outlet/></RouterGuard>,
        children: [
          {
            path: '', 
            element: <SchedulesPage />,
            loader: ({ request }) => {
              const url = new URL(request.url);
              const filters = Object.fromEntries(url.searchParams.entries());
              return DataLoader({ endpoint: "/schedule", filters });
            },
          },
          {
            path: ':scheduleId',  
            element: <ScheduleDetailsPage />, 
            loader: ({ params }) => DataLoader({ endpoint: "/schedule", params: params.scheduleId }),
          },
          {
            path: 'create', element: <ScheduleCreatingPage/>
          }
        ]
      },
      {
        path: "schedule-exceptions", 
        element: <RouterGuard requiredRoles={['Admin', 'Doctor']}><Outlet/></RouterGuard>,
        children: [
          {
            path: '',
            element: <ScheduleExceptionsPage />,
            loader: ({ request }) => {
              const url = new URL(request.url);
              const filters = Object.fromEntries(url.searchParams.entries());
              return DataLoader({ endpoint: "/schedule-exception", filters });
            },
          },
          {
            path: ':scheduleExceptionId',
            element: <ScheduleExceptionDetailPage />,
            loader: ({ params }) => DataLoader({ endpoint: "/schedule-exception", params: params.scheduleExceptionId }),
          },
          {
            path: 'create', element: <RouterGuard requiredRoles={['Doctor']}><ScheduleExceptionForm/></RouterGuard>
          },
          { path:'edit/:scheduleExceptionId', element: <RouterGuard requiredRoles={['Doctor']}><ScheduleExceptionForm/></RouterGuard>, loader: ({ params }) => DataLoader({ endpoint: "/schedule-exception", params: params.scheduleExceptionId }) }
        ]
      },
      { path: "assigned-visits", 
        element: <RouterGuard requiredRoles={['Doctor']}><AssignedRequests/></RouterGuard>,
        loader: ({ params }) => {
          const doctorId = 2;
          const dateStart = new Date(Date.now());
          const dateEnd = new Date(dateStart);
          dateEnd.setMonth(dateEnd.getMonth() + 1);

          const filters = {
            doctorId,
            DateRangeStart: dateStart.toISOString(),
            DateRangeEnd: dateEnd.toISOString()
          };

          return DataLoader({ endpoint: "/visit-request/assigned", filters });
        },
        
      },
      {
        path: 'current-visits',
        element: <RouterGuard requiredRoles={['Doctor']}><CurrentVisits /></RouterGuard>,
        loader: ({ params }) => {
          const doctorId = 2;
          const date = new Date(Date.now()).toISOString();
          const filters = {doctorId, date };
          return  DataLoader({endpoint: "/visit-request/current", filters})
        },
      },
      {
        path: 'analytics',
        element: <RouterGuard requiredRoles={['Admin','Doctor']}><AnalyticsPage /></RouterGuard>,
        loader: ({request}) => {
          const url = new URL(request.url);

          const now = new Date();
          
          const filters = Object.fromEntries(url.searchParams.entries());
          const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 2);
          const endOfMonth = new Date(now.getFullYear(), now.getMonth() + 1, 0, 23, 59, 59);
          
          filters.startDate = startOfMonth.toISOString();
          filters.endDate = endOfMonth.toISOString();
          
          return  DataLoader({endpoint: "/analytics/travel", filters})
        }
      }
    ]
  },
  {
    path: "*",
    // element: <NotFoundPage />,
  },
]);

export default function App() {
  return(<UserProvider>
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  </UserProvider>)
    
}
