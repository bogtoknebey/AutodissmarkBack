using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autodissmark.ApplicationDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcapellaEntity_TextEntity_TextEntityId",
                table: "AcapellaEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AcapellaEntity_VoiceEntity_VoiceEntityId",
                table: "AcapellaEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_DissAcapellaEntity_AcapellaEntity_AcapellaEntityId",
                table: "DissAcapellaEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_DissAcapellaEntity_DissEntity_DissEntityId",
                table: "DissAcapellaEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_DissEntity_BeatEntity_BeatEntityId",
                table: "DissEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_TextEntity_Authors_AuthorEntityId",
                table: "TextEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_VoiceEntity_ArtistEntity_ArtistEntityId",
                table: "VoiceEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoiceEntity",
                table: "VoiceEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TextEntity",
                table: "TextEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DissEntity",
                table: "DissEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DissAcapellaEntity",
                table: "DissAcapellaEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BeatEntity",
                table: "BeatEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistEntity",
                table: "ArtistEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AcapellaEntity",
                table: "AcapellaEntity");

            migrationBuilder.RenameTable(
                name: "VoiceEntity",
                newName: "Voices");

            migrationBuilder.RenameTable(
                name: "TextEntity",
                newName: "Texts");

            migrationBuilder.RenameTable(
                name: "DissEntity",
                newName: "Disses");

            migrationBuilder.RenameTable(
                name: "DissAcapellaEntity",
                newName: "DissAcapellas");

            migrationBuilder.RenameTable(
                name: "BeatEntity",
                newName: "Beats");

            migrationBuilder.RenameTable(
                name: "ArtistEntity",
                newName: "Artists");

            migrationBuilder.RenameTable(
                name: "AcapellaEntity",
                newName: "Acapellas");

            migrationBuilder.RenameIndex(
                name: "IX_VoiceEntity_ArtistEntityId",
                table: "Voices",
                newName: "IX_Voices_ArtistEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_TextEntity_AuthorEntityId",
                table: "Texts",
                newName: "IX_Texts_AuthorEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_DissEntity_BeatEntityId",
                table: "Disses",
                newName: "IX_Disses_BeatEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_DissAcapellaEntity_DissEntityId",
                table: "DissAcapellas",
                newName: "IX_DissAcapellas_DissEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_DissAcapellaEntity_AcapellaEntityId",
                table: "DissAcapellas",
                newName: "IX_DissAcapellas_AcapellaEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_AcapellaEntity_VoiceEntityId",
                table: "Acapellas",
                newName: "IX_Acapellas_VoiceEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_AcapellaEntity_TextEntityId",
                table: "Acapellas",
                newName: "IX_Acapellas_TextEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Voices",
                table: "Voices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Texts",
                table: "Texts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disses",
                table: "Disses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DissAcapellas",
                table: "DissAcapellas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Beats",
                table: "Beats",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artists",
                table: "Artists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Acapellas",
                table: "Acapellas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Acapellas_Texts_TextEntityId",
                table: "Acapellas",
                column: "TextEntityId",
                principalTable: "Texts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Acapellas_Voices_VoiceEntityId",
                table: "Acapellas",
                column: "VoiceEntityId",
                principalTable: "Voices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DissAcapellas_Acapellas_AcapellaEntityId",
                table: "DissAcapellas",
                column: "AcapellaEntityId",
                principalTable: "Acapellas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DissAcapellas_Disses_DissEntityId",
                table: "DissAcapellas",
                column: "DissEntityId",
                principalTable: "Disses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Disses_Beats_BeatEntityId",
                table: "Disses",
                column: "BeatEntityId",
                principalTable: "Beats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Texts_Authors_AuthorEntityId",
                table: "Texts",
                column: "AuthorEntityId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Voices_Artists_ArtistEntityId",
                table: "Voices",
                column: "ArtistEntityId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acapellas_Texts_TextEntityId",
                table: "Acapellas");

            migrationBuilder.DropForeignKey(
                name: "FK_Acapellas_Voices_VoiceEntityId",
                table: "Acapellas");

            migrationBuilder.DropForeignKey(
                name: "FK_DissAcapellas_Acapellas_AcapellaEntityId",
                table: "DissAcapellas");

            migrationBuilder.DropForeignKey(
                name: "FK_DissAcapellas_Disses_DissEntityId",
                table: "DissAcapellas");

            migrationBuilder.DropForeignKey(
                name: "FK_Disses_Beats_BeatEntityId",
                table: "Disses");

            migrationBuilder.DropForeignKey(
                name: "FK_Texts_Authors_AuthorEntityId",
                table: "Texts");

            migrationBuilder.DropForeignKey(
                name: "FK_Voices_Artists_ArtistEntityId",
                table: "Voices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Voices",
                table: "Voices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Texts",
                table: "Texts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Disses",
                table: "Disses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DissAcapellas",
                table: "DissAcapellas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Beats",
                table: "Beats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artists",
                table: "Artists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Acapellas",
                table: "Acapellas");

            migrationBuilder.RenameTable(
                name: "Voices",
                newName: "VoiceEntity");

            migrationBuilder.RenameTable(
                name: "Texts",
                newName: "TextEntity");

            migrationBuilder.RenameTable(
                name: "Disses",
                newName: "DissEntity");

            migrationBuilder.RenameTable(
                name: "DissAcapellas",
                newName: "DissAcapellaEntity");

            migrationBuilder.RenameTable(
                name: "Beats",
                newName: "BeatEntity");

            migrationBuilder.RenameTable(
                name: "Artists",
                newName: "ArtistEntity");

            migrationBuilder.RenameTable(
                name: "Acapellas",
                newName: "AcapellaEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Voices_ArtistEntityId",
                table: "VoiceEntity",
                newName: "IX_VoiceEntity_ArtistEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Texts_AuthorEntityId",
                table: "TextEntity",
                newName: "IX_TextEntity_AuthorEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Disses_BeatEntityId",
                table: "DissEntity",
                newName: "IX_DissEntity_BeatEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_DissAcapellas_DissEntityId",
                table: "DissAcapellaEntity",
                newName: "IX_DissAcapellaEntity_DissEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_DissAcapellas_AcapellaEntityId",
                table: "DissAcapellaEntity",
                newName: "IX_DissAcapellaEntity_AcapellaEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Acapellas_VoiceEntityId",
                table: "AcapellaEntity",
                newName: "IX_AcapellaEntity_VoiceEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Acapellas_TextEntityId",
                table: "AcapellaEntity",
                newName: "IX_AcapellaEntity_TextEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoiceEntity",
                table: "VoiceEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextEntity",
                table: "TextEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DissEntity",
                table: "DissEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DissAcapellaEntity",
                table: "DissAcapellaEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BeatEntity",
                table: "BeatEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistEntity",
                table: "ArtistEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AcapellaEntity",
                table: "AcapellaEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AcapellaEntity_TextEntity_TextEntityId",
                table: "AcapellaEntity",
                column: "TextEntityId",
                principalTable: "TextEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AcapellaEntity_VoiceEntity_VoiceEntityId",
                table: "AcapellaEntity",
                column: "VoiceEntityId",
                principalTable: "VoiceEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DissAcapellaEntity_AcapellaEntity_AcapellaEntityId",
                table: "DissAcapellaEntity",
                column: "AcapellaEntityId",
                principalTable: "AcapellaEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DissAcapellaEntity_DissEntity_DissEntityId",
                table: "DissAcapellaEntity",
                column: "DissEntityId",
                principalTable: "DissEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DissEntity_BeatEntity_BeatEntityId",
                table: "DissEntity",
                column: "BeatEntityId",
                principalTable: "BeatEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TextEntity_Authors_AuthorEntityId",
                table: "TextEntity",
                column: "AuthorEntityId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceEntity_ArtistEntity_ArtistEntityId",
                table: "VoiceEntity",
                column: "ArtistEntityId",
                principalTable: "ArtistEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
