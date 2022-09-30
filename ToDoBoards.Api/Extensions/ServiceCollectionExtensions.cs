using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using ToDoBoards.Api.V1.Mappings;
using ToDoBoards.Api.V1.Services;

namespace ToDoBoards.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiVersions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddApiVersioning(
            options =>
            {
                // Will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
            });

        serviceCollection.AddVersionedApiExplorer(
            options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

        return serviceCollection;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        serviceCollection.AddSwaggerGen(
            options =>
            {
                // integrate xml comments
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        return serviceCollection;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(typeof(BoardMappingProfile));
        serviceCollection.AddScoped<BoardServiceV1>();
        serviceCollection.AddScoped<ToDoServiceV1>();

        return serviceCollection;
    }
}