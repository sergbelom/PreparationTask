using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PreparationTaskService.DAL.Migrations
{
    //public partial class Init : Migration
    //{
    //    protected override void Up(MigrationBuilder migrationBuilder)
    //    {
    //        migrationBuilder.EnsureSchema(name: "preptask");

    //        migrationBuilder.AlterDatabase().Annotation("Npgsql:PostgresExtension:postgis", ",,");

    //        migrationBuilder.CreateTable(
    //            name: "STREETS",
    //            schema: "preptask",
    //            columns: table => new
    //            {
    //                Id = table.Column<int>(type: "integer", nullable: false)
    //                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
    //                Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
    //                Capacity = table.Column<int>(type: "integer", nullable: false),
    //                Geometry = table.Column<LineString>(type: "geometry", nullable: false)
    //            },
    //            constraints: table =>
    //            {
    //                table.PrimaryKey("PK_STREETS", x => x.Id);
    //            });
    //    }

    //    /// <inheritdoc />
    //    protected override void Down(MigrationBuilder migrationBuilder)
    //    {
    //        migrationBuilder.DropTable(
    //            name: "STREETS",
    //            schema: "preptask");

    //    }
    //}
}
