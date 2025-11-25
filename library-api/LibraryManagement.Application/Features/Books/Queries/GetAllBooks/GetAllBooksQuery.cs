using MediatR;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Features.Books.Queries.GetAllBooks;

public class GetAllBooksQuery : IRequest<IEnumerable<BookDto>>
{
}

