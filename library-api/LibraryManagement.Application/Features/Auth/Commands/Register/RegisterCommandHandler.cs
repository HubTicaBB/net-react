using MediatR;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCommandHandler(UserManager<IdentityUser> userManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new IdentityUser
        {
            UserName = request.RegisterDto.Email,
            Email = request.RegisterDto.Email
        };

        var result = await _userManager.CreateAsync(user, request.RegisterDto.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // Assign default role as Member
        await _userManager.AddToRoleAsync(user, "Member");

        var tokenResponse = await _jwtTokenService.GenerateTokenAsync(user);
        return tokenResponse;
    }
}

