using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace PreparationTaskService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<MultiPoint>(
                name: "Geometry",
                schema: "preptask",
                table: "STREETS",
                type: "geometry",
                nullable: false,
                oldClrType: typeof(MultiPoint),
                oldType: "geography");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<MultiPoint>(
                name: "Geometry",
                schema: "preptask",
                table: "STREETS",
                type: "geography",
                nullable: false,
                oldClrType: typeof(MultiPoint),
                oldType: "geometry");
        }
    }
}
