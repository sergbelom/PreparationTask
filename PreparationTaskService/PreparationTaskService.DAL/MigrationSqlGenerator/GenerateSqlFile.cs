using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
 
namespace PreparationTaskService.DAL.MigrationSqlGenerator
{
    public partial class PrepMigrationSqlGenerator
    {
        protected override void Generate(
            MigrationOperation operation,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            if (operation is ExecuteSqlFileOperation sqlFileOperation)
            {
                Generate(sqlFileOperation, model, builder);
            }
            else
            {
                base.Generate(operation, model, builder);
            }
        }

        private void Generate(
            ExecuteSqlFileOperation operation,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            var sql = string.Empty;
            var filePath = operation.RelativeFilePath;
            var path = Path.Combine(AppContext.BaseDirectory, filePath);

            using (var streamReader = new StreamReader(path)) { 
                sql = streamReader.ReadToEnd();            
            }

            if (!string.IsNullOrWhiteSpace(sql)) {
                var sqlOperation = new SqlOperation()
                {
                    Sql = sql
                };
                base.Generate(operation, model, builder);
            }
        }
    }

    public class ExecuteSqlFileOperation : MigrationOperation
    {
        public string RelativeFilePath { get; set; }
    }
}
