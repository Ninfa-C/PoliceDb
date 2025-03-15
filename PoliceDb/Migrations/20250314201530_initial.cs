using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PoliceDb.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anagrafiche",
                columns: table => new
                {
                    IdAnagrafica = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Indirizzo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Città = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CAP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CF = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anagrafiche", x => x.IdAnagrafica);
                });

            migrationBuilder.CreateTable(
                name: "TipiViolazione",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipiViolazione", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Verbali",
                columns: table => new
                {
                    IdVerbale = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAnagrafica = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataViolazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IndirizzoViolazione = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NominativoAgente = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DataTrascrizioneVerbale = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotaleImporto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotaleDecurtamentoPunti = table.Column<int>(type: "int", nullable: false),
                    ScadenzaContestazione = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verbali", x => x.IdVerbale);
                    table.ForeignKey(
                        name: "FK_Verbali_Anagrafiche_IdAnagrafica",
                        column: x => x.IdAnagrafica,
                        principalTable: "Anagrafiche",
                        principalColumn: "IdAnagrafica",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViolazioniVerbali",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdVerbale = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdViolazione = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Importo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DecurtamentoPunti = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViolazioniVerbali", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViolazioniVerbali_TipiViolazione_IdViolazione",
                        column: x => x.IdViolazione,
                        principalTable: "TipiViolazione",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ViolazioniVerbali_Verbali_IdVerbale",
                        column: x => x.IdVerbale,
                        principalTable: "Verbali",
                        principalColumn: "IdVerbale",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Anagrafiche",
                columns: new[] { "IdAnagrafica", "CAP", "CF", "Città", "Cognome", "Indirizzo", "Nome" },
                values: new object[,]
                {
                    { new Guid("a3208b53-dba6-4857-95b4-264cc4209d3b"), "80121", "GLLFRN95E01C123U", "Napoli", "Gialli", "Viale Libertà 15", "Francesco" },
                    { new Guid("b7bcfd07-7f8b-47e7-b636-8c6b351a0d0f"), "40123", "NRAANN75D61G312T", "Bologna", "Neri", "Via Garibaldi 8", "Anna" },
                    { new Guid("b95d59ac-702d-4b32-bc59-e0f25f5f576b"), "50122", "VRDGLI90C41D612K", "Firenze", "Verdi", "Piazza Duomo 3", "Giulia" },
                    { new Guid("c1fcded1-0f9d-45c6-8c78-64f84600fa1a"), "20121", "RSSMRA80A01H501Z", "Milano", "Rossi", "Via Roma 10", "Mario" },
                    { new Guid("e8d5c12f-b41f-4748-9d3d-d6f2387582c4"), "10121", "BNCPLC85B01F205X", "Torino", "Bianchi", "Corso Venezia 5", "Luca" }
                });

            migrationBuilder.InsertData(
                table: "TipiViolazione",
                columns: new[] { "Id", "Descrizione" },
                values: new object[,]
                {
                    { new Guid("b51a10bc-e722-45a5-9357-b81b75f2d387"), "Guida senza cintura di sicurezza" },
                    { new Guid("bd3d24c4-c47a-4772-8bf4-13fecac97cb4"), "Guida in stato di ebbrezza" },
                    { new Guid("c6d1d5b0-7bfa-4c2b-bd92-7bbed7e6f001"), "Passaggio con il semaforo rosso" },
                    { new Guid("d2a2c31f-ea47-4c69-a4c8-d9f3f8c2a2f3"), "Eccesso di velocità" },
                    { new Guid("f6a06f68-ea4c-4ad7-9b10-8ef0ed2c93d7"), "Utilizzo del cellulare alla guida" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anagrafiche_CF",
                table: "Anagrafiche",
                column: "CF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Verbali_IdAnagrafica",
                table: "Verbali",
                column: "IdAnagrafica");

            migrationBuilder.CreateIndex(
                name: "IX_ViolazioniVerbali_IdVerbale_IdViolazione",
                table: "ViolazioniVerbali",
                columns: new[] { "IdVerbale", "IdViolazione" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ViolazioniVerbali_IdViolazione",
                table: "ViolazioniVerbali",
                column: "IdViolazione");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViolazioniVerbali");

            migrationBuilder.DropTable(
                name: "TipiViolazione");

            migrationBuilder.DropTable(
                name: "Verbali");

            migrationBuilder.DropTable(
                name: "Anagrafiche");
        }
    }
}
