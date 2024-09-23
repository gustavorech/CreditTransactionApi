﻿// <auto-generated />
using System;
using CreditTransactionApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CreditTransactionApi.Data.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.AccountPartition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<string>("AccountPartitionType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("AccountPartitions");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.Merchant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("MerchantCategoryCode")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MerchantCategoryCode");

                    b.ToTable("Merchants");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.MerchantCategory", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Code"));

                    b.Property<string>("AccountPartitionType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("MerchantCategories");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.TransactionEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountPartitionId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("MerchantId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("NewBalance")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("PreviousBalance")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<Guid>("TransactionRequestId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AccountPartitionId");

                    b.HasIndex("MerchantId");

                    b.HasIndex("TransactionRequestId")
                        .IsUnique();

                    b.ToTable("TransactionEntries");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.TransactionRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("MerchantCategoryCode")
                        .HasColumnType("integer");

                    b.Property<string>("MerchantName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ResultCode")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("TransactionDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("TransactionRequests");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.AccountPartition", b =>
                {
                    b.HasOne("CreditTransactionApi.Data.Account", "Account")
                        .WithMany("AccountPartitions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.Merchant", b =>
                {
                    b.HasOne("CreditTransactionApi.Data.MerchantCategory", "MerchantCategory")
                        .WithMany("Merchants")
                        .HasForeignKey("MerchantCategoryCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MerchantCategory");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.TransactionEntry", b =>
                {
                    b.HasOne("CreditTransactionApi.Data.AccountPartition", "AccountPartition")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountPartitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CreditTransactionApi.Data.Merchant", "Merchant")
                        .WithMany("Transactions")
                        .HasForeignKey("MerchantId");

                    b.HasOne("CreditTransactionApi.Data.TransactionRequest", "TransactionRequest")
                        .WithOne("CreditTransaction")
                        .HasForeignKey("CreditTransactionApi.Data.TransactionEntry", "TransactionRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountPartition");

                    b.Navigation("Merchant");

                    b.Navigation("TransactionRequest");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.Account", b =>
                {
                    b.Navigation("AccountPartitions");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.AccountPartition", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.Merchant", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.MerchantCategory", b =>
                {
                    b.Navigation("Merchants");
                });

            modelBuilder.Entity("CreditTransactionApi.Data.TransactionRequest", b =>
                {
                    b.Navigation("CreditTransaction");
                });
#pragma warning restore 612, 618
        }
    }
}
