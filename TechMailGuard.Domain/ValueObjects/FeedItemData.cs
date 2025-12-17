namespace TechMailGuard.Domain.ValueObjects;
public sealed record FeedItemData
{
    public string Title { get; init; }
    public string ItemUrl { get; init; }
    public string Summary { get; init; }
    public DateTimeOffset PublishDate { get; init; }
    public string AuthorName { get; init; }

   
    public FeedItemData(string title, string itemUrl, string summary, DateTimeOffset publishDate, string authorName)
    {
       
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty.", nameof(title));

        Title = title;
        ItemUrl = itemUrl;
        Summary = summary;
        PublishDate = publishDate;
        AuthorName = authorName;
    }
}
