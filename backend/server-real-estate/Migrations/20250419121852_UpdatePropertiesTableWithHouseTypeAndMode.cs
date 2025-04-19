using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server_real_estate.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePropertiesTableWithHouseTypeAndMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HouseType",
                schema: "dbo",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mode",
                schema: "dbo",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HouseType",
                schema: "dbo",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Mode",
                schema: "dbo",
                table: "Properties");
        }
    }
}
