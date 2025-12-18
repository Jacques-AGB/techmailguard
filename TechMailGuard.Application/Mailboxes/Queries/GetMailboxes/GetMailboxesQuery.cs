using MediatR;
using TechMailGuard.Domain.Interfaces;

namespace TechMailGuard.Application.Mailboxes.Queries.GetMailboxes;
public record GetMailboxesQuery() : IRequest<IEnumerable<MailboxResponse>>;

public class GetMailboxesQueryHandler : IRequestHandler<GetMailboxesQuery, IEnumerable<MailboxResponse>>
{
    private readonly IMailboxRepository _repository;

    public GetMailboxesQueryHandler(IMailboxRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MailboxResponse>> Handle(GetMailboxesQuery request, CancellationToken ct)
    {
        var mailboxes = await _repository.GetAllAsync(ct);

        return mailboxes.Select(m => new MailboxResponse(
            m.Id,
            m.EmailAddress.Value,
            m.Provider));
    }
}