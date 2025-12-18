using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMailGuard.Domain.Interfaces;
using TechMailGuard.Domain.ValueObjects;

namespace TechMailGuard.Application.Mailboxes.Commands.AddSubscription;
public record AddSubscriptionCommand(Guid MailboxId,string SenderEmail,string NewsletterName) : IRequest<Guid>;

public class AddSubscriptionCommandHandler : IRequestHandler<AddSubscriptionCommand, Guid>
{
    private readonly IMailboxRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddSubscriptionCommandHandler(IMailboxRepository subscriptionRepository, IUnitOfWork unitOfWork)
    {
        _repository = subscriptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddSubscriptionCommand request, CancellationToken ct)
    {
        var mailbox = await _repository.GetByIdAsync(request.MailboxId, ct);

        if (mailbox == null)
            throw new KeyNotFoundException($"Mailbox {request.MailboxId} not found.");

       
        var senderEmailVo = EmailAddress.Create(request.SenderEmail);
        var subscription = mailbox.AddSubscrition(senderEmailVo, request.NewsletterName);

        await _unitOfWork.SaveChangesAsync(ct);

        return subscription.Id;
    }
}
