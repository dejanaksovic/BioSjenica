using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bioSjenica.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Plants_LatinicName_CommonName_ImageUrl",
                table: "Plants");

            migrationBuilder.DropIndex(
                name: "IX_Animals_RingNumber_LatinicName_CommonName",
                table: "Animals");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plants_CommonName",
                table: "Plants",
                column: "CommonName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plants_ImageUrl",
                table: "Plants",
                column: "ImageUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plants_LatinicName",
                table: "Plants",
                column: "LatinicName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_CommonName",
                table: "Animals",
                column: "CommonName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_LatinicName",
                table: "Animals",
                column: "LatinicName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_RingNumber",
                table: "Animals",
                column: "RingNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Plants_CommonName",
                table: "Plants");

            migrationBuilder.DropIndex(
                name: "IX_Plants_ImageUrl",
                table: "Plants");

            migrationBuilder.DropIndex(
                name: "IX_Plants_LatinicName",
                table: "Plants");

            migrationBuilder.DropIndex(
                name: "IX_Animals_CommonName",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_Animals_LatinicName",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_Animals_RingNumber",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_LatinicName_CommonName_ImageUrl",
                table: "Plants",
                columns: new[] { "LatinicName", "CommonName", "ImageUrl" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_RingNumber_LatinicName_CommonName",
                table: "Animals",
                columns: new[] { "RingNumber", "LatinicName", "CommonName" },
                unique: true);
        }
    }
}
