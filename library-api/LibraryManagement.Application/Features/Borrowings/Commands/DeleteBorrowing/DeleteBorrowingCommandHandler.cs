using MediatR;
using LibraryManagement.Application.Interfaces;

namespace LibraryManagement.Application.Features.Borrowings.Commands.DeleteBorrowing;

public class DeleteBorrowingCommandHandler : IRequestHandler<DeleteBorrowingCommand, bool>
{
    private readonly IBorrowingRepository _borrowingRepository;

    public DeleteBorrowingCommandHandler(IBorrowingRepository borrowingRepository)
    {
        _borrowingRepository = borrowingRepository;
    }

    public async Task<bool> Handle(DeleteBorrowingCommand request, CancellationToken cancellationToken)
    {
        return await _borrowingRepository.DeleteAsync(request.Id);
    }
}

