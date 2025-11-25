import { useAuth } from "../contexts/AuthContext";

export const Home = () => {
  const { user, logout } = useAuth();

  const handleLogout = async () => {
    await logout();
  };

  return (
    <div className="min-h-screen bg-gray-100">
      <div className="bg-white shadow">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center py-6">
            <h1 className="text-3xl font-bold text-gray-900">
              Library Management System
            </h1>
            <div className="flex items-center gap-4">
              <span className="text-gray-700">
                Welcome, {user?.email} ({user?.role})
              </span>
              <button
                onClick={handleLogout}
                className="bg-red-600 hover:bg-red-700 text-white px-4 py-2 rounded-md text-sm font-medium"
              >
                Logout
              </button>
            </div>
          </div>
        </div>
      </div>

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="bg-white rounded-lg shadow p-6">
          <h2 className="text-2xl font-semibold text-gray-800 mb-4">
            Dashboard
          </h2>
          <p className="text-gray-600">
            Welcome to the Library Management System. This is your home page.
          </p>
          <div className="mt-4">
            <p className="text-sm text-gray-500">
              Your role: <span className="font-semibold">{user?.role}</span>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};
