using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditTransactionApi.Data;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(e => e.TransactionRequest)
            .WithOne(e => e.CreditTransaction)
            .HasForeignKey<Transaction>(e => e.TransactionRequestId)
            .IsRequired();

        builder.HasOne(e => e.AccountPartition)
            .WithMany(e => e.Transactions)
            .HasForeignKey(e => e.AccountPartitionId)
            .IsRequired();

        builder.HasOne(e => e.Merchant)
            .WithMany(e => e.Transactions)
            .HasForeignKey(e => e.MerchantId)
            .IsRequired(false);

        builder.Property(e => e.PreviousBalance)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(e => e.NewBalance)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();
    }
}