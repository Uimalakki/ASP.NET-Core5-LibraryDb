using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryApi.Migrations
{
    public partial class ChangedTableNameForBookCollections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCollection_Books_BookId",
                table: "BookCollection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookCollection",
                table: "BookCollection");

            migrationBuilder.RenameTable(
                name: "BookCollection",
                newName: "BookCollections");

            migrationBuilder.RenameIndex(
                name: "IX_BookCollection_BookId",
                table: "BookCollections",
                newName: "IX_BookCollections_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookCollections",
                table: "BookCollections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCollections_Books_BookId",
                table: "BookCollections",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCollections_Books_BookId",
                table: "BookCollections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookCollections",
                table: "BookCollections");

            migrationBuilder.RenameTable(
                name: "BookCollections",
                newName: "BookCollection");

            migrationBuilder.RenameIndex(
                name: "IX_BookCollections_BookId",
                table: "BookCollection",
                newName: "IX_BookCollection_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookCollection",
                table: "BookCollection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCollection_Books_BookId",
                table: "BookCollection",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
