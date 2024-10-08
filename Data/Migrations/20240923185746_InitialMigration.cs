﻿using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MerchantCategories",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AccountPartitionType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantCategories", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "TransactionRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MerchantCategoryCode = table.Column<int>(type: "integer", nullable: false),
                    MerchantName = table.Column<string>(type: "text", nullable: false),
                    TransactionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ResultCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountPartitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    AccountPartitionType = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPartitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountPartitions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MerchantCategoryCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Merchants_MerchantCategories_MerchantCategoryCode",
                        column: x => x.MerchantCategoryCode,
                        principalTable: "MerchantCategories",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountPartitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchantId = table.Column<Guid>(type: "uuid", nullable: true),
                    PreviousBalance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    NewBalance = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionEntries_AccountPartitions_AccountPartitionId",
                        column: x => x.AccountPartitionId,
                        principalTable: "AccountPartitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionEntries_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionEntries_TransactionRequests_TransactionRequestId",
                        column: x => x.TransactionRequestId,
                        principalTable: "TransactionRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountPartitions_AccountId",
                table: "AccountPartitions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_MerchantCategoryCode",
                table: "Merchants",
                column: "MerchantCategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEntries_AccountPartitionId",
                table: "TransactionEntries",
                column: "AccountPartitionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEntries_MerchantId",
                table: "TransactionEntries",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEntries_TransactionRequestId",
                table: "TransactionEntries",
                column: "TransactionRequestId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionEntries");

            migrationBuilder.DropTable(
                name: "AccountPartitions");

            migrationBuilder.DropTable(
                name: "Merchants");

            migrationBuilder.DropTable(
                name: "TransactionRequests");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "MerchantCategories");
        }
    }
}