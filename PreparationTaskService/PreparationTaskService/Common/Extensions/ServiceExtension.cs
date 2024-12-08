﻿using PreparationTaskService.Services;

namespace PreparationTaskService.Common.Extensions
{
    public static class ServiceExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IStreetService, StreetService>();
            services.AddSingleton<IStreetServiceDb, StreetServiceDb>();

            //TODO: AutoMapper
        }
    }
}
