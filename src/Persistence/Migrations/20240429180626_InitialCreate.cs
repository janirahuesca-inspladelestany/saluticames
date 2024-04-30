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
                name: "EnumLookup<DifficultyLevel>",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumLookup<DifficultyLevel>", x => x.Id);
                    table.UniqueConstraint("AK_EnumLookup<DifficultyLevel>_Value", x => x.Value);
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
                name: "Summits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Altitude = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Region = table.Column<int>(type: "int", nullable: false),
                    DifficultyLevel = table.Column<int>(type: "int", nullable: false),
                    CatalogueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Summits_Catalogues_CatalogueId",
                        column: x => x.CatalogueId,
                        principalTable: "Catalogues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Summits_EnumLookup<DifficultyLevel>_DifficultyLevel",
                        column: x => x.DifficultyLevel,
                        principalTable: "EnumLookup<DifficultyLevel>",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Summits_EnumLookup<Region>_Region",
                        column: x => x.Region,
                        principalTable: "EnumLookup<Region>",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EnumLookup<DifficultyLevel>",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { -1, "NONE", -1 },
                    { 1, "EASY", 1 },
                    { 2, "MODERATE", 2 },
                    { 3, "DIFFICULT", 3 }
                });

            migrationBuilder.InsertData(
                table: "EnumLookup<Region>",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { -1, "NONE", -1 },
                    { 1, "Pla de l'Estany", 1 },
                    { 2, "Garrotxa", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Summits_CatalogueId",
                table: "Summits",
                column: "CatalogueId");

            migrationBuilder.CreateIndex(
                name: "IX_Summits_DifficultyLevel",
                table: "Summits",
                column: "DifficultyLevel");

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
                name: "Summits");

            migrationBuilder.DropTable(
                name: "Catalogues");

            migrationBuilder.DropTable(
                name: "EnumLookup<DifficultyLevel>");

            migrationBuilder.DropTable(
                name: "EnumLookup<Region>");
        }
    }
}
