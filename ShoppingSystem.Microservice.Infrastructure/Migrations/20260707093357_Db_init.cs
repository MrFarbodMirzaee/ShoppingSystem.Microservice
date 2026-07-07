using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Db_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Shopping");

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "Shopping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Primary key."),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Represents the street portion of an address."),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Represents the city portion of an address."),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Represents the state or province portion of an address."),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Represents the country portion of an address."),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Represents the postal code of an address."),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()", comment: "Automatically assigned upon creation."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Used by soft delete functionality.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                },
                comment: "Entity containing address details required for order fulfillment and customer information.");

            migrationBuilder.CreateTable(
                name: "Cart",
                schema: "Shopping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Primary key."),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "References the customer related to the entity."),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()", comment: "Automatically assigned upon creation."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Used by soft delete functionality.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Id);
                },
                comment: "Aggregate Root responsible for managing cart items and maintaining cart consistency.");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "Shopping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Human-readable name."),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Detailed textual information about the entity."),
                    ParentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "References the parent category in a hierarchical structure."),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, comment: "Determines if the entity is currently active and available for use."),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()", comment: "Automatically assigned upon creation."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Used by soft delete functionality.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalSchema: "Shopping",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Aggregate Root responsible for managing product classification and category information.");

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "Shopping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Primary key."),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "References the user related to the entity."),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Represents the human-readable order number."),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Represents the numeric value of a monetary amount."),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, comment: "Represents the ISO currency code associated with the monetary value."),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Represents the lifecycle state."),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()", comment: "Automatically assigned upon creation."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Used by soft delete functionality.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                },
                comment: "Aggregate Root responsible for order creation, order status transitions, and order item management.");

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "Shopping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Primary key."),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Human-readable name."),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Detailed textual information about the entity."),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Represents the numeric value of a monetary amount."),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, comment: "Represents the ISO currency code associated with the monetary value."),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "References the related category."),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, comment: "Determines if the entity is currently available for use or purchase."),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()", comment: "Automatically assigned upon creation."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Used by soft delete functionality.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Shopping",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Aggregate Root responsible for managing product details, pricing, and product-related data.");

            migrationBuilder.CreateTable(
                name: "OrderItem",
                schema: "Shopping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Primary key."),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "References the related product."),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Represents the display name of the product."),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Represents the numeric value of a monetary amount."),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, comment: "Represents the ISO currency code associated with the monetary value."),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Represents the number of units."),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "References the related order."),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()", comment: "Automatically assigned upon creation."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Used by soft delete functionality.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Shopping",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Entity owned by Order aggregate that stores purchased product information and price snapshots.");

            migrationBuilder.CreateTable(
                name: "Payment",
                schema: "Shopping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Primary key."),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "References the related order."),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Represents the numeric value of a monetary amount."),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, comment: "Represents the ISO currency code associated with the monetary value."),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "References the external or internal payment transaction."),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Represents the lifecycle state."),
                    PaidAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, comment: "Records when the payment was successfully processed."),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()", comment: "Automatically assigned upon creation."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Used by soft delete functionality.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Payment_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Shopping",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Aggregate Root responsible for handling payment information and transaction states.");

            migrationBuilder.CreateTable(
                name: "CartItem",
                schema: "Shopping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Primary key."),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "References the related product."),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Represents the display name of the product."),
                    Quantity = table.Column<byte>(type: "tinyint", nullable: false, comment: "Represents the number of units."),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Represents the price per individual unit."),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "References the related shopping cart."),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()", comment: "Automatically assigned upon creation."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Used by soft delete functionality.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_CartItem_Cart_CartId",
                        column: x => x.CartId,
                        principalSchema: "Shopping",
                        principalTable: "Cart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Shopping",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Entity owned by Cart aggregate that stores product reference and quantity information.");

            migrationBuilder.CreateTable(
                name: "Inventory",
                schema: "Shopping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Primary key."),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "References the related product."),
                    Quantity = table.Column<byte>(type: "tinyint", nullable: false, comment: "Represents the number of units."),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Represents the lifecycle state."),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()", comment: "Automatically assigned upon creation."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Used by soft delete functionality.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Inventory_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Shopping",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Aggregate Root responsible for stock availability, reservation, and inventory updates.");

            migrationBuilder.CreateTable(
                name: "ProductImage",
                schema: "Shopping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Primary key."),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "References the related product."),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Stores the original or assigned file name."),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Specifies the storage location of the file."),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Represents the MIME type of the stored file."),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false, comment: "Represents the total size of the stored file."),
                    IsMain = table.Column<bool>(type: "bit", nullable: false, comment: "Specifies if the record is marked as the primary item."),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()", comment: "Automatically assigned upon creation."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Used by soft delete functionality.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ProductImage_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Shopping",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Entity responsible for storing product image references and related metadata.");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId_ProductId",
                schema: "Shopping",
                table: "CartItem",
                columns: new[] { "CartId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ProductId",
                schema: "Shopping",
                table: "CartItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                schema: "Shopping",
                table: "Category",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryId",
                schema: "Shopping",
                table: "Category",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductId",
                schema: "Shopping",
                table: "Inventory",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderNumber",
                schema: "Shopping",
                table: "Order",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                schema: "Shopping",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                schema: "Shopping",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderId",
                schema: "Shopping",
                table: "Payment",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                schema: "Shopping",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Name",
                schema: "Shopping",
                table: "Product",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                schema: "Shopping",
                table: "ProductImage",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId_IsMain",
                schema: "Shopping",
                table: "ProductImage",
                columns: new[] { "ProductId", "IsMain" },
                unique: true,
                filter: "[IsMain] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address",
                schema: "Shopping");

            migrationBuilder.DropTable(
                name: "CartItem",
                schema: "Shopping");

            migrationBuilder.DropTable(
                name: "Inventory",
                schema: "Shopping");

            migrationBuilder.DropTable(
                name: "OrderItem",
                schema: "Shopping");

            migrationBuilder.DropTable(
                name: "Payment",
                schema: "Shopping");

            migrationBuilder.DropTable(
                name: "ProductImage",
                schema: "Shopping");

            migrationBuilder.DropTable(
                name: "Cart",
                schema: "Shopping");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "Shopping");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "Shopping");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "Shopping");
        }
    }
}
