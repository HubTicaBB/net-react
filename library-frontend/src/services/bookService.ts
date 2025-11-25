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
