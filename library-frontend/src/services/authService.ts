import api from "./api";
import type {
  RegisterDto,
  LoginDto,
  AuthResponseDto,
  RefreshTokenDto,
} from "../types/api";

export const authService = {
  // Register a new user
  register: async (data: RegisterDto): Promise<AuthResponseDto> => {
    const response = await api.post<AuthResponseDto>(
      "/api/auth/register",
      data
    );
    return response.data;
  },

  // Login user
  login: async (data: LoginDto): Promise<AuthResponseDto> => {
    const response = await api.post<AuthResponseDto>("/api/auth/login", data);
    return response.data;
  },

  // Refresh access token
  refreshToken: async (data: RefreshTokenDto): Promise<AuthResponseDto> => {
    const response = await api.post<AuthResponseDto>("/api/auth/refresh", data);
    return response.data;
  },
};
