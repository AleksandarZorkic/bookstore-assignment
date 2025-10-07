using BookstoreApplication.Models;
using BookstoreApplication.Repositories.Interfaces;
using BookstoreApplication.Services.Interfaces;

namespace BookstoreApplication.Services.Implementations
{
    public class AwardService : IAwardService
    {
        private readonly IAwardRepository _awards;
        public AwardService(IAwardRepository awards) => _awards = awards;

        public Task<List<Award>> GetAllAsync(CancellationToken ct = default)
            => _awards.GetAllAsync();

        public Task<Award?> GetByIdAsync(int id, CancellationToken ct = default)
            => _awards.GetByIdAsync(id);

        public async Task<Award> CreateAsync(Award dto, CancellationToken ct = default)
        {
            var created = await _awards.AddAsync(dto);
            await _awards.SaveChangesAsync();
            return created;
        }

        public async Task UpdateAsync(int id, Award dto, CancellationToken ct = default)
        {
            if (id != dto.Id) throw new ArgumentException("ID mismatch.");
            if (!await _awards.ExistsAsync(id)) throw new KeyNotFoundException();

            await _awards.UpdateAsync(dto);
            await _awards.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _awards.DeleteAsync(id);
            await _awards.SaveChangesAsync();
        }
    }
}
