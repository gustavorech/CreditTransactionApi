using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditTransactionApi.Data;

public class TransactionRequestConfiguration : IEntityTypeConfiguration<TransactionRequest>
{
    public void Configure(EntityTypeBuilder<TransactionRequest> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.AccountId)
            .IsRequired();

        builder.Property(e => e.Amount)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(e => e.MerchantCategoryCode)
            .IsRequired();

        builder.Property(e => e.TransactionDate)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(e => e.ResultCode)
            .HasConversion<EnumToStringValueConverter<TransactionResultCode>>()
            .IsRequired(false);
    }
}