using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using PreparationTaskService.DAL.MigrationSqlGenerator;

namespace PreparationTaskService.DAL
{
    public class DesignTimeDatabaseContext : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseNpgsql("Server=localhost;Port=5432;Database=preparationtask;User Id=postgres;Password=postgres",          //Integrated Sequrity=True
                x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsHistoryTable("__EFMigrationsHistory", DALConstants.PREP_SCHEMA_NAME);
                })
                .ReplaceService<IMigrationsSqlGenerator, PrepMigrationSqlGenerator>();

            return new DatabaseContext(builder.Options);
        }
    }
}
