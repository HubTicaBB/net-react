using MediatR;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Features.Borrowings.Queries.GetAllBorrowings;

public class GetAllBorrowingsQuery : IRequest<IEnumerable<BorrowingDto>>
{
}

