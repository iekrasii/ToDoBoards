using System;
using Microsoft.Extensions.Configuration;

namespace ToDoBoards.Common.Extensions;

public static class ConfigurationExtensions
{
    public static T BindSettings<T>(this IConfiguration configuration) where T : new()
    {
        var suffix = "Configuration";
        var configurationClassName = typeof(T).Name;
        
        if (!configurationClassName.EndsWith(suffix))
            throw new InvalidOperationException($"Classes to bind application settings to must end with word '{suffix}'");

        var configurationSectionName = configurationClassName.Substring(0, configurationClassName.Length - suffix.Length);
        
        var option = new T();
        configuration.GetSection(configurationSectionName).Bind(option);
        return option;
    }
}