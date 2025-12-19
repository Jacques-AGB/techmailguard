using System.Net.Http.Json;
using TechMailGuard.Application.Dtos;

namespace TechMailGuard.Web.Services;

public class EmailHttpClient
{
    private readonly HttpClient _http;

    public EmailHttpClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<NewsletterDto>> GetNewslettersAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<NewsletterDto>>("api/emails/newsletters")
                   ?? new List<NewsletterDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur API : {ex.Message}");
            return new List<NewsletterDto>();
        }
    }
}