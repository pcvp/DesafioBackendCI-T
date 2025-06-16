using Ambev.DeveloperEvaluation.Application.Base;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

/// <summary>
/// Handler for processing DeleteUserCommand requests
/// </summary>
public class DeleteUserHandler : BaseCommandHandler, IRequestHandler<DeleteUserCommand, DeleteUserResponse>
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of DeleteUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="unitOfWork">The unit of work</param>
    public DeleteUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Handles the DeleteUserCommand request
    /// </summary>
    /// <param name="command">The DeleteUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The delete operation result</returns>
    public async Task<DeleteUserResponse> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.Id, cancellationToken);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {command.Id} not found");

        var success = await _userRepository.DeleteAsync(command.Id, cancellationToken);
        if (!success)
            throw new InvalidOperationException($"Failed to delete user with ID {command.Id}");

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit user deletion transaction");

        return new DeleteUserResponse { Success = true };
    }
}
