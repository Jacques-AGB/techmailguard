using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechMailGuard.Domain.Events;

namespace TechMailGuard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestUnsubscribeController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestUnsubscribeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("trigger")]
    public async Task<IActionResult> TriggerManualTest(Guid mailboxId, Guid subId)
    {
        // On simule le déclenchement de l'événement de domaine directement
        // Dans la vraie appli, cet événement est lancé par l'agrégat Mailbox
        var domainEvent = new UnsubscribeRequestedEvent(mailboxId, subId);

        // On publie l'événement via MediatR
        await _mediator.Publish(domainEvent);

        return Ok(new { Message = "Événement envoyé au robot !", MailboxId = mailboxId });
    }
}