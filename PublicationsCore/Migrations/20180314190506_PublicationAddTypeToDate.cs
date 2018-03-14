using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PublicationsCore.Migrations
{
    public partial class PublicationAddTypeToDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Publications",
                type: "datetime(3)",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Publications",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(3)");
        }
    }
}
