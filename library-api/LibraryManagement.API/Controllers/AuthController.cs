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
        
        // Set httpOnly cookies for tokens
        SetTokenCookies(result);
        
        // Return user info without tokens
        return Ok(new AuthResponseDto
        {
            UserId = result.UserId,
            Email = result.Email,
            Role = result.Role,
            Expiration = result.Expiration
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var command = new LoginCommand { LoginDto = loginDto };
        var result = await _mediator.Send(command);
        
        // Set httpOnly cookies for tokens
        SetTokenCookies(result);
        
        // Return user info without tokens
        return Ok(new AuthResponseDto
        {
            UserId = result.UserId,
            Email = result.Email,
            Role = result.Role,
            Expiration = result.Expiration
        });
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken()
    {
        // Get refresh token from cookie instead of body
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized("Refresh token not found");
        }

        // Get access token from cookie
        var token = Request.Cookies["token"];
        
        var refreshTokenDto = new RefreshTokenDto
        {
            Token = token ?? string.Empty,
            RefreshToken = refreshToken
        };
        
        var command = new RefreshTokenCommand { RefreshTokenDto = refreshTokenDto };
        var result = await _mediator.Send(command);
        
        // Set new httpOnly cookies for tokens
        SetTokenCookies(result);
        
        // Return user info without tokens
        return Ok(new AuthResponseDto
        {
            UserId = result.UserId,
            Email = result.Email,
            Role = result.Role,
            Expiration = result.Expiration
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Clear cookies
        Response.Cookies.Delete("token");
        Response.Cookies.Delete("refreshToken");
        return Ok(new { message = "Logged out successfully" });
    }

    private void SetTokenCookies(AuthResponseDto authResponse)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // Prevents JavaScript access (XSS protection)
            Secure = false, // Set to true in production with HTTPS
            SameSite = SameSiteMode.Lax, // CSRF protection
            Expires = authResponse.Expiration
        };

        // Set access token cookie
        Response.Cookies.Append("token", authResponse.Token, cookieOptions);

        // Set refresh token cookie (longer expiration - 7 days)
        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // Set to true in production with HTTPS
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", authResponse.RefreshToken, refreshCookieOptions);
    }
}

