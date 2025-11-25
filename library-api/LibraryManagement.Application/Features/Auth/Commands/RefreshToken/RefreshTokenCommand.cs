using MediatR;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<AuthResponseDto>
{
    public RefreshTokenDto RefreshTokenDto { get; set; } = null!;
}

