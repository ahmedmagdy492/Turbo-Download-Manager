using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turbo_Download_Manager.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileDownloadEntries",
                columns: table => new
                {
                    FileId = table.Column<string>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: true),
                    DownloadUrl = table.Column<string>(type: "TEXT", nullable: true),
                    SavePath = table.Column<string>(type: "TEXT", nullable: true),
                    HasCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ProgressPercent = table.Column<string>(type: "TEXT", nullable: true),
                    StartDownloadDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDownloadEntries", x => x.FileId);
                });

            migrationBuilder.CreateTable(
                name: "DownloadThreadEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ByteToResumeFrom = table.Column<long>(type: "INTEGER", nullable: false),
                    AssignedDownloadLength = table.Column<long>(type: "INTEGER", nullable: false),
                    FileDownloadEntryId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadThreadEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DownloadThreadEntries_FileDownloadEntries_FileDownloadEntryId",
                        column: x => x.FileDownloadEntryId,
                        principalTable: "FileDownloadEntries",
                        principalColumn: "FileId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DownloadThreadEntries_FileDownloadEntryId",
                table: "DownloadThreadEntries",
                column: "FileDownloadEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadThreadEntries");

            migrationBuilder.DropTable(
                name: "FileDownloadEntries");
        }
    }
}
