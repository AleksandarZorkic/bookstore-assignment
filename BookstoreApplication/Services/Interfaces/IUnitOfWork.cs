using System.Threading;
using System.Threading.Tasks;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task SaveAsync(CancellationToken ct = default);
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }
}
