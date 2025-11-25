using MediatR;
using AutoMapper;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Application.Features.Borrowings.Queries.GetAllBorrowings;

public class GetAllBorrowingsQueryHandler : IRequestHandler<GetAllBorrowingsQuery, IEnumerable<BorrowingDto>>
{
    private readonly IBorrowingRepository _borrowingRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public GetAllBorrowingsQueryHandler(IBorrowingRepository borrowingRepository, IMapper mapper, UserManager<IdentityUser> userManager)
    {
        _borrowingRepository = borrowingRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<IEnumerable<BorrowingDto>> Handle(GetAllBorrowingsQuery request, CancellationToken cancellationToken)
    {
        var borrowings = await _borrowingRepository.GetAllAsync();
        var borrowingDtos = new List<BorrowingDto>();

        foreach (var borrowing in borrowings)
        {
            var dto = _mapper.Map<BorrowingDto>(borrowing);
            var user = await _userManager.FindByIdAsync(borrowing.UserId);
            dto.UserName = user?.Email ?? "Unknown";
            dto.BookTitle = borrowing.Book?.Title ?? "Unknown";
            borrowingDtos.Add(dto);
        }

        return borrowingDtos;
    }
}

