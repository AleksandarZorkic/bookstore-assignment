using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookstoreApplication.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ae4637db-259c-4f3a-b59f-26fa28a362f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1ba13ab-e4df-4299-a734-77e183cf91c2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "role-biblio-id", "cs1", "Bibliotekar", "BIBLIOTEKAR" },
                    { "role-urednik-id", "cs2", "Urednik", "UREDNIK" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user-biblio-1", 0, "07636b11-9fd5-437d-a9f7-a2374af6aacf", "biblio1@biblioteka.com", true, false, null, null, "BIBLIO1@BIBLIOTEKA.COM", "BIBLIO1", "AQAAAAIAAYagAAAAEBhOFhUG/S2uCEetAY5/ImfdCtPFtxu+28R05wK9NJO/GX22MFPBn7y2ppdThe56QA==", null, false, "b609a2b5-128f-43f8-8720-ed7710073ed2", null, false, "biblio1" },
                    { "user-urednik-1", 0, "b535b366-ea99-448e-ad9a-fdd70e579f27", "urednik1@biblioteka.com", true, false, null, "Urednik", "UREDNIK1@BIBLIOTEKA.COM", "UREDNIK1", "AQAAAAIAAYagAAAAEACiMOTSBAIcnQk9wEBEA/qdrm3UemBBof9VgRrlvEcniJCDlriPbIvHTUWYpg5kuQ==", null, false, "9e8b281e-fbc2-4cd1-aab5-a02df6166468", "Prvi", false, "urednik1" },
                    { "user-urednik-2", 0, "5a1d86b1-39b7-463e-9b13-218c9023c82b", "urednik2@biblioteka.com", true, false, null, null, "UREDNIK2@BIBLIOTEKA.COM", "UREDNIK2", "AQAAAAIAAYagAAAAELquf/7MEr6z/q/HjUr4YVhSCZhc1vVfEAMdIcxcVYjwPatpn1luBCzTkHVAjWpdDQ==", null, false, "ada5ff65-a5c2-4309-8643-bb98c648f5fb", null, false, "urednik2" },
                    { "user-urednik-3", 0, "52da9751-5bef-4fe6-9715-0a8699ea733a", "urednik3@biblioteka.com", true, false, null, null, "UREDNIK3@BIBLIOTEKA.COM", "UREDNIK3", "AQAAAAIAAYagAAAAEL894LICQk6+LQGwiiVbELAskhk4fjWpAqzJ6rRF9lv8q74F6qMias6mM5H7xBFYqg==", null, false, "01fa1d41-5f06-4dd0-92c1-1ef9c43f0987", null, false, "urednik3" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "role-biblio-id", "user-biblio-1" },
                    { "role-urednik-id", "user-urednik-1" },
                    { "role-urednik-id", "user-urednik-2" },
                    { "role-urednik-id", "user-urednik-3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-biblio-id", "user-biblio-1" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-urednik-id", "user-urednik-1" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-urednik-id", "user-urednik-2" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-urednik-id", "user-urednik-3" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-biblio-id");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-urednik-id");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-biblio-1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-urednik-1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-urednik-2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-urednik-3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ae4637db-259c-4f3a-b59f-26fa28a362f3", null, "Bibliotekar", "BIBLIOTEKAR" },
                    { "b1ba13ab-e4df-4299-a734-77e183cf91c2", null, "Urednik", "UREDNIK" }
                });
        }
    }
}
