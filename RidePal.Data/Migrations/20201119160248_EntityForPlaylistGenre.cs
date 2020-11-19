using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RidePal.Data.Migrations
{
    public partial class EntityForPlaylistGenre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PlaylistGenres",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "PlaylistGenres",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PlaylistGenres",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "PlaylistGenres",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PlaylistGenres");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "PlaylistGenres");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PlaylistGenres");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "PlaylistGenres");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "96e6c779-8b92-4394-a684-36907edcbddc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "9c392f15-e0e0-4339-a37a-9d170f83e9fc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "53b29fc4-ca54-4cf3-844c-7e538ea9f60f", "AQAAAAEAACcQAAAAEFWJ96gVoe1GM+IJ+GKDSev3oXiy1JDZqzJjuBXSsjVIKf2q1P+RG0kYc7NpsnXtyA==", "bc539ca0-8228-4668-aa96-306c00b8eb3a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b41dae0e-1d3f-4ab8-aa29-76b9fef2a9d8", "AQAAAAEAACcQAAAAECH8JDAmDnmU6kBwcVxGUkTRbYPWx5kAGy0tdgcoxHqSeYEgnhPdwUbCp8vaISQOWw==", "9472a4be-7c8a-4c8f-b926-cc9b823daf29" });
        }
    }
}
