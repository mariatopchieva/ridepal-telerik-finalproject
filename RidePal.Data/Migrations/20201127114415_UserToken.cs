using Microsoft.EntityFrameworkCore.Migrations;

namespace RidePal.Data.Migrations
{
    public partial class UserToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "26f56013-cd1f-40b6-ad63-af6cd601eb18");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2c5b3124-21e5-472e-a77a-28608be5c673");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0705ae6d-dab6-4c9b-9568-3b78eb8afda8", "AQAAAAEAACcQAAAAEGHjR+s4x63tBkENtwBlM9l85G3nnMS5aoudvqf23aRZ5O3s/U0Js9lhSrnx9RdQgw==", "feeab815-a2c2-42e1-909e-613de017952b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "622c87a5-64e3-4ef9-8da5-58e66bdcf724", "AQAAAAEAACcQAAAAEA1Xn5gH4ZCI9l9Z3dq9BmC/l2SqxTmLYYTu197giVkB6ZP8Mz3kpS+qLjYuaePj5A==", "58843524-eb40-44b9-9836-aa1a5eee3725" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "AspNetUsers");

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
    }
}
