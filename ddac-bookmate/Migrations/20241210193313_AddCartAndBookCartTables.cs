using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ddac_bookmate.Migrations
{
    /// <inheritdoc />
    public partial class AddCartAndBookCartTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookWishlist_Books_BookId",
                table: "BookWishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_BookWishlist_Wishlist_WishlistId",
                table: "BookWishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_AspNetUsers_UserId",
                table: "Wishlist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlist",
                table: "Wishlist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookWishlist",
                table: "BookWishlist");

            migrationBuilder.DropIndex(
                name: "IX_BookWishlist_BookId",
                table: "BookWishlist");

            migrationBuilder.RenameTable(
                name: "Wishlist",
                newName: "Wishlists");

            migrationBuilder.RenameTable(
                name: "BookWishlist",
                newName: "BookWishlists");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlist_UserId",
                table: "Wishlists",
                newName: "IX_Wishlists_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookWishlist_WishlistId",
                table: "BookWishlists",
                newName: "IX_BookWishlists_WishlistId");

            migrationBuilder.AddColumn<bool>(
                name: "IsTrending",
                table: "Books",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StarRating",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedDate",
                table: "Books",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "BookWishlistId",
                table: "BookWishlists",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists",
                column: "WishlistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookWishlists",
                table: "BookWishlists",
                columns: new[] { "BookId", "WishlistId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookWishlists_Books_BookId",
                table: "BookWishlists",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookWishlists_Wishlists_WishlistId",
                table: "BookWishlists",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "WishlistId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_AspNetUsers_UserId",
                table: "Wishlists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookWishlists_Books_BookId",
                table: "BookWishlists");

            migrationBuilder.DropForeignKey(
                name: "FK_BookWishlists_Wishlists_WishlistId",
                table: "BookWishlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_AspNetUsers_UserId",
                table: "Wishlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookWishlists",
                table: "BookWishlists");

            migrationBuilder.DropColumn(
                name: "IsTrending",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "StarRating",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UploadedDate",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "Wishlists",
                newName: "Wishlist");

            migrationBuilder.RenameTable(
                name: "BookWishlists",
                newName: "BookWishlist");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlist",
                newName: "IX_Wishlist_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookWishlists_WishlistId",
                table: "BookWishlist",
                newName: "IX_BookWishlist_WishlistId");

            migrationBuilder.AlterColumn<int>(
                name: "BookWishlistId",
                table: "BookWishlist",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlist",
                table: "Wishlist",
                column: "WishlistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookWishlist",
                table: "BookWishlist",
                column: "BookWishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_BookWishlist_BookId",
                table: "BookWishlist",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookWishlist_Books_BookId",
                table: "BookWishlist",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookWishlist_Wishlist_WishlistId",
                table: "BookWishlist",
                column: "WishlistId",
                principalTable: "Wishlist",
                principalColumn: "WishlistId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_AspNetUsers_UserId",
                table: "Wishlist",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
