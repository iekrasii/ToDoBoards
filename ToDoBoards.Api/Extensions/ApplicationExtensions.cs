using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace ToDoBoards.Api.Extensions;

public static class ApplicationExtensions
{
    public static void ApplySwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
    }
}