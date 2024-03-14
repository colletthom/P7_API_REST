using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P7CreateRestApi.Migrations
{
    public partial class CurvePointClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BidLists",
                table: "BidLists");

            migrationBuilder.RenameTable(
                name: "BidLists",
                newName: "BidList");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BidList",
                table: "BidList",
                column: "BidListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BidList",
                table: "BidList");

            migrationBuilder.RenameTable(
                name: "BidList",
                newName: "BidLists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BidLists",
                table: "BidLists",
                column: "BidListId");
        }
    }
}
