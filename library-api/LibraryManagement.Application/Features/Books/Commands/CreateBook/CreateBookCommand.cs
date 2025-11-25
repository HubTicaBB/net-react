using MediatR;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Features.Books.Commands.CreateBook;

public class CreateBookCommand : IRequest<BookDto>
{
    public CreateBookDto Book { get; set; } = null!;
}

