using BookingService.Models;
using BookingService.Repositories;

namespace BookingService.Services;

public class ComboDetailService : IComboDetailService
{
    private readonly IComboDetailRepository _comboDetailRepository;

    public ComboDetailService(IComboDetailRepository comboDetailRepository)
    {
        _comboDetailRepository = comboDetailRepository;
    }

    public async Task<IEnumerable<ComboDetail>> GetAllAsync()
    {
        return await _comboDetailRepository.GetAllAsync();
    }

    public async Task<ComboDetail?> GetByIdAsync(int id)
    {
        return await _comboDetailRepository.GetByIdAsync(id);
    }

    public async Task CreateComboDetailAsync(ComboDetail comboDetail)
    {
        await _comboDetailRepository.AddAsync(comboDetail);
    }

    public async Task UpdateAsync(ComboDetail comboDetail)
    {
        await _comboDetailRepository.UpdateAsync(comboDetail);
    }

    public async Task DeleteAsync(int id)
    {
        await _comboDetailRepository.DeleteAsync(id);
    }
}
