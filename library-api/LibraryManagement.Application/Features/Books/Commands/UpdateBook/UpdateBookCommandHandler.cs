using MediatR;
using AutoMapper;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;

namespace LibraryManagement.Application.Features.Books.Commands.UpdateBook;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDto>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public UpdateBookCommandHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<BookDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var existingBook = await _bookRepository.GetByIdAsync(request.Id);
        if (existingBook == null)
        {
            throw new KeyNotFoundException($"Book with ID {request.Id} not found.");
        }

        _mapper.Map(request.Book, existingBook);
        existingBook.UpdatedAt = DateTime.UtcNow;
        var updatedBook = await _bookRepository.UpdateAsync(existingBook);
        return _mapper.Map<BookDto>(updatedBook);
    }
}

