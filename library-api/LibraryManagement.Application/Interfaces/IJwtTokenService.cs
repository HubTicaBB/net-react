using LibraryManagement.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LibraryManagement.Application.Interfaces;

public interface IJwtTokenService
{
    Task<AuthResponseDto> GenerateTokenAsync(IdentityUser user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}

