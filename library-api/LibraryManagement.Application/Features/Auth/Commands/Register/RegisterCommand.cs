using MediatR;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public RegisterDto RegisterDto { get; set; } = null!;
}

