using MediatR;
using AutoMapper;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Application.Features.Borrowings.Commands.CreateBorrowing;

public class CreateBorrowingCommandHandler : IRequestHandler<CreateBorrowingCommand, BorrowingDto>
{
    private readonly IBorrowingRepository _borrowingRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public CreateBorrowingCommandHandler(
        IBorrowingRepository borrowingRepository,
        IBookRepository bookRepository,
        IMapper mapper,
        UserManager<IdentityUser> userManager)
    {
        _borrowingRepository = borrowingRepository;
        _bookRepository = bookRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<BorrowingDto> Handle(CreateBorrowingCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Borrowing.BookId);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with ID {request.Borrowing.BookId} not found.");
        }

        if (book.AvailableCopies <= 0)
        {
            throw new InvalidOperationException("No available copies of this book.");
        }

        var borrowing = new Borrowing
        {
            BookId = request.Borrowing.BookId,
            UserId = request.UserId,
            DueDate = request.Borrowing.DueDate,
            Status = Domain.Entities.BorrowingStatus.Borrowed
        };

        book.AvailableCopies--;
        await _bookRepository.UpdateAsync(book);

        var createdBorrowing = await _borrowingRepository.CreateAsync(borrowing);
        var dto = _mapper.Map<BorrowingDto>(createdBorrowing);
        var user = await _userManager.FindByIdAsync(request.UserId);
        dto.UserName = user?.Email ?? "Unknown";
        dto.BookTitle = book.Title;
        return dto;
    }
}

