using System.Threading;
using System.Threading.Tasks;
using BookstoreApplication.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookstoreApplication.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        private IDbContextTransaction? _tx;

        public UnitOfWork(AppDbContext db) => _db = db;

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_tx != null) return;
            _tx = await _db.Database.BeginTransactionAsync(ct);
        }

        public Task SaveAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);

        public async Task CommitAsync(CancellationToken ct = default)
        {
            if (_tx == null) return;
            await _db.SaveChangesAsync(ct);
            await _tx.CommitAsync(ct);
            await _tx.DisposeAsync();
            _tx = null;
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            if (_tx == null) return;
            await _tx.RollbackAsync(ct);
            await _tx.DisposeAsync();
            _tx = null;
        }
    }
}
