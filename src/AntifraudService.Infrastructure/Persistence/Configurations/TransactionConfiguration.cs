using AntifraudService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntifraudService.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .ValueGeneratedOnAdd();

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
                .IsRequired()
                .HasDefaultValue(TransactionStatus.Pending);

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); // También podemos hacer que CreatedAt se genere automáticamente

            builder.ToTable("Transactions");
        }
    }
}