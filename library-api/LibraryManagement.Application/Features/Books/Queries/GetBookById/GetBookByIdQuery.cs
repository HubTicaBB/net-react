using MediatR;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Features.Books.Queries.GetBookById;

public class GetBookByIdQuery : IRequest<BookDto?>
{
    public int Id { get; set; }
}

