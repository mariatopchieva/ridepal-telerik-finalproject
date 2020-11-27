using Microsoft.EntityFrameworkCore.Migrations;

namespace RidePal.Data.Migrations
{
    public partial class RemovedNameFromStartLocationAndDestinationProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "83fa9cc4-fb57-4634-abb8-fdb59f39e90e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "0f23125a-3f5a-45ec-9150-2e729c3e1c2f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b5fb33f2-1610-4327-bea2-5b6e2d0e1ed5", "AQAAAAEAACcQAAAAEHc9l00KiIsP3TG2pdUVWfaKWyEq3oAvLG/UwiFOgHxKnxCVdDNuLwsNwjNV2PhAqg==", "df93ffa5-bb07-4eee-ac0e-7b31f960d7d6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bb27947e-7082-4fc8-ba2e-f68587d12169", "AQAAAAEAACcQAAAAELkBHXqdon6Z4APA3qc83poqMoSRuAGAxXr+OjUZzXumokQmep9N8AZOMHvLTml6vQ==", "93c02bbd-3e6d-4763-b061-ffeac7cac559" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
