using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonYonetimUygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIslemAndUcretFromRandevular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropColumn(
		   name: "Islem",
		   table: "Randevular");

			migrationBuilder.DropColumn(
				name: "Ucret",
				table: "Randevular");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<string>(
			name: "Islem",
			table: "Randevular",
			type: "nvarchar(max)",
			nullable: true);

			migrationBuilder.AddColumn<double>(
				name: "Ucret",
				table: "Randevular",
				type: "float",
				nullable: false,
				defaultValue: 0.0);
		}
    }
}
