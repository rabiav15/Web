using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonYonetimUygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salonlar",
                columns: table => new
                {
                    SalonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalonAd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalonAdres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalonTelefon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salonlar", x => x.SalonID);
                });

            migrationBuilder.CreateTable(
                name: "Calisanlar",
                columns: table => new
                {
                    CalisanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalisanAd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalisanSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UzmanlikAlani = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalonID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calisanlar", x => x.CalisanID);
                    table.ForeignKey(
                        name: "FK_Calisanlar_Salonlar_SalonID",
                        column: x => x.SalonID,
                        principalTable: "Salonlar",
                        principalColumn: "SalonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calisanlar_SalonID",
                table: "Calisanlar",
                column: "SalonID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calisanlar");

            migrationBuilder.DropTable(
                name: "Salonlar");
        }
    }
}
