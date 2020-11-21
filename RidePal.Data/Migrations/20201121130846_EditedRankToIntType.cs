using Microsoft.EntityFrameworkCore.Migrations;

namespace RidePal.Data.Migrations
{
    public partial class EditedRankToIntType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rank",
                table: "Playlists",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Rank",
                table: "Playlists",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d525fbf3-a20b-488d-88de-4968d9e866ac");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "155587b5-53a0-49cb-a34a-f66bae939b83");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "441cc3ec-dda2-4531-92b9-c38451ae022c", "AQAAAAEAACcQAAAAEPKGhntPJTBWwLxhrRtchDPzYRdIxdEaqDh4p/O5NCvblfyU6MOk0PW0R3T4z8LoJg==", "e5e1f970-eb4f-4f2d-95cc-54975bab55c2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d4c0bd67-eb72-454e-be77-dc1bf09bcfc2", "AQAAAAEAACcQAAAAECKgkTviweTbRtXAVoZKwY+efX1lEXSuuIwCVWCnDcMTMUKj3UezalvDGxc5qs4Cbw==", "3664d59c-a974-4cb3-b70d-58c34d4dd987" });
        }
    }
}
