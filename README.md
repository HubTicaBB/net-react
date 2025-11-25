# React

## Step 1: Create React app with TypeScript and Tailwind CSS

### What I'll do:
1. Create a new React app with TypeScript using Vite (faster than Create React App)
```
npm create vite@latest library-frontend -- --template react-ts
cd library-frontend
```
3. Install and configure Tailwind CSS
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
    tailwindcss: {},
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
5. Set up a basic project structure
```
mkdir -p components pages services types hooks utils
```

### Why this way:
1. Vite: faster dev server and builds, better developer experience
2. TypeScript: catches errors early, improves code quality
3. Tailwind CSS: utility-first CSS for rapid UI development

### What I'll create:
1. Initialize a Vite + React + TypeScript project in library-frontend
2. Install Tailwind CSS and its dependencies
3, Configure Tailwind in `tailwind.config.js`
4. Add Tailwind directives to CSS
5. Set up a basic folder structure (components, pages, services, etc.)


## Step 2: Setting up the API client (Axios configuration)

### What I'll do:
- Install Axios
- Create an API client with base configuration
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


