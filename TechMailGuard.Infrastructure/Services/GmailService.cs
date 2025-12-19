using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Text;
using System.Text.RegularExpressions;
using TechMailGuard.Application.Dtos;
using TechMailGuard.Domain.Interfaces;


using GoogleApi = Google.Apis.Gmail.v1;

namespace TechMailGuard.Infrastructure.Services;

public class GmailService : IGmailService
{
    private readonly string[] Scopes = {
        GoogleApi.GmailService.Scope.GmailReadonly,
        GoogleApi.GmailService.Scope.GmailModify
    };

    private const string ApplicationName = "TechMailGuard";



    public async Task<List<NewsletterDto>> GetLatestNewslettersAsync(CancellationToken ct)
    {
        var result = new List<NewsletterDto>();
        var service = await GetGmailServiceInstanceAsync(ct);

        var listRequest = service.Users.Messages.List("me");
        listRequest.MaxResults = 30;
        var listResponse = await listRequest.ExecuteAsync(ct);

        if (listResponse.Messages == null) return result;

        foreach (var msgSummary in listResponse.Messages)
        {

            var msg = await service.Users.Messages.Get("me", msgSummary.Id).ExecuteAsync(ct);

            var headers = msg.Payload.Headers;
            var unsubscribeHeader = headers.FirstOrDefault(h => h.Name == "List-Unsubscribe")?.Value;


            if (!string.IsNullOrEmpty(unsubscribeHeader))
            {
                var from = headers.FirstOrDefault(h => h.Name == "From")?.Value ?? "Inconnu";
                var subject = headers.FirstOrDefault(h => h.Name == "Subject")?.Value ?? "Sans sujet";


                var match = Regex.Match(from, @"(.*)<(.*)>");
                string name = match.Success ? match.Groups[1].Value.Trim() : from;
                string email = match.Success ? match.Groups[2].Value.Trim() : from;

                result.Add(new NewsletterDto
                {
                    MessageId = msg.Id,
                    SenderName = name.Replace("\"", ""),
                    SenderEmail = email,
                    Body = ExtractBody(msg),
                    Subject = subject,
                    ReceivedAt = DateTimeOffset.FromUnixTimeMilliseconds(msg.InternalDate ?? 0).DateTime,
                    HasUnsubscribeHeader = true,
                    UnsubscribeUrl = ExtractUrl(unsubscribeHeader)
                });
            }
        }
        return result;
    }



    public async Task<string?> GetLatestEmailHtmlAsync(string senderEmail, CancellationToken ct)
    {
        var service = await GetGmailServiceInstanceAsync(ct);

        var listRequest = service.Users.Messages.List("me");
        listRequest.Q = $"from:{senderEmail}";
        listRequest.MaxResults = 1;

        var listResponse = await listRequest.ExecuteAsync(ct);
        var messageInfo = listResponse.Messages?.FirstOrDefault();

        if (messageInfo == null) return null;

        var message = await service.Users.Messages.Get("me", messageInfo.Id).ExecuteAsync(ct);
        return ExtractHtmlFromBody(message);
    }



    private async Task<GoogleApi.GmailService> GetGmailServiceInstanceAsync(CancellationToken ct)
    {
        string jsonPath = Path.Combine(AppContext.BaseDirectory, "client_secret.json");

        if (!File.Exists(jsonPath))
            throw new FileNotFoundException($"Fichier client_secret.json introuvable à : {jsonPath}");

        UserCredential credential;
        using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
        {
            string credPath = Path.Combine(AppContext.BaseDirectory, "token.json");
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                Scopes,
                "user",
                ct,
                new FileDataStore(credPath, true));
        }

        return new GoogleApi.GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }

    private string? ExtractHtmlFromBody(Message message) => FindHtmlPart(message.Payload);

    private string? FindHtmlPart(MessagePart part)
    {
        if (part.MimeType == "text/html" && part.Body?.Data != null)
        {
            string base64Data = part.Body.Data.Replace('-', '+').Replace('_', '/');
            byte[] data = Convert.FromBase64String(base64Data);
            return Encoding.UTF8.GetString(data);
        }

        if (part.Parts != null)
        {
            foreach (var subPart in part.Parts)
            {
                var result = FindHtmlPart(subPart);
                if (result != null) return result;
            }
        }
        return null;
    }

    private string? ExtractUrl(string headerValue)
    {
        var match = Regex.Match(headerValue, @"<(https?://[^>]+)>");
        return match.Success ? match.Groups[1].Value : null;
    }
    private string ExtractBody(Message msg)
    {
        string body = "";

        if (msg.Payload.Parts != null && msg.Payload.Parts.Count > 0)
        {

            var part = msg.Payload.Parts.FirstOrDefault(p => p.MimeType == "text/plain")
                       ?? msg.Payload.Parts.FirstOrDefault(p => p.MimeType == "text/html");

            if (part?.Body?.Data != null)
            {
                body = part.Body.Data;
            }
        }
        else if (msg.Payload.Body?.Data != null)
        {
            body = msg.Payload.Body.Data;
        }

        if (string.IsNullOrEmpty(body)) return "Contenu indisponible";


        byte[] data = Convert.FromBase64String(body.Replace('-', '+').Replace('_', '/'));
        return System.Text.Encoding.UTF8.GetString(data);
    }

}