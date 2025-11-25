using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces;

public interface IBorrowingRepository
{
    Task<IEnumerable<Borrowing>> GetAllAsync();
    Task<IEnumerable<Borrowing>> GetByUserIdAsync(string userId);
    Task<Borrowing?> GetByIdAsync(int id);
    Task<Borrowing> CreateAsync(Borrowing borrowing);
    Task<Borrowing> UpdateAsync(Borrowing borrowing);
    Task<bool> DeleteAsync(int id);
}

