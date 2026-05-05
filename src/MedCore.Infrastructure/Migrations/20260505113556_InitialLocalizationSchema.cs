using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialLocalizationSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "localization");

            migrationBuilder.CreateTable(
                name: "translations",
                schema: "localization",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    culture = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_translations", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_translations_culture",
                schema: "localization",
                table: "translations",
                column: "culture");

            migrationBuilder.CreateIndex(
                name: "ix_translations_culture_key",
                schema: "localization",
                table: "translations",
                columns: new[] { "culture", "key" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "translations",
                schema: "localization");
        }
    }
}
