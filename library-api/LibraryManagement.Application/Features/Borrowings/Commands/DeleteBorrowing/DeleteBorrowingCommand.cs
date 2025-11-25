using MediatR;

namespace LibraryManagement.Application.Features.Borrowings.Commands.DeleteBorrowing;

public class DeleteBorrowingCommand : IRequest<bool>
{
    public int Id { get; set; }
}

