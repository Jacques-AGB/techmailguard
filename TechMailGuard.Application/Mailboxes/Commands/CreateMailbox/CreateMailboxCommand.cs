using MediatR;
using TechMailGuard.Domain.Aggregates;
using TechMailGuard.Domain.Interfaces;
using TechMailGuard.Domain.ValueObjects;

namespace TechMailGuard.Application.Mailboxes.Commands.CreateMailbox;
public record CreateMailboxCommand( string Email, string Provider) : IRequest<Guid>;

public class CreateMailboxCommandHandler : IRequestHandler<CreateMailboxCommand, Guid>
{
    private readonly IMailboxRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMailboxCommandHandler(IMailboxRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }


    public async Task<Guid> Handle(CreateMailboxCommand request, CancellationToken cancellationToken)
    {
        var emailVo = EmailAddress.Create(request.Email);

        var tempUserId = Guid.NewGuid();

        var mailbox = Mailbox.Create(
            tempUserId,
            emailVo,
            request.Provider 
        );

        await _repository.AddAsync(mailbox, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return mailbox.Id;
    }
}