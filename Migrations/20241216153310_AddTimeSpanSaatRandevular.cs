using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonYonetimUygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeSpanSaatRandevular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Saat",
                table: "Randevular",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Saat",
                table: "Randevular");
        }
    }
}
