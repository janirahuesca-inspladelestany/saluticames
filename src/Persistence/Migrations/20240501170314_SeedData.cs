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
            migrationBuilder.DeleteData(
                table: "Catalogues",
                keyColumn: "Id",
                keyValue: new Guid("1e54e5d1-2e7f-472f-a6f3-9b7d59128be8"));

            migrationBuilder.InsertData(
                table: "Catalogues",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("eba4215e-87b6-4fcf-91e4-e90f16c97736"), "Repte dels 100 Cims de la FEEC" });

            migrationBuilder.InsertData(
                table: "EnumLookup<Region>",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[] { 3, "Ripollès", 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Catalogues",
                keyColumn: "Id",
                keyValue: new Guid("eba4215e-87b6-4fcf-91e4-e90f16c97736"));

            migrationBuilder.DeleteData(
                table: "EnumLookup<Region>",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.InsertData(
                table: "Catalogues",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("1e54e5d1-2e7f-472f-a6f3-9b7d59128be8"), "Repte dels 100 Cims de la FEEC" });
        }
    }
}
