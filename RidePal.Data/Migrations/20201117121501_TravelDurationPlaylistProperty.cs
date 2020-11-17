using Microsoft.EntityFrameworkCore.Migrations;

namespace RidePal.Data.Migrations
{
    public partial class TravelDurationPlaylistProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TravelDuration",
                table: "Playlists",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "cbe17af6-9313-47a7-b4cc-a88429b12932");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "6189f78b-44a8-4515-bd30-9e35901580fd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4c51bfac-9663-41c5-a5d7-669091fd01c6", "AQAAAAEAACcQAAAAEJrqEf+8e1ZYoR5YLgrztw9fjyuXp14UwXgQY2k91e0i1EwYwrLx5a4UGa139U82FA==", "a2589e2f-f53e-4c58-a059-9f2c460cc520" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7cc05572-f7c7-4fcc-994f-e8bb0174edb1", "AQAAAAEAACcQAAAAEF5SMrR9F/wsyVYUqXIq2RuJXDVu0pMgp11nJp6v2ZzBc+YMV9prT/NSvegQHvip4w==", "a9c5fe06-51d4-46e9-8923-6713bce97cf2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TravelDuration",
                table: "Playlists");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "cab460b9-2fd0-4914-be6c-d956ac8aa29a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "dcf7c33c-c937-4bf2-b1fa-215150af61ce");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "86f7c91c-5666-46a4-b16a-98cbc60000e4", "AQAAAAEAACcQAAAAELZvhe0T8tomUY+gxYqMX1vhIjxYPfcbXaQApJdWaA4tGoXAjSNhq78AFuh8AcuywQ==", "ad0c5393-ef84-4e22-8d1f-073e4681e605" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "98e0f532-b99d-40fd-ae4f-cfa6606d2c09", "AQAAAAEAACcQAAAAELpHoWtWePXC0C9999JV9V6jnmqpeK/Rll2IFEqd7JQtzznUHAHitGz9q/xqK9u8bw==", "820d406f-0ede-4408-baf1-e9f0634750a0" });
        }
    }
}
