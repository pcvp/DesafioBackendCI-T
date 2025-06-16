using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsCancelledFromSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sales_IsCancelled",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_IsCancelled_SaleDate",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ProductId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TotalSaleAmount",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "SaleItems");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Sales",
                newName: "Status");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "SaleItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Sales_Status",
                table: "Sales",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_Status_SaleDate",
                table: "Sales",
                columns: new[] { "Status", "SaleDate" });

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_ProductId",
                table: "SaleItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItems_Products_ProductId",
                table: "SaleItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Branches_BranchId",
                table: "Sales",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Customers_CustomerId",
                table: "Sales",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleItems_Products_ProductId",
                table: "SaleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Branches_BranchId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Customers_CustomerId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_Status",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_Status_SaleDate",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_SaleItems_ProductId",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SaleItems");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Sales",
                newName: "Quantity");

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Sales",
                type: "numeric(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Sales",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Sales",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSaleAmount",
                table: "Sales",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "Sales",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "SaleItems",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_IsCancelled",
                table: "Sales",
                column: "IsCancelled");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_IsCancelled_SaleDate",
                table: "Sales",
                columns: new[] { "IsCancelled", "SaleDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ProductId",
                table: "Sales",
                column: "ProductId");
        }
    }
}
