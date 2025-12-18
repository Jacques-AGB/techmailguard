using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechMailGuard.Domain.Aggregates;
using TechMailGuard.Domain.ValueObjects;

namespace TechMailGuard.Infrastructure.Persistence.Configurations;

public class MailboxConfiguration : IEntityTypeConfiguration<Mailbox>
{
    public void Configure(EntityTypeBuilder<Mailbox> builder)
    {
        builder.ToTable("Mailboxes");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.UserId)
            .IsRequired();

        builder.Property(m => m.Provider)
            .HasMaxLength(50) 
            .IsRequired();

        builder.Property(m => m.EmailAddress)
            .HasConversion(
                email => email.Value,
                value => EmailAddress.Create(value)
            )
            .HasColumnName("Email")
            .IsRequired();

        builder.HasMany(m => m.Subscriptions)
            .WithOne()
            .HasForeignKey("MailboxId")
            .OnDelete(DeleteBehavior.Cascade); 
    }
}