using BookstoreApplication.Models.Entities;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IAwardService
    {
        Task<List<Award>> GetAllAsync(CancellationToken ct = default);
        Task<Award?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Award> CreateAsync(Award dto, CancellationToken ct = default);
        Task UpdateAsync(int id, Award dto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
