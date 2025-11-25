import type { AuthResponseDto } from "../types/api";

// Store user data (tokens are in httpOnly cookies, not accessible to JavaScript)
export const saveAuthData = (authData: AuthResponseDto): void => {
  // Only store user info, not tokens (tokens are in httpOnly cookies)
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
// Note: We can't check cookies directly (httpOnly), so we check if user data exists
// In a real app, you might want to verify with the backend
export const isAuthenticated = (): boolean => {
  return !!getCurrentUser();
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
// Note: Cookies are cleared by the backend, we just clear local user data
export const clearAuthData = (): void => {
  localStorage.removeItem("user");
};
