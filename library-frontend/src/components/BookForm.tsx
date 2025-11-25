import { useState, type FormEvent, useEffect } from "react";
import type { BookDto, CreateBookDto, UpdateBookDto } from "../types/api";

interface BookFormProps {
  book?: BookDto;
  onSubmit: (data: CreateBookDto | UpdateBookDto) => Promise<void>;
  onCancel: () => void;
  loading?: boolean;
}

export const BookForm = ({
  book,
  onSubmit,
  onCancel,
  loading = false,
}: BookFormProps) => {
  const [formData, setFormData] = useState<CreateBookDto>({
    title: book?.title || "",
    author: book?.author || "",
    isbn: book?.isbn || "",
    publishedYear: book?.publishedYear || new Date().getFullYear(),
    availableCopies: book?.availableCopies || 0,
  });

  const [error, setError] = useState("");

  useEffect(() => {
    if (book) {
      setFormData({
        title: book.title,
        author: book.author,
        isbn: book.isbn,
        publishedYear: book.publishedYear,
        availableCopies: book.availableCopies,
      });
    }
  }, [book]);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]:
        name === "publishedYear" || name === "availableCopies"
          ? parseInt(value) || 0
          : value,
    }));
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError("");

    // Validation
    if (!formData.title.trim()) {
      setError("Title is required");
      return;
    }
    if (!formData.author.trim()) {
      setError("Author is required");
      return;
    }
    if (!formData.isbn.trim()) {
      setError("ISBN is required");
      return;
    }
    if (
      formData.publishedYear < 1000 ||
      formData.publishedYear > new Date().getFullYear() + 1
    ) {
      setError("Please enter a valid publication year");
      return;
    }
    if (formData.availableCopies < 0) {
      setError("Available copies cannot be negative");
      return;
    }

    try {
      await onSubmit(formData);
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Failed to save book. Please try again."
      );
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
          {error}
        </div>
      )}

      <div>
        <label
          htmlFor="title"
          className="block text-sm font-medium text-gray-700 mb-1"
        >
          Title *
        </label>
        <input
          type="text"
          id="title"
          name="title"
          required
          value={formData.title}
          onChange={handleChange}
          className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
        />
      </div>

      <div>
        <label
          htmlFor="author"
          className="block text-sm font-medium text-gray-700 mb-1"
        >
          Author *
        </label>
        <input
          type="text"
          id="author"
          name="author"
          required
          value={formData.author}
          onChange={handleChange}
          className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
        />
      </div>

      <div>
        <label
          htmlFor="isbn"
          className="block text-sm font-medium text-gray-700 mb-1"
        >
          ISBN *
        </label>
        <input
          type="text"
          id="isbn"
          name="isbn"
          required
          value={formData.isbn}
          onChange={handleChange}
          className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
        />
      </div>

      <div className="grid grid-cols-2 gap-4">
        <div>
          <label
            htmlFor="publishedYear"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Published Year *
          </label>
          <input
            type="number"
            id="publishedYear"
            name="publishedYear"
            required
            min="1000"
            max={new Date().getFullYear() + 1}
            value={formData.publishedYear}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
          />
        </div>

        <div>
          <label
            htmlFor="availableCopies"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Available Copies *
          </label>
          <input
            type="number"
            id="availableCopies"
            name="availableCopies"
            required
            min="0"
            value={formData.availableCopies}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
          />
        </div>
      </div>

      <div className="flex gap-3 pt-4">
        <button
          type="submit"
          disabled={loading}
          className="flex-1 bg-indigo-600 hover:bg-indigo-700 text-white px-4 py-2 rounded-md text-sm font-medium disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
        >
          {loading ? "Saving..." : book ? "Update Book" : "Create Book"}
        </button>
        <button
          type="button"
          onClick={onCancel}
          disabled={loading}
          className="flex-1 bg-gray-300 hover:bg-gray-400 text-gray-800 px-4 py-2 rounded-md text-sm font-medium disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
        >
          Cancel
        </button>
      </div>
    </form>
  );
};
