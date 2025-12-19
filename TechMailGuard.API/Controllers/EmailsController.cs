using Microsoft.AspNetCore.Mvc;
using TechMailGuard.Application.Dtos;
using TechMailGuard.Domain.Dtos;
using TechMailGuard.Domain.Interfaces;
using TechMailGuard.Infrastructure.Services;

namespace TechMailGuard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailsController : ControllerBase
{
    private readonly IGeminiService _geminiService;
    private readonly IGmailService _gmailService;

    public EmailsController(IGmailService gmailService, IGeminiService geminiService)
    {
        _gmailService = gmailService;
        _geminiService = geminiService;
    }

    [HttpGet("newsletters")]
    public async Task<IActionResult> GetNewsletters(CancellationToken ct)
    {
        var dtos = await _gmailService.GetLatestNewslettersAsync(ct);

        if (dtos == null || !dtos.Any())
        {
            return Ok(new List<NewsletterDto>()); 
        }

        return Ok(dtos);
    }

    [HttpPost("generate-veille")]
    public async Task<IActionResult> GenerateVeille([FromBody] VeilleRequest request)
    {

        var emails = await _gmailService.GetLatestNewslettersAsync(default);

  
        var contents = emails.Select(e => e.Body).ToList();

        var rapport = await _geminiService.GenerateVeilleAsync(request.Sujets, contents);

        return Ok(new { Rapport = rapport });
    }
}
