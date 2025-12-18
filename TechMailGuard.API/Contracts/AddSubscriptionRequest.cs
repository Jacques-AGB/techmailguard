namespace TechMailGuard.API.Contracts;

public record AddSubscriptionRequest(
    string SenderEmail,
    string NewsletterName
);