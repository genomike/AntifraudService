using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AntifraudService.Domain.Entities;

namespace AntifraudService.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.SourceAccountId)
                .IsRequired();

            builder.Property(t => t.TargetAccountId)
                .IsRequired();

            builder.Property(t => t.TransferTypeId)
                .IsRequired();

            builder.Property(t => t.Value)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Status)
                .IsRequired();

            builder.ToTable("Transactions");
        }
    }
}