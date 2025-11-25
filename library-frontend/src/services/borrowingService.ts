import api from "./api";
import type {
  BorrowingDto,
  CreateBorrowingDto,
  UpdateBorrowingDto,
} from "../types/api";

export const borrowingService = {
  // Get all borrowings (Librarian only)
  getAll: async (): Promise<BorrowingDto[]> => {
    const response = await api.get<BorrowingDto[]>("/api/borrowings");
    return response.data;
  },

  // Get borrowing by ID
  getById: async (id: number): Promise<BorrowingDto> => {
    const response = await api.get<BorrowingDto>(`/api/borrowings/${id}`);
    return response.data;
  },

  // Create a new borrowing
  create: async (data: CreateBorrowingDto): Promise<BorrowingDto> => {
    const response = await api.post<BorrowingDto>("/api/borrowings", data);
    return response.data;
  },

  // Update a borrowing
  update: async (
    id: number,
    data: UpdateBorrowingDto
  ): Promise<BorrowingDto> => {
    const response = await api.put<BorrowingDto>(`/api/borrowings/${id}`, data);
    return response.data;
  },

  // Delete a borrowing
  delete: async (id: number): Promise<void> => {
    await api.delete(`/api/borrowings/${id}`);
  },
};
