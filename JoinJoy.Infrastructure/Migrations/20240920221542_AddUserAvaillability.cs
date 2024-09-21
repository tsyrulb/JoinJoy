using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoinJoy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAvaillability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Activities_ActivityId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_UserId2",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "UserAvailabilities");

            migrationBuilder.DropTable(
                name: "Availabilities");

            migrationBuilder.DropIndex(
                name: "IX_Matches_UserId2",
                table: "Matches");

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

            migrationBuilder.AddColumn<int>(
                name: "User2Id",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Conversations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_User2Id",
                table: "Matches",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Activities_ActivityId",
                table: "Matches",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_User2Id",
                table: "Matches",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Activities_ActivityId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_User2Id",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_User2Id",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "UnavailableDay",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UnavailableEndTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UnavailableStartTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "User2Id",
                table: "Matches");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Conversations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Availabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availabilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAvailabilities",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AvailabilityId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAvailabilities", x => new { x.UserId, x.AvailabilityId });
                    table.ForeignKey(
                        name: "FK_UserAvailabilities_Availabilities_AvailabilityId",
                        column: x => x.AvailabilityId,
                        principalTable: "Availabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAvailabilities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_UserId2",
                table: "Matches",
                column: "UserId2");

            migrationBuilder.CreateIndex(
                name: "IX_UserAvailabilities_AvailabilityId",
                table: "UserAvailabilities",
                column: "AvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Activities_ActivityId",
                table: "Matches",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_UserId2",
                table: "Matches",
                column: "UserId2",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
