using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autodissmark.ApplicationDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtistEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeatEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    URI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BPM = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeatEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoiceEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtistEntityId = table.Column<int>(type: "int", nullable: false),
                    Speed = table.Column<double>(type: "float", nullable: false),
                    Pitch = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoiceEntity_ArtistEntity_ArtistEntityId",
                        column: x => x.ArtistEntityId,
                        principalTable: "ArtistEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorEntityId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextEntity_Authors_AuthorEntityId",
                        column: x => x.AuthorEntityId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DissEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeatEntityId = table.Column<int>(type: "int", nullable: false),
                    URI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DissEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DissEntity_BeatEntity_BeatEntityId",
                        column: x => x.BeatEntityId,
                        principalTable: "BeatEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcapellaEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextEntityId = table.Column<int>(type: "int", nullable: false),
                    VoiceEntityId = table.Column<int>(type: "int", nullable: false),
                    DurationMilliseconds = table.Column<int>(type: "int", nullable: false),
                    StartDelayMilliseconds = table.Column<int>(type: "int", nullable: false),
                    EndDelayMilliseconds = table.Column<int>(type: "int", nullable: false),
                    URI = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcapellaEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcapellaEntity_TextEntity_TextEntityId",
                        column: x => x.TextEntityId,
                        principalTable: "TextEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcapellaEntity_VoiceEntity_VoiceEntityId",
                        column: x => x.VoiceEntityId,
                        principalTable: "VoiceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DissAcapellaEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DissEntityId = table.Column<int>(type: "int", nullable: false),
                    AcapellaEntityId = table.Column<int>(type: "int", nullable: false),
                    StartPointMilliseconds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DissAcapellaEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DissAcapellaEntity_AcapellaEntity_AcapellaEntityId",
                        column: x => x.AcapellaEntityId,
                        principalTable: "AcapellaEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DissAcapellaEntity_DissEntity_DissEntityId",
                        column: x => x.DissEntityId,
                        principalTable: "DissEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcapellaEntity_TextEntityId",
                table: "AcapellaEntity",
                column: "TextEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AcapellaEntity_VoiceEntityId",
                table: "AcapellaEntity",
                column: "VoiceEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DissAcapellaEntity_AcapellaEntityId",
                table: "DissAcapellaEntity",
                column: "AcapellaEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DissAcapellaEntity_DissEntityId",
                table: "DissAcapellaEntity",
                column: "DissEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DissEntity_BeatEntityId",
                table: "DissEntity",
                column: "BeatEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TextEntity_AuthorEntityId",
                table: "TextEntity",
                column: "AuthorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_VoiceEntity_ArtistEntityId",
                table: "VoiceEntity",
                column: "ArtistEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DissAcapellaEntity");

            migrationBuilder.DropTable(
                name: "AcapellaEntity");

            migrationBuilder.DropTable(
                name: "DissEntity");

            migrationBuilder.DropTable(
                name: "TextEntity");

            migrationBuilder.DropTable(
                name: "VoiceEntity");

            migrationBuilder.DropTable(
                name: "BeatEntity");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "ArtistEntity");
        }
    }
}
