# Step 3: Setting up routing and authentication context

## What I'll do:

- Install React Router for navigation
- Create an AuthContext to manage authentication state globally
- Set up protected routes (require authentication)
- Create route structure for the app
- Create basic Login and Register pages

## Why this way:

- React Router: standard routing for React apps
- AuthContext: centralizes auth state so all components can access it
- Protected routes: automatically redirect unauthenticated users
- Separation: auth logic separate from components

## What I'll create:

- src/contexts/AuthContext.tsx - Authentication context provider
- src/components/ProtectedRoute.tsx - Component to protect routes
- src/pages/Login.tsx - Login page
- src/pages/Register.tsx - Register page
- src/pages/Home.tsx - Home/Dashboard page

## Update App.tsx with routing

- Structure:
- AuthContext provides: user, login, register, logout functions
- ProtectedRoute checks authentication before rendering
- Pages for login, register, and home
