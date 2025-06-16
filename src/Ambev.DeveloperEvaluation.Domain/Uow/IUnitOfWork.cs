namespace Ambev.DeveloperEvaluation.Domain.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit(CancellationToken cancellationToken);
    }
}
