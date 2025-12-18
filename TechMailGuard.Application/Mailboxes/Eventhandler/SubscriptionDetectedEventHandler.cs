using MediatR;
using TechMailGuard.Domain.Events;

namespace TechMailGuard.Application.Mailboxes.Eventhandler;
public class SubscriptionDetectedEventHandler : INotificationHandler<SubscriptionDetectedEvent>
{
    public SubscriptionDetectedEventHandler()
    {
    }

    public Task Handle(SubscriptionDetectedEvent notification, CancellationToken ct)
    {

        Console.WriteLine("***************************************************");
        Console.WriteLine($"[DOMAIN EVENT] Nouveau contrat détecté !");
        Console.WriteLine($"Mailbox ID: {notification.MailboxId}");
        Console.WriteLine($"Subscription ID: {notification.SubscriptionId}");
        Console.WriteLine($"Détecté le: {DateTime.Now}");
        Console.WriteLine("***************************************************");

        return Task.CompletedTask;
    }
}
