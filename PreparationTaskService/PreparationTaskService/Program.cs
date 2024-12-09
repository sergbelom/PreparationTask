using Microsoft.EntityFrameworkCore;
using PreparationTaskService.Common.Extensions;
using PreparationTaskService.DAL;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefultConnection");

builder.Services.AddPooledDbContextFactory<PrepTaskDatabaseContext>(options =>
{
    options.UseNpgsql(connectionString, x => x.UseNetTopologySuite());
});

builder.Services.AddControllers();
builder.Services.RegisterServices();

var host = builder.Build();


host.UseHttpsRedirection();
host.MapControllers();

host.Run();