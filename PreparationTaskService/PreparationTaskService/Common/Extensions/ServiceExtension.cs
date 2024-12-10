using PreparationTaskService.Mapper;
using PreparationTaskService.Services;
using PreparationTaskService.Services.Interfaces;

namespace PreparationTaskService.Common.Extensions
{
    public static class ServiceExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IStreetService, StreetService>();
            services.AddSingleton<IStreetServiceDb, StreetServiceDb>();
            services.AddAutoMapper(typeof(StreetProfile));
        }
    }
}
