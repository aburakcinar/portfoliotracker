﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PortfolioTracker.WebApp.DataStore;

#nullable disable

namespace PortfolioTracker.WebApp.Migrations
{
    [DbContext(typeof(PortfolioContext))]
    [Migration("20241206230905_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("PortfolioTracker.WebApp.DataStore.StockPurchase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TransactionGroupId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TransactionGroupId");

                    b.ToTable("StockPurchases");
                });

            modelBuilder.Entity("PortfolioTracker.WebApp.DataStore.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int>("InOut")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("TransactionGroupId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TransactionGroupId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("PortfolioTracker.WebApp.DataStore.TransactionGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TransactionGroups");
                });

            modelBuilder.Entity("PortfolioTracker.WebApp.DataStore.StockPurchase", b =>
                {
                    b.HasOne("PortfolioTracker.WebApp.DataStore.TransactionGroup", "TransactionGroup")
                        .WithMany()
                        .HasForeignKey("TransactionGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TransactionGroup");
                });

            modelBuilder.Entity("PortfolioTracker.WebApp.DataStore.Transaction", b =>
                {
                    b.HasOne("PortfolioTracker.WebApp.DataStore.TransactionGroup", null)
                        .WithMany("Transactions")
                        .HasForeignKey("TransactionGroupId");
                });

            modelBuilder.Entity("PortfolioTracker.WebApp.DataStore.TransactionGroup", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
