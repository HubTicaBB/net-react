import type { BookDto } from "../types/api";
import { useAuth } from "../contexts/AuthContext";

interface BookCardProps {
  book: BookDto;
  onEdit: (book: BookDto) => void;
  onDelete: (id: number) => void;
}

export const BookCard = ({ book, onEdit, onDelete }: BookCardProps) => {
  const { isLibrarian } = useAuth();

  return (
    <div className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
      <div className="flex justify-between items-start mb-4">
        <div className="flex-1">
          <h3 className="text-xl font-semibold text-gray-900 mb-2">
            {book.title}
          </h3>
          <p className="text-gray-600 mb-1">
            <span className="font-medium">Author:</span> {book.author}
          </p>
          <p className="text-gray-600 mb-1">
            <span className="font-medium">ISBN:</span> {book.isbn}
          </p>
          <p className="text-gray-600 mb-1">
            <span className="font-medium">Published:</span> {book.publishedYear}
          </p>
          <p className="text-gray-600">
            <span className="font-medium">Available Copies:</span>{" "}
            <span
              className={
                book.availableCopies > 0
                  ? "text-green-600 font-semibold"
                  : "text-red-600 font-semibold"
              }
            >
              {book.availableCopies}
            </span>
          </p>
        </div>
      </div>

      {isLibrarian() && (
        <div className="flex gap-2 mt-4 pt-4 border-t border-gray-200">
          <button
            onClick={() => onEdit(book)}
            className="flex-1 bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-md text-sm font-medium transition-colors"
          >
            Edit
          </button>
          <button
            onClick={() => onDelete(book.id)}
            className="flex-1 bg-red-600 hover:bg-red-700 text-white px-4 py-2 rounded-md text-sm font-medium transition-colors"
          >
            Delete
          </button>
        </div>
      )}
    </div>
  );
};
