using System.Text.RegularExpressions;

namespace TechMailGuard.Domain.ValueObjects;
public readonly record struct EmailAddress 
{
    private static readonly Regex EmailRegex  =  new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    private string value { get; init; }

    private EmailAddress(string value) 
    {
        value = value.ToLowerInvariant();
    }

    private static EmailAddress Create(string value) 
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email address cannot be null or empty.", nameof(value));

        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException("Invalid email address format.", nameof(value));

        return new EmailAddress(value);
    }
}
