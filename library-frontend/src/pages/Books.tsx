import { useState, useEffect } from "react";
import { useAuth } from "../contexts/AuthContext";
import { bookService } from "../services/bookService";
import type { BookDto, CreateBookDto, UpdateBookDto } from "../types/api";
import { BookCard } from "../components/BookCard";
import { BookForm } from "../components/BookForm";

export const Books = () => {
  const { user, logout, isLibrarian } = useAuth();
  const [books, setBooks] = useState<BookDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [showForm, setShowForm] = useState(false);
  const [editingBook, setEditingBook] = useState<BookDto | null>(null);
  const [formLoading, setFormLoading] = useState(false);

  useEffect(() => {
    loadBooks();
  }, []);

  const loadBooks = async () => {
    try {
      setLoading(true);
      setError("");
      const data = await bookService.getAll();
      setBooks(data);
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Failed to load books. Please try again."
      );
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setEditingBook(null);
    setShowForm(true);
  };

  const handleEdit = (book: BookDto) => {
    setEditingBook(book);
    setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this book?")) {
      return;
    }

    try {
      await bookService.delete(id);
      await loadBooks(); // Reload books after deletion
    } catch (err) {
      alert(
        err instanceof Error
          ? err.message
          : "Failed to delete book. Please try again."
      );
    }
  };

  const handleFormSubmit = async (data: CreateBookDto | UpdateBookDto) => {
    try {
      setFormLoading(true);
      if (editingBook) {
        // Update existing book
        await bookService.update(editingBook.id, data as UpdateBookDto);
      } else {
        // Create new book
        await bookService.create(data as CreateBookDto);
      }
      setShowForm(false);
      setEditingBook(null);
      await loadBooks(); // Reload books after create/update
    } catch (err) {
      throw err; // Let BookForm handle the error display
    } finally {
      setFormLoading(false);
    }
  };

  const handleFormCancel = () => {
    setShowForm(false);
    setEditingBook(null);
  };

  const handleLogout = async () => {
    await logout();
  };

  return (
    <div className="min-h-screen bg-gray-100">
      {/* Header */}
      <div className="bg-white shadow">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center py-6">
            <h1 className="text-3xl font-bold text-gray-900">Books</h1>
            <div className="flex items-center gap-4">
              <span className="text-gray-700">
                {user?.email} ({user?.role})
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

      {/* Main Content */}
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Create Button (Librarian only) */}
        {isLibrarian() && !showForm && (
          <div className="mb-6">
            <button
              onClick={handleCreate}
              className="bg-indigo-600 hover:bg-indigo-700 text-white px-6 py-2 rounded-md text-sm font-medium transition-colors"
            >
              + Add New Book
            </button>
          </div>
        )}

        {/* Book Form (Create/Edit) */}
        {showForm && (
          <div className="bg-white rounded-lg shadow-md p-6 mb-6">
            <h2 className="text-2xl font-semibold text-gray-800 mb-4">
              {editingBook ? "Edit Book" : "Create New Book"}
            </h2>
            <BookForm
              book={editingBook || undefined}
              onSubmit={handleFormSubmit}
              onCancel={handleFormCancel}
              loading={formLoading}
            />
          </div>
        )}

        {/* Error Message */}
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-6">
            {error}
          </div>
        )}

        {/* Loading State */}
        {loading ? (
          <div className="flex items-center justify-center py-12">
            <div className="text-lg text-gray-600">Loading books...</div>
          </div>
        ) : (
          <>
            {/* Books Grid */}
            {books.length === 0 ? (
              <div className="bg-white rounded-lg shadow-md p-12 text-center">
                <p className="text-gray-600 text-lg">No books found.</p>
                {isLibrarian() && (
                  <p className="text-gray-500 mt-2">
                    Click "Add New Book" to get started.
                  </p>
                )}
              </div>
            ) : (
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {books.map((book) => (
                  <BookCard
                    key={book.id}
                    book={book}
                    onEdit={handleEdit}
                    onDelete={handleDelete}
                  />
                ))}
              </div>
            )}
          </>
        )}
      </div>
    </div>
  );
};
