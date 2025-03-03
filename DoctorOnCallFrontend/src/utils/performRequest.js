
const performRequest = async (url, method = "GET", body = null, customHeaders = {}) => {
    const accessToken = localStorage.getItem("accessToken");

    const headers = {
        "Content-Type": "application/json",
        ...customHeaders
    };

    if (accessToken) {
        headers["Authorization"] = `Bearer ${accessToken}`;
    }

    const options = {
        method,
        headers,
        ...(body ? { body: JSON.stringify(body) } : {}),
    };

    try {
        const response = await fetch(url, options);

        // Якщо статус 204 (No Content), повертаємо null, щоб очистити кеш у React Query
        if (response.status === 204) {
            return null;
        }

        // Якщо є вміст, читаємо JSON
        const data = response.headers.get("Content-Length") !== "0" ? await response.json() : null;

        if (!response.ok) {
            throw data || new Error("Request failed");
        }

        return data;
    } catch (error) {
        console.error("Request error:", error);
        throw error;
    }
};

export default performRequest;


