using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekat_Web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Infrastructure.Configurations
{
    public class ConsumerProductConfiguration : IEntityTypeConfiguration<ConsumerProduct>
    {
        public void Configure(EntityTypeBuilder<ConsumerProduct> builder)
        {
            //builder.HasKey(x => new { x.ConsumerID, x.ProductID});

            builder.HasKey(x => x.Id); //Podesavam primarni kljuc tabele
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            /*
            builder.HasOne(x => x.Consumer)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ConsumerID);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Consumers)
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}
