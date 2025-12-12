import axios from "axios";

const api = axios.create({
  baseURL:
    import.meta.env.VITE_API_URL ?? "https://localhost:7029/api/v1",
});

export default api;

