﻿using HiveFS.FruitData;
using HiveFS.FruitService;
using HiveFS.WeatherData;
using HiveFS.WeatherService;
using Microsoft.OpenApi.Models;

namespace HiveFS.TakeHomeAssessment
{
    internal static class FruitServiceDependencies
    {
        public static IServiceCollection AddWeatherDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IWeatherApi, WeatherApi>();
            services.AddScoped<IWeatherRepository, WeatherRepository>();
            return services;
        }

        public static IServiceCollection AddFruitDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFruitApi, FruitApi>();
            services.AddScoped<IFruitRepository, FruitRepository>();
            return services;
        }

        public static IServiceCollection AddSwaggerGenAndSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "JWT Token Authentication API",
                    Description = "ASP.NET Core 3.1 Web API"
                });
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}
