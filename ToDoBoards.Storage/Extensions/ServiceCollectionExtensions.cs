using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoBoards.Common.Extensions;
using ToDoBoards.Common.Interfaces;
using ToDoBoards.Storage.Configuration;
using ToDoBoards.Storage.Storage;

namespace ToDoBoards.Storage.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Db storage
    /// </summary>
    /// <param name="serviceCollection">Service collection</param>
    /// <param name="configuration">Application configuration</param>
    /// <returns></returns>
    public static IServiceCollection AddDbStorage(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var settings = configuration.BindSettings<StorageConfiguration>();

        serviceCollection.AddSingleton(op =>
        {
            {
                var cb = new DbContextOptionsBuilder<StorageDbContext>();
                cb = settings.InMemory.Enabled
                    ? cb.UseInMemoryDatabase(settings.InMemory.Name)
                    : cb.UseSqlServer(settings.ConnectionString);

                return cb.Options;
            }
        });
        serviceCollection.AddDbContext<StorageDbContext>();
        serviceCollection.AddScoped<IBoardStorage, BoardStorage>();
        serviceCollection.AddScoped<IToDoStorage, ToDoStorage>();
        serviceCollection.AddScoped<IRateLimitStorage, RateLimitStorage>();
        
        return serviceCollection;
    }
}