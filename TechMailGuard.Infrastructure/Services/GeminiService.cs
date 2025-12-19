using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TechMailGuard.Domain.Interfaces;

namespace TechMailGuard.Infrastructure.Services;
public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["Gemini:ApiKey"] ?? throw new ArgumentNullException("ApiKey manquante");
    }

    public async Task<string> GenerateVeilleAsync(string sujets, List<string> emailContents)
    {


      
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3-flash-preview:generateContent?key={_apiKey}";

        var prompt = $"Tu es un expert en veille technologique pour KokouinTech. " +
             $"Résume les emails suivants en HTML (utilise <ul>, <li>, <h4>) sur les sujets : {sujets}. " +
             $"IMPORTANT : Pour chaque offre ou article cité, trouve dans le contenu de l'email le lien URL direct " +
             $"(vers le site de recrutement, le blog ou l'article original). " +
             $"Ajoute ce lien à la fin de chaque point sous la forme : " +
             $"<a href='URL_TROUVEE' target='_blank' class='ms-2 text-orange' style='font-size: 0.8rem;'>[Accéder à la page]</a>. " +
             $"Si tu ne trouves pas de lien direct, n'affiche rien pour ce point. " +
             $"Emails : {string.Join(" || ", emailContents)}";

        var requestBody = new
        {
            contents = new[] { new { parts = new[] { new { text = prompt } } } }
        };

        var response = await _httpClient.PostAsJsonAsync(url, requestBody);

       
        if (!response.IsSuccessStatusCode)
        {
            var errorDetails = await response.Content.ReadAsStringAsync();
            return $"<p class='text-danger'>Erreur API Gemini ({response.StatusCode}) : {errorDetails}</p>";
        }

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        try
        {
         
            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text").GetString() ?? "Résumé indisponible.";
        }
        catch (KeyNotFoundException)
        {
            return "<p class='text-warning'>L'IA a répondu mais le format est inattendu.</p>";
        }
    }
}