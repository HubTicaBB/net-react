using MediatR;
using AutoMapper;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Application.Features.Borrowings.Commands.UpdateBorrowing;

public class UpdateBorrowingCommandHandler : IRequestHandler<UpdateBorrowingCommand, BorrowingDto>
{
    private readonly IBorrowingRepository _borrowingRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public UpdateBorrowingCommandHandler(
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

    public async Task<BorrowingDto> Handle(UpdateBorrowingCommand request, CancellationToken cancellationToken)
    {
        var existingBorrowing = await _borrowingRepository.GetByIdAsync(request.Id);
        if (existingBorrowing == null)
        {
            throw new KeyNotFoundException($"Borrowing with ID {request.Id} not found.");
        }

        var wasReturned = existingBorrowing.Status == Domain.Entities.BorrowingStatus.Borrowed &&
                         request.Borrowing.Status == BorrowingStatus.Returned;

        existingBorrowing.ReturnDate = request.Borrowing.ReturnDate ?? existingBorrowing.ReturnDate;
        existingBorrowing.Status = (Domain.Entities.BorrowingStatus)request.Borrowing.Status;

        if (wasReturned)
        {
            var book = await _bookRepository.GetByIdAsync(existingBorrowing.BookId);
            if (book != null)
            {
                book.AvailableCopies++;
                await _bookRepository.UpdateAsync(book);
            }
        }

        var updatedBorrowing = await _borrowingRepository.UpdateAsync(existingBorrowing);
        var dto = _mapper.Map<BorrowingDto>(updatedBorrowing);
        var user = await _userManager.FindByIdAsync(updatedBorrowing.UserId);
        dto.UserName = user?.Email ?? "Unknown";
        dto.BookTitle = updatedBorrowing.Book?.Title ?? "Unknown";
        return dto;
    }
}

