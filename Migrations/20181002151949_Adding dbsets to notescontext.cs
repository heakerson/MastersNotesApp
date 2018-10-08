using Microsoft.EntityFrameworkCore.Migrations;

namespace Nerdable.NotesApi.Migrations
{
    public partial class Addingdbsetstonotescontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagAlwaysIncludeRelationship_Tags_AlwaysIncludeTagId",
                table: "TagAlwaysIncludeRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_TagAlwaysIncludeRelationship_Tags_ChildTagId",
                table: "TagAlwaysIncludeRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_TagNoteRelationship_Notes_NoteId",
                table: "TagNoteRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_TagNoteRelationship_Tags_TagId",
                table: "TagNoteRelationship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagNoteRelationship",
                table: "TagNoteRelationship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagAlwaysIncludeRelationship",
                table: "TagAlwaysIncludeRelationship");

            migrationBuilder.RenameTable(
                name: "TagNoteRelationship",
                newName: "TagNoteRelationships");

            migrationBuilder.RenameTable(
                name: "TagAlwaysIncludeRelationship",
                newName: "TagAlwaysIncludeRelationships");

            migrationBuilder.RenameIndex(
                name: "IX_TagNoteRelationship_NoteId",
                table: "TagNoteRelationships",
                newName: "IX_TagNoteRelationships_NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_TagAlwaysIncludeRelationship_ChildTagId",
                table: "TagAlwaysIncludeRelationships",
                newName: "IX_TagAlwaysIncludeRelationships_ChildTagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagNoteRelationships",
                table: "TagNoteRelationships",
                columns: new[] { "TagId", "NoteId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagAlwaysIncludeRelationships",
                table: "TagAlwaysIncludeRelationships",
                columns: new[] { "AlwaysIncludeTagId", "ChildTagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TagAlwaysIncludeRelationships_Tags_AlwaysIncludeTagId",
                table: "TagAlwaysIncludeRelationships",
                column: "AlwaysIncludeTagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagAlwaysIncludeRelationships_Tags_ChildTagId",
                table: "TagAlwaysIncludeRelationships",
                column: "ChildTagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagNoteRelationships_Notes_NoteId",
                table: "TagNoteRelationships",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagNoteRelationships_Tags_TagId",
                table: "TagNoteRelationships",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagAlwaysIncludeRelationships_Tags_AlwaysIncludeTagId",
                table: "TagAlwaysIncludeRelationships");

            migrationBuilder.DropForeignKey(
                name: "FK_TagAlwaysIncludeRelationships_Tags_ChildTagId",
                table: "TagAlwaysIncludeRelationships");

            migrationBuilder.DropForeignKey(
                name: "FK_TagNoteRelationships_Notes_NoteId",
                table: "TagNoteRelationships");

            migrationBuilder.DropForeignKey(
                name: "FK_TagNoteRelationships_Tags_TagId",
                table: "TagNoteRelationships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagNoteRelationships",
                table: "TagNoteRelationships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagAlwaysIncludeRelationships",
                table: "TagAlwaysIncludeRelationships");

            migrationBuilder.RenameTable(
                name: "TagNoteRelationships",
                newName: "TagNoteRelationship");

            migrationBuilder.RenameTable(
                name: "TagAlwaysIncludeRelationships",
                newName: "TagAlwaysIncludeRelationship");

            migrationBuilder.RenameIndex(
                name: "IX_TagNoteRelationships_NoteId",
                table: "TagNoteRelationship",
                newName: "IX_TagNoteRelationship_NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_TagAlwaysIncludeRelationships_ChildTagId",
                table: "TagAlwaysIncludeRelationship",
                newName: "IX_TagAlwaysIncludeRelationship_ChildTagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagNoteRelationship",
                table: "TagNoteRelationship",
                columns: new[] { "TagId", "NoteId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagAlwaysIncludeRelationship",
                table: "TagAlwaysIncludeRelationship",
                columns: new[] { "AlwaysIncludeTagId", "ChildTagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TagAlwaysIncludeRelationship_Tags_AlwaysIncludeTagId",
                table: "TagAlwaysIncludeRelationship",
                column: "AlwaysIncludeTagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagAlwaysIncludeRelationship_Tags_ChildTagId",
                table: "TagAlwaysIncludeRelationship",
                column: "ChildTagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

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
    }
}
