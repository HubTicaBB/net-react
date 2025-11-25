using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Features.Auth.Commands.Login;
using LibraryManagement.Application.Features.Auth.Commands.Register;
using LibraryManagement.Application.Features.Auth.Commands.RefreshToken;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
    {
        var command = new RegisterCommand { RegisterDto = registerDto };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var command = new LoginCommand { LoginDto = loginDto };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        var command = new RefreshTokenCommand { RefreshTokenDto = refreshTokenDto };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

