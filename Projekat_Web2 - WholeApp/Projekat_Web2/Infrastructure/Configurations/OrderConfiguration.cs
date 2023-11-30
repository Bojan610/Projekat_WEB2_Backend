using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekat_Web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id); //Podesavam primarni kljuc tabele

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            /*
            builder.HasMany
            builder.HasOne(x => x.ConsumerPrevious) //Kazemo da Student ima jedan fakultet
                  .WithMany(x => x.MyPreviousOrders) // A jedan fakultet vise studenata
                  .HasForeignKey(x => x.ConsumerPreviousID) // Strani kljuc  je facultyId
                  .OnDelete(DeleteBehavior.NoAction);// Ako se obrise fakultet kaskadno se brisu svi njegovi studenti

            builder.HasOne(x => x.DelivererPrevious) //Kazemo da Student ima jedan fakultet
                 .WithMany(x => x.MyPreviousOrders) // A jedan fakultet vise studenata
                 .HasForeignKey(x => x.DelivererPreviousID) // Strani kljuc  je facultyId
                 .OnDelete(DeleteBehavior.NoAction);// Ako se obrise fakultet kaskadno se brisu svi njegovi studenti
            
            */
            builder.HasOne(x => x.ConsumerCurrent)
                .WithOne(x => x.CurrentOrder_Con)
                .HasForeignKey<Order>(x => x.ConsumerCurrentID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.DelivererCurrent)
              .WithOne(x => x.CurrentOrder_Del)
              .HasForeignKey<Order>(x => x.DelivererCurrentID)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Products) //Student slusa vise predmeta
                 .WithMany(x => x.Orders);//Na jednom predmetu je vise studenata            
        }
    }
}
