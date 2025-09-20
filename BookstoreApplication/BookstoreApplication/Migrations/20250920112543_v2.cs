using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookstoreApplication.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorAwards_Authors_AuthorId",
                table: "AuthorAwards");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorAwards_Awards_AwardId",
                table: "AuthorAwards");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorAwards",
                table: "AuthorAwards");

            migrationBuilder.RenameTable(
                name: "AuthorAwards",
                newName: "AuthorAwardBridge");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Authors",
                newName: "Birthday");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorAwards_AwardId",
                table: "AuthorAwardBridge",
                newName: "IX_AuthorAwardBridge_AwardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorAwardBridge",
                table: "AuthorAwardBridge",
                columns: new[] { "AuthorId", "AwardId", "YearAwarded" });

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorAwardBridge_Authors_AuthorId",
                table: "AuthorAwardBridge",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorAwardBridge_Awards_AwardId",
                table: "AuthorAwardBridge",
                column: "AwardId",
                principalTable: "Awards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorAwardBridge_Authors_AuthorId",
                table: "AuthorAwardBridge");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorAwardBridge_Awards_AwardId",
                table: "AuthorAwardBridge");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorAwardBridge",
                table: "AuthorAwardBridge");

            migrationBuilder.RenameTable(
                name: "AuthorAwardBridge",
                newName: "AuthorAwards");

            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "Authors",
                newName: "DateOfBirth");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorAwardBridge_AwardId",
                table: "AuthorAwards",
                newName: "IX_AuthorAwards_AwardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorAwards",
                table: "AuthorAwards",
                columns: new[] { "AuthorId", "AwardId", "YearAwarded" });

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorAwards_Authors_AuthorId",
                table: "AuthorAwards",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorAwards_Awards_AwardId",
                table: "AuthorAwards",
                column: "AwardId",
                principalTable: "Awards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
