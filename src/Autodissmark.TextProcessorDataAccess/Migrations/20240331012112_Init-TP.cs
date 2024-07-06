using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autodissmark.TextProcessorDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dictionaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dictionaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TextBases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ONNXURI = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextBases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArtistTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtistEntityId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtistTexts_Artists_ArtistEntityId",
                        column: x => x.ArtistEntityId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DictionaryEntityId = table.Column<int>(type: "int", nullable: false),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictionaryWords_Dictionaries_DictionaryEntityId",
                        column: x => x.DictionaryEntityId,
                        principalTable: "Dictionaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextBaseArtists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextBaseEntityId = table.Column<int>(type: "int", nullable: false),
                    ArtistEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextBaseArtists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextBaseArtists_Artists_ArtistEntityId",
                        column: x => x.ArtistEntityId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextBaseArtists_TextBases_TextBaseEntityId",
                        column: x => x.TextBaseEntityId,
                        principalTable: "TextBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextBaseDicitonaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextBaseEntityId = table.Column<int>(type: "int", nullable: false),
                    DictionaryEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextBaseDicitonaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextBaseDicitonaries_Dictionaries_DictionaryEntityId",
                        column: x => x.DictionaryEntityId,
                        principalTable: "Dictionaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextBaseDicitonaries_TextBases_TextBaseEntityId",
                        column: x => x.TextBaseEntityId,
                        principalTable: "TextBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTexts_ArtistEntityId",
                table: "ArtistTexts",
                column: "ArtistEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryWords_DictionaryEntityId",
                table: "DictionaryWords",
                column: "DictionaryEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TextBaseArtists_ArtistEntityId",
                table: "TextBaseArtists",
                column: "ArtistEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TextBaseArtists_TextBaseEntityId",
                table: "TextBaseArtists",
                column: "TextBaseEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TextBaseDicitonaries_DictionaryEntityId",
                table: "TextBaseDicitonaries",
                column: "DictionaryEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TextBaseDicitonaries_TextBaseEntityId",
                table: "TextBaseDicitonaries",
                column: "TextBaseEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistTexts");

            migrationBuilder.DropTable(
                name: "DictionaryWords");

            migrationBuilder.DropTable(
                name: "TextBaseArtists");

            migrationBuilder.DropTable(
                name: "TextBaseDicitonaries");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Dictionaries");

            migrationBuilder.DropTable(
                name: "TextBases");
        }
    }
}
