using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NzWalk.API.Migrations
{
    /// <inheritdoc />
    public partial class ModifiesProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Walks",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Walks",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Walks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Regions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "Regions",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Regions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Difficulties",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Difficulties",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Walks",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Walks",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Walks",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Regions",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Regions",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Regions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Difficulties",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Difficulties",
                newName: "id");
        }
    }
}
