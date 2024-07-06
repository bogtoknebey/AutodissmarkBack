using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autodissmark.ApplicationDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acapellas_Voices_VoiceEntityId",
                table: "Acapellas");

            migrationBuilder.AlterColumn<int>(
                name: "VoiceEntityId",
                table: "Acapellas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Acapellas_Voices_VoiceEntityId",
                table: "Acapellas",
                column: "VoiceEntityId",
                principalTable: "Voices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acapellas_Voices_VoiceEntityId",
                table: "Acapellas");

            migrationBuilder.AlterColumn<int>(
                name: "VoiceEntityId",
                table: "Acapellas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Acapellas_Voices_VoiceEntityId",
                table: "Acapellas",
                column: "VoiceEntityId",
                principalTable: "Voices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
