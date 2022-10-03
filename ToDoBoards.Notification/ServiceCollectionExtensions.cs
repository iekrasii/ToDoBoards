using Microsoft.Extensions.DependencyInjection;
using ToDoBoards.Common.Interfaces;

namespace ToDoBoards.Notification;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleNotifier(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<INotifier, ConsoleNotifier>();
        return serviceCollection;
    }
}