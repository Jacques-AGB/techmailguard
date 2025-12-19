using System.Text.RegularExpressions;
using TechMailGuard.Domain.Interfaces;

namespace TechMailGuard.Infrastructure.Services;

public class EmailUnsubscribeService : IEmailUnsubscribeService
{
    private readonly HttpClient _httpClient;

    public EmailUnsubscribeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> TryUnsubscribeAsync(string emailHtmlContent, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(emailHtmlContent))
            return false;

        var regex = new Regex(
            @"href\s*=\s*[""'](?<url>https?://[^""']+)[""']",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        var matches = regex.Matches(emailHtmlContent);

        foreach (Match match in matches)
        {
            var url = match.Groups["url"].Value;

            if (!ContainsUnsubscribeKeyword(url))
                continue;

          
            if (await CallUrlPhysicallyAsync(url, ct))
                return true;
        }

        return false;
    }


    public async Task<bool> UnsubscribeFromHeaderAsync(string unsubscribeHeaderValue, CancellationToken ct)
    {
        var match = Regex.Match(unsubscribeHeaderValue, @"<(https?://[^>]+)>");

        if (match.Success)
        {
            return await CallUrlPhysicallyAsync(match.Groups[1].Value, ct);
        }

        return false;
    }


    private async Task<bool> CallUrlPhysicallyAsync(string url, CancellationToken ct)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) TechMailGuard/1.0");

            var response = await _httpClient.SendAsync(request, ct);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private bool ContainsUnsubscribeKeyword(string url)
    {
        var keywords = new[] { "unsubscribe", "desabonner", "optout", "manage-subscription" };
        return keywords.Any(k => url.Contains(k, StringComparison.OrdinalIgnoreCase));
    }
}