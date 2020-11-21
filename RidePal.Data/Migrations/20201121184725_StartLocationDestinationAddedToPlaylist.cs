using Microsoft.EntityFrameworkCore.Migrations;

namespace RidePal.Data.Migrations
{
    public partial class StartLocationDestinationAddedToPlaylist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "Playlists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartLocation",
                table: "Playlists",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f72b7fc6-0b49-414a-b499-3e109c6c83b5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "64bde844-f7e0-4297-82b4-4effe0775f25");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b1d48f73-ce05-417f-bc91-04d75fb50822", "AQAAAAEAACcQAAAAEIwRX3HC9a4PGdnu0nr7n7hieKpFM614OK8B196J9jRHvjmGN/nzj++AEa7drTSiww==", "1baecd3d-2b59-4e9c-a7a3-46e6fa215503" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "452b59a2-74bc-46e1-a0f9-7aff726698b7", "AQAAAAEAACcQAAAAEEf3sSLn4FygScWfM4XBcrYR+mx8Gc/UhWVIQ6wunIZxZzRKYGkhwfouxso8WAJVyQ==", "d52243b5-8a44-44a9-8da0-042ef465a9d2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destination",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "StartLocation",
                table: "Playlists");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6c1ed329-1cba-44c7-9078-d99722a80086");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "929205af-4794-42a6-bc0d-7e29767e8ae4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0fc3ca71-7244-45a0-a14d-bf3d4e923474", "AQAAAAEAACcQAAAAENyawyV03pNj2ZeQODA2Gu+v5Y4v1bRpMTov5fOYfIqYQNjMystTHmNOnyrXnc4eHg==", "be1ddb93-53aa-441d-8305-386c56e6cc18" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "875c6a6b-4aaa-48d3-a791-72b413c7de6f", "AQAAAAEAACcQAAAAEJN4kE/Lbnp3VUF7khD4j8fib2mh2y1HSIgq7wf9VZioFeZkFuVliDSW9TYjiFQitg==", "300c769e-c58e-45d4-a425-ce7f795e35a7" });
        }
    }
}
