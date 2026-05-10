using Microsoft.EntityFrameworkCore.Migrations;

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
                name: "Localization");

            migrationBuilder.CreateTable(
                name: "Translations",
                schema: "Localization",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Culture = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Translations_Culture",
                schema: "Localization",
                table: "Translations",
                column: "Culture");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_Culture_Key",
                schema: "Localization",
                table: "Translations",
                columns: new[] { "Culture", "Key" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Translations",
                schema: "Localization");
        }
    }
}
