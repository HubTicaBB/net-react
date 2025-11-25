using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Features.Books.Commands.CreateBook;
using LibraryManagement.Application.Features.Books.Commands.UpdateBook;
using LibraryManagement.Application.Features.Books.Commands.DeleteBook;
using LibraryManagement.Application.Features.Books.Queries.GetAllBooks;
using LibraryManagement.Application.Features.Books.Queries.GetBookById;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
    {
        var query = new GetAllBooksQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        var query = new GetBookByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Librarian")]
    public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto createBookDto)
    {
        var command = new CreateBookCommand { Book = createBookDto };
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Librarian")]
    public async Task<ActionResult<BookDto>> Update(int id, [FromBody] UpdateBookDto updateBookDto)
    {
        var command = new UpdateBookCommand { Id = id, Book = updateBookDto };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteBookCommand { Id = id };
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return NoContent();
    }
}

