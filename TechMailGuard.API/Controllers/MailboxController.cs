using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechMailGuard.API.Contracts;
using TechMailGuard.Application.Mailboxes.Commands.AddSubscription;
using TechMailGuard.Application.Mailboxes.Commands.CreateMailbox;
using TechMailGuard.Application.Mailboxes.Queries.GetMailboxes;

namespace TechMailGuard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailboxController : ControllerBase
{
    private readonly IMediator _mediator;

    public MailboxController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMailboxCommand command, CancellationToken ct)
    {
        try
        {
            var result = await _mediator.Send(command, ct);
            return Ok(new { id = result });
        }
        catch (ArgumentException ex) 
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex) 
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/subscriptions")]
    public async Task<IActionResult> AddSubscription(Guid id, [FromBody] AddSubscriptionRequest request, CancellationToken ct)
    {
        try
        {
            var command = new AddSubscriptionCommand(id, request.SenderEmail, request.NewsletterName);

            var subscriptionId = await _mediator.Send(command, ct);
            return Ok(new { id = subscriptionId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var query = new GetMailboxesQuery();
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    
}