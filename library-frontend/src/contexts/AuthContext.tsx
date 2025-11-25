import {
  createContext,
  useContext,
  useState,
  useEffect,
  type ReactNode,
} from "react";
import type { LoginDto, RegisterDto } from "../types/api";
import { authService } from "../services/authService";
import {
  saveAuthData,
  getCurrentUser,
  clearAuthData,
  isAuthenticated,
} from "../utils/auth";

interface AuthContextType {
  user: { userId: string; email: string; role: string } | null;
  loading: boolean;
  login: (credentials: LoginDto) => Promise<void>;
  register: (data: RegisterDto) => Promise<void>;
  logout: () => Promise<void>;
  isAuthenticated: () => boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [user, setUser] = useState<{
    userId: string;
    email: string;
    role: string;
  } | null>(null);
  const [loading, setLoading] = useState(true);

  // Load user from localStorage on mount
  useEffect(() => {
    const loadUser = () => {
      const currentUser = getCurrentUser();
      setUser(currentUser);
      setLoading(false);
    };

    loadUser();
  }, []);

  const login = async (credentials: LoginDto) => {
    try {
      const response = await authService.login(credentials);
      // Save user data (tokens are in httpOnly cookies)
      saveAuthData(response);
      setUser({
        userId: response.userId,
        email: response.email,
        role: response.role,
      });
    } catch (error) {
      throw error;
    }
  };

  const register = async (data: RegisterDto) => {
    try {
      const response = await authService.register(data);
      // Save user data (tokens are in httpOnly cookies)
      saveAuthData(response);
      setUser({
        userId: response.userId,
        email: response.email,
        role: response.role,
      });
    } catch (error) {
      throw error;
    }
  };

  const logout = async () => {
    try {
      await authService.logout();
      clearAuthData();
      setUser(null);
    } catch (error) {
      // Even if logout fails, clear local state
      clearAuthData();
      setUser(null);
    }
  };

  const value: AuthContextType = {
    user,
    loading,
    login,
    register,
    logout,
    isAuthenticated,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
