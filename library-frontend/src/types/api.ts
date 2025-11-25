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
