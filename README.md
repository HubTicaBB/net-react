## Steg 1: Skapa react app med Typescript och Tailwind CSS

### 1. Skapa React app med TypeScript
https://create-react-app.dev/docs/adding-typescript/ 

```
npx create-react-app YOUR_APP_NAME --template typescript
// eller
yarn create react-app YOUR_APP_NAME --template typescript
// eller
// (faster dev server and builds, better developer experience)
npm create vite@latest YOUR_APP_NAME -- --template react-ts
```

### 2. Installera Tailwind (CSS, för snabb UI)
https://tailwindcss.com/docs/styling-with-utility-classes 

```
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p
npm install @tailwindcss/postcss 
```

### 3. Konfigurera Tailwind

Skapa fil postcss.config.js on the root:
```
export default {
  plugins: {
    "@tailwindcss/postcss": {},
    autoprefixer: {},
  },
};
```

Skapa fil tailwind.config.js on the root:
```
 /** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {},
  },
  plugins: [],
};
```

Updatera src/index.css, ta bort allt, ersätt med: 
```
@import "tailwindcss";
```

## Steg 2: Sätta upp API-klienten (Axios configuration)

### 1. Installera Axios
```
npm install axios
```

### 2. Create TypeScript types matching your backend DTOs:
```
export interface RegisterDto {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}
```

### 3. Create Axios instance with interceptors
```
const api: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    // Get token from localStorage
    const token = localStorage.getItem("token");

   if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);
```

### 4. Create Service files (by resource)
 
```
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

### 5. Create Auth utility

### 6. Create .env file
```
VITE_API_BASE_URL=http://localhost:5146
```

Vite automatically loads .env files and exposes variables prefixed with VITE_ to the client.


