using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bioSjenica.Migrations
{
    /// <inheritdoc />
    public partial class onetomanyregionfeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "FeedingGorunds");

            migrationBuilder.RenameColumn(
                name: "Info",
                table: "Animals",
                newName: "Region");

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "FeedingGorunds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FeedingGorunds_RegionId",
                table: "FeedingGorunds",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedingGorunds_Regions_RegionId",
                table: "FeedingGorunds",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedingGorunds_Regions_RegionId",
                table: "FeedingGorunds");

            migrationBuilder.DropIndex(
                name: "IX_FeedingGorunds_RegionId",
                table: "FeedingGorunds");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "FeedingGorunds");

            migrationBuilder.RenameColumn(
                name: "Region",
                table: "Animals",
                newName: "Info");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "FeedingGorunds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
