using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoinJoy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserUnavailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnavailableDay",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UnavailableEndTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UnavailableStartTime",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserUnavailabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUnavailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserUnavailabilities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserUnavailabilities_UserId",
                table: "UserUnavailabilities",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserUnavailabilities");

            migrationBuilder.AddColumn<int>(
                name: "UnavailableDay",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "UnavailableEndTime",
                table: "Users",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "UnavailableStartTime",
                table: "Users",
                type: "time",
                nullable: true);
        }
    }
}
