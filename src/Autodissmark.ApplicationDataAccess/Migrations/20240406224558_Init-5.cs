using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autodissmark.ApplicationDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Texts");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDate",
                table: "Texts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "Texts");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Texts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
