using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nerdable.NotesApi.Migrations
{
    public partial class CreatedcompositekeyforTagNoteRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagNoteRelationship_Notes_NoteId",
                table: "TagNoteRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_TagNoteRelationship_Tags_TagId",
                table: "TagNoteRelationship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagNoteRelationship",
                table: "TagNoteRelationship");

            migrationBuilder.DropIndex(
                name: "IX_TagNoteRelationship_TagId",
                table: "TagNoteRelationship");

            migrationBuilder.DropColumn(
                name: "TagNoteRelationshipId",
                table: "TagNoteRelationship");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "TagNoteRelationship",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NoteId",
                table: "TagNoteRelationship",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagNoteRelationship",
                table: "TagNoteRelationship",
                columns: new[] { "TagId", "NoteId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TagNoteRelationship_Notes_NoteId",
                table: "TagNoteRelationship",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagNoteRelationship_Tags_TagId",
                table: "TagNoteRelationship",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagNoteRelationship_Notes_NoteId",
                table: "TagNoteRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_TagNoteRelationship_Tags_TagId",
                table: "TagNoteRelationship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagNoteRelationship",
                table: "TagNoteRelationship");

            migrationBuilder.AlterColumn<int>(
                name: "NoteId",
                table: "TagNoteRelationship",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "TagNoteRelationship",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "TagNoteRelationshipId",
                table: "TagNoteRelationship",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagNoteRelationship",
                table: "TagNoteRelationship",
                column: "TagNoteRelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_TagNoteRelationship_TagId",
                table: "TagNoteRelationship",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagNoteRelationship_Notes_NoteId",
                table: "TagNoteRelationship",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagNoteRelationship_Tags_TagId",
                table: "TagNoteRelationship",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
