using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories;

public class ComboDetailRepository : IComboDetailRepository
{
    private readonly BookingDbContext _context;

    public ComboDetailRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ComboDetail>> GetAllAsync()
    {
        return await _context.ComboDetails
            .Include(cd => cd.Combo)
            .Include(cd => cd.Food)
            .Include(cd => cd.Drink)
            .ToListAsync();
    }

    public async Task<ComboDetail?> GetByIdAsync(int id)
    {
        return await _context.ComboDetails
            .Include(cd => cd.Combo)
            .Include(cd => cd.Food)
            .Include(cd => cd.Drink)
            .FirstOrDefaultAsync(cd => cd.Id == id);
    }

    public async Task AddAsync(ComboDetail comboDetail)
    {
        await _context.ComboDetails.AddAsync(comboDetail);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ComboDetail comboDetail)
    {
        var existing = await _context.ComboDetails.FindAsync(comboDetail.Id);
        if (existing == null)
        {
            throw new Exception("Không tìm thấy chi tiết combo để cập nhật.");
        }

        existing.ComboId = comboDetail.ComboId;
        existing.DrinkId = comboDetail.DrinkId;
        existing.FoodId = comboDetail.FoodId;
        existing.Price = comboDetail.Price;

        _context.ComboDetails.Update(existing);
        await _context.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var comboDetail = await _context.ComboDetails.FindAsync(id);
        if (comboDetail != null)
        {
            _context.ComboDetails.Remove(comboDetail);
            await _context.SaveChangesAsync();
        }
    }
}
