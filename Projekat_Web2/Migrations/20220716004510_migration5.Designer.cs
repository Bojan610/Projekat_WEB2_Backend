﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Projekat_Web2.Infrastructure;

namespace Projekat_Web2.Migrations
{
    [DbContext(typeof(WebAppDbContext))]
    [Migration("20220716004510_migration5")]
    partial class migration5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Projekat_Web2.Models.ConsumerProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConsumerID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ConsumerID");

                    b.HasIndex("ProductID");

                    b.ToTable("ConsumerProduct");
                });

            modelBuilder.Entity("Projekat_Web2.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConsumerEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DelivererEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConsumerEmail");

                    b.HasIndex("DelivererEmail");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Projekat_Web2.Models.OrderProduct", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderProduct");
                });

            modelBuilder.Entity("Projekat_Web2.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConsumerEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Ingredients")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConsumerEmail");

                    b.HasIndex("OrderId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Projekat_Web2.Models.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Birth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserKind")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("Projekat_Web2.Models.Admin", b =>
                {
                    b.HasBaseType("Projekat_Web2.Models.User");

                    b.HasDiscriminator().HasValue("Admin");
                });

            modelBuilder.Entity("Projekat_Web2.Models.Consumer", b =>
                {
                    b.HasBaseType("Projekat_Web2.Models.User");

                    b.Property<int?>("CurrentOrder_ConId")
                        .HasColumnType("int");

                    b.HasIndex("CurrentOrder_ConId");

                    b.HasDiscriminator().HasValue("Consumer");
                });

            modelBuilder.Entity("Projekat_Web2.Models.Deliverer", b =>
                {
                    b.HasBaseType("Projekat_Web2.Models.User");

                    b.Property<int?>("CurrentOrder_DelId")
                        .HasColumnType("int");

                    b.Property<string>("Verified")
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("CurrentOrder_DelId");

                    b.HasDiscriminator().HasValue("Deliverer");
                });

            modelBuilder.Entity("Projekat_Web2.Models.ConsumerProduct", b =>
                {
                    b.HasOne("Projekat_Web2.Models.Consumer", "Consumer")
                        .WithMany()
                        .HasForeignKey("ConsumerID");

                    b.HasOne("Projekat_Web2.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consumer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Projekat_Web2.Models.Order", b =>
                {
                    b.HasOne("Projekat_Web2.Models.Consumer", null)
                        .WithMany("MyPreviousOrders")
                        .HasForeignKey("ConsumerEmail");

                    b.HasOne("Projekat_Web2.Models.Deliverer", null)
                        .WithMany("MyPreviousOrders")
                        .HasForeignKey("DelivererEmail");
                });

            modelBuilder.Entity("Projekat_Web2.Models.OrderProduct", b =>
                {
                    b.HasOne("Projekat_Web2.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projekat_Web2.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Projekat_Web2.Models.Product", b =>
                {
                    b.HasOne("Projekat_Web2.Models.Consumer", null)
                        .WithMany("Products")
                        .HasForeignKey("ConsumerEmail");

                    b.HasOne("Projekat_Web2.Models.Order", null)
                        .WithMany("Products")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("Projekat_Web2.Models.Consumer", b =>
                {
                    b.HasOne("Projekat_Web2.Models.Order", "CurrentOrder_Con")
                        .WithMany()
                        .HasForeignKey("CurrentOrder_ConId");

                    b.Navigation("CurrentOrder_Con");
                });

            modelBuilder.Entity("Projekat_Web2.Models.Deliverer", b =>
                {
                    b.HasOne("Projekat_Web2.Models.Order", "CurrentOrder_Del")
                        .WithMany()
                        .HasForeignKey("CurrentOrder_DelId");

                    b.Navigation("CurrentOrder_Del");
                });

            modelBuilder.Entity("Projekat_Web2.Models.Order", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Projekat_Web2.Models.Consumer", b =>
                {
                    b.Navigation("MyPreviousOrders");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Projekat_Web2.Models.Deliverer", b =>
                {
                    b.Navigation("MyPreviousOrders");
                });
#pragma warning restore 612, 618
        }
    }
}
