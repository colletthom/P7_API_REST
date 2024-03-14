using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P7CreateRestApi.Migrations
{
    public partial class correctionContextCurvePoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "CurvePoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurveId = table.Column<byte>(type: "tinyint", nullable: true),
                    AsOfDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Term = table.Column<double>(type: "float", nullable: true),
                    CurvePointValue = table.Column<double>(type: "float", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurvePoints", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurvePoints");

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
    }
}
