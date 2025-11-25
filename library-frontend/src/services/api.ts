import axios, {
  type AxiosError,
  type AxiosInstance,
  type InternalAxiosRequestConfig,
} from "axios";
import type { AuthResponseDto, RefreshTokenDto } from "../types/api";

// Get API base URL from environment variables
const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL || "http://localhost:5146";

// Create Axios instance
const api: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Request Interceptor: Add JWT token to all requests
api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    // Get token from localStorage
    const token = localStorage.getItem("token");

    // If token exists, add it to Authorization header
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

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
        // Try to refresh the token
        const refreshToken = localStorage.getItem("refreshToken");

        if (refreshToken) {
          const refreshData: RefreshTokenDto = {
            token: localStorage.getItem("token") || "",
            refreshToken: refreshToken,
          };

          // Call refresh endpoint
          const response = await axios.post<AuthResponseDto>(
            `${API_BASE_URL}/api/auth/refresh`,
            refreshData
          );

          const { token, refreshToken: newRefreshToken } = response.data;

          // Update tokens in localStorage
          localStorage.setItem("token", token);
          localStorage.setItem("refreshToken", newRefreshToken);

          // Update the original request with new token
          if (originalRequest.headers) {
            originalRequest.headers.Authorization = `Bearer ${token}`;
          }

          // Retry the original request
          return api(originalRequest);
        }
      } catch (refreshError) {
        // If refresh fails, clear tokens and redirect to login
        localStorage.removeItem("token");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("user");

        // Redirect to login page (we'll implement routing later)
        window.location.href = "/login";
        return Promise.reject(refreshError);
      }
    }

    // For other errors, just reject with the error
    return Promise.reject(error);
  }
);

export default api;
