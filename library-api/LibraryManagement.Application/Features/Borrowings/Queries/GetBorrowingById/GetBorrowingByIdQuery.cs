using MediatR;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Features.Borrowings.Queries.GetBorrowingById;

public class GetBorrowingByIdQuery : IRequest<BorrowingDto?>
{
    public int Id { get; set; }
}

