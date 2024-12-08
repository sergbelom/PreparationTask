using Microsoft.EntityFrameworkCore;
using PreparationTaskService;
using PreparationTaskService.Common.Extensions;
using PreparationTaskService.DAL;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddPooledDbContextFactory<DatabaseContext>(options =>
{
    options.UseNpgsql("Server=localhost;Port=5432;Database=preparationtask;User Id=postgres;Password=postgres", x => x.UseNetTopologySuite());
});

builder.Services.AddControllers();
builder.Services.RegisterServices();
//builder.Services.AddHostedService<Worker>();

var host = builder.Build();


host.UseHttpsRedirection();
host.MapControllers();

host.Run();