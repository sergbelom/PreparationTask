using Microsoft.EntityFrameworkCore.Migrations;

namespace PreparationTaskService.DAL.MigrationSqlGenerator
{
    public static class MigrationBuilderExtensions
    {
        public static MigrationBuilder ExecuteSqlFile(
            this MigrationBuilder builder,
            string filePath)
        {
            builder.Operations.Add(new ExecuteSqlFileOperation
            {
                RelativeFilePath = filePath
            });
            return builder;
        }
    }
}
