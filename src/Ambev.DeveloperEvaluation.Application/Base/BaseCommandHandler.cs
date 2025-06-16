using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Application.Base
{
    public abstract class BaseCommandHandler
    {
        private readonly IUnitOfWork _uow;

        protected BaseCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        protected async Task<bool> Commit(CancellationToken cancellationToken = default)
        {
            return await _uow.Commit(cancellationToken);
        }
    }
}
