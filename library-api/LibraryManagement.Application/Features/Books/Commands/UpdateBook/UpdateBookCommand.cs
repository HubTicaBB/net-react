using MediatR;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Features.Books.Commands.UpdateBook;

public class UpdateBookCommand : IRequest<BookDto>
{
    public int Id { get; set; }
    public UpdateBookDto Book { get; set; } = null!;
}

