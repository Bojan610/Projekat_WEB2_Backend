﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserAdminAPI.Infrastructure;

namespace UserAdminAPI.Migrations
{
    [DbContext(typeof(UsersDbContext))]
    partial class UsersDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("UserAdminAPI.Models.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Birth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("UserKind")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .HasColumnType("longtext");

                    b.HasKey("Email");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("UserAdminAPI.Models.Admin", b =>
                {
                    b.HasBaseType("UserAdminAPI.Models.User");

                    b.HasDiscriminator().HasValue("Admin");
                });

            modelBuilder.Entity("UserAdminAPI.Models.Consumer", b =>
                {
                    b.HasBaseType("UserAdminAPI.Models.User");

                    b.HasDiscriminator().HasValue("Consumer");
                });

            modelBuilder.Entity("UserAdminAPI.Models.Deliverer", b =>
                {
                    b.HasBaseType("UserAdminAPI.Models.User");

                    b.Property<string>("Verified")
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("Deliverer");
                });
#pragma warning restore 612, 618
        }
    }
}
