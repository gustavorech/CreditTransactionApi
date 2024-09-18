using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditTransactionApi.Data;

public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
{
    public void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired();

        builder.HasOne(e => e.MerchantCategory)
            .WithMany(e => e.Merchants)
            .HasForeignKey(e => e.MerchantCategoryCode)
            .IsRequired();
    }
}