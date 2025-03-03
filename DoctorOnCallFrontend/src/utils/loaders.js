import {queryClient} from "./queryClient.js";
import performRequest from "./performRequest.js";

export async function loader({ endpoint, params, filters }) {
    const apiBaseUrl = import.meta.env.VITE_BACKEND_URL;
    let url = new URL(`${apiBaseUrl}${endpoint}`);

    if (filters) {
        Object.entries(filters).forEach(([key, value]) => {
            if (value) url.searchParams.append(key, value);
        });
    }

    if (params) {
        url = new URL(`${url.toString()}/${params}`);
    }

    console.log(url.toString());

    return queryClient.fetchQuery({
        queryKey: [endpoint, params, filters],
        queryFn: async () => performRequest(url.toString()),
        staleTime: 1000 * 60 * 5,
    });
}



export async function fetchData({ page, controller,...filters }) {
    const apiBaseUrl = import.meta.env.VITE_BACKEND_URL;
    const accessToken = localStorage.getItem("accessToken");
    
    console.log(accessToken);
    
    const queryParams = new URLSearchParams({
        PageNumber: page,
        ...filters,
    });
    const url = `${apiBaseUrl}/${controller}?${queryParams.toString()}`;
    


    const response = await fetch(url, {
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
    });

    if (!response.ok) {
        throw new Error("Failed to fetch doctors");
    }

    return response.json();
}


