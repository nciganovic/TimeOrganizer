using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeOrganizer.Migrations
{
    public partial class AddUserRelationshipTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRelationships",
                columns: table => new
                {
                    ApplicationUserId_Sender = table.Column<string>(nullable: false),
                    ApplicationUserId_Reciver = table.Column<string>(nullable: false),
                    RelationshipStatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRelationships", x => new { x.ApplicationUserId_Sender, x.ApplicationUserId_Reciver });
                    table.ForeignKey(
                        name: "FK_UserRelationships_AspNetUsers_ApplicationUserId_Reciver",
                        column: x => x.ApplicationUserId_Reciver,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRelationships_AspNetUsers_ApplicationUserId_Sender",
                        column: x => x.ApplicationUserId_Sender,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRelationships_RelationshipStatuses_RelationshipStatusId",
                        column: x => x.RelationshipStatusId,
                        principalTable: "RelationshipStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRelationships_ApplicationUserId_Reciver",
                table: "UserRelationships",
                column: "ApplicationUserId_Reciver");

            migrationBuilder.CreateIndex(
                name: "IX_UserRelationships_RelationshipStatusId",
                table: "UserRelationships",
                column: "RelationshipStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRelationships");
        }
    }
}
