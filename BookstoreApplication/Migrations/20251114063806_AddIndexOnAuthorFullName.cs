using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookstoreApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexOnAuthorFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Authors_FullName",
                table: "Authors",
                column: "FullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Authors_FullName",
                table: "Authors");
        }
    }
}
