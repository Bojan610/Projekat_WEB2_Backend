using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekat_Web2.Models;

namespace Projekat_Web2.Infrastructure.Configurations
{
    public class OrderProductDetailsConfiguration : IEntityTypeConfiguration<OrderProductDetails>
    {
        public void Configure(EntityTypeBuilder<OrderProductDetails> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
