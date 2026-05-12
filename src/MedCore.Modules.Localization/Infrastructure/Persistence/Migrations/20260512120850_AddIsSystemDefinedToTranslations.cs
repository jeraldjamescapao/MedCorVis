using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedCore.Modules.Localization.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsSystemDefinedToTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystemDefined",
                schema: "Localization",
                table: "Translations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystemDefined",
                schema: "Localization",
                table: "Translations");
        }
    }
}
