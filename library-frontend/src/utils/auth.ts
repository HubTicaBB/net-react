import type { AuthResponseDto } from "../types/api";

// Store user authentication data
export const saveAuthData = (authData: AuthResponseDto): void => {
  localStorage.setItem("token", authData.token);
  localStorage.setItem("refreshToken", authData.refreshToken);
  localStorage.setItem(
    "user",
    JSON.stringify({
      userId: authData.userId,
      email: authData.email,
      role: authData.role,
    })
  );
};

// Get current user from localStorage
export const getCurrentUser = (): {
  userId: string;
  email: string;
  role: string;
} | null => {
  const userStr = localStorage.getItem("user");
  if (!userStr) return null;
  return JSON.parse(userStr);
};

// Check if user is logged in
export const isAuthenticated = (): boolean => {
  return !!localStorage.getItem("token");
};

// Check if user has a specific role
export const hasRole = (role: string): boolean => {
  const user = getCurrentUser();
  return user?.role === role;
};

// Check if user is a librarian
export const isLibrarian = (): boolean => {
  return hasRole("Librarian");
};

// Check if user is a member
export const isMember = (): boolean => {
  return hasRole("Member");
};

// Clear authentication data (logout)
export const clearAuthData = (): void => {
  localStorage.removeItem("token");
  localStorage.removeItem("refreshToken");
  localStorage.removeItem("user");
};
