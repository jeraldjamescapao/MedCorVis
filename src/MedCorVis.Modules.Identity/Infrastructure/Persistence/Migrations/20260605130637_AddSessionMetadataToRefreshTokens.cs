using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedCorVis.Modules.Identity.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionMetadataToRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                schema: "Identity",
                table: "RefreshTokens",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RevokedAtUtc",
                schema: "Identity",
                table: "RefreshTokens",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                schema: "Identity",
                table: "RefreshTokens",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                schema: "Identity",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "RevokedAtUtc",
                schema: "Identity",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                schema: "Identity",
                table: "RefreshTokens");
        }
    }
}
