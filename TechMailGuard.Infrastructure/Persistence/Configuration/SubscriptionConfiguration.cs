using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechMailGuard.Domain.Entities;
using TechMailGuard.Domain.ValueObjects;

namespace TechMailGuard.Infrastructure.Persistence.Configuration;
public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.SenderEmail)
            .HasConversion(
                email => email.Value,
                value => EmailAddress.Create(value)
            )
            .HasColumnName("SenderEmail")
            .IsRequired();


        builder.Property(s => s.Status)
            .HasConversion<string>();
    }
}
