using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catalogues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnumLookup<Region>",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumLookup<Region>", x => x.Id);
                    table.UniqueConstraint("AK_EnumLookup<Region>_Value", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "Hikers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hikers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Summits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Altitude = table.Column<int>(type: "int", nullable: false),
                    Region = table.Column<int>(type: "int", nullable: false),
                    CatalogueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Summits_Catalogues_CatalogueId",
                        column: x => x.CatalogueId,
                        principalTable: "Catalogues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Summits_EnumLookup<Region>_Region",
                        column: x => x.Region,
                        principalTable: "EnumLookup<Region>",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Diaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HikerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diaries_Hikers_HikerId",
                        column: x => x.HikerId,
                        principalTable: "Hikers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Climbs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HikerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SummitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AscensionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Climbs", x => x.Id);
                    table.UniqueConstraint("AK_Climbs_HikerId_SummitId", x => new { x.HikerId, x.SummitId });
                    table.ForeignKey(
                        name: "FK_Climbs_Diaries_DiaryId",
                        column: x => x.DiaryId,
                        principalTable: "Diaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Catalogues",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("972008bb-bbca-4bea-bfb2-2d4b2f9ef31b"), "Repte dels 100 Cims de la FEEC" });

            migrationBuilder.InsertData(
                table: "EnumLookup<Region>",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { -1, "NONE", -1 },
                    { 1, "Pla de l'Estany", 1 },
                    { 2, "Garrotxa", 2 },
                    { 3, "Ripollès", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Climbs_DiaryId",
                table: "Climbs",
                column: "DiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Diaries_HikerId",
                table: "Diaries",
                column: "HikerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Summits_CatalogueId",
                table: "Summits",
                column: "CatalogueId");

            migrationBuilder.CreateIndex(
                name: "IX_Summits_Name",
                table: "Summits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Summits_Region",
                table: "Summits",
                column: "Region");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Climbs");

            migrationBuilder.DropTable(
                name: "Summits");

            migrationBuilder.DropTable(
                name: "Diaries");

            migrationBuilder.DropTable(
                name: "Catalogues");

            migrationBuilder.DropTable(
                name: "EnumLookup<Region>");

            migrationBuilder.DropTable(
                name: "Hikers");
        }
    }
}
