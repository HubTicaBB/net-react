namespace LibraryManagement.Application.DTOs;

public class BorrowingDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateBorrowingDto
{
    public int BookId { get; set; }
    public DateTime DueDate { get; set; }
}

public class UpdateBorrowingDto
{
    public DateTime? ReturnDate { get; set; }
    public BorrowingStatus Status { get; set; }
}

public enum BorrowingStatus
{
    Borrowed = 1,
    Returned = 2,
    Overdue = 3
}

