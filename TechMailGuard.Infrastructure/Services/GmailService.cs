using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using TechMailGuard.Domain.Interfaces;
using System.Text;

// Alias pour éviter la confusion entre ta classe GmailService et celle de Google
using GoogleApi = Google.Apis.Gmail.v1;

namespace TechMailGuard.Infrastructure.Services;

public class GmailService : IGmailService
{
    // Scopes nécessaires : lecture seule et modification (pour marquer comme lu ou supprimer si besoin)
    private readonly string[] Scopes = {
        GoogleApi.GmailService.Scope.GmailReadonly,
        GoogleApi.GmailService.Scope.GmailModify
    };

    private const string ApplicationName = "TechMailGuard";

    public async Task<string?> GetLatestEmailHtmlAsync(string senderEmail, CancellationToken ct)
    {
        UserCredential credential;

        // On définit le chemin vers le fichier secret dans le répertoire d'exécution (bin)
        string jsonPath = Path.Combine(AppContext.BaseDirectory, "client_secret.json");

        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException($"Le fichier d'identifiants est introuvable. Assure-toi qu'il est bien copié dans le répertoire de sortie (Propriétés -> Copier si plus récent). Chemin tenté : {jsonPath}");
        }

        // 1. Authentification OAuth2
        using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
        {
            // Le token sera stocké localement pour ne pas demander la connexion à chaque fois
            string credPath = Path.Combine(AppContext.BaseDirectory, "token.json");

            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                Scopes,
                "user",
                ct,
                new FileDataStore(credPath, true));
        }

        // 2. Création du service client Google
        var service = new GoogleApi.GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        // 3. Recherche du dernier message de l'expéditeur spécifique
        var listRequest = service.Users.Messages.List("me");
        listRequest.Q = $"from:{senderEmail}";
        listRequest.MaxResults = 1;

        var listResponse = await listRequest.ExecuteAsync(ct);
        var messageInfo = listResponse.Messages?.FirstOrDefault();

        if (messageInfo == null)
        {
            Console.WriteLine($"[GMAIL] Aucun message trouvé pour {senderEmail}");
            return null;
        }

        // 4. Récupération du contenu complet du message (Payload)
        var message = await service.Users.Messages.Get("me", messageInfo.Id).ExecuteAsync(ct);

        // 5. Extraction et décodage du HTML
        return ExtractHtmlFromBody(message);
    }

    private string? ExtractHtmlFromBody(Message message)
    {
        return FindHtmlPart(message.Payload);
    }

    private string? FindHtmlPart(MessagePart part)
    {
        // Si la partie actuelle est du HTML
        if (part.MimeType == "text/html" && part.Body?.Data != null)
        {
            // Décodage du format Base64Url spécifique à Google
            string base64Data = part.Body.Data.Replace('-', '+').Replace('_', '/');
            byte[] data = Convert.FromBase64String(base64Data);
            return Encoding.UTF8.GetString(data);
        }

        // Si le mail est "Multipart" (contient plusieurs parties), on cherche récursivement
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
}