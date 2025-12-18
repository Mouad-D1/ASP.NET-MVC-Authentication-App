using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplicationAchats.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategorieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategorieId);
                });

            migrationBuilder.CreateTable(
                name: "CLIENT",
                columns: table => new
                {
                    NUMCLI = table.Column<int>(type: "int", nullable: false),
                    NOMCLI = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    VILLE = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CATEGORIE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    COMPTE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CLIENT__64C8A28346C65F99", x => x.NUMCLI);
                });

            migrationBuilder.CreateTable(
                name: "Marques",
                columns: table => new
                {
                    MarqueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marques", x => x.MarqueId);
                });

            migrationBuilder.CreateTable(
                name: "CLIENTAdress",
                columns: table => new
                {
                    NUMCLI = table.Column<int>(type: "int", nullable: false),
                    ADDRESS1 = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    STATE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ClientAddr", x => x.NUMCLI);
                    table.ForeignKey(
                        name: "fk_ClientAddr_Client",
                        column: x => x.NUMCLI,
                        principalTable: "CLIENT",
                        principalColumn: "NUMCLI");
                });

            migrationBuilder.CreateTable(
                name: "COMMANDE",
                columns: table => new
                {
                    NUMCOM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NUMCLI = table.Column<int>(type: "int", nullable: false),
                    DATECOM = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_commande", x => x.NUMCOM);
                    table.ForeignKey(
                        name: "fk_commande_client",
                        column: x => x.NUMCLI,
                        principalTable: "CLIENT",
                        principalColumn: "NUMCLI");
                });

            migrationBuilder.CreateTable(
                name: "produits",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    prix = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    quantite = table.Column<int>(type: "int", nullable: false),
                    DATE_AJOUT = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "(getdate())"),
                    DISPONIBLE = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CategorieId = table.Column<int>(type: "int", nullable: true),
                    MarqueId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__produits__3213E83F6174ADA6", x => x.id);
                    table.ForeignKey(
                        name: "FK_produits_Categories_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "Categories",
                        principalColumn: "CategorieId");
                    table.ForeignKey(
                        name: "FK_produits_Marques_MarqueId",
                        column: x => x.MarqueId,
                        principalTable: "Marques",
                        principalColumn: "MarqueId");
                });

            migrationBuilder.CreateTable(
                name: "DETAIL_COMMANDE",
                columns: table => new
                {
                    ID_DETAIL = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_COMMANDE = table.Column<int>(type: "int", nullable: false),
                    ID_PRODUIT = table.Column<int>(type: "int", nullable: false),
                    QUANTITE = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_detail", x => x.ID_DETAIL);
                    table.ForeignKey(
                        name: "fk_detail_commande",
                        column: x => x.ID_COMMANDE,
                        principalTable: "COMMANDE",
                        principalColumn: "NUMCOM");
                    table.ForeignKey(
                        name: "fk_detail_produit",
                        column: x => x.ID_PRODUIT,
                        principalTable: "produits",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_COMMANDE_NUMCLI",
                table: "COMMANDE",
                column: "NUMCLI");

            migrationBuilder.CreateIndex(
                name: "IX_DETAIL_COMMANDE_ID_COMMANDE",
                table: "DETAIL_COMMANDE",
                column: "ID_COMMANDE");

            migrationBuilder.CreateIndex(
                name: "IX_DETAIL_COMMANDE_ID_PRODUIT",
                table: "DETAIL_COMMANDE",
                column: "ID_PRODUIT");

            migrationBuilder.CreateIndex(
                name: "IX_produits_CategorieId",
                table: "produits",
                column: "CategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_produits_MarqueId",
                table: "produits",
                column: "MarqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CLIENTAdress");

            migrationBuilder.DropTable(
                name: "DETAIL_COMMANDE");

            migrationBuilder.DropTable(
                name: "COMMANDE");

            migrationBuilder.DropTable(
                name: "produits");

            migrationBuilder.DropTable(
                name: "CLIENT");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Marques");
        }
    }
}
