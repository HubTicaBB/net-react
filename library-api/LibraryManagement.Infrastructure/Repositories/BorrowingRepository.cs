using Microsoft.EntityFrameworkCore;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories;

public class BorrowingRepository : IBorrowingRepository
{
    private readonly ApplicationDbContext _context;

    public BorrowingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Borrowing>> GetAllAsync()
    {
        return await _context.Borrowings
            .Include(b => b.Book)
            .ToListAsync();
    }

    public async Task<IEnumerable<Borrowing>> GetByUserIdAsync(string userId)
    {
        return await _context.Borrowings
            .Include(b => b.Book)
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    public async Task<Borrowing?> GetByIdAsync(int id)
    {
        return await _context.Borrowings
            .Include(b => b.Book)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Borrowing> CreateAsync(Borrowing borrowing)
    {
        _context.Borrowings.Add(borrowing);
        await _context.SaveChangesAsync();
        return borrowing;
    }

    public async Task<Borrowing> UpdateAsync(Borrowing borrowing)
    {
        _context.Borrowings.Update(borrowing);
        await _context.SaveChangesAsync();
        return borrowing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var borrowing = await _context.Borrowings.FindAsync(id);
        if (borrowing == null) return false;

        _context.Borrowings.Remove(borrowing);
        await _context.SaveChangesAsync();
        return true;
    }
}

