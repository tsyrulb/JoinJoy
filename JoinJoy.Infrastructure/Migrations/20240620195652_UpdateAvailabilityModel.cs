using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoinJoy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAvailabilityModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysOfWeek",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "Seasons",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "TimeOfDay",
                table: "Availabilities");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Availabilities",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Availabilities",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Availabilities");

            migrationBuilder.AddColumn<string>(
                name: "DaysOfWeek",
                table: "Availabilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Seasons",
                table: "Availabilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeOfDay",
                table: "Availabilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
