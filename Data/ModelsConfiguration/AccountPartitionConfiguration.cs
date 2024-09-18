using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditTransactionApi.Data;

public class AccountPartitionConfiguration : IEntityTypeConfiguration<AccountPartition>
{
    public void Configure(EntityTypeBuilder<AccountPartition> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.AccountPartitionType)
            .HasConversion(
                v => v.ToString(),
                v => (AccountPartitionType)Enum.Parse(typeof(AccountPartitionType), v))
            .IsRequired();

        builder.HasOne(e => e.Account)
            .WithMany(e => e.AccountPartitions)
            .HasForeignKey(e => e.AccountId)
            .IsRequired();

        builder.Property(e => e.Balance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
    }
}