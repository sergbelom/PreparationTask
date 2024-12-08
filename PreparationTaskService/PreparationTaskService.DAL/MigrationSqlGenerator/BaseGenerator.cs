using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreparationTaskService.DAL.MigrationSqlGenerator
{
    public partial class PrepMigrationSqlGenerator : NpgsqlMigrationsSqlGenerator
    {
        public PrepMigrationSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, INpgsqlSingletonOptions npgsqlSingletonOptions) : base(dependencies, npgsqlSingletonOptions)
        {
        }
    }
}
