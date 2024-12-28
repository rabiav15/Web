using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonYonetimUygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRandevularTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Tarih",
                table: "Randevular",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Randevular",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Randevular",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SalonID",
                table: "Randevular",
                type: "int",
                nullable: false,
                defaultValue: 0);

           

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Randevular",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_SalonId",
                table: "Randevular",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_UserId",
                table: "Randevular",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Salonlar_SalonID",
                table: "Randevular",
                column: "SalonID",
                principalTable: "Salonlar",
                principalColumn: "SalonID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_User_UserId",
                table: "Randevular",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Salonlar_SalonId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_User_UserId",
                table: "Randevular");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Randevular_SalonId",
                table: "Randevular");

            migrationBuilder.DropIndex(
                name: "IX_Randevular_UserId",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "SalonID",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "SalonId",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Randevular");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Tarih",
                table: "Randevular",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
