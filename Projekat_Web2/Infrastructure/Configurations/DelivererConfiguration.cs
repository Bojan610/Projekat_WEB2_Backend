using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekat_Web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Infrastructure.Configurations
{
    public class DelivererConfiguration : IEntityTypeConfiguration<Deliverer>
    {
        public void Configure(EntityTypeBuilder<Deliverer> builder)
        {
            /*builder.HasOne(x => x.CurrentOrder) //Consumer moze da ima jednu trenutnu porudzbinu
                 .WithOne(x => x.DelivererCurrent) // Jedna porudzbina se nalazi kod jednog Consumer-a
                 .HasForeignKey<Deliverer>(x => x.CurrentOrderID) // Strani kljuc  
                 .OnDelete(DeleteBehavior.Cascade);// Ako se obrise fakultet kaskadno se brisu svi njegovi studenti
        */

           
        }
    }
}
