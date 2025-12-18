using MediatR;
using TechMailGuard.Domain.Interfaces;

namespace TechMailGuard.Application.Mailboxes.Commands.Unsubscribe;
public record UnsubscribeCommand(Guid MailboxId, Guid SubscriptionId) : IRequest;
public class UnsubscribeCommandHandler : IRequestHandler<UnsubscribeCommand>
{
    private readonly IMailboxRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UnsubscribeCommandHandler(IMailboxRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UnsubscribeCommand request, CancellationToken ct)
    {
        var mailbox = await _repository.GetByIdAsync(request.MailboxId, ct);
        if (mailbox == null) throw new KeyNotFoundException();

        mailbox.MarkSubscriptionForUnsubscribe(request.SubscriptionId);

        await _unitOfWork.SaveChangesAsync(ct);
    }
}

