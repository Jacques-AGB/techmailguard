using MediatR;
using TechMailGuard.Domain.Events;
using TechMailGuard.Domain.Interfaces;

namespace TechMailGuard.Application.Mailboxes.Eventhandler;
public class ProcessPhysicalUnsubscribeHandler : INotificationHandler<UnsubscribeRequestedEvent>
{

    private readonly IEmailUnsubscribeService _unsubscribeService;
    private readonly IGmailService _gmailService;

    public ProcessPhysicalUnsubscribeHandler(
        IEmailUnsubscribeService unsubscribeService,
        IGmailService gmailService)
    {
        // IL MANQUAIT CES DEUX LIGNES :
        _unsubscribeService = unsubscribeService;
        _gmailService = gmailService;
    }

    public async Task Handle(UnsubscribeRequestedEvent notification, CancellationToken ct)
    {
        // On récupère l'email de l'expéditeur depuis l'événement 
        // (Assure-toi que ton record UnsubscribeRequestedEvent contient cette info)
        string senderEmail = "kokouintech@gmail.com"; // À remplacer par notification.SenderEmail plus tard

        Console.WriteLine($"[ROBOT] Recherche du dernier mail de : {senderEmail}...");

        // 1. Appel au vrai service Gmail (Ouvre le navigateur la 1ère fois)
        string? realHtml = await _gmailService.GetLatestEmailHtmlAsync(senderEmail, ct);

        if (!string.IsNullOrEmpty(realHtml))
        {
            // 2. Tentative de désinscription "physique"
            bool success = await _unsubscribeService.TryUnsubscribeAsync(realHtml, ct);

            if (success)
            {
                Console.WriteLine($"[SUCCESS] Le robot a trouvé et cliqué sur le lien pour : {notification.SubscriptionId}");

                // ÉTAPE FUTURE : Tu pourrais ici déclencher une autre commande 
                // pour passer le statut de la souscription à "Unsubscribed" officiellement.
            }
            else
            {
                Console.WriteLine("[AIDE] Aucun lien de désinscription trouvé dans le contenu HTML.");
            }
        }
        else
        {
            Console.WriteLine("[ERREUR] Aucun mail trouvé pour cet expéditeur dans Gmail.");
        }
    }
}
