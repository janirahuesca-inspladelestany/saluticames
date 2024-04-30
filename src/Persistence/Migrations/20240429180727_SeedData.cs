using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Catalogues",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("21bb85b0-811f-42bc-979a-a45b976685dd"), "Repte dels 100 Cims de la FEEC" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Catalogues",
                keyColumn: "Id",
                keyValue: new Guid("21bb85b0-811f-42bc-979a-a45b976685dd"));
        }
    }
}
