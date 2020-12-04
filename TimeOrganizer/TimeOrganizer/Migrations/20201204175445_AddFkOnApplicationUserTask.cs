using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeOrganizer.Migrations
{
    public partial class AddFkOnApplicationUserTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelationshipStatusId",
                table: "ApplicationUserTask",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTask_RelationshipStatusId",
                table: "ApplicationUserTask",
                column: "RelationshipStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserTask_RelationshipStatuses_RelationshipStatusId",
                table: "ApplicationUserTask",
                column: "RelationshipStatusId",
                principalTable: "RelationshipStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserTask_RelationshipStatuses_RelationshipStatusId",
                table: "ApplicationUserTask");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserTask_RelationshipStatusId",
                table: "ApplicationUserTask");

            migrationBuilder.DropColumn(
                name: "RelationshipStatusId",
                table: "ApplicationUserTask");
        }
    }
}
