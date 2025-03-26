using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleLeadsAPI.Migrations
{
    /// <inheritdoc />
    public partial class DateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Leads",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Leads");
        }
    }
}
