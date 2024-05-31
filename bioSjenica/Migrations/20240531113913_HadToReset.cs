using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bioSjenica.Migrations
{
    /// <inheritdoc />
    public partial class HadToReset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RingNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LatinicName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommonName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LatinicName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommonName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SpecialDecision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Area = table.Column<float>(type: "real", nullable: false),
                    Villages = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProtectionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    SSN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FistName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayGrade = table.Column<float>(type: "real", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.SSN);
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

            migrationBuilder.CreateTable(
                name: "FeedingGorunds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroundNumber = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    StartWork = table.Column<int>(type: "int", nullable: false),
                    EndWork = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedingGorunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedingGorunds_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantRegion",
                columns: table => new
                {
                    PlantsId = table.Column<int>(type: "int", nullable: false),
                    RegionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantRegion", x => new { x.PlantsId, x.RegionsId });
                    table.ForeignKey(
                        name: "FK_PlantRegion_Plants_PlantsId",
                        column: x => x.PlantsId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantRegion_Regions_RegionsId",
                        column: x => x.RegionsId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AnimalFeedingGround_FeedingGroundsId",
                table: "AnimalFeedingGround",
                column: "FeedingGroundsId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalRegion_RegionsId",
                table: "AnimalRegion",
                column: "RegionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_RingNumber_LatinicName_CommonName",
                table: "Animals",
                columns: new[] { "RingNumber", "LatinicName", "CommonName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedingGorunds_GroundNumber",
                table: "FeedingGorunds",
                column: "GroundNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedingGorunds_RegionId",
                table: "FeedingGorunds",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantRegion_RegionsId",
                table: "PlantRegion",
                column: "RegionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_LatinicName_CommonName_ImageUrl",
                table: "Plants",
                columns: new[] { "LatinicName", "CommonName", "ImageUrl" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Name",
                table: "Regions",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalFeedingGround");

            migrationBuilder.DropTable(
                name: "AnimalRegion");

            migrationBuilder.DropTable(
                name: "PlantRegion");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "FeedingGorunds");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
