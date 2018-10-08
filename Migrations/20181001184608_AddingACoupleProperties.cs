using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nerdable.NotesApi.Migrations
{
    public partial class AddingACoupleProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tags",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NoteId",
                table: "Tags",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TagId1",
                table: "Tags",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Notes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Notes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Tags_NoteId",
                table: "Tags",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TagId1",
                table: "Tags",
                column: "TagId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Notes_NoteId",
                table: "Tags",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tags_TagId1",
                table: "Tags",
                column: "TagId1",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Notes_NoteId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tags_TagId1",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_NoteId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_TagId1",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "TagId1",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Notes");
        }
    }
}
