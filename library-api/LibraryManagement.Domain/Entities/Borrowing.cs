namespace LibraryManagement.Domain.Entities;

public class Borrowing
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }
    public BorrowingStatus Status { get; set; } = BorrowingStatus.Borrowed;

    // Navigation properties
    public Book Book { get; set; } = null!;
}

public enum BorrowingStatus
{
    Borrowed = 1,
    Returned = 2,
    Overdue = 3
}

