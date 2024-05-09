﻿using System;
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
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Altitude = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    IsEssential = table.Column<bool>(type: "bit", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    DifficultyLevelId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_Summits_EnumLookup<DifficultyLevel>_DifficultyLevelId",
                        column: x => x.DifficultyLevelId,
                        principalTable: "EnumLookup<DifficultyLevel>",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Summits_EnumLookup<Region>_RegionId",
                        column: x => x.RegionId,
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
                    CatalogueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HikerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diaries_Catalogues_CatalogueId",
                        column: x => x.CatalogueId,
                        principalTable: "Catalogues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Diaries_Hikers_HikerId",
                        column: x => x.HikerId,
                        principalTable: "Hikers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Climbs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SummitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AscensionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Climbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Climbs_Diaries_DiaryId",
                        column: x => x.DiaryId,
                        principalTable: "Diaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Climbs_Summits_SummitId",
                        column: x => x.SummitId,
                        principalTable: "Summits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Catalogues",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), "Repte dels 100 Cims de la FEEC" });

            migrationBuilder.InsertData(
                table: "EnumLookup<DifficultyLevel>",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { -1, "N/A", -1 },
                    { 1, "Fàcil", 1 },
                    { 2, "Moderat", 2 },
                    { 3, "Difícil", 3 }
                });

            migrationBuilder.InsertData(
                table: "EnumLookup<Region>",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { -1, "N/A", -1 },
                    { 1, "Alt Camp", 1 },
                    { 2, "Alt Empordà", 2 },
                    { 3, "Alt Penedès", 3 },
                    { 4, "Alt Urgell", 4 },
                    { 5, "Alta Ribagorça", 5 },
                    { 6, "Anoia", 6 },
                    { 7, "Bages", 7 },
                    { 8, "Baix Camp", 8 },
                    { 9, "Baix Ebre", 9 },
                    { 10, "Baix Empordà", 10 },
                    { 11, "Baix Llobregat", 11 },
                    { 12, "Baix Penedès", 12 },
                    { 13, "Barcelonès", 13 },
                    { 14, "Berguedà", 14 },
                    { 15, "Cerdanya", 15 },
                    { 16, "Conca de Barberà", 16 },
                    { 17, "Garraf", 17 },
                    { 18, "Garrigues", 18 },
                    { 19, "Garrotxa", 19 },
                    { 20, "Gironès", 20 },
                    { 21, "Maresme", 21 },
                    { 22, "Montsià", 22 },
                    { 23, "Noguera", 23 },
                    { 24, "Osona", 24 },
                    { 25, "Pallars Jussà", 25 },
                    { 26, "Pallars Sobirà", 26 },
                    { 27, "Pla d'Urgell", 27 },
                    { 28, "Pla de l'Estany", 28 },
                    { 29, "Priorat", 29 },
                    { 30, "Ribera d'Ebre", 30 },
                    { 31, "Ripollès", 31 },
                    { 32, "Segarra", 32 },
                    { 33, "Segrià", 33 },
                    { 34, "Selva", 34 },
                    { 35, "Solsonès", 35 },
                    { 36, "Tarragonès", 36 },
                    { 37, "Terra Alta", 37 },
                    { 38, "Urgell", 38 },
                    { 39, "Val d'Aran", 39 },
                    { 40, "Vallès Occidental", 40 },
                    { 41, "Vallès Oriental", 41 },
                    { 42, "Moianès", 42 },
                    { 43, "Lluçanès", 43 },
                    { 100, "Andorra", 100 },
                    { 200, "Capcir", 200 },
                    { 201, "Cerdanya Nord", 201 },
                    { 202, "Rosselló", 202 },
                    { 203, "Conflent", 203 },
                    { 204, "Vallespir", 204 },
                    { 205, "Fenolledès", 205 }
                });

            migrationBuilder.InsertData(
                table: "Hikers",
                columns: new[] { "Id", "Name", "Surname" },
                values: new object[] { "12345678P", "Kilian", "Gordet" });

            migrationBuilder.InsertData(
                table: "Diaries",
                columns: new[] { "Id", "CatalogueId", "HikerId", "Name" },
                values: new object[] { new Guid("4bd06887-b4d7-49b7-a18b-0100b2dfc3fa"), new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), "12345678P", "El meu diari dels 100 cims de la FEEC" });

            migrationBuilder.InsertData(
                table: "Summits",
                columns: new[] { "Id", "Altitude", "CatalogueId", "DifficultyLevelId", "IsEssential", "Latitude", "Longitude", "Name", "RegionId" },
                values: new object[,]
                {
                    { new Guid("00753579-504d-4808-895c-d82e344e997b"), 2921, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.56986f, 1.932099f, "Carlit", 201 },
                    { new Guid("00948c5b-ffcd-4532-b7bd-6eb7436e5461"), 287, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.60888f, 2.573683f, "Pedracastell o Creu de Canet", 21 },
                    { new Guid("00ade412-c791-470a-a463-53eb79e4c4ec"), 2874, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.52514f, 1.651239f, "Alt del Griu", 100 },
                    { new Guid("00b5ece7-267c-47a5-8822-be4edcf72e7f"), 492, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.1399f, 0.602189f, "Pic de l'Àguila", 30 },
                    { new Guid("00f3b59d-861b-4e8e-bad0-31c2542a73a2"), 1181, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.87937f, 0.360899f, "L'Espina", 9 },
                    { new Guid("01325578-2fd6-481d-bfd9-b85f80b33496"), 2862, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.50812f, 1.664673f, "Pic dels Pessons o Gargantillar", 100 },
                    { new Guid("01348901-5fbc-42a7-9dab-f6bd13a52161"), 2811, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.58779f, 1.741817f, "Roc Melé", 100 },
                    { new Guid("01951b92-a0be-490c-aaff-14d7482bc18d"), 922, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.10746f, 0.873961f, "Mola de Colldejou", 8 },
                    { new Guid("02c94f25-b0e8-458f-97c5-e2177d1123bd"), 1348, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.56532f, 2.561282f, "Santa Anna dels Quatre Termes", 203 },
                    { new Guid("02efb02b-f3df-4ee1-ab8e-696e8f3c9909"), 264, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.64222f, 2.670392f, "Montpalau", 21 },
                    { new Guid("036d92c2-7ade-4f5e-beca-2a6c585e8d3b"), 842, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.61121f, 1.763283f, "Torre Alta (Castell Ferran)", 6 },
                    { new Guid("03d105bf-db3e-44a3-ae52-7e7fafe4aba1"), 2950, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.55495f, 0.979381f, "Pic de Subenuix", 25 },
                    { new Guid("03e0254c-5330-484d-b90d-99015930fe92"), 2699, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.62239f, 0.866017f, "Tuc des Monges", 5 },
                    { new Guid("04122ef7-a059-45bd-9177-8faf60117554"), 303, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.4703f, 2.206586f, "Puig Castellar", 13 },
                    { new Guid("04738947-55c6-49f3-97ce-a00fadc2162a"), 2878, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.65342f, 1.493444f, "Tristaina", 100 },
                    { new Guid("04e79888-f093-4c03-88b4-35239aeb7d1c"), 2321, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.18943f, 1.738038f, "Cap de la Gallina Pelada", 14 },
                    { new Guid("05047290-54ba-4f83-b784-30d97eb7fa6c"), 2885, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.6104f, 0.842595f, "Punta d'Harlé", 5 },
                    { new Guid("054c43bb-cfda-45e5-91a2-625160466126"), 175, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.11244f, 3.04993f, "Puig Segalar", 2 },
                    { new Guid("05f7fcfb-5b64-4502-ae1d-10b5714d9455"), 1211, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.20906f, 1.301153f, "Santa Fe", 4 },
                    { new Guid("06ee1f8d-e81e-4483-95e6-6e5e75e66e4d"), 2910, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.38327f, 2.116791f, "Puigmal", 201 },
                    { new Guid("075aec91-78df-4dc9-aa80-8c51482175fc"), 1163, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.28859f, 0.891232f, "Roca Corbatera", 29 },
                    { new Guid("081a81aa-f049-475d-aea0-cc06ba84f1a6"), 793, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.19788f, 2.446635f, "Sant Miquel del Mont", 19 },
                    { new Guid("08a5e34c-cdb3-49ee-9269-1c81db5359be"), 1045, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.84715f, 2.171544f, "Turó de Bellver", 42 },
                    { new Guid("08b3ff3d-6c96-4b6f-ba26-b9430e5cb2f9"), 2915, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.6087f, 1.592584f, "Pic de l'Estanyó", 100 },
                    { new Guid("09c8f3c9-5dbe-4071-868f-016573e05634"), 1630, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.19807f, 1.410964f, "Roc de Galliner", 4 },
                    { new Guid("0a2d28a2-0009-42dc-8ac9-f2bedd810092"), 2088, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.31628f, 1.711598f, "Roca Gran", 15 },
                    { new Guid("0ad9b3ff-e325-4963-bf90-2811bdf83d55"), 1448, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.09311f, 1.624209f, "la Guàrdia (Serra de Busa)", 35 },
                    { new Guid("0b7a0f2f-3bbf-4ba0-abdf-d763112b3333"), 2663, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.72005f, 1.216644f, "Pic de Mariola", 26 },
                    { new Guid("0b8ec28c-468d-4e25-957f-5cee7ccf55eb"), 484, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.50468f, 2.266641f, "Turó d'en Galzeran o d'en Mates", 21 },
                    { new Guid("0c45d0d1-72f1-49f1-b56c-1a6268f1b2e7"), 1025, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.00172f, 0.672191f, "Puig de Millà", 23 },
                    { new Guid("0caf5823-2cef-4439-8834-cd097a8478f9"), 2890, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.6031f, 1.008688f, "Montsaliente", 26 },
                    { new Guid("0cf52172-70e5-4ff4-9a3f-109d11c0128f"), 2593, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.63799f, 1.307491f, "Montarenyo de Boldís", 26 },
                    { new Guid("0e46887e-a42f-44d2-9730-13dcaef0875f"), 2645, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.71263f, 0.991789f, "Tuc de Vacivèr", 39 },
                    { new Guid("0e95a512-1bda-4fcc-94c6-4d5214adb6aa"), 881, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.42904f, 1.305758f, "El Cogulló de Cabra", 1 },
                    { new Guid("0f15877a-01e1-4086-b691-12fae177f791"), 1820, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.28856f, 1.798617f, "Cap de la Boixassa", 14 },
                    { new Guid("0fee6c73-4df1-495f-8ea2-b2ef79838b62"), 2933, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.58939f, 0.937807f, "Gran Tuc de Colomers", 5 },
                    { new Guid("1029d7d4-a72c-493f-a935-fdd323d1dc36"), 2861, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.60122f, 0.953482f, "Tuc de Ratera", 26 },
                    { new Guid("108bbe30-ae1a-46ad-a6e4-073888989862"), 594, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.29653f, 1.915486f, "La Morella", 11 },
                    { new Guid("1180e6ab-8bf1-402a-b486-e2b590089727"), 1344, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.36457f, 2.542173f, "Torres de Cabrenç", 204 },
                    { new Guid("11ac9921-4549-4d0a-96ae-98d89f6a47eb"), 614, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.05344f, 0.89505f, "Puig de la Cabrafiga", 8 },
                    { new Guid("12736894-9c04-4a6e-9049-4eaf8e5d6475"), 2469, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.65182f, 2.19241f, "Roc de Madres", 203 },
                    { new Guid("12afa194-e971-45e1-873b-5af1c6b64db0"), 1480, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.30723f, 0.974246f, "Roc de Sant Aventí", 25 },
                    { new Guid("1370a695-ecd1-40d5-9505-98018f98e22e"), 2282, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.18812f, 1.777023f, "Serrat Voltor", 14 },
                    { new Guid("13f5d565-ef4f-4e37-bb79-cd7d452cf15a"), 500, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.99244f, 2.715114f, "Sant Grau", 20 },
                    { new Guid("13fd35b2-9eb9-4d22-97b1-2c9dd257e2ac"), 895, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.05458f, 2.040229f, "Castell de Lluçà", 43 },
                    { new Guid("141f495e-d520-4027-956a-52fde724c25b"), 3144, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.66694f, 1.397901f, "Pica d'Estats", 26 },
                    { new Guid("14d9f23c-dc6d-4ca2-acdc-cd0a2c84c6ea"), 2851, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.69065f, 1.190422f, "Pic de Ventolau", 26 },
                    { new Guid("14fc8081-3087-4fe4-89c4-e77917778856"), 406, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.57723f, 0.482468f, "la Cogula", 22 },
                    { new Guid("14fcf911-51dd-45c0-92e9-406720f93fe4"), 2506, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.23994f, 1.702926f, "Pollegó Superior (Pedraforca)", 14 },
                    { new Guid("150d8b48-a43c-47bf-817e-7ca97da738bb"), 2903, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.64972f, 1.534401f, "Pic de Font Blanca", 100 },
                    { new Guid("163ec71f-784c-4218-b033-2cd1ec3dcb86"), 2445, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.23563f, 1.707024f, "Pollegó Inferior (Pedraforca)", 14 },
                    { new Guid("168502b2-ce1c-4785-87de-e87cba985a71"), 699, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.01611f, 0.658726f, "Torre de les Conclues", 23 },
                    { new Guid("16b31c1c-181d-4481-9c4c-1e8e4b7063c9"), 2493, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.42456f, 1.600995f, "Turó Punçó", 4 },
                    { new Guid("17264261-b89c-47fb-ae71-7a99d65ba4da"), 772, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.96349f, 0.58437f, "Les Picòssies", 9 },
                    { new Guid("17677b5c-a489-4fa8-ba9f-aa51d5d2ed76"), 2465, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.4169f, 2.34392f, "Costabona", 31 },
                    { new Guid("17dacdfd-428b-460e-9ef0-4ccaee105ab3"), 854, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.09126f, 1.309484f, "Roc de les Tres Creus", 4 },
                    { new Guid("17faefb2-188d-4765-b481-aacb5f7f15ca"), 866, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.5167f, 1.492183f, "Grony de Miralles", 6 },
                    { new Guid("186a8c55-f551-42e7-8697-917a2e10e836"), 2749, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.56841f, 1.014838f, "Gran Encantat", 26 },
                    { new Guid("18cf2c35-7f9d-4f2b-be7b-3dbd5c736464"), 646, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.11208f, 2.711729f, "Sant Patllari", 28 },
                    { new Guid("1924f7af-7e51-4629-be92-0fb90260fe9f"), 2750, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.45087f, 2.131261f, "Cambra d'Ase", 201 },
                    { new Guid("19edbd0e-b3e5-4030-91f6-dee636db8900"), 2903, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.625f, 1.420414f, "Pica Roja", 26 },
                    { new Guid("1a217cb4-b571-4499-910f-6db6a200d789"), 656, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.49043f, 3.075117f, "Torre de Madeloc", 202 },
                    { new Guid("1a364983-b1c8-44c8-bb61-8d19160dc37e"), 658, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.34586f, 1.885001f, "Montau", 3 },
                    { new Guid("1a859018-d94a-42a1-b6f8-4cf11f4aa69b"), 766, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.06833f, 0.803219f, "Montalt (punta nord)", 29 },
                    { new Guid("1acbd6ce-282a-48c2-9c51-14651be01ad7"), 368, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.71127f, 1.089982f, "Tossal d'Espígol", 38 },
                    { new Guid("1b5cca46-6c02-4fe9-804b-0080003b1f91"), 951, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.92584f, 0.641573f, "Los Picons", 23 },
                    { new Guid("1ba3367d-881e-487e-a7b0-a3c4360e6fcc"), 840, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.08405f, 0.875157f, "Cavall Bernat de Llaberia", 8 },
                    { new Guid("1bb8099c-bd28-4dce-ae17-4441fc0a925b"), 1778, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.51468f, 2.553556f, "Puig de l'Estella", 203 },
                    { new Guid("1c683c0b-e5dd-4bea-81af-a5af49e105f6"), 2409, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.30048f, 1.935535f, "Puigllançada", 14 },
                    { new Guid("1d2f75d9-eed0-4f44-8b5c-80d89c4227f3"), 2983, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.54054f, 1.011958f, "Pic de Peguera", 25 },
                    { new Guid("1de26af7-f512-40bc-a995-c51f00ca0c16"), 2543, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.46212f, 0.828503f, "Lo Corronco", 5 },
                    { new Guid("1e03087d-1bf4-4034-a77c-7c5e55c9fe99"), 2882, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.79375f, 0.917142f, "Maubèrme", 39 },
                    { new Guid("1e589668-834d-4b5b-90bd-b930112ac8f6"), 1015, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.49202f, 2.890699f, "Puig de Sant Cristau", 202 },
                    { new Guid("1fa96340-9815-4c7d-995b-074318004012"), 2870, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.42393f, 2.213732f, "Pic de l'Infern", 203 },
                    { new Guid("1ff192d5-8925-4066-934e-c0a040e1839b"), 1488, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.13767f, 2.350877f, "Puig Cubell", 19 },
                    { new Guid("2169f9a4-f731-4315-8609-44e4ec63ba9e"), 1097, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.37529f, 2.667094f, "Puig Falcó", 2 },
                    { new Guid("219b779e-696a-48c2-927a-bb61295763c6"), 409, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.24371f, 1.474335f, "Puig de Sant Antoni", 12 },
                    { new Guid("21f139c8-9e2e-4cf3-8223-642a5c411ce1"), 808, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.70267f, 2.241187f, "Puiggraciós", 41 },
                    { new Guid("225e7c9c-384e-4a27-a579-6a2639b4528c"), 1687, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.18472f, 1.078088f, "Gallinova", 25 },
                    { new Guid("22d83c89-e1cc-4001-a62b-7e5d25e69dbc"), 392, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.53746f, 2.387339f, "Castell de Burriac", 21 },
                    { new Guid("24332cf7-8f57-4178-8dfc-8b3c89612e14"), 1702, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.48328f, 1.922184f, "Turó de Bell-lloc", 201 },
                    { new Guid("24bc2c12-9647-4f8f-a514-7c61f29ba1e5"), 465, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.83066f, 2.635981f, "L'Argimon", 34 },
                    { new Guid("251fdead-2366-4566-a0e2-a3b30f13d59c"), 574, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.86286f, 0.852997f, "Monteró", 23 },
                    { new Guid("258bd96f-1c55-45df-8506-3788d6ff4a16"), 1818, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.32375f, 1.624994f, "Tossal de Badés", 4 },
                    { new Guid("25d4e13a-ac25-4070-8e59-b650606c70e4"), 1057, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.07873f, 2.153281f, "Els Munts", 24 },
                    { new Guid("260ab610-f99c-41dd-97e4-a322d597bb30"), 2570, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.72431f, 1.325358f, "Pic de Colatx", 26 },
                    { new Guid("26b4b359-c3b1-4671-a389-dd1170bb1ac8"), 2291, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.59866f, 1.179417f, "Montcaubo (lo Calbo)", 26 },
                    { new Guid("26f5d1f3-14aa-4570-91a5-0ee01837aef9"), 2020, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.43535f, 1.338294f, "Roc Roi", 4 },
                    { new Guid("2775a152-e2e9-478c-b4c3-72a49bb5298c"), 1104, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.1466f, 0.797767f, "Montllobar", 25 },
                    { new Guid("28976631-9b96-4b70-b0a6-40dfc75d1c67"), 1529, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.16555f, 2.289601f, "Castell de Milany", 24 },
                    { new Guid("29d7bb0e-454b-4600-a335-f747cb0d8177"), 942, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.70277f, 2.122957f, "Sant Sadurní de Gallifa", 40 },
                    { new Guid("2a2bc2f5-ed4e-42a4-873b-2ff94360ac92"), 2881, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.42622f, 2.232862f, "Bastiments", 203 },
                    { new Guid("2a53dd84-ec8d-4920-979b-ec68b961d410"), 762, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.97656f, 1.128754f, "Montmagastre", 23 },
                    { new Guid("2a6fc132-dcfd-4d1d-bb02-673282caa25e"), 317, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.21166f, 1.4173f, "La Mola", 12 },
                    { new Guid("2aa559bd-a24e-4b1c-9204-00a5a2f93995"), 832, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.74094f, 2.059581f, "Còdol del Castellar", 42 },
                    { new Guid("2b18177c-649f-49ba-83fd-cb6093d12366"), 1070, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.90903f, 0.362741f, "Moleta de les Canals", 9 },
                    { new Guid("2bfeed4b-932b-4ee2-a581-4237d2bee12f"), 1514, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.12518f, 2.387892f, "Puigsacalm", 19 },
                    { new Guid("2c698342-736f-47a3-8a22-75e3d21ef322"), 2676, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.62614f, 0.89524f, "Tuc de Ribereta", 5 },
                    { new Guid("2c979687-1c13-4bd3-bb23-cfe760ae42f2"), 463, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.28051f, 3.204968f, "Puig de l'Àliga (Cap de Creus)", 2 },
                    { new Guid("2d9085db-8e41-45ad-9755-2791a5e14920"), 2573, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.56297f, 1.454193f, "Alt de la Capa", 100 },
                    { new Guid("2eb6515c-f25c-41ef-8f41-b1151d88c96e"), 755, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.67138f, 1.954693f, "Turó del Mal Pas", 7 },
                    { new Guid("2eee10fe-16cf-444c-bbee-02a4b0aa4c41"), 1653, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.16772f, 1.910706f, "Sobrepuny", 14 },
                    { new Guid("2f811efe-8b2d-4088-aa9d-e15ef1816817"), 289, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.61299f, 0.84312f, "La Fita Alta", 27 },
                    { new Guid("2faaf23f-65da-4208-898c-36caf7710b3b"), 2781, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.50893f, 1.686569f, "Montmalús", 100 },
                    { new Guid("2fec2d2f-2984-4a94-beb9-bf36fd958c15"), 1223, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.60441f, 1.806206f, "Miranda dels Ecos", 6 },
                    { new Guid("30ef3da9-520a-43e5-ba12-8bb794fb373e"), 1503, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.30524f, 1.301323f, "Sant Quiri", 4 },
                    { new Guid("312e5673-82c1-4666-acc9-3c197bb204ba"), 2549, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.49918f, 0.971881f, "Pic de l'Espada", 25 },
                    { new Guid("3160de8e-4dd5-4edc-a47c-91e6b2649d19"), 1327, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.30152f, 0.914596f, "el Codó", 25 },
                    { new Guid("318d1de5-f8c4-462a-9bf8-90807578e10a"), 2040, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.28077f, 2.209888f, "Taga", 31 },
                    { new Guid("31923123-f7a5-42ed-8ee8-c000fa0225b2"), 1718, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.58848f, 1.223107f, "Pui Tabaca", 26 },
                    { new Guid("31961b64-80f6-4333-ac58-0cb10c503437"), 2693, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.39632f, 2.247721f, "Les Borregues", 31 },
                    { new Guid("31a3ab32-1212-4a81-b3c6-32fd453c09a1"), 717, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.22026f, 1.094067f, "Puig d'en Cama", 8 },
                    { new Guid("32b9d24c-f722-40be-91f0-5f6e6826525c"), 1358, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.46785f, 1.985909f, "Castell de Llívia", 15 },
                    { new Guid("34cb141b-ea0e-4595-b636-55cc3c0a4565"), 2721, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.78778f, 1.076722f, "Pic de Clavera", 26 },
                    { new Guid("355c4e97-243f-40c5-a1ae-50a1d25a3463"), 1153, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.10453f, 1.362394f, "Roc del Migdia (la Valldan)", 35 },
                    { new Guid("357b63a4-49c5-44e9-96de-c2642f7e5582"), 551, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.01247f, 0.873881f, "Punta del Pallars", 8 },
                    { new Guid("35941634-2f13-4677-a1bf-e25148a81569"), 1525, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.08925f, 1.727427f, "Els Tossals", 14 },
                    { new Guid("3625987a-31ca-4ea1-9dd9-6778064f4789"), 885, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.06528f, 1.26543f, "Roc de Cogul", 4 },
                    { new Guid("3685a5f7-e5fe-4d05-bcc6-02e18be0fc26"), 273, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.47721f, 2.176612f, "Turó de Montcada", 40 },
                    { new Guid("36bdfe77-db56-4dfa-afc5-25a1b0721a4b"), 2753, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.80581f, 0.96045f, "Malh de Bolard", 39 },
                    { new Guid("36e36a6f-14b9-4b76-a7f5-ba04d7b729f1"), 2056, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.28572f, 2.080664f, "Costa Pubilla o Pla de Pujalts", 31 },
                    { new Guid("3750a616-bd08-4917-bbcb-3cce2f73ddb4"), 523, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.17904f, 0.643459f, "Lo Tormo", 30 },
                    { new Guid("37b474bd-28cc-4186-9c46-38fb28fdd20e"), 2485, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.64687f, 0.908438f, "Tuc de Salana", 39 },
                    { new Guid("391576ca-1cfa-4484-a156-8a916436e873"), 1913, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.34179f, 1.13303f, "Tossal del Puial", 26 },
                    { new Guid("3929ebd1-39c1-4330-a76d-19e9b6620086"), 2387, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.1848f, 1.52091f, "Pedró dels Quatre Batlles", 4 },
                    { new Guid("39517e2a-ad0e-485a-81fe-183ebaa824d2"), 1344, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.10902f, 2.349318f, "Puig de les Àligues", 24 },
                    { new Guid("3982fde9-845a-4c2f-8d73-7cc0f023e468"), 1757, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.20137f, 1.218321f, "l'Oratori", 4 },
                    { new Guid("39d16596-a9f9-4480-9743-9ce4ca17bf03"), 464, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.28387f, 1.697382f, "Puig de l'Àliga", 3 },
                    { new Guid("3a153acd-6083-4293-bcf7-a1ee73d962e6"), 1087, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.27343f, 2.69285f, "Tossa d'Espinau", 2 },
                    { new Guid("3a45f6b2-574e-4d54-9295-5434d8743198"), 1547, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.16899f, 2.340265f, "Santa Magdalena de Cambrils", 24 },
                    { new Guid("3a684d05-192b-4fb1-ade4-87e17cb05627"), 1132, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.58841f, 1.826477f, "Miranda de Santa Magdalena", 11 },
                    { new Guid("3a7c720b-d402-4a50-9e9a-a08b9343968b"), 1350, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.75873f, 0.289653f, "Catinell", 9 },
                    { new Guid("3af6a6c4-1cdc-4782-af68-a738cf19ff33"), 2679, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.70924f, 1.012332f, "Tuc de Marimanya", 26 },
                    { new Guid("3af6b127-6e57-431d-8d18-d3978be6af68"), 2853, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.71197f, 1.277718f, "Pic de Certascan", 26 },
                    { new Guid("3b30a4fb-2a92-4d8d-a0b2-2c611a914112"), 1178, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.60102f, 1.813379f, "L'Albarda Castellana", 11 },
                    { new Guid("3b677d9d-f2a1-450d-961d-fea95de3b6e1"), 1062, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.30563f, 0.842026f, "Punta dels Pins Carrassers", 29 },
                    { new Guid("3bb097e7-4058-41c7-9a82-13c7f6bf82f2"), 947, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.89629f, 0.836346f, "Pala Alta", 23 },
                    { new Guid("3bc99aa0-0a30-4629-b024-c6fed8a5837c"), 2604, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.28146f, 1.670418f, "Pic de Costa Cabirolera", 4 },
                    { new Guid("3cd35a89-d489-4731-8263-71fbc89cb07e"), 2256, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.42023f, 1.734439f, "Tossal Gros (Pelat de Talltendre)", 15 },
                    { new Guid("3d13b5bd-6fdb-47f2-8158-bff8e7f323d8"), 2518, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.78526f, 0.758715f, "Montlude", 39 },
                    { new Guid("3e22df65-a844-4f29-a0c1-1b4814edcc2c"), 2534, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.78125f, 0.876909f, "Tuc des Armèros", 39 },
                    { new Guid("3e71fed6-ed72-4803-8712-89464f6d9578"), 923, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.34143f, 0.878149f, "Punta del General", 18 },
                    { new Guid("3f95b872-bdb9-4b1f-b355-8cea0a9d697d"), 2833, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.54935f, 1.916784f, "Puig Occidental de Coll Roig", 201 },
                    { new Guid("3f95dfe9-23cb-409d-a012-b9a912434ddc"), 1705, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 41.78933f, 2.443955f, "Les Agudes", 34 },
                    { new Guid("411edf02-e679-4163-b262-d97d1f1c6aa4"), 595, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.62026f, 1.815019f, "Turó de l'Ermità", 7 },
                    { new Guid("4130d5a7-ddd9-4b98-93bc-5fe69930a7cb"), 753, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.89858f, 0.625025f, "Morral del Cabrafeixet", 9 },
                    { new Guid("41a309ed-5df1-4969-9e50-e9aac22fdb6b"), 763, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 40.61364f, 0.530332f, "Torreta del Montsià", 22 },
                    { new Guid("41b090fa-0281-41bf-a833-e1ceae3f3ddf"), 1543, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.2202f, 2.049567f, "Pedró de Tubau", 14 },
                    { new Guid("4228595c-e6a6-47ff-aba8-0e311b8baaa6"), 1489, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.21158f, 1.044505f, "Rocalta", 25 },
                    { new Guid("4477595b-d2e5-40db-a45c-110a505eebc3"), 2912, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.62553f, 1.602514f, "Pic de la Serrera", 100 },
                    { new Guid("44a2b4c4-4f01-4e03-ad0f-6f1e59932792"), 2445, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.51858f, 0.765366f, "Tossal de les Roies de Cardet", 5 },
                    { new Guid("44b6508a-b960-43c7-a77e-b9bb51fa5db9"), 2956, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.61677f, 0.723704f, "Tuc de la Tallada", 39 },
                    { new Guid("451eb440-6f9d-46ed-a2fc-b9e40f751054"), 1267, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.52267f, 2.377506f, "Torre de Goà", 203 },
                    { new Guid("474b2999-2a80-4f56-b605-7a450a348980"), 830, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.08041f, 2.259536f, "Serrat Alt", 24 },
                    { new Guid("4761f041-c133-4b52-814f-82b774607536"), 3029, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.59138f, 0.827834f, "Pic de Comaloforno", 5 },
                    { new Guid("478db2ba-664a-43a2-bd7d-7a7020bb2591"), 1681, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.37122f, 1.560497f, "Roc Beneïdor", 4 },
                    { new Guid("47dadff0-4614-44ce-984b-d9cdae410683"), 2772, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.47294f, 0.930323f, "Pic de Filià (Tossal de Paiasso)", 25 },
                    { new Guid("48de7707-a3f3-4f9e-81da-2f56f288ce86"), 2915, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.48758f, 1.761933f, "Puigpedrós", 15 },
                    { new Guid("49b87691-12a8-4104-8e2e-33e5709744e5"), 2700, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.60837f, 1.039255f, "Lo Tésol (Tesó de Son)", 26 },
                    { new Guid("49cc1b6b-94be-41c1-ac35-c8447f89088b"), 459, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.75341f, 1.067247f, "Pilar d'Almenara", 38 },
                    { new Guid("49da0469-ddaa-4794-881e-6fe1f82881ce"), 1102, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.64115f, 2.017672f, "La Mola de Sant Llorenç del Munt", 40 },
                    { new Guid("49e90b5f-4740-41cd-94ab-f0a8cb0243b3"), 2778, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.6272f, 1.019481f, "Pui de la Bonaigua", 26 },
                    { new Guid("49ef6632-8e70-4326-905b-f23a2c6ded20"), 1265, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.83259f, 2.347413f, "Turó del Pou d'en Sala", 24 },
                    { new Guid("4a1f602e-f88a-4b26-85b4-da165b64f350"), 2284, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.20081f, 1.613706f, "Cap del Verd", 4 },
                    { new Guid("4bec1d21-ed18-4331-9d1d-c0a255e2d420"), 432, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.30107f, 3.248749f, "Puig dels Bufadors", 2 },
                    { new Guid("4bfef8c6-81b2-42d4-8446-fb79bedbdd7e"), 2861, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.42465f, 2.167884f, "Noufonts", 203 },
                    { new Guid("4c1f7ab5-40e6-49f0-9eb8-aa76f7256601"), 2436, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.40866f, 1.214854f, "Torreta de l'Orri", 26 },
                    { new Guid("4c5341eb-9847-4f94-9e8e-4d445fe4b379"), 1889, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.31686f, 0.809481f, "Pala del Teller", 5 },
                    { new Guid("4cd320e9-dff7-4085-b1f5-fca73457030c"), 963, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.40688f, 1.422262f, "Montagut d'Ancosa", 1 },
                    { new Guid("4daf0898-6823-4dd0-8137-b25a6f1a84bd"), 1345, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.73416f, 0.227329f, "El Negrell", 22 },
                    { new Guid("4df2ff5b-6307-471f-be59-6adf9a6c1ea4"), 495, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.38871f, 0.401385f, "Punta de Montmeneu", 33 },
                    { new Guid("4dfab0bd-7a55-4d25-9905-9479e4095b21"), 1248, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.10154f, 2.29375f, "Bellmunt", 24 },
                    { new Guid("4fcee297-2bc0-4334-b73f-c51a8cc9133f"), 1133, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.59999f, 1.805088f, "Montgròs", 6 },
                    { new Guid("4fff0b5a-23c3-417c-86de-2069bacbc4da"), 533, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.30846f, 1.886802f, "les Solius", 11 },
                    { new Guid("5018e7e6-1cfc-4216-ac75-dce043835c0f"), 703, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.38805f, 1.542632f, "Clapí Vell", 3 },
                    { new Guid("5040fc3e-0f20-48dc-8aed-237060413622"), 233, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.75455f, 2.730416f, "Turó de Puigmarí", 34 },
                    { new Guid("50ba89b2-1906-4de4-908a-c6e550e55c1b"), 939, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.64452f, 1.970874f, "Castellsapera", 40 },
                    { new Guid("50cb4c69-412e-4f5d-8bab-a08e2856b026"), 828, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.57278f, 1.304921f, "Tossal de Suró", 32 },
                    { new Guid("51b21192-436d-419d-ac69-6ddfc59a5b44"), 2942, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.5918f, 1.443655f, "Comapedrosa", 100 },
                    { new Guid("51fc94f0-c5f1-4940-b691-b331377199d4"), 682, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.31997f, 3.167825f, "Castell Saverdera", 2 },
                    { new Guid("527da28d-15b7-4a2b-89b7-18244983f7eb"), 328, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.73551f, 2.776659f, "Montbarbat", 21 },
                    { new Guid("52aedd06-cb0c-44c3-a08e-9d9fcdc5f25b"), 667, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.90175f, 1.108782f, "Puig de Grialó", 23 },
                    { new Guid("52e8e43b-3e68-42ac-a604-d2f33cb2f328"), 2778, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.61029f, 0.901178f, "Tuc de Lluçà", 5 },
                    { new Guid("54a5d7e8-626c-4e3a-a42b-3e5f995e8844"), 341, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.4513f, 2.024098f, "Puig Madrona", 11 },
                    { new Guid("551028d1-26ec-4139-96c9-7e8ea2d72f74"), 528, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.87558f, 0.570928f, "Montaspre", 9 },
                    { new Guid("554daa6a-7f7a-4a91-aaec-884f90ca26cb"), 383, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.02771f, 0.557909f, "l'Àliga (Mola del Broi)", 30 },
                    { new Guid("556bd444-1536-45ec-b164-c705078ba6bf"), 525, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.15394f, 0.977411f, "el Bres", 8 },
                    { new Guid("55bd9768-40da-4251-a120-13abebfe5221"), 2624, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.47778f, 2.215309f, "Puig de Gallinàs", 203 },
                    { new Guid("55c4d224-ac45-48e5-bc62-d5b9e26155a1"), 740, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.98101f, 2.388984f, "Puig de la Força", 24 },
                    { new Guid("55e36e7a-e9ad-42f2-921c-4775f07a1879"), 867, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.87079f, 2.320939f, "L'Enclusa", 24 },
                    { new Guid("564ba7f1-5837-4ba6-9e55-f87a83107e76"), 3023, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.59379f, 0.825995f, "Besiberri Sud", 5 },
                    { new Guid("570804da-73c8-46a7-a6a6-de87a09ed791"), 1072, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 40.94304f, 0.370789f, "Tossal d'Engrilló", 9 },
                    { new Guid("57bcdf96-fd5f-40f4-bfcb-e589ce2423c4"), 848, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.77064f, 1.574776f, "Castell de Boixadors", 6 },
                    { new Guid("5971b05f-201e-4642-96c7-03a478c02aeb"), 2889, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.58747f, 1.427663f, "Pic de Sanfonts", 100 },
                    { new Guid("5a39d63e-5d53-4f70-8f49-793464050c2d"), 542, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.19965f, 1.017457f, "Serret del Cisa", 8 },
                    { new Guid("5a5afafc-39d3-4496-92c9-92f0240093ea"), 2782, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.43222f, 2.187114f, "Pic de Racó Gros", 203 },
                    { new Guid("5b5c4cc4-0966-41e6-be41-af32f5dc187a"), 433, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.64303f, 0.561878f, "els Quatre Mollons", 22 },
                    { new Guid("5c1ca8db-77b0-487c-8190-a5669c990ba0"), 2547, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.36177f, 2.084664f, "Puig de Dòrria", 201 },
                    { new Guid("5c4cad72-5fdb-4d96-a584-a2c1e15455e4"), 877, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.9299f, 0.720439f, "Malera", 23 },
                    { new Guid("5c838b67-72ac-4e7d-9ba1-cefec694a628"), 994, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.47347f, 3.038041f, "Pic de Sallafort", 2 },
                    { new Guid("5dd19e23-8c52-4b31-b187-41b97db94eb3"), 991, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.28789f, 1.01719f, "Picorandan", 8 },
                    { new Guid("5f8d5754-7382-48b7-9b5c-febdcae62c18"), 2585, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.56519f, 0.798188f, "Pic Roi", 5 },
                    { new Guid("610db453-f6fd-44e7-9947-d4f1a1c0660c"), 507, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.72692f, 2.700226f, "Força Real", 202 },
                    { new Guid("6192895e-3c88-45c5-89ee-0a2f53dc20fd"), 387, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.25345f, 1.370483f, "Tossa Grossa de Montferri", 1 },
                    { new Guid("625cfc04-ca3d-4ea6-bbf8-3c0e6169366a"), 1157, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.06136f, 1.781976f, "Sant Salvador", 14 },
                    { new Guid("6262e97b-7539-48ff-8ac3-3f3292c72a63"), 2496, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.75038f, 1.132836f, "Pic de Montalt", 26 },
                    { new Guid("628d66f1-2d62-4e17-8adb-97a5aa3cd3fe"), 2753, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.45919f, 1.585502f, "Tossal de la Truita (Pic de Perafita)", 100 },
                    { new Guid("62fe0f46-44ad-4a02-99fa-29b0c3aef6ed"), 1150, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.94874f, 2.513502f, "Sant Benet", 34 },
                    { new Guid("639c08a9-688c-4328-b6bf-7015110067df"), 803, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.46886f, 1.112871f, "Tossal Gros de Vallbona", 16 },
                    { new Guid("6425a188-1955-4f12-b5d9-0a646ab79aa8"), 417, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.78044f, 2.975677f, "Puig de les Cols", 10 },
                    { new Guid("649899f7-5926-4fc6-9618-bf7a5185ebe4"), 40, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.4659f, 2.278682f, "Turó de Montgat", 21 },
                    { new Guid("652f092e-0fb5-48c5-9e97-daa5a98840fb"), 1613, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.3808f, 0.923423f, "Santa Bàrbara (Vall Fosca)", 25 },
                    { new Guid("65fe4368-46a4-4f09-a55e-e6efd8a531c7"), 635, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.57634f, 1.933969f, "Turó del Ros", 40 },
                    { new Guid("66d43ccd-43e1-44ab-85cb-c44ad5a05881"), 2211, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.15092f, 1.539442f, "Puig de les Morreres", 35 },
                    { new Guid("670978f8-5e76-4210-8bad-3907e062aeca"), 786, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.3488f, 1.224827f, "La Cogulla", 1 },
                    { new Guid("67a3deaa-8788-4ef1-b1e6-b0ece61cca06"), 2562, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.28708f, 1.584198f, "Torreta del Cadí", 4 },
                    { new Guid("687eed42-c627-4a87-8a8f-3cc73ae843c8"), 2860, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.6068f, 1.46403f, "Pic del Pla de l'Estany", 100 },
                    { new Guid("68962729-cc31-4292-88ce-1e5a831b4a3e"), 1098, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.71278f, 0.306421f, "la Portella del Pinell", 22 },
                    { new Guid("696f570c-cc1c-4143-b801-455085bbfc33"), 1034, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.79976f, 0.369778f, "Mola Castellona", 9 },
                    { new Guid("6a42775a-c791-4cf5-95d6-da1921c25875"), 1672, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.02107f, 0.959033f, "Tossal de Mirapallars-i-Urgell", 23 },
                    { new Guid("6c241448-2ad2-47c9-8c28-3e93467698cf"), 1300, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.28413f, 2.427763f, "Puig Ou", 19 },
                    { new Guid("6c2d31c3-149c-44fe-b7a2-456421dbf0dd"), 832, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.42801f, 2.802938f, "Puig dels Pruners", 2 },
                    { new Guid("6cb944a2-2bf4-4abe-8451-bb4b4d302547"), 499, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.57564f, 1.892993f, "Puig Cendrós", 11 },
                    { new Guid("6d83bbe4-3512-436b-9a11-7356bc635bca"), 468, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.36781f, 1.95404f, "Puig Vicenç", 11 },
                    { new Guid("6e3746d7-5431-4c00-b59b-cb1b55e56d4f"), 1273, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.74268f, 2.365525f, "Turó del Samon", 41 },
                    { new Guid("6e3a6e52-311d-4bf1-a7bd-7bc5e2b82e83"), 2905, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.60175f, 1.36165f, "Monteixo", 26 },
                    { new Guid("6f694baf-f781-467b-bb4c-d3658722b518"), 2173, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.70782f, 0.758684f, "Montcorbison", 39 },
                    { new Guid("6f764eda-7143-4e00-ba8a-336276659e01"), 2842, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.46275f, 1.631675f, "Tossal Bovinar", 15 },
                    { new Guid("706087f0-2849-4ec0-9c00-a73825280f03"), 1068, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.08379f, 1.270382f, "Sant Honorat", 4 },
                    { new Guid("7264c946-f38b-4bb3-89d7-200fa0ce7a80"), 524, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.66788f, 1.892855f, "Puigsoler", 7 },
                    { new Guid("7274ee63-19fc-4aaa-bb23-b0201231d483"), 1361, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.30512f, 2.375953f, "Sant Antoni", 31 },
                    { new Guid("7291c852-99e3-45b0-92e2-417521d1ec59"), 2470, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.60482f, 2.031042f, "Puig del Pam", 200 },
                    { new Guid("73a1add0-4910-48e8-a6b2-f8228d999f06"), 1391, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.97781f, 0.941309f, "Sant Mamet", 23 },
                    { new Guid("75ddb443-7e35-41b1-b207-48c52bb178b5"), 416, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.55541f, 1.815801f, "Turó de Can Dolcet", 6 },
                    { new Guid("76374015-df8e-40d6-90b4-e4fbf0f7ff1c"), 266, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.41843f, 2.153352f, "Turó d'en Móra (Turó del Carmel)", 13 },
                    { new Guid("7644f6bc-2f15-4c27-b9e2-1cc2fd452d31"), 2857, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.53073f, 0.957631f, "Pic de Mariolo (Pic de Neriolo)", 5 },
                    { new Guid("778845db-6ce9-4048-847e-154189cddd25"), 2581, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.53058f, 1.854635f, "el Punxó", 201 },
                    { new Guid("7806934d-8696-450d-acf5-4d99f8e926bc"), 928, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.83359f, 1.528941f, "Serra de Pinós", 35 },
                    { new Guid("79b9dcb8-9633-4aad-97b7-c16047b7d819"), 1010, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.2495f, 0.96072f, "Puig de Gallicant", 8 },
                    { new Guid("79c2d6cf-cba8-4343-ac67-783cc0948936"), 1326, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.20115f, 1.84353f, "Tossal Llisol", 14 },
                    { new Guid("79c7d2a1-2404-4a75-8a5c-60e7d09da07e"), 1027, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.11507f, 2.581472f, "Puigsallança", 19 },
                    { new Guid("7d197a63-d97e-43d0-b070-e7dfacf8b5d6"), 110, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.1449f, 1.337035f, "Sant Simplici", 36 },
                    { new Guid("7d60850e-5b16-45a2-ab7b-2b477a9352bb"), 1187, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.00981f, 2.445601f, "Rocallarga", 24 },
                    { new Guid("7d6ded5a-3cd8-43e8-a66f-abba982f28c2"), 1057, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.36073f, 2.696849f, "Puig de la Gavarra", 2 },
                    { new Guid("7d7cf62e-177b-4b0a-aeea-318b01ecc77a"), 2630, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.8257f, 0.858212f, "Tuc de Crabèra", 39 },
                    { new Guid("7ff37d97-2c3a-4f36-90bc-b85c70b1fa09"), 2657, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.50298f, 1.598505f, "Tossa del Braibal", 100 },
                    { new Guid("80e2c7f7-112a-43a8-b9c7-5f87f64f7798"), 2958, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.58524f, 0.906931f, "Pic de Contraix", 5 },
                    { new Guid("81263375-f4d0-4590-9389-7a4533ceb21c"), 682, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.1563f, 0.851249f, "lo Morral", 29 },
                    { new Guid("813ec322-c7b6-4093-810b-198b463a86fd"), 606, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.39268f, 3.096359f, "Puig d'Esquers", 2 },
                    { new Guid("81b8b660-30eb-46d3-8997-a52bdf7db85e"), 1699, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.37503f, 0.775128f, "La Faiada", 5 },
                    { new Guid("820b9d9c-3af2-48d2-9b2c-6aaf23a99eee"), 2791, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.51405f, 0.903326f, "Tuc des Carants", 5 },
                    { new Guid("823c3f5a-9ace-484f-8d26-b64a01b34e42"), 303, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.05211f, 3.131649f, "Castell de Montgrí", 10 },
                    { new Guid("82558750-0355-4acb-b57f-25c7e9eb09ea"), 2405, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.5048f, 1.460396f, "Bony de la Pica o Pica d'Os", 4 },
                    { new Guid("8268e8d0-e7dc-4768-94d9-e073d90ed716"), 2864, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.7108f, 1.1768f, "Mont-roig", 26 },
                    { new Guid("830e5447-b768-4c52-9a98-399eeb702f80"), 2830, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.43444f, 2.147147f, "Torre d'Eina", 201 },
                    { new Guid("83599546-f907-4de7-9ac5-5f04a402d17b"), 2506, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.42515f, 2.318749f, "Roca Colom", 203 },
                    { new Guid("83769397-545c-41b7-970c-084e5df21ea4"), 943, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.44833f, 1.516478f, "El Castellar (serra d'Ancosa)", 6 },
                    { new Guid("83da1893-5afb-43b9-946f-8e8308828759"), 2760, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.44995f, 1.580997f, "Monturull", 4 },
                    { new Guid("8445ca22-3e87-4d00-9910-b6449764b636"), 1610, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.11414f, 1.267513f, "El Coscollet", 4 },
                    { new Guid("848d852c-3076-46a8-9d9e-95fb5c1756e4"), 993, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.6049f, 1.792784f, "Miranda del Príncep", 6 },
                    { new Guid("8499f766-3438-48c9-83fd-3dfa1c752554"), 621, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.99286f, 0.397732f, "Rocamala", 37 },
                    { new Guid("84a8b708-4402-4ad2-8593-10111ddff15e"), 2983, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.57471f, 0.890943f, "Pic de la Pala Alta de Sarradé", 5 },
                    { new Guid("85a53ee5-d709-4e07-a92c-2903eca41b5c"), 1236, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.60538f, 1.811504f, "Sant Jeroni", 6 },
                    { new Guid("86e8d2bc-e818-4693-9c30-72a2f3e94f38"), 1526, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.11003f, 1.639453f, "El Cogul", 35 },
                    { new Guid("86f0430e-ae7e-4bfd-8d16-86dcd0da4983"), 1217, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.84502f, 0.327442f, "Tosseta Rasa", 9 },
                    { new Guid("86f710f7-4ba3-4bcb-8a99-26b4b70cb2b3"), 1193, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.83501f, 0.280338f, "la Miranda de Terranyes", 37 },
                    { new Guid("876332ab-ca30-474d-8cfe-6562d81e2e57"), 754, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.42776f, 3.08205f, "Puig d'en Jordà", 2 },
                    { new Guid("894155cb-9a81-4367-8896-7f75a3cc408a"), 841, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.87077f, 0.266146f, "lo Blau (Roca Grossa Moletans)", 37 },
                    { new Guid("89e3b0db-7a19-4205-9c47-fd4fc825d452"), 295, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.33778f, 2.011178f, "Sant Ramon", 11 },
                    { new Guid("8a09f62c-a97a-4e2a-ab59-9d1adf4ff3aa"), 852, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.88528f, 2.565848f, "Roques del Rei", 34 },
                    { new Guid("8a4624ce-c771-4ab5-83d9-a8345a782c25"), 2731, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.4921f, 2.468364f, "Puig de Tretzevents", 203 },
                    { new Guid("8a5d28b3-223f-4d11-8ac3-b007e78fe2cc"), 3073, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.66871f, 1.387458f, "Pic de Sotllo", 26 },
                    { new Guid("8b2a99bf-1eb3-40e9-9900-063cbbad00c8"), 1056, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.74748f, 2.296021f, "Tagamanent", 41 },
                    { new Guid("8b6a7a2a-6c3f-4144-8ef5-614f51734f6f"), 500, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.98742f, 0.593574f, "Coll de Pins", 30 },
                    { new Guid("8bad6113-718c-4ee5-ab22-55c1a2e86131"), 2673, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.56914f, 1.879615f, "Puig de Font Viva", 201 },
                    { new Guid("8bdedd08-0c60-4c91-a715-824c52b8ebba"), 1429, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.13255f, 1.192696f, "Serrat de Carrasquers", 4 },
                    { new Guid("8c066db0-2bad-42e0-87d5-73f3c992dc22"), 2830, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.6079f, 0.981018f, "Pic de Saboredo", 26 },
                    { new Guid("8d16a126-2a0d-48e6-afd4-c2eaa949ed00"), 733, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.63724f, 1.580985f, "Puig de Sant Miquel", 6 },
                    { new Guid("8ea15285-23fa-49e2-a88e-7bc6c9efe642"), 1211, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.1884f, 1.343498f, "Tossal de Balinyó", 4 },
                    { new Guid("8f4dc3d1-9222-4efc-bd01-6ae80eda7f1a"), 2753, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.45294f, 0.879226f, "Pica de Cerví", 25 },
                    { new Guid("8faa4805-3912-42b0-a710-4ce7e596f0ab"), 1014, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.9498f, 0.632397f, "Punta d'Ossos", 23 },
                    { new Guid("9039bc6f-030d-42c8-b0c1-8ae9a11d2f0e"), 1257, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.4821f, 2.94711f, "Puig Neulós", 2 },
                    { new Guid("9053f883-67b7-445d-a452-f705fe6c491a"), 1615, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.21519f, 1.96373f, "Roca del Joc (Roca de la Devesa Jussana)", 14 },
                    { new Guid("907e4ec5-f23e-43d9-8106-8e18ad92961d"), 1635, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.46007f, 2.537154f, "la Soca", 204 },
                    { new Guid("91760181-6a70-49ec-9029-e59b8f74be4e"), 2546, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.66111f, 2.002223f, "Pic de Baixollada", 200 },
                    { new Guid("921cae99-0ed2-499d-b86a-d2f006be8b33"), 789, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.15399f, 2.536068f, "Volcà del Croscat", 19 },
                    { new Guid("92457bc2-6973-4bc3-9c37-dac5f4756a11"), 1057, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.67504f, 2.004627f, "Montcau", 7 },
                    { new Guid("926a1917-f1c7-43ff-b832-d459cedf93eb"), 2713, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.39088f, 2.196201f, "Torreneules", 31 },
                    { new Guid("92e79514-64f3-41d4-8878-9e347dc4e53f"), 413, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.57909f, 1.884429f, "Sant Salvador de les Espases", 11 },
                    { new Guid("92ef9a82-82c1-48bb-bebd-1db82e8fc77b"), 668, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.62921f, 2.101229f, "Puig de la Creu", 40 },
                    { new Guid("92f49e69-b0c8-4082-be28-f88054dd5586"), 1315, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.29893f, 1.018841f, "Tossal de l'Àliga (la Geganta Adormida)", 26 },
                    { new Guid("93d0b048-84bb-4a9f-8da8-f04a485f2c86"), 1117, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.09051f, 1.961989f, "Serrat de Sant Isidre", 14 },
                    { new Guid("9405017c-6577-4ca1-8c0f-76b0b05dfb4d"), 697, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.96809f, 0.809551f, "Tossal de l'Alzina", 8 },
                    { new Guid("941882b6-7278-4779-b3b0-b95904c8264b"), 2013, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.2843f, 2.253992f, "Puig Estela", 31 },
                    { new Guid("968d26c0-e5d2-4027-a696-8ffb9c2b4a75"), 804, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.6945f, 2.017894f, "Roca Sereny", 7 },
                    { new Guid("969f1313-26af-41fb-8048-4c805f53cf33"), 2883, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.64322f, 0.705629f, "Malh des Pois (la Forcanada)", 39 },
                    { new Guid("96f0d5e2-1880-472a-9bd0-8779389ab3a0"), 2822, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.51725f, 1.721443f, "Pic Negre d'Envalira", 100 },
                    { new Guid("976f508f-d284-48bb-96fe-bc3b0279fa8f"), 2445, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.4237f, 1.033049f, "Tuc de la Cometa", 25 },
                    { new Guid("983a0481-939c-4ea8-80f9-93c5be949c85"), 658, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.84657f, 1.402071f, "Tossal de la Creu", 32 },
                    { new Guid("9b6b1d72-4cda-4171-bee3-a6df7c36506f"), 519, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.75733f, 2.92821f, "Puig Cadiretes", 20 },
                    { new Guid("9b75b58b-604b-4596-bfa2-4c2b25e3203b"), 861, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.33579f, 1.466714f, "Talaia del Montmell", 12 },
                    { new Guid("9b88ea1f-c973-4b19-b7cf-92bcbc2e8cf0"), 2905, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.4682f, 1.656561f, "Tossa Plana de Lles (pic de la Portelleta)", 100 },
                    { new Guid("9b9a0d9a-8192-492a-bbaf-2a3afa36623e"), 812, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.8993f, 0.417868f, "la Moleta", 9 },
                    { new Guid("9bdb76a1-0cd9-4448-8896-489069ef4dbf"), 2859, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.59753f, 1.394249f, "Pic de Gerri", 26 },
                    { new Guid("9c05b0bc-951d-41ec-bcc5-3aa3b54f93f9"), 832, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.95963f, 2.376163f, "Puig del Far", 24 },
                    { new Guid("9c6c2740-425f-46a0-a6e9-0fcddcfe1af1"), 1350, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 40.73279f, 0.170232f, "Tossal dels Tres Reis", 22 },
                    { new Guid("9cfa3f32-37d4-4730-9749-9f4805648b19"), 1189, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.11667f, 1.581828f, "Mola de Lord", 35 },
                    { new Guid("9d40e9ae-ed20-4754-98fd-c02ddd16fd0c"), 1379, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.11475f, 1.659226f, "Tossal de les Viudes", 14 },
                    { new Guid("9d51c6c4-678c-4bde-b642-82dedccefcee"), 1697, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 41.80882f, 2.382618f, "Matagalls", 24 },
                    { new Guid("9daa5b6d-5712-49e4-8fd5-2ebef9678e1d"), 1074, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.26878f, 2.48305f, "Montmajor", 19 },
                    { new Guid("9dc3f8ba-33ad-40e3-8897-4d6db87723ed"), 948, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.46572f, 1.345648f, "Sant Miquel de Montclar", 16 },
                    { new Guid("9e0202d4-0acb-4baa-b260-c4bfe4bcd207"), 131, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.55458f, 2.469031f, "Turó d'Onofre Arnau", 21 },
                    { new Guid("9f0c0b9f-fb5c-41f5-9991-f9cd43cd8035"), 2461, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.502f, 2.507173f, "Pic de Gallinassa", 203 },
                    { new Guid("9f5f192c-009b-495a-a133-4f4cc8992ff2"), 2099, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.50877f, 2.323217f, "Puig de les Tres Esteles", 203 },
                    { new Guid("9f73f9ca-87c9-4b8a-8992-4489bd3e83e3"), 326, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.77994f, 0.749767f, "Tossal de Mormur", 23 },
                    { new Guid("a01b6d31-f01f-4cab-9269-dd037c72c0c5"), 2339, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.76561f, 0.732791f, "Montanha d'Uishèra", 39 },
                    { new Guid("a1362d3c-8d87-47ad-a72d-d069176f0121"), 1308, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.076f, 2.407272f, "Cabrera", 19 },
                    { new Guid("a2fca415-0e16-4fe5-9491-7c783233bd22"), 2784, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.5189f, 2.45656f, "Canigó", 203 },
                    { new Guid("a31a0356-697c-4e4b-8378-c7b3df1afee6"), 2703, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.62392f, 0.946849f, "Tuc Gran dera Sendrosa", 39 },
                    { new Guid("a3215ba8-279a-453e-bc90-71e166b7bf8f"), 1319, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.74911f, 2.345652f, "el Sui", 41 },
                    { new Guid("a3482fc0-9a77-4ae1-975a-b0ee1f42915d"), 2789, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.51445f, 1.385138f, "Pic de Salòria", 4 },
                    { new Guid("a351511e-ddf4-447a-9818-77052738cfae"), 430, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.44217f, 2.129683f, "Turó de Magarola", 13 },
                    { new Guid("a3cffb33-4875-487a-b7ae-9636db6f953d"), 1002, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.99818f, 1.039182f, "el Cogulló", 23 },
                    { new Guid("a3e7b20a-928c-47ec-bdde-190ffd10ed13"), 401, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.78866f, 2.956193f, "Montclar", 10 },
                    { new Guid("a49d2916-e51b-421d-b4dc-62c71f017f04"), 1172, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.11926f, 1.951409f, "Salga Aguda", 14 },
                    { new Guid("a529bab3-41d4-4c8e-80ca-da99ebc13b24"), 533, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.88873f, 2.994298f, "Puig d'Arques", 10 },
                    { new Guid("a6d21836-e44a-4bef-ad04-58a96b413b99"), 1839, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.31412f, 0.838204f, "l'Avedoga d'Adons", 5 },
                    { new Guid("a6eab5fc-79f8-4fd3-bd09-ec6e4ba9e764"), 1631, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.31177f, 1.542997f, "Tossal de Lletó", 4 },
                    { new Guid("a7856a26-5bad-4d71-a6c6-f2dd0df47957"), 2965, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.65658f, 1.405891f, "Pic de Canalbona", 26 },
                    { new Guid("a7b7b0e7-82a3-4dc9-82a3-9a0196043787"), 2731, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.76778f, 0.933047f, "Tuc de Parros", 39 },
                    { new Guid("a8a8c4d4-a446-40e0-ac76-fc91a3464cfd"), 1156, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.59578f, 1.829739f, "Roca de Sant Salvador (l'Elefant)", 7 },
                    { new Guid("a966494a-38fa-4af2-877f-d04de1f5b802"), 2536, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.32058f, 1.892661f, "La Tosa", 14 },
                    { new Guid("a9be298e-0092-4339-8e11-dbb57d9a0708"), 1749, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.19357f, 1.123586f, "Cap de Carreu", 25 },
                    { new Guid("aa418db5-2ae0-4538-8104-2caf064bf200"), 425, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.03592f, 0.679571f, "les Càrcoles", 30 },
                    { new Guid("aa5c82d5-225b-4d40-9d2c-ffdef49b7fa4"), 834, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.19359f, 0.931585f, "Miranda de Puigcerver", 8 },
                    { new Guid("aaab65d9-adaf-4509-b06a-660e5af24afb"), 770, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.68215f, 1.984428f, "Pujol de la Mata", 7 },
                    { new Guid("aaac54ad-ac46-4456-afbe-730423a550b4"), 225, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.0554f, 3.189831f, "Roca Maura", 10 },
                    { new Guid("aada96b5-4865-4b09-b80f-6446a7ed2d76"), 1092, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.92773f, 0.368809f, "Punta de l'Aigua", 9 },
                    { new Guid("aaf04e71-e264-4cb1-8887-3aad1841c7b0"), 794, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.49817f, 3.026334f, "Torre de la Maçana", 202 },
                    { new Guid("ac261ff9-8bd4-4f03-a7db-55c27da5afd5"), 963, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.09578f, 1.908241f, "Montsent (Serrat de Picancel)", 14 },
                    { new Guid("ac424849-5588-43c3-aa56-5efd8a0b547f"), 2656, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.68283f, 1.131507f, "Pic de Pilàs", 26 },
                    { new Guid("ac46da2c-fbd5-48ab-ad86-b508b07e41d8"), 1124, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.02084f, 2.536639f, "El Far", 34 },
                    { new Guid("ace67c3b-320b-423c-9f59-338bc994179d"), 727, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 40.99062f, 0.847836f, "Molló Puntaire", 8 },
                    { new Guid("ad6b0013-94f0-433c-bd4c-8f636e4c322c"), 653, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.40808f, 1.884979f, "Puig d'Agulles", 3 },
                    { new Guid("add42e19-8f44-47c3-9daa-a57496c726d8"), 598, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.01749f, 2.6619f, "Sant Roc", 19 },
                    { new Guid("ae5e61c7-688e-46a0-b29f-be4892d1c913"), 2671, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.60485f, 1.933476f, "Puig de la Grava", 201 },
                    { new Guid("ae682039-da93-4c72-ba89-628111711f12"), 1281, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.43206f, 2.650714f, "Piló de Bellmaig", 204 },
                    { new Guid("ae835970-b3c0-4cd6-abdf-7b48f9afa5ee"), 464, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.96305f, 2.924637f, "Montigalar", 20 },
                    { new Guid("ae97dc66-a120-4868-8e3b-a235f3138b68"), 1229, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.13974f, 2.108336f, "Puig Cornador", 31 },
                    { new Guid("afa877f2-d093-43b3-9a6b-b9a3273dd300"), 2780, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.60918f, 1.735632f, "Pic d'Escobes", 100 },
                    { new Guid("afce425a-6324-40fc-a959-619f37de2677"), 2774, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.44633f, 2.155158f, "Roc del Boc", 201 },
                    { new Guid("b01872ab-de48-4ccf-9715-5dd0ebd9a171"), 626, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.60479f, 1.670811f, "Puig d'Aguilera", 6 },
                    { new Guid("b01cf4fd-166f-4483-8a43-3d442c366d7e"), 1969, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.32096f, 1.175846f, "Les Piques", 4 },
                    { new Guid("b116b575-8d4a-420d-a76d-a625a4f218bf"), 2886, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.53375f, 0.988832f, "Pic Tort", 25 },
                    { new Guid("b14a54fc-8533-48cb-968d-313351c9d2b9"), 325, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.77502f, 2.997967f, "Puig Gros", 10 },
                    { new Guid("b1c0d2e1-fcae-421a-9a06-0390ff03fba0"), 707, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.91853f, 2.865186f, "Montolier de Perellós", 202 },
                    { new Guid("b282186c-5bd5-4c58-afbc-54bf897fb0c7"), 346, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.33316f, 0.381876f, "Tossal Gros d'Almatret", 33 },
                    { new Guid("b2da3f6e-ac98-4576-8832-7e8dfece56b7"), 389, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.39366f, 2.097878f, "Sant Pere Màrtir", 11 },
                    { new Guid("b348c9be-c622-4e47-b166-8992435f9c7f"), 2077, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.23501f, 1.134684f, "Cap de Boumort", 25 },
                    { new Guid("b3e6e22e-c042-430a-9bfb-e26cc4432acd"), 1203, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.92312f, 2.533029f, "Sant Miquel de Solterra", 34 },
                    { new Guid("b3f5d469-5fff-49ea-9c75-40b28e2021aa"), 1239, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.07221f, 1.100163f, "Roca Roja", 23 },
                    { new Guid("b46efee8-7d31-47d2-89b7-fe28425f1efa"), 2710, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.65862f, 0.695561f, "Malh dera Artiga", 39 },
                    { new Guid("b48f3849-c79e-4808-a77a-21817ee4ae07"), 752, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 40.95689f, 0.338328f, "Santa Bàrbara", 37 },
                    { new Guid("b50c7634-c14b-4aac-ba1c-4d9beddd3c7c"), 2869, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.49238f, 1.730058f, "Tosseta de l'Esquella (Pic de Calm Colomer)", 15 },
                    { new Guid("b5c2ac49-5fd3-4726-b617-c26c7d77f7c4"), 1062, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.27753f, 0.805672f, "La Cogulla (Montsant)", 29 },
                    { new Guid("b7ae97b7-c657-4988-946d-e4035b5cd445"), 207, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.43521f, 3.170752f, "Puig de Cervera", 2 },
                    { new Guid("b8c82cdb-8087-49fd-8cbd-cb81f00d87a2"), 718, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.02895f, 0.736267f, "La Tossa (Tivissa)", 30 },
                    { new Guid("b8f178c1-6ef8-4cee-97b3-98ca0ad347ff"), 2634, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.64425f, 0.796356f, "Tuc de Sarrahèra", 39 },
                    { new Guid("b9523488-d81a-467b-a6a8-243b461fdd37"), 1441, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 40.80313f, 0.343121f, "Caro", 9 },
                    { new Guid("b9a64d0c-94f4-4b5b-a77a-ce70c70fe0f5"), 2883, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.49011f, 1.024423f, "Montsent de Pallars", 25 },
                    { new Guid("ba345051-a0c6-4ad6-9b3e-773f7b1e6030"), 624, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.85092f, 1.830099f, "Turó de la Torre o de Castellnou", 7 },
                    { new Guid("bb1b2e3b-d530-4778-bbd8-453f4d309d3e"), 522, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.62474f, 2.678001f, "Sant Martí de la Roca", 202 },
                    { new Guid("bbc0d51c-fbac-41fc-a3fb-9267cf8bae02"), 2370, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.62162f, 2.194762f, "Puig de la Pelada", 203 },
                    { new Guid("bc4a525c-19b1-4332-aea0-1948f70f98c5"), 942, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 40.94069f, 0.586019f, "Creu de Santos", 9 },
                    { new Guid("bcb45d0e-6d15-473b-ae64-dd13296e774c"), 1110, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.68511f, 2.437707f, "Roca Gelera", 203 },
                    { new Guid("bcdda778-c7d2-48d4-bd7a-ae70a358c2e6"), 1557, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.33331f, 2.527761f, "Comanegra", 19 },
                    { new Guid("bd5544e8-68db-459e-8b6c-2ff53113c113"), 576, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.86568f, 2.793761f, "la Serra (Vingrau)", 202 },
                    { new Guid("bdb6bb65-7306-4e5d-8f4a-3b4eaccb72bf"), 381, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.32295f, 1.743849f, "Penya del Papiol", 3 },
                    { new Guid("bdc86522-1ca2-4d44-b558-18bd64633813"), 522, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.81393f, 1.785542f, "Puig-alter", 7 },
                    { new Guid("bdea8b16-e29e-4a04-b15e-6e709414b810"), 228, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.96032f, 3.1701f, "Quermany Gros", 10 },
                    { new Guid("bdffd725-7733-49cc-9bff-6d6128ac374e"), 2861, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.47476f, 1.677613f, "La Muga", 15 },
                    { new Guid("be368112-816e-42d3-a715-c7ebed5e385f"), 1136, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.32555f, 1.06058f, "Mola d'Estat", 8 },
                    { new Guid("be6c52d8-9791-4862-9f07-09badb90e40a"), 2403, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.45461f, 2.396931f, "Puig de la Collada Verda", 203 },
                    { new Guid("bee2a01a-7ec2-4f51-af85-89d981df0b35"), 881, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.68747f, 1.686425f, "Cogulló de Cal Torre", 7 },
                    { new Guid("bef5dbf6-352a-44bc-a254-b165539a210c"), 2113, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.26439f, 1.597825f, "El Cadinell", 4 },
                    { new Guid("bf15a562-f705-4584-87c2-cfdad729db97"), 839, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.96354f, 2.592501f, "Puigdefrou", 34 },
                    { new Guid("bf4d48ee-7c27-4f4c-a850-5c10b15df127"), 672, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.43826f, 3.120929f, "Querroig", 2 },
                    { new Guid("bf7cea58-250f-4696-8341-8aa8b6e19bef"), 1692, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.2692f, 0.852649f, "Pui de Lleràs", 25 },
                    { new Guid("bf85b812-95d0-4529-9256-547fc2f091bf"), 2548, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.28369f, 1.726288f, "Comabona", 14 },
                    { new Guid("c05b086c-7918-45a1-a87e-fc9481912069"), 617, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.86126f, 1.93872f, "el Garrofí", 7 },
                    { new Guid("c06c85e5-709b-41a9-b939-24ce43e116de"), 534, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.31932f, 1.847853f, "Puig de la Mola", 3 },
                    { new Guid("c14de667-8442-4af4-aa91-ddc463b08c4c"), 3008, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.60526f, 0.826304f, "Besiberri Nord", 5 },
                    { new Guid("c16ee7fc-83c7-4de1-800c-6e49cc0ee0b2"), 2532, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.54134f, 0.808755f, "l'Aüt", 5 },
                    { new Guid("c1723748-9bb8-420b-a0a4-231074df4f3e"), 2952, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.59137f, 0.808478f, "Punta Senyalada", 5 },
                    { new Guid("c18da1af-599a-4f8a-a303-e3d8fd7fd8f7"), 1546, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.33308f, 0.7711f, "Tossal Gros (Serra de Sant Gervàs)", 5 },
                    { new Guid("c19853df-afe0-4799-980d-8376045b1760"), 2085, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.5607f, 1.264857f, "Pui de Cassibrós", 26 },
                    { new Guid("c200347e-924d-4658-b1d8-a4ec18c58e56"), 1299, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.12782f, 1.35435f, "Roca del Coscolló", 4 },
                    { new Guid("c2384471-bac7-4547-b6f5-067b0a18f5bc"), 1373, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.3128f, 2.630728f, "Bassegoda", 2 },
                    { new Guid("c23d449f-e976-40a2-846c-ce7266ec4fbe"), 526, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.82556f, 1.317351f, "Tossal del Mas de Nadal", 32 },
                    { new Guid("c23f7992-b21b-4790-bec5-517f9a398ed8"), 243, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.61953f, 0.709796f, "Tossal de la Moradilla", 33 },
                    { new Guid("c29cd54a-6e51-4377-88a0-9c456459a3b1"), 2633, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.65952f, 1.193625f, "Campirme", 26 },
                    { new Guid("c2d236d8-35f6-4e4f-bb55-73f5dae06e23"), 2659, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.68406f, 0.990518f, "Tuc de la Llança", 26 },
                    { new Guid("c2e93cb4-0915-4e39-ba8b-77cdb230cc97"), 2493, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.66158f, 1.036675f, "Pic de la Plana", 26 },
                    { new Guid("c3293a08-f1ad-436c-93a6-8f129ec6091e"), 662, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.64237f, 1.941816f, "Castell de Bocs", 40 },
                    { new Guid("c36597fb-e801-4395-979c-825a7836efc6"), 1870, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.1216f, 1.787085f, "Cogulló d'Estela", 14 },
                    { new Guid("c4470810-955c-47f0-9673-2f9d52bdfea2"), 591, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.88577f, 1.014055f, "Munt de Montsonís", 23 },
                    { new Guid("c47cdf92-1375-4a4e-95cf-91ebbfb0aa7e"), 659, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.05885f, 0.500928f, "Punta Redona", 37 },
                    { new Guid("c4a83a64-de81-42ab-b352-e3401afcabc9"), 447, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.62871f, 1.886947f, "Turó de l'Escletxa (Turó de Montconill)", 7 },
                    { new Guid("c4fa7598-1295-4454-b551-cf7e51ac45a2"), 2929, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.5984f, 1.442189f, "Roca Entravessada", 100 },
                    { new Guid("c572026a-12c1-416f-94ae-5b1209a74364"), 859, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.01025f, 2.206427f, "Sant Martí Xic (Castell de Voltregà)", 24 },
                    { new Guid("c6c670c6-51d2-4697-bf7a-cbd00b8faefd"), 930, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.64995f, 1.969698f, "Turó de la Pola (o de les Tres Creus)", 7 },
                    { new Guid("c6cb4279-67c3-479d-817d-85e796a92480"), 315, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.13284f, 2.738329f, "Puig Clarà", 28 },
                    { new Guid("c6d06ffa-bd94-4ac3-b48b-7d9a74d3f207"), 2562, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.52643f, 0.878312f, "Tuc de la Comamarja", 5 },
                    { new Guid("c72431b1-a4c6-4582-9415-1666722c4d24"), 2715, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.53151f, 1.752267f, "Pic dels Pedrons", 201 },
                    { new Guid("c7470d06-ea2d-438d-a7f1-706cfef6c967"), 1846, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.27552f, 1.26702f, "Cap de Pla Redon", 4 },
                    { new Guid("c78b3f93-037a-4df2-a24b-8b0601922ff8"), 2508, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.75127f, 0.851899f, "Tuc de Somont", 39 },
                    { new Guid("c8671a52-4aa3-487d-8c64-cac737b8e8bb"), 1076, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.26631f, 1.054504f, "Puig Pelat", 8 },
                    { new Guid("c93b2fe1-915f-4cd3-bde1-0f2769472399"), 597, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.60413f, 2.496322f, "Montalt", 21 },
                    { new Guid("c9464b72-b5f1-4298-afc3-1189bbda771b"), 2934, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.65746f, 1.367073f, "Pic de Baborte", 26 },
                    { new Guid("c94a18f5-4ee0-4b76-9ebc-43b8c080868c"), 1017, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 40.90065f, 0.326554f, "Roques de Benet (el Castell)", 37 },
                    { new Guid("c98e40fa-8ea6-49f7-bc7c-a9e91ce0789a"), 2741, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.69237f, 1.364376f, "Cap de Broate", 26 },
                    { new Guid("c998c3c8-41eb-472a-ae22-3cb7527f15c3"), 499, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.10941f, 0.576417f, "La Picossa", 30 },
                    { new Guid("c9d2338e-7aa4-4651-89c9-c0ccdbe3d1c5"), 2732, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.44176f, 2.209608f, "Pic de Coma Mitjana", 203 },
                    { new Guid("c9efde68-14c8-4520-980a-183eb9198421"), 3010, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.62948f, 0.698561f, "Tuc de Molières", 39 },
                    { new Guid("cab65539-35e3-417f-b96d-207e5572fab5"), 593, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.46656f, 0.847132f, "Els Bessons", 18 },
                    { new Guid("cb6b357b-faf4-4f5e-99ee-c489f9e5e46d"), 2738, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.53635f, 1.794993f, "Pic de Font Freda", 201 },
                    { new Guid("cbfe22ba-e880-4836-8253-0e346ac95fda"), 816, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.6553f, 2.128297f, "Pic del Vent", 41 },
                    { new Guid("cf79641f-b7af-494f-bb68-90315032192c"), 2750, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.58734f, 1.570304f, "Casamanya Nord", 100 },
                    { new Guid("cf833d42-8d9d-4243-8e0a-f1ebc4541d9b"), 293, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.68303f, 0.5556f, "El Montsianell", 22 },
                    { new Guid("cfe27178-d861-4db4-a66e-aeb9f41b26a1"), 1082, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.11816f, 1.931652f, "Serrat de Migdia (Serrat de Picancel)", 14 },
                    { new Guid("d00d2eef-3606-4e37-8581-8a033797538a"), 2894, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.50949f, 0.924102f, "Gran Pic del Pessó", 5 },
                    { new Guid("d0328b09-c8a9-4fb1-98e2-cc532e95a482"), 706, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.02776f, 0.437628f, "Puig Cavaller", 37 },
                    { new Guid("d086ed40-b9d2-47e7-9d5e-f6844e9ff2fb"), 1611, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.26493f, 1.89874f, "Sant Marc de Brocà", 14 },
                    { new Guid("d1367498-8a95-4e22-9fef-d207f0e208dd"), 429, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.98711f, 0.547846f, "lo Caramull", 9 },
                    { new Guid("d1c687c4-9ecd-44e0-ba61-0212114fafde"), 2504, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.6552f, 0.840139f, "Pujoalbo", 39 },
                    { new Guid("d2307093-f441-4022-be09-91c41c8a7d87"), 2163, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.74371f, 0.663349f, "Tuc d'Arres", 39 },
                    { new Guid("d36e343f-f1cf-410d-9765-9c4acc65295e"), 2810, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.61363f, 1.984449f, "Puig Peric", 200 },
                    { new Guid("d37ec0c0-f4fd-4d92-81a2-024b6230735a"), 715, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.90299f, 0.87865f, "Mont-roig (Cim de les Altures)", 23 },
                    { new Guid("d3886449-db0f-4ec6-87f3-857ba0551383"), 2780, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.61552f, 0.788591f, "Tuc de la Contesa", 5 },
                    { new Guid("d41efe2d-0e34-4e07-9f87-4a94826b0613"), 1415, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.09666f, 1.717248f, "Roc de les Monges (Serrat de la Qüestió)", 35 },
                    { new Guid("d4be4dae-5832-4426-a3e5-1f2435b186ae"), 538, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.04179f, 0.4748f, "Mola d'Irto", 37 },
                    { new Guid("d5054b7d-4865-4156-a588-69cfa66cd764"), 2909, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.52448f, 1.027159f, "Pic de la Mainera", 25 },
                    { new Guid("d5e03ce9-6248-419d-a426-102fc179c427"), 2714, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.41548f, 2.247755f, "Gra de Fajol", 31 },
                    { new Guid("d5f80276-0189-41dc-812a-7f45a2492f64"), 2606, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.64186f, 0.765531f, "Tuc deth Port de Vielha", 39 },
                    { new Guid("d6b8e683-28df-4557-b96a-8c19f710fff0"), 1706, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 41.77649f, 2.434813f, "Turó de l'Home", 41 },
                    { new Guid("d7caf9db-967a-4b80-87c1-af89488081be"), 2903, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.60667f, 0.995373f, "Pics de Bassiero (Occidental)", 26 },
                    { new Guid("d82e7183-4850-4018-b9a8-b7398ce7d8cb"), 1351, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.18282f, 1.002793f, "Sant Corneli", 25 },
                    { new Guid("d885357a-c238-4112-9285-d31188ecea26"), 907, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.13652f, 2.49813f, "Roca Lladre", 19 },
                    { new Guid("d902cc78-9b7a-4b7d-bc77-1df117976ef3"), 2750, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.6303f, 0.828969f, "Tossau de Mar", 39 },
                    { new Guid("d902d1ce-5b77-4eba-8627-a8a3a3a0530b"), 2194, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.84613f, 0.675124f, "Tuc deth Plan deth Ome (Vacanera)", 39 },
                    { new Guid("d94da097-e292-4496-8c0c-38ed7965728d"), 526, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.32455f, 2.825785f, "Santa Magdalena", 2 },
                    { new Guid("d983fd88-3d04-42f6-af88-48710687d9f2"), 879, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.90238f, 0.413559f, "La Coscollosa", 9 },
                    { new Guid("d9b3c859-da2e-4a86-9b63-fd2f2d8dd7c1"), 543, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.74087f, 1.780965f, "Collbaix", 7 },
                    { new Guid("d9c9bc6a-e075-41b1-8dd3-75bbdf260a95"), 2449, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.52155f, 0.765144f, "Cap de Gelada", 5 },
                    { new Guid("da56f491-e369-4aef-b8f7-089b3cd9ed9a"), 1451, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.42279f, 2.727291f, "Roc del Comptador", 2 },
                    { new Guid("daa0a809-791a-4d85-938f-2c7be0e05f06"), 442, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.86129f, 2.631007f, "Turó del Vent", 34 },
                    { new Guid("dabb8d7c-7c06-4b93-8f74-bbb7452ea19b"), 2269, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.71807f, 0.696961f, "Tuc d'Era Entecada", 39 },
                    { new Guid("db87eb6e-1b8a-4d74-93cc-a37a922990e1"), 2686, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.44689f, 0.917915f, "Pic de Llena", 25 },
                    { new Guid("dc1b3280-6e8a-4970-91eb-8d2ef3bd3a33"), 1201, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.326f, 1.004194f, "Tossal de la Baltasana", 8 },
                    { new Guid("dca56994-5700-4058-99e3-4763ac2fd747"), 152, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.96575f, 0.883355f, "El Torn", 8 },
                    { new Guid("dd51ba01-5d0c-46c8-bba0-3ebcc02ee51d"), 465, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.33759f, 1.57863f, "El Castellot", 3 },
                    { new Guid("dd69325b-d3ec-4ded-86fd-c896f89d97cc"), 2802, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.52716f, 1.052679f, "les Picardes", 26 },
                    { new Guid("ddcb08b4-23be-4f83-b78c-a71461227bee"), 1675, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.03986f, 0.766568f, "Penya Sant Alís", 23 },
                    { new Guid("ddd6797a-4c07-4616-9c3f-ca2eb60ffb77"), 1523, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.21453f, 1.267256f, "Penya Aguda", 4 },
                    { new Guid("dde2e31a-a5de-4f56-9b2d-d7be5fc61455"), 2870, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.56599f, 1.043251f, "Pui de Linya", 26 },
                    { new Guid("de379061-6534-4b72-8010-114dc80bc704"), 2141, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.40455f, 1.632949f, "Mirador del Pla de Llet", 4 },
                    { new Guid("dea5cf52-e2f1-48d9-b56a-1786daa1decf"), 1976, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.36338f, 2.318744f, "Puig de les Agudes", 31 },
                    { new Guid("ded5b31a-673a-4814-8aa8-60e1ca7a2d0d"), 2585, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.37011f, 2.219566f, "Balandrau", 31 },
                    { new Guid("df829a3a-83e9-4a2d-b769-59ee3b34edf6"), 483, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.9848f, 2.908628f, "Els Àngels", 20 },
                    { new Guid("e1a9c73d-9428-4dd8-b028-5ba547200860"), 2766, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.71894f, 1.049931f, "Pic de Moredo", 26 },
                    { new Guid("e2207652-cc6a-4885-aad7-2f2f95e14050"), 450, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.41219f, 2.054991f, "Puig d'Olorda", 11 },
                    { new Guid("e38433a7-6040-441e-8bd2-5682dbb77298"), 2639, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.69981f, 1.054304f, "Pic de Qüenca", 26 },
                    { new Guid("e3cb63cd-f50d-4a55-a7c1-d6cd57fb47bb"), 2137, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.51318f, 1.990995f, "Pic dels Moros", 201 },
                    { new Guid("e43e1c84-54d5-41d2-9134-b819f1195934"), 2802, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.78857f, 1.009844f, "Tuc de Barlonguèra", 39 },
                    { new Guid("e4780107-c02e-4f42-bb38-d3bf6c775663"), 394, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 40.9413f, 0.714712f, "Tossal de Montagut", 9 },
                    { new Guid("e51bd03a-fd30-462c-8765-3a0ef6e5c52b"), 1235, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.42735f, 2.69458f, "Roc de Sant Salvador", 204 },
                    { new Guid("e57e9249-2585-4a58-ae4c-c10d63de3b65"), 3014, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.5857f, 0.880287f, "Punta Alta", 5 },
                    { new Guid("e61b27b4-e84b-4572-8238-d94c60bc74d3"), 551, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.30117f, 1.458887f, "Puig Francàs", 12 },
                    { new Guid("e63ed590-52be-4aeb-9a40-3c0ef99d0bcc"), 2734, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.58374f, 0.970147f, "Pic del Portarró", 26 },
                    { new Guid("e678b78b-f766-4f4d-a2ff-8bb82756a678"), 1843, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.70858f, 2.26614f, "Tuc Dormidor", 203 },
                    { new Guid("e6a51c72-aa7b-4d27-982b-5c64931a70d4"), 1621, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.15115f, 1.347278f, "Cogulló de Turp", 4 },
                    { new Guid("e6e64b9b-b01a-447e-8334-028e44d099d9"), 1471, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.02627f, 0.912398f, "lo Peladet", 23 },
                    { new Guid("e6ef760e-f644-4bd2-aaaa-b21a28fba5ef"), 919, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.09263f, 0.862623f, "Miranda de Llaberia", 8 },
                    { new Guid("e73b868f-2074-4a79-a821-a6fb0fb97ca3"), 2740, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.45798f, 1.727174f, "La Carabassa", 15 },
                    { new Guid("e7c5f30f-2213-492e-8e16-4a702fbb18bb"), 2067, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.30555f, 2.002298f, "la Creueta", 14 },
                    { new Guid("e8ff2258-cba7-4d48-b0f2-cd48cecc3b70"), 842, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.95209f, 2.210461f, "Creu de Gurb", 24 },
                    { new Guid("e961b51b-1828-4e2e-81c1-5ba557adafdf"), 2276, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, true, 42.3063f, 1.842475f, "Penyes Altes", 14 },
                    { new Guid("e981fbc5-c7e1-4a15-a1fe-26ee5bee6d38"), 2842, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.5885f, 1.87309f, "Puig Pedrós de Lanós", 201 },
                    { new Guid("ea7bf66f-86c1-49e1-8f4f-0a6357494a51"), 1425, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.22797f, 0.814206f, "Tossal de Codonyac", 25 },
                    { new Guid("eabb072f-b295-4697-9210-bedaf1ebae00"), 1870, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.20793f, 1.568135f, "Roca de Migdia (Serra del Verd)", 4 },
                    { new Guid("ead9415c-b246-48d7-89e7-a2fff28e5194"), 2805, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.61482f, 1.477427f, "Pic de Cataperdís", 100 },
                    { new Guid("eae4c703-c090-47e4-a055-43bada5b8aa5"), 682, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.99049f, 1.843372f, "Serrat de la Madrona", 14 },
                    { new Guid("ebe7f5d3-1f33-4e65-89b4-1621bf880353"), 1938, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.13404f, 1.497651f, "Puig Sobirà", 35 },
                    { new Guid("ed3f1fc5-d7fb-4a85-815e-58ec5570315b"), 867, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.37535f, 1.239387f, "El Tossal Gros", 1 },
                    { new Guid("ed44cdb3-9637-4b55-8b31-8d79e3946a82"), 2833, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.63333f, 0.875353f, "Montardo", 39 },
                    { new Guid("ed79fec3-4525-4fe3-aaad-cd7e4e175e5c"), 2915, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.60373f, 1.441992f, "Pic de Medacorba", 100 },
                    { new Guid("ee7ffd76-5d48-4cbd-99d0-262780cedf9e"), 1056, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.29431f, 2.515424f, "Puig de Bestrecà", 31 },
                    { new Guid("eef3884e-ee54-48f9-8fb0-bd1a24f8ffba"), 2649, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, true, 42.28578f, 1.636429f, "Vulturó", 4 },
                    { new Guid("f07854af-dadf-4903-aaee-f0b6b12f900f"), 498, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.80541f, 2.761806f, "Torre del Far", 202 },
                    { new Guid("f1049deb-eab1-4024-9df5-84a181590b52"), 2826, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.58356f, 1.873621f, "Puig de Coma d'Or", 201 },
                    { new Guid("f127e074-cfca-421b-835b-4f17c60c8a97"), 2536, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.74286f, 0.875684f, "Tuc dera Pincèla", 39 },
                    { new Guid("f1ecf49f-cafc-4f82-967c-c27efc7f44c8"), 1125, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.25885f, 2.706297f, "El Mont", 2 },
                    { new Guid("f2c5391c-f784-481c-be99-24663fbfbcfd"), 524, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.31533f, 1.191293f, "Puigcabrer", 1 },
                    { new Guid("f3d77115-f53f-429b-9207-72f5f098344a"), 1022, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.3553f, 0.964329f, "Punta del Curull", 16 },
                    { new Guid("f4659895-2672-4237-bcac-5c3b29fe8a09"), 2763, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.61721f, 1.959298f, "Puig de la Cometa", 201 },
                    { new Guid("f4f9bce0-c193-4fa8-868e-b71f10ad06a6"), 2732, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.63091f, 1.468022f, "Pic de Cabanyó", 100 },
                    { new Guid("f52ef253-c5ec-4384-b65f-3a7db66d1472"), 2760, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.61916f, 1.664295f, "Pic de la Coma de Varilles", 100 },
                    { new Guid("f53d3d8b-c80c-4515-bc66-07ba1653b292"), 2588, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.61171f, 1.709588f, "Alt de Juclar", 100 },
                    { new Guid("f64bddcc-fd7d-4fdf-8a5c-17829d5acc65"), 2226, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 2, false, 42.47118f, 1.248309f, "Pui d'Urdosa", 26 },
                    { new Guid("f765751c-4f2e-4c3c-b9ed-0c488941984e"), 1030, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.12394f, 2.251122f, "Castell de Besora", 24 },
                    { new Guid("f7726c56-11de-4156-b4a3-1cfa5b9b3981"), 2834, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.52778f, 1.670181f, "Pic Alt del Cubil", 100 },
                    { new Guid("f79bc4c1-a719-4a53-9344-468ab97be564"), 1391, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.32449f, 2.555498f, "Puig de les Bruixes", 2 },
                    { new Guid("f7ba76ae-435f-48c5-a614-e1a59640181c"), 2517, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.50573f, 1.328839f, "Pic de Màniga", 26 },
                    { new Guid("f7fd7578-3630-4eed-8411-b03a2b8ad25f"), 740, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.89905f, 0.892467f, "Tossal de Sant Jordi", 23 },
                    { new Guid("f9f509a8-3930-473b-a845-15c2ccf9b2bc"), 672, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.31511f, 1.475108f, "Puig de la Cova", 12 },
                    { new Guid("faa2136e-6fa9-41af-9a13-7dfa94024e85"), 633, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.78277f, 2.540643f, "Turó de Montsoriu", 34 },
                    { new Guid("faa737f8-ce05-418b-8917-85f695355dd3"), 711, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.01629f, 0.785435f, "Mola de Genesies", 8 },
                    { new Guid("faff830f-cae8-411f-abd2-2bd825c84f47"), 385, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 42.0069f, 2.862092f, "Castell de Sant Miquel", 20 },
                    { new Guid("fba5435e-b924-41a9-9d9a-4ef655717300"), 443, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 42.616f, 2.705595f, "Roc de Mallorca", 202 },
                    { new Guid("fc14d57c-eb4a-40ac-be03-3b60757ab146"), 2782, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 3, false, 42.71274f, 1.261402f, "Pic de Flamisella", 26 },
                    { new Guid("fd99b392-f2a3-4755-9839-f916813bf1e7"), 641, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, false, 41.12702f, 0.914951f, "Santa Bàrbara (Escornalbou)", 8 },
                    { new Guid("ff1df0df-4546-4ebd-a3c5-6bc502b4eef4"), 1010, new Guid("3a711b1c-a40a-48b2-88e9-c1677591d546"), 1, true, 41.8758f, 2.108833f, "Puig de la Caritat", 42 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Climbs_DiaryId",
                table: "Climbs",
                column: "DiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Climbs_SummitId",
                table: "Climbs",
                column: "SummitId");

            migrationBuilder.CreateIndex(
                name: "IX_Diaries_CatalogueId",
                table: "Diaries",
                column: "CatalogueId");

            migrationBuilder.CreateIndex(
                name: "IX_Diaries_HikerId",
                table: "Diaries",
                column: "HikerId");

            migrationBuilder.CreateIndex(
                name: "IX_Summits_CatalogueId",
                table: "Summits",
                column: "CatalogueId");

            migrationBuilder.CreateIndex(
                name: "IX_Summits_DifficultyLevelId",
                table: "Summits",
                column: "DifficultyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Summits_Name",
                table: "Summits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Summits_RegionId",
                table: "Summits",
                column: "RegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Climbs");

            migrationBuilder.DropTable(
                name: "Diaries");

            migrationBuilder.DropTable(
                name: "Summits");

            migrationBuilder.DropTable(
                name: "Hikers");

            migrationBuilder.DropTable(
                name: "Catalogues");

            migrationBuilder.DropTable(
                name: "EnumLookup<DifficultyLevel>");

            migrationBuilder.DropTable(
                name: "EnumLookup<Region>");
        }
    }
}
