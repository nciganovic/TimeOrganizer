using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeOrganizer.Migrations
{
    public partial class AddTaskTypesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_School_SchoolId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_School_SchoolTypes_SchoolTypeId",
                table: "School");

            migrationBuilder.DropPrimaryKey(
                name: "PK_School",
                table: "School");

            migrationBuilder.RenameTable(
                name: "School",
                newName: "Schools");

            migrationBuilder.RenameIndex(
                name: "IX_School_SchoolTypeId",
                table: "Schools",
                newName: "IX_Schools_SchoolTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schools",
                table: "Schools",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TaskTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Schools_SchoolId",
                table: "AspNetUsers",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_SchoolTypes_SchoolTypeId",
                table: "Schools",
                column: "SchoolTypeId",
                principalTable: "SchoolTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Schools_SchoolId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_SchoolTypes_SchoolTypeId",
                table: "Schools");

            migrationBuilder.DropTable(
                name: "TaskTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schools",
                table: "Schools");

            migrationBuilder.RenameTable(
                name: "Schools",
                newName: "School");

            migrationBuilder.RenameIndex(
                name: "IX_Schools_SchoolTypeId",
                table: "School",
                newName: "IX_School_SchoolTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_School",
                table: "School",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_School_SchoolId",
                table: "AspNetUsers",
                column: "SchoolId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_School_SchoolTypes_SchoolTypeId",
                table: "School",
                column: "SchoolTypeId",
                principalTable: "SchoolTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
