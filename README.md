# React

## Step 1: Create React app with TypeScript and Tailwind CSS

### What I'll do:

1. Create a new React app with TypeScript using Vite (faster than Create React App)
```
npm create vite@latest library-frontend -- --template react-ts
cd library-frontend
```
2. Install and configure Tailwind CSS
```
npm install -D tailwindcss postcss autoprefixer
npm install @tailwindcss/postcss 
npx tailwindcss init -p
```
```js
// tailwind.config.js

/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
```
```js
// postcss.config.js

export default {
  plugins: {
    "@tailwindcss/postcss": {},
    autoprefixer: {},
  },
};
```
```css
// src/index.css

// replace existing css with
@tailwind base;
@tailwind components;
@tailwind utilities;
```

3. Set up a basic project structure
```
mkdir -p components pages services types hooks utils
```

### Why this way:
1. Vite: faster dev server and builds, better developer experience
2. TypeScript: catches errors early, improves code quality
3. Tailwind CSS: utility-first CSS for rapid UI development


## Step 2: Setting up the API client (Axios configuration)

### What I'll do:

1. Install Axios
```
npm install axios
```
2. Create an API client with base configuration
    1. Crate TS types:
```ts
// src/types/api.ts

// Auth Types
export interface RegisterDto {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface AuthResponseDto {
  token: string;
  refreshToken: string;
  expiration: string; // ISO date string
  userId: string;
  email: string;
  role: string;
}

export interface RefreshTokenDto {
  token: string;
  refreshToken: string;
}

// Book Types
export interface BookDto {
  id: number;
  title: string;
  author: string;
  isbn: string;
  publishedYear: number;
  availableCopies: number;
  createdAt: string; // ISO date string
}

export interface CreateBookDto {
  title: string;
  author: string;
  isbn: string;
  publishedYear: number;
  availableCopies: number;
}

export interface UpdateBookDto {
  title: string;
  author: string;
  isbn: string;
  publishedYear: number;
  availableCopies: number;
}

// Borrowing Types
export const BorrowingStatus = {
  Borrowed: 1,
  Returned: 2,
  Overdue: 3,
} as const;

export type BorrowingStatus =
  (typeof BorrowingStatus)[keyof typeof BorrowingStatus];

export interface BorrowingDto {
  id: number;
  bookId: number;
  bookTitle: string;
  userId: string;
  userName: string;
  borrowDate: string; // ISO date string
  returnDate: string | null; // ISO date string or null
  dueDate: string; // ISO date string
  status: string;
}

export interface CreateBorrowingDto {
  bookId: number;
  dueDate: string; // ISO date string
}

export interface UpdateBorrowingDto {
  returnDate: string | null; // ISO date string or null
  status: BorrowingStatus;
}

// API Error Response
export interface ApiError {
  message: string;
  errors?: Record<string, string[]>;
}
```
   3. Create API client
```ts
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
```
  3. Create Services
```ts
// src/services/authService.ts

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
```
```ts
src/services/bookService.ts
import api from "./api";
import type { BookDto, CreateBookDto, UpdateBookDto } from "../types/api";

export const bookService = {
  // Get all books
  getAll: async (): Promise<BookDto[]> => {
    const response = await api.get<BookDto[]>("/api/books");
    return response.data;
  },

  // Get book by ID
  getById: async (id: number): Promise<BookDto> => {
    const response = await api.get<BookDto>(`/api/books/${id}`);
    return response.data;
  },

  // Create a new book (Librarian only)
  create: async (data: CreateBookDto): Promise<BookDto> => {
    const response = await api.post<BookDto>("/api/books", data);
    return response.data;
  },

  // Update a book (Librarian only)
  update: async (id: number, data: UpdateBookDto): Promise<BookDto> => {
    const response = await api.put<BookDto>(`/api/books/${id}`, data);
    return response.data;
  },

  // Delete a book (Librarian only)
  delete: async (id: number): Promise<void> => {
    await api.delete(`/api/books/${id}`);
  },
};

```

- Set up interceptors for:
  - Adding authentication tokens to requests
  - Handling token refresh
  - Error handling
- Create TypeScript types for API responses
- Set up environment variables for the API base URL

### Why Axios over Fetch:
- Interceptors: automatically add tokens to all requests
- Better error handling: automatic JSON parsing, status code handling
- Request/response transformation: easier to transform data
- Timeout support: built-in timeout configuration
- More developer-friendly: cleaner API than fetch

### What I'll create:
- src/services/api.ts - Axios instance with interceptors
- src/types/api.ts - TypeScript types for API responses
- .env file - Environment variables for API URL
- Update vite.config.ts if needed for environment variables

#### Code structure:
- Base URL configuration (pointing to your .NET API)
- Request interceptor: adds JWT token from localStorage
- Response interceptor: handles errors and token refresh
- Type-safe API response types


