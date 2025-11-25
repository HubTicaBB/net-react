import axios, {
  type AxiosError,
  type AxiosInstance,
  type InternalAxiosRequestConfig,
} from "axios";
import type { AuthResponseDto } from "../types/api";

// Get API base URL from environment variables
const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL || "http://localhost:5146";

// Create Axios instance
const api: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true, // Important: Send cookies with requests
});

// Note: No request interceptor needed!
// httpOnly cookies are automatically sent by the browser
// We don't need to manually add Authorization headers

// Response Interceptor: Handle errors and token refresh
api.interceptors.response.use(
  (response) => {
    // If request succeeds, just return the response
    return response;
  },
  async (error: AxiosError) => {
    const originalRequest = error.config as InternalAxiosRequestConfig & {
      _retry?: boolean;
    };

    // If error is 401 (Unauthorized) and we haven't tried to refresh yet
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        // Call refresh endpoint - cookies are sent automatically
        // No need to pass tokens in body, they're in cookies
        await api.post<AuthResponseDto>("/api/auth/refresh", {});

        // Tokens are now in httpOnly cookies, automatically set by backend
        // Just retry the original request
        return api(originalRequest);
      } catch (refreshError) {
        // If refresh fails, redirect to login
        // Cookies will be cleared by backend on logout
        window.location.href = "/login";
        return Promise.reject(refreshError);
      }
    }

    // For other errors, just reject with the error
    return Promise.reject(error);
  }
);

export default api;
