using MediatR;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<AuthResponseDto>
{
    public LoginDto LoginDto { get; set; } = null!;
}

