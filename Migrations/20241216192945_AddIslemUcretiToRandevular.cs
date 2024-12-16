using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonYonetimUygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class AddIslemUcretiToRandevular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalısanID",
                table: "Randevular");

			migrationBuilder.AddColumn<double>(
	   name: "IslemUcreti",
	   table: "Randevular",
	   type: "float",
	   nullable: false,
	   defaultValue: 0.0);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalısanID",
                table: "Randevular",
                type: "int",
                nullable: false,
                defaultValue: 0);

			migrationBuilder.DropColumn(
		name: "IslemUcreti",
		table: "Randevular");
		}
    }
}
