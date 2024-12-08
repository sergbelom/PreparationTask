using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using PreparationTaskService.DAL.MigrationSqlGenerator;

namespace PreparationTaskService.DAL
{
    public class DesignTimePrepTaskDatabaseContext : IDesignTimeDbContextFactory<PrepTaskDatabaseContext>
    {
        public PrepTaskDatabaseContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PrepTaskDatabaseContext>();
            builder.UseNpgsql("Server=localhost;Port=5432;Database=preparationtask;User Id=postgres;Password=postgres",          //Integrated Sequrity=True
                x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsHistoryTable("__EFMigrationsHistory", DALConstants.PREP_SCHEMA_NAME);
                })
                .ReplaceService<IMigrationsSqlGenerator, PrepMigrationSqlGenerator>();

            return new PrepTaskDatabaseContext(builder.Options);
        }
    }
}
