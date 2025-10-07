using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookstoreApplication.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDate",
                table: "Books",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Authors",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Biography", "Birthday", "FullName" },
                values: new object[,]
                {
                    { 1, "British writer and journalist.", new DateTime(1903, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "George Orwell" },
                    { 2, "English novelist known for romantic fiction.", new DateTime(1775, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jane Austen" },
                    { 3, "British author best known for Harry Potter.", new DateTime(1965, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "J.K. Rowling" },
                    { 4, "American writer and humorist.", new DateTime(1835, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mark Twain" },
                    { 5, "Russian novelist, known for War and Peace.", new DateTime(1828, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Leo Tolstoy" }
                });

            migrationBuilder.InsertData(
                table: "Awards",
                columns: new[] { "Id", "Description", "Name", "StartYear" },
                values: new object[,]
                {
                    { 1, "Global award for outstanding contributions to literature.", "Nobel Prize in Literature", 1901 },
                    { 2, "Distinguished fiction by an American author.", "Pulitzer Prize for Fiction", 1917 },
                    { 3, "Best original novel in English.", "Booker Prize", 1969 },
                    { 4, "Annual U.S. awards for literature.", "National Book Award", 1950 }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "Id", "Address", "Name", "Website" },
                values: new object[,]
                {
                    { 1, "80 Strand, London", "Penguin Books", "https://penguin.co.uk" },
                    { 2, "50 Bedford Square, London", "Bloomsbury", "https://www.bloomsbury.com" },
                    { 3, "New York, USA", "Vintage Books", "https://www.vintagebooks.com" }
                });

            migrationBuilder.InsertData(
                table: "AuthorAwardBridge",
                columns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                values: new object[,]
                {
                    { 1, 2, 1949 },
                    { 1, 3, 1970 },
                    { 1, 4, 1951 },
                    { 2, 1, 1905 },
                    { 2, 2, 1925 },
                    { 2, 3, 1971 },
                    { 3, 2, 2000 },
                    { 3, 3, 1999 },
                    { 3, 4, 2001 },
                    { 4, 1, 1907 },
                    { 4, 2, 1918 },
                    { 4, 4, 1952 },
                    { 5, 1, 1902 },
                    { 5, 2, 1920 },
                    { 5, 3, 1970 }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "ISBN", "PageCount", "PublishedDate", "PublisherId", "Title" },
                values: new object[,]
                {
                    { 1, 1, "9780451524935", 328, new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "1984" },
                    { 2, 1, "9780451526342", 112, new DateTime(1945, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Animal Farm" },
                    { 3, 2, "9780141439518", 432, new DateTime(1813, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Pride and Prejudice" },
                    { 4, 2, "9780141439587", 474, new DateTime(1815, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Emma" },
                    { 5, 3, "9780747532699", 223, new DateTime(1997, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Harry Potter and the Philosopher's Stone" },
                    { 6, 3, "9780747538493", 251, new DateTime(1998, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Harry Potter and the Chamber of Secrets" },
                    { 7, 3, "9780747542155", 317, new DateTime(1999, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Harry Potter and the Prisoner of Azkaban" },
                    { 8, 4, "9780142437179", 366, new DateTime(1884, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Adventures of Huckleberry Finn" },
                    { 9, 4, "9780143039563", 274, new DateTime(1876, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "The Adventures of Tom Sawyer" },
                    { 10, 5, "9780140447934", 1225, new DateTime(1869, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "War and Peace" },
                    { 11, 5, "9780143035008", 864, new DateTime(1877, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Anna Karenina" },
                    { 12, 5, "9780553210354", 86, new DateTime(1886, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "The Death of Ivan Ilyich" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 1, 2, 1949 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 1, 3, 1970 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 1, 4, 1951 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 2, 1, 1905 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 2, 2, 1925 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 2, 3, 1971 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 3, 2, 2000 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 3, 3, 1999 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 3, 4, 2001 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 4, 1, 1907 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 4, 2, 1918 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 4, 4, 1952 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 5, 1, 1902 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 5, 2, 1920 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId", "YearAwarded" },
                keyValues: new object[] { 5, 3, 1970 });

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Awards",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Awards",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Awards",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Awards",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDate",
                table: "Books",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Authors",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
