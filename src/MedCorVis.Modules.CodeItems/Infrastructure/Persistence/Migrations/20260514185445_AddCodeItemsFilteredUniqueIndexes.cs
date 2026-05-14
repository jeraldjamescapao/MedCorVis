using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedCorVis.Modules.CodeItems.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeItemsFilteredUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Translations_EntityType_EntityId_Culture",
                schema: "CodeItems",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryId_Code",
                schema: "CodeItems",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Code",
                schema: "CodeItems",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_EntityType_EntityId_Culture",
                schema: "CodeItems",
                table: "Translations",
                columns: new[] { "EntityType", "EntityId", "Culture" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId_Code",
                schema: "CodeItems",
                table: "Items",
                columns: new[] { "CategoryId", "Code" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Code",
                schema: "CodeItems",
                table: "Categories",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Translations_EntityType_EntityId_Culture",
                schema: "CodeItems",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryId_Code",
                schema: "CodeItems",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Code",
                schema: "CodeItems",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_EntityType_EntityId_Culture",
                schema: "CodeItems",
                table: "Translations",
                columns: new[] { "EntityType", "EntityId", "Culture" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId_Code",
                schema: "CodeItems",
                table: "Items",
                columns: new[] { "CategoryId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Code",
                schema: "CodeItems",
                table: "Categories",
                column: "Code",
                unique: true);
        }
    }
}
