using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonYonetimUygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRandevularConstratins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<int>(
		name: "CalisanID",
		table: "Randevular",
		type: "int",
		nullable: false,
		defaultValue: 0);

			migrationBuilder.AlterColumn<int>(
				name: "IslemID",
				table: "Randevular",
				type: "int",
				nullable: false,
				defaultValue: 0);

		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
