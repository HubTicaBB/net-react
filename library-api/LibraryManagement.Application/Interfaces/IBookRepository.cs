using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task<Book> UpdateAsync(Book book);
    Task<bool> DeleteAsync(int id);
}

