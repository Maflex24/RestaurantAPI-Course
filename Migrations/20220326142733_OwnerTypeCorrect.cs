using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantAPI.Migrations
{
    public partial class OwnerTypeCorrect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_ownerId",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "ownerId",
                table: "Restaurants",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_ownerId",
                table: "Restaurants",
                newName: "IX_Restaurants_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_OwnerId",
                table: "Restaurants",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_OwnerId",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Restaurants",
                newName: "ownerId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants",
                newName: "IX_Restaurants_ownerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_ownerId",
                table: "Restaurants",
                column: "ownerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
