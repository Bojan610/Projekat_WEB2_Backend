using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekat_Web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Infrastructure.Configurations
{
    public class ConsumerConfiguration : IEntityTypeConfiguration<Consumer>
    {
        public void Configure(EntityTypeBuilder<Consumer> builder)
        {  
            /*
            builder.HasOne(x => x.CurrentOrder_Con) //Consumer moze da ima jednu trenutnu porudzbinu
                  .WithOne(x => x.ConsumerCurrent) // Jedna porudzbina se nalazi kod jednog Consumer-a
                  .HasForeignKey<Consumer>(x => x.CurrentOrderID_Con) // Strani kljuc  
                  .OnDelete(DeleteBehavior.NoAction);// Ako se obrise fakultet kaskadno se brisu svi njegovi studenti
            */        
            
            builder.HasMany(x => x.Products)
                   .WithMany(x => x.Consumers);       
        }
    }
}
