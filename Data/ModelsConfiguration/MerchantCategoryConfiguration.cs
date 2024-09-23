using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditTransactionApi.Data;

public class MerchantCategoryConfiguration : IEntityTypeConfiguration<MerchantCategory>
{
    public void Configure(EntityTypeBuilder<MerchantCategory> builder)
    {
        builder.HasKey(e => e.Code);

        builder.Property(e => e.Description)
            .IsRequired();

        builder.Property(e => e.AccountPartitionType)
            .HasConversion(
                v => v.ToString(),
                v => (AccountPartitionType)Enum.Parse(typeof(AccountPartitionType), v))
            .IsRequired();
    }
}