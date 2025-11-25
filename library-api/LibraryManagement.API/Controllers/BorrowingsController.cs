using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Features.Borrowings.Commands.CreateBorrowing;
using LibraryManagement.Application.Features.Borrowings.Commands.UpdateBorrowing;
using LibraryManagement.Application.Features.Borrowings.Commands.DeleteBorrowing;
using LibraryManagement.Application.Features.Borrowings.Queries.GetAllBorrowings;
using LibraryManagement.Application.Features.Borrowings.Queries.GetBorrowingById;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BorrowingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BorrowingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Librarian")]
    public async Task<ActionResult<IEnumerable<BorrowingDto>>> GetAll()
    {
        var query = new GetAllBorrowingsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BorrowingDto>> GetById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = new GetBorrowingByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        
        if (result == null) return NotFound();
        
        // Members can only view their own borrowings
        if (!User.IsInRole("Librarian") && result.UserId != userId)
        {
            return Forbid();
        }
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BorrowingDto>> Create([FromBody] CreateBorrowingDto createBorrowingDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var command = new CreateBorrowingCommand 
        { 
            Borrowing = createBorrowingDto,
            UserId = userId
        };
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BorrowingDto>> Update(int id, [FromBody] UpdateBorrowingDto updateBorrowingDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var getQuery = new GetBorrowingByIdQuery { Id = id };
        var existing = await _mediator.Send(getQuery);
        
        if (existing == null) return NotFound();
        
        // Members can only update their own borrowings
        if (!User.IsInRole("Librarian") && existing.UserId != userId)
        {
            return Forbid();
        }

        var command = new UpdateBorrowingCommand { Id = id, Borrowing = updateBorrowingDto };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var getQuery = new GetBorrowingByIdQuery { Id = id };
        var existing = await _mediator.Send(getQuery);
        
        if (existing == null) return NotFound();
        
        // Members can only delete their own borrowings
        if (!User.IsInRole("Librarian") && existing.UserId != userId)
        {
            return Forbid();
        }

        var command = new DeleteBorrowingCommand { Id = id };
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return NoContent();
    }
}

