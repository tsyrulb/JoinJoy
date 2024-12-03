using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoinJoy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConversationActivityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ConversationId",
                table: "Activities",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Conversations_ConversationId",
                table: "Activities",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Conversations_ConversationId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ConversationId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Activities");
        }
    }
}
