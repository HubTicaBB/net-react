using MediatR;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Features.Borrowings.Commands.UpdateBorrowing;

public class UpdateBorrowingCommand : IRequest<BorrowingDto>
{
    public int Id { get; set; }
    public UpdateBorrowingDto Borrowing { get; set; } = null!;
}

