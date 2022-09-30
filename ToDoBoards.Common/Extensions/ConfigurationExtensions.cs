using Microsoft.Extensions.Configuration;

namespace ToDoBoards.Common.Extensions;

public static class ConfigurationExtensions
{
    public static T BindSettings<T>(this IConfiguration configuration, string sectionName = null) where T : new()
    {
        var option = new T();
        configuration.GetSection(sectionName ?? typeof(T).Name).Bind(option);
        return option;
    }
}