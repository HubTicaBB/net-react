import api from "./api";
import type { RegisterDto, LoginDto, AuthResponseDto } from "../types/api";

export const authService = {
  // Register a new user
  // Tokens are automatically set as httpOnly cookies by the backend
  register: async (data: RegisterDto): Promise<AuthResponseDto> => {
    const response = await api.post<AuthResponseDto>(
      "/api/auth/register",
      data
    );
    return response.data;
  },

  // Login user
  // Tokens are automatically set as httpOnly cookies by the backend
  login: async (data: LoginDto): Promise<AuthResponseDto> => {
    const response = await api.post<AuthResponseDto>("/api/auth/login", data);
    return response.data;
  },

  // Refresh access token
  // Refresh token is read from httpOnly cookie by the backend
  refreshToken: async (): Promise<AuthResponseDto> => {
    const response = await api.post<AuthResponseDto>(
      "/api/auth/refresh",
      {} // Empty body - token comes from cookie
    );
    return response.data;
  },

  // Logout
  // Backend clears the httpOnly cookies
  logout: async (): Promise<void> => {
    await api.post("/api/auth/logout");
  },
};
