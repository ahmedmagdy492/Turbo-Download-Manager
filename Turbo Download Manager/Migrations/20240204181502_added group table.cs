using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turbo_Download_Manager.Migrations
{
    /// <inheritdoc />
    public partial class addedgrouptable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadThreadEntries");

            migrationBuilder.CreateTable(
                name: "DownloadGroupsModels",
                columns: table => new
                {
                    GroupID = table.Column<string>(type: "TEXT", nullable: false),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    StartByte = table.Column<long>(type: "INTEGER", nullable: false),
                    CurrentByte = table.Column<long>(type: "INTEGER", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    FileId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadGroupsModels", x => x.GroupID);
                    table.ForeignKey(
                        name: "FK_DownloadGroupsModels_FileDownloadEntries_FileId",
                        column: x => x.FileId,
                        principalTable: "FileDownloadEntries",
                        principalColumn: "FileId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DownloadGroupsModels_FileId",
                table: "DownloadGroupsModels",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadGroupsModels");

            migrationBuilder.CreateTable(
                name: "DownloadThreadEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileDownloadEntryId = table.Column<string>(type: "TEXT", nullable: true),
                    AssignedDownloadLength = table.Column<long>(type: "INTEGER", nullable: false),
                    ByteToResumeFrom = table.Column<long>(type: "INTEGER", nullable: false)
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
    }
}
