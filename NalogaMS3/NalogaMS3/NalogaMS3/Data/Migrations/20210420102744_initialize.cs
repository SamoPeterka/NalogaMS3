using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NalogaMS3.Data.Migrations
{
    public partial class initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DomaceNaloge",
                columns: table => new
                {
                    NalogaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naslov = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RokZaOddajo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomaceNaloge", x => x.NalogaID);
                });

            migrationBuilder.CreateTable(
                name: "Studenti",
                columns: table => new
                {
                    Student_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priimek = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenti", x => x.Student_ID);
                });

            migrationBuilder.CreateTable(
                name: "Studenti_Naloge",
                columns: table => new
                {
                    Student_NalogaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Student_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naloga_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatumOddaje = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ocena = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenti_Naloge", x => x.Student_NalogaID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomaceNaloge");

            migrationBuilder.DropTable(
                name: "Studenti");

            migrationBuilder.DropTable(
                name: "Studenti_Naloge");
        }
    }
}
