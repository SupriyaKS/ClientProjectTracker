using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientProjectTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerIdToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_OwnerId",
                table: "Clients",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_AspNetUsers_OwnerId",
                table: "Clients",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_AspNetUsers_OwnerId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_OwnerId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Clients");
        }
    }
}
