using ConsumerDelivererAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsumerDelivererAPI.Infrastructure.Configurations
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
