using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionOrdinateurs.API.Migrations
{
    /// <inheritdoc />
    public partial class DBUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ordinateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Marque = table.Column<string>(type: "TEXT", nullable: false),
                    Etat = table.Column<int>(type: "INTEGER", nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    Prix = table.Column<int>(type: "INTEGER", nullable: false),
                    Caracteristiques_RamGo = table.Column<int>(type: "INTEGER", nullable: false),
                    Caracteristiques_TypeOrdi = table.Column<int>(type: "INTEGER", nullable: false),
                    Caracteristiques_StockageGo = table.Column<int>(type: "INTEGER", nullable: false),
                    Vendu = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordinateurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reparations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrdinateurId = table.Column<int>(type: "INTEGER", nullable: false),
                    UnOrdinateurId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeReparation = table.Column<string>(type: "TEXT", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Statut = table.Column<string>(type: "TEXT", nullable: false),
                    Technicien = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reparations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reparations_Ordinateurs_UnOrdinateurId",
                        column: x => x.UnOrdinateurId,
                        principalTable: "Ordinateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reparations_UnOrdinateurId",
                table: "Reparations",
                column: "UnOrdinateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_Login",
                table: "Utilisateurs",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reparations");

            migrationBuilder.DropTable(
                name: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "Ordinateurs");
        }
    }
}
