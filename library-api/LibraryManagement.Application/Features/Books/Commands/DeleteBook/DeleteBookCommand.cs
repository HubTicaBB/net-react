using MediatR;

namespace LibraryManagement.Application.Features.Books.Commands.DeleteBook;

public class DeleteBookCommand : IRequest<bool>
{
    public int Id { get; set; }
}

