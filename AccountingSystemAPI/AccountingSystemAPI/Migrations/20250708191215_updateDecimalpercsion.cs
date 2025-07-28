using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateDecimalpercsion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__JournalDe__Journ__38996AB5",
                table: "JournalDetails");

            migrationBuilder.AddForeignKey(
                name: "FK__JournalDe__Journ__38996AB5",
                table: "JournalDetails",
                column: "JournalEntryId",
                principalTable: "JournalEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__JournalDe__Journ__38996AB5",
                table: "JournalDetails");

            migrationBuilder.AddForeignKey(
                name: "FK__JournalDe__Journ__38996AB5",
                table: "JournalDetails",
                column: "JournalEntryId",
                principalTable: "JournalEntries",
                principalColumn: "Id");
        }
    }
}
