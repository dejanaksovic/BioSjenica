using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bioSjenica.Migrations
{
    /// <inheritdoc />
    public partial class manytomanyrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Animals");

            migrationBuilder.CreateTable(
                name: "AnimalFeedingGround",
                columns: table => new
                {
                    AnimalsId = table.Column<int>(type: "int", nullable: false),
                    FeedingGroundsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalFeedingGround", x => new { x.AnimalsId, x.FeedingGroundsId });
                    table.ForeignKey(
                        name: "FK_AnimalFeedingGround_Animals_AnimalsId",
                        column: x => x.AnimalsId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalFeedingGround_FeedingGorunds_FeedingGroundsId",
                        column: x => x.FeedingGroundsId,
                        principalTable: "FeedingGorunds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalRegion",
                columns: table => new
                {
                    AnimalsId = table.Column<int>(type: "int", nullable: false),
                    RegionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalRegion", x => new { x.AnimalsId, x.RegionsId });
                    table.ForeignKey(
                        name: "FK_AnimalRegion_Animals_AnimalsId",
                        column: x => x.AnimalsId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalRegion_Regions_RegionsId",
                        column: x => x.RegionsId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalFeedingGround_FeedingGroundsId",
                table: "AnimalFeedingGround",
                column: "FeedingGroundsId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalRegion_RegionsId",
                table: "AnimalRegion",
                column: "RegionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalFeedingGround");

            migrationBuilder.DropTable(
                name: "AnimalRegion");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Animals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
