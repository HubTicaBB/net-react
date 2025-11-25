using MediatR;
using AutoMapper;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Application.Features.Borrowings.Queries.GetBorrowingById;

public class GetBorrowingByIdQueryHandler : IRequestHandler<GetBorrowingByIdQuery, BorrowingDto?>
{
    private readonly IBorrowingRepository _borrowingRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public GetBorrowingByIdQueryHandler(IBorrowingRepository borrowingRepository, IMapper mapper, UserManager<IdentityUser> userManager)
    {
        _borrowingRepository = borrowingRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<BorrowingDto?> Handle(GetBorrowingByIdQuery request, CancellationToken cancellationToken)
    {
        var borrowing = await _borrowingRepository.GetByIdAsync(request.Id);
        if (borrowing == null) return null;

        var dto = _mapper.Map<BorrowingDto>(borrowing);
        var user = await _userManager.FindByIdAsync(borrowing.UserId);
        dto.UserName = user?.Email ?? "Unknown";
        dto.BookTitle = borrowing.Book?.Title ?? "Unknown";
        return dto;
    }
}

