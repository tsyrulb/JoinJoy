using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoinJoy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PreferedDestinationsRemovedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPreferredDestinations");

            migrationBuilder.DropTable(
                name: "PreferredDestinations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreferredDestinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferredDestinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferredDestinations",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PreferredDestinationId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferredDestinations", x => new { x.UserId, x.PreferredDestinationId });
                    table.ForeignKey(
                        name: "FK_UserPreferredDestinations_PreferredDestinations_PreferredDestinationId",
                        column: x => x.PreferredDestinationId,
                        principalTable: "PreferredDestinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPreferredDestinations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferredDestinations_PreferredDestinationId",
                table: "UserPreferredDestinations",
                column: "PreferredDestinationId");
        }
    }
}
