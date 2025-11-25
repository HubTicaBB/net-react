using MediatR;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<IdentityUser> _userManager;

    public RefreshTokenCommandHandler(IJwtTokenService jwtTokenService, UserManager<IdentityUser> userManager)
    {
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
    }

    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwtTokenService.GetPrincipalFromExpiredToken(request.RefreshTokenDto.Token);
        var userId = principal.Identity?.Name;
        
        if (userId == null)
        {
            throw new UnauthorizedAccessException("Invalid token.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        // Validate refresh token (in a real app, you'd check this against a database)
        var tokenResponse = await _jwtTokenService.GenerateTokenAsync(user);
        return tokenResponse;
    }
}

