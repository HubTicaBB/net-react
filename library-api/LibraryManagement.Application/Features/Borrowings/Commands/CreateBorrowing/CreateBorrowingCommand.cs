using MediatR;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Features.Borrowings.Commands.CreateBorrowing;

public class CreateBorrowingCommand : IRequest<BorrowingDto>
{
    public CreateBorrowingDto Borrowing { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
}

