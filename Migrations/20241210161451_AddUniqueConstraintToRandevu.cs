using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonYonetimUygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToRandevu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateIndex(
		name: "IX_Randevular_CalisanID_Tarih",
		table: "Randevular",
		columns: new[] { "CalisanID", "Tarih" },
		unique: true);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropIndex(
		name: "IX_Randevular_CalisanID_Tarih",
		table: "Randevular");

		}
    }
}
